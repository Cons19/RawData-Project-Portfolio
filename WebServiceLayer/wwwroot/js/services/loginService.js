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
            .then(response => response.json())
            .then(json => {
                localStorage.setItem("jwt", json.token);
                callback(json);
                // localStorage.getItem("lastname");
            });
    };

    return {
        loginUser
    }
});