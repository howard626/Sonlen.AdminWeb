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
        /// 取消請假
        /// </summary>
        /// <param name="leave"></param>
        /// <returns></returns>
        [HttpPost("CancelDayOff")]
        public ActionResult<string> CancelDayOff([FromBody] LeaveRecord leave)
        {
            int result = _leaveService.CancelDayOff(leave);
            if (result > 0)
                return Ok("取消請假成功");
            else
                return BadRequest("取消請假失敗");
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
        /// 取得全部請假紀錄
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetAllLeaveRecord")]
        public ActionResult<string> GetAllLeaveRecord()
        {
            return Ok(_leaveService.GetAllLeaveRecord());
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

        /// <summary>
        /// 同意請假
        /// </summary>
        /// <param name="leave"></param>
        /// <returns></returns>
        [HttpPost("AgreeLeaveRecord")]
        public ActionResult<string> AgreeLeaveRecord([FromBody] LeaveRecord leave)
        {
            int result = _leaveService.AgreeLeaveRecord(leave.EmployeeID, leave.LeaveDate);
            return Ok(result > 0 ? "同意成功" : "同意失敗");
        }

        /// <summary>
        /// 駁回請假
        /// </summary>
        /// <param name="leave"></param>
        /// <returns></returns>
        [HttpPost("DisagreeLeaveRecord")]
        public ActionResult<string> DisagreeLeaveRecord([FromBody] LeaveRecord leave)
        {
            int result = _leaveService.DisagreeLeaveRecord(leave.EmployeeID, leave.LeaveDate);
            return Ok(result > 0 ? "駁回成功" : "駁回失敗");
        }

        /// <summary>
        /// 取得請假證明檔案
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost("GetLeaveProve")]
        public ActionResult<string> GetLeaveProve([FromBody] UploadFile file)
        {
            return Ok(_leaveService.GetLeaveProve(file));
        }
    }
}
