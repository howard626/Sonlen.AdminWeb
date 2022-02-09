using Sonlen.WebAdmin.Model;

namespace Sonlen.AdminWebAPI.Service
{
    public interface ILoginService
    {
        /// <summary>
        /// 取得員工列表
        /// </summary>
        /// <returns></returns>
        List<Employee> GetAllEmployees();

        /// <summary>
        /// 依帳號密碼取得員工資料
        /// </summary>
        /// <returns></returns>
        Employee? Login(string account, string password);

        /// <summary>
        /// 註冊新員工
        /// </summary>
        /// <returns></returns>
        string Register(RegisterModel model);

        /// <summary>
        /// 取得員工資料
        /// </summary>
        /// <returns></returns>
        public Employee? GetEmployee(string id);
    }
}