define(["knockout", "titleService", "postman"], function (ko, ts, postman) {
    return function (params) {
        let titles = ko.observableArray([]);
        let prev, next;

        ts.getTitles(json => {
            titles(json.items);
            prev = json.prev;
            next = json.next;
        });

        let previousPageButton = () => {
            console.log("previous");
        }

        let nextPageButton = () => {
            console.log(next);
            ts.getNextTitles(titles, next);
        }

        return {
            titles,
            nextPageButton,
            previousPageButton
        }
    };
});