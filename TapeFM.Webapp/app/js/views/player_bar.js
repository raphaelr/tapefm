Ext.define("TapeFM.view.PlayerBar", {
    extend: "Ext.toolbar.Toolbar",
    alias: "widget.playerbar",

    viewModel: {
        data: {
            status: TapeFM.model.ServerStatus.getInstance()
        },
        formulas: {
            playPauseCommand: function(get) {
                return get("status.isPaused") ? "Play" : "Pause";
            }
        }
    },

    items: [
        {
            bind: {
                text: "{playPauseCommand}"
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
            text: "Skip",
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
                    muteUnmuteCommand: function(get) {
                        return get("isPlaying") ? "Mute" : "Unmute";
                    }
                }
            },
            bind: {
                text: "{muteUnmuteCommand}"
            }
        }
    ]
});
