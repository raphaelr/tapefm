(function() {
    "use strict";

    function DirectoryListing(container, controller) {
        this.container = container;
        this.controller = controller;
    }
    DirectoryListing.prototype = {
        render: function(dir) {
            this.container.empty();
            this.addEntries(dir);
        },

        addEntries: function(dir) {
            DirectoryListing.getSortedEntries(dir).forEach(function(entry) {
                this.addEntry(entry);
            }.bind(this));
        },
        
        addEntry: function(entry) {
            var li = this.createEntry(entry);
            this.container.append(li);
        },

        createEntry: function(entry) {
            var li = $("<li>");
            li.text(entry.name);
            this.attachBehaviour(li, entry);
            return li;
        },

        attachBehaviour: function(li, entry) {
            if(entry instanceof tapefm.Directory) {
                this.attachDirectoryBehaviour(li, entry);
            }
        },

        attachDirectoryBehaviour: function(li, entry) {
            li.addClass("dir");
            li.click(function() {
                this.controller.chdir(entry);
            }.bind(this));
        }
    };


    $.extend(DirectoryListing, {
        getSortedEntries: function(dir) {
            return dir.getEntriesArray().sort(DirectoryListing.compareEntries);
        },

        compareEntries: function(a, b) {
            return (a instanceof tapefm.File) - (b instanceof tapefm.File) ||
                a.getName().localeCompare(b.getName());
        }
    });

    tapefm.DirectoryListing = DirectoryListing;
}());
