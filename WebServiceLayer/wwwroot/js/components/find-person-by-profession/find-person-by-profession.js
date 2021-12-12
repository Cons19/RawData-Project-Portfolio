define(["knockout", "postman", "personService"], function (ko, postman, ps) {
    return function (params) {
        let profession = ko.observable();
        let persons = ko.observableArray([]);
        let url = "api/persons/profession/";
        let errorMessage = document.getElementById("error-message");
        let search = () => {
            if (typeof profession() !== "undefined") {
                url = url + profession();
            }

            ps.findPersonByProfession(json => {
                console.log(json);
                if (json.status !== 404) {
                    errorMessage.textContent = "";
                    persons(json);
                } else {
                    errorMessage.textContent = "No persons were found for this search.";
                    persons("");
                }
            }, url);
        }

        return {
            search,
            profession,
            persons
        }
    };
});