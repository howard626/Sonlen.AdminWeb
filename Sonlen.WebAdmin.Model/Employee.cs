using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Sonlen.WebAdmin.Model
{
    /// <summary>
    /// 員工資料
    /// </summary>
    public class Employee
    {
        public Employee()
        { }

        public Employee(RegisterModel user)
        {
            Email = user.Account;
            EmployeeID = user.EmployeeID;
            EmployeeName = user.EmployeeName;
            ArrivalDate = user.ArrivalDate;
            Birthday = user.Birthday;
            BankCode = user.BankCode;
            BankAccountNO = user.BankAccountNO;
            CellPhone = user.CellPhone;
            Address = user.Address;
            Sex = "男".Equals(user.Sex) ? (byte)1 : (byte)2;
        }

        /// <summary>
        /// 員工編號
        /// </summary>
        public string EmployeeID { get; set; } = String.Empty;

        /// <summary>
        /// 員工電子信箱(帳號)
        /// </summary>
        public string Email { get; set; } = String.Empty;

        /// <summary>
        /// 員工名字
        /// </summary>
        public string EmployeeName { get; set; } = String.Empty;

        /// <summary>
        /// 員工到職日期
        /// </summary>
        public DateTime? ArrivalDate { get; set; }

        /// <summary>
        /// 員工生日
        /// </summary>
        public DateTime? Birthday { get; set; }

        /// <summary>
        /// 員工銀行帳戶代碼
        /// </summary>
        public string? BankCode { get; set; }

        /// <summary>
        /// 員工帳戶號碼
        /// </summary>
        public string? BankAccountNO { get; set; }

        /// <summary>
        /// 員工手機號碼
        /// </summary>
        public string? CellPhone { get; set; }

        /// <summary>
        /// 員工地址
        /// </summary>
        public string? Address { get; set; }

        /// <summary>
        /// 員工性別
        /// </summary>
        public byte Sex { get; set; }

        /// <summary>
        /// 密碼
        /// </summary>
        [JsonIgnore]
        public string? LoginKey { get; set; }

    }
}
