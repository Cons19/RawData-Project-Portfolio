define(["knockout", "postman", "functionService"], function (ko, postman, fs) {
    return function (params) {
        let title = ko.observable();
        let plot = ko.observable();
        let character = ko.observable();
        let actor = ko.observable();
        let titles = ko.observableArray();
        let userId = JSON.parse(atob(localStorage.getItem("jwt").split('.')[1])).id;
        let url = "api/titles/structured-search?";
        let errorMessage = document.getElementById("error-message");
 
        let search = () => {
            if (typeof title() !== "undefined")
            {
                url = url + "title=" + title() + "&";
            }
    
            if (typeof plot() !== "undefined")
            {
                url = url + "plot=" + plot() + "&";
            }
    
            if (typeof character() !== "undefined")
            {
                url = url + "inputCharacter=" + character() + "&";
            }
    
            if (typeof actor() !== "undefined")
            {
                url = url + "personName=" + actor() + "&";
            }

            if (typeof title() !== "undefined" && title().match('[=!@#$%^*?"{}|<>;]')) {
                alert("The title can't contain invalid characters!");
            } else if (typeof plot() !== "undefined" && plot().match('[=!@#$%^*?"{}|<>;]')) {
                alert("The plot can't contain invalid characters!");
            } else if (typeof character() !== "undefined" && character().match('[=!@#$%^*?"{}|<>;]')) {
                alert("The character can't contain invalid characters!");
            } else if (typeof actor() !== "undefined" && actor().match('[=!@#$%^*?"{}|<>;]')) {
                alert("The actor can't contain invalid characters!");
            } else {
                url = url + "userId=" + userId;
                fs.structuredSearch(json => {
                    if (json.status !== 404) {
                        let temporaryArray = [];
                        json.forEach(element => {
                            let title = { titleId: element.id, primaryTitle: element.primaryTitle, description: element.description };
                            temporaryArray.push(title);
                        });
                        errorMessage.textContent = "";
                        titles(temporaryArray);
                    } else {
                        errorMessage.textContent = "No titles were found for this search.";
                        titles("");
                    }

                    url = "api/titles/structured-search?";
                    title("");
                    plot("");
                    character("");
                    actor("");
                    temporaryArray = [];
                }, url);
            }
        }

        return {
            search,
            title,
            plot,
            character,
            actor,
            titles
        }
    };
});