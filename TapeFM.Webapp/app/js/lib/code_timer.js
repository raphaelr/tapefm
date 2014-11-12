function CodeTimer(func, ms) {
    var self = {};
    var timeoutId;

    self.plan = function() {
        if(timeoutId) {
            clearTimeout(timeoutId);
        }
        timeoutId = setTimeout(func, ms || 500);
    };

    return self;
}
