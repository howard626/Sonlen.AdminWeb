using Dapper;
using Sonlen.WebAdmin.Model;
using Sonlen.WebAdmin.Model.Utility;
using System.Data;
using System.Data.SqlClient;

namespace Sonlen.AdminWebAPI.Service
{
    public class PunchService : IPunchService
    {

        private readonly IPunchRecordService _punchRecordService;
        public PunchService( IPunchRecordService punchRecordService)
        {
            _punchRecordService = punchRecordService;
        }

        public int PunchIn(Employee employee)
        {
            int result;

            Punch? punch = _punchRecordService.GetDataByID($"{employee.EmployeeID},{DateTime.Today}");
            if (punch == null)
            {
                DateTime now = DateTime.Now;
                punch = new Punch()
                {
                    EmployeeID = employee.EmployeeID,
                    PunchDate = DateTime.Today,
                    PunchInTime = $"{now.Hour.ToString().PadLeft(2, '0')}:{now.Minute.ToString().PadLeft(2, '0')}",
                    PunchOutTime = null,
                    WorkHour = null
                };

                string msg = _punchRecordService.InsertData(punch);
                if (string.IsNullOrWhiteSpace(msg))
                {
                    result = 1;
                }
                else
                {
                    result = -99;//未知錯誤
                    return result;
                }
            }
            else 
            {
                result = -1; // 已經有上班打卡紀錄
            }

            return result;
        }

        public int PunchOut(Employee employee)
        {
            int result;

            Punch? punch = _punchRecordService.GetDataByID($"{employee.EmployeeID},{DateTime.Today}");
            if (punch != null)
            {
                if (string.IsNullOrEmpty(punch.PunchOutTime))
                {
                    DateTime now = DateTime.Now;
                    int punchInHour = int.Parse(punch.PunchInTime.Split(':')[0]);
                    int hour = now.Hour - punchInHour;
                    int min = now.Minute - int.Parse(punch.PunchInTime.Split(':')[1]);
                    if (now.Hour >= 13 && punchInHour <= 13)
                    {
                        hour -= 1;
                    }
                    if (min < 0)
                    {
                        min += 60;
                        hour -= 1;
                    }
                    decimal workHour = hour + (decimal)min / 60;
                    punch.PunchOutTime = $"{now.Hour.ToString().PadLeft(2, '0')}:{now.Minute.ToString().PadLeft(2, '0')}";
                    punch.WorkHour = workHour;

                    string msg = _punchRecordService.UpdateData(punch);
                    if (string.IsNullOrWhiteSpace(msg))
                    {
                        result = 1;
                    }
                    else
                    {
                        result = -99;//未知錯誤
                        return result;
                    }
                }
                else 
                {
                    result = -2; // 已經有下班打卡紀錄
                }
            }
            else 
            {
                result = -1; // 沒有上班打卡紀錄
            }
            
            return result;
        }
    }
}
