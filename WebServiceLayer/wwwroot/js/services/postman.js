define([], () => {
    let subscribers = [];

    let subscribe = (event, callback, target) => {
        let subscriber = { event, callback, target };

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