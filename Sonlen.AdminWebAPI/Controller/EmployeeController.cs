﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sonlen.AdminWebAPI.Service;
using Sonlen.WebAdmin.Model;

namespace Sonlen.AdminWebAPI.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {

        private readonly IConfiguration configuration;
        private readonly IEmployeeService _employeeService;
        private readonly IEmployeeDapperService _employeeDapperService;

        public EmployeeController(IConfiguration configuration, IEmployeeService employeeService, IEmployeeDapperService employeeDapperService)
        {
            this.configuration = configuration;
            _employeeService = employeeService;
            _employeeDapperService = employeeDapperService;
        }

        /// <summary>
        /// 取得員工資料
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost("Add")]
        public ActionResult<string> AddEmployee([FromBody] Employee employee)
        {
            int result = _employeeDapperService.AddEmployee(employee);
            if (result > 0)
                return Ok("新增員工成功");
            else if (result == -1)
                return BadRequest("新增員工失敗：EmployeeID 已經存在");
            else if (result == -2)
                return BadRequest("新增員工失敗：Email 已經存在");
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
                return BadRequest("更新員工失敗：EmployeeID 不存在");
            else if (result == -2)
                return BadRequest("更新員工失敗：Email 已經存在");
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
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost("GetEmployee")]
        public ActionResult<string> GetEmployeeById([FromBody] Employee employee)
        {
            return Ok(_employeeDapperService.GetEmployeeById(employee.EmployeeID ?? string.Empty));
        }

        /// <summary>
        /// 取得全部員工資料
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost("GetAllEmployees")]
        public ActionResult<string> GetAllEmployees()
        {
            return Ok(_employeeDapperService.GetAllEmployees());   
        }
    }
}