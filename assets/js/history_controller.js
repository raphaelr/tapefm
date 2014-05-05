(function() {
    "use strict";

    function HistoryController(controller) {
        this.controller = controller;
        this.attach();
    }
    HistoryController.prototype = {
        attach: function() {
            this.controller.on("tapefm:chdir", function(ev, directory) {
                this.recordChdir(directory);
            }.bind(this));

            $(window).on("popstate", function() {
                this.popState();
            }.bind(this));
        },

        recordChdir: function(directory) {
            this.pushState("library",  directory.getFullPath());
        },

        popState: function() {
            this.inPopState = true;
            try {
                this.popStateReal();
            } finally {
                this.inPopState = false;
            }
        },

        popStateReal: function() {
            var hash = location.hash.slice(1);
            this.tryMatch(hash, "library", function(path) {
                var directory = this.controller.getLibrary().resolve(path);
                this.controller.chdir(directory || this.controller.getLibrary().getRoot());
            }.bind(this));
        },

        tryMatch: function(hash, prefix, fn) {
            if(hash.indexOf(prefix) === 0) {
                fn(hash.slice(prefix.length));
            }
        },

        pushState: function(prefix, args) {
            if(!this.inPopState) {
                history.pushState(null, "", "#" + prefix + args);
            }
        }
    };

    tapefm.HistoryController = HistoryController;
}());
