
using System;
using System.Collections.Generic;
using DataAccessLayer.Domain;

namespace DataAccessLayer.Repository
{
    public interface ITitleRepository : IDisposable
    {
        IEnumerable<Title> GetTitles();
        Title GetTitle(string id);
        public IEnumerable<Title> SearchText(int id, string searchText);
    }
}