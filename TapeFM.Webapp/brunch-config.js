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
    },

    modules: {
        wrapper: false,
        definition: false
    }
};
