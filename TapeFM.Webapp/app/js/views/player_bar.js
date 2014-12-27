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
            xtype: "button",
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
