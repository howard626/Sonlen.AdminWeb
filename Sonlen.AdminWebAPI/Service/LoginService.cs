using Dapper;
using Sonlen.AdminWebAPI.Data;
using Sonlen.WebAdmin.Model;
using Sonlen.WebAdmin.Model.Utility;
using System.Data;
using System.Data.SqlClient;

namespace Sonlen.AdminWebAPI.Service
{
    public class LoginService : ILoginService
    {
        private readonly string connectString;
        private readonly IEmployeeService _employeeService;
        private readonly IResetPasswordService _resetPasswordService;

        public LoginService(IConfiguration configuration,
            IEmployeeService employeeService,
            IResetPasswordService resetPasswordService)
        {
            connectString = configuration["ConnectionStrings:DefaultConnection"];
            _employeeService = employeeService;
            _resetPasswordService = resetPasswordService;
        }

        public IDbConnection Connection
        {
            get { return new SqlConnection(connectString); }
        }

        /// 依帳號密碼取得員工資料
        public Employee? Login(string account, string password)
        {
            Employee employee = new Employee();
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@Email", account, DbType.String);
            parameters.Add("@LoginKey", password.ToMD5(), DbType.String);

            using (var conn = new SqlConnection(connectString))
            {
                employee = conn.QueryFirstOrDefault<Employee>("Login", parameters, commandType: CommandType.StoredProcedure);
            }
            return employee;
        }

        /// 設定重設密碼認證碼
        public void SetResetPasswordToken(string email, string token)
        {
            ResetPassword? resetPassword = _resetPasswordService.GetDataByID(email);
            if (resetPassword == null)
            {
                resetPassword = new ResetPassword()
                {
                    Email = email,
                    Token = token
                };

                _resetPasswordService.InsertData(resetPassword);
            }
            else
            {
                resetPassword.Token = token;
                _resetPasswordService.UpdateData(resetPassword);
            }
        }

        /// 重設密碼
        public string ResetPassword(ResetPasswordModel model)
        {
            string msg = string.Empty;
            if (string.IsNullOrEmpty(model.OldPassword))
            {
                ResetPassword? resetPassword = _resetPasswordService.GetDataByID(model.Account);
                Employee? employee = _employeeService.GetDataByID(model.Account);
                if (employee == null)
                {
                    msg = "帳號不存在，請重新輸入";
                }
                else
                {
                    if (resetPassword == null)
                    {
                        msg = "此帳號沒有申請重設密碼";
                    }
                    else
                    {
                        employee.LoginKey = model.Password.ToMD5();
                        _employeeService.UpdateData(employee);
                        _resetPasswordService.DeleteData(resetPassword);                        
                    }
                }
            }
            else
            {

            }

            return msg;
        }

        /// 註冊新員工
        public string Register(RegisterModel item)
        {
            string msg = string.Empty;

            using (var conn = Connection)
            {
                Employee Employee = new Employee()
                {
                    EmployeeID = item.EmployeeID,
                    Email = item.Account,
                    LoginKey = item.Password.ToMD5(),
                    EmployeeName = item.EmployeeName,
                    CellPhone = item.CellPhone,
                    Birthday = item.Birthday,
                    BankCode = item.BankCode,
                    BankAccountNO = item.BankAccountNO,
                    Address = item.Address,
                    Sex = "男".Equals(item.Sex) ? (byte)1 : (byte)2,
                    ArrivalDate = DateTime.Now
                };
                msg = _employeeService.InsertData(Employee);
            }
            return msg;
        }

    }
}
