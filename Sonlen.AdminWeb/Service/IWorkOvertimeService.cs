using Sonlen.WebAdmin.Model;

namespace Sonlen.AdminWeb.Service
{
    public interface IWorkOvertimeService : IConfirmService<WorkOvertimeViewModel>, IDataService<WorkOvertimeViewModel>
    {

        /// <summary>
        /// 從 EmployeeID 取得加班紀錄
        /// </summary>
        /// <returns></returns>
        Task<List<WorkOvertimeViewModel>> GetDataByEIDAsync();
    }
}