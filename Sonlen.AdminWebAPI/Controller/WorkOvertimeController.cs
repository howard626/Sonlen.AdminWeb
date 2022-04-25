using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sonlen.AdminWebAPI.Service;
using Sonlen.WebAdmin.Model;

namespace Sonlen.AdminWebAPI.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkOvertimeController : ControllerBase
    {
        private readonly IWorkOvertimeService _workOvertimeService;

        public WorkOvertimeController(IWorkOvertimeService punchService)
        {
            _workOvertimeService = punchService;
        }

        /// <summary>
        /// 取得全部加班紀錄
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetAllWorkOvertime")]
        public ActionResult<string> GetAllWorkOvertime()
        {
            return Ok(_workOvertimeService.GetAllWorkOvertime());
        }

        /// <summary>
        /// 從 EmployeeID 取得加班紀錄
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>
        [HttpPost("GetOvertimeRecordByEID")]
        public ActionResult<string> GetOvertimeRecordByEID([FromBody] Employee employee)
        {
            return Ok(_workOvertimeService.GetWorkOvertimeByEID(employee.EmployeeID));
        }

        /// <summary>
        /// 從 EmployeeID 取得加班紀錄
        /// </summary>
        /// <param name="overtime"></param>
        /// <returns></returns>
        [HttpPost("AddWorkOvertime")]
        public ActionResult<string> AddWorkOvertime([FromBody] WorkOvertimeViewModel overtime)
        {
            int result = _workOvertimeService.AddWorkOvertime(overtime);
            if (result > 0)
                return Ok("申請成功");
            else if (result == -2)
                return Ok("申請失敗：您已經於當天申請過加班");
            else
                return Ok("申請失敗");
        }

        /// <summary>
        /// 取消申請加班
        /// </summary>
        /// <param name="overtime"></param>
        /// <returns></returns>
        [HttpPost("CancelWorkOvertime")]
        public ActionResult<string> CancelWorkOvertime([FromBody] WorkOvertime overtime)
        {
            int result = _workOvertimeService.CancelWorkOvertime(overtime);
            if (result > 0)
                return Ok("取消申請加班成功");
            else
                return Ok("取消申請加班失敗");
        }

        /// <summary>
        /// 同意申請加班
        /// </summary>
        /// <param name="overtime"></param>
        /// <returns></returns>
        [HttpPost("AgreeWorkOvertime")]
        public ActionResult<string> AgreeWorkOvertime([FromBody] WorkOvertime overtime)
        {
            int result = _workOvertimeService.AgreeWorkOvertime(overtime);
            return Ok(result > 0 ? "同意成功" : "同意失敗");
        }

        /// <summary>
        /// 駁回申請加班
        /// </summary>
        /// <param name="overtime"></param>
        /// <returns></returns>
        [HttpPost("DisagreeWorkOvertime")]
        public ActionResult<string> DisagreeWorkOvertime([FromBody] WorkOvertime overtime)
        {
            int result = _workOvertimeService.DisagreeWorkOvertime(overtime);
            return Ok(result > 0 ? "駁回成功" : "駁回失敗");
        }
    }
}
