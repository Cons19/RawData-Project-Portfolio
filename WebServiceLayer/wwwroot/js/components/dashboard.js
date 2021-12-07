define(["knockout", "postman"], function (ko, postman) {
    return function (params) {

        postman.subscribe("login-user", login => {
            console.log(login);
        });


        return {
        }
    };
});