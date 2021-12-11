define(["knockout", "postman"], function (ko, postman) {
    return function (params) {
        let user = ko.observable();
        let userId = JSON.parse(atob(localStorage.getItem("jwt").split('.')[1])).id;

        let back = () => {
            postman.publish("changeView", "dashboard");  
        };

        return {
          back  
        }
    };
});