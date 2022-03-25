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

        /** 新增員工*/
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

        /** 更新員工資料*/
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

        /** 刪除員工*/
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

        /** 取得全部員工資料*/
        public IEnumerable<Employee> GetAllEmployees()
        {
            IEnumerable<Employee> employee;
            using (var conn = new SqlConnection(connectString))
            {
                employee = conn.Query<Employee>("GetAllEmployee", commandType: CommandType.StoredProcedure);
            }
            return employee;
        }

        /** 以 身分證字號 取得員工資料*/
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

        /** 以 email 取得員工資料*/
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

        /** 檢查是否已存在此 email*/
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

        /** 取得員工通知*/
        public IEnumerable<Notice> GeNoticeByEID(Employee employee)
        {
            IEnumerable<Notice> notices;
            using (var conn = new SqlConnection(connectString))
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@EmployeeID", employee.EmployeeID, DbType.String);
                notices = conn.Query<Notice>("GetNoticeByEID", parameters, commandType: CommandType.StoredProcedure);
            }
            return notices;
        }

        /** 設定通知為已讀*/
        public int SetNoticeToIsRead(List<Notice> notices)
        {
            int result = 0;

            using (var conn = new SqlConnection(connectString))
            {
                foreach (Notice notice in notices)
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@Id", notice.Id, DbType.Int32);
                    parameters.Add("@IsRead", 1, DbType.Int32);
                    result += conn.Execute("UpdNotice", parameters, commandType: CommandType.StoredProcedure);
                }
            }

            return result;
        }
    }
}
