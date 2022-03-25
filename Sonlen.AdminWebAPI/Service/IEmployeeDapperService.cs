using Sonlen.WebAdmin.Model;
using System.Data;

namespace Sonlen.AdminWebAPI.Service
{
    public interface IEmployeeDapperService
    {
        IDbConnection Connection { get; }

        /// <summary>
        /// 新增員工
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>
        int AddEmployee(Employee employee);

        /// <summary>
        /// 檢查是否已存在此 email
        /// </summary>
        /// <param name="email"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        bool CheckEmail(string email, string id);

        /// <summary>
        /// 刪除員工
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>
        int DeleteEmployee(Employee employee);

        /// <summary>
        /// 取得全部員工資料
        /// </summary>
        /// <returns></returns>
        IEnumerable<Employee> GetAllEmployees();

        /// <summary>
        /// 以 email 取得員工資料
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        Employee GetEmployeeByEmail(string email);

        /// <summary>
        /// 以 身分證字號 取得員工資料
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Employee GetEmployeeById(string id);

        /// <summary>
        /// 更新員工資料
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>
        int UpdateEmployee(Employee employee);

        /// <summary>
        /// 取得員工通知
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>
        IEnumerable<Notice> GeNoticeByEID(Employee employee);

        /// <summary>
        /// 設定通知為已讀
        /// </summary>
        /// <param name="notices"></param>
        /// <returns></returns>
        int SetNoticeToIsRead(List<Notice> notices);
    }
}