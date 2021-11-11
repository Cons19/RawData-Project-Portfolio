using DataAccessLayer.Domain.Functions;
using DataAccessLayer.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repository
{
    public class WordToWordRepository : IWordToWordRepository, IDisposable
    {
        private ImdbContext context;

        public WordToWordRepository(ImdbContext context)
        {
            this.context = context;
        }

        public IEnumerable<WordToWord> GetWordToWord(string[] words)
        {
            var query = "SELECT * FROM word_to_word(";

            foreach (string word in words)
            {
                query += "'" + word + "',";
            }

            query = query.Remove(query.Length - 1);

            query += ")";

            return context.WordToWord.FromSqlRaw(query).ToList();
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
