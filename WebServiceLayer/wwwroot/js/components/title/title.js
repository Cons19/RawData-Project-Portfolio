define(["knockout", "titleService", "bookmarkService", "postman"], function (ko, ts, bs, postman) {
    return function (params) {
        let url;
        const jwt = atob(localStorage.getItem("jwt").split('.')[1])

        if (params) {
            url = params.cur;
        } else
        {
            url = "api/titles";
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
        }

        let nextPageButton = () => {
            ts.getTitles((json) => {
                setBookmarkTitleHearts(json);
            }, next)
        }
        
        let details = (data) => {
            data.currentPage = cur;
            postman.publish("changeView", "title-details", data);
        }

        let bookmark = (data) => {
            let bookmarkTitleBody = { userId: JSON.parse(jwt).id, titleId: data.id.trim() };
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
            }, `api/bookmark-titles/user/${JSON.parse(jwt).id}`);
        }

        return {
            titles,
            nextPageButton,
            previousPageButton,
            details,
            bookmark,
            unbookmark
        }
    };
});