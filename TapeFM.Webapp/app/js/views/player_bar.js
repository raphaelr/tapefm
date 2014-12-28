Ext.define("TapeFM.view.PlayerBar", {
    extend: "Ext.toolbar.Toolbar",
    alias: "widget.playerbar",

    viewModel: {
        data: {
            status: TapeFM.model.ServerStatus.getInstance()
        },
        formulas: {
            playPauseGlyph: function(get) {
                return String(get("status.isPaused") ? 0xf04b : 0xf04c);
            }
        }
    },

    items: [
        {
            bind: {
                glyph: "{playPauseGlyph}"
            },
            listeners: {
                click: function() {
                    var serverStatus = TapeFM.model.ServerStatus.getInstance();
                    serverStatus.patch({
                        isPaused: !serverStatus.get("isPaused")
                    });
                }
            }
        },
        {
            xtype: "label",
            flex: 1,
            bind: {
                text: "{status.currentTrack}"
            }
        },
        {
            glyph: 0xf04e,
            listeners: {
                click: function() {
                    TapeFM.model.ServerStatus.getInstance().skip();
                }
            }
        },
        {
            controller: "player",
            viewModel: {
                data: {
                    isPlaying: false
                },
                formulas: {
                    muteUnmuteGlyph: function(get) {
                        return String(get("isPlaying") ? 0xf028 : 0xf026);
                    }
                }
            },
            bind: {
                glyph: "{muteUnmuteGlyph}"
            }
        }
    ]
});
