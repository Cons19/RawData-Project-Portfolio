define(["knockout", "personService"], function (ko, ps) {
    return function (params) {
        const url = "api/persons"
        let persons = ko.observableArray([]);
        let prev, next;

        ps.getPersons(json => {
            persons(json.items);
            prev = json.prev;
            next = json.next;
        }, url);

        let previousPageButton = () => {
            ps.getPersons((json) => {
                persons(json.items)
                prev = json.prev;
                next = json.next;
            }, prev)
        }

        let nextPageButton = () => {
            ps.getPersons((json) => {
                persons(json.items)
                prev = json.prev;
                next = json.next;
            }, next)
        }


        return {
            persons,
            nextPageButton,
            previousPageButton
        }
    };
});