define(["knockout", "loginService", "postman"], function (ko, ls, postman) {
    return function (params) {
        let currentView = params.currentView;

        let email = ko.observable();
        let password = ko.observable();

        let login = () => {
            console.log("login");
            let userCredentials = { email: email(), password: password() };
            ls.loginUser(userCredentials, user => {
                console.log(user);
                postman.publish("changeView", "dashboard");
            });
            email("");
            password("");
        }

        return {
            email,
            password,
            login
        }
    };
});