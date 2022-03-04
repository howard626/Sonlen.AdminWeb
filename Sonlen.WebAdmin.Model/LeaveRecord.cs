using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sonlen.WebAdmin.Model
{
    public class LeaveRecord
    {
        public string EmployeeID { get; set; } = string.Empty;

        public DateTime LeaveDate { get; set; }

        public string LeaveStartTime { get; set; } = string.Empty;

        public string LeaveEndTime { get; set; } = string.Empty;

        public int LeaveType { get; set; } 

        public int Accept { get; set; }

        public decimal? LeaveHour { get; set; }

        public string? Prove { get; set; }

    }

    public class LeaveRecordViewModel : LeaveRecord
    {
        public string LeaveDesc { get; set; } = string.Empty;

        public string AcceptDesc { get; set; } = string.Empty;

        public string LeaveDateDesc { get; set; } = string.Empty;
    }
}
