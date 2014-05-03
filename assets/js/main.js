$(function() {
    "use strict";

    var view = $("#page");
    var controller = new tapefm.Controller(view);

    // Controls
    var breadcrumbs = new tapefm.Breadcrumbs($("header .path"), controller);
    var directoryListing = new tapefm.DirectoryListing($("article ul"), controller);

    view.on("tapefm:chdir", function(ev, dir) {
        breadcrumbs.render(dir);
        directoryListing.render(dir);
    });

    view.trigger("tapefm:ready");

    $.extend(tapefm, {
        mainView: view,
        mainController: controller
    });
});
