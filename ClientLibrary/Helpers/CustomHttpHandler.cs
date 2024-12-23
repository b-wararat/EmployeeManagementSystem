using BaseLibrary.DTOs;
using ClientLibrary.Services.Contracts;

namespace ClientLibrary.Helpers
{
    public class CustomHttpHandler : DelegatingHandler
    {
        private readonly GetHttpClient getHttpClient;
        private readonly LocalStorageService localStorageService;
        private readonly IUserAccountService userAccountService;
        public CustomHttpHandler(GetHttpClient getHttpClient, LocalStorageService localStorageService, IUserAccountService userAccountService)
        {
            this.getHttpClient = getHttpClient;
            this.localStorageService = localStorageService;
            this.userAccountService = userAccountService;
        }
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            bool loginUrl = request.RequestUri!.AbsoluteUri.Contains("login");
            bool registerUrl = request.RequestUri!.AbsoluteUri.Contains("register");
            bool refreshTokenUrl = request.RequestUri!.AbsoluteUri.Contains("refresh-token");
             
            if (loginUrl || registerUrl || refreshTokenUrl) return await base.SendAsync(request, cancellationToken);

            var result = await base.SendAsync(request, cancellationToken);
            if (result.StatusCode == System.Net.HttpStatusCode.Unauthorized) {
                //Get token from local storage
                var stringToken = await localStorageService.GetToken();
                if(string.IsNullOrWhiteSpace(stringToken)) return result;

                //check if the header containers token
                string? token = string.Empty;
                try { token = request.Headers.Authorization!.Parameter; } catch { }
                var deserializedToken = Serializations.DeseruzlizeJsonString<UserSession>(stringToken);
                if(deserializedToken is null) return result;

                if (string.IsNullOrWhiteSpace(token)) {
                    request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", deserializedToken.Token);
                    return await base.SendAsync(request, cancellationToken);
                }

                //call for refresh token
                var newToken = await GetRefreshToken(deserializedToken.RefreshToken!);
                if(string.IsNullOrWhiteSpace(newToken)) return result;
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", newToken);
                return await base.SendAsync(request, cancellationToken);
            }

            return result;
        }

        private async Task<string> GetRefreshToken(string refreshToken)
        {
            var result = await userAccountService.RefreshTokenAsync(new RefreshToken {Token = refreshToken });
            string serializedToken = Serializations.SerializaObj(new UserSession {Token = result.Token,RefreshToken = result.RefreshToken });
            await localStorageService.SetToken(serializedToken);
            return result.Token;
        }
    }
}
