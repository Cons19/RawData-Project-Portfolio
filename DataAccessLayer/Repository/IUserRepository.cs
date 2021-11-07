using DataAccessLayer.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repository
{
    public interface IUserRepository : IDisposable
    {
        // user
        User GetUser(int id);
        IEnumerable<User> GetUsers();
        void CreateUser(User user);
        void UpdateUser(User user);
        bool DeleteUser(int id);
        void Save();

    }
}
