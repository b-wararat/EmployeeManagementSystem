using BaseLibrary.DTOs;
using Microsoft.AspNetCore.Components.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ClientLibrary.Helpers
{
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly LocalStorageService localStorageService;
        private readonly ClaimsPrincipal anonymous = new(new ClaimsIdentity());
        public CustomAuthenticationStateProvider(LocalStorageService localStorageService)
        {
            this.localStorageService = localStorageService;
        }
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var stringToken = await localStorageService.GetToken();
            if (string.IsNullOrWhiteSpace(stringToken)) return await Task.FromResult(new AuthenticationState(anonymous));

            var deserizlizetToken = Serializations.DeseruzlizeJsonString<UserSession>(stringToken);
            if(deserizlizetToken == null) return await Task.FromResult(new AuthenticationState(anonymous));

            var getUserClaims = DecryptToken(deserizlizetToken.Token!);
            if(getUserClaims is null) return await Task.FromResult(new AuthenticationState(anonymous));

            var claimsprincipal = SetClaimPrincipal(getUserClaims);
            return await Task.FromResult(new AuthenticationState(claimsprincipal));
        }

        public async Task UpdateAuthneticationState(UserSession userSession) {
            var claimsPrincipal = new ClaimsPrincipal();
            if (!string.IsNullOrWhiteSpace(userSession.Token) || !string.IsNullOrWhiteSpace(userSession.RefreshToken))
            {
                var serializeSesstion = Serializations.SerializaObj(userSession);
                await localStorageService.SetToken(serializeSesstion);
                var getUserClaims = DecryptToken(userSession.Token!);
                claimsPrincipal = SetClaimPrincipal(getUserClaims);
            }
            else {
                await localStorageService.RemoveToken(); //when logout
            }
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claimsPrincipal)));
        }

        private static CustomUserClaims DecryptToken(string jwtToken)
        {
            if(string.IsNullOrWhiteSpace(jwtToken)) return new CustomUserClaims();

            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(jwtToken);
            if(token is null) return new CustomUserClaims();

            var userId = token.Claims.FirstOrDefault(w => w.Type == ClaimTypes.NameIdentifier);
            var name = token.Claims.FirstOrDefault(w => w.Type == ClaimTypes.Name);
            var email = token.Claims.FirstOrDefault(w => w.Type == ClaimTypes.Email);
            var role = token.Claims.FirstOrDefault(w => w.Type == ClaimTypes.Role);
            return new CustomUserClaims(userId!.Value, name!.Value, email!.Value, role!.Value);
        }
        private static ClaimsPrincipal SetClaimPrincipal(CustomUserClaims claims)
        {
            if (string.IsNullOrWhiteSpace(claims.Email)) return new ClaimsPrincipal();
            return new ClaimsPrincipal(new ClaimsIdentity(
                new List<Claim> 
                {
                    new(ClaimTypes.NameIdentifier, claims.Id!),
                    new(ClaimTypes.Name, claims.Name!),
                    new(ClaimTypes.Email, claims.Email!),
                    new(ClaimTypes.Role, claims.Role!)
                }, "JwtAuth"));
        }

        
    }
}
