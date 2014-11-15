app.registerView({
    name: "library",
    constructor: function (directory) {
        var self = {};

        var breadcrumbs = [];
        var head = directory;
        while (head) {
            var breadcrumb = {
                name: head.parent ? head.name : " ",
                isRoot: !head.parent,
                open: (function(head) {
                    return function() {
                        app.navigateTo.library(head);
                    };
                }(head))
            };

            breadcrumbs.unshift(breadcrumb);
            head = head.parent;
        }

        self.breadcrumbs = breadcrumbs;

        self.entries = directory.getEntriesArray().map(function(data) {
            return {
                name: data.name,
                open: function () {
                    if (data.type === "directory") {
                        app.navigateTo.library(data);
                    } else if (data.type === "song") {
                        $.post("/api/control/current_track", {
                            path: data.song.path
                        });
                    }
                }
            };
        });

        self.openSettings = function() {
            app.navigateTo.settings({
                back: function() {
                    app.navigateTo.library(directory);
                }
            });
        };

        return self;
    }
});
