define(["knockout", "postman"], function (ko, postman) {
    isUserAuth = localStorage.getItem("jwt")

    let currentView
    if (isUserAuth) {
        currentView = ko.observable("dashboard");
    } else {
        document.getElementsByTagName("nav")[0].style.display = "none"
        currentView = ko.observable("login-user");
    }

    let menuItems = [
        { title: "Dashboard", component: "dashboard" },
        { title: "Title", component: "title" },
        { title: "Person", component: "person" },
    ];

    let changeContent = menuItem => {
        if (isUserAuth) {
            console.log(menuItem.component)
            postman.publish("changeView", menuItem.component);
        } else {
            alert("Please login!")
        }
    };

    let logout = () => {
        localStorage.removeItem("jwt");
        location.reload();
    }

    let isActive = menuItem => {
        return menuItem.component === currentView() ? "active" : "";
    }

    postman.subscribe("changeView", function (data) {
        currentView(data);
    });

    return {
        menuItems,
        changeContent,
        isActive,
        currentView,
        logout,
    }
});