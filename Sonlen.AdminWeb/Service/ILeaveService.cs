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
        /// <param name="leave"></param>
        /// <returns></returns>
        Task<string> LeaveOffAsync(LeaveRecord leave);

        /// <summary>
        /// 取得請假類別
        /// </summary>
        /// <returns></returns>
        Task<List<LeaveType>> GetLeaveTypesAsync();

        /// <summary>
        /// 取得全部請假紀錄
        /// </summary>
        /// <returns></returns>
        Task<List<LeaveRecordViewModel>> GetAllLeaveRecordAsync();

        /// <summary>
        /// 從 EmployeeID 取得請假紀錄
        /// </summary>
        /// <returns></returns>
        Task<List<LeaveRecordViewModel>> GetLeaveRecordByEIDAsync();

        /// <summary>
        /// 取得請假證明
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        Task<UploadFile> GetLeaveProveAsync(string fileName);

        /// <summary>
        /// 同意請假
        /// </summary>
        /// <param name="leave"></param>
        /// <returns></returns>
        Task<string> AgreeLeaveRecordAsync(LeaveRecordViewModel leave);

        /// <summary>
        /// 駁回請假
        /// </summary>
        /// <param name="leave"></param>
        /// <returns></returns>
        Task<string> DisagreeLeaveRecordAsync(LeaveRecordViewModel leave);
    }
}