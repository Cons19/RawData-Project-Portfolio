using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using DataAccessLayer.Domain;

namespace DataAccessLayer.Repository
{
    public class TitleRepository : ITitleRepository, IDisposable
    {
        private ImdbContext context;

        public TitleRepository(ImdbContext context)
        {
            this.context = context;
        }

        public IEnumerable<Title> GetTitles()
        {
            return context.Titles.ToList();
        }

        public Title GetTitle(string id)
        {
            return context.Titles.Find(id);
        }

        public IEnumerable<Title> SearchText(int id, string searchText)
        {
            return context.Titles.FromSqlInterpolated($"select * from search_string({id},{searchText})").ToList();
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