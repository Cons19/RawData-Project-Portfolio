define(["knockout", "personService", "bookmarkService", "postman"], function (ko, ps, bs, postman) {
    return function (params) {
        let url;
        const jwt = atob(localStorage.getItem("jwt").split('.')[1])

        if (params) {
            url = params.cur;
        } else
        {
            url = "api/persons";
        }

        let persons = ko.observableArray([]);
        let prev, cur, next;

        ps.getPersons(json => {
            setBookmarkPersonHearts(json);
        }, url);


        let previousPageButton = () => {
            ps.getPersons((json) => {
                setBookmarkPersonHearts(json);
            }, prev)
        }

        let nextPageButton = () => {
            ps.getPersons((json) => {
                setBookmarkPersonHearts(json);
            }, next)
        }

        let details = (data) => {
            data.currentPage = cur;
            postman.publish("changeView", "person-details", data);
        }

        let bookmark = (data) => {
            console.log(data)
            let bookmarkPersonBody = { userId: JSON.parse(jwt).id, personId: data.id.trim() };
            bs.createBookmarkPerson(bookmarkPersonBody, bookmarkPerson => {
                alert(`${data.name} added to bookmarks!`);
                location.reload();
            });
        }

        let unbookmark = (data) => {
            if (confirm(`Are you sure you want to remove ${data.name} from your bookmarks?`)) {
                bs.deleteBookmarkPerson(() => {
                    alert(`${data.name} removed from bookmarks!`);
                    location.reload();
                }, `api/bookmark-persons/${data.bookmarkPersonId}`);
            }
        }

        function setBookmarkPersonHearts(json) {
            bs.getBookmarkPerson((bookmarkPersons) => {
                if (bookmarkPersons.status !== 404) {
                    json.items.forEach((person) => {
                        bookmarkPersons.forEach(bookmarkPerson => {
                            if (person.id === bookmarkPerson.personId) {
                                person.bookmarked = true;
                                person.bookmarkPersonId = bookmarkPerson.id;
                            } else if (person.id !== bookmarkPerson.userId && person.bookmarked !== true) {
                                person.bookmarked = false;
                            }
                        });
                    });
                } else {
                    json.items.forEach((person) => {
                        person.bookmarked = false;
                    });
                }
                persons(json.items);
                prev = json.prev;
                cur = json.cur;
                next = json.next;
            }, `api/bookmark-persons/user/${JSON.parse(jwt).id}`);
        }

        mouseOverBookmark = (data, event) => {
            const icon = event.target;

            if (icon.classList.contains("fas")) {
                icon.classList.remove("fas")
                icon.classList.add("far")
                icon.style.color = "black"
            }


            if (icon.classList.contains("far")) {
                icon.classList.remove("far")
                icon.classList.add("fas")
                icon.style.color = "red"
            }
        }

        mouseOutBookmark = (data, event) => {
            const icon = event.target;

            if (icon.classList.contains("fas")) {
                icon.classList.remove("fas")
                icon.classList.add("far")
                icon.style.color = "black"
            }
        }

        return {
            persons,
            nextPageButton,
            previousPageButton,
            details,
            bookmark,
            unbookmark,
            mouseOutBookmark,
            mouseOverBookmark
        }
    };
});