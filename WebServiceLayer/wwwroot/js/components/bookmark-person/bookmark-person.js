define(["knockout", "bookmarkService", "personService", "postman"], function (ko, bs, ps, postman) {
    return function (params) {
        const jwt = atob(localStorage.getItem("jwt").split('.')[1])
        let url = "api/bookmark-persons/user/" + JSON.parse(jwt).id;

        let bookmarkPersons = ko.observableArray([]);
        bs.getBookmarkPerson(json => {
            json.forEach((bookmark) => {
                let getPersonByIdURL = "api/persons/" + bookmark.personId.trim()
                ps.getPersons(person => {
                    bookmark.personName = person.name;
                }, getPersonByIdURL);
            });
            bookmarkPersons(json);
        }, url);

        let deleteRow = (data) => {
            console.log(data);
            if (confirm(`Are you sure you want to remove ${data.personName} from your bookmarks?`)) {
                bs.deleteBookmarkPerson(() => {
                    alert(`${data.personName} removed from bookmarks!`);
                    location.reload();
                }, `api/bookmark-persons/${data.id}`);
            }
        }

        return {
            bookmarkPersons,
            deleteRow
        }
    };
});