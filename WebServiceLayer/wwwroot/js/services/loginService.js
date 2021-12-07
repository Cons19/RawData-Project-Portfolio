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
                // localStorage.getItem("lastname");
            })
            .catch(function (error) {
                alert("Not authorized");
            });
        };

    return {
        loginUser
    }
});