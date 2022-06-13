using Dapper;
using Sonlen.WebAdmin.Model;
using Sonlen.WebAdmin.Model.Utility;
using System.Data;
using System.Data.SqlClient;

namespace Sonlen.AdminWebAPI.Service
{
    public class WorkOvertimeRecordService : IWorkOvertimeRecordService
    {
        private readonly string connectString;

        public WorkOvertimeRecordService(IConfiguration configuration)
        {
            connectString = configuration["ConnectionStrings:DefaultConnection"];
        }

        public IDbConnection Connection
        {
            get { return new SqlConnection(connectString); }
        }
        public string DeleteData(WorkOvertime item)
        {
            string msg = string.Empty;
            using (var conn = Connection)
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@EmployeeID", item.EmployeeID, DbType.String);
                parameters.Add("@OverDate", item.OverDate, DbType.Date);
                int result = conn.Execute("DelWorkOvertime", parameters, commandType: CommandType.StoredProcedure);
                if (result <= 0)
                    msg = "發生未知錯誤";
            }

            return msg;
        }

        

        public IEnumerable<WorkOvertime> GetDatasByEID(string eid)
        {
            IEnumerable<WorkOvertime> leave;
            using (var conn = new SqlConnection(connectString))
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@EmployeeID", eid, DbType.String);

                leave = conn.Query<WorkOvertime>("GetWorkOvertimeByEID", parameters, commandType: CommandType.StoredProcedure);
            }
            return leave;
        }

        /// <summary>
        /// 以 PK 取得資料(以,區分各欄位(員工身分證,加班日期))
        /// </summary>
        /// <param name="pks">pks = 員工身分證,加班日期</param>
        /// <returns></returns>
        public WorkOvertime? GetDataByID(string pks)
        {
            string[] pk = pks.Split(',');
            if (pk.Length == 2)
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@EmployeeID", pk[0], DbType.String);
                parameters.Add("@OverDate", pk[1], DbType.Date);

                using (var conn = new SqlConnection(connectString))
                {
                    return conn.QueryFirstOrDefault<WorkOvertime>("GetWorkOvertime", parameters, commandType: CommandType.StoredProcedure);
                }
            }
            throw new Exception();
        }

        public string InsertData(WorkOvertime item)
        {
            string msg = string.Empty;
            using (var conn = Connection)
            {
                DynamicParameters parameters = item.ToDynamicParameters();
                int result = conn.Execute("InsWorkOvertime", parameters, commandType: CommandType.StoredProcedure);
                if (result <= 0)
                    msg = "發生未知錯誤";
            }

            return msg;
        }

        public string UpdateData(WorkOvertime item)
        {
            string msg = string.Empty;
            using (var conn = Connection)
            {
                DynamicParameters parameters = item.ToDynamicParameters();
                int result = conn.Execute("UpdWorkOvertime", parameters, commandType: CommandType.StoredProcedure);
                if (result <= 0)
                    msg = "發生未知錯誤";
            }

            return msg;
        }

        public string Agree(WorkOvertime item)
        {
            string msg = string.Empty;
            using (var conn = Connection)
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@EmployeeID", item.EmployeeID, DbType.String);
                parameters.Add("@OverDate", item.OverDate, DbType.Date);
                int result = conn.Execute("AgreeWorkOvertime", parameters, commandType: CommandType.StoredProcedure);
                if (result <= 0)
                    msg = "發生未知錯誤";
            }

            return msg;
        }

        public string Disagree(WorkOvertime item)
        {
            string msg = string.Empty;
            using (var conn = Connection)
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@EmployeeID", item.EmployeeID, DbType.String);
                parameters.Add("@OverDate", item.OverDate, DbType.Date);
                int result = conn.Execute("DisagreeWorkOvertime", parameters, commandType: CommandType.StoredProcedure);
                if (result <= 0)
                    msg = "發生未知錯誤";
            }

            return msg;
        }

        public IEnumerable<WorkOvertimeViewModel> GetAllViewDatas()
        {
            IEnumerable<WorkOvertimeViewModel> item;
            using (var conn = new SqlConnection(connectString))
            {
                item = conn.Query<WorkOvertimeViewModel>("GetAllWorkOvertime", commandType: CommandType.StoredProcedure);
            }
            return item;
        }

        public decimal GetSum133HourByDate(string employeeID, string logMon)
        {
            
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@EmployeeID", employeeID, DbType.String);
            parameters.Add("@StartDate", (logMon + "01").TWDateStringToDate(), DbType.Date);
            parameters.Add("@EndDate", (logMon + "01").TWDateStringToDate().AddMonths(1).AddDays(-1), DbType.Date);

            using (var conn = new SqlConnection(connectString))
            {
                return conn.QueryFirstOrDefault<decimal>("GetSum133HourByDate", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public decimal GetSum166HourByDate(string employeeID, string logMon)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@EmployeeID", employeeID, DbType.String);
            parameters.Add("@StartDate", (logMon + "01").TWDateStringToDate(), DbType.Date);
            parameters.Add("@EndDate", (logMon + "01").TWDateStringToDate().AddMonths(1).AddDays(-1), DbType.Date);

            using (var conn = new SqlConnection(connectString))
            {
                return conn.QueryFirstOrDefault<decimal>("GetSum166HourByDate", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public decimal GetSum200HourByDate(string employeeID, string logMon)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@EmployeeID", employeeID, DbType.String);
            parameters.Add("@StartDate", (logMon + "01").TWDateStringToDate(), DbType.Date);
            parameters.Add("@EndDate", (logMon + "01").TWDateStringToDate().AddMonths(1).AddDays(-1), DbType.Date);

            using (var conn = new SqlConnection(connectString))
            {
                return conn.QueryFirstOrDefault<decimal>("GetSum200HourByDate", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        #region 未實作項目

        public IEnumerable<WorkOvertime> GetAllDatas()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
