using Sonlen.WebAdmin.Model;

namespace Sonlen.AdminWeb.Service
{
    public interface ILoginService
    {
        Task<bool> LoginAsync(LoginModel userInfo);
        Task LogoutAsync();
    }
}