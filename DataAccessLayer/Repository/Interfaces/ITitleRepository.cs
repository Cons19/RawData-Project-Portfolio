using System;
using System.Collections.Generic;
using DataAccessLayer.Domain;
using DataAccessLayer.Domain.Functions;

namespace DataAccessLayer.Repository
{
    public interface ITitleRepository : IDisposable
    {
        IEnumerable<Title> GetTitles(QueryString queryString);
        Title GetTitle(string id);
        public IEnumerable<SearchTitle> SearchText(int id, string searchText, QueryString queryString);
        public IEnumerable<StructuredStringSearch> StructuredStringSearch(int userId, string? title, string? plot, string? inputCharacter, string? personName, QueryString queryString);
        public IEnumerable<ExactMatch> ExactMatch(string word1, string word2, string word3, string? category, QueryString queryString);
        public Exception RateTitle(int userId, string titleId, int rate);
        public IEnumerable<BestMatch> BestMatch(string word1, string word2, string word3, QueryString queryString);
        public IEnumerable<SimilarTitle> SimilarTitle(string title_id, QueryString queryString);
        int NumberOfTitles();
    }
}