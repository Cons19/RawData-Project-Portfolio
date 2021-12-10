define(["knockout", "titleService", "postman"], function (ko, ts, postman) {
    return function (params) {
        const url = "api/titles"
        let titles = ko.observableArray([]);
        let prev, next;

        ts.getTitles(json => {
            titles(json.items);
            prev = json.prev;
            next = json.next;
        }, url);

        let previousPageButton = () => {
            ts.getTitles((json) => {
                titles(json.items)
                prev = json.prev;
                next = json.next;
            }, prev)
        }

        let nextPageButton = () => {
            ts.getTitles((json) => {
                titles(json.items)
                prev = json.prev;
                next = json.next;
            }, next)
        }

        let details = (data) => {
            postman.publish("changeView", "title-details", data);
        }

        return {
            titles,
            nextPageButton,
            previousPageButton,
            details
        }
    };
});