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

            if (typeof profession() !== "undefined" && profession().match('[=!@#$%^*?"{}|<>;]')) {
                alert("The profession can't contain invalid characters!");
            } else {
                ps.findPersonByProfession(json => {
                    console.log(json);
                    if (json.status !== 404) {
                        let temporaryArray = [];
                        errorMessage.textContent = "";
                        json.forEach(person => {
                            temporaryArray.push(person);
                        });
                        persons(temporaryArray);
                    } else {
                        errorMessage.textContent = "No persons were found for this search.";
                        persons("");
                    }
                    temporaryArray = [];
                    url = "api/persons/profession/";
                    profession("");
                }, url);
            }
        }

        return {
            search,
            profession,
            persons
        }
    };
});