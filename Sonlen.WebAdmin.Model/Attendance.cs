using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sonlen.WebAdmin.Model
{
    /// <summary>
    /// 考勤資料
    /// </summary>
    public class Attendance
    {
        public Attendance(){ }

        public Attendance(AttendancePrintModel model)
        {
            EmployeeID = model.EmployeeID;
            Mon_Day = model.Mon_Day;
            Set_Day = model.Set_Day;
            Thing_Hour_Total = model.Thing_Hour_Total;
            Thing_Hour = model.Thing_Hour;
            Sick_Hour = model.Sick_Hour;
            Sick_Hour_NoPay = model.Sick_Hour_NoPay;
            Sick_Hour_Pay =  model.Sick_Hour_Pay;
            Sick_Hour_Total = model.Sick_Hour_Total;
        }

        /// <summary>
        /// 員工編號(工號)
        /// </summary>
        public string EmployeeID { get; set; } = string.Empty;

        /// <summary>
        /// 當月天數
        /// </summary>
        public int Mon_Day { get; set; }

        /// <summary>
        /// 當月應出勤天數
        /// </summary>
        public int Set_Day { get; set; }

        /// <summary>
        /// 當年度事假時數
        /// </summary>
        public decimal Thing_Hour_Total { get; set; }

        /// <summary>
        /// 當月事假時數
        /// </summary>        
        public decimal Thing_Hour { get; set; }

        /// <summary>
        /// 當年度病假時數
        /// </summary>
        public decimal Sick_Hour_Total { get; set; }

        /// <summary>
        /// 當月病假時數
        /// </summary>
        public decimal Sick_Hour { get; set; }

        /// <summary>
        /// 當月半薪病假時數
        /// </summary>
        public decimal Sick_Hour_Pay { get; set; }

        /// <summary>
        /// 當月不給薪病假時數
        /// </summary>
        public decimal Sick_Hour_NoPay { get; set; }

        /// <summary>
        /// 當月月份
        /// </summary>
        public string LogMon { get; set; } = string.Empty;
    }

    /// <summary>
    /// 考勤列印資料
    /// </summary>
    public class AttendancePrintModel
    {
        /// <summary>
        /// 員工編號(工號)
        /// </summary>
        [Display(Description = "工號")]
        public string EmployeeID { get; set; } = string.Empty;

        /// <summary>
        /// 員工名字
        /// </summary>
        [Display(Description = "姓名")]
        public string EmployeeName { get; set; } = string.Empty;

        /// <summary>
        /// 員工到職日期
        /// </summary>
        [Display(Description = "到職日")]
        public DateTime? ArrivalDate { get; set; }

        /// <summary>
        /// 當月天數
        /// </summary>
        [Display(Description = "當月天數")]
        public int Mon_Day { get; set; }

        /// <summary>
        /// 當月應出勤天數
        /// </summary>
        [Display(Description = "當月應出勤天數")]
        public int Set_Day { get; set; }

        /// <summary>
        /// 當年度事假時數
        /// </summary>
        [Display(Description = "當年度事假時數")]
        public decimal Thing_Hour_Total { get; set; }

        /// <summary>
        /// 當月事假時數
        /// </summary>
        [Display(Description = "當月事假時數")]
        public decimal Thing_Hour { get; set; }

        /// <summary>
        /// 當年度病假時數
        /// </summary>
        [Display(Description = "當年度病假時數")]
        public decimal Sick_Hour_Total { get; set; }

        /// <summary>
        /// 當月病假時數
        /// </summary>
        [Display(Description = "當月病假時數")]
        public decimal Sick_Hour { get; set; }

        /// <summary>
        /// 當月半薪病假時數
        /// </summary>
        [Display(Description = "當月半薪病假時數")]
        public decimal Sick_Hour_Pay { get; set; }

        /// <summary>
        /// 當月不給薪病假時數
        /// </summary>
        [Display(Description = "當月不給薪病假時數")]
        public decimal Sick_Hour_NoPay { get; set; }
    }


    /// <summary>
    /// 考勤列印參數
    /// </summary>
    public class AttendanceViewModel
    {
        /// <summary>
        /// 列印年份
        /// </summary>
        public int Year { get; set; }

        /// <summary>
        /// 列印月份
        /// </summary>
        public int Month { get; set; }

        /// <summary>
        /// 列印員工編號
        /// </summary>
        public string EmployeeID { get; set; } = string.Empty;
    }
}
