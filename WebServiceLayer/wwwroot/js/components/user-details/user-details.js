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
            //let updatedUser = {name: name(), email: email(), password: password()};
            let updatedUser = {name: name(), email: email()};
            us.updateUser(updatedUser, json => {
                if (json !== undefined) {
                    alert("User updated!");
                    location.reload();
                }
            }, url);
        }

        let remove = () => {
            us.removeUser(() => {
                alert("User deleted!");
                localStorage.removeItem("jwt");
                location.reload();
            }, url);
        };

        let back = () => {
            postman.publish("changeView", "dashboard");  
        };

        return {
            name,
            email,
            password,
            update,
            remove,
            back  
        }
    };
});