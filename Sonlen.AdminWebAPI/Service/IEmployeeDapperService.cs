using Sonlen.WebAdmin.Model;
using System.Data;

namespace Sonlen.AdminWebAPI.Service
{
    public interface IEmployeeDapperService
    {
        IDbConnection Connection { get; }

        int AddEmployee(Employee employee);
        bool CheckEmail(string email, string id);
        int DeleteEmployee(Employee employee);
        IEnumerable<Employee> GetAllEmployees();
        Employee GetEmployeeByEmail(string email);
        Employee GetEmployeeById(string id);
        int UpdateEmployee(Employee employee);
    }
}