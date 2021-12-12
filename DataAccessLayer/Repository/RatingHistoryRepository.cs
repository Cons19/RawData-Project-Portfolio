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

        public IEnumerable<RatingHistory> GetRatingHistoryByUserId(int userId, QueryString? queryString)
        {
            if (queryString != null)
            {
                return context.RatingHistory.ToArray().Where(x => x.UserId == userId)
                        .Skip(queryString.Page * queryString.PageSize)
                        .Take(queryString.PageSize)
                        .ToList();
            }

            return context.RatingHistory.ToArray().Where(x => x.UserId == userId).ToList();
        }

        public void CreateRatingHistory(RatingHistory ratingHistory)
        {
            context.RatingHistory.Add(ratingHistory);
        }

        public void UpdateRatingHistory(RatingHistory ratingHistory)
        {
            context.Entry(ratingHistory).State = EntityState.Modified;
        }

        public bool DeleteRatingHistory(int userId)
        {
            IEnumerable<RatingHistory> ratingHistory = context.RatingHistory.ToArray().Where(x => x.UserId == userId);
            if (!ratingHistory.Any())
            {
                return false;
            }
            context.RatingHistory.RemoveRange(ratingHistory);
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