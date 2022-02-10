using Sonlen.AdminWebAPI.Data;
using Sonlen.WebAdmin.Model;
using Sonlen.WebAdmin.Model.Utility;

namespace Sonlen.AdminWebAPI.Service
{
    public class LoginService : ILoginService
    {
        private readonly DataContext _context;

        public LoginService(DataContext context)
        {
            this._context = context;
        }

        public List<Employee> GetAllEmployees()
        {
            return _context.Employee.ToList();
        }
        
        public Employee? Login(string account, string password)
        {
            return _context.Employee.FirstOrDefault(u => u.Email == account && u.LoginKey == password.ToMD5());
        }

        public void SetResetPasswordToken(string email, string token)
        {
            ResetPassword? resetPassword = _context.ResetPassword.FirstOrDefault(r => r.Email == email);
            if (resetPassword == null)
            {
                resetPassword = new ResetPassword()
                {
                    Email = email,
                    Token = token
                };

                _context.ResetPassword.Add(resetPassword);
                _context.SaveChanges();
            }
            else 
            {
                resetPassword.Token = token;

                _context.ResetPassword.Update(resetPassword);
                _context.SaveChanges();
            }
            
        }

        public string Register(RegisterModel model)
        {
            string msg = string.Empty;

            if (_context.Employee.FirstOrDefault(u => u.Email == model.Account || u.EmployeeID == model.EmployeeID) != null)
            {
                msg = "帳號或身分證字號已重複，請重新輸入";
            }
            else
            {
                Employee Employee = new Employee()
                {
                    EmployeeID = model.EmployeeID,
                    Email = model.Account,
                    LoginKey = model.Password.ToMD5(),
                    EmployeeName = model.EmployeeName,
                    CellPhone = model.CellPhone,
                    Birthday = model.Birthday,
                    BankCode = model.BankCode,
                    BankAccountNO = model.BankAccountNO,
                    Address = model.Address,
                    Sex = "男".Equals(model.Sex) ? (byte)1 : (byte)2,
                    ArrivalDate = DateTime.Now
                };
                _context.Employee.Add(Employee);
                _context.SaveChanges();
                msg = "註冊成功";
            }

            return msg;
        }

        public string ResetPassword(ResetPasswordModel model)
        {
            string msg = string.Empty;
            if (string.IsNullOrEmpty(model.OldPassword))
            {
                ResetPassword resetPassword = _context.ResetPassword.FirstOrDefault(r => r.Token == model.Token) ?? new ResetPassword();
                Employee? employee = _context.Employee.FirstOrDefault(u => u.Email == resetPassword.Email);
                if (employee == null)
                {
                    msg = "帳號不存在，請重新輸入";
                }
                else
                {
                    employee.LoginKey = model.Password.ToMD5();
                    _context.Employee.Update(employee);
                    _context.ResetPassword.Remove(resetPassword);
                    _context.SaveChanges();
                }
            }
            else 
            {

            }

            return msg;
        }

        public Employee? GetEmployeeById(string id)
        {
            return _context.Employee.FirstOrDefault(u => u.EmployeeID == id);
        }

        public Employee? GetEmployeeByEmail(string email)
        {
            return _context.Employee.FirstOrDefault(u => u.Email == email);
        }
    }
}
