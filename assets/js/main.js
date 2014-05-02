$(function() {
    "use strict";

    var view = $("#page");
    var controller = new tapefm.Controller(view);

    view.on("tapefm:chdir", function(ev, dir) {
        loadEntriesIntoView(getSortedEntries(dir));
        loadBreadcrumbs(dir);
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

    function loadBreadcrumbs(dir) {
        getBreadcrumbContainer().empty();
        addBreadcrumb(controller.library.getRoot()).addClass("glyphicon").addClass("glyphicon-music");
        addBreadcrumbRecursive(dir);
    }
    
    function getBreadcrumbContainer() {
        return $("header .path");
    }

    function addBreadcrumb(dir) {
        var crumb = $("<div class='dir'></div>");
        crumb.text(dir.getName());
        crumb.click(function() {
            controller.chdir(dir);
        });
        getBreadcrumbContainer().append(crumb);
        return crumb;
    }

    function addBreadcrumbRecursive(dir) {
        if(dir.getParent()) {
            addBreadcrumbRecursive(dir.getParent());
        }
        if(dir.getName()) {
            addBreadcrumb(dir);
        }
    }

    view.trigger("tapefm:ready");

    $.extend(tapefm, {
        mainView: view,
        mainController: controller
    });
});
