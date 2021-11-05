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
    [Route("api/titles")]
    public class TitleController : Controller
    {
        IDataService _dataService;
        LinkGenerator _linkGenerator;

        public TitleController(IDataService dataService, LinkGenerator linkGenerator)
        {
            _dataService = dataService;
            _linkGenerator = linkGenerator;
        }

        [HttpGet("{id}", Name = nameof(GetTitle))]
        public IActionResult GetTitle(string id)
        {
            var title = _dataService.GetTitle(id);

            if (title == null)
            {
                return NotFound();
            }

            return Ok(GetTitleViewModel(title));
        }
        private TitleViewModel GetTitleViewModel(Title title)
        {
            return new TitleViewModel
            {
                Url = _linkGenerator.GetUriByName(HttpContext, nameof(GetTitle), new { title.Id }),
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
