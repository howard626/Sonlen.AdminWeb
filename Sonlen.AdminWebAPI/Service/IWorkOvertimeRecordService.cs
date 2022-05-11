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
    }
}