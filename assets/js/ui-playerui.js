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
                this.setSong(song);
            }.bind(this));
        },
        
        setSong: function(song) {
            $(".song", this.container).text(song.getFullPath());
        }
    };

    tapefm.PlayerUI = PlayerUI;
}());
