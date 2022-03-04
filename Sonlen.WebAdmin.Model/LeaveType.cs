using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sonlen.WebAdmin.Model
{
    public class LeaveType
    {
        public int Id { get; set; }

        public string LeaveName { get; set; } = string.Empty;

        public string LeaveDesc { get; set; } = string.Empty;

        public int Pay { get; set; }

        public int? TotalHour { get; set; }
    }
}
