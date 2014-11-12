app.registerView({
    name: "loading",
    constructor: function() {
        var loader = new LibraryLoader();
        loader.onLoad(function() {
            app.navigateTo.library(loader.rootDirectory);
        });
    }
});
