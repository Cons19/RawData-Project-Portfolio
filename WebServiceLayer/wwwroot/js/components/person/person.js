define(["knockout", "personService", "postman"], function (ko, ps, postman) {
    return function (params) {
        let url;

        if (params) {
            url = params.cur;
        } else
        {
            url = "api/persons";
        }

        let persons = ko.observableArray([]);
        let prev, cur, next;

        ps.getPersons(json => {
            persons(json.items);
            prev = json.prev;
            cur = json.cur;
            next = json.next;
        }, url);


        let previousPageButton = () => {
            ps.getPersons((json) => {
                persons(json.items)
                prev = json.prev;
                cur = json.cur;
                next = json.next;
            }, prev)
        }

        let nextPageButton = () => {
            ps.getPersons((json) => {
                persons(json.items)
                prev = json.prev;
                cur = json.cur;
                next = json.next;
            }, next)
        }

        let details = (data) => {
            data.currentPage = cur;
            postman.publish("changeView", "person-details", data);
        }

        return {
            persons,
            nextPageButton,
            previousPageButton,
            details
        }
    };
});