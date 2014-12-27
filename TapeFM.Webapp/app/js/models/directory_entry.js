Ext.define("TapeFM.model.DirectoryEntry", {
    extend: "Ext.data.Model",
    fields: [
        { name: "isDirectory", type: "boolean" },
        { name: "name", type: "string" },
        { name: "fullPath", type: "string" }
    ]
});
