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
using DataAccessLayer.Repository.Interfaces;
using DataAccessLayer.Domain.Functions;

namespace Testing
{
    public class UserControllerTest
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IPersonRepository> _personRepositoryMock;
        private readonly Mock<LinkGenerator> _linkGeneratorMock;
        private readonly Mock<IConfiguration> _configurationMock;
        private readonly ImdbContext _imdbContext;
        private readonly UserRepository _userRepository;
        private readonly PersonRepository _personRepository;
        private readonly TitleRepository _titleRepository;
        private readonly WordToWordRepository _wordToWordRepository;
        private UpdatePersonsRatingRepository _updatePersonsRatingRepository;


        public UserControllerTest()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _linkGeneratorMock = new Mock<LinkGenerator>();
            _configurationMock = new Mock<IConfiguration>();
            _personRepositoryMock = new Mock<IPersonRepository>();

            _imdbContext = new ImdbContext();
            _userRepository = new UserRepository(_imdbContext);
            _updatePersonsRatingRepository = new UpdatePersonsRatingRepository(_imdbContext);
            _personRepository = new PersonRepository(_imdbContext);
            _titleRepository = new TitleRepository(_imdbContext);
            _wordToWordRepository = new WordToWordRepository(_imdbContext);
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

        // Name rating Test
        [Fact]
        public void UpdatePersonsRatingFunctionality()
        {
            // check if a person that is supposed to have a rating, has one
            _updatePersonsRatingRepository.UpdatePersonsRating();
            var person = _personRepository.GetPerson("nm0000056");

            Assert.NotNull(person.Rating);
        }

        // Popular Actors Test
        [Fact]
        public void PopularActorsFunctionality()
        {
            var response = _personRepository.PopularActors("Casino Royale");

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
        public void SimilarMoviesFunctionality()
        {
            var response = _titleRepository.SimilarTitle("tt0052520");

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
        public void ExactMatchFunctionality()
        {
            var response = _titleRepository.ExactMatch("apple", "mads", "mikkelsen", "");

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
        public void BestMatchFunctionality()
        {
            var response = _titleRepository.BestMatch("apple", "mads", "mikkelsen");

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
        public void WordsToWordsFunctionality()
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
