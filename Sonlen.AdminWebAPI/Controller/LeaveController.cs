using Microsoft.AspNetCore.Mvc;
using Sonlen.AdminWebAPI.Service;
using Sonlen.WebAdmin.Model;

namespace Sonlen.AdminWebAPI.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeaveController : ControllerBase
    {
        private readonly ILeaveService _leaveService;

        public LeaveController(ILeaveService leaveService)
        {
            _leaveService = leaveService;
        }

        /// <summary>
        /// 請假
        /// </summary>
        /// <param name="leaveViewModel"></param>
        /// <returns></returns>
        [HttpPost("DayOff")]
        public ActionResult<string> DayOff([FromBody] LeaveViewModel leaveViewModel)
        {
            int result = _leaveService.DayOff(leaveViewModel);
            if (result > 0)
                return Ok("請假成功");
            else if (result == -2)
                return BadRequest("請假失敗：你已經請假過了");
            else
                return BadRequest("請假失敗");
        }

        /// <summary>
        /// 取得請假類別
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost("GetLeaveTypes")]
        public ActionResult<string> GetLeaveTypes()
        {
            return Ok(_leaveService.GetLeaveTypes());
        }

        /// <summary>
        /// 從 EmployeeID 取得請假紀錄
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>
        [HttpPost("GetLeaveRecordByEID")]
        public ActionResult<string> GetLeaveRecordByEID([FromBody] Employee employee)
        {
            return Ok(_leaveService.GetLeaveRecordByEID(employee.EmployeeID));
        }

        
    }
}
