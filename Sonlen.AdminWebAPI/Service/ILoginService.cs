using Sonlen.WebAdmin.Model;

namespace Sonlen.AdminWebAPI.Service
{
    public interface ILoginService
    {

        /// <summary>
        /// 依帳號密碼取得員工資料
        /// </summary>
        /// <param name="account"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        Employee? Login(string account, string password);

        /// <summary>
        /// 設定重設密碼認證碼
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        void SetResetPasswordToken(string email, string token);

        /// <summary>
        /// 重設密碼
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        string ResetPassword(ResetPasswordModel model);

        /// <summary>
        /// 註冊新員工
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        string Register(RegisterModel item);
    }
}