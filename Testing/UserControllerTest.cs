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
using Moq;
using WebServiceLayer.Controllers;
using WebServiceLayer.ViewModels;
using Xunit;

namespace Testing
{
    public class UserControllerTest
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<LinkGenerator> _linkGeneratorMock;

        public UserControllerTest()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _linkGeneratorMock = new Mock<LinkGenerator>();
        }

        [Fact]
        public void FirstCheck()
        {
            Assert.True(true);
        }

        [Fact]
        public void CreateUser()
        {
            var ctrl = new UserController(_userRepositoryMock.Object, _linkGeneratorMock.Object);
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
            var ctrl = new UserController(_userRepositoryMock.Object, null);

            var result = ctrl.GetUser(-1);

            Assert.IsType<NotFoundResult>(result);
        }

        /*
         *
         * Helper methods
         *
         */

        private UserController CreateUserController()
        {
            var ctrl = new UserController(_userRepositoryMock.Object, _linkGeneratorMock.Object);
            ctrl.ControllerContext = new ControllerContext();
            ctrl.ControllerContext.HttpContext = new DefaultHttpContext();
            return ctrl;
        }
    }
}
