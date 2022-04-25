using Dapper;
using Sonlen.WebAdmin.Model;
using Sonlen.WebAdmin.Model.Utility;
using System.Data;
using System.Data.SqlClient;
using System.Transactions;

namespace Sonlen.AdminWebAPI.Service
{
    public class WorkOvertimeService : IWorkOvertimeService
    {
        private readonly string connectString;

        public WorkOvertimeService(IConfiguration configuration)
        {
            connectString = configuration["ConnectionStrings:DefaultConnection"];
        }

        public IDbConnection Connection
        {
            get { return new SqlConnection(connectString); }
        }

        /** 取得全部加班紀錄*/
        public IEnumerable<WorkOvertimeViewModel> GetAllWorkOvertime()
        {
            IEnumerable<WorkOvertimeViewModel> workOvertime;
            using (var conn = new SqlConnection(connectString))
            {
                workOvertime = conn.Query<WorkOvertimeViewModel>("GetAllWorkOvertime", commandType: CommandType.StoredProcedure);
            }
            return workOvertime;
        }

        /** 從 EmployeeID 取得加班紀錄*/
        public IEnumerable<WorkOvertime> GetWorkOvertimeByEID(string employeeID)
        {
            IEnumerable<WorkOvertime> workOvertime;
            using (var conn = new SqlConnection(connectString))
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@EmployeeID", employeeID, DbType.String);
                workOvertime = conn.Query<WorkOvertime>("GetWorkOvertimeByEID", parameters, commandType: CommandType.StoredProcedure);
            }
            return workOvertime;
        }

        /** 申請加班*/
        public int AddWorkOvertime(WorkOvertimeViewModel overtime)
        {
            int result;
            if (string.IsNullOrEmpty(overtime.EmployeeID))
            {
                result = -1; //未輸入請假資料
            }
            else
            {
                using (TransactionScope transactionScope = new TransactionScope())
                using (var conn = Connection)
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@EmployeeID", overtime.EmployeeID, DbType.String);
                    parameters.Add("@OverDate", overtime.OverDate, DbType.Date);

                    WorkOvertime workOvertime = conn.QueryFirstOrDefault<WorkOvertime>("GetWorkOvertime", parameters, commandType: CommandType.StoredProcedure);
                    if (workOvertime == null)
                    {
                        parameters = new DynamicParameters();
                        parameters.Add("@EmployeeID", Setting.LEAVE_APPROVED_ID, DbType.String);
                        parameters.Add("@Content", $"{overtime.EmployeeName} 於 {overtime.OverDate:yyyy/MM/dd} 申請加班，請去審核是否核准。", DbType.String);
                        parameters.Add("@CreateDate", DateTime.Now, DbType.DateTime);
                        parameters.Add("@NoticeId", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);
                        conn.Execute("InsNotice", parameters, commandType: CommandType.StoredProcedure);
                        int noticeId = parameters.Get<int>("@NoticeId");
                        if (noticeId < 0)
                        {
                            result = -99;//未知錯誤
                            return result;
                        }
                        workOvertime = new WorkOvertime(overtime);
                        workOvertime.NoticeId = noticeId;
                        parameters = workOvertime.ToDynamicParameters();
                        result = conn.Execute("InsWorkOvertime", parameters, commandType: CommandType.StoredProcedure);
                        if (result > 0)
                            transactionScope.Complete();
                    }
                    else
                    {
                        result = -2; // 當天已經有申請紀錄
                    }
                }
            }
            return result;
        }

        /** 取消申請加班*/
        public int CancelWorkOvertime(WorkOvertime overtime)
        {
            int result;
            if (string.IsNullOrEmpty(overtime.EmployeeID))
            {
                result = -1; //未輸入請假資料
            }
            else
            {
                using (var conn = Connection)
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@EmployeeID", overtime.EmployeeID, DbType.String);
                    parameters.Add("@OverDate", overtime.OverDate, DbType.Date);
                    result = conn.Execute("DelWorkOvertime", parameters, commandType: CommandType.StoredProcedure);
                }
            }
            return result;
        }

        /** 同意申請加班*/
        public int AgreeWorkOvertime(WorkOvertime overtime)
        {
            int result = 0;
            using (TransactionScope transactionScope = new TransactionScope())
            using (var conn = new SqlConnection(connectString))
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@EmployeeID", overtime.EmployeeID, DbType.String);
                parameters.Add("@OverDate", overtime.OverDate, DbType.Date);
                result = conn.Execute("AgreeWorkOvertime", parameters, commandType: CommandType.StoredProcedure);

                parameters = new DynamicParameters();
                parameters.Add("@EmployeeID", overtime.EmployeeID, DbType.String);
                parameters.Add("@Content", $"管理員已同意您於{overtime.OverDate.ToTWDateString()}的申請加班。", DbType.String);
                parameters.Add("@CreateDate", DateTime.Now, DbType.DateTime);
                conn.Execute("InsNotice", parameters, commandType: CommandType.StoredProcedure);

                transactionScope.Complete();
            }
            return result;
        }

        /** 駁回申請加班*/
        public int DisagreeWorkOvertime(WorkOvertime overtime)
        {
            int result = 0;
            using (TransactionScope transactionScope = new TransactionScope())
            using (var conn = new SqlConnection(connectString))
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@EmployeeID", overtime.EmployeeID, DbType.String);
                parameters.Add("@OverDate", overtime.OverDate, DbType.Date);
                result = conn.Execute("DisagreeWorkOvertime", parameters, commandType: CommandType.StoredProcedure);

                parameters = new DynamicParameters();
                parameters.Add("@EmployeeID", overtime.EmployeeID, DbType.String);
                parameters.Add("@Content", $"管理員已駁回您於{overtime.OverDate.ToTWDateString()}的申請加班。", DbType.String);
                parameters.Add("@CreateDate", DateTime.Now, DbType.DateTime);
                conn.Execute("InsNotice", parameters, commandType: CommandType.StoredProcedure);

                transactionScope.Complete();
            }
            return result;
        }
    }
}
