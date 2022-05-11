using Dapper;
using Sonlen.WebAdmin.Model;
using Sonlen.WebAdmin.Model.Utility;
using System.Data;
using System.Data.SqlClient;

namespace Sonlen.AdminWebAPI.Service
{
    public class LeaveTypeService : ILeaveTypeService
    {
        private readonly string connectString;

        public LeaveTypeService(IConfiguration configuration)
        {
            connectString = configuration["ConnectionStrings:DefaultConnection"];
        }

        public IDbConnection Connection
        {
            get { return new SqlConnection(connectString); }
        }

        public string DeleteData(LeaveType item)
        {
            string msg = string.Empty;
            using (var conn = Connection)
            {
                DynamicParameters parameters = item.ToDynamicParameters();
                int result = conn.Execute("DelLeaveType", parameters, commandType: CommandType.StoredProcedure);
                if (result <= 0)
                    msg = "發生未知錯誤";
            }

            return msg;
        }

        public IEnumerable<LeaveType> GetAllDatas()
        {
            IEnumerable<LeaveType> leave;
            using (var conn = new SqlConnection(connectString))
            {
                leave = conn.Query<LeaveType>("GetAllLeaveType", commandType: CommandType.StoredProcedure);
            }
            return leave;
        }

        public LeaveType? GetDataByID(string id)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@Id", id, DbType.Int32);

            using (var conn = new SqlConnection(connectString))
            {
                return conn.QueryFirstOrDefault<LeaveType>("GetLeaveTypeById", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public string InsertData(LeaveType item)
        {
            string msg = string.Empty;
            using (var conn = Connection)
            {
                DynamicParameters parameters = item.ToDynamicParameters();
                int result = conn.Execute("InsLeaveType", parameters, commandType: CommandType.StoredProcedure);
                if (result <= 0)
                    msg = "發生未知錯誤";
            }

            return msg;
        }

        public string UpdateData(LeaveType item)
        {
            string msg = string.Empty;
            using (var conn = Connection)
            {
                DynamicParameters parameters = item.ToDynamicParameters();
                int result = conn.Execute("UpdLeaveType", parameters, commandType: CommandType.StoredProcedure);
                if (result <= 0)
                    msg = "發生未知錯誤";
            }

            return msg;
        }
    }
}
