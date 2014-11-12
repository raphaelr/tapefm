app.registerView({
    name: "settings",
    constructor: function(config) {
        var self = {};
        var needsReload = false;

        function init() {
            $.get("/api/status", function(status) {
                self.bitrate(status.bitrateKbps);
            });

            var updateBitrateTimer = new CodeTimer(function() {
                var bitrate = self.bitrate();
                if(bitrate > 10 && bitrate < 1000) {
                    $.post("/api/control/bitrate?kbps=" + bitrate);
                }
            });

            self.bitrate.subscribe(updateBitrateTimer.plan);
        }

        self.bitrate = ko.observable(300);

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
