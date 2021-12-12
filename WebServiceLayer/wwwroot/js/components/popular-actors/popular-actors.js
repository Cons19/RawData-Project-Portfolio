define(["knockout", "postman", "personService"], function (ko, postman, ps) {
    return function (params) {
        let title = ko.observable();
        let titles = ko.observableArray();
        let url = "api/persons/popular-actors/";
        let errorMessage = document.getElementById("error-message");
 
        let search = () => {
            if (typeof title() !== "undefined")
            {
                url = url + title();
            }
    
            ps.popularActors(json => {
                if (json.status !== 404) 
                {
                    let temporaryArray = [];
                    json.forEach(element => {
                        let title = { name : element.name, rating: element.rating };
                        temporaryArray.push(title);
                    });
                    errorMessage.textContent =  "";
                    titles(temporaryArray);
                } else {
                    errorMessage.textContent =  "No titles were found for this search.";
                    titles("");
                }

                url = "api/persons/popular-actors/";
                title("");
                temporaryArray = [];
            }, url);
        }

        return {
            search,
            title,
            titles
        }
    };
});