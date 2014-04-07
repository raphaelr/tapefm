$(function() {
    "use strict";

    var player = {
        current: null,
        queue: [],
    };
    window.player = player;

    player.play = function(filename) {
        player.queue = [filename];
        player.next();
    };

    player.loadNext = function() {
        if(!player.queue.length) {
            player.queue.push(library.fsFlat[Math.floor(library.fsFlat.length * Math.random())]);
        }
        player.queue[0] = player.preload(player.queue[0]);
    };

    player.next = function() {
        if(player.current) {
            player.current.pause();
            player.current.src = "";
            player.current.load();
            player.current = null;
        }
        player.current = player.preload(player.queue.shift());
        player.current.play();
        player.loadNext();
        $(".song").text(player.current.filename.split("/").pop());
    };

    player.preload = function(filename) {
        if(typeof filename !== "string") return filename;
        var audio = new Audio("/music/" + filename);
        $(audio).on("ended", function() {
            player.next();
        });
        audio.filename = filename;
        audio.load();
        return audio;
    };

    var library = {
        fs: null,
        fsFlat: null,
        cwd: null,
    };
    window.library = library;

    library.loadLibrary = function() {
        return $.get("/library").then(function(files) {
            library.fsFlat = files;
            library.fs = { ".fullpath": "" };
            files.forEach(function(f) {
                var head = library.fs;
                var fullpath = "";
                f.split("/").forEach(function(part, i, split) {
                    fullpath += part + "/";
                    if(i === split.length - 1) {
                        head[part] = true;
                    } else if(!(part in head)) {
                        var newdir = {
                            ".fullpath": fullpath
                        };
                        head = head[part] = newdir;
                    } else {
                        head = head[part];
                    }
                });
            });
        });
    };

    library.launch = function(dir, filename) {
        if(dir[filename] === true) {
            player.play(dir[".fullpath"] + filename);
        } else if(filename in dir) {
            library.chdir(dir[filename]);
        }
    };

    library.chdir = function(dir) {
        library.cwd = dir;
        // Path
        var path = $(".path").empty();
        path.append($("<div class='dir glyphicon glyphicon-music'></div>").click(function() {
            library.chdir(library.fs);
        }));
        var head = library.fs;
        dir[".fullpath"].split("/").forEach(function(x) {
            if(x === "") { return; }
            head = head[x] || head;
            var localHead = head;
            var tag = $("<div class='dir'></div>").text(x).click(function() {
                library.chdir(localHead);
            });
            path.append(tag);
        });

        // List
        var list = $("article ul").empty();
        var keys = Object.keys(dir).filter(function(x) { return x[0] !== "."; }).sort();
        keys.forEach(function(key) {
            var li = $("<li></li>");
            if(dir[key] !== true) {
                li.addClass("dir");
            }
            li.text(key);
            list.append(li);
            li.click(function() {
                library.launch(dir, key);
            });
        });
    };

    library.loadLibrary().then(function() {
        library.chdir(library.fs);
    });
});
