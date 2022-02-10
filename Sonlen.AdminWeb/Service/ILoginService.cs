using Sonlen.WebAdmin.Model;

namespace Sonlen.AdminWeb.Service
{
    public interface ILoginService
    {
        /// <summary>
        /// 登入
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        Task<string> LoginAsync(LoginModel userInfo);

        /// <summary>
        /// 登出
        /// </summary>
        /// <returns></returns>
        Task LogoutAsync();

        /// <summary>
        /// 註冊
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        Task<string> RegisterAsync(RegisterModel userInfo);

        /// <summary>
        /// 忘記密碼
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        Task<bool> ForgotPasswordAsync(LoginModel userInfo);

        /// <summary>
        /// 重設密碼
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        Task<bool> ResetPasswordAsync(ResetPasswordModel userInfo);

        /// <summary>
        /// 取得員工資料
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        Task<Employee?> GetEmployeeAsync(string id);

        /// <summary>
        /// 取得全部員工資料
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        Task<List<Employee>> GetAllEmployeeAsync();
    }
}