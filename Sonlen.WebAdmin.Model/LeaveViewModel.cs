using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sonlen.WebAdmin.Model
{
    /// <summary>
    /// 請假
    /// </summary>
    public class LeaveViewModel
    {
        /// <summary>
        /// 員工資料
        /// </summary>
        public Employee? Employee { get; set; }

        /// <summary>
        /// 請假日期
        /// </summary>
        public DateTime LeaveDate { get; set; } = DateTime.Now;

        /// <summary>
        /// 請假類別編號
        /// </summary>
        public int LeaveType { get; set; }

        /// <summary>
        /// 請假時間(起)
        /// </summary>
        public string LeaveStartTime { get; set; } = string.Empty;

        /// <summary>
        /// 請假時間(迄)
        /// </summary>
        public string LeaveEndTime { get; set; } = string.Empty;

        /// <summary>
        /// 請假證明
        /// </summary>
        public UploadFile? File { get; set; }
    }
}
