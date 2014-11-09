function TapeFmApplication() {
    var self = {};
    var views = {};
    var currentView = ko.observable();

    self.navigateTo = {};
    self.dbgViews = views;

    self.registerView = function(spec) {
        var name = spec.name;
        var view = ko.observable();

        views[name] = ko.computed(function() {
            return (spec.permanent || currentView() === view()) ? view() : null;
        });

        if (spec.permanent) {
            view(new spec.constructor());
        } else {
            self.navigateTo[name] = function (invocationSpec) {
                view(new spec.constructor(invocationSpec));
                currentView(view());
            };
        }
    }

    self.initialize = function () {
        $.connection.hub.start();
        ko.applyBindings(views);
        self.navigateTo.loading();
    };

    return self;
}

window.app = new TapeFmApplication();

document.addEventListener("DOMContentLoaded", function() {
    app.initialize();
    var loader = new LibraryLoader();
    loader.onLoad(function() {
        app.navigateTo.library(loader.rootDirectory);
    });
});