using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sonlen.WebAdmin.Model
{
    /// <summary>
    /// 公司資料
    /// </summary>
    public class Company
    {
        /// <summary>
        /// 統一編號
        /// </summary>
        public string ID { get; set; } = string.Empty;

        /// <summary>
        /// 公司名稱
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 公司地址
        /// </summary>
        public string Address { get; set; } = string.Empty;

        /// <summary>
        /// 公司聯絡人
        /// </summary>
        public string Contact { get; set; } = string.Empty;

        /// <summary>
        /// 公司電話
        /// </summary>
        public string TellPhone { get; set; } = string.Empty;
    }
}
