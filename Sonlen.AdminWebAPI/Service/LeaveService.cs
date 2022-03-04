using Dapper;
using Sonlen.WebAdmin.Model;
using Sonlen.WebAdmin.Model.Utility;
using System.Data;
using System.Data.SqlClient;

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

                        //如果有包含13點，減去午休的一小時
                        if (leaveEndHour >= 13 && leaveStartHour <= 13)
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
                            LeaveStartTime = $"{leave.LeaveStartTime.Substring(0, 2)}:{leave.LeaveStartTime.Substring(2, 2)}" ,
                            LeaveEndTime = $"{leave.LeaveEndTime.Substring(0, 2)}:{leave.LeaveEndTime.Substring(2, 2)}",
                            Accept = 0,
                            LeaveHour = leaveHour
                        };
                        if (leave.File != null)
                        {
                            string ext = Path.GetExtension(leave.File.FileName);
                            leave.File.FileName = leave.Employee.EmployeeID + "_" + leave.LeaveDate.ToString("yyyyMMdd") + "_" + leave.LeaveType;
                            _fileService.UploadFile(leave.File.FileContent ?? Array.Empty<byte>(), leave.File.FileName, ext);
                            leaveRecord.Prove = leave.File.FileName;
                        }
                        parameters = leaveRecord.ToDynamicParameters();
                        result = conn.Execute("LeaveOff", parameters, commandType: CommandType.StoredProcedure);
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

            return result;
        }

        /** 取得請假類別*/
        public IEnumerable<LeaveType> GetLeaveTypes()
        {
            IEnumerable<LeaveType> leave;
            using (var conn = new SqlConnection(connectString))
            {
                leave = conn.Query<LeaveType>("GetLeaveType", commandType: CommandType.StoredProcedure);
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
        
    }
}
