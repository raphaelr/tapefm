require "bundler/setup"
require "sinatra"
require "yaml"
require "json"

Config = YAML.load_file("config.yml")

# Transcoder {{{
class Transcoder
    BLOCKSIZE = 1024*50

    def initialize(fn)
        decoder = Config[:decoders][File.extname(fn).slice(1..-1)].gsub("$fn", fn)
        @pipe = IO.popen("#{decoder} | #{Config[:encoder][:command]}", "rb")
    end

    def next_block
        begin
            buffer = @pipe.readpartial(BLOCKSIZE)
            return buffer
        rescue EOFError
            @pipe.close
            return nil
        end
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
    stream do |out|
        while true
            b = t.next_block
            break unless b
            out << b
        end
    end
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
