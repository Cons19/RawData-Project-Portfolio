using DataAccessLayer.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

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
            string salt = getSalt();
            string hash = getHash(user.Password + salt);
            user.Password = hash;
            user.Salt = salt;
            context.User.Add(user);
        }

        public User GetUser(int userID)
        {
            return context.User.Find(userID);
        }

        public User GetUserByEmail(string email)
        {
            return context.User.Where(x => x.Email == email).FirstOrDefault();
        }

        public void UpdateUser(User user)
        {
            string salt = getSalt();
            string hash = getHash(user.Password + salt);
            user.Password = hash;
            user.Salt = salt;
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

        public User LoginUser(string email, string password)
        {
            var user = GetUserByEmail(email);
            if (user != null)
            {
                string salt = user.Salt;
                string hash = getHash(password + salt);
                if (user.Password == hash)
                {
                    return user;
                }
            }
            return null;
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

        private static string getHash(string text)
        {
            // SHA512 is disposable by inheritance.  
            using (var sha256 = SHA256.Create())
            {
                // Send a sample text to hash.  
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(text));
                // Get the hashed string.  
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }

        private static string getSalt()
        {
            byte[] bytes = new byte[128 / 8];
            using (var keyGenerator = RandomNumberGenerator.Create())
            {
                keyGenerator.GetBytes(bytes);
                return BitConverter.ToString(bytes).Replace("-", "").ToLower();
            }
        }
    }
}
