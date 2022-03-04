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
        /// 取得請假類別
        /// </summary>
        /// <returns></returns>
        IEnumerable<LeaveType> GetLeaveTypes();

        /// <summary>
        /// 從 EmployeeID 取得請假紀錄
        /// </summary>
        /// <returns></returns>
        IEnumerable<LeaveRecord> GetLeaveRecordByEID(string employeeID);
    }
}