using Dapper;
using Sonlen.WebAdmin.Model;
using Sonlen.WebAdmin.Model.Utility;
using System.Data;
using System.Data.SqlClient;

namespace Sonlen.AdminWebAPI.Service
{
    public class LeaveRecordService : ILeaveRecordService
    {
        private readonly string connectString;
        
        public LeaveRecordService(IConfiguration configuration)
        {
            connectString = configuration["ConnectionStrings:DefaultConnection"];    
        }

        public IDbConnection Connection
        {
            get { return new SqlConnection(connectString); }
        }
        public string DeleteData(LeaveRecord item)
        {
            string msg = string.Empty;
            using (var conn = Connection)
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@EmployeeID", item.EmployeeID, DbType.String);
                parameters.Add("@LeaveDate", item.LeaveDate, DbType.Date);
                int result = conn.Execute("DelLeaveRecord", parameters, commandType: CommandType.StoredProcedure);
                if (result <= 0)
                    msg = "發生未知錯誤";
            }

            return msg;
        }

        public IEnumerable<LeaveRecord> GetAllDatas()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<LeaveRecord> GetDatasByEID(string eid)
        {
            IEnumerable<LeaveRecord> leave;
            using (var conn = new SqlConnection(connectString))
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@EmployeeID", eid, DbType.String);

                leave = conn.Query<LeaveRecord>("GetLeaveRecordByEID", parameters, commandType: CommandType.StoredProcedure);
            }
            return leave;
        }

        public LeaveRecord? GetDataByID(string pks)
        {
            string[] pk = pks.Split(',');
            if (pk.Length == 2)
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@EmployeeID", pk[0], DbType.String);
                parameters.Add("@LeaveDate", pk[1], DbType.Date);

                using (var conn = new SqlConnection(connectString))
                {
                    return conn.QueryFirstOrDefault<LeaveRecord>("GetLeaveRecord", parameters, commandType: CommandType.StoredProcedure);
                }
            }
            throw new Exception();
        }

        public string InsertData(LeaveRecord item)
        {
            string msg = string.Empty;
            using (var conn = Connection)
            {
                DynamicParameters parameters = item.ToDynamicParameters();
                int result = conn.Execute("InsLeaveRecord", parameters, commandType: CommandType.StoredProcedure);
                if (result <= 0)
                    msg = "發生未知錯誤";
            }

            return msg;
        }

        public string UpdateData(LeaveRecord item)
        {
            string msg = string.Empty;
            using (var conn = Connection)
            {
                DynamicParameters parameters = item.ToDynamicParameters();
                int result = conn.Execute("UpdLeaveRecord", parameters, commandType: CommandType.StoredProcedure);
                if (result <= 0)
                    msg = "發生未知錯誤";
            }

            return msg;
        }

        public string Agree(LeaveRecord item)
        {
            string msg = string.Empty;
            using (var conn = Connection)
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@EmployeeID", item.EmployeeID, DbType.String);
                parameters.Add("@LeaveDate", item.LeaveDate, DbType.Date);
                int result = conn.Execute("AgreeLeaveRecord", parameters, commandType: CommandType.StoredProcedure);
                if (result <= 0)
                    msg = "發生未知錯誤";
            }

            return msg;
        }

        public string Disagree(LeaveRecord item)
        {
            string msg = string.Empty;
            using (var conn = Connection)
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@EmployeeID", item.EmployeeID, DbType.String);
                parameters.Add("@LeaveDate", item.LeaveDate, DbType.Date);
                int result = conn.Execute("DisagreeLeaveRecord", parameters, commandType: CommandType.StoredProcedure);
                if (result <= 0)
                    msg = "發生未知錯誤";
            }

            return msg;
        }

        public IEnumerable<LeaveRecordViewModel> GetAllViewDatas()
        {
            IEnumerable<LeaveRecordViewModel> leave;
            using (var conn = new SqlConnection(connectString))
            {
                leave = conn.Query<LeaveRecordViewModel>("GetAllLeaveRecord", commandType: CommandType.StoredProcedure);
            }
            return leave;
        }

        public IEnumerable<LeaveRecord> GetDatasByDate(AttendanceViewModel model)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@EmployeeID", model.EmployeeID, DbType.String);
            parameters.Add("@StartDate", new DateTime(model.Year + 1911, model.Month, 1), DbType.Date);
            parameters.Add("@EndDate", new DateTime(model.Year + 1911, model.Month, 1).AddMonths(1).AddDays(-1), DbType.Date);

            using (var conn = new SqlConnection(connectString))
            {
                return conn.Query<LeaveRecord>("GetLeaveRecordByDate", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public IEnumerable<LeaveRecord> GetDatasByYear(int year, string? employeeID = null)
        {
            DynamicParameters parameters = new DynamicParameters();
            if (!string.IsNullOrEmpty(employeeID))
            {
                parameters.Add("@EmployeeID", employeeID, DbType.String);
            }
            parameters.Add("@StartDate", new DateTime(year + 1911, 1, 1), DbType.Date);
            parameters.Add("@EndDate", new DateTime(year + 1912, 1, 1).AddDays(-1), DbType.Date);

            using (var conn = new SqlConnection(connectString))
            {
                return conn.Query<LeaveRecord>("GetLeaveRecordByDate", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public decimal GetSumHourByID(int typeID ,int year = 0, string? employeeID = null)
        {
            if (year == 0)
            {
                year = DateTime.Now.Year - 1911;
            }
            DynamicParameters parameters = new DynamicParameters();
            if (!string.IsNullOrEmpty(employeeID))
            {
                parameters.Add("@EmployeeID", employeeID, DbType.String);
            }
            parameters.Add("@TypeID", typeID, DbType.Int32);
            parameters.Add("@StartDate", new DateTime(year + 1911, 1, 1), DbType.Date);
            parameters.Add("@EndDate", new DateTime(year + 1912, 1, 1).AddDays(-1), DbType.Date);

            using (var conn = new SqlConnection(connectString))
            {
                return conn.QueryFirstOrDefault<decimal>("GetSumLeaveHourByID", parameters, commandType: CommandType.StoredProcedure);
            }
        }
    }
}
