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
using DataAccessLayer.Domain.Functions;

namespace WebServiceLayer.Controllers
{
    [ApiController]
    [Route("api/titles")]
    public class TitleController : Controller
    {
        ITitleRepository _titleRepository;
        LinkGenerator _linkGenerator;
        IUserRepository _userRepository;

        public TitleController(ITitleRepository titleRepository, LinkGenerator linkGenerator, IUserRepository userRepository)
        {
            _titleRepository = titleRepository;
            _linkGenerator = linkGenerator;
            _userRepository = userRepository;
        }

        [HttpGet]
        public IActionResult GetTiles()
        {
            var users = _titleRepository.GetTitles();

            return Ok(users.Select(x => GetTitleViewModel(x)));
        }

        [HttpGet("{id}", Name = nameof(GetTitle))]
        public IActionResult GetTitle(string id)
        {
            var title = _titleRepository.GetTitle(id);

            if (title == null)
            {
                return NotFound();
            }

            return Ok(GetTitleViewModel(title));
        }

        [HttpGet("search/{searchText}/user/{id}")]
        public IActionResult SearchText(int id, string searchText)
        {
            // check if user with the given id exists
            if (_userRepository.GetUser(id) == null)
            {
                return NotFound("User Id does not exists!");
            }

            var titles = _titleRepository.SearchText(id, searchText);

            if (titles.Count() == 0)
            {
                return NotFound();
            }

            return Ok(titles);
        }

        // Query is string-based. So an example would be api/titles/structured-search?plot=see&personName=Mads+miKkelsen&userId=1
        // The space is encoded as a +
        // If a filter in the request is mispelled or not written at all, it will be ignored
        // All parameters excepted the userId are defaulted to null
        // The userId is the only mandatory parameter, the rest are optional
        [HttpGet("structured-search")]
        public IActionResult StructuredStringSearch(int userId, string? title = null, string? plot = null, string? inputCharacter = null, string? personName = null)
        {
            // check if user with the given id exists
            if (_userRepository.GetUser(userId) == null)
            {
                return NotFound("User Id does not exists!");
            }

            var titles = _titleRepository.StructuredStringSearch(userId, title, plot, inputCharacter, personName);

            if (titles.Count() == 0)
            {
                return NotFound();
            }

            return Ok(titles);
        }

        // Query is string-based. So an example would be api/titles/exact-match?word1=apple&word2=mads&word3=mikkelsen
        // If a filter in the request is mispelled or not written at all, it will be ignored
        // Category is defaulted to empty string
        // The 3 words are mandatory parameters, the category is optional
        [HttpGet("exact-match")]
        public IActionResult ExactMatch(string word1, string word2, string word3, string? category = "")
        {
            var titles = _titleRepository.ExactMatch(word1, word2, word3, category);

            if (titles.Count() == 0)
            {
                return NotFound();
            }

            return Ok(titles);
        }

        private TitleViewModel GetTitleViewModel(Title title)
        {
            return new TitleViewModel
            {
                // Some IDs have a space encoded as %20. Here we remove the encoding from the URL
                Url = (_linkGenerator.GetUriByName(HttpContext, nameof(GetTitle), new { title.Id })).Replace("%20", ""),
                TitleType = title.TitleType,
                PrimaryTitle = title.PrimaryTitle,
                OriginalTitle = title.OriginalTitle,
                IsAdult = title.IsAdult,
                StartYear = title.StartYear,
                EndYear = title.EndYear,
                RunTimeMinutes = title.RunTimeMinutes,
                Poster = title.Poster,
                Awards = title.Awards,
                Plot = title.Plot
            };
        }

        private SearchTitleViewModel GetSearchTitleViewModel(SearchTitle title)
        {
            return new SearchTitleViewModel
            {
                Id = title.Id,
                PrimaryTitle = title.PrimaryTitle
            };
        }

        private StructuredStringSearchViewModel GetStructuredStringSearchViewModel (StructuredStringSearch text)
        {
            return new StructuredStringSearchViewModel
            {
                Id = text.Id,
                PrimaryTitle = text.PrimaryTitle,
                Description = text.PrimaryTitle
            };
        }

        private ExactMatchViewModel GetExactMatchViewModel(ExactMatch text)
        {
            return new ExactMatchViewModel
            {
                Id = text.Id,
                Title = text.Title
            };
        }
    }
}
