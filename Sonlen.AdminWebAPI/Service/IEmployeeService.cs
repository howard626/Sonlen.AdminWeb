using Sonlen.WebAdmin.Model;

namespace Sonlen.AdminWebAPI.Service
{
    public interface IEmployeeService
    {
        /// <summary>
        /// 新增員工
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>
        int AddEmployee(Employee employee);

        /// <summary>
        /// 刪除員工
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>
        int DeleteEmployee(Employee employee);

        /// <summary>
        /// 更新員工資料
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>
        int UpdateEmployee(Employee employee);

        /// <summary>
        /// 取得全部員工資料
        /// </summary>
        /// <returns></returns>
        List<Employee> GetAllEmployees();

        /// <summary>
        /// 以帳號取得單一員工資料
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        Employee? GetEmployeeByEmail(string email);

        /// <summary>
        /// 以身分證字號取得單一員工資料
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Employee? GetEmployeeById(string id);

        /// <summary>
        /// 檢查 Email 是否有重複
        /// </summary>
        /// <param name="email"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        bool CheckEmail(string email, string id);
    }
}