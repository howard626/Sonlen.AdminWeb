using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Sonlen.AdminWebAPI.Service;
using Sonlen.WebAdmin.Model;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Sonlen.AdminWebAPI.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IConfiguration configuration;
        private readonly ILoginService _loginService;

        public LoginController(IConfiguration configuration, ILoginService loginService)
        {
            this.configuration = configuration;
            _loginService = loginService;
        }

        /// <summary>
        /// 登入
        /// </summary>
        /// <param name="loginInfo"></param>
        /// <returns></returns>
        [HttpPost("Login")]
        public ActionResult<UserToken> Login([FromBody] LoginModel loginInfo)
        {
            Employee? employee = _loginService.Login(loginInfo.Account, loginInfo.Password);

            if (employee != null)
            {
                return BuildToken(employee);
            }

            return BadRequest("帳號或密碼錯誤");
        }

        /// <summary>
        /// 註冊
        /// </summary>
        /// <param name="register"></param>
        /// <returns></returns>
        [HttpPost("Register")]
        public ActionResult<string> Register([FromBody] RegisterModel register)
        {

            return Ok(_loginService.Register(register));
        }

        /// <summary>
        /// 取得員工資料
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost("GetEmployee")]
        public ActionResult<string> GetEmployee(string id)
        {

            return Ok(_loginService.GetEmployee(id));
        }

        /// <summary>
        /// 取得全部員工資料
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost("GetAllEmployees")]
        public ActionResult<string> GetAllEmployees()
        {

            return Ok(_loginService.GetAllEmployees());
        }

        /// <summary>
        /// 建立Token
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>
        private UserToken BuildToken(Employee employee)
        {
            //記在jwt payload中的聲明，可依專案需求自訂Claim
            var claims = new List<Claim>()
            {
                new Claim("Name", employee.EmployeeName),
                new Claim("role", "employee"),
                new Claim("EmployeeID", employee.EmployeeID)
            };

            
            //取得對稱式加密 JWT Signature 的金鑰
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["jwt:Key"]));
            var credential = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            //設定token有效期限
            DateTime expireTime = DateTime.Now.AddMinutes(30);

            //產生token
            JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            JwtSecurityToken jwtSecurityToken = new JwtSecurityToken(
                issuer: null,
                audience: null,
                claims: claims,
                expires: expireTime,
                signingCredentials: credential
                );
            string jwtToken = jwtSecurityTokenHandler.WriteToken(jwtSecurityToken);

            //建立UserToken物件後回傳client
            UserToken userToken = new UserToken()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                token = jwtToken,
                ExpireTime = expireTime
            };

            return userToken;
        }
    }
}
