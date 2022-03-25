using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sonlen.WebAdmin.Model
{
    /// <summary>
    /// 設定檔
    /// </summary>
    public class Setting
    {
        /// <summary>
        /// API 網址
        /// </summary>
        public static readonly string API_ADDRESS = "http://localhost:5149/api/";

        /// <summary>
        /// 請假審核人員工編號
        /// </summary>
        public static readonly string LEAVE_APPROVED_ID = "A123456789";
    }
}
