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
    [Route("api/bookmark-persons")]
    public class BookmarkPersonController : Controller
    {
        IBookmarkPersonRepository _bookmarkPersonRepository;
        IUserRepository _userRepository;
        IPersonRepository _personRepository;
        LinkGenerator _linkGenerator;

        public BookmarkPersonController(IBookmarkPersonRepository bookmarkPersonRepository, IPersonRepository personRepository, IUserRepository userRepository, LinkGenerator linkGenerator)
        {
            _bookmarkPersonRepository = bookmarkPersonRepository;
            _userRepository = userRepository;
            _personRepository = personRepository;
            _linkGenerator = linkGenerator;
        }

        [HttpGet]
        public IActionResult GetBookmarkPersons()
        {
            var bookmarkPersons = _bookmarkPersonRepository.GetBookmarkPersons();

            return Ok(bookmarkPersons.Select(x => GetBookmarkPersonViewModel(x)));
        }

        [HttpGet("user/{userId}")]
        public IActionResult GetBookmarkPersonForUser(int userId)
        {
            var bookmarkPersons = _bookmarkPersonRepository.GetBookmarkPersonsForUser(userId);

            if (bookmarkPersons.Count == 0)
            {
                return NotFound();
            }

            return Ok(bookmarkPersons.Select(x => GetBookmarkPersonViewModel(x)));
        }


        [HttpGet("{id}", Name = nameof(GetBookmarkPerson))]
        public IActionResult GetBookmarkPerson(int id)
        {
            var bookmarkPerson = _bookmarkPersonRepository.GetBookmarkPerson(id);

            if (bookmarkPerson == null)
            {
                return NotFound();
            }

            return Ok(GetBookmarkPersonViewModel(bookmarkPerson));
        }

        [HttpPost]
        public IActionResult CreateBookmarkPerson(BookmarkPerson bookmarkPerson)
        {
            // check if user with the given id exists
            var userId = bookmarkPerson.UserId;
            var personId = bookmarkPerson.PersonId;

            if (_userRepository.GetUser(userId) == null)
            {
                return NotFound("User Id does not exists!");
            }

            // check if the person with the given id exists

            if (_personRepository.GetPerson(personId) == null)
            {
                return NotFound("Person Id does not exists!");
            }

            // check if the bookmark already exists
            var checkedBookmarkPersons = _bookmarkPersonRepository.GetBookmarkPersonsForUser(userId);

            foreach (BookmarkPerson t in checkedBookmarkPersons)
            {
                if (t.PersonId.Trim() == personId)
                {
                    return Conflict("Bookmark already exists!");
                }
            }

            _bookmarkPersonRepository.CreateBookmarkPerson(bookmarkPerson);
            _bookmarkPersonRepository.Save();

            return Created("", GetBookmarkPersonViewModel(bookmarkPerson));
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteBookmarkPerson(int id)
        {
            var bookmarkPerson = _bookmarkPersonRepository.GetBookmarkPerson(id);

            if (bookmarkPerson == null)
            {
                return NotFound();
            }

            _bookmarkPersonRepository.DeleteBookmarkPerson(id);
            _bookmarkPersonRepository.Save();

            return NoContent();
        }

        private BookmarkPersonViewModel GetBookmarkPersonViewModel(BookmarkPerson bookmarkPerson)
        {
            return new BookmarkPersonViewModel
            {
                Url = _linkGenerator.GetUriByName(HttpContext, nameof(GetBookmarkPerson), new { bookmarkPerson.Id }),
                UserId = bookmarkPerson.UserId,
                PersonId = bookmarkPerson.PersonId
            };
        }
    }
}
