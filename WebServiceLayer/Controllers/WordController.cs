using DataAccessLayer.Repository.Interfaces;
using WebServiceLayer.Attributes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System.Collections.Generic;
using System.Linq;

namespace WebServiceLayer.Controllers
{
    [Authorization]
    [ApiController]
    [Route("api/words")]
    public class WordController : Controller
    {
        IWordToWordRepository _wordToWordRepository;

        public WordController(IWordToWordRepository wordToWordRepository)
        {
            _wordToWordRepository = wordToWordRepository;
        }

        [HttpGet]
        public IActionResult GetWordToWord([FromQuery(Name = "words")] string[] words)
        {
            var wordsCounter = _wordToWordRepository.GetWordToWord(words);

            if (wordsCounter.Count() == 0)
            {
                return NotFound();
            }

            return Ok(wordsCounter);
        }

        protected override void Dispose(bool disposing)
        {
            _wordToWordRepository.Dispose();
            base.Dispose(disposing);
        }
    }
}
