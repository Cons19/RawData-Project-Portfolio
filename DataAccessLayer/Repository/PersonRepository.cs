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

        public IEnumerable<Person> GetPersons(QueryString queryString)
        {
            var result = context.Persons.AsEnumerable();

            result = result
                .Skip(queryString.Page * queryString.PageSize)
                .Take(queryString.PageSize);

            return result.ToList();
        }

        public Person GetPerson(string id)
        {
            return context.Persons.Find(id);
        }

        public IEnumerable<FindPersonByProfession> FindPersonByProfession(string profession)
        {
            return context.FindPersonByProfession.FromSqlInterpolated($"SELECT * FROM find_persons_by_profession({profession}) LIMIT 50").ToList();
        }

        public IEnumerable<PopularActors> PopularActors(string title)
        {
            return context.PopularActors.FromSqlInterpolated($"SELECT * FROM top_actors_by_movie({title}) LIMIT 50").ToList();
        }

        public int NumberOfPersons()
        {
            return context.Persons.Count();
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