require "bundler/setup"
require "sinatra"
require "yaml"
require "json"

Config = YAML.load_file("config.yml")

# Transcoder {{{
class Transcoder
    BLOCKSIZE = 1024*50

    def initialize(fn)
        decoder = Config[:decoder][File.extname(fn).slice(1..-1)].gsub("$fn", fn)
        @dec = IO.popen(decoder, "rb")
        @enc = IO.popen(Config[:encoder][:command], "w+b")
    end

    def each
        decoding = true
        while true
            if decoding
                begin
                    decoded = @dec.readpartial(BLOCKSIZE)
                    @enc.write(decoded)
                rescue EOFError
                    @enc.close_write
                    decoding = false
                end
            end

            next unless IO.select([@enc], [], [], 0)
            begin
                encoded = @enc.readpartial(BLOCKSIZE)
                yield encoded
            rescue EOFError
                @enc.close_read
                break
            end
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
    [200, { "Content-Type" => Config[:encoder][:mime] }, Transcoder.new(realname)]
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

post "/library/refresh" do
    refresh_library
    dump_library
end
# }}}

#  vim: set et tw=100 ts=4 sw=4 fdm=marker:
