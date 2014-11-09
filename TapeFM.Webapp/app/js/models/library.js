function LibraryNode(spec) {
    var self = {};

    self.name = spec.name;
    self.fullPath = spec.fullPath || "";

    return self;
}

function LibraryDirectory(spec) {
    var self = new LibraryNode(spec);
    var entryMap = {};

    self.type = "directory";

    self.hasEntry = function (name) {
        return entryMap.hasOwnProperty(name);
    };

    self.getEntry = function (name) {
        return entryMap[name];
    };

    self.addEntry = function (entry) {
        entryMap[entry.name] = entry;
        entry.fullPath = self.fullPath + "/" + entry.name;
        entry.parent = self;
        return entry;
    };

    self.getEntriesArray = function () {
        return Object.keys(entryMap).sort().map(function (key) { return entryMap[key]; });
    }

    return self;
}

function LibrarySong(song) {
    var self = new LibraryNode({ name: song.path.split('/').pop() });

    self.type = "song";
    self.song = song;

    return self;
}

LibraryNode.createFromSongs = function (songs) {
    var root = new LibraryDirectory({ name: "root" });

    function findParent(path) {
        var head = root;
        var split = path.split("/");

        split.pop();
        var ok = split.every(function (part) {
            if (head.type === "directory") {
                var entry = head.hasEntry(part)
                    ? head.getEntry(part)
                    : head.addEntry(new LibraryDirectory({ name: part }));
                head = entry;
                return true;
            } else {
                return false;
            }
        });

        return ok ? head : null;
    }

    songs.forEach(function (song) {
        findParent(song.path).addEntry(new LibrarySong(song));
    });

    return root;
};

function LibraryLoader() {
    var self = {};
    var loadXhr;

    function beginLoad() {
        loadXhr = $.get("/api/songs", function (result) {
            self.rootDirectory = LibraryNode.createFromSongs(result);
        });
    };

    self.onLoad = function (callback) {
        loadXhr.then(callback);
    };

    beginLoad();
    return self;
}