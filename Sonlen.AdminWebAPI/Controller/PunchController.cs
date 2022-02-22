using Microsoft.AspNetCore.Mvc;
using Sonlen.AdminWebAPI.Service;
using Sonlen.WebAdmin.Model;

namespace Sonlen.AdminWebAPI.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class PunchController : ControllerBase
    {
        private readonly IPunchService _punchServiceService;

        public PunchController(IPunchService punchServiceService)
        {
            _punchServiceService = punchServiceService;
        }

        /// <summary>
        /// 上班打卡
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>
        [HttpPost("In")]
        public ActionResult<string> PunchIn([FromBody] Employee employee)
        {
            int result = _punchServiceService.PunchIn(employee);
            if (result > 0)
                return Ok("打卡成功");
            else if (result == -1)
                return BadRequest("打卡失敗：你已經上班打卡過了");
            else
                return BadRequest("打卡失敗");
        }

        /// <summary>
        /// 下班打卡
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>
        [HttpPost("Out")]
        public ActionResult<string> PunchOut([FromBody] Employee employee)
        {
            int result = _punchServiceService.PunchOut(employee);
            if (result > 0)
                return Ok("打卡成功");
            else if (result == -1)
                return BadRequest("打卡失敗：你還沒上班打卡");
            else if (result == -2)
                return BadRequest("打卡失敗：你已經下班打卡過了");
            else
                return BadRequest("打卡失敗");
        }
    }
}
