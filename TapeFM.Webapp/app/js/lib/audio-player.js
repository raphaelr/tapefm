function AudioPlayer(spec) {
    var self = {};

    var player = document.createElement("audio");

    self.setActive = function(active) {
        if (active) {
            player.src = spec.url;
            player.load();
            player.play();
        } else if (player) {
            player.src = "";
            player.load();
        }
    }

    return self;
}