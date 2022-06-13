using Sonlen.WebAdmin.Model;

namespace Sonlen.AdminWebAPI.Service
{
    public interface IWorkOvertimeRecordService : IDataService<WorkOvertime>, IConfirmService<WorkOvertime>
    {

        /// <summary>
        /// 取得全部資料(包含名字)
        /// </summary>
        /// <returns></returns>
        IEnumerable<WorkOvertimeViewModel> GetAllViewDatas();

        /// <summary>
        /// 以 EID 取得資料
        /// </summary>
        /// <param name="eid"></param>
        /// <returns></returns>
        IEnumerable<WorkOvertime> GetDatasByEID(string eid);

        /// <summary>
        /// 以年月取得當月加班1.33時數
        /// </summary>
        /// <param name="employeeID"></param>
        /// <param name="logMon"></param>
        /// <returns></returns>
        decimal GetSum133HourByDate(string employeeID, string logMon);

        /// <summary>
        /// 以年月取得當月加班1.66時數
        /// </summary>
        /// <param name="employeeID"></param>
        /// <param name="logMon"></param>
        /// <returns></returns>
        decimal GetSum166HourByDate(string employeeID, string logMon);

        /// <summary>
        /// 以年月取得當月加班2.00時數
        /// </summary>
        /// <param name="employeeID"></param>
        /// <param name="logMon"></param>
        /// <returns></returns>
        decimal GetSum200HourByDate(string employeeID, string logMon);
    }
}