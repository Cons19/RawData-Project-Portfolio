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

namespace WebServiceLayer.Controllers
{
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

        [HttpGet]
        public IActionResult GetPersons()
        {
            var persons = _personRepository.GetPersons();

            return Ok(persons.Select(x => GetPersonViewModel(x)));
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
        public IActionResult FindPersonByProfession(string profession)
        {
            var persons = _personRepository.FindPersonByProfession(profession);

            if (persons.Count() == 0)
            {
                return NotFound();
            }

            return Ok(persons);
        }

        [HttpGet("popular-actors/{title}")]
        public IActionResult PopularActors(string title)
        {
            var persons = _personRepository.PopularActors(title);

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
                Name = person.Name,
                BirthYear = person.BirthYear,
                DeathYear = person.DeathYear,
                Rating = person.Rating,
            };
        }

        private FindPersonByProfessionViewModel GetFindPersonByProfessionViewModel(FindPersonByProfession person)
        {
            return new FindPersonByProfessionViewModel
            {
                Name = person.Name
            };
        }
    }
}
