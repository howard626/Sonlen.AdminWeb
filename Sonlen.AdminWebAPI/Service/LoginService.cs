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

        public Employee? GetEmployee(string id)
        {
            return _context.Employee.FirstOrDefault(u => u.EmployeeID == id);
        }
    }
}
