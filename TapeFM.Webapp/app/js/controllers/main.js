Ext.define("TapeFM.controller.Main", {
    extend: "Ext.app.ViewController",
    alias: "controller.main",

    init: function() {
        this.browse("/");
    },

    browse: function(dirname) {
        this.getView().getStore().load({
            params: {
                dirname: dirname
            },
            scope: this,
            callback: function() {
                this.lookupReference("breadcrumbs").setCurrentPath(dirname);
            }
        });
    },

    onItemClick: function(sender, record) {
        if(record.get("isDirectory")) {
            this.browse(record.get("fullPath"));
        }
    },

    onBreadcrumbClick: function(sender, path) {
        this.browse(path);
    }
});
