using Sonlen.WebAdmin.Model;
using System.Data;

namespace Sonlen.AdminWebAPI.Service
{
    public interface IPunchService
    {
        /// <summary>
        /// 上班打卡
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>
        int PunchIn(Employee employee);

        /// <summary>
        /// 下班打卡
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>
        int PunchOut(Employee employee);
    }
}