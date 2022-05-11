using Sonlen.WebAdmin.Model;
using System.Data;

namespace Sonlen.AdminWebAPI.Service
{
    public interface ILeaveService
    {
        /// <summary>
        /// 請假
        /// </summary>
        /// <param name="leave"></param>
        /// <returns></returns>
        int DayOff(LeaveViewModel leave);

        /// <summary>
        /// 取消請假
        /// </summary>
        /// <param name="leave"></param>
        /// <returns></returns>
        int CancelDayOff(LeaveRecord leave);

        /// <summary>
        /// 同意請假
        /// </summary>
        /// <param name="leave"></param>
        /// <returns></returns>
        int AgreeLeaveRecord(LeaveRecord leave);

        /// <summary>
        /// 駁回請假
        /// </summary>
        /// <param name="leave"></param>
        /// <returns></returns>
        int DisagreeLeaveRecord(LeaveRecord leave);
    }
}