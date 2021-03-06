using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Sonlen.AdminWebAPI.LogSetting;
using Sonlen.AdminWebAPI.Service;
using Sonlen.WebAdmin.Model;
using Sonlen.WebAdmin.Model.Utility;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Mail;
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
        private readonly IEmployeeService _employeeService;

        public LoginController(IConfiguration configuration, ILoginService loginService, IEmployeeService employeeService)
        {
            this.configuration = configuration;
            _loginService = loginService;
            _employeeService = employeeService;
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
        /// <param name="employee"></param>
        /// <returns></returns>
        [HttpPost("GetEmployee")]
        public ActionResult<string> GetEmployeeById([FromBody] Employee employee)
        {

            return Ok(_employeeService.GetDataByID(employee.EmployeeID ?? string.Empty));
        }

        /// <summary>
        /// 取得全部員工資料
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetAllEmployees")]
        public ActionResult<string> GetAllEmployees()
        {
            return Ok(_employeeService.GetAllDatas());
        }

        /// <summary>
        /// 寄出忘記密碼重設確認信件
        /// </summary>
        /// <param name="loginInfo"></param>
        /// <returns></returns>
        [HttpPost("ForgotPassword")]
        public ActionResult<string> ForgotPassword([FromBody] LoginModel loginInfo)
        {

            Employee? employee = _employeeService.GetDataByEmail(loginInfo.Account);

            if (employee != null)
            {
                try
                {
                    string token = Guid.NewGuid().ToString().ToMD5();
                    using (MailMessage msg = new MailMessage())
                    {
                        msg.To.Add(loginInfo.Account);
                        msg.From = new MailAddress(configuration["Email:DefaultAccount"], "神倫資訊有限公司", Encoding.UTF8);
                        /* 上面3個參數分別是發件人地址，發件人姓名，編碼*/
                        msg.Subject = "神倫資訊有限公司-忘記密碼";//郵件標題
                        msg.SubjectEncoding = Encoding.UTF8;//郵件標題編碼
                        msg.Body = $"有人申請重設密碼，如果是您本人申請的<a href=\"https://localhost:44341/ResetPassword/{loginInfo.Account}/{token}\">請點選此處</a>。"; //郵件內容
                        msg.BodyEncoding = Encoding.UTF8;//郵件內容編碼 
                                                         //msg.Attachments.Add(new Attachment(@"D:\test2.docx"));  //附件
                        msg.IsBodyHtml = true;//是否是HTML郵件 
                                              //msg.Priority = MailPriority.High;//郵件優先級 

                        using (SmtpClient client = new SmtpClient("smtp.gmail.com", 587))
                        {
                            client.Credentials = new System.Net.NetworkCredential(configuration["Email:DefaultAccount"], configuration["Email:DefaultPassword"]); //寄送 gmail 的帳號
                            client.EnableSsl = true; //gmail預設開啟驗證
                            client.Send(msg); //寄出信件
                        }
                    }

                    _loginService.SetResetPasswordToken(loginInfo.Account, token);
                    return Ok(token);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }

            return BadRequest("此帳號不存在");
        }

        /// <summary>
        /// 重設密碼
        /// </summary>
        /// <param name="loginInfo"></param>
        /// <returns></returns>
        [HttpPost("ResetPassword")]
        public ActionResult<string> ResetPassword([FromBody] ResetPasswordModel loginInfo)
        {
            if (string.IsNullOrEmpty(_loginService.ResetPassword(loginInfo)))
            {
                return Ok("重設密碼成功");
            }

            return BadRequest("此帳號不存在");
        }

        /// <summary>
        /// 建立Token
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>
        private UserToken BuildToken(Employee employee)
        {
            //記在jwt payload中的聲明，可依專案需求自訂Claim
            string role = "employee";
            if (Setting.LEAVE_APPROVED_ID.Equals(employee.EmployeeID))
                role = "admin";
            var claims = new List<Claim>()
            {
                new Claim("Name", employee.EmployeeName),
                new Claim(ClaimTypes.Role, role),
                new Claim("EmployeeID", employee.EmployeeID),
                new Claim("Email", employee.Email)
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
