using Blazored.LocalStorage;
using Blazored.SessionStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;
using static System.Net.WebRequestMethods;

namespace Maew123.Web
{
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly ILocalStorageService localStorageService;
        private readonly ISessionStorageService sessionStorageService;
        private readonly NavigationManager _navigationManager;
        private readonly HttpClient _http;

        public CustomAuthenticationStateProvider(ILocalStorageService localStorageService, HttpClient http, ISessionStorageService sessionstorageService, NavigationManager navigationManager)
        {
            this.localStorageService = localStorageService;
            _http = http;
            sessionStorageService = sessionstorageService;
            this._navigationManager = navigationManager;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                var token = await localStorageService.GetItemAsync<string>("JwtToken");

                var identity = new ClaimsIdentity();
                _http.DefaultRequestHeaders.Authorization = null;

                if (!string.IsNullOrEmpty(token))
                {
                    if (IsTokenExpired(token))
                    {
                        await localStorageService.RemoveItemAsync("JwtToken");

                        _navigationManager.NavigateTo("/login");
                        return new AuthenticationState(new ClaimsPrincipal());
                    }

                    identity = new ClaimsIdentity(ParseClaimsFromJwt(token), "JwtAuth");
                    _http.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Bearer", token.Replace("\"", ""));
                }

                var user = new ClaimsPrincipal(identity);
                var state = new AuthenticationState(user);

                NotifyAuthenticationStateChanged(Task.FromResult(state));

                return state;
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        private IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
        {
            var payload = jwt.Split(".")[1];
            var jsonBytes = ParseBase64WithoutPadding(payload);
            var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);

            var claims = keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString()!));

            return claims;
        }
        private byte[] ParseBase64WithoutPadding(string base64)
        {
            switch (base64.Length % 4)
            {
                case 2: base64 += "=="; break;
                case 3: base64 += "="; break;
            }
            return Convert.FromBase64String(base64);
        }


        private bool IsTokenExpired(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadToken(token) as JwtSecurityToken;

            if (jwtToken == null || jwtToken.ValidTo == null)
            {
                return true;
            }

            return jwtToken.ValidTo <= DateTime.UtcNow;
        }
    }
}
