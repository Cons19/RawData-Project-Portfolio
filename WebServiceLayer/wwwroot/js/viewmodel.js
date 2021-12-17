define(["knockout", "postman", "searchService", "userService"], function (ko, postman, ss, us) {
    let isTokenExpired = true;
    const jwt = localStorage.getItem("jwt");
    let url = `api/titles/search/`;

    if (jwt) {
        isTokenExpired = !(JSON.parse(atob(jwt.split('.')[1])).exp < Date.now());
    }

    let currentView;
    let currentViewParams = ko.observable();

    if (!isTokenExpired) {
        currentView = ko.observable("title");
    } else {
        document.getElementsByTagName("nav")[0].style.display = "none";
        currentView = ko.observable("login-user");
        localStorage.removeItem("jwt");
    }

    let menuItems = [
        { title: "Title", component: "title" },
        { title: "Person", component: "person" }
    ];

    let menuItemsSearch = [
        { title: "Popular Actors", component: "popular-actors" },
        { title: "Structured Search", component: "structured-search" },
        { title: "Best Match", component: "best-match" },
        { title: "Find Person by Profession", component: "find-person-by-profession" },
        { title: "Exact Match", component: "exact-match" },
        { title: "Word to words", component: "word-to-words" }
    ];

    let userItems = [
        { title: "Profile", component: "user-details" },
        { title: "Search History", component: "search-history" },
        { title: "Rating History", component: "rating-history" },
        { title: "Bookmark Titles", component: "bookmark-title" },
        { title: "Bookmark Persons", component: "bookmark-person" },
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

        if (typeof text !== "undefined" && text !== "") {
            url = url + text + "/user/" + userId;
        }

        if (typeof text !== "undefined" && text !== "" && text.match('[=!@#$%^*?"{}|<>;]')) {
            alert("The search text can't contain invalid characters!");
        } else {
            ss.getSearchString((json) => {
                if (json.status !== 404) {
                    postman.publish("changeView", "search-result", json);
                } else {
                    alert("This search result does not exist!");
                }
            }, url);
            url = `api/titles/search/`;
        }
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
        menuItemsSearch,
        changeContent,
        changeUserContent,
        isActive,
        currentView,
        currentViewParams,
        logout,
        searchText
    }
});