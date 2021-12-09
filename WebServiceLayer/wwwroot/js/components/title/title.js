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
            ts.getNextTitles((json) => {
                titles(json.items)
                prev = json.prev;
                next = json.next;
            }, prev)
        }

        let nextPageButton = () => {
            ts.getNextTitles((json) => {
                titles(json.items)
                prev = json.prev;
                next = json.next;
            }, next)
        }

        let details = (data) => {
            postman.publish("titleDetails", data);
            postman.publish("changeView", "title-details");
        }

        return {
            titles,
            nextPageButton,
            previousPageButton,
            details
        }
    };
});