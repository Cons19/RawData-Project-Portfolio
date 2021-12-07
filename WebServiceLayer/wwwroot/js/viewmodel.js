define(["knockout", "postman"], function (ko, postman) {
    let currentView = ko.observable("login-user");

    postman.subscribe("changeView", function (data) {
        console.log(data)
        currentView(data);
    });

    return {
        currentView
    }
});