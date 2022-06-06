using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sonlen.AdminWebAPI.Service;
using Sonlen.WebAdmin.Model;

namespace Sonlen.AdminWebAPI.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttendanceController : ControllerBase
    {
        private readonly IAttendanceService _attendanceService;

        public AttendanceController(IAttendanceService attendanceService)
        {
            _attendanceService = attendanceService;
        }

        /// <summary>
        /// 列印考勤
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("Print")]
        public ActionResult<string> Print([FromBody] AttendanceViewModel model)
        {
            return Ok(_attendanceService.Print(model));
        }
    }
}
