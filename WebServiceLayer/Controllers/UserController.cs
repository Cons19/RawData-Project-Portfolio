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
    [Route("api/users")]
    public class UserController : Controller
    {
        IUserRepository _userRepository;
        LinkGenerator _linkGenerator;

        public UserController(IUserRepository userRepository, LinkGenerator linkGenerator)
        {
            _userRepository = userRepository;
            _linkGenerator = linkGenerator;
        }

        [HttpGet]
        public IActionResult GetUsers()
        {
            var users = _userRepository.GetUsers();

            return Ok(users.Select(x => GetUserViewModel(x)));
        }

        [HttpGet("{id}", Name = nameof(GetUser))]
        public IActionResult GetUser(int id)
        {
            Console.WriteLine(id);
            var user = _userRepository.GetUser(id);

            if (user == null)
            {
                return NotFound();
            }

            UserViewModel viewModelUser = GetUserViewModel(user);

            return Ok(viewModelUser);
        }

        [HttpPost]
        public IActionResult CreateUser(CreateUpdateUserViewModel model)
        {
            var user = new User
            {
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                Name = model.Name,
                Email = model.Email,
                Password = model.Password
            };

            _userRepository.CreateUser(user);
            _userRepository.Save();

            return Created("", user);
        }

        [HttpPut("{id}", Name = nameof(UpdateUser))]
        public IActionResult UpdateUser(int id, CreateUpdateUserViewModel model)
        {
            var user = _userRepository.GetUser(id);

            if (user == null)
            {
                return NotFound();
            }

            user.Name = model.Name;
            user.Email = model.Email;
            user.Password = model.Password;

            _userRepository.UpdateUser(user);
            _userRepository.Save();
            return Ok(user);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int id)
        {
            var isDeleted = _userRepository.DeleteUser(id);
            _userRepository.Save();

            if (isDeleted) return NoContent();
            return NotFound();
        }

        [HttpPost("login")]
        public IActionResult LoginUser(LoginUserViewModel model)
        {
            var user = new User
            {
                Email = model.Email,
                Password = model.Password
            };

            var loggedInUser = _userRepository.LoginUser(model.Email, model.Password);

            if (loggedInUser == null)
            {
                return NotFound();
            }
            return Ok(loggedInUser);
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

        protected override void Dispose(bool disposing)
        {
            _userRepository.Dispose();
            base.Dispose(disposing);
        }
    }
}
