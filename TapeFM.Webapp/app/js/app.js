Ext.define("TapeFM.model.DirectoryEntry", {
    extend: "Ext.data.Model",
    fields: [
        { name: "isDirectory", type: "boolean" },
        { name: "name", type: "string" },
        { name: "fullPath", type: "string" }
    ]
});

Ext.define("TapeFM.controller.Main", {
    extend: "Ext.app.ViewController",
    alias: "controller.Main",

    init: function() {
        this.browse("/");
    },

    browse: function(dirname) {
        this.getView().getStore().load({
            params: {
                dirname: dirname
            }
        });
    },

    onItemClick: function(sender, record) {
        if(record.get("isDirectory")) {
            this.browse(record.get("fullPath"));
        }
    }
});

Ext.define("TapeFM.view.Main", {
    extend: "Ext.grid.Panel",
    controller: "Main",

    store: {
        model: "TapeFM.model.DirectoryEntry",
        proxy: {
            type: "ajax",
            url: "/api/browse"
        }
    },

    hideHeaders: true,
    columns: [
        { text: "Name", dataIndex: "name", flex: 1 }
    ],

    listeners: {
        itemclick: "onItemClick"
    }
});

Ext.application({
    name: "TapeFM",
    autoCreateViewport: "TapeFM.view.Main"
});
