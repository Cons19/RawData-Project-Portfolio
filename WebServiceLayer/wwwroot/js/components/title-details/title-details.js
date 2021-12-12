define(["knockout", "titleService", "postman"], function (ko, ts, postman) {
    return function (params) {
        let title = ko.observable();
        let similarTitles = ko.observableArray([]);
        let url = "api/titles/similar-title/" + params.id;
        let errorMessage = document.getElementById("error-message");
        title(params);

        let back = () => {
            ts.getTitles(json => {
                postman.publish("changeView", "title", json);
            }, params.currentPage);
        };

        ts.getSimilarTitles(json => {
            if (json.status === 404) {
                errorMessage.textContent = "No similar titles were found for this title.";
                document.getElementById("similarTitles").style.display = "none";
            } else {
                errorMessage.textContent = "";
                document.getElementById("similarTitles").style.display = "block";
                console.log(json);
                similarTitles(json);
            }
        }, url);


        return {
            back,
            title,
            similarTitles
        }
    };
});