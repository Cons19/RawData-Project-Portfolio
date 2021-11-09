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
    }
}
