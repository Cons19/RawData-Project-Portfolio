using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
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
        public Title GetTitle(string id)
        {
            return context.Titles.Find(id);
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