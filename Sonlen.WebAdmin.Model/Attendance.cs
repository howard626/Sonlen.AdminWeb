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
            Thing_Day_Total = model.Thing_Day_Total;
            Thing_Day = model.Thing_Day;
            Sick_Day = model.Sick_Day;
            Sick_Day_NoPay = model.Sick_Day_NoPay;
            Sick_Day_Pay =  model.Sick_Day_Pay;
            Sick_Day_Total = model.Sick_Day_Total;
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
        /// 當年度事假天數
        /// </summary>
        public decimal Thing_Day_Total { get; set; }

        /// <summary>
        /// 當月事假天數
        /// </summary>        
        public decimal Thing_Day { get; set; }

        /// <summary>
        /// 當年度病假天數
        /// </summary>
        public decimal Sick_Day_Total { get; set; }

        /// <summary>
        /// 當月病假天數
        /// </summary>
        public decimal Sick_Day { get; set; }

        /// <summary>
        /// 當月半薪病假天數
        /// </summary>
        public decimal Sick_Day_Pay { get; set; }

        /// <summary>
        /// 當月不給薪病假天數
        /// </summary>
        public decimal Sick_Day_NoPay { get; set; }

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
        /// 當年度事假天數
        /// </summary>
        [Display(Description = "當年度事假天數")]
        public decimal Thing_Day_Total { get; set; }

        /// <summary>
        /// 當月事假天數
        /// </summary>
        [Display(Description = "當月事假天數")]
        public decimal Thing_Day { get; set; }

        /// <summary>
        /// 當年度病假天數
        /// </summary>
        [Display(Description = "當年度病假天數")]
        public decimal Sick_Day_Total { get; set; }

        /// <summary>
        /// 當月病假天數
        /// </summary>
        [Display(Description = "當月病假天數")]
        public decimal Sick_Day { get; set; }

        /// <summary>
        /// 當月半薪病假天數
        /// </summary>
        [Display(Description = "當月半薪病假天數")]
        public decimal Sick_Day_Pay { get; set; }

        /// <summary>
        /// 當月不給薪病假天數
        /// </summary>
        [Display(Description = "當月不給薪病假天數")]
        public decimal Sick_Day_NoPay { get; set; }
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
