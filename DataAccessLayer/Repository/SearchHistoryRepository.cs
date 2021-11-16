using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using DataAccessLayer.Domain;
using DataAccessLayer;

namespace DataAccessLayer.Repository
{
    public class SearchHistoryRepository : ISearchHistoryRepository, IDisposable
    {
        private ImdbContext context;

        public SearchHistoryRepository(ImdbContext context)
        {
            this.context = context;
        }

        public IEnumerable<SearchHistory> GetSearchHistoryByUserId(int userId, QueryString queryString)
        {
            return context.SearchHistory.ToArray().Where(x => x.UserId == userId)
                    .Skip(queryString.Page * queryString.PageSize)
                    .Take(queryString.PageSize)
                    .ToList();
        }

        public void CreateSearchHistory(SearchHistory searchHistory)
        {
            context.SearchHistory.Add(searchHistory);
        }

        public bool DeleteSearchHistory(int userId)
        {
            IEnumerable<SearchHistory> searchHistory = context.SearchHistory.ToArray().Where(x => x.UserId == userId);
            if (searchHistory == null)
            {
                return false;
            }
            context.SearchHistory.RemoveRange(searchHistory);
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