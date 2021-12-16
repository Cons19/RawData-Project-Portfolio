using DataAccessLayer;
using DataAccessLayer.Domain;
using DataAccessLayer.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebServiceLayer.ViewModels;
using WebServiceLayer.Controllers;
using WebServiceLayer.Attributes;
using DataAccessLayer.Repository.Interfaces;

namespace WebServiceLayer.Controllers
{
    [Authorization]
    [ApiController]
    [Route("api/persons")]
    public class PersonController : Controller
    {
        IPersonRepository _personRepository;
        LinkGenerator _linkGenerator;
        IUserRepository _userRepository;

        public PersonController(IPersonRepository personRepository, LinkGenerator linkGenerator, IUserRepository userRepository)
        {
            _personRepository = personRepository;
            _linkGenerator = linkGenerator;
            _userRepository = userRepository;
        }

        [HttpGet(Name = nameof(GetPersons))]
        public IActionResult GetPersons([FromQuery] QueryString queryString)
        {
            var persons = _personRepository.GetPersons(queryString);

            var items = persons.Select(GetPersonViewModel);

            var result = CreateResultModel(queryString, _personRepository.NumberOfPersons(), items);

            return Ok(result);
        }

        [HttpGet("{id}", Name = nameof(GetPerson))]
        public IActionResult GetPerson(string id)
        {
            var person = _personRepository.GetPerson(id);

            if (person == null)
            {
                return NotFound();
            }

            return Ok(GetPersonViewModel(person));
        }

        [HttpGet("profession/{profession}")]
        public IActionResult FindPersonByProfession([FromQuery] QueryString queryString, string profession)
        {
            var persons = _personRepository.FindPersonByProfession(profession, queryString);

            if (persons.Count() == 0)
            {
                return NotFound();
            }

            return Ok(persons);
        }

        [HttpGet("popular-actors/{title}")]
        public IActionResult PopularActors([FromQuery] QueryString queryString, string title)
        {
            var persons = _personRepository.PopularActors(title, queryString);

            if (persons.Count() == 0)
            {
                return NotFound();
            }

            return Ok(persons);
        }

        [HttpGet("co-actor/{personName}")]
        public IActionResult CoACtor([FromQuery] QueryString queryString, string personName)
        {
            var persons = _personRepository.CoActor(personName, queryString);

            if (persons.Count() == 0)
            {
                return NotFound();
            }

            return Ok(persons);
        }

        private PersonViewModel GetPersonViewModel(Person person)
        {
            return new PersonViewModel
            {
                Url = (_linkGenerator.GetUriByName(HttpContext, nameof(GetPerson), new { person.Id })).Replace("%20", ""),
                Id = person.Id,
                Name = person.Name,
                BirthYear = person.BirthYear,
                DeathYear = person.DeathYear,
                Rating = person.Rating,
            };
        }

        private object CreateResultModel(QueryString queryString, int total, IEnumerable<PersonViewModel> model)
        {
            return new
            {
                total,
                prev = CreateNextPageLink(queryString),
                cur = CreateCurrentPageLink(queryString),
                next = CreateNextPageLink(queryString, total),
                items = model
            };
        }

        private string CreateNextPageLink(QueryString queryString, int total)
        {
            var lastPage = GetLastPage(queryString.PageSize, total);
            return queryString.Page >= lastPage ? null : GetPersonsUrl(queryString.Page + 1, queryString.PageSize);
        }

        private string CreateCurrentPageLink(QueryString queryString)
        {
            return GetPersonsUrl(queryString.Page, queryString.PageSize);
        }

        private string CreateNextPageLink(QueryString queryString)
        {
            return queryString.Page <= 0 ? null : GetPersonsUrl(queryString.Page - 1, queryString.PageSize);
        }

        private string GetPersonsUrl(int page, int pageSize)
        {
            return _linkGenerator.GetUriByName(
                HttpContext,
                nameof(GetPersons),
                new { page, pageSize });
        }

        private static int GetLastPage(int pageSize, int total)
        {
            return (int)Math.Ceiling(total / (double)pageSize) - 1;
        }
    }
}
