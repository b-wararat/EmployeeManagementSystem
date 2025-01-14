using BaseLibrary.DTOs;
using BaseLibrary.Responses;
using ClientLibrary.Helpers;
using ClientLibrary.Services.Contracts;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Json;

namespace ClientLibrary.Services.Implementations
{
    public class UserAccountService : IUserAccountService
    {
        private readonly GetHttpClient getHttpClient;
        private readonly NavigationManager navManager;
        public UserAccountService(GetHttpClient getHttpClient, NavigationManager navManager)
        {
            this.getHttpClient = getHttpClient;
            this.navManager = navManager;
        }
        public async Task<GeneralResponse> CreateAsync(Register user)
        {
            var httpClient = getHttpClient.GetPublicHttpClient();
            var result = await httpClient.PostAsJsonAsync($"{Constants.AuthUrl}/register", user);
            if (!result.IsSuccessStatusCode) return new GeneralResponse(false, "Error occured");
            return await result.Content.ReadFromJsonAsync<GeneralResponse>();
        }
        public async Task<LoginResponse> SignInAsync(Login user)
        {
            var httpClient = getHttpClient.GetPublicHttpClient();
            var result = await httpClient.PostAsJsonAsync($"{Constants.AuthUrl}/login", user);
            if (!result.IsSuccessStatusCode) return new LoginResponse(false, "Error occured");
            return await result.Content.ReadFromJsonAsync<LoginResponse>();
        }
        public async Task<WeatherForecast[]> GetWeatherForecast()
        {
            var httpClient = await getHttpClient.GetPrivateHttpClient();
            var result = await httpClient.GetFromJsonAsync<WeatherForecast[]>($"{Constants.WeatherForecastUrl}/");
            return result!;
        }

        public async Task<LoginResponse> RefreshTokenAsync(RefreshToken token)
        {
            var httpClient = getHttpClient.GetPublicHttpClient();
            var result = await httpClient.PostAsJsonAsync($"{Constants.AuthUrl}/refresh-token", token);
            if (!result.IsSuccessStatusCode) return new LoginResponse(false, "Error occured");
            return await result.Content.ReadFromJsonAsync<LoginResponse>();
        }

        public async Task CheckUserAuthentication(Task<AuthenticationState> authenticationState)
        {
            var user = (await authenticationState).User;
            var isLogin = user.Identity!.IsAuthenticated;

            if (isLogin)
            {
                navManager.NavigateTo("/home/dashboard");
            }
            else {
                navManager.NavigateTo("/identity/account/login");
            }
        }
    }
}
