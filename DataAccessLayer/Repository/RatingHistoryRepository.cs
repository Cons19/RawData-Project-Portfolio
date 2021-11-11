using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using DataAccessLayer.Domain;
using DataAccessLayer;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repository
{
    public class RatingHistoryRepository : IRatingHistoryRepository, IDisposable
    {
        private ImdbContext context;

        public RatingHistoryRepository(ImdbContext context)
        {
            this.context = context;
        }
        
        public RatingHistory GetRatingHistory(int id)
        {
            return context.RatingHistory.Find(id);
        }

        public IEnumerable<RatingHistory> GetRatingHistoryByUserId(int userId)
        {
            return context.RatingHistory.ToArray().Where(x => x.UserId == userId);
        }

        public void CreateRatingHistory(RatingHistory ratingHistory)
        {
            context.RatingHistory.Add(ratingHistory);
        }

        public void UpdateRatingHistory(RatingHistory ratingHistory)
        {
            context.Entry(ratingHistory).State = EntityState.Modified;
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