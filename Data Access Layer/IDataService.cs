using Data_Access_Layer.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_Access_Layer
{
    public interface IDataService
    {
        // user
        User GetUser(int id);
        IList<User> GetUsers();
        User CreateUser(string name, string email, string password);
        bool UpdateUser(int id, string name, string email, string password);
        bool DeleteUser(int id);


    }
}
