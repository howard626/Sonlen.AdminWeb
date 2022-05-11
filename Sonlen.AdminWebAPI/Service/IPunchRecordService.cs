using Sonlen.WebAdmin.Model;

namespace Sonlen.AdminWebAPI.Service
{
    public interface IPunchRecordService : IDataService<Punch>
    {
        /// <summary>
        /// 以 EID 取得資料
        /// </summary>
        /// <param name="eid"></param>
        /// <returns></returns>
        IEnumerable<Punch> GetDatasByEID(string eid);
    }
}