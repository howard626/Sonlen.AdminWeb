using Dapper;
using Sonlen.WebAdmin.Model;
using Sonlen.WebAdmin.Model.Utility;
using System.Data;
using System.Data.SqlClient;
using System.Transactions;

namespace Sonlen.AdminWebAPI.Service
{
    public class LeaveService : ILeaveService
    {
        private readonly string connectString;

        public IFileService _fileService;
        public LeaveService(IConfiguration configuration, IFileService fileService)
        {
            connectString = configuration["ConnectionStrings:DefaultConnection"];
            _fileService = fileService;
        }

        public IDbConnection Connection
        {
            get { return new SqlConnection(connectString); }
        }

        /** 請假*/
        public int DayOff(LeaveViewModel leave)
        {
            int result;
            if (string.IsNullOrEmpty(leave.LeaveStartTime) || string.IsNullOrEmpty(leave.LeaveEndTime))
            {
                result = -3; //未輸入請假時間
            }
            else
            {
                using (TransactionScope transactionScope = new TransactionScope())
                using (var conn = Connection)
                {
                    if (leave.Employee != null)
                    {
                        DynamicParameters parameters = new DynamicParameters();
                        parameters.Add("@EmployeeID", leave.Employee.EmployeeID, DbType.String);
                        parameters.Add("@LeaveDate", leave.LeaveDate, DbType.Date);

                        LeaveRecord leaveRecord = conn.QueryFirstOrDefault<LeaveRecord>("GetLeaveRecord", parameters, commandType: CommandType.StoredProcedure);
                        if (leaveRecord == null)
                        {
                            DateTime now = DateTime.Now;
                            int leaveStartHour = int.Parse(leave.LeaveStartTime.Substring(0, 2));
                            int leaveStartMin = int.Parse(leave.LeaveStartTime.Substring(2, 2));
                            int leaveEndHour = int.Parse(leave.LeaveEndTime.Substring(0, 2));
                            int leaveEndMin = int.Parse(leave.LeaveEndTime.Substring(2, 2));
                            int hour = leaveEndHour - leaveStartHour;
                            int min = leaveEndMin - leaveStartMin;

                            parameters = new DynamicParameters();
                            parameters.Add("@EmployeeID", Setting.LEAVE_APPROVED_ID, DbType.String);
                            parameters.Add("@Content", $"{leave.Employee.EmployeeName} 於 {leave.LeaveDate:yyyy/MM/dd} {leave.LeaveStartTime} ~ {leave.LeaveEndTime} 請 {GetLeaveType(leave.LeaveType).LeaveName}，請去審核是否准許。", DbType.String);
                            parameters.Add("@CreateDate", DateTime.Now, DbType.DateTime);
                            parameters.Add("@NoticeId", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);
                            conn.Execute("InsNotice", parameters, commandType: CommandType.StoredProcedure);
                            int noticeId = parameters.Get<int>("@NoticeId");
                            if (noticeId < 0)
                            {
                                result = -99;//未知錯誤
                                return result;
                            }
                            //如果有包含13點，減去午休的一小時
                            if (leaveEndHour > 13 && leaveStartHour < 13)
                            {
                                hour -= 1;
                            }
                            if (min < 0)
                            {
                                min += 60;
                                hour -= 1;
                            }
                            decimal leaveHour = hour + (decimal)min / 60;
                            leaveRecord = new LeaveRecord()
                            {
                                EmployeeID = leave.Employee.EmployeeID,
                                LeaveDate = leave.LeaveDate,
                                LeaveType = leave.LeaveType,
                                LeaveStartTime = $"{leave.LeaveStartTime.Substring(0, 2)}:{leave.LeaveStartTime.Substring(2, 2)}",
                                LeaveEndTime = $"{leave.LeaveEndTime.Substring(0, 2)}:{leave.LeaveEndTime.Substring(2, 2)}",
                                Accept = 0,
                                LeaveHour = leaveHour,
                                NoticeId = noticeId
                            };
                            if (leave.File != null)
                            {
                                string ext = Path.GetExtension(leave.File.FileName);
                                leave.File.FileName = $"{leave.Employee.EmployeeID}_{leave.LeaveDate:yyyyMMdd}_{leave.LeaveType}{ext}";
                                _fileService.UploadFile(leave.File.FileContent ?? Array.Empty<byte>(), leave.File.FileName);
                                leaveRecord.Prove = leave.File.FileName;
                            }
                            parameters = leaveRecord.ToDynamicParameters();
                            result = conn.Execute("LeaveOff", parameters, commandType: CommandType.StoredProcedure);
                            if(result > 0)
                                transactionScope.Complete();
                        }
                        else
                        {
                            result = -2; // 當天已經有請假紀錄
                        }
                    }
                    else
                    {
                        result = -1; // Employee IS NULL
                    }
                }
            }
            return result;
        }

        /** 取消請假*/
        public int CancelDayOff(LeaveRecord leave)
        {
            int result;
            if (string.IsNullOrEmpty(leave.EmployeeID))
            {
                result = -1; //未輸入請假資料
            }
            else
            {
                using (var conn = Connection)
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@EmployeeID", leave.EmployeeID, DbType.String);
                    parameters.Add("@LeaveDate", leave.LeaveDate, DbType.Date);
                    result = conn.Execute("DelLeaveRecord", parameters, commandType: CommandType.StoredProcedure);
                }
            }
            return result;
        }

        /** 取得請假類別*/
        public IEnumerable<LeaveType> GetLeaveTypes()
        {
            IEnumerable<LeaveType> leave;
            using (var conn = new SqlConnection(connectString))
            {
                leave = conn.Query<LeaveType>("GetAllLeaveType", commandType: CommandType.StoredProcedure);
            }
            return leave;
        }

        /** 取得請假類別*/
        public LeaveType GetLeaveType(int id)
        {
            LeaveType leave;
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@Id", id, DbType.Int32);

            using (var conn = new SqlConnection(connectString))
            {
                leave = conn.QueryFirstOrDefault<LeaveType>("GetLeaveTypeById", parameters, commandType: CommandType.StoredProcedure);

            }
            return leave;
        }

        /** 從 EmployeeID 取得請假紀錄*/
        public IEnumerable<LeaveRecord> GetLeaveRecordByEID(string employeeID)
        {
            IEnumerable<LeaveRecord> leave;
            using (var conn = new SqlConnection(connectString))
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@EmployeeID", employeeID, DbType.String);
                leave = conn.Query<LeaveRecord>("GetLeaveRecordByEID", parameters, commandType: CommandType.StoredProcedure);   
            }
            return leave;
        }

        /** 取得全部請假紀錄*/
        public IEnumerable<LeaveRecordViewModel> GetAllLeaveRecord()
        {
            IEnumerable<LeaveRecordViewModel> leave;
            using (var conn = new SqlConnection(connectString))
            {
                leave = conn.Query<LeaveRecordViewModel>("GetAllLeaveRecord", commandType: CommandType.StoredProcedure);
            }
            return leave;
        }

        /** 同意請假*/
        public int AgreeLeaveRecord(string employeeID, DateTime leaveDate)
        {
            int result = 0;
            using (var conn = new SqlConnection(connectString))
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@EmployeeID", employeeID, DbType.String);
                parameters.Add("@LeaveDate", leaveDate, DbType.Date);
                result = conn.Execute("AgreeLeaveRecord", parameters, commandType: CommandType.StoredProcedure);

                parameters = new DynamicParameters();
                parameters.Add("@EmployeeID", employeeID, DbType.String);
                parameters.Add("@Content", $"管理員已同意您於{leaveDate.ToTWDateString()}的請假。", DbType.String);
                parameters.Add("@CreateDate", DateTime.Now, DbType.DateTime);
                conn.Execute("InsNotice", parameters, commandType: CommandType.StoredProcedure);
            }
            return result;
        }

        /** 駁回請假*/
        public int DisagreeLeaveRecord(string employeeID, DateTime leaveDate)
        {
            int result = 0;
            using (var conn = new SqlConnection(connectString))
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@EmployeeID", employeeID, DbType.String);
                parameters.Add("@LeaveDate", leaveDate, DbType.Date);
                result = conn.Execute("DisagreeLeaveRecord", parameters, commandType: CommandType.StoredProcedure);

                parameters = new DynamicParameters();
                parameters.Add("@EmployeeID", employeeID, DbType.String);
                parameters.Add("@Content", $"管理員已駁回您於{leaveDate.ToTWDateString()}的請假。", DbType.String);
                parameters.Add("@CreateDate", DateTime.Now, DbType.DateTime);
                conn.Execute("InsNotice", parameters, commandType: CommandType.StoredProcedure);
            }
            return result;
        }

        /** 取得請假證明檔案*/
        public UploadFile GetLeaveProve(UploadFile file)
        {
            return _fileService.DownloadFile(file.FileName);
        }
    }
}
