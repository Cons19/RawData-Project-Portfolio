define(["knockout", "postman", "searchService", "userService"], function (ko, postman, ss, us) {
    isUserAuth = localStorage.getItem("jwt");

    let currentView;
    let currentViewParams = ko.observable();

    if (isUserAuth) {
        currentView = ko.observable("dashboard");
    } else {
        document.getElementsByTagName("nav")[0].style.display = "none";
        currentView = ko.observable("login-user");
    }

    let menuItems = [
        { title: "Dashboard", component: "dashboard" },
        { title: "Title", component: "title" },
        { title: "Person", component: "person" },
    ];

    let changeContent = menuItem => {
        if (isUserAuth) {
            postman.publish("changeView", menuItem.component);
        } else {
            alert("Please login!");
        }
    };

    let userItems = [
        { title: "Profile", component: "user-details" },
        { title: "Search History", component: "search-history" },
        { title: "Rating History", component: "rating-history" },
        { title: "Logout"}
    ];

    let changeUserContent = menuItem => {
        if (isUserAuth) {
            if (menuItem.title == "Logout") {
                logout();
                return;
            }
            if (menuItem.title = "Profile")
            {
                let userId = JSON.parse(atob(localStorage.getItem("jwt").split('.')[1])).id;
                let url = "api/users/" + userId;
                us.getUserById(json => {
                    if (json !== undefined) {
                        postman.publish("changeView", "user-details", json);
                    }
                }, url);
            } else {
                postman.publish("changeView", menuItem.component);
            }
        } else {
            alert("Please login!");
        }
    };

    let logout = () => {
        localStorage.removeItem("jwt");
        location.reload();
    }

    this.searchTextValue = ko.observable();

    let searchText = () => {
        let text = this.searchTextValue();
        const userId = JSON.parse(atob(localStorage.getItem("jwt").split('.')[1])).id;
        const url = `api/titles/search/${text}/user/${userId}`;

        ss.getSearchString((json) => {
            postman.publish("changeView", "search-result", json);
        }, url);
    }

    let isActive = menuItem => {
        return menuItem.component === currentView() ? "active" : "";
    }

    postman.subscribe("changeView", function (data, params) {
        currentView(data);
        currentViewParams(params);
    });

    return {
        menuItems,
        userItems,
        changeContent,
        changeUserContent,
        isActive,
        currentView,
        currentViewParams,
        logout,
        searchText
    }
});