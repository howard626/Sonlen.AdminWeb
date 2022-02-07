using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sonlen.WebAdmin.Model
{
    public class LoginModel
    {
        [Required(ErrorMessage = "帳號為必填欄位")]
        [EmailAddress(ErrorMessage = "不符合 Email 格式")]
        public string Account { get; set; } = string.Empty;

        [Required(ErrorMessage = "密碼為必填欄位")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        public bool RememberMe { get; set; } = false;
    }
}
