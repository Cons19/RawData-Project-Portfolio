define(["knockout", "searchHistoryService", "buffer"], function (ko, shs, bf) {
    return function (params) {
        const jwt = Buffer.from(localStorage.getItem("jwt").split('.')[1], 'base64').toString();
        console.log(jwt)
        const url = "api/search-history/user" + `/${jwt.Id}`

        let searchHistory = ko.observableArray([]);
        let prev, next;

        shs.getSearchHistory(json => {
            searchHistory(json.items);
            prev = json.prev;
            next = json.next;
        }, url);


        postman.subscribe("changeView", function (data) {
            currentView(data);
        });

        return {

        }
    };
});