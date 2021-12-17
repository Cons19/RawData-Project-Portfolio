using DataAccessLayer;
using DataAccessLayer.Domain;
using DataAccessLayer.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Web;
using System.Linq;
using System.Threading.Tasks;
using WebServiceLayer.ViewModels;
using WebServiceLayer.Attributes;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace WebServiceLayer.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController : Controller
    {
        IUserRepository _userRepository;
        LinkGenerator _linkGenerator;
        IConfiguration _configuration;

        public UserController(IUserRepository userRepository, LinkGenerator linkGenerator, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _linkGenerator = linkGenerator;
            _configuration = configuration;
        } 

        [Authorization]
        [HttpGet("{id}", Name = nameof(GetUser))]
        public IActionResult GetUser(int id)
        {
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

        [Authorization]
        [HttpPut("{id}", Name = nameof(UpdateUser))]
        public IActionResult UpdateUser(int id, CreateUpdateUserViewModel model)
        {
            var user = _userRepository.GetUser(id);
            var passChanged = false;

            if (user == null)
            {
                return NotFound();
            }

            if (model.Name is not null) {
                user.Name = model.Name;
            }
                
            if (model.Email is not null) {
                user.Email = model.Email;
            }
            
            if (model.Password is not null) {
                if (!model.Password.Equals(user.Password)) {
                    passChanged = true;
                    user.Password = model.Password;
                }
            }            

            _userRepository.UpdateUser(user, passChanged);
            _userRepository.Save();
            return Ok(user);
        }

        [Authorization]
        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int id)
        {
            var isDeleted = _userRepository.DeleteUser(id);
            _userRepository.Save();

            if (isDeleted) 
            {
                return NoContent();
            }
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
                return Unauthorized();
            }

            int.TryParse(_configuration.GetSection("Auth:PasswordSize").Value, out int pwdSize);

            if (pwdSize == 0)
            {
                throw new ArgumentException("No password size");
            }

            string secret = _configuration.GetSection("Auth:Secret").Value;
            if (string.IsNullOrEmpty(secret))
            {
                throw new ArgumentException("No secret");
            }

            var tokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.UTF8.GetBytes(secret);

            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", loggedInUser.Id.ToString()) }),
                Expires = DateTime.Now.AddHours(24),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var securityToken = tokenHandler.CreateToken(tokenDescription);
            var token = tokenHandler.WriteToken(securityToken);

            return Ok(new { loggedInUser, token });
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
