using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sonlen.WebAdmin.Model
{
    public class LeaveViewModel
    {
        public Employee? Employee { get; set; }

        public DateTime LeaveDate { get; set; }

        public int LeaveType { get; set; }

        public string LeaveStartTime { get; set; } = string.Empty;

        public string LeaveEndTime { get; set; } = string.Empty;

        public UploadFile? File { get; set; }
    }
}
