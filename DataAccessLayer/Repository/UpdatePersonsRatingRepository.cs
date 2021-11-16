using DataAccessLayer.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repository
{
    public class UpdatePersonsRatingRepository : IUpdatePersonsRatingRepository
    {
        private ImdbContext context;

        public UpdatePersonsRatingRepository(ImdbContext context)
        {
            this.context = context;
        }

        public void UpdatePersonsRating()
        {
            context.Database.ExecuteSqlRaw("SELECT * FROM update_persons_rating()");
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
