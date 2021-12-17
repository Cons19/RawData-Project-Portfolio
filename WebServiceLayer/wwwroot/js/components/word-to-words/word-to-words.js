define(["knockout", "postman", "wordService", "wordcloud"], function (ko, postman, ws, wc) {
    return function (params) {
        let word1 = ko.observable();
        let word2 = ko.observable();
        let word3 = ko.observable();
        let word4 = ko.observable();
        let word5 = ko.observable();
        let words = ko.observableArray();
        let url = "api/words?";
        let errorMessage = document.getElementById("error-message");
        document.getElementById("wordsCloud").style.display = "none";

        let search = () => {
            if (typeof word1() !== "undefined") {
                url = url + "words=" + word1() + "&";
            }

            if (typeof word2() !== "undefined") {
                url = url + "words=" + word2() + "&";
            }

            if (typeof word3() !== "undefined") {
                url = url + "words=" + word3() + "&";
            }

            if (typeof word4() !== "undefined") {
                url = url + "words=" + word4() + "&";
            }

            if (typeof word5() !== "undefined") {
                url = url + "words=" + word5();
            }

            if (typeof word1() !== "undefined" && word1().match('[=!@#$%^*?"{}|<>;]')) {
                alert("The word1 can't contain invalid characters!");
            } else if (typeof word2() !== "undefined" && word2().match('[=!@#$%^*?"{}|<>;]')) {
                alert("The word2 can't contain invalid characters!");
            } else if (typeof word3() !== "undefined" && word3().match('[=!@#$%^*?"{}|<>;]')) {
                alert("The word3 can't contain invalid characters!");
            } else if (typeof word4() !== "undefined" && word4().match('[=!@#$%^*?"{}|<>;]')) {
                alert("The word4 can't contain invalid characters!");
            } else if (typeof word5() !== "undefined" && word5().match('[=!@#$%^*?"{}|<>;]')) {
                alert("The word5 can't contain invalid characters!");
            } else {
                ws.wordToWordsQuerying(json => {
                    if (json.status === 404) {
                        errorMessage.textContent = "No word to words were found to display for the searched words.";
                        document.getElementById("wordsTable").style.display = "none";
                        document.getElementById("wordsCloud").style.display = "none";
                    } else {
                        let temporaryArray = [];
                        errorMessage.textContent = "";
                        document.getElementById("wordsTable").style.display = "block";
                        document.getElementById("wordsCloud").style.display = "block";
                        let list = [];
                        json.forEach(element => {
                            temporaryArray.push(element);
                            list.push([element.word, element.counter])
                        })
                        words(temporaryArray);

                        wc(document.getElementById('keywords'), { list });

                    }
                    temporaryArray = [];
                    url = "api/words?";
                    if (typeof word1() !== "undefined") {
                        word1(undefined);
                    }

                    if (typeof word2() !== "undefined") {
                        word2(undefined);
                    }

                    if (typeof word3() !== "undefined") {
                        word3(undefined);
                    }

                    if (typeof word4() !== "undefined") {
                        word4(undefined);
                    }

                    if (typeof word5() !== "undefined") {
                        word5(undefined);
                    }
                }, url);
            }
        }

        return {
            search,
            word1,
            word2,
            word3,
            word4,
            word5,
            words
        }
    };
});