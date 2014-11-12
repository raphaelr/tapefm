app.registerView({
    name: "settings",
    constructor: function(config) {
        var self = {};
        var needsReload = false;

        function init() {
            var updateBitrateTimer = new CodeTimer(function() {
                var bitrate = self.bitrate();
                if(bitrate > 10 && bitrate < 1000) {
                    $.post("/api/control/bitrate?kbps=" + bitrate);
                }
            });

            $.get("/api/status", function(status) {
                self.bitrate(status.bitrateKbps);
                self.emptyPlaylistMode(status.emptyPlaylistMode);
                
                self.bitrate.subscribe(updateBitrateTimer.plan);
                self.emptyPlaylistMode.subscribe(function(mode) {
                    $.post("/api/control/empty_playlist_mode?mode=" + mode);
                });
            });
        }

        self.bitrate = ko.observable(300);
        self.emptyPlaylistMode = ko.observable();

        self.back = function() {
            if(needsReload) {
                app.navigateTo.loading();
            } else {
                config.back();
            }
        };

        self.rescanLibrary = function() {
            $.ajax({
                type: "DELETE",
                url: "/api/cache"
            });
            needsReload = true;
        };

        init();
        return self;
    }
});
