define([], () => {
    let getRatingHistory = (callback, url) => {
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

    let updateRatingHistory = (callback, url, payload) => {
        fetch(url, {
            method: 'PUT',
            body: JSON.stringify(payload),
            headers: new Headers({
                'Content-Type': 'application/json',
                'Authorization': 'Bearer ' + localStorage.getItem("jwt")
            })
        })
            .then(response => response.json())
            .then(json => {
                callback(json);
            });
    };

    return {
        getRatingHistory,
        updateRatingHistory
    }
});