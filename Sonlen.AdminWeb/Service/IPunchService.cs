
namespace Sonlen.AdminWeb.Service
{
    public interface IPunchService
    {
        /// <summary>
        /// 上班打卡
        /// </summary>
        /// <returns></returns>
        Task<string> PunchInAsync();

        /// <summary>
        /// 下班打卡
        /// </summary>
        /// <returns></returns>
        Task<string> PunchOutAsync();
    }
}