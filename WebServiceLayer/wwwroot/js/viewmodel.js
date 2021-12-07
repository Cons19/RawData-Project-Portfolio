define(["knockout", "postman"], function (ko, postman) {

    let menuItems = [
        { title: "Login", component: "login-user" },
        { title: "Dashboard", component: "dashboard" },
        { title: "Bookmarks", component: "bookmarks" }
    ];

    let currentView = ko.observable(menuItems[0].component);

    let changeContent = menuItem => {
        console.log(menuItem);
        currentView(menuItem.component)
    };

    let isActive = menuItem => {
        return menuItem.component === currentView() ? "active" : "";
    }

    postman.subscribe("changeView", function (data) {
        currentView(data);
    });


    postman.subscribe("changeView", function (data) {
        currentView(data);
    });

    return {
        currentView,
        menuItems,
        changeContent,
        isActive
    }
});