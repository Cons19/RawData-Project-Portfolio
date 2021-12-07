define([], () => {

    let getTitles = (callback) => {
        fetch("api/titles", {
            method: 'GET',
            headers: new Headers({
                'Authorization': 'Bearer ' + localStorage.getItem("jwt")
            })             
        })
        .then(response => response.json())
            .then(json => {
                console.log("Titles:", json);
                callback(json);
            });
    };
    
    let getNextTitles = (callback, next) => {
        fetch(next, {
            method: 'GET',
            headers: new Headers({
                'Authorization': 'Bearer ' + localStorage.getItem("jwt")
            })
        })
            .then(response => response.json())
            .then(json => {
                console.log("Titles:", json);
                callback(json.items);
            });
    };

    return {
        getTitles,
        getNextTitles
    }
});