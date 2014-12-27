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
            var text = component;
            if(index === 0) {
                text = "Library";
            }

            self.add({
                text: text,
                listeners: {
                    click: {
                        fn: function() {
                            var path = components.slice(0, index).join("/");
                            self.fireEvent("breadcrumbclick", self, path);
                        }
                    }
                }
            });
        });
    },
});
