using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using DataAccessLayer.Domain;
using DataAccessLayer.Domain.Functions;

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
            return context.Titles.ToList().Take(50);
        }

        public Title GetTitle(string id)
        {
            return context.Titles.Find(id);
        }

        public IEnumerable<SearchTitle> SearchText(int id, string searchText)
        {
            return context.SearchTitle.FromSqlInterpolated($"SELECT * FROM search_string({id},{searchText})").ToList();
        }

        public IEnumerable<StructuredStringSearch> StructuredStringSearch(int userId, string? title, string? plot, string? inputCharacter, string? personName)
        {
            return context.StructuredStringSearch.FromSqlInterpolated($"SELECT * FROM structured_string_search({title},{plot},{inputCharacter},{personName},{userId})").ToList();
        }

        public IEnumerable<ExactMatch> ExactMatch(string word1, string word2, string word3, string? category)
        {   
            return context.ExactMatch.FromSqlInterpolated($"SELECT * FROM exact_match({word1},{word2},{word3},{category})").ToList();           
        }
        public IEnumerable<BestMatch> BestMatch(string? word1, string? word2, string? word3)
        {
            return context.BestMatch.FromSqlInterpolated($"SELECT * FROM bestmatch({word1},{word2},{word3}) LIMIT 50").ToList();
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