define(["knockout", "postman", "searchService", "userService"], function (ko, postman, ss, us) {
    let isTokenExpired = true;
    const jwt = localStorage.getItem("jwt");

    if (jwt) {
        isTokenExpired = !(JSON.parse(atob(jwt.split('.')[1])).exp < Date.now());
    }

    let currentView;
    let currentViewParams = ko.observable();

    if (!isTokenExpired) {
        currentView = ko.observable("dashboard");
    } else {
        document.getElementsByTagName("nav")[0].style.display = "none";
        currentView = ko.observable("login-user");
        localStorage.removeItem("jwt");
    }

    let menuItems = [
        { title: "Dashboard", component: "dashboard" },
        { title: "Title", component: "title" },
        { title: "Person", component: "person" },
        { title: "Structured Search", component: "structured-search" },
        { title: "Popular Actors", component: "popular-actors" },
        { title: "Best Match", component: "best-match" }
    ];

    let userItems = [
        { title: "Profile", component: "user-details" },
        { title: "Search History", component: "search-history" },
        { title: "Rating History", component: "rating-history" },
        { title: "Logout"}
    ];

    let changeContent = menuItem => {
        postman.publish("changeView", menuItem.component);
    };

    let changeUserContent = menuItem => {
        if (menuItem.title == "Logout") {
            logout();
            return;
        }
        if (menuItem.title == "Profile")
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
    };

    let logout = () => {
        localStorage.removeItem("jwt");
        isTokenExpired = true;
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