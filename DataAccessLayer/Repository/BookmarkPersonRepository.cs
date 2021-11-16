using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using DataAccessLayer.Domain;

namespace DataAccessLayer.Repository
{
    public class BookmarkPersonRepository : IBookmarkPersonRepository, IDisposable
    {
        private ImdbContext context;

        public BookmarkPersonRepository(ImdbContext context)
        {
            this.context = context;
        }
        public void CreateBookmarkPerson(BookmarkPerson bookmarkPerson)
        {
            context.BookmarkPersons.Add(bookmarkPerson);
        }

        public IList<BookmarkPerson> GetBookmarkPersonsForUser(int userId, QueryString queryString)
        {
            IList<BookmarkPerson> allBookmarkPersons = context.BookmarkPersons.ToList();
            if (queryString != null)
            {
                return allBookmarkPersons.Where(x => x.UserId == userId)
                        .Skip(queryString.Page * queryString.PageSize)
                        .Take(queryString.PageSize)
                        .ToList();
            }
            return allBookmarkPersons.Where(x => x.UserId == userId).ToList();
        }

        public BookmarkPerson GetBookmarkPerson(int id)
        {
            return context.BookmarkPersons.Find(id);
        }

        public IEnumerable<BookmarkPerson> GetBookmarkPersons(QueryString queryString)
        {
            return context.BookmarkPersons.ToList()
                    .Skip(queryString.Page * queryString.PageSize)
                    .Take(queryString.PageSize)
                    .ToList();
        }

        public bool DeleteBookmarkPerson(int id)
        {
            var bookmarkPerson = context.BookmarkPersons.SingleOrDefault(x => x.Id == id);
            if (bookmarkPerson != null)
            {
                context.BookmarkPersons.Remove(bookmarkPerson);
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