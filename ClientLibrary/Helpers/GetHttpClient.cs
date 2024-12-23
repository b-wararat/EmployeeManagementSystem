
using BaseLibrary.DTOs;
using System.Net.Http;

namespace ClientLibrary.Helpers
{
    public class GetHttpClient
    {
        private const string headerkey = "Authorization";
        private readonly IHttpClientFactory httpClientFactory;
        private readonly LocalStorageService localStorageService;
        public GetHttpClient(IHttpClientFactory httpClientFactory, LocalStorageService localStorageService)
        {
            this.httpClientFactory = httpClientFactory;
            this.localStorageService = localStorageService;
        }
        public async Task<HttpClient> GetPrivateHttpClient() {
            var client = httpClientFactory.CreateClient("SystemApiClient");
            var stringToken = await localStorageService.GetToken();
            if (string.IsNullOrWhiteSpace(stringToken)) return client; //if stringToken is null, means that new user
            
            var deserializaToken = Serializations.DeseruzlizeJsonString<UserSession>(stringToken);
            if(deserializaToken is null) return client;

            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", deserializaToken.Token);
            return client;
        }
        public HttpClient GetPublicHttpClient() {
            var client = httpClientFactory.CreateClient("SystemApiClient");
            client.DefaultRequestHeaders.Remove(headerkey);
            return client;
        }
    }
}
