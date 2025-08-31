using HRCoreSuite.Frontend.ViewModels.Auth;
using HRCoreSuite.Frontend.ViewModels.Common;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Net.Http.Json;
using System.Security.Claims;

namespace HRCoreSuite.Frontend.Services
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<AuthService> _logger;

        public AuthService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor, ILogger<AuthService> logger)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        public async Task<bool> LoginAsync(LoginViewModel loginViewModel)
        {
            var requestDto = new LoginRequestDto
            {
                Username = loginViewModel.Username,
                Password = loginViewModel.Password
            };

            try
            {
                var response = await _httpClient.PostAsJsonAsync("/api/auth/login", requestDto);
                if (!response.IsSuccessStatusCode) return false;

                var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<LoginResponseDto>>();
                var loginData = apiResponse?.Data;
                if (loginData == null || string.IsNullOrEmpty(loginData.Token)) return false;

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, loginData.UserName),
                    new Claim("access_token", loginData.Token)
                };
                loginData.Roles.ForEach(role => claims.Add(new Claim(ClaimTypes.Role, role)));

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties { IsPersistent = true };

                await _httpContextAccessor.HttpContext!.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception during login for user {Username}.", loginViewModel.Username);
                return false;
            }
        }

        public async Task LogoutAsync()
        {
            await _httpContextAccessor.HttpContext!.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }
    }
}