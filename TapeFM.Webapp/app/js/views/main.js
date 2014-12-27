Ext.define("TapeFM.view.Main", {
    extend: "Ext.grid.Panel",
    controller: "main",

    hideHeaders: true,

    store: {
        model: "TapeFM.model.DirectoryEntry",
        proxy: {
            type: "ajax",
            url: "/api/browse"
        }
    },

    tbar: {
        xtype: "breadcrumbbar",
        reference: "breadcrumbs",

        listeners: {
            breadcrumbclick: "onBreadcrumbClick"
        }
    },

    bbar: {
        xtype: "playerbar"
    },

    columns: [
        { text: "Name", dataIndex: "name", flex: 1 }
    ],

    listeners: {
        itemclick: "onItemClick"
    }
});
