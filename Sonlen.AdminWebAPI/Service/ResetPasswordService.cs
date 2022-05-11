using Dapper;
using Sonlen.WebAdmin.Model;
using Sonlen.WebAdmin.Model.Utility;
using System.Data;
using System.Data.SqlClient;

namespace Sonlen.AdminWebAPI.Service
{
    public class ResetPasswordService : IResetPasswordService
    {
        private readonly string connectString;

        public ResetPasswordService(IConfiguration configuration)
        {
            connectString = configuration["ConnectionStrings:DefaultConnection"];
        }

        public IDbConnection Connection
        {
            get { return new SqlConnection(connectString); }
        }
        public string DeleteData(ResetPassword item)
        {
            string msg = string.Empty;
            using (var conn = Connection)
            {
                DynamicParameters parameters = item.ToDynamicParameters();
                int result = conn.Execute("DelResetPasswordToken", parameters, commandType: CommandType.StoredProcedure);
                if (result <= 0)
                    msg = "發生未知錯誤";
            }

            return msg;
        }

        

        public ResetPassword? GetDataByID(string id)
        {

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@Email", id, DbType.String);

            using (var conn = new SqlConnection(connectString))
            {
                return conn.QueryFirstOrDefault<ResetPassword>("GetResetPasswordToken", parameters, commandType: CommandType.StoredProcedure);
            }

        }

        public string InsertData(ResetPassword item)
        {
            string msg = string.Empty;
            using (var conn = Connection)
            {
                DynamicParameters parameters = item.ToDynamicParameters();
                int result = conn.Execute("DelResetPasswordToken", parameters, commandType: CommandType.StoredProcedure);
                if (result <= 0)
                    msg = "發生未知錯誤";
            }

            return msg;
        }

        public string UpdateData(ResetPassword item)
        {
            string msg = string.Empty;
            using (var conn = Connection)
            {
                DynamicParameters parameters = item.ToDynamicParameters();
                int result = conn.Execute("UpdResetPasswordToken", parameters, commandType: CommandType.StoredProcedure);
                if (result <= 0)
                    msg = "發生未知錯誤";
            }

            return msg;
        }

        #region 未實作項目

        public IEnumerable<ResetPassword> GetAllDatas()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
