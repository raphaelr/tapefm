Ext.define("TapeFM.model.ServerStatus", {
    extend: "Ext.data.Model",

    fields: [
        { name: "currentTrack", type: "string" },
        { name: "isPaused", type: "boolean" }
    ],

    proxy: {
        type: "ajax",
        url: "/api/status"
    },

    statics: {
        getInstance: function() {
            if(!this.instance) {
                this.instance = this.create();
                this.instance.load();
            }
            return this.instance;
        }
    }
});
