using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sonlen.WebAdmin.Model
{
    /// <summary>
    /// 加班
    /// </summary>
    public class WorkOvertime
    {
        public WorkOvertime() { }
        public WorkOvertime(WorkOvertimeViewModel over)
        {
            EmployeeID = over.EmployeeID;
            OverDate = over.OverDate;
            Project = over.Project;
            Pay_133 = over.Pay_133;
            Pay_166 = over.Pay_166;
            Pay_200 = over.Pay_200;
        }
        /// <summary>
        /// 員工身分證字號
        /// </summary>
        [Display(Description = "員工身分證字號")]
        public string EmployeeID { get; set; } = String.Empty;

        /// <summary>
        /// 加班日期
        /// </summary>
        [Display(Description = "加班日期")]
        public DateTime OverDate { get; set; } = DateTime.Now;

        /// <summary>
        /// 加班項目
        /// </summary>
        [Display(Description = "加班項目")]
        public string Project { get; set; } = String.Empty;

        /// <summary>
        /// 是否核准(0:未審核 1:核准 -1:不核准)
        /// </summary>
        public int Accept { get; set; }

        /// <summary>
        /// 加班時數(1.33)
        /// </summary>
        [Display(Description = "加班時數(1.33)")]
        public decimal Pay_133 { get; set; }

        /// <summary>
        /// 加班時數(1.66)
        /// </summary>
        [Display(Description = "加班時數(1.66)")]
        public decimal Pay_166 { get; set; }

        /// <summary>
        /// 加班時數(2.00)
        /// </summary>
        [Display(Description = "加班時數(2.00)")]
        public decimal Pay_200 { get; set; }

        /// <summary>
        /// 通知編號
        /// </summary>
        public int NoticeId { get; set; }

    }

    public class WorkOvertimeViewModel : WorkOvertime
    {
        public WorkOvertimeViewModel() { }
        public WorkOvertimeViewModel(WorkOvertime over)
        {
            EmployeeID = over.EmployeeID;
            OverDate = over.OverDate;
            Project = over.Project;
            Pay_133 = over.Pay_133;
            Pay_166 = over.Pay_166;
            Pay_200 = over.Pay_200;
        }

        /// <summary>
        /// 員工名稱
        /// </summary>
        [Display(Description = "員工名稱")]
        public string EmployeeName { get; set; } = string.Empty;

        /// <summary>
        /// 審核狀態說明
        /// </summary>
        [Display(Description = "審核狀態")]
        public string AcceptDesc { get; set; } = string.Empty;
    }

}
