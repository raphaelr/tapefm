exports.config = {
    files: {
        javascripts: {
            joinTo: {
                "app.js": /^app/,
                "vendor.js": /^bower_components/
            },

            order: {
                after: ["app/js/app.js"]
            }
        },
        stylesheets: {
            joinTo: {
                "vendor.css": /^bower_components/
            }
        }
    },

    modules: {
        wrapper: false,
        definition: false
    }
};
