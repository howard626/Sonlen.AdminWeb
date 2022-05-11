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
    }
}
