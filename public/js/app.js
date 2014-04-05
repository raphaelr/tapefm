$(function() {
    "use strict";

    var app = {
        fs: null,
        fsFlat: null,
        cwd: null,
    };
    window.app = app;

    app.loadLibrary = function() {
        return $.get("/library").then(function(files) {
            app.fsFlat = files;
            app.fs = { ".fullpath": "" };
            files.forEach(function(f) {
                var head = app.fs;
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

    app.launch = function(dir, filename) {
        if(dir[filename] === true) {
            app.play(dir[".fullpath"] + filename);
        } else if(filename in dir) {
            app.chdir(dir[filename]);
        }
    };

    app.chdir = function(dir) {
        app.cwd = dir;
        // Path
        var path = $(".path").empty();
        path.append($("<div class='dir glyphicon glyphicon-music'></div>").click(function() {
            app.chdir(app.fs);
        }));
        var head = app.fs;
        dir[".fullpath"].split("/").forEach(function(x) {
            if(x === "") { return; }
            head = head[x] || head;
            var localHead = head;
            var tag = $("<div class='dir'></div>").text(x).click(function() {
                app.chdir(localHead);
            });
            path.append(tag);
        });

        // List
        var table = $("article table").empty();
        var keys = Object.keys(dir).filter(function(x) { return x[0] !== "."; }).sort();
        for(var i=0; i<keys.length; i+=2) {
            var a = keys[i], b = keys[i+1];
            var r1 = $("<tr></tr>"), r2 = $("<tr></tr>");
            var t1 = $("<td class='a box' rowspan='2'></td>");
            var t2 = $("<td class='a text'></td>");
            var t3 = $(b ? "<td rowspan='2' class='b box'></td>" : "<td rowspan='2' class='box'></td>");
            var t4 = $(b ? "<td class='b text'></td>" : "<td class='text'></td>");
            t2.text(a);
            if(b) t4.text(b);
            r1.append([t1, t2, t3]);
            r2.append(t4);
            table.append([r1, r2]);

            [[t1, a], [t2, a], [t3, b], [t4, b]].forEach(function(p) {
                $(p[0]).click(function() {
                    app.launch(dir, p[1]);
                });
            });
        }
    };

    app.loadLibrary().then(function() {
        app.chdir(app.fs);
    });
});
