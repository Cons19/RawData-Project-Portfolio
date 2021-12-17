define(["knockout", "postman", "searchService"], function (ko, postman, ss) {
    return function (params) {
        let results = ko.observableArray([]);
        let shouldShowButtons = ko.observable(true);
        let errorMessage = document.getElementById("error-message");

        console.log(params.items);
        if (params.items !== undefined) {
            results(params.items);

            if (params.next == null && params.prev == null) {
                shouldShowButtons(false);
            } else {
                prev = params.prev;
                next = params.next;
            }
            errorMessage.textContent = "";
        } else {
            errorMessage.textContent = "No titles were found for this search.";
            results("");
        }

        let previousPageButton = () => {
            ss.getSearchString((json) => {
                if (json.status !== 404) {
                    results(json.items)
                    prev = json.prev;
                    next = json.next;
                    errorMessage.textContent = "";
                } else {
                    errorMessage.textContent = "No titles were found for this search.";
                    results("");
                }
            }, prev)
        }

        let nextPageButton = () => {
            ss.getSearchString((json) => {
                if (json.status !== 404) {
                    results(json.items)
                    if (json.next == null && json.prev == null) {
                        shouldShowButtons(false)
                    }
                    prev = json.prev;
                    next = json.next;
                    errorMessage.textContent = "";
                } else {
                    errorMessage.textContent = "No titles were found for this search.";
                    results("");
                }
            }, next)
        }

        return {
            results,
            previousPageButton,
            nextPageButton,
            shouldShowButtons
        }
    };
});