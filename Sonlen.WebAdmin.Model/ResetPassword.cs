using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sonlen.WebAdmin.Model
{
    /// <summary>
    /// 重設密碼
    /// </summary>
    public class ResetPassword
    {
        /// <summary>
        /// 重設密碼之電子信箱
        /// </summary>
        [Key]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// 驗證碼
        /// </summary>
        public string Token { get; set; } = string.Empty;
    }
}
