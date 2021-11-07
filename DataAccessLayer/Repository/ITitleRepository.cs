
using System;
using DataAccessLayer.Domain;

namespace DataAccessLayer.Repository
{
    public interface ITitleRepository : IDisposable
    {
        Title GetTitle(string id);
    }
}