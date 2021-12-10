define(["knockout", "postman"], function (ko, postman) {
    return function (params) {
        let results = ko.observableArray([])
        results(params)
        // postman.subscribe("changeView", function (data, target) {
        //     console.log(target)
        //     console.log(data)
        //     // results(data);
        //     // console.log(results());
        // });

        // console.log(results());

        return {
            results
        }
    };
});