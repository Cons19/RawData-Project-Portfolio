define([], () => {

    let getPersons = (callback, url) => {
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

    let popularActors = (callback, url) => {
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

    let coActors = (callback, url) => {
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

    let findPersonByProfession = (callback, url) => {
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

    return {
        getPersons,
        popularActors,
        coActors,
        findPersonByProfession
    }
});