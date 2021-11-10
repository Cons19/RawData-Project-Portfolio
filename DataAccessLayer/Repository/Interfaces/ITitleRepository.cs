
using System;
using System.Collections.Generic;
using DataAccessLayer.Domain;

namespace DataAccessLayer.Repository
{
    public interface ITitleRepository : IDisposable
    {
        IEnumerable<Title> GetTitles();
        Title GetTitle(string id);
        public IEnumerable<SearchTitle> SearchText(int id, string searchText);
        public IEnumerable<StructuredStringSearch> StructuredStringSearch(int userId, string? title, string? plot, string? inputCharacter, string? personName);
    }
}