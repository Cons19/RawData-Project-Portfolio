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

        [HttpGet("{id}", Name = nameof(GetUser))]
        public IActionResult GetUser(int id)
        {
            var user = _dataService.GetUser(id);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [HttpPost]
        public IActionResult CreateUser(CreateUpdateUserViewModel model)
        {
            var user = new User
            {
                Name = model.Name,
                Email = model.Email,
                Password = model.Password
            };

            var newUser = _dataService.CreateUser(user.Name, user.Email, user.Password);

            return Created("", newUser);

        }

        [HttpPut("{id}", Name = nameof(UpdateUser))]
        public IActionResult UpdateUser(int id, CreateUpdateUserViewModel model)
        {
            var user = _dataService.GetUser(id);

            if (user == null)
            {
                return NotFound();
            }

            var updatedUser = new User
            {
                Id = id,
                Name = model.Name,
                Email = model.Email,
                Password = model.Password
            };

            _dataService.UpdateUser(id, updatedUser.Name, updatedUser.Email, updatedUser.Password);

            return Ok(updatedUser);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteCategory(int id)
        {
            var user = _dataService.GetUser(id);

            if (user == null)
            {
                return NotFound();
            }

            _dataService.DeleteUser(id);

            return Ok(user);
        }

        private UserViewModel GetUserViewModel(User user)
        {
            return new UserViewModel
            {
                Url = _linkGenerator.GetUriByName(HttpContext, nameof(GetUser), new { user.Id }),
                Name = user.Name,
                Email = user.Email,
                Password = user.Password
            };
        }
    }
}
