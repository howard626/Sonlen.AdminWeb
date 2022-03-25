using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sonlen.WebAdmin.Model
{
    /// <summary>
    /// 登入
    /// </summary>
    public class LoginModel
    {
        /// <summary>
        /// 帳號
        /// </summary>
        [Display(Name = "帳號")]
        [Required(ErrorMessage = "帳號為必填欄位")]
        [EmailAddress(ErrorMessage = "不符合 Email 格式")]
        public string Account { get; set; } = string.Empty;

        /// <summary>
        /// 密碼
        /// </summary>
        [Display(Name = "密碼")]
        [Required(ErrorMessage = "密碼為必填欄位")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        /// <summary>
        /// 是否記得我
        /// </summary>
        public bool RememberMe { get; set; } = false;
    }

    /// <summary>
    /// 註冊
    /// </summary>
    public class RegisterModel : LoginModel
    {
        /// <summary>
        /// 確認密碼
        /// </summary>
        [Display(Name = "確認密碼")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "密碼與確認密碼不相符")]
        public string ConfirmPassword { get; set; } = string.Empty;

        /// <summary>
        /// 員工身分證字號
        /// </summary>
        [Required(ErrorMessage = "員工身分證字號為必填欄位")]
        [Display(Name = "員工身分證字號")]
        public string EmployeeID { get; set; } = string.Empty;

        /// <summary>
        /// 員工名稱
        /// </summary>
        [Required(ErrorMessage = "員工名稱為必填欄位")]
        [Display(Name = "員工名稱")]
        public string EmployeeName { get; set; } = string.Empty;

        /// <summary>
        /// 員工行動電話號碼
        /// </summary>
        [Display(Name = "員工行動電話號碼")]
        public string CellPhone { get; set; } = string.Empty;

        /// <summary>
        /// 員工生日
        /// </summary>
        [Display(Name = "員工生日")]
        public DateTime? Birthday { get; set; }

        /// <summary>
        /// 銀行編號
        /// </summary>
        [Display(Name = "銀行編號")]
        public string BankCode { get; set; } = string.Empty;

        /// <summary>
        /// 銀行帳號
        /// </summary>
        [Display(Name = "銀行帳號")]
        public string BankAccountNO { get; set; } = string.Empty;

        /// <summary>
        /// 地址
        /// </summary>
        [Display(Name = "地址")]
        public string Address { get; set; } = string.Empty;

        /// <summary>
        /// 到職日期
        /// </summary>
        [Display(Name = "到職日期")]
        public DateTime? ArrivalDate { get; set; }

        /// <summary>
        /// 員工性別
        /// </summary>
        [Required(ErrorMessage = "性別為必填欄位")]
        [Display(Name = "員工性別")]
        public string Sex { get; set; } = string.Empty;
    }

    /// <summary>
    /// 重設密碼
    /// </summary>
    public class ResetPasswordModel : LoginModel
    {
        /// <summary>
        /// 確認密碼
        /// </summary>
        [Display(Name = "確認密碼")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "密碼與確認密碼不相符")]
        public string ConfirmPassword { get; set; } = string.Empty;

        /// <summary>
        /// 舊密碼
        /// </summary>
        public string OldPassword { get; set; } = string.Empty;

        /// <summary>
        /// 驗證碼
        /// </summary>
        public string Token { get; set; } = string.Empty;
    }
}
