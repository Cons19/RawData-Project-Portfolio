using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using DataAccessLayer.Domain;

namespace DataAccessLayer.Repository
{
    public class PersonRepository : IPersonRepository, IDisposable
    {
        private ImdbContext context;

        public PersonRepository(ImdbContext context)
        {
            this.context = context;
        }

        public IEnumerable<Person> GetPersons()
        {
            return context.Persons.ToList();
        }

        public Person GetPerson(string id)
        {
            return context.Persons.Find(id);
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