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
        /// 取得全部資料(包含名字)
        /// </summary>
        /// <returns></returns>
        IEnumerable<LeaveRecordViewModel> GetAllViewDatas();
    }
}