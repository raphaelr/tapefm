app.registerView({
    name: "controls",
    permanent: true,
    constructor: function () {
        var self = {};

        var player = new AudioPlayer({
            url: "/listen/stream"
        });

        function init() {
            Trackservice.start({
                trackChanged: self.currentSong
            });
        }

        self.currentSong = ko.observable();
        self.isPaused = ko.observable(false);
        self.isListening = ko.observable(false);

        self.togglePause = function() {
            self.isPaused(!self.isPaused());
            $.post("/api/control/pause?paused=" + self.isPaused());
        };

        self.skip = function() {
            $.post("/api/control/skip");
        };

        self.toggleListen = function() {
            self.isListening(!self.isListening());
            player.setActive(self.isListening());
        };

        init();
        return self;
    }
});
