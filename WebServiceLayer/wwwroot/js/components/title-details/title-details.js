define(["knockout", "titleService", "postman"], function (ko, ts, postman) {
    return function (params) {
        let title = ko.observable();

        title(params);

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