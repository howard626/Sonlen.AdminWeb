using Dapper;
using Sonlen.WebAdmin.Model;
using Sonlen.WebAdmin.Model.Utility;
using System.Data;
using System.Data.SqlClient;
using System.Transactions;

namespace Sonlen.AdminWebAPI.Service
{
    public class NoticeService : INoticeService
    {
        private readonly string connectString;

        public NoticeService(IConfiguration configuration)
        {
            connectString = configuration["ConnectionStrings:DefaultConnection"];
        }

        public IDbConnection Connection
        {
            get { return new SqlConnection(connectString); }
        }

        public string DeleteData(Notice item)
        {
            string msg = string.Empty;
            using (var conn = Connection)
            {
                DynamicParameters parameters = item.ToDynamicParameters();
                int result = conn.Execute("DelNotice", parameters, commandType: CommandType.StoredProcedure);
                if (result <= 0)
                    msg = "發生未知錯誤";
            }

            return msg;
        }

        public IEnumerable<Notice> GetAllDatas()
        {
            IEnumerable<Notice> leave;
            using (var conn = new SqlConnection(connectString))
            {
                leave = conn.Query<Notice>("GetAllNotice", commandType: CommandType.StoredProcedure);
            }
            return leave;
        }

        public IEnumerable<Notice> GetDatasByEID(string eid)
        {
            IEnumerable<Notice> notices;
            using (var conn = new SqlConnection(connectString))
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@EmployeeID", eid, DbType.String);
                notices = conn.Query<Notice>("GetNoticeByEID", parameters, commandType: CommandType.StoredProcedure);
            }
            return notices;
        }

        public Notice? GetDataByID(string id)
        {
            using (var conn = new SqlConnection(connectString))
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@ID", id, DbType.Int32);
                return conn.QueryFirstOrDefault<Notice>("GetNoticeByID", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public string InsertData(Notice item)
        {
            string msg = string.Empty;
            using (var conn = Connection)
            {
                DynamicParameters parameters = new DynamicParameters();

                parameters.Add("@EmployeeID", item.EmployeeID, DbType.String);
                parameters.Add("@Content", item.Content, DbType.String);
                parameters.Add("@CreateDate", item.CreateDate, DbType.DateTime);
                parameters.Add("@NoticeId", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);
                
                int result = conn.Execute("InsNotice", parameters, commandType: CommandType.StoredProcedure);
                if (result <= 0)
                    msg = "發生未知錯誤";
                else
                {
                    msg = parameters.Get<int>("@NoticeId").ToString();
                }
            }

            return msg;
        }

        public string UpdateData(Notice item)
        {
            string msg = string.Empty;
            using (var conn = Connection)
            {
                DynamicParameters parameters = item.ToDynamicParameters();
                int result = conn.Execute("UpdNotice", parameters, commandType: CommandType.StoredProcedure);
                if (result <= 0)
                    msg = "發生未知錯誤";
            }

            return msg;
        }

        public int SetNoticeToIsRead(List<Notice> notices)
        {
            int result = 0;

            using (var conn = new SqlConnection(connectString))
            using (TransactionScope transactionScope = new TransactionScope())
            {
                foreach (Notice notice in notices)
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@Id", notice.Id, DbType.Int32);
                    parameters.Add("@IsRead", 1, DbType.Int32);
                    result = conn.Execute("UpdNotice", parameters, commandType: CommandType.StoredProcedure);
                    if (result <= 0) 
                    {
                        result = -99;
                        return result;
                    }
                }
                transactionScope.Complete();
            }

            return result;
        }
    }
}
