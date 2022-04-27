using Dapper;
using Sonlen.AdminWebAPI.Data;
using Sonlen.WebAdmin.Model;
using Sonlen.WebAdmin.Model.Utility;
using System.Data;
using System.Data.SqlClient;

namespace Sonlen.AdminWebAPI.Service
{
    public class EmployeeService : IEmployeeService
    {
        private readonly string connectString;

        public EmployeeService(IConfiguration configuration)
        {
            connectString = configuration["ConnectionStrings:DefaultConnection"];
        }

        public IDbConnection Connection
        {
            get { return new SqlConnection(connectString); }
        }

        public Employee? GetDataByEmail(string email)
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

        /** 新增員工*/
        public string InsertData(Employee employee)
        {
            string msg = string.Empty;

            if (CheckEmail(employee.Email, employee.EmployeeID))
            {
                if (GetDataByID(employee.EmployeeID) == null)
                {
                    using (var conn = Connection)
                    {
                        employee.LoginKey = employee.EmployeeID.ToMD5();
                        DynamicParameters parameters = employee.ToDynamicParameters();
                        int result = conn.Execute("InsEmployee", parameters, commandType: CommandType.StoredProcedure);
                        if (result <= 0)
                            msg = "發生未知錯誤";
                    }
                }
                else
                {
                    msg = "此身分證字號已存在";
                }
            }
            else
            {
                msg = "此 Email 已存在";
            }

            return msg;
        }

        /** 以 身分證字號 取得員工資料*/
        public Employee? GetDataByID(string id)
        {
            Employee? employee;
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@EmployeeID", id, DbType.String);

            using (var conn = new SqlConnection(connectString))
            {
                employee = conn.QueryFirstOrDefault<Employee>("GetEmployeeByID", parameters, commandType: CommandType.StoredProcedure);
            }
            return employee;
        }

        public IEnumerable<Employee> GetAllDatas()
        {
            IEnumerable<Employee> employee;
            using (var conn = new SqlConnection(connectString))
            {
                employee = conn.Query<Employee>("GetAllEmployee", commandType: CommandType.StoredProcedure);
            }
            return employee;
        }

        public string UpdateData(Employee item)
        {
            string msg = string.Empty;
            if (CheckEmail(item.Email, item.EmployeeID))
            {
                if (GetDataByID(item.EmployeeID) == null)
                {
                    msg = "身份證字號不存在";
                }
                else
                {
                    using (var conn = new SqlConnection(connectString))
                    {
                        DynamicParameters parameters = item.ToDynamicParameters();
                        int result = conn.Execute("UpdEmployee", parameters, commandType: CommandType.StoredProcedure);
                    }
                }
            }
            else
            {
                msg = "Email 已存在";
            }
            return msg;
        }

        public string DeleteData(Employee item)
        {
            string msg = string.Empty;

            using (var conn = new SqlConnection(connectString))
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@EmployeeID", item.EmployeeID, DbType.String);
                int result = conn.Execute("DelEmployee", parameters, commandType: CommandType.StoredProcedure);
                if (result <= 0)
                    msg = "發生未知錯誤";
            }
            return msg;
        }
    }
}
