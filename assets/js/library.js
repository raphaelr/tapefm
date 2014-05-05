(function() {
    "use strict";

    function Library() {
        this.root = new Directory();
    }
    Library.fromFileArray = function(files) {
        var lib = new Library();
        files.forEach(lib.createFile.bind(lib));
        return lib;
    };
    Library.prototype = {
        getRoot: function() {
            return this.root;
        },

        createFile: function(name) {
            var dir = this.getOrCreateDirectory(path.dirname(name));
            dir.createFile(path.filename(name));
        },

        getOrCreateDirectory: function(name) {
            var head = this.root;
            path.split(name).forEach(function(currentDirectory) {
                head = head.getOrCreateDirectory(currentDirectory);
            });
            return head;
        },

        getRandomFile: function() {
            var current = this.root;
            while(!(current instanceof File)) {
                var entries = current.getEntriesArray();
                if(entries.length === 0) {
                    return null;
                }
                current = entries[Math.floor(Math.random() * entries.length)];
            }
            return current;
        },

        resolve: function(name) {
            var head = this.root;
            path.split(name).forEach(function(currentEntry) {
                head = head ? head.getEntry(currentEntry) : null;
            });
            return head;
        }
    };

    function DirectoryEntry(parent, name) {
        this.parent = parent;
        this.name = name;
    }
    DirectoryEntry.prototype = {
        getName: function() {
            return this.name;
        },

        getParent: function() {
            return this.parent;
        },

        getFullPath: function() {
            var result = "";
            if(this.parent && this.parent.name) {
                result += this.parent.getFullPath();
            }
            return result + "/" + (this.name || "");
        }
    };
    
    function Directory() {
        DirectoryEntry.apply(this, arguments);
        this.entries = {};
    }
    Directory.prototype = Object.create(DirectoryEntry.prototype);
    $.extend(Directory.prototype, {
        createFile: function(name) {
            this.createEntry(File, name);
        },

        getOrCreateDirectory: function(name) {
            this.createDirectoryUnlessExists(name);
            return this.getEntry(name);
        },

        createDirectoryUnlessExists: function(name) {
            if(!this.hasEntry(name)) {
                this.createDirectory(name);
            }
        },

        hasEntry: function(name) {
            return this.entries.hasOwnProperty(name);
        },

        createDirectory: function(name) {
            this.createEntry(Directory, name);
        },

        createEntry: function(Type, name) {
            this.entries[name] = new Type(this, name);
        },

        getEntry: function(name) {
            if(name === "") {
                return this;
            }
            return this.entries[name];
        },

        getEntriesArray: function() {
            return Object.keys(this.entries).map(function(k) {
                return this.entries[k];
            }.bind(this));
        },
    });

    function File() {
        DirectoryEntry.apply(this, arguments);
    }
    File.prototype = Object.create(DirectoryEntry.prototype);
    $.extend(File.prototype, {
    });

    var path = {
        split: function(name) {
            if(name[0] === "/") {
                name = name.slice(1);
            }
            return name.split("/");
        },
        
        join: function(parts) {
            return parts.join("/");
        },
        
        dirname: function(name) {
            var parts = path.split(name);
            parts.pop();
            return path.join(parts);
        },

        filename: function(name) {
            var parts = path.split(name);
            return parts.pop();
        }
    };

    $.extend(tapefm, {
        Library: Library,
        File: File,
        Directory: Directory,
        path: path
    });
}());
