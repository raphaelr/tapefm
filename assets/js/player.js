(function() {
    "use strict";

    function Player() {
        this.current = null;
        this.next = null;
    }
    Player.prototype = {
        play: function(path) {
            var audio = this.createAudio(path);
            this.makeCurrent(audio);
        },
        
        createAudio: function(path) {
            var audio = new Audio("/music" + path);
            return audio;
        },
        
        makeCurrent: function(audio) {
            audio.play();
            this.current = audio;
        },
    };

    tapefm.Player = Player;
}());
