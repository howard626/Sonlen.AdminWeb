using Sonlen.WebAdmin.Model;

namespace Sonlen.AdminWeb.Service
{
    public interface IWorkOvertimeService
    {
        /// <summary>
        /// 申請加班
        /// </summary>
        /// <param name="overtime"></param>
        /// <returns></returns>
        Task<string> AddWorkOvertimeAsync(WorkOvertime overtime);

        /// <summary>
        /// 核准加班
        /// </summary>
        /// <param name="overtime"></param>
        /// <returns></returns>
        Task<string> AgreeWorkOvertimeAsync(WorkOvertimeViewModel overtime);

        /// <summary>
        /// 取消申請加班
        /// </summary>
        /// <param name="overtime"></param>
        /// <returns></returns>
        Task<string> CancelWorkOvertimeAsync(WorkOvertimeViewModel overtime);

        /// <summary>
        /// 駁回加班
        /// </summary>
        /// <param name="overtime"></param>
        /// <returns></returns>
        Task<string> DisagreeWorkOvertimeAsync(WorkOvertimeViewModel overtime);

        /// <summary>
        /// 取得所有加班記錄
        /// </summary>
        /// <returns></returns>
        Task<List<WorkOvertimeViewModel>> GetAllWorkOvertimeAsync();

        /// <summary>
        /// 從 EmployeeID 取得加班紀錄
        /// </summary>
        /// <returns></returns>
        Task<List<WorkOvertimeViewModel>> GetOvertimeRecordByEIDAsync();
    }
}