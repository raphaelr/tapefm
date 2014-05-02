//= require header
//= require_tree .

//$(function() {
//    "use strict";
//
//    var player = {
//        current: null,
//        queue: [],
//    };
//    window.player = player;
//
//    player.play = function(filename) {
//        player.queue = [filename];
//        player.next();
//    };
//
//    player.loadNext = function() {
//        if(!player.queue.length) {
//            player.queue.push(library.fsFlat[Math.floor(library.fsFlat.length * Math.random())]);
//        }
//        player.queue[0] = player.preload(player.queue[0]);
//    };
//
//    player.next = function() {
//        if(player.current) {
//            player.current.pause();
//            player.current.src = "";
//            player.current.load();
//            player.current = null;
//        }
//        player.loadNext();
//        player.current = player.queue.shift();
//        player.current.play();
//        $(".song").text(player.current.filename);
//        $(".state").removeClass("glyphicon-play");
//        $(".state").addClass("glyphicon-pause");
//    };
//
//    player.preload = function(filename) {
//        if(typeof filename !== "string") return filename;
//        var audio = new Audio("/music/" + filename);
//        $(audio).one("ended", function() {
//            player.next();
//        });
//        $(audio).one("suspend", function() {
//            player.loadNext();
//        });
//        audio.filename = filename;
//        audio.load();
//        return audio;
//    };
//    
//    player.togglePause = function() {
//        if(!player.current) return;
//        if(player.current.paused) {
//            player.current.play();
//            $(".state").removeClass("glyphicon-play");
//            $(".state").addClass("glyphicon-pause");
//        } else {
//            player.current.pause();
//            $(".state").removeClass("glyphicon-pause");
//            $(".state").addClass("glyphicon-play");
//        }
//    };
//
//
//    library.loadLibrary().then(function() {
//        library.chdir(library.fs);
//    });
//
//    $(".skip").click(function() {
//        player.next();
//    });
//
//    $(".state").click(function() {
//        player.togglePause();
//    });
//});
