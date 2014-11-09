function Trackservice(config) {
    var self = {};

    function init() {
        var trackservice = $.connection.trackservice;
        trackservice.client.setCurrentTrack = publishTrack;

        $.get("/api/status", function(status) {
            publishTrack(status.currentTrack || "");
        });
    }

    function publishTrack(track) {
        config.trackChanged(track);
    }

    init();
    return self;
}

Trackservice.start = Trackservice;