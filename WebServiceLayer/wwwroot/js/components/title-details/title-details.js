define(["knockout", "titleService", "postman"], function (ko, ts, postman) {
    return function (params) {
        let title = ko.observable();
        console.log(params);
        title(params);

        // It should go back to the original page, not the first one...
        let back = () => {
            ts.getTitles(json => {
                postman.publish("changeView", "title", json);
            }, params.currentPage);
        };

        return {
            back,
            title
        }
    };
});