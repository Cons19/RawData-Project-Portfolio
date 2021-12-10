define(["knockout", "searchHistoryService", "postman"], function (ko, shs, postman) {
    return function (params) {
        const jwt = atob(localStorage.getItem("jwt").split('.')[1])
        const url = "api/search-history/user/" + JSON.parse(jwt).id

        let searchHistory = ko.observableArray([]);

        let deleteRow = () => {
            shs.deleteSearchHistory(() => { }, url);

            location.reload();
        }

        shs.getSearchHistory(json => {
            if (json !== undefined ) {
                searchHistory(json);
            }
        }, url);

        
        return {
            searchHistory,
            deleteRow
        }
    };
});