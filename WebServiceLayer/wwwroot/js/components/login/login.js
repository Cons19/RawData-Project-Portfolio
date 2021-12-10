define(["knockout", "loginService", "postman"], function (ko, ls, postman) {
    return function (params) {

        let email = ko.observable();
        let password = ko.observable();

        let login = () => {
            let userCredentials = { email: email(), password: password() };
            ls.loginUser(userCredentials, user => {
                document.getElementsByTagName("nav")[0].style.display = "block";
                postman.publish("changeView", "dashboard");
            });
            email("");
            password("");
        }

        let register = () => {
            postman.publish("changeView", "register");
        }

        return {
            email,
            password,
            login,
            register
        }
    };
});