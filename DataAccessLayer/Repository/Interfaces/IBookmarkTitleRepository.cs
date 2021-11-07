
using System;
using System.Collections.Generic;
using DataAccessLayer.Domain;

namespace DataAccessLayer.Repository
{
    public interface IBookmarkTitleRepository : IDisposable
    {
        IList<BookmarkTitle> GetBookmarkTitlesForUser(int userId);
        BookmarkTitle GetBookmarkTitle(int id);
        IEnumerable<BookmarkTitle> GetBookmarkTitles();
        void CreateBookmarkTitle(BookmarkTitle bookmarkTitle);
        bool DeleteBookmarkTitle(int id);
        void Save();
    }
}