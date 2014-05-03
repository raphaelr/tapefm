(function() {
    "use strict";

    function Breadcrumbs(container, controller) {
        this.container = container;
        this.controller = controller;
    }
    Breadcrumbs.prototype = {
        render: function(dir) {
            this.container.empty();
            this.addBreadcrumbRecursive(dir);
        },

        addBreadcrumbRecursive: function(dir) {
            if(!dir) return;

            this.addBreadcrumbRecursive(dir.getParent());
            this.addBreadcrumb(dir);
        },

        addBreadcrumb: function(dir) {
            var crumb = this.createBreadcrumb(dir);
            this.styleBreadcrumb(crumb, dir);
            this.container.append(crumb);
        },

        createBreadcrumb: function(dir) {
            var crumb = $("<div class='dir'>");
            crumb.text(dir.getName());
            crumb.click(function() {
                this.controller.chdir(dir);
            }.bind(this));
            return crumb;
        },

        styleBreadcrumb: function(crumb, dir) {
            if(!dir.getParent()) {
                crumb.addClass("glyphicon glyphicon-music");
            }
        }
    };

    tapefm.Breadcrumbs = Breadcrumbs;
}());
