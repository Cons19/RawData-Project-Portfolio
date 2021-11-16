
using System;
using System.Collections.Generic;
using DataAccessLayer.Domain;

namespace DataAccessLayer.Repository
{
    public interface IPersonRepository : IDisposable
    {
        IEnumerable<Person> GetPersons(QueryString queryString);
        Person GetPerson(string id);
        IEnumerable<FindPersonByProfession> FindPersonByProfession(string profession, QueryString queryString);
        IEnumerable<PopularActors> PopularActors(string title, QueryString queryString);
        public IEnumerable<CoActor> CoActor(string personId, QueryString queryString);
        int NumberOfPersons();
    }
}