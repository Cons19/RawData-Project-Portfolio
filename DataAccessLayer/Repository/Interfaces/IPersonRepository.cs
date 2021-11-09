
using System;
using System.Collections.Generic;
using DataAccessLayer.Domain;

namespace DataAccessLayer.Repository
{
    public interface IPersonRepository : IDisposable
    {
        IEnumerable<Person> GetPersons();
        Person GetPerson(string id);
    }
}