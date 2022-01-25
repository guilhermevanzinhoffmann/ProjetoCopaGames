using CopaGamesBackEnd.Interfaces;
using CopaGamesBackEnd.Models;
using CopaGamesBackEnd.Services;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CopaGamesTests.ServiceTests
{
    public class GameServiceTests
    {
        [Fact]
        public async Task GetGamesAsync_DeveRetornarHttpResponseMessageAsync()
        {
            // Arrange
            var newGames = GetGames();
            
            var mock = new Mock<IClient>();

            HttpContent content = new StringContent(JsonConvert.SerializeObject(newGames));

            var httpResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = content
            };

            mock.Setup(x => x.GetAsync(It.IsAny<string>()))
                 .ReturnsAsync(httpResponse);

            IClient mockClient = mock.Object;

            var service = new GameService(mockClient);

            // Act
            var result = await service.GetGamesAsync();

            // Assert
            var objectResult = Assert.IsType<HttpResponseMessage>(result);
            var ddd = objectResult.RequestMessage?.RequestUri;
            var games = Assert.IsAssignableFrom<IEnumerable<Game>>(await objectResult.Content.ReadFromJsonAsync<IEnumerable<Game>>());
            Assert.Equal(2, games.Count());
            Assert.Contains(games, g => g.Id == newGames.ElementAt(0).Id);
            Assert.Contains(games, q => q.Id == newGames.ElementAt(1).Id);

        }

        [Fact]
        public void GetGamesAsync_DeveUtilizarURLCorreta()
        {
            // Arrange
            var mock = new Mock<IClient>();

            IClient mockClient = mock.Object;

            var service = new GameService(mockClient);

            // Act
            var result = service._URL;

            // Assert
            Assert.Equal("https://l3-processoseletivo.azurewebsites.net/api/Competidores?copa=games", result);
        }

        [Theory]
        [InlineData(10, 9)]
        [InlineData(1, 0)]
        [InlineData(9.9, 9.8)]
        [InlineData(0.1, 0)]
        public void CompareGames_ComGameAComNotaMaiorQueGameB_DeveRetornarGameA(float notaA, float notaB)
        {
            // Arrange
            var games = GetGames();
            var gameA = games.ElementAt(0);
            var gameB = games.ElementAt(1);
            gameA.Nota = notaA;
            gameB.Nota = notaB;
            
            var mock = new Mock<IClient>();

            IClient mockClient = mock.Object;

            var service = new GameService(mockClient);

            // Act
            var result = service.CompareGames(gameA, gameB);

            //Assert
            Assert.Equal(gameA.Id, result.Id);
        }

        [Theory]
        [InlineData(9, 10)]
        [InlineData(0, 1)]
        [InlineData(9.8, 9.9)]
        [InlineData(0, 0.1)]
        public void CompareGames_ComGameAComNotaMenorQueGameB_DeveRetornarGameB(float notaA, float notaB)
        {
            // Arrange
            var games = GetGames();
            var gameA = games.ElementAt(0);
            var gameB = games.ElementAt(1);
            gameA.Nota = notaA;
            gameB.Nota = notaB;

            var mock = new Mock<IClient>();

            IClient mockClient = mock.Object;

            var service = new GameService(mockClient);

            // Act
            var result = service.CompareGames(gameA, gameB);

            //Assert
            Assert.Equal(gameB.Id, result.Id);
        }

        [Theory]
        [InlineData(2000, 1999)]
        [InlineData(2022, 2021)]
        [InlineData(2000, 1888)]
        public void CompareGames_ComGamesDeMesmaNotaEGameAMaisNovoQueGameB_DeveRetornarGameA(int anoA, int anoB)
        {
            // Arrange
            var games = GetGames();
            var gameA = games.ElementAt(0);
            var gameB = games.ElementAt(1);
            gameA.Ano = anoA;
            gameB.Ano = anoB;

            var mock = new Mock<IClient>();

            IClient mockClient = mock.Object;

            var service = new GameService(mockClient);

            // Act
            var result = service.CompareGames(gameA, gameB);

            //Assert
            Assert.Equal(gameA.Id, result.Id);
        }

        [Theory]
        [InlineData(1998, 1999)]
        [InlineData(2021, 2022)]
        [InlineData(1888, 2000)]
        public void CompareGames_ComGamesDeMesmaNotaEGameAMaisVelhoQueGameB_DeveRetornarGameB(int anoA, int anoB)
        {
            // Arrange
            var games = GetGames();
            var gameA = games.ElementAt(0);
            var gameB = games.ElementAt(1);
            gameA.Ano = anoA;
            gameB.Ano = anoB;

            var mock = new Mock<IClient>();

            IClient mockClient = mock.Object;

            var service = new GameService(mockClient);

            // Act
            var result = service.CompareGames(gameA, gameB);

            //Assert
            Assert.Equal(gameB.Id, result.Id);
        }

        [Theory]
        [InlineData("a", "b")]
        [InlineData("aa", "ab")]
        [InlineData("ab", "abc")]
        [InlineData("abc0", "abc1")]
        [InlineData("A", "B")]
        [InlineData("AA", "AB")]
        [InlineData("AB", "ABC")]
        [InlineData("ABC0", "ABC1")]
        [InlineData("012", "123")]
        [InlineData("000", "001")]
        public void CompareGames_ComGamesComMesmaNotaEAnoEGameAComTituloAnteriorQueGameB_DeveRetornarGameA(string nomeA, string nomeB)
        {
            // Arrange
            var games = GetGames();
            var gameA = games.ElementAt(0);
            var gameB = games.ElementAt(1);
            gameA.Titulo = nomeA;
            gameB.Titulo = nomeB;

            var mock = new Mock<IClient>();

            IClient mockClient = mock.Object;

            var service = new GameService(mockClient);

            // Act
            var result = service.CompareGames(gameA, gameB);

            //Assert
            Assert.Equal(gameA.Id, result.Id);
        }

        [Theory]
        [InlineData("b", "a")]
        [InlineData("ab", "aa")]
        [InlineData("abc", "ab")]
        [InlineData("abc1", "abc0")]
        [InlineData("B", "A")]
        [InlineData("AB", "AA")]
        [InlineData("ABC", "AB")]
        [InlineData("ABC1", "ABC0")]
        [InlineData("123", "012")]
        [InlineData("001", "000")]
        public void CompareGames_ComGamesComMesmaNotaEAnoEGameAComTituloPosteriorQueGameB_DeveRetornarGameB(string nomeA, string nomeB)
        {
            // Arrange
            var games = GetGames();
            var gameA = games.ElementAt(0);
            var gameB = games.ElementAt(1);
            gameA.Titulo = nomeA;
            gameB.Titulo = nomeB;

            var mock = new Mock<IClient>();

            IClient mockClient = mock.Object;

            var service = new GameService(mockClient);

            // Act
            var result = service.CompareGames(gameA, gameB);

            //Assert
            Assert.Equal(gameB.Id, result.Id);
        }

        [Fact]
        public void GetResult_ComGame1DeMaiorNotaEGame7ComSegundaMaiorNota_DeveRetornarListaComGame1emPrimeiroeGame6EmSegundo()
        {
            // Arrange
            var newGames = GetGames(8);
            var game1 = newGames.ElementAt(0);
            var game6 = newGames.ElementAt(5);
            game1.Nota = 12;
            game6.Nota = 11;
            
            var mock = new Mock<IClient>();

            IClient mockClient = mock.Object;

            var service = new GameService(mockClient);

            // Act
            var result = service.GetResult(newGames);

            // Assert
            Assert.Equal(2, result.Count());
            Assert.Contains(game1.Id, result.ElementAt(0).Id);
            Assert.Contains(game6.Id, result.ElementAt(1).Id);

        }

        [Fact]
        public void GetResult_ComGame1DeMaiorNotaGame2ComSegundaMaiorNotaEGame6ComTerceiraMaiorNota_DeveRetornarListaComGame1emPrimeiroeGame6EmSegundo()
        {
            // Arrange
            var newGames = GetGames(8);
            var game1 = newGames.ElementAt(0);
            var game2 = newGames.ElementAt(1);
            var game6 = newGames.ElementAt(5);
            game1.Nota = 13;
            game2.Nota = 12;
            game6.Nota = 11;

            var mock = new Mock<IClient>();

            IClient mockClient = mock.Object;

            var service = new GameService(mockClient);

            // Act
            var result = service.GetResult(newGames);

            // Assert
            Assert.Equal(2, result.Count());
            Assert.Contains(game1.Id, result.ElementAt(0).Id);
            Assert.Contains(game6.Id, result.ElementAt(1).Id);

        }

        [Fact]
        public void GetResult_ComGamesDeMesmaNotaEComGame1MaisNovoEGame6SegundoMaisNovo_DeveRetornarListaComGame1emPrimeiroeGame6EmSegundo()
        {
            // Arrange
            var newGames = GetGames(8);
            var game1 = newGames.ElementAt(0);
            var game6 = newGames.ElementAt(5);
            game1.Ano = 2022;
            game6.Ano = 2021;

            var mock = new Mock<IClient>();

            IClient mockClient = mock.Object;

            var service = new GameService(mockClient);

            // Act
            var result = service.GetResult(newGames);

            // Assert
            Assert.Equal(2, result.Count());
            Assert.Contains(game1.Id, result.ElementAt(0).Id);
            Assert.Contains(game6.Id, result.ElementAt(1).Id);

        }

        [Fact]
        public void GetResult_ComGamesDeMesmaNotaEComGame1MaisNovoGame2SegundoMaisNovoEGame6TerceiroMaisNovo_DeveRetornarListaComGame1emPrimeiroeGame6EmSegundo()
        {
            // Arrange
            var newGames = GetGames(8);
            var game1 = newGames.ElementAt(0);
            var game2 = newGames.ElementAt(1);
            var game6 = newGames.ElementAt(5);
            game1.Ano = 2022;
            game2.Ano = 2021;
            game6.Ano = 2020;

            var mock = new Mock<IClient>();

            IClient mockClient = mock.Object;

            var service = new GameService(mockClient);

            // Act
            var result = service.GetResult(newGames);

            // Assert
            Assert.Equal(2, result.Count());
            Assert.Contains(game1.Id, result.ElementAt(0).Id);
            Assert.Contains(game6.Id, result.ElementAt(1).Id);

        }

        [Fact]
        public void GetResult_ComGamesDeMesmaNotaEAnoEComGame1ComTituloPrimeiroNaOrdemAlfabeticaEGame6ComTituloSegundoNaOrdemAlfabetica_DeveRetornarListaComGame1emPrimeiroeGame6EmSegundo()
        {
            // Arrange
            var newGames = GetGames(8);
            var game1 = newGames.ElementAt(0);
            var game6 = newGames.ElementAt(5);
            game1.Titulo = "a";
            game6.Titulo = "b";

            var mock = new Mock<IClient>();

            IClient mockClient = mock.Object;

            var service = new GameService(mockClient);

            // Act
            var result = service.GetResult(newGames);

            // Assert
            Assert.Equal(2, result.Count());
            Assert.Contains(game1.Id, result.ElementAt(0).Id);
            Assert.Contains(game6.Id, result.ElementAt(1).Id);

        }

        [Fact]
        public void GetResult_ComGamesDeMesmaNotaEAnoComGame1ComTituloPrimeiroNaOrdemAlfabeticaGame2ComTituloEmSegundoNaOrdemAlfabeticaEGame6ComTituloEmTerceiroNaOrdemAlfabetica_DeveRetornarListaComGame1emPrimeiroeGame6EmSegundo()
        {
            // Arrange
            var newGames = GetGames(8);
            var game1 = newGames.ElementAt(0);
            var game2 = newGames.ElementAt(1);
            var game6 = newGames.ElementAt(5);
            game1.Titulo = "a";
            game2.Titulo = "b";
            game6.Titulo = "c";

            var mock = new Mock<IClient>();

            IClient mockClient = mock.Object;

            var service = new GameService(mockClient);

            // Act
            var result = service.GetResult(newGames);

            // Assert
            Assert.Equal(2, result.Count());
            Assert.Contains(game1.Id, result.ElementAt(0).Id);
            Assert.Contains(game6.Id, result.ElementAt(1).Id);

        }

        [Fact]
        public void GetResult_ComListaDeGamesDeTamanhoMenorQue8_DeveLancarArgumentException()
        {
            // Arrange
            var newGames = GetGames(7);

            var mock = new Mock<IClient>();

            IClient mockClient = mock.Object;

            var service = new GameService(mockClient);

            // Act && Assert
            Assert.Throws<ArgumentException>(() => service.GetResult(newGames));
        }

        [Fact]
        public void GetResult_ComListaDeGamesNula_DeveLancarArgumentNullException()
        {
            // Arrange
            var mock = new Mock<IClient>();

            IClient mockClient = mock.Object;

            var service = new GameService(mockClient);

            // Act && Assert
#pragma warning disable CS8625 // Não é possível converter um literal nulo em um tipo de referência não anulável.
            Assert.Throws<ArgumentNullException>(() => service.GetResult(null));
#pragma warning restore CS8625 // Não é possível converter um literal nulo em um tipo de referência não anulável.
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
