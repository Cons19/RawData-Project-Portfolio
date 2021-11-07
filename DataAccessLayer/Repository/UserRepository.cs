using DataAccessLayer.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repository
{
    public class UserRepository : IUserRepository, IDisposable
    {
        private ImdbContext context;

        public UserRepository(ImdbContext context)
        {
            this.context = context;
        }

        public void CreateUser(User user)
        {
            context.User.Add(user);
        }

        public User GetUser(int userID)
        {
            return context.User.Find(userID);
        }

        public IEnumerable<User> GetUsers()
        {
            return context.User.ToList();
        }

        public void UpdateUser(User user)
        {
            context.Entry(user).State = EntityState.Modified;

        }

        public bool DeleteUser(int userID)
        {
            User user = context.User.Find(userID);
            if (user == null)
            {
                return false;
            }
            context.User.Remove(user);
            return true;
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
