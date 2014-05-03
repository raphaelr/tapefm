(function() {
    "use strict";

    function Controller(view) {
        this.view = view;
        this.library = new tapefm.Library();
        this.player = new tapefm.Player();
        this.player.getRandomFile = function() {
            return this.library.getRandomFile().getFullPath();
        }.bind(this);

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

        play: function(file) {
            this.player.play(file.getFullPath());
            this.trigger("tapefm:songchange", file);
            this.trigger("tapefm:unpause");
        },

        togglePause: function() {
            if(this.player.isPaused()) {
                this.player.unpause();
                this.trigger("tapefm:unpause");
            } else {
                this.player.pause();
                this.trigger("tapefm:pause");
            }
        },

        skip: function() {
            this.player.playNext();
        },

        getView: function() {
            return this.view;
        },

        getLibrary: function() {
            return this.library;
        },

        getPlayer: function() {
            return this.player;
        }
    };

    function delegate(target, source, name) {
        target[name] = source[name].bind(source);
    }

    $.extend(tapefm, {
        Controller: Controller
    });
}());
