using HRCoreSuite.Frontend.Services;
using HRCoreSuite.Frontend.ViewModels.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HRCoreSuite.Frontend.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly IAuthService _authService;

        [BindProperty]
        public LoginViewModel Input { get; set; } = new();

        public LoginModel(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            if (User.Identity?.IsAuthenticated ?? false)
            {
                return LocalRedirect("/");
            }
            await _authService.LogoutAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
        {
            if (!ModelState.IsValid) return Page();

            var success = await _authService.LoginAsync(Input);
            if (!success)
            {
                ModelState.AddModelError(string.Empty, "Login gagal.");
                return Page();
            }

            return LocalRedirect(returnUrl ?? "/");
        }
    }
}