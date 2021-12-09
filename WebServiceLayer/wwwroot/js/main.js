/// <reference path="lib/jquery/dist/jquery.min.js" />
/// <reference path="lib/requirejs/text.js" />
/// <reference path="lib/knockout/build/output/knockout-latest.debug.js" />

require.config({
    baseUrl: 'js',
    paths: {
        text: "lib/requirejs/text",
        jquery: "lib/jquery/dist/jquery.min",
        knockout: "lib/knockout/build/output/knockout-latest.debug",
        loginService: "services/loginService",
        titleService: "services/titleService",
        loginComponent: "components/user/login",
        dashboard: "components/dashboard/dashboard",
        titleComponent: "components/title/title",
        tileDetailsComponent: "components/title/details/details",
        postman: "services/postman",
        buffer: "lib/buffer/index.js"
    }
});

// component registration
require(['knockout'], (ko) => {
    ko.components.register("login-user", {
        viewModel: { require: "components/user/login" },
        template: { require: "text!components/user/login.html" }
    });
    ko.components.register("dashboard", {
        viewModel: { require: "components/dashboard/dashboard" },
        template: { require: "text!components/dashboard/dashboard.html" }
    });
    ko.components.register("title", {
        viewModel: { require: "components/title/title" },
        template: { require: "text!components/title/title.html" }
    });
    ko.components.register("title-details", {
        viewModel: { require: "components/title-details/title-details" },
        template: { require: "text!components/title-details/title-details.html" }
    });
});

require(["knockout", "viewmodel"], function (ko, viewmodel) {

    ko.applyBindings(viewmodel);
});