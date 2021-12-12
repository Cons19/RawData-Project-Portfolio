using System;
using System.Collections.Generic;
using DataAccessLayer.Domain;

namespace DataAccessLayer.Repository
{
    public interface IRatingHistoryRepository : IDisposable
    {
        RatingHistory GetRatingHistory(int id);
        IEnumerable<RatingHistory> GetRatingHistoryByUserId(int userId, QueryString queryString);
        void CreateRatingHistory(RatingHistory ratingHistory);
        void UpdateRatingHistory(RatingHistory ratingHistory);
        bool DeleteRatingHistory(int userId);
        void Save();
    }
}