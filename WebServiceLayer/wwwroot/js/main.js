﻿/// <reference path="lib/jquery/dist/jquery.min.js" />
/// <reference path="lib/requirejs/text.js" />
/// <reference path="lib/knockout/build/output/knockout-latest.debug.js" />

require.config({
    baseUrl: 'js',
    paths: {
        text: "lib/requirejs/text",
        jquery: "lib/jquery/dist/jquery.min",
        knockout: "lib/knockout/build/output/knockout-latest.debug",
        loginService: "services/loginService",
        loginComponent: "components/user/login",
        bookmarkComponent: "components/bookmarks/bookmarks",
        postman: "services/postman"
        //bootstrap: "lib/bootstrap/dist/css/bootstrap.css",
    }
});

// component registration
require(['knockout'], (ko) => {
    ko.components.register("login-user", {
        viewModel: { require: "components/user/login" },
        template: { require: "text!components/user/login.html" }
    });
    ko.components.register("dashboard", {
        viewModel: { require: "components/dashboard" },
        template: { require: "text!components/dashboard.html" }
    });
    ko.components.register("bookmarks", {
        viewModel: { require: "components/bookmarks/bookmarks" },
        template: { require: "text!components/bookmarks.html" }
    });
});

require(["knockout", "viewmodel"], function (ko, viewmodel) {

    ko.applyBindings(viewmodel);
});