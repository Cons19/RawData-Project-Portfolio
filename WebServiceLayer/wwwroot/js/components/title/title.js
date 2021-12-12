define(["knockout", "titleService", "ratingHistoryService", "postman"], function (ko, ts, rhs, postman ) {
    return function (params) {
        const userId = JSON.parse(atob(localStorage.getItem("jwt").split('.')[1])).id;
        let url;

        if (params) {
            url = params.cur;
        } else
        {
            url = `api/titles?userId=${userId}`
        }

        let titles = ko.observableArray([]);
        let prev, cur, next;

        ts.getTitles(json => {
            titles(json.items);
            prev = json.prev;
            cur = json.cur;
            next = json.next;
        }, url);

        let previousPageButton = () => {
            ts.getTitles((json) => {
                titles(json.items)
                prev = json.prev;
                cur = json.cur;
                next = json.next;
            }, prev)
        };

        let nextPageButton = () => {
            ts.getTitles((json) => {
                titles(json.items)
                prev = json.prev;
                cur = json.cur;
                next = json.next;
            }, next)
        };
        
        let details = (data) => {
            data.currentPage = cur;
            postman.publish("changeView", "title-details", data);
        };

		updateRate = (data, event) => {
            payload = {
                "userId": userId,
                "titleId": data.id.trim(),
                "rate": event.target.parentElement.dataset.rating
            }
            rhs.updateRatingHistory(() => {
                location.reload();
            }, "api/rating-history", payload)
		};

        mouseOverRate = (data, event) => {
            starNumber = event.target.parentElement.dataset.rating
            stars = event.target.parentElement.parentElement.querySelectorAll('span')

            stars.forEach(star => {
                icon = star.querySelector("i")

                if (icon.classList.contains("fas")) {
                    icon.classList.remove("fas")
                    icon.classList.add("far")
                    icon.style.color = "black"
                }
            });

            for (i = 0; i < starNumber; i++) {
                icon = stars[i].querySelector("i")

                if (icon.classList.contains("far")) {
                    icon.classList.remove("far")
                    icon.classList.add("fas")
                    icon.style.color = "yellow"
                }
            }
        }

        mouseOutRate = (data, event) => {
            starNumber = event.target.parentElement.dataset.rating
            stars = event.target.parentElement.parentElement.querySelectorAll('span')
            for (i = 0; i < starNumber; i++) {
                icon = stars[i].querySelector("i")

                if (icon.classList.contains("fas")) {
                    icon.classList.remove("fas")
                    icon.classList.add("far")
                    icon.style.color = "black"
                }
            }
        }

        return {
            titles,
            nextPageButton,
            previousPageButton,
            details,
            updateRate,
            mouseOutRate,
            mouseOverRate,
        }
    };
});