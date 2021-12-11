define(["knockout", "titleService", "postman"], function (ko, ts, postman) {
    return function (params) {
        let url;

        if (params) {
            url = params.cur;
        } else
        {
            url = "api/titles";
        }

        let titles = ko.observableArray([]);
        let prev, cur, next;

        ts.getTitles(json => {
            titles(json.items);
            prev = json.prev;
            cur = json.cur;
            next = json.next;
        }, url);

        let previousPageButton = () => {
            ts.getTitles((json) => {
                titles(json.items)
                prev = json.prev;
                cur = json.cur;
                next = json.next;
            }, prev)
        }

        let nextPageButton = () => {
            ts.getTitles((json) => {
                titles(json.items)
                prev = json.prev;
                cur = json.cur;
                next = json.next;
            }, next)
        }
        
        let details = (data) => {
            data.currentPage = cur;
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