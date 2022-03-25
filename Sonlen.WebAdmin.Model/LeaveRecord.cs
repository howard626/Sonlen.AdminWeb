using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sonlen.WebAdmin.Model
{
    /// <summary>
    /// 請假紀錄
    /// </summary>
    public class LeaveRecord
    {
        /// <summary>
        /// 員工編號
        /// </summary>
        public string EmployeeID { get; set; } = string.Empty;

        /// <summary>
        /// 請假日期
        /// </summary>
        public DateTime LeaveDate { get; set; }

        /// <summary>
        /// 請假時間(起)
        /// </summary>
        public string LeaveStartTime { get; set; } = string.Empty;

        /// <summary>
        /// 請假時間(迄)
        /// </summary>
        public string LeaveEndTime { get; set; } = string.Empty;

        /// <summary>
        /// 請假類型編號
        /// </summary>
        public int LeaveType { get; set; }

        /// <summary>
        /// 是否同意編號(0:未確認 -1:不同意 1:同意)
        /// </summary>
        public int Accept { get; set; }

        /// <summary>
        /// 請假時數
        /// </summary>
        [Display(Description = "請假時數")]
        public decimal? LeaveHour { get; set; }

        /// <summary>
        /// 請假證明
        /// </summary>
        [Display(Description = "請假證明")]
        public string? Prove { get; set; }

    }

    /// <summary>
    /// 請假顯示欄位
    /// </summary>
    public class LeaveRecordViewModel : LeaveRecord
    {
        /// <summary>
        /// 請假類型說明
        /// </summary>
        [Display(Description = "請假類型")]
        public string LeaveDesc { get; set; } = string.Empty;

        /// <summary>
        /// 員工名稱
        /// </summary>
        [Display(Description = "員工名稱")]
        public string EmployeeName { get; set; } = string.Empty;

        /// <summary>
        /// 請假日期說明
        /// </summary>
        [Display(Description = "請假日期")]
        public string LeaveDateDesc { get; set; } = string.Empty;

        /// <summary>
        /// 審核狀態說明
        /// </summary>
        [Display(Description = "審核狀態")]
        public string AcceptDesc { get; set; } = string.Empty;
    }
}
