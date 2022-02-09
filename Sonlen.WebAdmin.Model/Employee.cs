using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sonlen.WebAdmin.Model
{
    public class Employee
    {
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

        public string? LoginKey { get; set; }

    }
}
