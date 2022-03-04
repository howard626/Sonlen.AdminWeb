using Sonlen.WebAdmin.Model;

namespace Sonlen.AdminWeb.Service
{
    public interface ILeaveService
    {
        /// <summary>
        /// 請假
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<string> LeaveOnAsync(LeaveViewModel model);

        /// <summary>
        /// 取消請假
        /// </summary>
        /// <param name="leaveDate"></param>
        /// <param name="leaveType"></param>
        /// <returns></returns>
        Task<string> LeaveOffAsync(DateTime leaveDate);

        /// <summary>
        /// 取得請假類別
        /// </summary>
        /// <returns></returns>
        Task<List<LeaveType>> GetLeaveTypesAsync();

        /// <summary>
        /// 從 EmployeeID 取得請假紀錄
        /// </summary>
        /// <returns></returns>
        Task<List<LeaveRecordViewModel>> GetLeaveRecordByEIDAsync();
    }
}