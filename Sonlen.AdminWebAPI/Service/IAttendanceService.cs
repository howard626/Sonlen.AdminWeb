using Sonlen.WebAdmin.Model;
using System.Data;

namespace Sonlen.AdminWebAPI.Service
{
    /// <summary>
    /// 考勤資料
    /// </summary>
    public interface IAttendanceService: IDataService<Attendance>, IPrintService<AttendanceViewModel,AttendancePrintModel>
    {
    }
}