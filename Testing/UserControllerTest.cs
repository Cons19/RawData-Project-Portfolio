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
using Npgsql;

namespace Testing
{
    public class UserControllerTest
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<ITitleRepository> _titleRepositoryMock;
        private readonly Mock<LinkGenerator> _linkGeneratorMock;
        private readonly Mock<IConfiguration> _configurationMock;
        private readonly Mock<IMapper> _mapperMock;

        private readonly ImdbContext _imdbContext;
        private readonly UserRepository _userRepository;
        private readonly TitleRepository _titleRepository;
        private readonly PersonRepository _personRepository;

        public UserControllerTest()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _titleRepositoryMock = new Mock<ITitleRepository>();
            _linkGeneratorMock = new Mock<LinkGenerator>();
            _configurationMock = new Mock<IConfiguration>();
            _mapperMock = new Mock<IMapper>();

            _imdbContext = new ImdbContext();
            _userRepository = new UserRepository(_imdbContext);
            _titleRepository = new TitleRepository(_imdbContext);
            _personRepository = new PersonRepository(_imdbContext);
        }

        [Fact]
        public void CreateUser_ValidNewUser_UserRepositoryCreateUserMustBeCalledOnce()
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

            // Here, we could have any id
            // What matters is the return type
            var result = ctrl.GetUser(1);

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void GetUser_ValidUserId_ReturnsUserTypeAndUserId()
        {
            var response = _userRepository.GetUser(1);

            Assert.IsType<User>(response);

            // Here, the id matters
            Assert.Equal(1, response.Id);
        }

        [Fact]
        public void GetUser_InvalidUserId_ReturnsNotFoundStatus()
        {
            var ctrl = new UserController(_userRepositoryMock.Object, null, null);

            // Here, we could have any id
            // What matters is the return type
            var result = ctrl.GetUser(-1);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void GetUser_ValidUserEmail_ReturnsUserTypeAndUserEmail()
        {
            var result = _userRepository.GetUserByEmail("em24@email.com");

            Assert.IsType<User>(result);

            Assert.Equal("em24@email.com", result.Email);
        }
        
        [Fact]
        public void GetUser_InvalidUserEmail_ReturnsNull()
        {
            var result = _userRepository.GetUserByEmail("em25@email.com");

            Assert.Null(result);
        }

        [Fact]
        public void UpdateUser_ValidIdValidNewUser_DataServiceUpdateUserMustBeCalledOnce()
        {
            _userRepositoryMock.Setup(x => x.GetUser(It.IsAny<int>())).Returns(new User());

            var ctrl = new UserController(_userRepositoryMock.Object, _linkGeneratorMock.Object, _configurationMock.Object);

            ctrl.ControllerContext = new ControllerContext();

            ctrl.ControllerContext.HttpContext = new DefaultHttpContext();

            ctrl.UpdateUser(1, new CreateUpdateUserViewModel { Name = "Dragos", Email = "something@ruc.dk", Password = "pass" });

            _userRepositoryMock.Verify(x => x.UpdateUser(It.IsAny<User>()), Times.Once);
        }
        /*
        [Fact]
        public void UpdateUser_InvalidId_DataServiceUpdateUserMustNeverBeCalled()
        {
            _userRepositoryMock.Setup(x => x.GetUser(It.IsAny<int>())).Returns(new User());

            var ctrl = new UserController(_userRepositoryMock.Object, _linkGeneratorMock.Object, _configurationMock.Object);

            ctrl.ControllerContext = new ControllerContext();

            ctrl.ControllerContext.HttpContext = new DefaultHttpContext();

            var result = ctrl.UpdateUser(-1, new CreateUpdateUserViewModel { Name = "Dragos", Email = "something@ruc.dk", Password = "pass" });

            _userRepositoryMock.Verify(x => x.UpdateUser(It.IsAny<User>()), Times.Never);
        }
        */

        [Fact]
        public void DeleteUser_ValidId_DataServiceDeleteUserMustReturnNoContentStatus()
        {
            _userRepositoryMock.Setup(x => x.DeleteUser(It.IsAny<int>())).Returns(true);

            var ctrl = new UserController(_userRepositoryMock.Object, _linkGeneratorMock.Object, _configurationMock.Object);

            ctrl.ControllerContext = new ControllerContext();

            ctrl.ControllerContext.HttpContext = new DefaultHttpContext();

            var result = ctrl.DeleteUser(1);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public void DeleteUser_InvalidId_DataServiceDeleteUserMustReturnNotFoundStatus()
        {
            _userRepositoryMock.Setup(x => x.GetUser(It.IsAny<int>())).Returns(new User());

            var ctrl = new UserController(_userRepositoryMock.Object, _linkGeneratorMock.Object, _configurationMock.Object);

            ctrl.ControllerContext = new ControllerContext();

            ctrl.ControllerContext.HttpContext = new DefaultHttpContext();

            var result = ctrl.DeleteUser(29);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void LoginUser_ValidEmailValidPassword_DataServiceLoginUserMustBeCalledOnce()
        {
            var ctrl = new UserController(_userRepositoryMock.Object, _linkGeneratorMock.Object, _configurationMock.Object);

            ctrl.ControllerContext = new ControllerContext();

            ctrl.ControllerContext.HttpContext = new DefaultHttpContext();

            ctrl.LoginUser(new LoginUserViewModel());

            _userRepositoryMock.Verify(x => x.LoginUser(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }
        [Fact]
        public void LoginUser_ValidEmailValidPassword_DataServiceLoginUserMustReturnBadRequestStatus()
        {
            var ctrl = new UserController(_userRepositoryMock.Object, _linkGeneratorMock.Object, _configurationMock.Object);

            ctrl.ControllerContext = new ControllerContext();

            ctrl.ControllerContext.HttpContext = new DefaultHttpContext();

            var result = ctrl.LoginUser(new LoginUserViewModel { Email = "asd@ruc.dk", Password = "asd" });

            Assert.IsType<BadRequestResult>(result);
        }


        // Functions
        // No. 5
        [Fact]
        public void SearchText_ValidUserIdValidString_TitleRepositorySearchTextMustReturnListOfSearchTitle()
        {
            var result = _titleRepository.SearchText(1, "apple");

            Assert.IsType<List<SearchTitle>>(result);
        }

        [Fact]
        public void SearchText_ValidUserIdValidString_TitleRepositorySearchTextMustReturnEmptyList()
        {
            var result = _titleRepository.SearchText(1, "zzzzzzzzzzzzzzz");

            Assert.Empty(result);
        }


        // No. 6
        [Fact]
        public void RateTitle_ValidUserIdValidTitleIdValidRating_TitleRepositoryRateTitleMustReturnNull()
        {
            var result = _titleRepository.RateTitle(1, "tt2066994 ", 7);
            // Here, returning null means executing the script
            Assert.Null(result);
        }

        [Fact]
        public void RateTitle_InvalidUserIdValidTitleIdValidRating_TitleRepositoryRateTitleMustReturnPostgresException()
        {
            var result = _titleRepository.RateTitle(-1, "tt2066994 ", 7);

            Assert.IsType<PostgresException>(result);
        }


        [Fact]
        public void RateTitle_ValidUserIdInvalidTitleIdValidRating_TitleRepositoryRateTitleMustReturnException()
        {
            var result = _titleRepository.RateTitle(1, "tt2066994z ", 7);

            Assert.IsType<PostgresException>(result);
        }
        
        // No. 7
        [Fact]
        public void StructuredStringSearch_ValidIdValidPlotValidCharacter_TitleRepositoryStructuredStringSearchMustReturnListOfStructuredStringSearch()
        {
            var result = _titleRepository.StructuredStringSearch(1, null, "see", null, "maDs miKkelsen");

            Assert.IsType<List<StructuredStringSearch>>(result);
        }

        [Fact]
        public void StructuredStringSearch_ValidId_TitleRepositoryStructuredStringSearchMustReturnEmptyList()
        {
            var result = _titleRepository.StructuredStringSearch(1, null, null, null, "zzzzzzzzzzzzzz");

            Assert.Empty(result);
        }

        // No. 8
        [Fact]
        public void FindPersonByProfession_ValidProfession_PersonRepositoryFindPersonByProfessionMustReturnListOfFindPersonByProfession()
        {
            var result = _personRepository.FindPersonByProfession("actor");

            Assert.IsType<List<FindPersonByProfession>>(result);
        }

        [Fact]
        public void FindPersonByProfession_InvalidProfession_PersonRepositoryFindPersonByProfessionMustReturnEmptyList()
        {
            var result = _personRepository.FindPersonByProfession("zzzzzzzzzzzzzz");

            Assert.Empty(result);
        }

        // No. 9
        [Fact]
        public void CoActor_ValidPersonId_PersonRepositoryCoActorMustReturnListOfCoActor()
        {
            var result = _personRepository.CoActor("nm0003502 ");

            Assert.IsType<List<CoActor>>(result);
        }

        [Fact]
        public void CoActor_InvalidPersonId_PersonRepositoryCoActorMustReturnEmptyList()
        {
            var result = _personRepository.CoActor("zzzzzzzzzzzzzz ");

            Assert.Empty(result);
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
