using CopaGamesBackEnd.Interfaces;

namespace CopaGamesBackEnd
{
    public class Client : IClient
    {
        public async Task<HttpResponseMessage> GetAsync(string url)
        {
            return await HttpClientProvider.GetHttpClient().GetAsync(url); 
        }
    }
    
    public static class HttpClientProvider 
    {
        private static HttpClient _httpClient = new HttpClient();

        public static HttpClient GetHttpClient()
            => _httpClient;
    }
}