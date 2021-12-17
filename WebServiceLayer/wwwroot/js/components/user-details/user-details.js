define(["knockout", "userService", "postman"], function (ko, us, postman) {
    return function (params) {
        let name = ko.observable();
        let email = ko.observable();
        let password = ko.observable();
        let userId = JSON.parse(atob(localStorage.getItem("jwt").split('.')[1])).id;
        let url = "api/users/" + userId;

        name(params.name);
        email(params.email);
        password(params.password);

        let update = () => {
            let updatedUser = { name: name(), email: email() };
            let regex = new RegExp('[a-z0-9]+@[a-z]+\.[a-z]{2,3}');

            if (typeof name() !== "undefined" && name().match('[=!@#$%^*?":{}|<>;]')) {
                alert("The name can't contain invalid characters!");
            } else if (typeof email() !== "undefined" && !regex.test(email())) {
                alert("You must write a valid email!");
            } else {
                us.updateUser(updatedUser, json => {
                    if (json !== undefined) {
                        alert("User updated!");
                        location.reload();
                    }
                }, url);
            }
        }

        let remove = () => {
            us.removeUser(() => {
                alert("User deleted!");
                localStorage.removeItem("jwt");
                location.reload();
            }, url);
        };

        return {
            name,
            email,
            password,
            update,
            remove
        }
    };
});