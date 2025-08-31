using System.Security.Claims;
using HRCoreSuite.Frontend.Services;
using HRCoreSuite.Frontend.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HRCoreSuite.Frontend.Pages
{
    [Microsoft.AspNetCore.Authorization.AllowAnonymous] 
    public class LoginModel : PageModel
    {
        private readonly ApiClient _apiClient;

        [BindProperty]
        public LoginViewModel Input { get; set; } = new();

        public string? ReturnUrl { get; set; }

        public LoginModel(ApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<IActionResult> OnGetAsync(string? returnUrl = null)
        {
            if (User.Identity?.IsAuthenticated ?? false)
            {
                return LocalRedirect("/");
            }

            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            ReturnUrl = returnUrl;
            
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
        {
            ReturnUrl = returnUrl;
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var loginResponse = await _apiClient.LoginAsync(Input.Username, Input.Password);

            if (loginResponse == null || string.IsNullOrEmpty(loginResponse.Token))
            {
                ModelState.AddModelError(string.Empty, "Login gagal. Periksa kembali username dan password Anda.");
                return Page();
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, loginResponse.UserName),
                new Claim(ClaimTypes.Email, loginResponse.Email),
                new Claim("access_token", loginResponse.Token)
            };

            foreach (var role in loginResponse.Roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(60)
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            return LocalRedirect(Url.IsLocalUrl(ReturnUrl) ? ReturnUrl : "/");
        }
    }
}