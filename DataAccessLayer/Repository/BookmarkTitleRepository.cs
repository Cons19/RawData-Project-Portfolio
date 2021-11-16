using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using DataAccessLayer.Domain;

namespace DataAccessLayer.Repository
{
    public class BookmarkTitleRepository : IBookmarkTitleRepository, IDisposable
    {
        private ImdbContext context;

        public BookmarkTitleRepository(ImdbContext context)
        {
            this.context = context;
        }
        public void CreateBookmarkTitle(BookmarkTitle bookmarkTitle)
        {
            context.BookmarkTitles.Add(bookmarkTitle);
        }

        public IList<BookmarkTitle> GetBookmarkTitlesForUser(int userId, QueryString queryString)
        {
            IList<BookmarkTitle> allBookmarkTitles = context.BookmarkTitles.ToList();

            if (queryString != null)
            {
                return allBookmarkTitles.Where(x => x.UserId == userId)
                        .Skip(queryString.Page * queryString.PageSize)
                        .Take(queryString.PageSize)
                        .ToList();
            }

            return allBookmarkTitles.Where(x => x.UserId == userId).ToList();
        }

        public BookmarkTitle GetBookmarkTitle(int id)
        {
            return context.BookmarkTitles.Find(id);
        }

        public IEnumerable<BookmarkTitle> GetBookmarkTitles(QueryString queryString)
        {
            return context.BookmarkTitles
                    .Skip(queryString.Page * queryString.PageSize)
                    .Take(queryString.PageSize)
                    .ToList();
        }

        public bool DeleteBookmarkTitle(int id)
        {
            var bookmarkTitle = context.BookmarkTitles.SingleOrDefault(x => x.Id == id);
            if (bookmarkTitle != null)
            {
                context.BookmarkTitles.Remove(bookmarkTitle);
                return context.SaveChanges() > 0;
            }
            return false;
        }

        public void Save()
        {
            context.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}