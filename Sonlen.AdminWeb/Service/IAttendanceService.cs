using Sonlen.WebAdmin.Model;

namespace Sonlen.AdminWeb.Service
{
    public interface IAttendanceService
    {
        /// <summary>
        /// 列印考勤
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<UploadFile> PrintAsync(AttendanceViewModel model);
    }
}