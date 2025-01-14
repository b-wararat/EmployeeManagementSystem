
using BaseLibrary.DTOs;
using BaseLibrary.Responses;
using Microsoft.AspNetCore.Components.Authorization;

namespace ClientLibrary.Services.Contracts
{
    public interface IUserAccountService
    {
        Task<GeneralResponse> CreateAsync(Register user);
        Task<LoginResponse> SignInAsync(Login user);
        Task<LoginResponse> RefreshTokenAsync(RefreshToken token);
        Task<WeatherForecast[]> GetWeatherForecast();
        Task CheckUserAuthentication(Task<AuthenticationState> authenticationState);
    }
}
