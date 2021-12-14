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

        public IEnumerable<Title> GetTitles(QueryString queryString)
        {
            var result = context.Titles.AsEnumerable();

            result = result
                .Skip(queryString.Page * queryString.PageSize)
                .Take(queryString.PageSize);

            return result.ToList();
        }

        public Title GetTitle(string id)
        {
            return context.Titles.Find(id);
        }

        public object[] SearchText(int id, string searchText, QueryString queryString)
        {
            var items = context.SearchTitle.FromSqlInterpolated($"SELECT * FROM search_string({id},{searchText})");
            var total = items.Count();
        
            var searchedItems = items.Skip(queryString.Page * queryString.PageSize)
                                .Take(queryString.PageSize)
                                .ToList();

            return new object[] { searchedItems, total };
        }
        public int GetSearchTextCount(int id, string searchText)
        {
            return context.SearchTitle.FromSqlInterpolated($"SELECT * FROM search_string({id},{searchText})").Count();
        }

        public IEnumerable<StructuredStringSearch> StructuredStringSearch(int userId, string? title, string? plot, string? inputCharacter, string? personName, QueryString queryString)
        {
            return context.StructuredStringSearch.FromSqlInterpolated($"SELECT * FROM structured_string_search({title},{plot},{inputCharacter},{personName},{userId})")
                    .Skip(queryString.Page * queryString.PageSize)
                    .Take(queryString.PageSize)
                    .ToList();
        }

        public IEnumerable<ExactMatch> ExactMatch(string word1, string word2, string word3, string? category, QueryString queryString)
        {
            return context.ExactMatch.FromSqlInterpolated($"select * from exact_match({word1},{word2},{word3},{category})")
                    .Skip(queryString.Page * queryString.PageSize)
                    .Take(queryString.PageSize)
                    .ToList();
        }

        public Exception RateTitle(int userId, string titleId, int rating)
        {
            try
            {
                context.Database.ExecuteSqlRaw("select rate({0},{1},{2})", userId, titleId, rating);
            }
            catch (Exception e)
            {
                return e;
            }

            return null;
        }

        public IEnumerable<BestMatch> BestMatch(string? word1, string? word2, string? word3, QueryString queryString)
        {
            return context.BestMatch.FromSqlInterpolated($"SELECT * FROM bestmatch({word1},{word2},{word3})")
                    .Skip(queryString.Page * queryString.PageSize)
                    .Take(queryString.PageSize)
                    .ToList();
        }

        public IEnumerable<SimilarTitle> SimilarTitle(string title_id, QueryString queryString)
        {
            return context.SimilarTitle.FromSqlInterpolated($"SELECT * FROM similar_movies({title_id})")
                    .Skip(queryString.Page * queryString.PageSize)
                    .Take(queryString.PageSize)
                    .ToList();
        }
        public int NumberOfSearchText()
        {
            return context.SearchTitle.Count();
        }

        public int NumberOfTitles()
        {
            return context.Titles.Count();
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