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

    }
}
