using Dapper;
using Sonlen.WebAdmin.Model;
using Sonlen.WebAdmin.Model.Utility;
using System.Data;
using System.Data.SqlClient;

namespace Sonlen.AdminWebAPI.Service
{
    public class PunchRecordService : IPunchRecordService
    {
        private readonly string connectString;

        public PunchRecordService(IConfiguration configuration)
        {
            connectString = configuration["ConnectionStrings:DefaultConnection"];
        }

        public IDbConnection Connection
        {
            get { return new SqlConnection(connectString); }
        }
        public string DeleteData(Punch item)
        {
            string msg = string.Empty;
            using (var conn = Connection)
            {
                DynamicParameters parameters = item.ToDynamicParameters();
                int result = conn.Execute("DelPunchRecord", parameters, commandType: CommandType.StoredProcedure);
                if (result <= 0)
                    msg = "發生未知錯誤";
            }

            return msg;
        }

        public IEnumerable<Punch> GetAllDatas()
        {
            IEnumerable<Punch> leave;
            using (var conn = new SqlConnection(connectString))
            {
                leave = conn.Query<Punch>("GetAllPunchRecord", commandType: CommandType.StoredProcedure);
            }
            return leave;
        }

        public IEnumerable<Punch> GetDatasByEID(string eid)
        {
            IEnumerable<Punch> leave;
            using (var conn = new SqlConnection(connectString))
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@EmployeeID", eid, DbType.String);

                leave = conn.Query<Punch>("GetPunchRecordByEID", parameters, commandType: CommandType.StoredProcedure);
            }
            return leave;
        }

        public Punch? GetDataByID(string pks)
        {
            string[] pk = pks.Split(',');
            if (pk.Length == 2)
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@EmployeeID", pk[0], DbType.String);
                parameters.Add("@PunchDate", pk[1], DbType.Date);

                using (var conn = new SqlConnection(connectString))
                {
                    return conn.QueryFirstOrDefault<Punch>("GetPunchRecord", parameters, commandType: CommandType.StoredProcedure);
                }
            }
            throw new Exception();
        }

        public string InsertData(Punch item)
        {
            string msg = string.Empty;
            using (var conn = Connection)
            {
                DynamicParameters parameters = item.ToDynamicParameters();
                int result = conn.Execute("InsPunchRecord", parameters, commandType: CommandType.StoredProcedure);
                if (result <= 0)
                    msg = "發生未知錯誤";
            }

            return msg;
        }

        public string UpdateData(Punch item)
        {
            string msg = string.Empty;
            using (var conn = Connection)
            {
                DynamicParameters parameters = item.ToDynamicParameters();
                int result = conn.Execute("UpdPunchRecord", parameters, commandType: CommandType.StoredProcedure);
                if (result <= 0)
                    msg = "發生未知錯誤";
            }

            return msg;
        }
    }
}
