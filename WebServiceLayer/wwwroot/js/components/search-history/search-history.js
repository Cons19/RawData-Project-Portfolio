define(["knockout", "searchHistoryService"], function (ko, shs) {
    return function (params) {
        const jwt = atob(localStorage.getItem("jwt").split('.')[1])
        const url = "api/search-history/user/" + JSON.parse(jwt).id
        let searchHistory = ko.observableArray([]);

        shs.getSearchHistory(json => {
            if (json !== undefined ) {
                searchHistory(json);
            }
        }, url);

        return {
            searchHistory
        }
    };
});