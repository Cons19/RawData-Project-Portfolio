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

        public IEnumerable<SearchTitle> SearchText(int id, string searchText)
        {
            return context.SearchTitle.FromSqlInterpolated($"select * from search_string({id},{searchText})").ToList();
        }

        public IEnumerable<StructuredStringSearch> StructuredStringSearch(int userId, string? title, string? plot, string? inputCharacter, string? personName)
        {
            return context.StructuredStringSearch.FromSqlInterpolated($"select * from structured_string_search({title},{plot},{inputCharacter}, {personName},{userId})").ToList();
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