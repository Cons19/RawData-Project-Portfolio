using System;
using System.Collections.Generic;
using DataAccessLayer.Domain;

namespace DataAccessLayer.Repository
{
    public interface ISearchHistoryRepository : IDisposable
    {
        IEnumerable<SearchHistory> GetSearchHistoryByUserId(int userId, QueryString queryString);
        void CreateSearchHistory(SearchHistory searchHistory);
        bool DeleteSearchHistory(int searchHistoryId);
        void Save();
    }
}