using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer;
using DataAccessLayer.Domain;
using DataAccessLayer.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Moq;
using WebServiceLayer.Controllers;
using WebServiceLayer.ViewModels;
using Xunit;
using AutoMapper;

namespace Testing
{
    public class UserControllerTest
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<LinkGenerator> _linkGeneratorMock;
        private readonly Mock<IConfiguration> _configurationMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly ImdbContext _imdbContext;
        private readonly UserRepository _userRepository;

        public UserControllerTest()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _linkGeneratorMock = new Mock<LinkGenerator>();
            _configurationMock = new Mock<IConfiguration>();
            _mapperMock = new Mock<IMapper>();
            _imdbContext = new ImdbContext();
            _userRepository = new UserRepository(_imdbContext);
        }

        [Fact]
        public void FirstCheck()
        {
            Assert.True(true);
        }


        //Integration Tests
        [Fact]
        public void CreateUser()
        {
            var ctrl = new UserController(_userRepositoryMock.Object, _linkGeneratorMock.Object, _configurationMock.Object);
            ctrl.ControllerContext = new ControllerContext();
            ctrl.ControllerContext.HttpContext = new DefaultHttpContext();

            ctrl.CreateUser(new CreateUpdateUserViewModel());

            _userRepositoryMock.Verify(x => x.CreateUser(It.IsAny<User>()), Times.Once);

        }

        [Fact]
        public void GetUser_ValidUserId_ReturnsOkStatus()
        {
            _userRepositoryMock.Setup(x => x.GetUser(It.IsAny<int>())).Returns(new User());

            var ctrl = CreateUserController();

            var result = ctrl.GetUser(1);

            Assert.IsType<OkObjectResult>(result);

        }


        [Fact]
        public void GetUser_InvalidUserId_ReturnsNotFoundStatus()
        {
            var ctrl = new UserController(_userRepositoryMock.Object, null, null);

            var result = ctrl.GetUser(-1);

            Assert.IsType<NotFoundResult>(result);
        }


        //Unit Tests
        [Fact]
        public void GetUser()
        {
            var response = _userRepository.GetUser(1);

            Assert.IsType<User>(response);
            Assert.Equal(1, response.Id);
        }

        /*
         *
         * Helper methods
         *
         */

        private UserController CreateUserController()
        {
            var ctrl = new UserController(_userRepositoryMock.Object, _linkGeneratorMock.Object, _configurationMock.Object);
            ctrl.ControllerContext = new ControllerContext();
            ctrl.ControllerContext.HttpContext = new DefaultHttpContext();
            return ctrl;
        }
    }
}
