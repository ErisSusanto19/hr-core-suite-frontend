using HRCoreSuite.Frontend.ViewModels.Auth;
using System.Threading.Tasks;

namespace HRCoreSuite.Frontend.Services
{
    public interface IAuthService
    {
        Task<bool> LoginAsync(LoginViewModel loginViewModel);

        Task LogoutAsync();
    }
}