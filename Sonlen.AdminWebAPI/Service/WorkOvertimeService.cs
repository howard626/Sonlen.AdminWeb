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
        private readonly INoticeService _noticeService;
        private readonly IWorkOvertimeRecordService _workOvertimeRecordService;
        
        public WorkOvertimeService(INoticeService noticeService
            , IWorkOvertimeRecordService workOvertimeRecordService)
        {
            _noticeService = noticeService;
            _workOvertimeRecordService = workOvertimeRecordService;
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
                {
                    WorkOvertime? workOvertime = _workOvertimeRecordService.GetDataByID($"{overtime.EmployeeID},{overtime.OverDate}");
                    if (workOvertime == null)
                    {
                        Notice notice = new Notice()
                        {
                            EmployeeID = Setting.LEAVE_APPROVED_ID,
                            Content = $"{overtime.EmployeeName} 於 {overtime.OverDate:yyyy/MM/dd} 申請加班，請去審核是否核准。",
                            CreateDate = DateTime.Now
                        };
                        string noticeId = _noticeService.InsertData(notice);
                        if (noticeId.Equals("發生未知錯誤"))
                        {
                            result = -99;//未知錯誤
                            return result;
                        }

                        workOvertime = new WorkOvertime(overtime);
                        workOvertime.NoticeId = int.Parse(noticeId);
                        string msg = _workOvertimeRecordService.InsertData(workOvertime);
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
                string msg = _workOvertimeRecordService.DeleteData(overtime);
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

        /** 同意申請加班*/
        public int AgreeWorkOvertime(WorkOvertime overtime)
        {
            int result = 0;
            using (TransactionScope transactionScope = new TransactionScope())
            {
                if (string.IsNullOrEmpty(_workOvertimeRecordService.Agree(overtime)))
                {
                    Notice notice = new Notice()
                    {
                        EmployeeID = overtime.EmployeeID,
                        Content = $"管理員已同意您於{overtime.OverDate.ToTWDateString()}的申請加班。",
                        CreateDate = DateTime.Now
                    };

                    if (!"發生未知錯誤".Equals(_noticeService.InsertData(notice)))
                    {
                        result = 1;
                        transactionScope.Complete();
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
            }
            return result;
        }

        /** 駁回申請加班*/
        public int DisagreeWorkOvertime(WorkOvertime overtime)
        {
            int result = 0;
            using (TransactionScope transactionScope = new TransactionScope())
            {
                if (string.IsNullOrEmpty(_workOvertimeRecordService.Disagree(overtime)))
                {
                    Notice notice = new Notice()
                    {
                        EmployeeID = overtime.EmployeeID,
                        Content = $"管理員已駁回您於{overtime.OverDate.ToTWDateString()}的申請加班。",
                        CreateDate = DateTime.Now
                    };

                    if (!"發生未知錯誤".Equals(_noticeService.InsertData(notice)))
                    {
                        result = 1;
                        transactionScope.Complete();
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
            }
            return result;
        }
    }
}