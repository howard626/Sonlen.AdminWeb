using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sonlen.AdminWebAPI.Service;
using Sonlen.WebAdmin.Model;

namespace Sonlen.AdminWebAPI.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalaryController : ControllerBase
    {
        private readonly ISalaryService _salaryService;

        public SalaryController(ISalaryService salaryService)
        {
            _salaryService = salaryService;
        }

        /// <summary>
        /// 取得薪資資料
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("GetSalary")]
        public ActionResult<string> GetSalary([FromBody] SalaryViewModel model)
        {
            return Ok(_salaryService.GetDataByID($"{model.EmployeeID},{model.PayMon}"));
        }
        

        /// <summary>
        /// 列印薪資
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("Print")]
        public ActionResult<string> Print([FromBody] SalaryViewModel model)
        {
            return Ok(_salaryService.Print(model));
        }
    }
}
