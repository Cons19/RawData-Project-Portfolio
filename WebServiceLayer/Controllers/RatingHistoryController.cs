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
using WebServiceLayer.Attributes;

namespace WebServiceLayer.Controllers
{
    [Authorization]
    [ApiController]
    [Route("api/rating-history")]
    public class RatingHistoryController : Controller
    {
        IRatingHistoryRepository _ratingHistoryRepository;
        IUserRepository _userRepository;
        ITitleRepository _titleRepository;

        public RatingHistoryController(IRatingHistoryRepository ratingHistoryRepository, ITitleRepository titleRepository, IUserRepository userRepository)
        {
            _ratingHistoryRepository = ratingHistoryRepository;
            _userRepository = userRepository;
            _titleRepository = titleRepository;
        }

        [HttpGet("user/{userId}")]
        public IActionResult GetRatingHistoryByUserId([FromQuery] QueryString queryString, int userId)
        {
            var ratingHistory = _ratingHistoryRepository.GetRatingHistoryByUserId(userId, queryString);
            if (ratingHistory.Count() == 0)
            {
                return NotFound();
            }

            return Ok(ratingHistory);
        }

        [HttpPost]
        public IActionResult CreateRatingHistory(RatingHistoryViewModel model)
        {
            var ratingHistory = new RatingHistory
            {
                UserId = model.UserId,
                TitleId = model.TitleId,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                Rate = model.Rate
            };

            // check if user with the given id exists
            var userId = ratingHistory.UserId;
            var titleId = ratingHistory.TitleId;

            if (_userRepository.GetUser(userId) == null)
            {
                return NotFound("User Id does not exists!");
            }

            // check if the title with the given id exists
            if (_titleRepository.GetTitle(titleId) == null)
            {
                return NotFound("Title Id does not exists!");
            }

            // check if the rating history already exists
            var checkedRatingHistory = _ratingHistoryRepository.GetRatingHistoryByUserId(userId, null);

            foreach (RatingHistory rh in checkedRatingHistory)
            {
                if (rh.TitleId.Trim() == titleId)
                {
                    return Conflict("Rating History already exists!");
                }
            }

            _ratingHistoryRepository.CreateRatingHistory(ratingHistory);
            _ratingHistoryRepository.Save();

            return Created("", GetRatingHistoryViewModel(ratingHistory));
        }

        [HttpPut("{id}", Name = nameof(UpdateRatingHistory))]
        public IActionResult UpdateRatingHistory(int id, RatingHistoryViewModel model)
        {
            var ratingHistory = _ratingHistoryRepository.GetRatingHistory(id);

            if (ratingHistory == null)
            {
                return NotFound();
            }

            ratingHistory.Rate = model.Rate;

            _ratingHistoryRepository.UpdateRatingHistory(ratingHistory);
            _ratingHistoryRepository.Save();
            return Ok(GetRatingHistoryViewModel(ratingHistory));
        }

        private RatingHistoryViewModel GetRatingHistoryViewModel(RatingHistory ratingHistory)
        {
            return new RatingHistoryViewModel
            {
                UserId = ratingHistory.UserId,
                TitleId = ratingHistory.TitleId,
                Rate = ratingHistory.Rate
            };
        }

        protected override void Dispose(bool disposing)
        {
            _ratingHistoryRepository.Dispose();
            base.Dispose(disposing);
        }
    }
}
