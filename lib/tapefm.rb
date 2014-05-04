require "sinatra"
require "yaml"
require "json"

Config = YAML.load_file("config.yml")

set :public_folder, File.join(File.dirname(__FILE__), "..", "public")

# Transcoder {{{
class Transcoder
    BLOCKSIZE = 1024*50

    attr_reader :current_block

    def initialize(fn)
        @pipe = IO.popen(get_transcode_command(fn), "rb")
    end

    def fetch_block
        begin
            @current_block = @pipe.readpartial(BLOCKSIZE)
            return true
        rescue EOFError
            @pipe.close
            return false
        end
    end

    private
    def get_transcode_command(fn)
        extension = File.extname(fn).slice(1..-1)
        decoder_cmd = Config[:decoders][extension.to_sym].gsub("$fn", fn)
        encoder_cmd = Config[:encoder][:command]
        "#{decoder_cmd} | #{encoder_cmd}"
    end
end
# }}}
# Main routes {{{
get "/" do
    call env.merge("PATH_INFO" => "/index.html")
end

get "/music/*" do
    base = File.expand_path(Config[:library][:path])
    realname = File.expand_path(File.join(base, params[:splat].join("/")))
    return 404 unless realname.start_with? base
    return 404 unless File.exist? realname

    status 200
    headers "Content-Type" => Config[:encoder][:mime]

    t = Transcoder.new(realname)
    stream { |out| out << t.current_block while t.fetch_block }
end
# }}}
# Library {{{
$library = nil

def dump_library
    [200, { "Content-Type" => "application/json" }, JSON.dump($library)]
end

def refresh_library
    $library = Dir.glob(Config[:library][:path] + Config[:library][:glob]).map { |p| p.slice(Config[:library][:path].length..-1) }
end

get "/library" do
    refresh_library unless $library
    dump_library
end

get "/library/refresh" do
    refresh_library
    dump_library
end
# }}}

#  vim: set et tw=100 ts=4 sw=4 fdm=marker:
