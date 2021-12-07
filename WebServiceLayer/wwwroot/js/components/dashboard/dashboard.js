define(["knockout", "postman"], function (ko, postman) {
    return function (params) {

        let titleButton = () => {
            postman.publish("changeView", "title");
        }
        let personButton = () => {
            postman.publish("changeView", "person");
        }

        postman.subscribe("loginUser", login => {
        }, "dashboard");

        return {
            titleButton,
            personButton
        }
    };
});