(function() {
    "use strict";

    function PlayerUI(container, controller) {
        this.container = container;
        this.controller = controller;
        this.attach();
    }
    PlayerUI.prototype = {
        attach: function() {
            this.controller.on("tapefm:songchange", function(ev, song) {
                this.setPath(song);
            }.bind(this));

            this.controller.on("tapefm:pause", function(ev, song) {
                this.pause();
            }.bind(this));

            this.controller.on("tapefm:unpause", function(ev, song) {
                this.unpause();
            }.bind(this));

            $(".state", this.container).click(function() {
                this.controller.togglePause();
            }.bind(this));

            $(".skip", this.container).click(function() {
                this.controller.skip();
            }.bind(this));
        },
        
        setPath: function(path) {
            $(".song", this.container).text(path);
        },

        pause: function() {
            this.getStateElement().removeClass("glyphicon-pause").addClass("glyphicon-play");
        },

        unpause: function() {
            this.getStateElement().removeClass("glyphicon-play").addClass("glyphicon-pause");
        },

        getStateElement: function() {
            return $(".state", this.container);
        }
    };

    tapefm.PlayerUI = PlayerUI;
}());
