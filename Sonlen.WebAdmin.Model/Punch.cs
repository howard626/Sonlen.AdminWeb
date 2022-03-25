using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sonlen.WebAdmin.Model
{
    /// <summary>
    /// 打卡
    /// </summary>
    public class Punch
    {
        /// <summary>
        /// 員工編號
        /// </summary>
        public string EmployeeID { get; set; } = String.Empty;

        /// <summary>
        /// 打卡日期
        /// </summary>
        public DateTime PunchDate { get; set; }

        /// <summary>
        /// 打卡時間(上班)
        /// </summary>
        public string PunchInTime { get; set; } = String.Empty;

        /// <summary>
        /// 打卡時間(下班)
        /// </summary>
        public string? PunchOutTime { get; set; }

        /// <summary>
        /// 工作時數
        /// </summary>
        public decimal? WorkHour { get; set; }
    }
}
