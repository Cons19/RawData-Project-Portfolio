define(["knockout", "postman", "functionService"], function (ko, postman, fs) {
    return function (params) {
        let firstWord = ko.observable();
        let secondWord = ko.observable();
        let thirdWord = ko.observable();
        let titles = ko.observableArray();
        let url = "api/titles/best-match?";
        let errorMessage = document.getElementById("error-message");
 
        let search = () => {
            if (typeof firstWord() !== "undefined")
            {
                url = url + "word1=" + firstWord() + "&";
            }

            if (typeof secondWord() !== "undefined")
            {
                url = url + "word2=" + secondWord() + "&";
            }

            if (typeof thirdWord() !== "undefined")
            {
                url = url + "word3=" + thirdWord() + "&";
            }

            if (typeof firstWord() !== "undefined" && firstWord().match('[=!@#$%^*?"{}|<>;]')) {
                alert("The firstWord can't contain invalid characters!");
            } else if (typeof secondWord() !== "undefined" && secondWord().match('[=!@#$%^*?"{}|<>;]')) {
                alert("The secondWord can't contain invalid characters!");
            } else if (typeof thirdWord() !== "undefined" && thirdWord().match('[=!@#$%^*?"{}|<>;]')) {
                alert("The thirdWord can't contain invalid characters!");
            } else {
                fs.bestMatch(json => {
                    if (json.status !== 404) {
                        let temporaryArray = [];
                        json.forEach(element => {
                            let title = { id: element.id, rank: element.rank, title: element.title };
                            temporaryArray.push(title);
                        });
                        errorMessage.textContent = "";
                        titles(temporaryArray);
                    } else {
                        errorMessage.textContent = "No titles were found for this search.";
                        titles("");
                    }

                    url = "api/titles/best-match?";
                    firstWord("");
                    secondWord("");
                    thirdWord("");
                    temporaryArray = [];
                }, url);
            }
        }

        return {
            search,
            firstWord,
            secondWord,
            thirdWord,
            titles
        }
    };
});