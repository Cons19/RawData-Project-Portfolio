define([], () => {

    let loginUser = (userCredentials, callback) => {
        let param = {
            method: "POST",
            body: JSON.stringify(userCredentials),
            headers: {
                "Content-Type": "application/json"
            }
        }
        fetch("/api/users/login", param)
            .then(response => {
                if (response.status == 401)
                    throw new Error(response.status);
                return response.json();
            })
            .then(json => {
                localStorage.setItem("jwt", json.token);
                callback(json);
            })
            .catch(function (error) {
                errorMessage = document.getElementById("error-message")
                if (errorMessage != null) {
                    errorMessage.textContent = "Unable to login this user."
                }
            });
        };  

    return {
        loginUser
    }
});