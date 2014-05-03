(function() {
    "use strict";

    function Player(view) {
        this.view = view;
        this.current = null;
        this.next = null;
    }
    Player.prototype = {
        play: function(path) {
            if(this.next) {
                this.unload(this.next);
            }
            this.next = this.createAudio(path);
            this.playNext();
        },
        
        createAudio: function(path) {
            var audio = new Audio();
            audio.preload = "none";
            audio.src = "/music" + path;
            audio.path = path;
            $(audio).one("suspend", this.audioLoaded.bind(this, audio));
            return audio;
        },
        
        makeCurrent: function(audio) {
            this.removeCurrent();
            $(audio).on("ended", this.playNext.bind(this));
            if(audio.currentSrc === "") {
                console.log("player: Loading current", audio.src);
                audio.load();
            }

            audio.play();
            this.current = audio;
            this.view.trigger("tapefm:songchange", audio.path);
            this.view.trigger("tapefm:unpause");
            console.log("player: Now playing", audio.src);
        },

        removeCurrent: function() {
            if(this.current) {
                this.current.pause();
                this.unload(this.current);
                this.current = null;
            }
        },

        playNext: function() {
            if(!this.next) {
                this.setNewNext();
            }
            this.makeCurrent(this.next);
            this.setNewNext();
            this.loadNextIfCurrentLoaded();
        },

        setNewNext: function() {
            this.next = this.createAudio(this.getRandomFile());
        },

        audioLoaded: function(audio) {
            audio.loaded = true;
            console.log("player: Loaded " +
                    (this.current === audio ? "current" :
                     this.next === audio ? "next" :
                     "unknown") , audio.src);

            this.loadNextIfCurrentLoaded();
        },

        loadNextIfCurrentLoaded: function() {
            if(this.current && this.current.loaded && !this.next.loaded) {
                console.log("player: Loading next", this.next.src);
                this.next.load();
            }
        },

        unload: function(audio) {
            audio.src = "";
            audio.load();
        },

        getRandomFile: function() {
            throw new Error("getRandomFile must be set by caller");
        },

        isPaused: function() {
            return !this.current || this.current.paused;
        },

        pause: function() {
            if(this.current) {
                this.current.pause();
                this.view.trigger("tapefm:pause");
            }
        },

        unpause: function() {
            if(this.current) {
                this.current.play();
                this.view.trigger("tapefm:unpause");
            }
        }
    };

    tapefm.Player = Player;
}());
