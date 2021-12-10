/// <reference path="lib/jquery/dist/jquery.min.js" />
/// <reference path="lib/requirejs/text.js" />
/// <reference path="lib/knockout/build/output/knockout-latest.debug.js" />

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
        searchHistoryService: "services/searchHistory",
        searchService: "services/searchResult",
        
        dashboard: "components/dashboard/dashboard",
        loginComponent: "components/user/login",
        titleComponent: "components/title/title",
        tileDetailsComponent: "components/title/details/details",
        postman: "services/postman",
        personComponent: "components/person/person",
        searchHistoryComponent: "components/search-history/search-history",
        searchResultComponent: "components/search-result/search-result",
    }
});

// component registration
require(['knockout'], (ko) => {
    ko.components.register("dashboard", {
        viewModel: { require: "components/dashboard/dashboard" },
        template: { require: "text!components/dashboard/dashboard.html" }
    });
    ko.components.register("login-user", {
        viewModel: { require: "components/user/login" },
        template: { require: "text!components/user/login.html" }
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
});

require(["knockout", "viewmodel"], function (ko, viewmodel) {
    ko.applyBindings(viewmodel);
});