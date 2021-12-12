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

namespace WebServiceLayer.Controllers
{
    [Authorization]
    [ApiController]
    [Route("api/bookmark-titles")]
    public class BookmarkTitleController : Controller
    {
        IBookmarkTitleRepository _bookmarkTitleRepository;
        IUserRepository _userRepository;
        ITitleRepository _titleRepository;
        LinkGenerator _linkGenerator;

        public BookmarkTitleController(IBookmarkTitleRepository bookmarkTitleRepository, ITitleRepository titleRepository, IUserRepository userRepository, LinkGenerator linkGenerator)
        {
            _bookmarkTitleRepository = bookmarkTitleRepository;
            _userRepository = userRepository;
            _titleRepository = titleRepository;
            _linkGenerator = linkGenerator;
        }

        [HttpGet]
        public IActionResult GetBookmarkTitles([FromQuery] QueryString queryString)
        {
            var bookmarkTitles = _bookmarkTitleRepository.GetBookmarkTitles(queryString);

            return Ok(bookmarkTitles.Select(x => GetBookmarkTitleViewModel(x)));
        }

        [HttpGet("user/{userId}")]
        public IActionResult GetBookmarkTitleForUser([FromQuery] QueryString queryString, int userId)
        {
            var bookmarkTitles = _bookmarkTitleRepository.GetBookmarkTitlesForUser(userId, queryString);

            if (bookmarkTitles.Count == 0)
            {
                return NotFound();
            }

            return Ok(bookmarkTitles.Select(x => GetBookmarkTitleViewModel(x)));
        }


        [HttpGet("{id}", Name = nameof(GetBookmarkTitle))]
        public IActionResult GetBookmarkTitle(int id)
        {
            var bookmarkTitle = _bookmarkTitleRepository.GetBookmarkTitle(id);

            if (bookmarkTitle == null)
            {
                return NotFound();
            }

            return Ok(GetBookmarkTitleViewModel(bookmarkTitle));
        }

        [HttpPost]
        public IActionResult CreateBookmarkTitle(BookmarkTitle bookmarkTitle)
        {
            // check if user with the given id exists
            var userId = bookmarkTitle.UserId;
            var titleId = bookmarkTitle.TitleId;

            if (_userRepository.GetUser(userId) == null)
            {
                return NotFound("User Id does not exists!");
            }

            // check if the title with the given id exists

            if (_titleRepository.GetTitle(titleId) == null)
            {
                return NotFound("Title Id does not exists!");
            }

            // check if the bookmark already exists
            var checkedBookmarkTitles = _bookmarkTitleRepository.GetBookmarkTitlesForUser(userId, null);

            foreach (BookmarkTitle t in checkedBookmarkTitles)
            {
                if (t.TitleId.Trim() == titleId)
                {
                    return Conflict("Bookmark already exists!");
                }
            }

            _bookmarkTitleRepository.CreateBookmarkTitle(bookmarkTitle);
            _bookmarkTitleRepository.Save();

            return Created("", GetBookmarkTitleViewModel(bookmarkTitle));
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteBookmarkTitle(int id)
        {
            var bookmarkTitle = _bookmarkTitleRepository.GetBookmarkTitle(id);

            if (bookmarkTitle == null)
            {
                return NotFound();
            }

            _bookmarkTitleRepository.DeleteBookmarkTitle(id);
            _bookmarkTitleRepository.Save();

            return NoContent();
        }

        private BookmarkTitleViewModel GetBookmarkTitleViewModel(BookmarkTitle bookmarkTitle)
        {
            return new BookmarkTitleViewModel
            {
                Url = _linkGenerator.GetUriByName(HttpContext, nameof(GetBookmarkTitle), new { bookmarkTitle.Id }),
                Id = bookmarkTitle.Id,
                UserId = bookmarkTitle.UserId,
                TitleId = bookmarkTitle.TitleId
            };
        }
    }
}
