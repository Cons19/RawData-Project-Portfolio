define([], () => {
    let subscribers = [];

    let subscribe = (event, callback) => {
        let subscriber = { event, callback };

        if (!subscribers.find(x => x.target === target && x.event === event))
            subscribers.push(subscriber);

        console.log(subscriber);
    };

    let publish = (event, data) => {

        subscribers.forEach(x => {
            if (x.event === event)
                x.callback(data);
        });
    };

    return {
        subscribe,
        publish
    }

});