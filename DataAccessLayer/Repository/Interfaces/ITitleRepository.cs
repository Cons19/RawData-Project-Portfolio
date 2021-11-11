using System;
using System.Collections.Generic;
using DataAccessLayer.Domain;
using DataAccessLayer.Domain.Functions;

namespace DataAccessLayer.Repository
{
    public interface ITitleRepository : IDisposable
    {
        IEnumerable<Title> GetTitles();
        Title GetTitle(string id);
        public IEnumerable<SearchTitle> SearchText(int id, string searchText);
        public IEnumerable<StructuredStringSearch> StructuredStringSearch(int userId, string? title, string? plot, string? inputCharacter, string? personName);
        public IEnumerable<ExactMatch> ExactMatch(string word1, string word2, string word3, string? category);
        public Exception RateTitle(int userId, string titleId, int rate);
        public IEnumerable<BestMatch> BestMatch(string word1, string word2, string word3);
    }
}