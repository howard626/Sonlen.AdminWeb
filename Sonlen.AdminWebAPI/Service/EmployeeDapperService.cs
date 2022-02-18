using Dapper;
using Sonlen.AdminWebAPI.Data;
using Sonlen.WebAdmin.Model;
using Sonlen.WebAdmin.Model.Utility;
using System.Data;
using System.Data.SqlClient;

namespace Sonlen.AdminWebAPI.Service
{
    public class EmployeeDapperService : IEmployeeDapperService
    {
        private readonly string connectString;

        public EmployeeDapperService(IConfiguration configuration)
        {
            connectString = configuration["ConnectionStrings:DefaultConnection"];
        }

        public IDbConnection Connection
        {
            get { return new SqlConnection(connectString); }
        }

        public int AddEmployee(Employee employee)
        {
            int result;

            if (CheckEmail(employee.Email, employee.EmployeeID))
            {
                if (GetEmployeeById(employee.EmployeeID) == null)
                {
                    using (var conn = Connection)
                    {
                        employee.LoginKey = employee.EmployeeID.ToMD5();
                        DynamicParameters parameters = employee.ToDynamicParameters();
                        result = conn.Execute("InsEmployee", parameters, commandType: CommandType.StoredProcedure);
                    }
                }
                else 
                {
                    result = -1; // ID 已存在
                }
            }
            else
            {
                result = -2; // Email 已存在
            }

            return result;
        }

        public int UpdateEmployee(Employee employee)
        {
            int result;

            if (CheckEmail(employee.Email, employee.EmployeeID))
            {
                employee.LoginKey = GetEmployeeById(employee.EmployeeID)?.LoginKey;
                if (string.IsNullOrEmpty(employee.LoginKey))
                {
                    return -3; // ID 不存在
                }
                else
                {
                    using (var conn = new SqlConnection(connectString))
                    {
                        DynamicParameters parameters = employee.ToDynamicParameters();
                        result = conn.Execute("UpdEmployee", parameters, commandType: CommandType.StoredProcedure);
                    }
                }
            }
            else
            {
                result = -2; // Email 已存在
            }

            return result;
        }

        public int DeleteEmployee(Employee employee)
        {
            int result;
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@EmployeeID", employee.EmployeeID, DbType.String);

            using (var conn = new SqlConnection(connectString))
            {
                result = conn.Execute("DelEmployee", parameters, commandType: CommandType.StoredProcedure);
            }

            return result;
        }

        public IEnumerable<Employee> GetAllEmployees()
        {
            IEnumerable<Employee> employee;
            using (var conn = new SqlConnection(connectString))
            {
                employee = conn.Query<Employee>("GetAllEmployee", commandType: CommandType.StoredProcedure);
            }
            return employee;
        }

        public Employee GetEmployeeById(string id)
        {
            Employee employee = new Employee();
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@EmployeeID", id, DbType.String);
            
            using (var conn = new SqlConnection(connectString))
            {
                employee = conn.QueryFirstOrDefault<Employee>("GetEmployeeByID", parameters, commandType: CommandType.StoredProcedure);
            }
            return employee;
        }

        public Employee GetEmployeeByEmail(string email)
        {
            Employee employee = new Employee();
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@Email", email, DbType.String);

            using (var conn = new SqlConnection(connectString))
            {
                employee = conn.QueryFirstOrDefault<Employee>("GetEmployeeByEmail", parameters, commandType: CommandType.StoredProcedure);
            }
            return employee;
        }

        public bool CheckEmail(string email, string id)
        {
            Employee? employee;
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@EmployeeID", id, DbType.String);
            parameters.Add("@Email", email, DbType.String);

            using (var conn = new SqlConnection(connectString))
            {
                employee = conn.QueryFirstOrDefault<Employee>("CheckEmployeeEmail", parameters, commandType: CommandType.StoredProcedure);
            }
            return employee == null;
        }
    }
}
