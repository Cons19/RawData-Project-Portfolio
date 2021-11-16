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
        User GetUser(int id);
        void CreateUser(User user);
        User LoginUser(string email, string password);
        void UpdateUser(User user);
        bool DeleteUser(int id);
        void Save();
    }
}
