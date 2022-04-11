﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sonlen.AdminWebAPI.LogSetting;
using Sonlen.AdminWebAPI.Service;
using Sonlen.WebAdmin.Model;

namespace Sonlen.AdminWebAPI.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeDapperService _employeeDapperService;

        public EmployeeController(IEmployeeDapperService employeeDapperService)
        {
            _employeeDapperService = employeeDapperService;
        }

        /// <summary>
        /// 取得員工資料
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>
        [HttpPost("Add")]
        public ActionResult<string> AddEmployee([FromBody] Employee employee)
        {
            int result = _employeeDapperService.AddEmployee(employee);
            if (result > 0)
                return Ok("新增員工成功");
            else if (result == -1)
                return Ok("新增員工失敗：EmployeeID 已經存在");
            else if (result == -2)
                return Ok("新增員工失敗：Email 已經存在");
            else
                return BadRequest("新增員工失敗");
        }

        /// <summary>
        /// 取得員工資料
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>
        [HttpPost("Update")]
        public ActionResult<string> UpdateEmployee([FromBody] Employee employee)
        {
            int result = _employeeDapperService.UpdateEmployee(employee);
            
            if (result > 0)
                return Ok("更新員工成功");
            else if (result == -3)
                return Ok("更新員工失敗：EmployeeID 不存在");
            else if (result == -2)
                return Ok("更新員工失敗：Email 已經存在");
            else
                return BadRequest("更新員工失敗");
        }

        /// <summary>
        /// 取得員工資料
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>
        [HttpPost("Delete")]
        public ActionResult<string> DeleteEmployee([FromBody] Employee employee)
        {
            if (_employeeDapperService.DeleteEmployee(employee) > 0)
                return Ok("刪除員工成功");
            else
                return BadRequest("刪除員工失敗");
        }

        /// <summary>
        /// 取得員工資料
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>
        [HttpPost("GetEmployee")]
        public ActionResult<string> GetEmployeeById([FromBody] Employee employee)
        {
            return Ok(_employeeDapperService.GetEmployeeById(employee.EmployeeID ?? string.Empty));
        }

        /// <summary>
        /// 取得全部員工資料
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetAllEmployees")]
        public ActionResult<string> GetAllEmployees()
        {
            return Ok(_employeeDapperService.GetAllEmployees());   
        }

        /// <summary>
        /// 取得員工通知
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>
        [HttpPost("GeNoticeByEID")]
        public ActionResult<string> GeNoticeByEID([FromBody] Employee employee)
        {
            return Ok(_employeeDapperService.GeNoticeByEID(employee));
        }

        /// <summary>
        /// 設定通知為已讀
        /// </summary>
        /// <param name="notices"></param>
        /// <returns></returns>
        [HttpPost("SetNoticeToIsRead")]
        public ActionResult<string> SetNoticeToIsRead([FromBody] List<Notice> notices)
        {
            int result = _employeeDapperService.SetNoticeToIsRead(notices);
            if (result > 0)
                return Ok("已將通知標誌為已讀");
            else
                return BadRequest("通知標誌已讀失敗");
        }
    }
}
