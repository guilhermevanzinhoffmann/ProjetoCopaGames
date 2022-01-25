using CopaGamesBackEnd.Models;

namespace CopaGamesBackEnd.Interfaces
{
    public interface IGameService
    {
        Task<HttpResponseMessage> GetGamesAsync();
        IEnumerable<Game> GetResult(IEnumerable<Game> games);
    }
}