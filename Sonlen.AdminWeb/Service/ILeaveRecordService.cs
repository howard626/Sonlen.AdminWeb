using Sonlen.WebAdmin.Model;

namespace Sonlen.AdminWeb.Service
{
    public interface ILeaveRecordService : IConfirmService<LeaveRecordViewModel>, IDataService<LeaveRecordViewModel>
    {
        /// <summary>
        /// 請假
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<string> LeaveOnAsync(LeaveViewModel model);

        /// <summary>
        /// 從 EmployeeID 取得請假紀錄
        /// </summary>
        /// <returns></returns>
        Task<List<LeaveRecordViewModel>> GetDataByEIDAsync();

        /// <summary>
        /// 取得請假證明
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        Task<UploadFile> GetLeaveProveAsync(string fileName);
    }
}