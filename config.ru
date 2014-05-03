require "bundler/setup"
require "sprockets"
require "./lib/tapefm"

map "/assets" do
    env = Sprockets::Environment.new
    env.append_path "assets/js"
    env.append_path "assets/css"
    run env
end

map "/" do
    run Sinatra::Application
end
