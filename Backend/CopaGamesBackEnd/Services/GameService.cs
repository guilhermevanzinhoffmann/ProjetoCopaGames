using CopaGamesBackEnd.Interfaces;
using CopaGamesBackEnd.Models;

namespace CopaGamesBackEnd.Services
{
    public class GameService : IGameService
    {
        private readonly IClient _client;
        public readonly string _URL = "https://l3-processoseletivo.azurewebsites.net/api/Competidores?copa=games";
        
        public GameService(IClient client)
        {
            _client = client;
        }

        public async Task<HttpResponseMessage> GetGamesAsync()
            => await _client.GetAsync(_URL);
            
        public IEnumerable<Game> GetResult(IEnumerable<Game> games)
        {
            if (games == null)
                throw new ArgumentNullException($"{nameof(games)} não pode ser nulo.");

            if (games.Count() != 8)
                throw new ArgumentException("Número de games selecionados inválido.");

            var v1 = CompareGames(games.ElementAt(0), games.ElementAt(7));
            var v2 = CompareGames(games.ElementAt(1), games.ElementAt(6));
            var v3 = CompareGames(games.ElementAt(2), games.ElementAt(5));
            var v4 = CompareGames(games.ElementAt(3), games.ElementAt(4));

            var v5 = CompareGames(v1, v2);
            var v6 = CompareGames(v3, v4);

            var winner = CompareGames(v5, v6);
            var second = winner.Id == v5.Id ? v6 : v5;

            return new List<Game>()
            {
                winner,
                second
            };
        }

        public Game CompareGames(Game gameA, Game gameB)
        {
            if (gameA.Nota > gameB.Nota)
            {
                return gameA;
            }
            else if (gameA.Nota == gameB.Nota)
            {
                if (gameA.Ano > gameB.Ano)
                {
                    return gameA;
                }
                else if (gameA.Ano == gameB.Ano)
                {
                    var comparison = string.Compare(gameA.Titulo, gameB.Titulo, comparisonType: StringComparison.OrdinalIgnoreCase);

                    if (comparison > 0)
                        return gameB;

                    return gameA;
                }
                else
                {
                    return gameB;
                }
            }
            else
            {
                return gameB;
            }
        }
    }
}