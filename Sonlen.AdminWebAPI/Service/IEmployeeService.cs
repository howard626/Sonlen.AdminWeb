using Sonlen.WebAdmin.Model;

namespace Sonlen.AdminWebAPI.Service
{
    public interface IEmployeeService : IDataService<Employee>
    {
        /// <summary>
        /// 以帳號取得單一員工資料
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        Employee? GetDataByEmail(string email);

        /// <summary>
        /// 檢查 Email 是否有重複
        /// </summary>
        /// <param name="email"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        bool CheckEmail(string email, string id);
    }
}