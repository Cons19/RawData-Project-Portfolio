define(["knockout", "loginService", "postman"], function (ko, ls, postman) {
    return function (params) {
        let name = ko.observable();
        let email = ko.observable();
        let password = ko.observable();

        let register = () => {
            let userCredentials = { name: name(), email: email(), password: password() };
            ls.registerUser(userCredentials, user => {
                document.getElementsByTagName("nav")[0].style.display = "block";
                postman.publish("changeView", "title");
            });
            name("");
            email("");
            password("");
        }

        let cancel = () => {
            postman.publish("changeView", "login-user");
        }

        return {
            name,
            email,
            password,
            cancel,
            register
        }
    };
});