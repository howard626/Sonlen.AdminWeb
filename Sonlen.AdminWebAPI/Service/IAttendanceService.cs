using Sonlen.WebAdmin.Model;
using System.Data;

namespace Sonlen.AdminWebAPI.Service
{
    public interface IAttendanceService: IDataService<Attendance>
    {
        /// <summary>
        /// 列印考勤資料
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        UploadFile Print(AttendanceViewModel model);
    }
}