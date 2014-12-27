Ext.define("TapeFM.controller.Player", {
    extend: "Ext.app.ViewController",
    alias: "controller.player",

    init: function() {
        var self = this;
        this.control({
            "button": {
                click: function() {
                    self.setPlaying(!self.isPlaying);
                    self.getView().viewModel.setData({ isPlaying: self.isPlaying });
                }
            }
        });
    },

    setPlaying: function(isPlaying) {
        if(!this.audio) {
            this.audio = document.createElement("audio");
        }

        this.isPlaying = isPlaying;
        if(isPlaying) {
            this.audio.src = "/listen/stream";
            this.audio.load();
            this.audio.play();
        } else {
            this.audio.src = "";
            this.audio.load();
            this.audio.pause();
        }
    }
});
