using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Client.Providers;

public class AppAuthenticationStateProvider(ILocalStorageService localStorageService) : AuthenticationStateProvider
{
    private readonly ILocalStorageService localStorageService = localStorageService;
    private readonly JwtSecurityTokenHandler jwtSecurityTokenHandler = new();
    internal const string localstorageBearerTokenName = "bearerToken";

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        try
        {
            string savedToken = await localStorageService.GetItemAsync<string>(localstorageBearerTokenName);

            if (string.IsNullOrWhiteSpace(savedToken))
            {
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }

            JwtSecurityToken jwtSecurityToken = jwtSecurityTokenHandler.ReadJwtToken(savedToken);
            DateTime expiry = jwtSecurityToken.ValidTo;

            if (expiry == DateTime.UtcNow)
            {
                await localStorageService.RemoveItemAsync(localstorageBearerTokenName);
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }

            IList<Claim> claims = ParseClaims(jwtSecurityToken);
            ClaimsPrincipal user = new(new ClaimsIdentity(claims, "jwt"));

            return new AuthenticationState(user);
        }
        catch (System.Exception)
        {
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }
    }

    internal async Task SignIn()
    {
        string savedToken = await localStorageService.GetItemAsync<string>(localstorageBearerTokenName);
        JwtSecurityToken jwtSecurityToken = jwtSecurityTokenHandler.ReadJwtToken(savedToken);

        IList<Claim> claims = ParseClaims(jwtSecurityToken);
        ClaimsPrincipal user = new(new ClaimsIdentity(claims, "jwt"));

        Task<AuthenticationState> authenticationState = Task.FromResult(new AuthenticationState(user));
        NotifyAuthenticationStateChanged(authenticationState);
    }

    internal void SignOut()
    {
        ClaimsPrincipal claimsPrincipal = new(new ClaimsIdentity());
        Task<AuthenticationState> authenticationState = Task.FromResult(new AuthenticationState(claimsPrincipal));
        NotifyAuthenticationStateChanged(authenticationState);
    }

    private IList<Claim> ParseClaims(JwtSecurityToken securityToken)
    {
        IList<Claim> claims = [.. securityToken.Claims];

        claims.Add(new Claim(ClaimTypes.Name, securityToken.Subject));

        return claims;
    }
}
