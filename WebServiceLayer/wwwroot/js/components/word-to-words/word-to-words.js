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

            ws.wordToWordsQuerying(json => {
                if (json.status === 404) {
                    errorMessage.textContent = "No word to words were found to display for the searched words.";
                    document.getElementById("wordsTable").style.display = "none";
                } else {
                    errorMessage.textContent = "";
                    document.getElementById("wordsTable").style.display = "block";
                    console.log(json);
                    words(json);
                    console.log(WordCloud.isSupported)
                    let list = [];
                    json.forEach(element => {
                        list.push([element.word, element.counter])
                    })
                    console.log(list);
                    wc(document.getElementById('keywords'), { list });
                }


            }, url);
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