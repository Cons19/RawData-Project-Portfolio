define(["knockout", "postman"], function (ko, postman) {
    return function (params) {
        isUserAuth = localStorage.getItem("jwt")
        let menuItems = [
            { title: "Dashboard", component: "dashboard" },
            { title: "Title", component: "title" },
            { title: "Person", component: "person" },
        ];

        let currentView
        if (isUserAuth) {
            currentView = ko.observable("dashboard");
        } else {
            currentView = ko.observable("login-user");
        }

        let changeContent = menuItem => {
            console.log(menuItem);
            if (isUserAuth) {
                postman.publish("changeView", menuItem.component);
            } else {
                alert("Please login!")
            }
        };

        let isActive = menuItem => {
            return menuItem.component === currentView() ? "active" : "";
        }

        let titleButton = () => {
            postman.publish("changeView", "title");
        }
        let personButton = () => {
            postman.publish("changeView", "person");
        }

        postman.subscribe("changeView", function (data) {
            currentView(data);
        });

        postman.subscribe("loginUser", login => {
        }, "dashboard");

        return {
            menuItems,
            changeContent,
            isActive,
            titleButton,
            personButton
        }
    };
});