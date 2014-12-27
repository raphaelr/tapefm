Ext.define("TapeFM.controller.Main", {
    extend: "Ext.app.ViewController",
    alias: "controller.main",

    init: function() {
        this.control({
            "gridpanel": {
                rowclick: {
                    scope: this,
                    fn: this.onRowClick
                }
            },
            "breadcrumbbar": {
                breadcrumbclick: {
                    scope: this,
                    fn: this.onBreadcrumbClick
                }
            }
        });
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

    onRowClick: function(sender, record) {
        if(record.get("isDirectory")) {
            this.browse(record.get("fullPath"));
        } else {
            TapeFM.model.ServerStatus.getInstance().patch({
                currentTrack: record.get("fullPath")
            });
        }
    },

    onBreadcrumbClick: function(sender, path) {
        this.browse(path);
    }
});
