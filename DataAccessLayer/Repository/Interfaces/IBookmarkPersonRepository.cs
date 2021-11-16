
using System;
using System.Collections.Generic;
using DataAccessLayer.Domain;

namespace DataAccessLayer.Repository
{
    public interface IBookmarkPersonRepository : IDisposable
    {
        IList<BookmarkPerson> GetBookmarkPersonsForUser(int userId, QueryString queryString);
        BookmarkPerson GetBookmarkPerson(int id);
        IEnumerable<BookmarkPerson> GetBookmarkPersons(QueryString queryString);
        void CreateBookmarkPerson(BookmarkPerson bookmarkPerson);
        bool DeleteBookmarkPerson(int id);
        void Save();
    }
}