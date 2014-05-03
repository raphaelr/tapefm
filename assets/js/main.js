$(function() {
    "use strict";

    var view = $("#page");
    var controller = new tapefm.Controller(view);

    // Controls
    var breadcrumbs = new tapefm.Breadcrumbs($("header .path"), controller);

    view.on("tapefm:chdir", function(ev, dir) {
        loadEntriesIntoView(getSortedEntries(dir));
        breadcrumbs.render(dir);
    });

    function loadEntriesIntoView(entries) {
        var list = $("article ul");
        list.empty();
        entries.forEach(function(entry) {
            list.append(createLiForEntry(entry));
        });
    }

    function createLiForEntry(entry) {
        var li = $("<li></li>");
        li.text(entry.name);
        if(entry instanceof tapefm.Directory) {
            li.addClass("dir");
            li.click(function() {
                controller.chdir(entry);
            });
        }
        return li;
    }

    function getSortedEntries(dir) {
        return dir.getEntriesArray().sort(function(a, b) {
            return (a instanceof tapefm.File) - (b instanceof tapefm.File) ||
                a.getName().localeCompare(b.getName());
        });
    }

    view.trigger("tapefm:ready");

    $.extend(tapefm, {
        mainView: view,
        mainController: controller
    });
});
