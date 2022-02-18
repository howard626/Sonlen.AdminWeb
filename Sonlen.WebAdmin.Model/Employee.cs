using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Sonlen.WebAdmin.Model
{
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
        public string EmployeeID { get; set; } = String.Empty;

        public string Email { get; set; } = String.Empty;

        public string EmployeeName { get; set; } = String.Empty;

        public DateTime? ArrivalDate { get; set; }

        public DateTime? Birthday { get; set; }

        public string? BankCode { get; set; }

        public string? BankAccountNO { get; set; }

        public string? CellPhone { get; set; }

        public string? Address { get; set; }

        public byte Sex { get; set; }

        [JsonIgnore]
        public string? LoginKey { get; set; }

    }
}
