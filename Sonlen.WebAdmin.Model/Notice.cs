using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sonlen.WebAdmin.Model
{
    /// <summary>
    /// 通知
    /// </summary>
    public class Notice
    {
        /// <summary>
        /// 通知編號
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 通知內容
        /// </summary>
        public string Content { get; set; } = string.Empty;

        /// <summary>
        /// 通知員工編號
        /// </summary>
        public string EmployeeID { get; set; } = string.Empty;

        /// <summary>
        /// 是否已讀
        /// </summary>
        public int IsRead { get; set; }

        /// <summary>
        /// 通知創建日期
        /// </summary>
        public DateTime CreateDate { get; set; } = DateTime.Now;
    }
}
