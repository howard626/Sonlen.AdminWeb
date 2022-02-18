using Sonlen.WebAdmin.Model;

namespace Sonlen.AdminWeb.Service
{
    public interface IEmployeeService
    {
        /// <summary>
        /// 新增員工
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>
        Task<string> AddEmployeeAsync(Employee employee);

        /// <summary>
        /// 刪除員工
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>
        Task<string> DeleteEmployeeAsync(Employee employee);

        /// <summary>
        /// 取得全部員工資料
        /// </summary>
        /// <returns></returns>
        Task<List<Employee>> GetAllEmployeeAsync();

        /// <summary>
        /// 以身分證字號取得單一員工資料
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Employee?> GetEmployeeAsync(string id);

        /// <summary>
        /// 更新員工資料
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>
        Task<string> UpdateEmployeeAsync(Employee employee);
    }
}