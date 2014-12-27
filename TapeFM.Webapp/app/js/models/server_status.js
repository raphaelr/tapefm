Ext.define("TapeFM.model.ServerStatus", {
    extend: "Ext.data.Model",

    fields: [
        { name: "currentTrack", type: "string" },
        { name: "isPaused", type: "boolean" },
        { name: "bitrateKbps", type: "int" },
        { name: "emptyPlaylistMode", type: "string" }
    ],

    proxy: {
        type: "ajax",
        url: "/api/status"
    },

    initializeLiveUpdates: function() {
        var self = this;
        function reconnect() {
            $.connection.hub.start({
                callback: function() {
                    self.load();
                }
            });
        }

        $.connection.statusUpdates.client.update = function() {
            self.load();
        };
        $.connection.hub.disconnected(function() {
            setTimeout(function() {
                reconnect();
            }, 5000);
        });
        reconnect();
    },

    patch: function(data) {
        Ext.Ajax.request({
            url: "/api/status",
            method: "POST",
            jsonData: data,

            scope: this,
            success: function() {
                this.load();
            }
        });
    },

    skip: function() {
        Ext.Ajax.request({
            url: "/api/skip",
            method: "POST"
        });
    },

    statics: {
        getInstance: function() {
            if(!this.instance) {
                this.instance = this.create();
                this.instance.load();
                this.instance.initializeLiveUpdates();
            }
            return this.instance;
        }
    }
});
