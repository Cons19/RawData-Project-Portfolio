using System;
using System.Collections.Generic;
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
using DataAccessLayer.Domain.Functions;
using Npgsql;

namespace Testing
{
    public class UserControllerTest
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<LinkGenerator> _linkGeneratorMock;
        private readonly Mock<IConfiguration> _configurationMock;

        private readonly ImdbContext _imdbContext;
        private readonly UserRepository _userRepository;
        private readonly PersonRepository _personRepository;
        private readonly TitleRepository _titleRepository;
        private readonly WordToWordRepository _wordToWordRepository;
        private readonly UpdatePersonsRatingRepository _updatePersonsRatingRepository;


        public UserControllerTest()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _linkGeneratorMock = new Mock<LinkGenerator>();
            _configurationMock = new Mock<IConfiguration>();

            _imdbContext = new ImdbContext();
            _userRepository = new UserRepository(_imdbContext);
            _updatePersonsRatingRepository = new UpdatePersonsRatingRepository(_imdbContext);
            _personRepository = new PersonRepository(_imdbContext);
            _titleRepository = new TitleRepository(_imdbContext);
            _wordToWordRepository = new WordToWordRepository(_imdbContext);
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

            _userRepositoryMock.Verify(x => x.UpdateUser(It.IsAny<User>(), true), Times.Once);
        }

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
        public void LoginUser_InvalidEmailInvalidPassword_DataServiceLoginUserMustReturnUnauthorizedStatus()
        {
            var ctrl = new UserController(_userRepositoryMock.Object, _linkGeneratorMock.Object, _configurationMock.Object);

            ctrl.ControllerContext = new ControllerContext();

            ctrl.ControllerContext.HttpContext = new DefaultHttpContext();

            var result = ctrl.LoginUser(new LoginUserViewModel { Email = "asd@ruc.dk", Password = "asd" });

            Assert.IsType<UnauthorizedResult>(result);
        }

        // Functions
        [Fact]
        public void SearchText_ValidUserIdValidString_TitleRepositorySearchTextMustReturnListOfSearchTitle()
        {
            var result = _titleRepository.SearchText(1, "apple", new DataAccessLayer.QueryString());

            Assert.IsType<Object[]>(result);
        }

        [Fact]
        public void SearchText_ValidUserIdValidString_TitleRepositorySearchTextMustReturnEmptyList()
        {
            var result = _titleRepository.SearchText(1, "zzzzzzzzzzzzzzz", new DataAccessLayer.QueryString());

            Assert.Equal(new object[] { new List<SearchTitle>(), 0 }, result);
            Assert.IsType<Object[]>(result);
        }

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

        [Fact]
        public void StructuredStringSearch_ValidIdValidPlotValidCharacter_TitleRepositoryStructuredStringSearchMustReturnListOfStructuredStringSearch()
        {
            var result = _titleRepository.StructuredStringSearch(1, null, "see", null, "maDs miKkelsen", new DataAccessLayer.QueryString());

            Assert.IsType<List<StructuredStringSearch>>(result);
        }

        [Fact]
        public void StructuredStringSearch_ValidId_TitleRepositoryStructuredStringSearchMustReturnEmptyList()
        {
            var result = _titleRepository.StructuredStringSearch(1, null, null, null, "zzzzzzzzzzzzzz", new DataAccessLayer.QueryString());

            Assert.Empty(result);
        }

        [Fact]
        public void FindPersonByProfession_ValidProfession_PersonRepositoryFindPersonByProfessionMustReturnListOfFindPersonByProfession()
        {
            var result = _personRepository.FindPersonByProfession("actor", new DataAccessLayer.QueryString());

            Assert.IsType<List<FindPersonByProfession>>(result);
        }

        [Fact]
        public void FindPersonByProfession_InvalidProfession_PersonRepositoryFindPersonByProfessionMustReturnEmptyList()
        {
            var result = _personRepository.FindPersonByProfession("zzzzzzzzzzzzzz", new DataAccessLayer.QueryString());

            Assert.Empty(result);
        }

        [Fact]
        public void CoActor_ValidPersonId_PersonRepositoryCoActorMustReturnListOfCoActor()
        {
            var result = _personRepository.CoActor("John Cleese", new DataAccessLayer.QueryString());
            Assert.IsType<List<CoActor>>(result);

        }

        [Fact]
        public void CoActor_InvalidPersonId_PersonRepositoryCoActorMustReturnEmptyList()
        {
            var result = _personRepository.CoActor("zzzzzzzzzzzzzz", new DataAccessLayer.QueryString());
            Assert.Empty(result);
        }

        [Fact]
        public void UpdatePersonsRating_ValidPersonId_PersonRepositoryGetPersonMustReturnNotNull()
        {
            // check if a person that is supposed to have a rating, has one
            _updatePersonsRatingRepository.UpdatePersonsRating();
            var person = _personRepository.GetPerson("nm0000056");

            Assert.NotNull(person.Rating);
        }

        // Popular Actors Test
        [Fact]
        public void PopularActors_ValidTitle_PersonRepositoryPopularActorsMustReturnNotNullPopularActorsTypeAndTwoActorNames()
        {
            var response = _personRepository.PopularActors("Casino Royale", new DataAccessLayer.QueryString());

            foreach (PopularActors actor in response)
            {
                Assert.NotNull(actor);
                Assert.IsType<PopularActors>(actor);
            }
            Assert.Contains(response, r => r.Name == "Jeffrey Wright");
            Assert.Contains(response, r => r.Name == "Daniel Craig");
        }

        // Similar Movies Test
        [Fact]
        public void SimilarTitles_ValidTitle_TitleRepositorySimilarTitlesMustReturnNotNullSimilarTitleTypeIdNameStartYearAndGenre()
        {
            var response = _titleRepository.SimilarTitle("tt0052520", new DataAccessLayer.QueryString());

            foreach (SimilarTitle title in response)
            {
                Assert.NotNull(title);
                Assert.IsType<SimilarTitle>(title);
            }
            Assert.Contains(response, r => r.Id == "tt1531642 ");
            Assert.Contains(response, r => r.Name == "Charles Beaumont: The Short Life of Twilight Zone's Magic Man");
            Assert.Contains(response, r => r.StartYear == "2010");
            Assert.Contains(response, r => r.Genre == "Fantasy");
        }

        // Exact Match Test
        [Fact]
        public void ExactMatch_ValidThreeWordsEmptyCategory_TitleRepositoryExactMatchMustReturnNotNullExactMatchTypeIdAndTitle()
        {
            var response = _titleRepository.ExactMatch("apple", "mads", "mikkelsen", "", new DataAccessLayer.QueryString());

            foreach (ExactMatch title in response)
            {
                Assert.NotNull(title);
                Assert.IsType<ExactMatch>(title);
            }
            Assert.Contains(response, r => r.Id == "tt0418455 ");
            Assert.Contains(response, r => r.Title == "Adam's Apples");
        }

        // Best Match Test
        [Fact]
        public void BestMatch_ValidThreeWords_TitleRepositoryBestMatchMustReturnNotNullBestMatchTypeIdRankAndTitle()
        {
            var response = _titleRepository.BestMatch("apple", "mads", "mikkelsen", new DataAccessLayer.QueryString());

            foreach (BestMatch title in response)
            {
                Assert.NotNull(title);
                Assert.IsType<BestMatch>(title);
            }
            Assert.Contains(response, r => r.Id == "tt0418455 ");
            Assert.Contains(response, r => r.Rank == 3);
            Assert.Contains(response, r => r.Title == "Adam's Apples");
        }

        // Words to Words Test
        [Fact]
        public void WordsToWords_ValidThreeWords_WordToWordRepositoryWordsToWordsMustReturnNotNullWordToWordTypeCounterAndWord()
        {
            string[] words = { "village", "fisherman", "sicily" };
            var response = _wordToWordRepository.GetWordToWord(words);

            foreach (WordToWord word in response)
            {
                Assert.NotNull(word);
                Assert.IsType<WordToWord>(word);
            }
            Assert.Contains(response, r => r.Counter == 552);
            Assert.Contains(response, r => r.Word == "village");
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
