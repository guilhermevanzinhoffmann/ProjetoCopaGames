using CopaGamesBackEnd.Controllers;
using CopaGamesBackEnd.Interfaces;
using CopaGamesBackEnd.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace CopaGamesTests.ControllerTests
{
    public class GameControllerTests
    {
        [Fact]
        public async Task GetAllAsync_DeveRetornarListaDeGamesAsync()
        {
            // Arrange
            var newGames = GetGames();
            var mock = new Mock<IGameService>();
            HttpContent content = new StringContent(JsonConvert.SerializeObject(newGames));
            
            var httpResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = content
            };
            
            mock.Setup(x => x.GetGamesAsync())
                .ReturnsAsync(httpResponse);

            IGameService gameService = mock.Object;
            
            var controller = new GameController(gameService);

            // Act
            var result = await controller.GetAllAsync();

            // Assert
            var objectResult = Assert.IsType<OkObjectResult>(result.Result);
            var games = Assert.IsAssignableFrom<IEnumerable<Game>>(objectResult.Value);
            Assert.Equal(2, games.Count());
            Assert.Contains(games, g => g.Id == newGames.ElementAt(0).Id);
            Assert.Contains(games, q => q.Id == newGames.ElementAt(1).Id);
        }

        [Fact]
        public async Task GetAllAsync_ComListaDeGamesVazia_DeveRetornarNoContentAsync()
        {
            // Arrange
            var newGames = new List<Game>();
            var mock = new Mock<IGameService>();
            HttpContent content = new StringContent(JsonConvert.SerializeObject(newGames));

            var httpResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = content
            };

            mock.Setup(x => x.GetGamesAsync())
                .ReturnsAsync(httpResponse);

            IGameService gameService = mock.Object;

            var controller = new GameController(gameService);

            // Act
            var result = await controller.GetAllAsync();

            // Assert
            Assert.IsType<NoContentResult>(result.Result);
        }

        [Fact]
        public async Task GetAllAsync_SemReceberContent_DeveRetornarInternalServerErrorAsync()
        {
            // Arrange
            var mock = new Mock<IGameService>();

            var httpResponse = new HttpResponseMessage(HttpStatusCode.OK);

            mock.Setup(x => x.GetGamesAsync())
                .ReturnsAsync(httpResponse);

            IGameService gameService = mock.Object;

            var controller = new GameController(gameService);

            // Act
            var result = await controller.GetAllAsync();

            // Assert
            var objectResult = Assert.IsType<StatusCodeResult>(result.Result);
            Assert.Equal((int)HttpStatusCode.InternalServerError, objectResult.StatusCode);
        }

        [Fact]
        public async Task GetAllAsync_DeveRetornarNotFoundAsync()
        {
            // Arrange
            var mock = new Mock<IGameService>();

            var httpResponse = new HttpResponseMessage(HttpStatusCode.NotFound);

            mock.Setup(x => x.GetGamesAsync())
                .ReturnsAsync(httpResponse);

            IGameService gameService = mock.Object;

            var controller = new GameController(gameService);

            // Act
            var result = await controller.GetAllAsync();

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task GetAllAsync_DeveRetornarInternalServerErrorAsync()
        {
            // Arrange
            var mock = new Mock<IGameService>();

            var httpResponse = new HttpResponseMessage(HttpStatusCode.InternalServerError);

            mock.Setup(x => x.GetGamesAsync())
                .ReturnsAsync(httpResponse);

            IGameService gameService = mock.Object;

            var controller = new GameController(gameService);

            // Act
            var result = await controller.GetAllAsync();

            // Assert
            var objectResult = Assert.IsType<StatusCodeResult>(result.Result);
            Assert.Equal((int)HttpStatusCode.InternalServerError, objectResult.StatusCode);
        }

        [Fact]
        public void Result_DeveRetornarListaDeGames()
        {
            // Arrange
            var newGames = GetGames();
            var winnerGames = GetGames();

            var mock = new Mock<IGameService>();

            mock.Setup(x => x.GetResult(newGames))
                .Returns(winnerGames);

            IGameService gameService = mock.Object;

            var controller = new GameController(gameService);

            // Act
            var result = controller.Result(newGames);

            // Assert
            var objectResult = Assert.IsType<OkObjectResult>(result.Result);
            var games = Assert.IsAssignableFrom<IEnumerable<Game>>(objectResult.Value);
            Assert.Equal(2, games.Count());
            Assert.Contains(games, g => g.Id == winnerGames.ElementAt(0).Id);
            Assert.Contains(games, q => q.Id == winnerGames.ElementAt(1).Id);
        }

        [Fact]
        public void Result_ComListaDeGamesVencedoresVazia_DeveRetornarNoContent()
        {
            // Arrange
            var newGames = GetGames();
            var mock = new Mock<IGameService>();

            mock.Setup(x => x.GetResult(newGames))
                .Returns(new List<Game>());

            IGameService gameService = mock.Object;

            var controller = new GameController(gameService);

            // Act
            var result = controller.Result(newGames);

            // Assert
            Assert.IsType<NoContentResult>(result.Result);
        }

        [Fact]
        public void Result_ComExceptionLancada_DeveRetornarInternalServerError()
        {
            // Arrange
            var newGames = GetGames();
            var mock = new Mock<IGameService>();

            mock.Setup(x => x.GetResult(newGames))
                .Throws(new Exception());

            IGameService gameService = mock.Object;

            var controller = new GameController(gameService);

            // Act
            var result = controller.Result(newGames);

            // Assert
            var objectResult = Assert.IsType<StatusCodeResult>(result.Result);
            Assert.Equal((int)HttpStatusCode.InternalServerError, objectResult.StatusCode);
        }

        [Fact]
        public void Result_ComArgumentNullExceptionLancada_DeveRetornarBadRequestObjectResult()
        {
            // Arrange
            var newGames = GetGames();
            var mock = new Mock<IGameService>();

            mock.Setup(x => x.GetResult(newGames))
                .Throws(new ArgumentNullException());

            IGameService gameService = mock.Object;

            var controller = new GameController(gameService);

            // Act
            var result = controller.Result(newGames);

            // Assert
            var objectResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.NotNull(objectResult);
        }

        [Fact]
        public void Result_ComArgumentExceptionLancada_DeveRetornarBadRequestObjectResult()
        {
            // Arrange
            var newGames = GetGames();
            var mock = new Mock<IGameService>();

            mock.Setup(x => x.GetResult(newGames))
                .Throws(new ArgumentException());

            IGameService gameService = mock.Object;

            var controller = new GameController(gameService);

            // Act
            var result = controller.Result(newGames);

            // Assert
            var objectResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.NotNull(objectResult);
        }

        private IEnumerable<Game> GetGames(int quantity = 2)
        {
            var games = new List<Game>();

            for (int i = 0; i < quantity; i++)
            {
                games.Add(new Game { Id = $"game_{i}", Ano = 2000, Nota = 10, Titulo = $"Game {i} (CSL)", UrlImagem = $"/game_{i}" });
            }
            
            return games;
        }
    }
}
