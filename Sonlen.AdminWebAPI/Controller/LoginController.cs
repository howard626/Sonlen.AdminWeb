using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
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
        public ActionResult<string> GetEmployeeById([FromBody] Employee employee)
        {

            return Ok(_loginService.GetEmployeeById(employee.EmployeeID ?? string.Empty));
        }

        /// <summary>
        /// 取得全部員工資料
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost("GetAllEmployees")]
        public ActionResult<string> GetAllEmployees(AuthModel<string> auth)
        {
            if (CheckAuth(auth.Token))
                return Ok(_loginService.GetAllEmployees());
            else
                return BadRequest("權限不足");
        }

        /// <summary>
        /// 寄出忘記密碼重設確認信件
        /// </summary>
        /// <param name="loginInfo"></param>
        /// <returns></returns>
        [HttpPost("ForgotPassword")]
        public ActionResult<string> ForgotPassword([FromBody] LoginModel loginInfo)
        {

            Employee? employee = _loginService.GetEmployeeByEmail(loginInfo.Account);

            if (employee != null)
            {
                try
                {
                    string token = Guid.NewGuid().ToString().ToMD5();
                    using (MailMessage msg = new MailMessage())
                    {
                        msg.To.Add(loginInfo.Account);
                        msg.From = new MailAddress("howardchang@sonlen.com.tw", "神倫資訊有限公司", Encoding.UTF8);
                        /* 上面3個參數分別是發件人地址（可以隨便寫），發件人姓名，編碼*/
                        msg.Subject = "神倫資訊有限公司-忘記密碼";//郵件標題
                        msg.SubjectEncoding = Encoding.UTF8;//郵件標題編碼
                        msg.Body = $"有人申請重設密碼，如果是您本人申請的<a href=\"https://localhost:44341/ResetPassword/{loginInfo.Account}/{token}\">請點選此處</a>。"; //郵件內容
                        msg.BodyEncoding = Encoding.UTF8;//郵件內容編碼 
                                                         //msg.Attachments.Add(new Attachment(@"D:\test2.docx"));  //附件
                        msg.IsBodyHtml = true;//是否是HTML郵件 
                                              //msg.Priority = MailPriority.High;//郵件優先級 

                        using (SmtpClient client = new SmtpClient("smtp.gmail.com", 587))
                        {
                            client.Credentials = new System.Net.NetworkCredential("howardchang@sonlen.com.tw", "H626LEmm@"); //這裡要填正確的帳號跟密碼
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
        /// 寄出忘記密碼重設確認信件
        /// </summary>
        /// <param name="loginInfo"></param>
        /// <returns></returns>
        [HttpPost("ResetPassword")]
        public ActionResult<string> ResetPassword([FromBody] ResetPasswordModel loginInfo)
        {
            string result = _loginService.ResetPassword(loginInfo);
            if (string.IsNullOrEmpty(result))
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
            var claims = new List<Claim>()
            {
                new Claim("Name", employee.EmployeeName),
                new Claim("role", "employee"),
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

        /// <summary>
        /// 檢查授權
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        private bool CheckAuth(string token)
        {
            if (!string.IsNullOrEmpty(token))
            {
                if (token.Split('.').Length == 3)
                {

                    try
                    {
                        var claims = JwtParser.ParseClaimsFromJwt(token);
                        if (claims.Any())
                        {
                            var employee = _loginService.GetEmployeeByEmail(claims.FirstOrDefault(c => c.Type == "Email")?.Value ?? string.Empty);
                            if (employee != null)
                            {
                                return true;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            return false;
        }
    }
}
