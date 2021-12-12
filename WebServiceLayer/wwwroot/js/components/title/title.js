define(["knockout", "titleService", "ratingHistoryService", "bookmarkService", "postman"], function (ko, ts, rhs, bs, postman ) {
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

        ts.getTitles((json) => {
            setBookmarkTitleHearts(json);
        }, url);

        let previousPageButton = () => {
            ts.getTitles((json) => {
                setBookmarkTitleHearts(json);
            }, prev)
        };

        let nextPageButton = () => {
            ts.getTitles((json) => {
                setBookmarkTitleHearts(json);
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

        let bookmark = (data) => {
            let bookmarkTitleBody = { userId: userId, titleId: data.id.trim() };
            bs.createBookmarkTitle(bookmarkTitleBody, bookmarkTitle => {
                alert(`${data.primaryTitle} added to bookmarks!`);
                location.reload();
            });
        }

        let unbookmark = (data) => {
            if (confirm(`Are you sure you want to remove ${data.primaryTitle} from your bookmarks?`)) {
                bs.deleteBookmarkTitle(() => {
                    alert(`${data.primaryTitle} removed from bookmarks!`);
                    location.reload();
                }, `api/bookmark-titles/${data.bookmarkTitleId}`);
            }
        }

        function setBookmarkTitleHearts(json) {
            bs.getBookmarkTitle((bookmarkTitles) => {
                json.items.forEach((title) => {
                    bookmarkTitles.forEach(bookmarkTitle => {
                        if (title.id === bookmarkTitle.titleId) {
                            title.bookmarked = true;
                            title.bookmarkTitleId = bookmarkTitle.id;
                        } else if (title.id !== bookmarkTitle.titleId && title.bookmarked !== true) {
                            title.bookmarked = false;
                        }
                    });
                });

                titles(json.items);
                prev = json.prev;
                cur = json.cur;
                next = json.next;
            }, `api/bookmark-titles/user/${userId}`);
        }

        return {
            titles,
            nextPageButton,
            previousPageButton,
            details,
            updateRate,
            mouseOutRate,
            mouseOverRate,
            bookmark,
            unbookmark
        }
    };
});