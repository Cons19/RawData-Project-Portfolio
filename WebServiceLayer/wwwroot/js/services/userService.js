define([], () => {
    
    let getUserById = (callback, url) => {
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
        
    let updateUser = (updatedUser, callback, url) => {
        let param = {
            method: "PUT",
            body: JSON.stringify(updatedUser),
            headers: {
                "Content-Type": "application/json",
                'Authorization': 'Bearer ' + localStorage.getItem("jwt")
            }
        };
        fetch(url, param)
            .then(response => {
                if (response.status == 404)
                    throw new Error(response.status);
                return response.json();
            })
            .then(json => {
                callback(json);
            })
            .catch(function (error) {
                errorMessage = document.getElementById("error-message")
                if (errorMessage != null) {
                    errorMessage.textContent = "Unable to update this user."
                }
            });
    };
        
    let removeUser = (callback, url) => {
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

    return {
        getUserById,
        updateUser,
        removeUser
    }
});