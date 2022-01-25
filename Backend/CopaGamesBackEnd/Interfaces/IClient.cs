namespace CopaGamesBackEnd.Interfaces
{
    public interface IClient
    { 
        Task<HttpResponseMessage> GetAsync(string url);
    }
}
