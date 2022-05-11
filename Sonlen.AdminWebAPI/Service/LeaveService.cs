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
        private readonly ILeaveTypeService _leaveTypeService;
        private readonly IFileService _fileService;
        private readonly ILeaveRecordService _leaveRecordService;
        private readonly INoticeService _noticeService;

        public LeaveService(ILeaveTypeService leaveTypeService
            , IFileService fileService
            , ILeaveRecordService leaveRecordService
            , INoticeService noticeService)
        {
            _fileService = fileService;
            _leaveRecordService = leaveRecordService;
            _leaveTypeService = leaveTypeService;
            _noticeService = noticeService;
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
                {
                    if (leave.Employee != null)
                    {
                        LeaveRecord? leaveRecord = _leaveRecordService.GetDataByID($"{leave.Employee.EmployeeID},{leave.LeaveDate}");
                        if (leaveRecord == null)
                        {
                            DateTime now = DateTime.Now;
                            int leaveStartHour = int.Parse(leave.LeaveStartTime.Substring(0, 2));
                            int leaveStartMin = int.Parse(leave.LeaveStartTime.Substring(2, 2));
                            int leaveEndHour = int.Parse(leave.LeaveEndTime.Substring(0, 2));
                            int leaveEndMin = int.Parse(leave.LeaveEndTime.Substring(2, 2));
                            int hour = leaveEndHour - leaveStartHour;
                            int min = leaveEndMin - leaveStartMin;

                            Notice notice = new Notice() 
                            {
                                EmployeeID = Setting.LEAVE_APPROVED_ID,
                                Content = $"{leave.Employee.EmployeeName} 於 {leave.LeaveDate:yyyy/MM/dd} {leave.LeaveStartTime} ~ {leave.LeaveEndTime} 請 {_leaveTypeService.GetDataByID(leave.LeaveType.ToString())?.LeaveName}，請去審核是否准許。",
                                CreateDate = DateTime.Now
                            };
                            
                            string noticeId = _noticeService.InsertData(notice);
                            if (noticeId.Equals("發生未知錯誤"))
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
                                NoticeId = int.Parse(noticeId)
                            };
                            if (leave.File != null)
                            {
                                string ext = Path.GetExtension(leave.File.FileName);
                                leave.File.FileName = $"{leave.Employee.EmployeeID}_{leave.LeaveDate:yyyyMMdd}_{leave.LeaveType}{ext}";
                                _fileService.UploadFile(leave.File.FileContent ?? Array.Empty<byte>(), leave.File.FileName);
                                leaveRecord.Prove = leave.File.FileName;
                            }
                            
                            string msg = _leaveRecordService.InsertData(leaveRecord);
                            if (string.IsNullOrWhiteSpace(msg))
                            {
                                result = 1;
                                transactionScope.Complete();
                            }
                            else 
                            { 
                                result = -99;//未知錯誤
                                return result;
                            }
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
                string msg = _leaveRecordService.DeleteData(leave);
                if (string.IsNullOrWhiteSpace(msg))
                {
                    result = 1;
                }
                else
                {
                    result = -99;//未知錯誤
                }
            }
            return result;
        }

        /** 同意請假*/
        public int AgreeLeaveRecord(LeaveRecord leave)
        {
            int result;

            if (string.IsNullOrEmpty(_leaveRecordService.Agree(leave)))
            {
                Notice notice = new Notice()
                {
                    EmployeeID = leave.EmployeeID,
                    Content = $"管理員已同意您於{leave.LeaveDate.ToTWDateString()}的請假。",
                    CreateDate = DateTime.Now
                };

                if (!"發生未知錯誤".Equals(_noticeService.InsertData(notice)))
                {
                    result = 1;
                }
                else 
                {
                    result = -99;
                }
            }
            else 
            {
                result = -99;
            }

            return result;
        }

        /** 駁回請假*/
        public int DisagreeLeaveRecord(LeaveRecord leave)
        {
            int result;
            if (string.IsNullOrEmpty(_leaveRecordService.Disagree(leave)))
            {
                Notice notice = new Notice()
                {
                    EmployeeID = leave.EmployeeID,
                    Content = $"管理員已駁回您於{leave.LeaveDate.ToTWDateString()}的請假。",
                    CreateDate = DateTime.Now
                };

                if (!"發生未知錯誤".Equals(_noticeService.InsertData(notice)))
                {
                    result = 1;
                }
                else
                {
                    result = -99;
                }
            }
            else
            {
                result = -99;
            }
            
            return result;
        }   
    }
}
