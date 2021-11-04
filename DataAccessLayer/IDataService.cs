using DataAccessLayer.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public interface IDataService
    {
        // User
        User GetUser(int id);
        IList<User> GetUsers();
        User CreateUser(string name, string email, string password);
        bool UpdateUser(int id, string name, string email, string password);
        bool DeleteUser(int id);
        
        // BookmarkTitle
        IList<BookmarkTitle> GetBookmarkTitlesForUser(int userId);
        IList<BookmarkTitle> GetBookmarkTitles();
        BookmarkTitle CreateBookmarkTitle(int userId, string titleId);
        bool DeleteBookmarkTitle(int userId, string titleId);
    }
}
