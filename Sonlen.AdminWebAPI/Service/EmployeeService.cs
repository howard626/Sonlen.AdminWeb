using Sonlen.AdminWebAPI.Data;
using Sonlen.WebAdmin.Model;
using Sonlen.WebAdmin.Model.Utility;

namespace Sonlen.AdminWebAPI.Service
{
    public class EmployeeService : IEmployeeService
    {
        private readonly DataContext _context;

        public EmployeeService(DataContext context)
        {
            this._context = context;
        }

        public int AddEmployee(Employee employee)
        {
            if (!string.IsNullOrEmpty(employee.EmployeeID))
            {
                Employee? tempEmployee = GetEmployeeById(employee.EmployeeID);
                if (tempEmployee == null)
                {
                    employee.LoginKey = employee.EmployeeID.ToMD5();
                    _context.Employee.Add(employee);
                    _context.SaveChanges();
                    return 0;
                }
                else
                    // ID已經存在
                    return -2;
            }
            // employee 為空值
            return -1;
        }

        public int UpdateEmployee(Employee employee)
        {
            if (!string.IsNullOrEmpty(employee.EmployeeID))
            {
                Employee? tempEmployee = GetEmployeeById(employee.EmployeeID);
                if (tempEmployee != null)
                {
                    if (CheckEmail(tempEmployee.Email, tempEmployee.EmployeeID))
                    {
                        employee.LoginKey = tempEmployee.LoginKey;
                        tempEmployee = employee;
                        _context.Update(tempEmployee);
                        _context.SaveChanges();
                        return 0;
                    }
                    else
                        // Email 已經存在
                        return -2;
                }
                else
                    // ID 不存在
                    return -3;
            }
            // employee 為空值
            return -1;
        }

        public int DeleteEmployee(Employee employee)
        {
            if (!string.IsNullOrEmpty(employee.EmployeeID))
            {
                Employee? tempEmployee = GetEmployeeById(employee.EmployeeID);
                if (tempEmployee != null)
                {
                    _context.Employee.Remove(tempEmployee);
                    _context.SaveChanges();
                    return 0;
                }
                else
                    // ID 不存在
                    return -2;
            }
            // employee 為空值
            return -1;
        }
        
        public List<Employee> GetAllEmployees()
        {
            return _context.Employee.ToList();
        }

        public Employee? GetEmployeeById(string id)
        {
            return _context.Employee.FirstOrDefault(u => u.EmployeeID == id);
        }

        public Employee? GetEmployeeByEmail(string email)
        {
            return _context.Employee.FirstOrDefault(u => u.Email == email);
        }

        public bool CheckEmail(string email, string id)
        {
            return _context.Employee.FirstOrDefault(u => u.Email == email && u.EmployeeID != id) == null;
        }
    }
}
