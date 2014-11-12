app.registerView({
    name: "settings",
    constructor: function(config) {
        var self = {};
        var needsReload = false;

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

        return self;
    }
});
