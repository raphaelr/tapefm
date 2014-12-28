Ext.define("TapeFM.view.BreadcrumbBar", {
    extend: "Ext.toolbar.Toolbar",
    alias: "widget.breadcrumbbar",

    setCurrentPath: function(path) {
        var components;
        if(Ext.isEmpty(path) || path === "/") {
            components = [""];
        } else {
            components = path.split("/");
            if(components[0] !== "") {
                components.unshift("");
            }
            if(components[components.length - 1] === "") {
                components.pop();
            }
        }

        this.removeAll(true);
        var self = this;
        components.forEach(function(component, index) {
            var config = {
                listeners: {
                    click: function() {
                        var path = components.slice(0, index).join("/");
                        self.fireEvent("breadcrumbclick", self, path);
                    }
                }
            };

            if(index === 0) {
                config.glyph = 0xf001;
            } else {
                config.text = component;
            }

            self.add(config);
        });
    },
});
