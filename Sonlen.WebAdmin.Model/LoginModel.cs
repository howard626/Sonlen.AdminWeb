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
        [Display(Name = "帳號")]
        [Required(ErrorMessage = "帳號為必填欄位")]
        [EmailAddress(ErrorMessage = "不符合 Email 格式")]
        public string Account { get; set; } = string.Empty;

        [Display(Name = "密碼")]
        [Required(ErrorMessage = "密碼為必填欄位")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        public bool RememberMe { get; set; } = false;
    }

    public class RegisterModel : LoginModel
    {
        [Display(Name = "確認密碼")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "密碼與確認密碼不相符")]
        public string ConfirmPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "員工身分證字號為必填欄位")]
        [Display(Name = "員工身分證字號")]
        public string EmployeeID { get; set; } = string.Empty;

        [Required(ErrorMessage = "員工名稱為必填欄位")]
        [Display(Name = "員工名稱")]
        public string EmployeeName { get; set; } = string.Empty;

        [Display(Name = "員工行動電話號碼")]
        public string CellPhone { get; set; } = string.Empty;

        [Display(Name = "員工生日")]
        public DateTime? Birthday { get; set; }

        [Display(Name = "銀行帳號")]
        public string BankCode { get; set; } = string.Empty;

        [Display(Name = "銀行編號")]
        public string BankAccountNO { get; set; } = string.Empty;

        [Display(Name = "地址")]
        public string Address { get; set; } = string.Empty;

        [Required(ErrorMessage = "性別為必填欄位")]
        [Display(Name = "員工性別")]
        public string Sex { get; set; } = string.Empty;
    }
}
