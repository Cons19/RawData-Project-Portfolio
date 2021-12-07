define(["knockout", "titleService", "postman"], function (ko, ts, postman) {
    return function (params) {
        let titles = ko.observableArray([]);

        ts.getTitles(titles);


        let previousPageButton = () => {
        //    ts.getTitles.prev
        }
        let nextPageButton = (next) => {

            console.log(next);
            ts.getTitles(titles);
            ts.getNextTitles(titles, next);
        }


        return {
            titles,
            nextPageButton,
            previousPageButton
        }
    };
});