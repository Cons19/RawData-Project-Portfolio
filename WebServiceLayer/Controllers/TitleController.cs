using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebServiceLayer.ViewModels;
using WebServiceLayer.Controllers;
using WebServiceLayer.Attributes;
using DataAccessLayer;
using DataAccessLayer.Domain;
using DataAccessLayer.Domain.Functions;
using DataAccessLayer.Repository;
using DataAccessLayer.Repository.Interfaces;

namespace WebServiceLayer.Controllers
{
    [Authorization]
    [ApiController]
    [Route("api/titles")]
    public class TitleController : Controller
    {
        ITitleRepository _titleRepository;
        LinkGenerator _linkGenerator;
        IUserRepository _userRepository;
        IUpdatePersonsRatingRepository _updatePersonsRatingRepository;

        public TitleController(ITitleRepository titleRepository, LinkGenerator linkGenerator, IUserRepository userRepository, IUpdatePersonsRatingRepository updatePersonsRatingRepository)
        {
            _titleRepository = titleRepository;
            _linkGenerator = linkGenerator;
            _userRepository = userRepository;
            _updatePersonsRatingRepository = updatePersonsRatingRepository;
        }

        [HttpGet(Name = nameof(GetTitles))]
        public IActionResult GetTitles([FromQuery] QueryString queryString)
        {
            var titles = _titleRepository.GetTitles(queryString);

            var items = titles.Select(GetTitleViewModel);

            var result = CreateResultModel(queryString, _titleRepository.NumberOfTitles(), items);

            return Ok(result);
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

        [HttpGet("search/{searchText}/user/{id}", Name = nameof(SearchText))]
        public IActionResult SearchText([FromQuery] QueryString queryString, int id, string searchText)
        {
            // check if user with the given id existss
            if (_userRepository.GetUser(id) == null)
            {
                return NotFound("User Id does not exists!");
            }

            var resultQuery = _titleRepository.SearchText(id, searchText, queryString);
            var titles = (IEnumerable<SearchTitle>)resultQuery[0];
            var items = titles.Select(GetSearchTitleViewModel);
            var count = (int)resultQuery[1];

            if (titles.Count() == 0)
            {
                return NotFound();
            }

            // Console.WriteLine(GetSearchTitlesUrl(0, 10, searchText, id));
            var result = CreateSearchResultModel(queryString, count, items, searchText, id);

            return Ok(result);
        }

        // Query is string-based. So an example would be api/titles/structured-search?plot=see&personName=Mads+miKkelsen&userId=1
        // The space is encoded as a +
        // If a filter in the request is mispelled or not written at all, it will be ignored
        // All parameters excepted the userId are defaulted to null
        // The userId is the only mandatory parameter, the rest are optional
        [HttpGet("structured-search")]
        public IActionResult StructuredStringSearch([FromQuery] QueryString queryString, int userId, string? title = null, string? plot = null, string? inputCharacter = null, string? personName = null)
        {
            // check if user with the given id exists
            if (_userRepository.GetUser(userId) == null)
            {
                return NotFound("User Id does not exists!");
            }

            var titles = _titleRepository.StructuredStringSearch(userId, title, plot, inputCharacter, personName, queryString);

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
        public IActionResult ExactMatch([FromQuery] QueryString queryString, string word1, string word2, string word3, string? category = "")
        {
            var titles = _titleRepository.ExactMatch(word1, word2, word3, category, queryString);

            if (titles.Count() == 0)
            {
                return NotFound();
            }

            return Ok(titles);
        }

        // Query is string-based. So an example would be api/titles/best-match?word1=apple&word2=mads&word3=mikkelsen
        // If a filter in the request is mispelled or not written at all, it will be ignored
        [HttpGet("best-match")]
        public IActionResult BestMatch([FromQuery] QueryString queryString, string? word1 = "", string? word2 = "", string? word3 = "")
        {
            var titles = _titleRepository.BestMatch(word1, word2, word3, queryString);

            if (titles.Count() == 0)
            {
                return NotFound();
            }

            return Ok(titles);
        }

        [HttpGet("similar-title/{id}")]
        public IActionResult SimilarTitle([FromQuery] QueryString queryString, string id)
        {
            var titles = _titleRepository.SimilarTitle(id, queryString);

            if (titles.Count() == 0)
            {
                return NotFound();
            }

            return Ok(titles);
        }

        [HttpPost("rate-title")]
        public IActionResult RateTitle(RateTitleViewModel model)
        {
            var userId = model.UserId;
            var titleId = model.TitleId;

            if (_userRepository.GetUser(userId) == null)
            {
                return NotFound("User Id does not exists!");
            }

            // check if the person with the given id exists
            if (_titleRepository.GetTitle(titleId) == null)
            {
                return NotFound("Title Id does not exists!");
            }

            _updatePersonsRatingRepository.UpdatePersonsRating();

            var result = _titleRepository.RateTitle(model.UserId, model.TitleId, model.Rating);
            return Ok(result);
        }

        private TitleViewModel GetTitleViewModel(Title title)
        {
            return new TitleViewModel
            {
                // Some IDs have a space encoded as %20. Here we remove the encoding from the URL
                Url = (_linkGenerator.GetUriByName(HttpContext, nameof(GetTitle), new { title.Id })).Replace("%20", ""),
                Id = title.Id,
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
                PrimaryTitle = title.PrimaryTitle,
            };
        }

        private object CreateResultModel(QueryString queryString, int total, IEnumerable<TitleViewModel> model)
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

        private object CreateSearchResultModel(QueryString queryString, int total, IEnumerable<SearchTitleViewModel> model, string searchText, int userId)
        {
            return new
            {
                total,
                prev = CreateNextPageSearchTextLink(queryString, searchText, userId),
                cur = CreateCurrentPageSearchTextLink(queryString, searchText, userId),
                next = CreateNextPageSearchTextLink(queryString, total, searchText, userId),
                items = model
            };
        }


        private string CreateNextPageLink(QueryString queryString, int total)
        {
            var lastPage = GetLastPage(queryString.PageSize, total);
            return queryString.Page >= lastPage ? null : GetTitlesUrl(queryString.Page + 1, queryString.PageSize);
        }

        private string CreateCurrentPageLink(QueryString queryString)
        {
            return GetTitlesUrl(queryString.Page, queryString.PageSize);
        }

        private string CreateNextPageLink(QueryString queryString)
        {
            return queryString.Page <= 0 ? null : GetTitlesUrl(queryString.Page - 1, queryString.PageSize);
        }

        private string GetTitlesUrl(int page, int pageSize)
        {
            return _linkGenerator.GetUriByName(
                HttpContext,
                nameof(GetTitles),
                new { page, pageSize });
        }
        private static int GetLastPage(int pageSize, int total)
        {
            return (int)Math.Ceiling(total / (double)pageSize) - 1;
        }

        private string GetSearchTitlesUrl(int page, int pageSize, string searchText, int userId)
        {
            return $"http://localhost:5000/api/titles/search/{searchText}/user/{userId}?page={page}&pageSize={pageSize}";
        }

        private string CreateNextPageSearchTextLink(QueryString queryString, int total, string searchText, int userId)
        {
            var lastPage = GetLastPage(queryString.PageSize, total);
            return queryString.Page >= lastPage ? null : GetSearchTitlesUrl(queryString.Page + 1, queryString.PageSize, searchText, userId);
        }

        private string CreateCurrentPageSearchTextLink(QueryString queryString, string searchText, int userId)
        {
            return GetSearchTitlesUrl(queryString.Page, queryString.PageSize, searchText, userId);
        }

        private string CreateNextPageSearchTextLink(QueryString queryString, string searchText, int userId)
        {
            return queryString.Page <= 0 ? null : GetSearchTitlesUrl(queryString.Page - 1, queryString.PageSize, searchText, userId);
        }
    }
}
