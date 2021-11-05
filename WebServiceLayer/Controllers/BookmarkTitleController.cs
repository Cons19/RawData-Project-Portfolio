using DataAccessLayer;
using DataAccessLayer.Domain;
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
    [Route("api/bookmarktitles")]
    public class BookmarkTitleController : Controller
    {
        IDataService _dataService;
        LinkGenerator _linkGenerator;

        public BookmarkTitleController(IDataService dataService, LinkGenerator linkGenerator)
        {
            _dataService = dataService;
            _linkGenerator = linkGenerator;
        }

        [HttpGet]
        public IActionResult GetBookmarkTitles()
        {
            var bookmarkTitles = _dataService.GetBookmarkTitles();

            return Ok(bookmarkTitles.Select(x => GetBookmarkTitleViewModel(x)));
        }

        [HttpGet("user/{userId}")]
        public IActionResult GetBookmarkTitleForUser(int userId)
        {
            var bookmarkTitles = _dataService.GetBookmarkTitlesForUser(userId);

            if (bookmarkTitles.Count == 0)
            {
                return NotFound();
            }

            return Ok(bookmarkTitles.Select(x => GetBookmarkTitleViewModel(x)));
        }


        [HttpGet("{id}", Name = nameof(GetBookmarkTitle))]
        public IActionResult GetBookmarkTitle(int id)
        {
            var bookmarkTitle = _dataService.GetBookmarkTitle(id);

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
            if (_dataService.GetUser(userId) == null)
            {
                return NotFound(userId);
            }

            // check if the title with the given id exists
            /*
            if (_dataService.GetTitle(titleId) == null)
            {
                return NotFound(_dataService.GetTitle(titleId));
            }*/

            // check if the bookmark already exists
            var checkedBookmarkTitles = _dataService.GetBookmarkTitlesForUser(userId);
            
            foreach (BookmarkTitle t in checkedBookmarkTitles)
            {
                if (t.TitleId == titleId)
                {
                    return Conflict();
                }
            }
            
            var createdBookmarkTitle = _dataService.CreateBookmarkTitle(bookmarkTitle.UserId, bookmarkTitle.TitleId);

            return Created("", GetBookmarkTitleViewModel(createdBookmarkTitle)); 
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteBookmarkTitle(int id)
        {
            var bookmarkTitle = _dataService.GetBookmarkTitle(id);

            if (bookmarkTitle == null)
            {
                return NotFound();
            }

            _dataService.DeleteBookmarkTitle(id);

            return Ok(GetBookmarkTitleViewModel(bookmarkTitle));
        }

        private BookmarkTitleViewModel GetBookmarkTitleViewModel(BookmarkTitle bookmarkTitle)
        {
            return new BookmarkTitleViewModel
            {
                //UserUrl = _linkGenerator.GetUriByName(HttpContext, nameof(GetUser(bookmarkTitle.UserId)), new { bookmarkTitle.UserId }),
                //TitleUrl = _linkGenerator.GetUriByName(HttpContext, nameof(GetUser(bookmarkTitle.Title)), new { bookmarkTitle.TitleId }),
                Url = _linkGenerator.GetUriByName(HttpContext, nameof(GetBookmarkTitle), new { bookmarkTitle.Id }),
                UserId = bookmarkTitle.UserId,
                TitleId = bookmarkTitle.TitleId
            };
        }
    }
}
