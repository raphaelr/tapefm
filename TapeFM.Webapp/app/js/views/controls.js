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

        var isPaused = ko.observable(false);
        var isListening = ko.observable(false);

        self.currentSong = ko.observable();

        self.pauseStatus = ko.computed(function() {
            return isPaused() ? "❚❚" : "▶";
        });

        self.listenStatus = ko.computed(function () {
            return isListening() ? "🔊" : "🔇";
        });

        self.togglePause = function() {
            isPaused(!isPaused());
        };

        self.skip = function() {
            $.post("/api/control/skip");
        };

        self.toggleListen = function() {
            isListening(!isListening());
            player.setActive(isListening());
        };

        init();
        return self;
    }
});
