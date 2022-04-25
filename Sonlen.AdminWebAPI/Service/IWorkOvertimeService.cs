using Sonlen.WebAdmin.Model;
using System.Data;

namespace Sonlen.AdminWebAPI.Service
{
    public interface IWorkOvertimeService
    {
        IDbConnection Connection { get; }

        /// <summary>
        /// 取得全部加班紀錄
        /// </summary>
        /// <returns></returns>
        IEnumerable<WorkOvertimeViewModel> GetAllWorkOvertime();

        /// <summary>
        /// 從 EmployeeID 取得加班紀錄
        /// </summary>
        /// <returns></returns>
        IEnumerable<WorkOvertime> GetWorkOvertimeByEID(string employeeID);

        /// <summary>
        /// 申請加班
        /// </summary>
        /// <param name="overtime"></param>
        /// <returns></returns>
        int AddWorkOvertime(WorkOvertimeViewModel overtime);

        /// <summary>
        /// 取消申請加班
        /// </summary>
        /// <param name="overtime"></param>
        /// <returns></returns>
        int CancelWorkOvertime(WorkOvertime overtime);

        /// <summary>
        /// 同意申請加班
        /// </summary>
        /// <param name="overtime"></param>
        /// <returns></returns>
        int AgreeWorkOvertime(WorkOvertime overtime);

        /// <summary>
        /// 駁回申請加班
        /// </summary>
        /// <param name="overtime"></param>
        /// <returns></returns>
        int DisagreeWorkOvertime(WorkOvertime overtime);
    }
}