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

        [HttpGet("{id}", Name = nameof(GetBookmarkTitleForUser))]
        public IActionResult GetBookmarkTitleForUser(int id)
        {
            var bookmarkTitles = _dataService.GetBookmarkTitlesForUser(id);

            if (bookmarkTitles.Count == 0)
            {
                return NotFound();
            }

            return Ok(bookmarkTitles.Select(x => GetBookmarkTitleViewModel(x)));
        }

        [HttpPost]
        public IActionResult CreateBookmarkTitle(BookmarkTitle bookmarkTitle)
        {
            BookmarkTitleViewModel newBookmarkTitle = new BookmarkTitleViewModel();
            var createdBookmarkTitle = _dataService.CreateBookmarkTitle(bookmarkTitle.UserId, bookmarkTitle.TitleId);
            newBookmarkTitle.UserId = createdBookmarkTitle.UserId;
            newBookmarkTitle.TitleId = createdBookmarkTitle.TitleId;
            return Created("", newBookmarkTitle);
        }

        [HttpDelete("{userId}/{titleId}")]
        public IActionResult DeleteBookmarkTitle(int userId, string titleId)
        {
            var bookmarkTitleByUser = _dataService.GetBookmarkTitlesForUser(userId);
            BookmarkTitleViewModel newBookmarkTitle = new BookmarkTitleViewModel();
            foreach (BookmarkTitle t in bookmarkTitleByUser)
            {
                if (t.TitleId.Trim() == titleId)
                {
                    _dataService.DeleteBookmarkTitle(userId, titleId);
                    newBookmarkTitle.UserId = userId;
                    newBookmarkTitle.TitleId = titleId;
                    return Ok(newBookmarkTitle);
                }
            }
            return NotFound();
        }

        private BookmarkTitleViewModel GetBookmarkTitleViewModel(BookmarkTitle bookmarkTitle)
        {
            return new BookmarkTitleViewModel
            {
                //UserUrl = _linkGenerator.GetUriByName(HttpContext, nameof(GetUser(bookmarkTitle.UserId)), new { bookmarkTitle.UserId }),
                //TitleUrl = _linkGenerator.GetUriByName(HttpContext, nameof(GetUser(bookmarkTitle.Title)), new { bookmarkTitle.TitleId }),
                UserId = bookmarkTitle.UserId,
                TitleId = bookmarkTitle.TitleId
            };
        }
    }
}
