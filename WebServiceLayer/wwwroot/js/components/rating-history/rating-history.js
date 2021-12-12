define(["knockout", "ratingHistoryService", "postman"], function (ko, rhs, postman) {
    return function (params) {
        const jwt = atob(localStorage.getItem("jwt").split('.')[1]);
        const url = "api/rating-history/user/" + JSON.parse(jwt).id;

        let ratingHistory = ko.observableArray([]);

        let deleteRatingHistory = () => {
            rhs.deleteRatingHistory(() => { }, url);

            location.reload();
        }

        rhs.getRatingHistory(json => {
            if (json !== undefined) {
console.log(json);
                ratingHistory(json);
            }
        }, url);

        
        return {
            ratingHistory,
            deleteRatingHistory
        }
    };
});