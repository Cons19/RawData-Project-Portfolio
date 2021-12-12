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

    let deleteRatingHistory = (callback, url) => {
        fetch(url, {
            method: 'DELETE',
            headers: new Headers({
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
        deleteRatingHistory
    }
});