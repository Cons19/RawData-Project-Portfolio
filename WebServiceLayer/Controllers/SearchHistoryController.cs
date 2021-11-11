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

namespace WebServiceLayer.Controllers
{
    [ApiController]
    [Route("api/search-history")]
    public class SearchHistoryController : Controller
    {
        ISearchHistoryRepository _searchHistoryRepository;

        public SearchHistoryController(ISearchHistoryRepository searchHistoryRepository)
        {
            _searchHistoryRepository = searchHistoryRepository;
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
