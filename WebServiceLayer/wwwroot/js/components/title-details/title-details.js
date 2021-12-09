define(["knockout", "postman"], function (ko, postman) {
    return function (params) {
        let title = ko.observable();

        postman.subscribe("titleDetails", function (data) {
            title(data);
            console.log("data from details INSIDE", title());
        });

        console.log("data from details", title());

        // It should go back to the original page, not the first one...
        let back = () => {
            postman.publish("changeView", "title");
        };

        return {
            back,
            title
        }
    };
});