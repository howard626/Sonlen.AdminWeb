using Sonlen.WebAdmin.Model;
using System.Data;

namespace Sonlen.AdminWebAPI.Service
{
    public interface ILeaveService
    {
        IDbConnection Connection { get; }

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
        /// 取得請假類別
        /// </summary>
        /// <returns></returns>
        IEnumerable<LeaveType> GetLeaveTypes();

        /// <summary>
        /// 取得請假類別
        /// </summary>
        /// <returns></returns>
        LeaveType GetLeaveType(int id);

        /// <summary>
        /// 從 EmployeeID 取得請假紀錄
        /// </summary>
        /// <returns></returns>
        IEnumerable<LeaveRecord> GetLeaveRecordByEID(string employeeID);

        /// <summary>
        /// 取得全部請假紀錄
        /// </summary>
        /// <returns></returns>
        IEnumerable<LeaveRecordViewModel> GetAllLeaveRecord();

        /// <summary>
        /// 同意請假
        /// </summary>
        /// <param name="employeeID"></param>
        /// <param name="leaveDate"></param>
        /// <returns></returns>
        int AgreeLeaveRecord(string employeeID, DateTime leaveDate);

        /// <summary>
        /// 駁回請假
        /// </summary>
        /// <param name="employeeID"></param>
        /// <param name="leaveDate"></param>
        /// <returns></returns>
        int DisagreeLeaveRecord(string employeeID, DateTime leaveDate);

        /// <summary>
        /// 取得請假證明檔案
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        UploadFile GetLeaveProve(UploadFile file);
    }
}