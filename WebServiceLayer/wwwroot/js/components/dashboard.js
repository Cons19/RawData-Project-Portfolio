define(["knockout", "postman"], function (ko, postman) {
    return function (params) {

        postman.subscribe("loginUser", login => {
            console.log("we are now on dashboard");
        }, "dashboard");

        return {
        }
    };
});