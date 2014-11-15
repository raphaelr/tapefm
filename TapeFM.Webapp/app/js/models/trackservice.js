function Trackservice(config) {
    var self = {};

    function init() {
        $.connection.trackservice.client.setCurrentTrack = publishTrack;
        $.connection.hub.disconnected(function() {
            setTimeout(function() {
                reconnect();
            }, 5000);
        });
        reconnect();
    }

    function reconnect() {
        $.connection.hub.start();
    }

    function publishTrack(track) {
        config.trackChanged(track || "Dead Air");
    }

    init();
    return self;
}

Trackservice.start = Trackservice;
