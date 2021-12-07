define(["knockout", "postman"], function (ko, postman) {
    isUserAuth = localStorage.getItem("jwt")

    let currentView
    if (isUserAuth) {
        currentView = ko.observable("dashboard");
    } else {
        currentView = ko.observable("login-user");
    }

    postman.subscribe("changeView", function (data) {
        currentView(data);
    });

    return {
        currentView,
    }
});