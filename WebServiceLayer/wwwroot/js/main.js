/// <reference path="lib/jquery/dist/jquery.min.js" />
/// <reference path="lib/requirejs/text.js" />
/// <reference path="lib/knockout/build/output/knockout-latest.debug.js" />

require.config({
    baseUrl: 'js',
    paths: {
        text: "lib/requirejs/text",
        jquery: "lib/jquery/dist/jquery.min",
        knockout: "lib/knockout/build/output/knockout-latest.debug",
        buffer: "https://bundle.run/buffer@6.0.3",

        postman: "services/postman",
        loginService: "services/loginService",
        titleService: "services/titleService",
        personService: "services/personService",
        searchHistoryService: "services/searchHistory",

        dashboard: "components/dashboard/dashboard",
        loginComponent: "components/user/login",
        titleComponent: "components/title/title",
        personComponent: "components/person/person",
        searchHistoryComponent: "components/searchHistory/searchHistory",
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
    ko.components.register("person", {
        viewModel: { require: "components/person/person" },
        template: { require: "text!components/person/person.html" }
    });
    ko.components.register("search-history", {
        viewModel: { require: "components/searchHistory/searchHistory" },
        template: { require: "text!components/searchHistory/searchHistory.html" }
    });
});

require(["knockout", "viewmodel"], function (ko, viewmodel) {
    ko.applyBindings(viewmodel);
});