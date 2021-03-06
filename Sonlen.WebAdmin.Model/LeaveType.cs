using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sonlen.WebAdmin.Model
{
    /// <summary>
    /// 請假類型
    /// </summary>
    public class LeaveType
    {
        /// <summary>
        /// 請假類型編號
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 請假類型名稱
        /// </summary>
        public string LeaveName { get; set; } = string.Empty;

        /// <summary>
        /// 請假類型說明
        /// </summary>
        public string LeaveDesc { get; set; } = string.Empty;

        /// <summary>
        /// 是否支付薪水(0:不支付 1:半薪 2:全薪)
        /// </summary>
        public int Pay { get; set; }

        /// <summary>
        /// 可請假總時數
        /// </summary>
        public int? TotalHour { get; set; }
    }
}
