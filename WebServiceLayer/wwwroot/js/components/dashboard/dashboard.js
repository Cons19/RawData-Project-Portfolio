define(["knockout", "postman"], function (ko, postman) {
    return function (params) {
        isUserAuth = localStorage.getItem("jwt")

        let currentView
        if (isUserAuth) {
            currentView = ko.observable("dashboard");
        } else {
            currentView = ko.observable("login-user");
        }

        let titleButton = () => {
            postman.publish("changeView", "title");
        }
        let personButton = () => {
            postman.publish("changeView", "person");
        }

        postman.subscribe("changeView", function (data) {
            currentView(data);
        });

        postman.subscribe("loginUser", login => {
        }, "dashboard");

        return {
            titleButton,
            personButton
        }
    };
});