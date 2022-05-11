using Sonlen.WebAdmin.Model;

namespace Sonlen.AdminWebAPI.Service
{
    public interface INoticeService : IDataService<Notice>
    {
        /// <summary>
        /// 以 EID 取得資料
        /// </summary>
        /// <param name="eid"></param>
        /// <returns></returns>
        IEnumerable<Notice> GetDatasByEID(string eid);

        /// <summary>
        /// 設定通知為已讀
        /// </summary>
        /// <param name="notices"></param>
        /// <returns></returns>
        int SetNoticeToIsRead(List<Notice> notices);
    }
}