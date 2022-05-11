using Sonlen.WebAdmin.Model;

namespace Sonlen.AdminWeb.Service
{
    public interface IEmployeeService : IDataService<Employee>
    {
        /// <summary>
        /// 取得員工通知
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>
        Task<List<Notice>> GetEmployeeNoticeAsync(Employee employee);

        /// <summary>
        /// 設定通知為已讀
        /// </summary>
        /// <param name="notices"></param>
        /// <returns></returns>
        Task<string> SetNoticeToIsReadAsync(List<Notice> notices);
    }
}