using Dapper;
using Sonlen.WebAdmin.Model;
using Sonlen.WebAdmin.Model.Utility;
using System.Data;
using System.Data.SqlClient;

namespace Sonlen.AdminWebAPI.Service
{
    /// <summary>
    /// 公司資料
    /// </summary>
    public class CompanyService : ICompanyService
    {
        private readonly string connectString;

        public CompanyService(IConfiguration configuration)
        {
            connectString = configuration["ConnectionStrings:DefaultConnection"];
        }

        public IDbConnection Connection
        {
            get { return new SqlConnection(connectString); }
        }

        public string DeleteData(Company item)
        {
            string msg = string.Empty;
            using (var conn = Connection)
            {
                DynamicParameters parameters = item.ToDynamicParameters();
                int result = conn.Execute("DelCompany", parameters, commandType: CommandType.StoredProcedure);
                if (result <= 0)
                    msg = "發生未知錯誤";
            }

            return msg;
        }

        public IEnumerable<Company> GetAllDatas()
        {
            IEnumerable<Company> leave;
            using (var conn = new SqlConnection(connectString))
            {
                leave = conn.Query<Company>("GetAllCompany", commandType: CommandType.StoredProcedure);
            }
            return leave;
        }

        public Company? GetDataByID(string id)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@ID", id, DbType.String);

            using (var conn = new SqlConnection(connectString))
            {
                return conn.QueryFirstOrDefault<Company>("GetCompanyByID", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public string InsertData(Company item)
        {
            string msg = string.Empty;
            using (var conn = Connection)
            {
                DynamicParameters parameters = item.ToDynamicParameters();
                int result = conn.Execute("InsCompany", parameters, commandType: CommandType.StoredProcedure);
                if (result <= 0)
                    msg = "發生未知錯誤";
            }

            return msg;
        }

        public string UpdateData(Company item)
        {
            string msg = string.Empty;
            using (var conn = Connection)
            {
                DynamicParameters parameters = item.ToDynamicParameters();
                int result = conn.Execute("UpdCompany", parameters, commandType: CommandType.StoredProcedure);
                if (result <= 0)
                    msg = "發生未知錯誤";
            }

            return msg;
        }
    }
}
