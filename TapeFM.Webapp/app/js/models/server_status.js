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
