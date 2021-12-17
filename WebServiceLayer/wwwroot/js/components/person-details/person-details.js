define(["knockout", "personService", "postman"], function (ko, ps, postman) {
    return function (params) {
        let person = ko.observable();
        let coActors = ko.observableArray([]);
        let errorMessage = document.getElementById("error-message");
        person(params);
        
        let back = () => {
            ps.getPersons(json => {
                postman.publish("changeView", "person", json);
            }, params.currentPage);
        };

        ps.coActors((json) => {
            if (json.status === 404) {
                errorMessage.textContent = "No co actors were found for this person.";
                document.getElementById("coActors").style.display = "none";
            } else {
                errorMessage.textContent = "";
                document.getElementById("coActors").style.display = "block";
                coActors(json);
            }
        }, `api/persons/co-actor/${person().name}`)

        return {
            back,
            person,
            coActors
        }
    };
});