define(["knockout", "personService", "postman"], function (ko, ps, postman) {
    return function (params) {
        let person = ko.observable();
        person(params);
        
        let back = () => {
            ps.getPersons(json => {
                postman.publish("changeView", "person", json);
            }, params.currentPage);
        };


        coActors = ps.coActors(() => { },
            `api/persons/co-actor/${person().name}`)

        return {
            back,
            person,
            coActors
        }
    };
});