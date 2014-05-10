(function() {
    "use strict";

    function NotificationControls(controller) {
        this.controller = controller;
        NotificationControls.requestPermission(function() {
            this.showNotification();
        }.bind(this));
    }

    $.extend(NotificationControls, {
        requestPermission: function(grantedCallback) {
            if(Notification.permission === "granted") {
                grantedCallback();
            } else if(Notification.permission === "default") {
                Notification.requestPermission(function(permission) {
                    if(permission === "granted") {
                        grantedCallback();
                    }
                });
            }
        }
    });

    NotificationControls.prototype = {
        showNotification: function() {
            this.notification = new Notification("Skip song", {
                body: "102.9MHz tapeFM"
            });
            this.notification.onclick = function() {
                this.controller.skip();
                this.showNotification();
            }.bind(this);
        }
    };

    tapefm.NotificationControls = NotificationControls;
}());
