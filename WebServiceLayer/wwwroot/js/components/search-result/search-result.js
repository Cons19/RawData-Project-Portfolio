define(["knockout", "postman", "searchService"], function (ko, postman, ss) {
    return function (params) {
        let results = ko.observableArray([])
        let shouldShowButtons = ko.observable(true)
        results(params.items)

        if (params.next == null && params.prev == null) {
            shouldShowButtons(false)
        } else {
            prev = params.prev;
            next = params.next;
        }

        let previousPageButton = () => {
            ss.getSearchString((json) => {
                results(json.items)
                prev = json.prev;
                next = json.next;
            }, prev)
        }

        let nextPageButton = () => {
            ss.getSearchString((json) => {
                results(json.items)
                if (json.next == null && json.prev == null) {
                    shouldShowButtons(false)
                }
                prev = json.prev;
                next = json.next;
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