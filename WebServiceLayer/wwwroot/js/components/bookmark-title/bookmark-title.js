define(["knockout", "bookmarkService", "titleService", "postman"], function (ko, bs, ts, postman) {
    return function (params) {
        const jwt = atob(localStorage.getItem("jwt").split('.')[1])
        let url = "api/bookmark-titles/user/" + JSON.parse(jwt).id;

        let bookmarkTitles = ko.observableArray([]);
        bs.getBookmarkTitle(json => {
            json.forEach((bookmark) => {
                let getTitleByIdURL = "api/titles/" + bookmark.titleId.trim()
                ts.getTitles(title => {
                    bookmark.primaryTitle = title.primaryTitle;
                }, getTitleByIdURL);
            });
            bookmarkTitles(json);
        }, url);

        let deleteRow = (data) => {
            console.log(data);
            if (confirm(`Are you sure you want to remove ${data.primaryTitle} from your bookmarks?`)) {
                bs.deleteBookmarkTitle(() => {
                    alert(`${data.primaryTitle} removed from bookmarks!`);
                    location.reload();
                }, `api/bookmark-titles/${data.id}`);
            }
        }

        return {
            bookmarkTitles,
            deleteRow
        }
    };
});