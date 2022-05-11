using Sonlen.WebAdmin.Model;
using System.Data;

namespace Sonlen.AdminWebAPI.Service
{
    public interface IWorkOvertimeService
    {

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