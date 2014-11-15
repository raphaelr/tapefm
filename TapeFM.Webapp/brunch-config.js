exports.config = {
    files: {
        javascripts: {
            joinTo: {
                "app.js": /^app/,
                "vendor.js": /^bower_components/
            },
        },
        stylesheets: {
            joinTo: {
                "app.css": /^app/,
                "vendor.css": /^bower_components/
            }
        }
    },

    modules: {
        wrapper: false,
        definition: false
    }
};
