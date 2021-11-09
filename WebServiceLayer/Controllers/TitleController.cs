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

        public TitleController(ITitleRepository titleRepository, LinkGenerator linkGenerator)
        {
            _titleRepository = titleRepository;
            _linkGenerator = linkGenerator;
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
            var titles = _titleRepository.SearchText(id, searchText);
            if (titles == null)
            {
                return NotFound();
            }
            return Ok(titles.Select(x => GetTitleViewModel(x)));
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
    }
}
