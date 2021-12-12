require.config({
    baseUrl: 'js',
    paths: {
        text: "lib/requirejs/text",
        jquery: "lib/jquery/dist/jquery.min",
        knockout: "lib/knockout/build/output/knockout-latest.debug",

        postman: "services/postman",
        loginService: "services/loginService",
        titleService: "services/titleService",
        personService: "services/personService",
        searchHistoryService: "services/searchHistoryService",
        searchService: "services/searchResultService",
        ratingHistoryService: "services/ratingHistoryService",
        userService: "services/userService",
        functionService: "services/functionService",

        loginComponent: "components/login/login",
        bookmarkService: "services/bookmarkService",
        
        titleComponent: "components/title/title",
        tileDetailsComponent: "components/title/details/details",
        personComponent: "components/person/person",
        searchHistoryComponent: "components/search-history/search-history",
        searchResultComponent: "components/search-result/search-result",
        personDetailsComponent: "components/person-details/person-details",
        registerComponent: "components/register/register",
        userDetailsComponent: "components/user-details/user-details",
        structuredSearchComponent: "components/structured-search/structured-search",
        popularActorsComponent: "components/popular-actors/popular-actors",
        bestMatchComponent: "components/best-match/best-match",
        exactMatchComponent: "components/best-match/exact-match",
        ratingHistoryComponent: "components/rating-history/rating-history"
        bookmarkTitleComponent: "components/bookmark-title/bookmark-title",
        bookmarkPersonComponent: "components/bookmark-person/bookmark-person",
    }
});

// component registration
require(['knockout'], (ko) => {
    ko.components.register("login-user", {
        viewModel: { require: "components/login/login" },
        template: { require: "text!components/login/login.html" }
    });
    ko.components.register("title", {
        viewModel: { require: "components/title/title" },
        template: { require: "text!components/title/title.html" }
    });
    ko.components.register("title-details", {
        viewModel: { require: "components/title-details/title-details" },
        template: { require: "text!components/title-details/title-details.html" }
    });
    ko.components.register("person", {
        viewModel: { require: "components/person/person" },
        template: { require: "text!components/person/person.html" }
    });
    ko.components.register("search-history", {
        viewModel: { require: "components/search-history/search-history" },
        template: { require: "text!components/search-history/search-history.html" }
    });
    ko.components.register("search-result", {
        viewModel: { require: "components/search-result/search-result" },
        template: { require: "text!components/search-result/search-result.html" }
    });
    ko.components.register("person-details", {
        viewModel: { require: "components/person-details/person-details" },
        template: { require: "text!components/person-details/person-details.html" }
    });
    ko.components.register("register", {
        viewModel: { require: "components/register/register" },
        template: { require: "text!components/register/register.html" }
    });
    ko.components.register("user-details", {
        viewModel: { require: "components/user-details/user-details" },
        template: { require: "text!components/user-details/user-details.html" }
    });
    ko.components.register("structured-search", {
        viewModel: { require: "components/structured-search/structured-search" },
        template: { require: "text!components/structured-search/structured-search.html" }
    });
    ko.components.register("popular-actors", {
        viewModel: { require: "components/popular-actors/popular-actors" },
        template: { require: "text!components/popular-actors/popular-actors.html" }
    });
    ko.components.register("best-match", {
        viewModel: { require: "components/best-match/best-match" },
        template: { require: "text!components/best-match/best-match.html" }
    });
    ko.components.register("exact-match", {
        viewModel: { require: "components/best-match/best-match" },
        template: { require: "text!components/best-match/best-match.html" }
    });
    ko.components.register("rating-history", {
        viewModel: { require: "components/rating-history/rating-history" },
        template: { require: "text!components/rating-history/rating-history.html" }
    });
    ko.components.register("bookmark-title", {
        viewModel: { require: "components/bookmark-title/bookmark-title" },
        template: { require: "text!components/bookmark-title/bookmark-title.html" }
    });
    ko.components.register("bookmark-person", {
        viewModel: { require: "components/bookmark-person/bookmark-person" },
        template: { require: "text!components/bookmark-person/bookmark-person.html" }
    });
});

require(["knockout", "viewmodel"], function (ko, viewmodel) {
    ko.applyBindings(viewmodel);
});