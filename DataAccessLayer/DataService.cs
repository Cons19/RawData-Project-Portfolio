using DataAccessLayer.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class DataService : IDataService
    {
        public User CreateUser(string name, string email, string password)
        {
            var ctx = new ImdbContext();
            User user = new User();
            DateTime timeStamp = DateTime.Now;
            user.Name = name;
            user.Email = email;
            user.Password = password;
            user.CreatedAt = timeStamp;
            user.UpdatedAt = timeStamp;
            ctx.Users.Add(user);
            ctx.SaveChanges();
            return user;
        }

        public User GetUser(int id)
        {
            var ctx = new ImdbContext();
            return ctx.Users.Find(id);
        }

        public IList<User> GetUsers()
        {
            var ctx = new ImdbContext();
            return ctx.Users.ToList();
        }

        public bool UpdateUser(int id, string name, string email, string password)
        {
            var ctx = new ImdbContext();
            DateTime timeStamp = DateTime.Now;
            var newUser = ctx.Users.SingleOrDefault(x => x.Id == id);
            if (newUser != null)
            {
                newUser.Name = name;
                newUser.Email = email;
                newUser.Password = password;
                newUser.UpdatedAt = timeStamp;
                return ctx.SaveChanges() > 0;
            }
            return false;
        }

        public bool DeleteUser(int id)
        {
            var ctx = new ImdbContext();
            var user = ctx.Users.SingleOrDefault(x => x.Id == id);
            if (user != null)
            {
                ctx.Users.Remove(user);
                return ctx.SaveChanges() > 0;
            }
            return false;
        }

        public BookmarkTitle CreateBookmarkTitle(int userId, string titleId)
        {
            var ctx = new ImdbContext();
            BookmarkTitle bookmarkTitle = new BookmarkTitle();
            bookmarkTitle.UserId = userId;
            bookmarkTitle.TitleId = titleId;
            ctx.BookmarkTitles.Add(bookmarkTitle);
            ctx.SaveChanges();
            return bookmarkTitle;
        }

        public IList<BookmarkTitle> GetBookmarkTitlesForUser(int userId)
        {
            var ctx = new ImdbContext();
            IList<BookmarkTitle> allBookmarkTitles = ctx.BookmarkTitles.ToList();
            return allBookmarkTitles.Where(x => x.UserId == userId).ToList();
        }

        public BookmarkTitle GetBookmarkTitle(int id)
        {
            var ctx = new ImdbContext();
            return ctx.BookmarkTitles.Find(id);
        }

        public IList<BookmarkTitle> GetBookmarkTitles()
        {
            var ctx = new ImdbContext();
            return ctx.BookmarkTitles.ToList();
        }

        public bool DeleteBookmarkTitle(int id)
        {
            var ctx = new ImdbContext();
            var bookmarkTitle = ctx.BookmarkTitles.SingleOrDefault(x => x.Id == id);
            if (bookmarkTitle != null)
            {
                ctx.BookmarkTitles.Remove(bookmarkTitle);
                return ctx.SaveChanges() > 0;
            }
            return false;
        }
    }
}
