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
    [Route("api/search-history")]
    public class SearchHistoryController : Controller
    {
        ISearchHistoryRepository _searchHistoryRepository;
        IUserRepository _userRepository;

        public SearchHistoryController(ISearchHistoryRepository searchHistoryRepository, IUserRepository userRepository)
        {
            _searchHistoryRepository = searchHistoryRepository;
            _userRepository = userRepository;
        }

        [HttpGet("user/{userId}")]
        public IActionResult GetSearchHistoryByUserId(int userId)
        {
            var searchHistory = _searchHistoryRepository.GetSearchHistoryByUserId(userId);
            if (searchHistory.Count() == 0)
            {
                return NotFound();
            }

            return Ok(searchHistory);
        }

        [HttpPost]
        public IActionResult CreateSearchHistory(SearchHistoryViewModel model)
        {
            var searchHistory = new SearchHistory
            {
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                UserId = model.UserId,
                SearchText = model.SearchText,
            };

            // check if user with the given id exists
            var userId = searchHistory.UserId;

            if (_userRepository.GetUser(userId) == null)
            {
                return NotFound("User Id does not exists!");
            }

            _searchHistoryRepository.CreateSearchHistory(searchHistory);
            _searchHistoryRepository.Save();

            return Created("", searchHistory);
        }

        [HttpDelete("user/{userId}")]
        public IActionResult DeleteSearchHistory(int userId)
        {
            var isDeleted = _searchHistoryRepository.DeleteSearchHistory(userId);
            _searchHistoryRepository.Save();

            if (isDeleted) return NoContent();
            return NotFound();
        }

        protected override void Dispose(bool disposing)
        {
            _searchHistoryRepository.Dispose();
            base.Dispose(disposing);
        }
    }
}
