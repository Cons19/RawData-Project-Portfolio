define(["knockout", "loginService", "postman"], function (ko, ls, postman) {
    return function (params) {
        let name = ko.observable();
        let email = ko.observable();
        let password = ko.observable();

        let register = () => {
            let userCredentials = { name: name(), email: email(), password: password() };
            let regex = new RegExp('[a-z0-9]+@[a-z]+\.[a-z]{2,3}');
            if (typeof name() !== "undefined" && name().match('[=!@#$%^*?":{}|<>;]')) {
                alert("The name can't contain invalid characters!");
            } else if (typeof email() !== "undefined" && !regex.test(email())) {
                alert("You must write a valid email!");
            } else {
                ls.registerUser(userCredentials, user => {
                    document.getElementsByTagName("nav")[0].style.display = "block";
                    postman.publish("changeView", "title");
                });
            }
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