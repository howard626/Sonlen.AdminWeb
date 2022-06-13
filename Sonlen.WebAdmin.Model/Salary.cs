using Sonlen.WebAdmin.Model.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sonlen.WebAdmin.Model
{
    /// <summary>
    /// 薪資資料
    /// </summary>
    public class Salary
    {
        /// <summary>
        /// 列印員工編號
        /// </summary>
        public string EmployeeID { get; set; } = string.Empty;

        /// <summary>
        /// 支付年月
        /// </summary>
        public string PayMon { get; set; } = string.Empty;

        /// <summary>
        /// 負責專案名稱
        /// </summary>
        public string ProjectName { get; set; } = string.Empty;

        /// <summary>
        /// 匯款帳戶
        /// </summary>
        public string Account { get; set; } = string.Empty;

        /// <summary>
        /// 當月薪水
        /// </summary>
        public int Pay { get; set; }

        /// <summary>
        /// 代扣勞保費
        /// </summary>
        public int Labor { get; set; }

        /// <summary>
        /// 代扣健保費
        /// </summary>
        public int Health { get; set; }

        /// <summary>
        /// 專案獎金
        /// </summary>
        public int Project { get; set; }

        /// <summary>
        /// 駐點獎金
        /// </summary>
        public int Stay { get; set; }

        /// <summary>
        /// 年終獎金(月)
        /// </summary>
        public decimal Year_End { get; set; }

    }

    /// <summary>
    /// 薪資網頁參數
    /// </summary>
    public class SalaryViewModel :Salary
    {
        /// <summary>
        /// 列印年份
        /// </summary>
        [QueryParamIgnore]
        public int Year { get; set; }

        /// <summary>
        /// 列印月份
        /// </summary>
        [QueryParamIgnore]
        public int Month { get; set; }
    }
}
