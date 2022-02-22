using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sonlen.WebAdmin.Model
{
    public class Punch
    {
        public string EmployeeID { get; set; } = String.Empty;

        public DateTime PunchDate { get; set; }

        public string PunchInTime { get; set; } = String.Empty;

        public string? PunchOutTime { get; set; }

        public decimal? WorkHour { get; set; }
    }
}
