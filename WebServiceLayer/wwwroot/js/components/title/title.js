define(["knockout", "titleService", "postman"], function (ko, ts, postman) {
    return function (params) {
        console.log("params titles", params);
        let url = "api/titles"
        //if (params.currentPage) {
        //    url = params.currentPage;
        //}

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