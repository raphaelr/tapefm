(function() {
    "use strict";

    function Controller(view) {
        this.view = view;
        delegate(this, view, "on");
        delegate(this, view, "one");
        delegate(this, view, "trigger");

        this.on("tapefm:ready", this.reset.bind(this));
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
            this.trigger("tapefm:chdir", directory);
        },

        getView: function() {
            return this.view;
        },

        getLibrary: function() {
            return this.library;
        },
    };

    function delegate(target, source, name) {
        target[name] = source[name].bind(source);
    }

    $.extend(tapefm, {
        Controller: Controller
    });
}());
