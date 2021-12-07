define(["knockout", "postman"], function (ko, postman) {
    return function (params) {

        postman.subscribe("loginUser", login => {
        }, "dashboard");

        return {
        }
    };
});