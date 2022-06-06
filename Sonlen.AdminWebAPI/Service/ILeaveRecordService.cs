using Sonlen.WebAdmin.Model;
using System.Data;

namespace Sonlen.AdminWebAPI.Service
{
    public interface ILeaveRecordService : IDataService<LeaveRecord>, IConfirmService<LeaveRecord>
    {
        /// <summary>
        /// 以 EID 取得資料
        /// </summary>
        /// <param name="eid"></param>
        /// <returns></returns>
        IEnumerable<LeaveRecord> GetDatasByEID(string eid);

        /// <summary>
        /// 以月份取得資料
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        IEnumerable<LeaveRecord> GetDatasByDate(AttendanceViewModel model);

        /// <summary>
        /// 以年份取得資料
        /// </summary>
        /// <param name="year"></param>
        /// <param name="employeeID"></param>
        /// <returns></returns>
        IEnumerable<LeaveRecord> GetDatasByYear(int year, string? employeeID = null);

        /// <summary>
        /// 以請假 ID 取得當年度總請假時數
        /// </summary>
        /// <param name="typeID"></param>
        /// <param name="year"></param>
        /// <param name="employeeID"></param>
        /// <returns></returns>
        decimal GetSumHourByID(int typeID, int year = 0, string? employeeID = null);

        /// <summary>
        /// 取得全部資料(包含名字)
        /// </summary>
        /// <returns></returns>
        IEnumerable<LeaveRecordViewModel> GetAllViewDatas();
    }
}