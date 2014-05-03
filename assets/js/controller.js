(function() {
    "use strict";

    function Controller(view) {
        this.view = view;
        view.on("tapefm:ready", this.reset.bind(this));
    }
    Controller.prototype = {
        reset: function() {
            this.reloadLibrary();
        },
        
        reloadLibrary: function() {
            $.get("/library", function(files) {
                this.library = tapefm.Library.fromFileArray(files);
                this.chdir(this.library.getRoot());
            }.bind(this));
        },

        chdir: function(directory) {
            this.view.trigger("tapefm:chdir", directory);
        },

        getView: function() {
            return this.view;
        },

        getLibrary: function() {
            return this.library;
        },
    };

    $.extend(tapefm, {
        Controller: Controller
    });
}());
