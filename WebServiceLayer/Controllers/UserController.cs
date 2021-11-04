using DataAccessLayer;
using DataAccessLayer.Domain;
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
    [Route("api/users")]
    public class UserController : Controller
    {
        IDataService _dataService;
        LinkGenerator _linkGenerator;

        public UserController(IDataService dataService, LinkGenerator linkGenerator)
        {
            _dataService = dataService;
            _linkGenerator = linkGenerator;
        }

        [HttpGet]
        public IActionResult GetUsers()
        {
            var users = _dataService.GetUsers();

            return Ok(users.Select(x => GetUserViewModel(x)));
        }

        private UserViewModel GetUserViewModel(User user)
        {
            return new UserViewModel
            {
                Url = _linkGenerator.GetUriByName(HttpContext, nameof(GetUsers), new { user.Id }),
                Name = user.Name,
                Email = user.Email,
                Password = user.Password
            };
        }
    }
}
