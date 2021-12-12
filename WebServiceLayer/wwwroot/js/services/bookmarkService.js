define([], () => {
    let getBookmarkTitle = (callback, url) => {
        fetch(url, {
            method: 'GET',
            headers: new Headers({
                'Authorization': 'Bearer ' + localStorage.getItem("jwt")
            })
        })
        .then(response => response.json())
        .then(json => {
            callback(json);
        });
    };

    let getBookmarkPerson = (callback, url) => {
        fetch(url, {
            method: 'GET',
            headers: new Headers({
                'Authorization': 'Bearer ' + localStorage.getItem("jwt")
            })
        })
        .then(response => response.json())
        .then(json => {
            callback(json);
        });
    };

    let deleteBookmarkTitle = (callback, url) => {
        let param = {
            method: "DELETE",
            headers: {
                'Authorization': 'Bearer ' + localStorage.getItem("jwt")
            }
        };
        fetch(url, param)
            .then(() => {
                callback();
            });
    };

    let deleteBookmarkPerson = (callback, url) => {
        let param = {
            method: "DELETE",
            headers: {
                'Authorization': 'Bearer ' + localStorage.getItem("jwt")
            }
        };
        fetch(url, param)
            .then(() => {
                callback();
            });
    };

    let createBookmarkTitle = (bookmarkTitle, callback) => {
        let param = {
            method: "POST",
            body: JSON.stringify(bookmarkTitle),
            headers: {
                "Content-Type": "application/json",
                'Authorization': 'Bearer ' + localStorage.getItem("jwt")
            }
        }
        fetch("api/bookmark-titles", param)
            .then(response => {
                if (response.status == 401)
                    throw new Error(response.status);
                return response.json();
            })
            .then(json => {
                console.log(json);
                callback(json);
            })
            .catch(function (error) {
                console.log("error:", error);
                errorMessage = document.getElementById("error-message")
                if (errorMessage != null) {
                    errorMessage.textContent = "Unable to create this bookmark."
                }
            });
    };

    let createBookmarkPerson = (bookmarkPerson, callback) => {
        let param = {
            method: "POST",
            body: JSON.stringify(bookmarkPerson),
            headers: {
                "Content-Type": "application/json",
                'Authorization': 'Bearer ' + localStorage.getItem("jwt")
            }
        }
        fetch("api/bookmark-persons", param)
            .then(response => {
                if (response.status == 401)
                    throw new Error(response.status);
                return response.json();
            })
            .then(json => {
                console.log(json);
                callback(json);
            })
            .catch(function (error) {
                console.log("error:", error);
                errorMessage = document.getElementById("error-message")
                if (errorMessage != null) {
                    errorMessage.textContent = "Unable to create this bookmark."
                }
            });
    };

    return {
        getBookmarkTitle,
        getBookmarkPerson,
        deleteBookmarkTitle,
        deleteBookmarkPerson,
        createBookmarkTitle,
        createBookmarkPerson
    }
});