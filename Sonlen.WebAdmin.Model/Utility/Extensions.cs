using System.Globalization;


namespace Sonlen.WebAdmin.Model.Utility
{
    public static class Extensions
    {
        /// <summary>
        /// 請假記錄轉換為輸出表格用資料
        /// </summary>
        /// <param name="leaveRecords">請假紀錄物件列表</param>
        /// <param name="leaveTypes">請假類型物件列表</param>
        /// <returns>轉換後的請假紀錄物件列表</returns>
        public static List<LeaveRecordViewModel> Init(this List<LeaveRecordViewModel> leaveRecords, List<LeaveType> leaveTypes)
        {
            leaveRecords.ForEach(record => {
                record.LeaveDesc = leaveTypes.FirstOrDefault(type => type.Id == record.LeaveType)?.LeaveName ?? string.Empty;
                switch (record.Accept)
                {
                    case -1: record.AcceptDesc = "駁回"; break;
                    case 0: record.AcceptDesc = "未審"; break;
                    case 1: record.AcceptDesc = "同意"; break;
                    default: record.AcceptDesc = "？？"; break;
                }
                record.LeaveDateDesc = $"{record.LeaveDate.ToTWDateString()} {record.LeaveStartTime} ~ {record.LeaveEndTime}";
            });

            return leaveRecords;
        }

        /// <summary>
        /// 請假記錄轉換為輸出表格用資料
        /// </summary>
        /// <param name="workOvertimes">加班紀錄物件列表</param>
        /// <returns>轉換後的請假紀錄物件列表</returns>
        public static List<WorkOvertimeViewModel> Init(this List<WorkOvertimeViewModel> workOvertimes)
        {
            workOvertimes.ForEach(record => {
                record.AcceptDesc = record.Accept switch
                {
                    -1 => "駁回",
                    0 => "未審",
                    1 => "同意",
                    _ => "？？",
                };
            });

            return workOvertimes;
        }
    }
}
