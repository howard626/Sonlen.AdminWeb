using Dapper;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using Sonlen.WebAdmin.Model;
using Sonlen.WebAdmin.Model.Utility;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Transactions;

namespace Sonlen.AdminWebAPI.Service
{
    public class AttendanceService : IAttendanceService
    {
        private readonly string connectString;
        private readonly IEmployeeService _employeeService;
        private readonly ILeaveRecordService _leaveRecordService;

        public AttendanceService(IConfiguration configuration
            , IEmployeeService employeeService
            , ILeaveRecordService leaveRecordService)
        {
            connectString = configuration["ConnectionStrings:DefaultConnection"];
            _employeeService = employeeService;
            _leaveRecordService = leaveRecordService;
        }

        public IDbConnection Connection
        {
            get { return new SqlConnection(connectString); }
        }

        public string DeleteData(Attendance item)
        {
            string msg = string.Empty;
            using (var conn = Connection)
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@EmployeeID", item.EmployeeID, DbType.String);
                parameters.Add("@LogMon", item.LogMon, DbType.String);
                int result = conn.Execute("DelAttendance", parameters, commandType: CommandType.StoredProcedure);
                if (result <= 0)
                    msg = "發生未知錯誤";
            }

            return msg;
        }

        public IEnumerable<Attendance> GetAllDatas()
        {
            throw new NotImplementedException();
        }

        public Attendance? GetDataByID(string pks)
        {
            string[] pk = pks.Split(',');
            if (pk.Length == 2)
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@EmployeeID", pk[0], DbType.String);
                parameters.Add("@LogMon", pk[1], DbType.String);

                using (var conn = new SqlConnection(connectString))
                {
                    return conn.QueryFirstOrDefault<Attendance>("GetAttendance", parameters, commandType: CommandType.StoredProcedure);
                }
            }
            return null;
        }

        public string InsertData(Attendance item)
        {
            string msg = string.Empty;
            using (var conn = Connection)
            {
                DynamicParameters parameters = item.ToDynamicParameters();
                int result = conn.Execute("InsAttendance", parameters, commandType: CommandType.StoredProcedure);
                if (result <= 0)
                    msg = "發生未知錯誤";
            }

            return msg;
        }

        public string UpdateData(Attendance item)
        {
            string msg = string.Empty;
            using (var conn = Connection)
            {
                DynamicParameters parameters = item.ToDynamicParameters();
                int result = conn.Execute("UpdAttendance", parameters, commandType: CommandType.StoredProcedure);
                if (result <= 0)
                    msg = "發生未知錯誤";
            }

            return msg;
        }

        public UploadFile Print(AttendanceViewModel model)
        {
            List<AttendancePrintModel> items = new List<AttendancePrintModel>();
            List<Employee> employees =  new List<Employee>();
            if (string.IsNullOrEmpty(model.EmployeeID))
            {
                employees = _employeeService.GetAllDatas().ToList();
            }
            else 
            {
                Employee? employee = _employeeService.GetDataByID(model.EmployeeID);
                if (employee != null)
                    employees.Add(employee);
                else
                    return new UploadFile();
            }
            
            int total_day = new DateTime(model.Year + 1911, model.Month, 1).AddMonths(1).AddDays(-1).Day;
            bool ok = true;
            using (TransactionScope transactionScope = new TransactionScope())
            {
                foreach (Employee employee in employees)
                {
                    AttendancePrintModel item = new AttendancePrintModel()
                    {
                        EmployeeID = employee.EmployeeID,
                        ArrivalDate = employee.ArrivalDate,
                        EmployeeName = employee.EmployeeName,
                        Mon_Day = total_day,
                        Set_Day = total_day,
                    };
                    model.EmployeeID = employee.EmployeeID;
                    List<LeaveRecord> leaveRecords_Mon = _leaveRecordService.GetDatasByDate(model).ToList();
                    decimal thing_Hr = 0, sick_Hr_Pay = 0, sick_Hr_NoPay = 0;
                    foreach (LeaveRecord leaveRecord in leaveRecords_Mon)
                    {
                        switch (leaveRecord.LeaveType)
                        {
                            //普通傷病假(未住院)
                            case 25:
                            //普通傷病假(住院(30天以內))
                            case 27:
                                sick_Hr_Pay += leaveRecord.LeaveHour ?? 0;
                                break;
                            //普通傷病假(住院(30天以上))
                            case 26:
                                sick_Hr_NoPay += leaveRecord.LeaveHour ?? 0;
                                break;
                            //事假
                            case 30:
                                thing_Hr += leaveRecord.LeaveHour ?? 0;
                                break;
                        }

                    }
                    //小時轉天數
                    item.Thing_Day = Decimal.Round(thing_Hr / 8, 1);
                    item.Sick_Day_Pay = Decimal.Round(sick_Hr_Pay / 8, 1);
                    item.Sick_Day_NoPay = Decimal.Round(sick_Hr_NoPay / 8, 1);
                    item.Sick_Day = item.Sick_Day_Pay + item.Sick_Day_NoPay;
                    item.Sick_Day_Total = Decimal.Round((_leaveRecordService.GetSumHourByID(25, model.Year, model.EmployeeID) +
                                          _leaveRecordService.GetSumHourByID(26, model.Year, model.EmployeeID) +
                                          _leaveRecordService.GetSumHourByID(27, model.Year, model.EmployeeID)) / 8, 1);
                    item.Thing_Day_Total = Decimal.Round(_leaveRecordService.GetSumHourByID(30, model.Year, model.EmployeeID) / 8, 1);
                    items.Add(item);

                    Attendance attendance = new Attendance(item);
                    attendance.LogMon = model.Year.ToString().PadLeft(3, '0') + model.Month.ToString().PadLeft(2, '0');

                    if (GetDataByID($"{attendance.EmployeeID},{attendance.LogMon}") == null)
                    {
                        if (!string.IsNullOrEmpty(InsertData(attendance)))
                        {
                            ok = false;
                            break;
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(UpdateData(attendance)))
                        {
                            ok = false;
                            break;
                        }
                    }
                }
                if (ok)
                {
                    transactionScope.Complete();
                }
            }
            UploadFile file = new UploadFile();
            file.FileName = $"{model.Year}年{model.Month}月考勤.xlsx";
            file.FileContent = ExportExcel(items, file.FileName);
            return file;
        }

        /// <summary>
        /// 繪製 EXCEL
        /// </summary>
        /// <param name="items"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private byte[] ExportExcel(List<AttendancePrintModel> items, string fileName)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // 關閉新許可模式通知
            ExcelPackage ep = new ExcelPackage();
            ExcelWorksheet sheet = ep.Workbook.Worksheets.Add(fileName);

            /*設定欄位名稱*/
            Type type = typeof(AttendancePrintModel);
            PropertyInfo[] propertyInfos = type.GetProperties();
            int row = 1, col = 1;
            List<int> printCol = new List<int>();
            for (int i = 0; i < propertyInfos.Length; i++)
            {
                DisplayAttribute? displayAttribute = propertyInfos[i].GetCustomAttribute<DisplayAttribute>();
                if (displayAttribute != null && !string.IsNullOrWhiteSpace(displayAttribute.Description))
                {
                    sheet.Cells[row, col].Value = displayAttribute.Description;
                    sheet.Cells[row, col].Style.Border.BorderAround(ExcelBorderStyle.Thin);             //邊框
                    sheet.Cells[row, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;  //水平對齊
                    sheet.Cells[row, col].Style.VerticalAlignment = ExcelVerticalAlignment.Center;      //垂直對齊
                    col++;
                    printCol.Add(i);
                }
            }
            row++; col = 1;
            /*寫入內容*/
            for (int i = 0; i < items.Count; i++)
            {
                foreach(int print in printCol)
                {
                    Type t = Nullable.GetUnderlyingType(propertyInfos[print].PropertyType) ?? propertyInfos[print].PropertyType;
                    string typeName = t.Name;
                    if (typeName.Equals("DateTime"))
                    {
                        DateTime? value = (DateTime?)propertyInfos[print].GetValue(items[i]);
                        if (value != null)
                        {
                            sheet.Cells[row, col].Value = value.ToTWDateString("yyy/MM/dd");
                        }
                        else 
                        {
                            sheet.Cells[row, col].Value = "";
                        }
                    }
                    else 
                    {
                        sheet.Cells[row, col].Value = propertyInfos[print].GetValue(items[i]);
                    }
                    sheet.Cells[row, col].Style.Border.BorderAround(ExcelBorderStyle.Thin);             //邊框
                    sheet.Cells[row, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;  //水平對齊
                    sheet.Cells[row, col].Style.VerticalAlignment = ExcelVerticalAlignment.Center;      //垂直對齊
                    col++;
                }
                row++; col = 1;
            }

            sheet.Cells[sheet.Dimension.Address].AutoFitColumns(15); //自動調整列寬
            sheet.Cells[sheet.Dimension.Address].Style.Font.Size = 12; // 設定 EXCEL 字型大小
            return ep.GetAsByteArray();
        }
    }
}
