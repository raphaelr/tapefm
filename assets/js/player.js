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
            this.removeCurrent();
            audio.play();
            this.current = audio;
        },

        removeCurrent: function() {
            if(this.current) {
                this.current.pause();
                this.current.src = "";
                this.current.load();
                this.current = null;
            }
        },

        isPaused: function() {
            return !this.current || this.current.paused;
        },

        pause: function() {
            if(this.current) {
                this.current.pause();
            }
        },

        unpause: function() {
            if(this.current) {
                this.current.play();
            }
        }
    };

    tapefm.Player = Player;
}());
