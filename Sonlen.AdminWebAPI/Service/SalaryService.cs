using Dapper;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using Sonlen.WebAdmin.Model;
using Sonlen.WebAdmin.Model.Utility;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;

namespace Sonlen.AdminWebAPI.Service
{
    public class SalaryService : ISalaryService
    {
        private readonly string connectString;
        private readonly IAttendanceService _attendanceService;
        private readonly ICompanyService _companyService;
        private readonly IEmployeeService _employeeService;
        private readonly ILeaveRecordService _leaveRecordService;
        private readonly IWorkOvertimeRecordService _workOvertimeRecordService;

        public SalaryService(IAttendanceService attendanceService,
            IConfiguration configuration,
            ICompanyService companyService,
            IEmployeeService employeeService,
            ILeaveRecordService leaveRecordService,
            IWorkOvertimeRecordService workOvertimeRecordService)
        {
            connectString = configuration["ConnectionStrings:DefaultConnection"];
            _attendanceService = attendanceService;
            _companyService = companyService;
            _employeeService = employeeService;
            _leaveRecordService = leaveRecordService;
            _workOvertimeRecordService = workOvertimeRecordService;
        }

        public IDbConnection Connection
        {
            get { return new SqlConnection(connectString); }
        }

        public string DeleteData(Salary item)
        {
            string msg = string.Empty;
            using (var conn = Connection)
            {
                DynamicParameters parameters = item.ToDynamicParameters();
                int result = conn.Execute("DelSalary", parameters, commandType: CommandType.StoredProcedure);
                if (result <= 0)
                    msg = "發生未知錯誤";
            }

            return msg;
        }

        /// <summary>
        /// 以 PK 取得資料(以,區分各欄位(員工身分證,支付年月))
        /// </summary>
        /// <param name="ids">ids = 員工身分證,支付年月</param>
        /// <returns></returns>
        public Salary? GetDataByID(string ids)
        {
            string[] id = ids.Split(',');
            if (id.Length == 2)
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@EmployeeID", id[0], DbType.String);
                parameters.Add("@PayMon", id[1], DbType.String);

                using (var conn = new SqlConnection(connectString))
                {
                    return conn.QueryFirstOrDefault<Salary>("GetSalaryByID", parameters, commandType: CommandType.StoredProcedure);
                }
            }
            throw new Exception("參數錯誤");
        }

        public string InsertData(Salary item)
        {
            string msg = string.Empty;
            using (var conn = Connection)
            {
                DynamicParameters parameters = item.ToDynamicParameters();
                int result = conn.Execute("InsSalary", parameters, commandType: CommandType.StoredProcedure);
                if (result <= 0)
                    msg = "發生未知錯誤";
            }

            return msg;
        }

        public string UpdateData(Salary item)
        {
            string msg = string.Empty;
            using (var conn = Connection)
            {
                DynamicParameters parameters = item.ToDynamicParameters();
                int result = conn.Execute("UpdSalary", parameters, commandType: CommandType.StoredProcedure);
                if (result <= 0)
                    msg = "發生未知錯誤";
            }

            return msg;
        }

        public UploadFile Print(SalaryViewModel model)
        {
            UploadFile file = new UploadFile();
            Company? company = _companyService.GetAllDatas().FirstOrDefault();
            if (company == null)
            {
                file.FileName = "公司資料空白";
                return file;
            }
            Employee? employee = _employeeService.GetDataByID(model.EmployeeID);
            if (employee == null)
            {
                file.FileName = "員工資料空白";
                return file;
            }
            Attendance? attendance = _attendanceService.GetDataByID($"{model.EmployeeID},{model.PayMon}");
            if (attendance == null)
            {
                file.FileName = "請先製作考勤資料";
                return file;
            }
            Salary? salary = GetDataByID($"{model.EmployeeID},{model.PayMon}");
            if (salary == null)
            {
                salary = model;
                InsertData(salary);
            }
            else 
            {
                UpdateData(salary);
            }
            file.FileName = $"{model.Year}年{model.Month}月薪資單_{employee.EmployeeName}.xlsx";
            file.FileContent = ExportExcel(model, file.FileName, employee, company, attendance);
            return file;
        }

        /// <summary>
        /// 繪製 EXCEL
        /// </summary>
        /// <param name="item">薪資資料</param>
        /// <param name="fileName">檔案名稱</param>
        /// <param name="employee">員工資料</param>
        /// <param name="company">公司資料</param>
        /// <param name="attendance">考勤資料</param>
        /// <returns></returns>
        public byte[] ExportExcel(SalaryViewModel item, string fileName, Employee employee, Company company, Attendance attendance)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // 關閉新許可模式通知
            ExcelPackage ep = new ExcelPackage();
            ExcelWorksheet sheet = ep.Workbook.Worksheets.Add(fileName);
            
            int row = 1, col = 1;
            sheet.Cells[row, col, row, col + 3].Merge = true;
            sheet.Cells[row, col, row, col + 3].Value = $"{item.Year}年{item.Month}月份薪資單";
            sheet.Cells[row, col, row, col + 3].Style.Border.BorderAround(ExcelBorderStyle.Thin);             //邊框
            sheet.Cells[row, col, row, col + 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;  //水平對齊
            sheet.Cells[row, col, row, col + 3].Style.VerticalAlignment = ExcelVerticalAlignment.Center;      //垂直對齊
            sheet.Cells[row, col, row, col + 3].Style.Font.Size = 18;                                         //字型大小
            row++;

            #region 公司資料

            sheet.Cells[row, col, row, col + 3].Merge = true;
            sheet.Cells[row, col, row, col + 3].Value = $"公司資料";
            sheet.Cells[row, col, row, col + 3].Style.Fill.PatternType = ExcelFillStyle.Solid;                //設定背景顏色的前置設定
            sheet.Cells[row, col, row, col + 3].Style.Fill.BackgroundColor.SetColor(Color.LightGray);         //背景顏色
            sheet.Cells[row, col, row, col + 3].Style.Border.BorderAround(ExcelBorderStyle.Thin);             //邊框
            sheet.Cells[row, col, row, col + 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;  //水平對齊
            sheet.Cells[row, col, row, col + 3].Style.VerticalAlignment = ExcelVerticalAlignment.Center;      //垂直對齊
            row++;

            sheet.Cells[row, col].Value = "公司名稱";
            sheet.Cells[row, col].Style.Border.BorderAround(ExcelBorderStyle.Thin);             //邊框
            sheet.Cells[row, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;    //水平對齊
            sheet.Cells[row, col].Style.VerticalAlignment = ExcelVerticalAlignment.Center;      //垂直對齊
            col++;

            sheet.Cells[row, col, row, col + 2].Merge = true;
            sheet.Cells[row, col, row, col + 2].Value = company.Name;
            sheet.Cells[row, col, row, col + 2].Style.Border.BorderAround(ExcelBorderStyle.Thin);             //邊框
            sheet.Cells[row, col, row, col + 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;    //水平對齊
            sheet.Cells[row, col, row, col + 2].Style.VerticalAlignment = ExcelVerticalAlignment.Center;      //垂直對齊
            row++;col = 1;

            sheet.Cells[row, col].Value = "聯絡地址";
            sheet.Cells[row, col].Style.Border.BorderAround(ExcelBorderStyle.Thin);             //邊框
            sheet.Cells[row, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;    //水平對齊
            sheet.Cells[row, col].Style.VerticalAlignment = ExcelVerticalAlignment.Center;      //垂直對齊
            col++;

            sheet.Cells[row, col, row, col + 2].Merge = true;
            sheet.Cells[row, col, row, col + 2].Value = company.Address;
            sheet.Cells[row, col, row, col + 2].Style.Border.BorderAround(ExcelBorderStyle.Thin);             //邊框
            sheet.Cells[row, col, row, col + 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;    //水平對齊
            sheet.Cells[row, col, row, col + 2].Style.VerticalAlignment = ExcelVerticalAlignment.Center;      //垂直對齊
            row++; col = 1;

            sheet.Cells[row, col].Value = "業務聯絡人";
            sheet.Cells[row, col].Style.Border.BorderAround(ExcelBorderStyle.Thin);             //邊框
            sheet.Cells[row, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;    //水平對齊
            sheet.Cells[row, col].Style.VerticalAlignment = ExcelVerticalAlignment.Center;      //垂直對齊
            col++;

            sheet.Cells[row, col, row, col + 2].Merge = true;
            sheet.Cells[row, col, row, col + 2].Value = company.Contact;
            sheet.Cells[row, col, row, col + 2].Style.Border.BorderAround(ExcelBorderStyle.Thin);             //邊框
            sheet.Cells[row, col, row, col + 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;    //水平對齊
            sheet.Cells[row, col, row, col + 2].Style.VerticalAlignment = ExcelVerticalAlignment.Center;      //垂直對齊
            row++; col = 1;

            sheet.Cells[row, col].Value = "連絡電話";
            sheet.Cells[row, col].Style.Border.BorderAround(ExcelBorderStyle.Thin);             //邊框
            sheet.Cells[row, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;    //水平對齊
            sheet.Cells[row, col].Style.VerticalAlignment = ExcelVerticalAlignment.Center;      //垂直對齊
            col++;

            sheet.Cells[row, col].Value = company.TellPhone;
            sheet.Cells[row, col].Style.Border.BorderAround(ExcelBorderStyle.Thin);             //邊框
            sheet.Cells[row, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;    //水平對齊
            sheet.Cells[row, col].Style.VerticalAlignment = ExcelVerticalAlignment.Center;      //垂直對齊
            col++;

            sheet.Cells[row, col].Value = "統編";
            sheet.Cells[row, col].Style.Border.BorderAround(ExcelBorderStyle.Thin);             //邊框
            sheet.Cells[row, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;    //水平對齊
            sheet.Cells[row, col].Style.VerticalAlignment = ExcelVerticalAlignment.Center;      //垂直對齊
            col++;

            sheet.Cells[row, col].Value = company.ID;
            sheet.Cells[row, col].Style.Border.BorderAround(ExcelBorderStyle.Thin);             //邊框
            sheet.Cells[row, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;    //水平對齊
            sheet.Cells[row, col].Style.VerticalAlignment = ExcelVerticalAlignment.Center;      //垂直對齊
            row++; col = 1;

            #endregion

            #region 本次工作內容 員工資料
            sheet.Cells[row, col, row, col + 3].Merge = true;
            sheet.Cells[row, col, row, col + 3].Value = $"本次工作內容";
            sheet.Cells[row, col, row, col + 3].Style.Fill.PatternType = ExcelFillStyle.Solid;                //設定背景顏色的前置設定
            sheet.Cells[row, col, row, col + 3].Style.Fill.BackgroundColor.SetColor(Color.LightGray);         //背景顏色
            sheet.Cells[row, col, row, col + 3].Style.Border.BorderAround(ExcelBorderStyle.Thin);             //邊框
            sheet.Cells[row, col, row, col + 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;  //水平對齊
            sheet.Cells[row, col, row, col + 3].Style.VerticalAlignment = ExcelVerticalAlignment.Center;      //垂直對齊
            row++;

            sheet.Cells[row, col].Value = "專案名稱";
            sheet.Cells[row, col].Style.Border.BorderAround(ExcelBorderStyle.Thin);             //邊框
            sheet.Cells[row, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;    //水平對齊
            sheet.Cells[row, col].Style.VerticalAlignment = ExcelVerticalAlignment.Center;      //垂直對齊
            col++;

            sheet.Cells[row, col, row, col + 2].Merge = true;
            sheet.Cells[row, col, row, col + 2].Value = item.ProjectName;
            sheet.Cells[row, col, row, col + 2].Style.Border.BorderAround(ExcelBorderStyle.Thin);             //邊框
            sheet.Cells[row, col, row, col + 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;    //水平對齊
            sheet.Cells[row, col, row, col + 2].Style.VerticalAlignment = ExcelVerticalAlignment.Center;      //垂直對齊
            row++; col = 1;

            sheet.Cells[row, col].Value = "姓名";
            sheet.Cells[row, col].Style.Border.BorderAround(ExcelBorderStyle.Thin);             //邊框
            sheet.Cells[row, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;    //水平對齊
            sheet.Cells[row, col].Style.VerticalAlignment = ExcelVerticalAlignment.Center;      //垂直對齊
            col++;

            sheet.Cells[row, col].Value = employee.EmployeeName;
            sheet.Cells[row, col].Style.Border.BorderAround(ExcelBorderStyle.Thin);             //邊框
            sheet.Cells[row, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;    //水平對齊
            sheet.Cells[row, col].Style.VerticalAlignment = ExcelVerticalAlignment.Center;      //垂直對齊
            col++;

            sheet.Cells[row, col, row, col + 1].Merge = true;
            sheet.Cells[row, col, row, col + 1].Value = item.Account;
            sheet.Cells[row, col, row, col + 1].Style.Border.BorderAround(ExcelBorderStyle.Thin);             //邊框
            sheet.Cells[row, col, row, col + 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;   //水平對齊
            sheet.Cells[row, col, row, col + 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;      //垂直對齊
            row++; col = 1;

            sheet.Cells[row, col].Value = "薪資";
            sheet.Cells[row, col].Style.Border.BorderAround(ExcelBorderStyle.Thin);             //邊框
            sheet.Cells[row, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;    //水平對齊
            sheet.Cells[row, col].Style.VerticalAlignment = ExcelVerticalAlignment.Center;      //垂直對齊
            col++;

            sheet.Cells[row, col].Value = item.Pay;
            sheet.Cells[row, col].Style.Border.BorderAround(ExcelBorderStyle.Thin);             //邊框
            sheet.Cells[row, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;   //水平對齊
            sheet.Cells[row, col].Style.VerticalAlignment = ExcelVerticalAlignment.Center;      //垂直對齊
            col++;

            sheet.Cells[row, col].Value = "到職日";
            sheet.Cells[row, col].Style.Border.BorderAround(ExcelBorderStyle.Thin);             //邊框
            sheet.Cells[row, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;    //水平對齊
            sheet.Cells[row, col].Style.VerticalAlignment = ExcelVerticalAlignment.Center;      //垂直對齊
            col++;

            sheet.Cells[row, col].Value = employee.ArrivalDate.ToTWDateString("yyy/MM/dd");
            sheet.Cells[row, col].Style.Border.BorderAround(ExcelBorderStyle.Thin);             //邊框
            sheet.Cells[row, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;   //水平對齊
            sheet.Cells[row, col].Style.VerticalAlignment = ExcelVerticalAlignment.Center;      //垂直對齊
            row++; col = 1;

            #endregion

            #region 出缺勤

            sheet.Cells[row, col, row, col + 3].Merge = true;
            sheet.Cells[row, col, row, col + 3].Value = $"出缺勤";
            sheet.Cells[row, col, row, col + 3].Style.Fill.PatternType = ExcelFillStyle.Solid;                //設定背景顏色的前置設定
            sheet.Cells[row, col, row, col + 3].Style.Fill.BackgroundColor.SetColor(Color.LightGray);         //背景顏色
            sheet.Cells[row, col, row, col + 3].Style.Border.BorderAround(ExcelBorderStyle.Thin);             //邊框
            sheet.Cells[row, col, row, col + 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;  //水平對齊
            sheet.Cells[row, col, row, col + 3].Style.VerticalAlignment = ExcelVerticalAlignment.Center;      //垂直對齊
            row++;

            sheet.Cells[row, col].Value = "項次";
            sheet.Cells[row, col].Style.Border.BorderAround(ExcelBorderStyle.Thin);             //邊框
            sheet.Cells[row, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;    //水平對齊
            sheet.Cells[row, col].Style.VerticalAlignment = ExcelVerticalAlignment.Center;      //垂直對齊
            col++;

            sheet.Cells[row, col].Value = "數量";
            sheet.Cells[row, col].Style.Border.BorderAround(ExcelBorderStyle.Thin);             //邊框
            sheet.Cells[row, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;    //水平對齊
            sheet.Cells[row, col].Style.VerticalAlignment = ExcelVerticalAlignment.Center;      //垂直對齊
            col++;

            sheet.Cells[row, col].Value = "單位";
            sheet.Cells[row, col].Style.Border.BorderAround(ExcelBorderStyle.Thin);             //邊框
            sheet.Cells[row, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;    //水平對齊
            sheet.Cells[row, col].Style.VerticalAlignment = ExcelVerticalAlignment.Center;      //垂直對齊
            col++;

            sheet.Cells[row, col].Value = "說明";
            sheet.Cells[row, col].Style.Border.BorderAround(ExcelBorderStyle.Thin);             //邊框
            sheet.Cells[row, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;    //水平對齊
            sheet.Cells[row, col].Style.VerticalAlignment = ExcelVerticalAlignment.Center;      //垂直對齊
            row++; col = 1;

            sheet.Cells[row, col].Value = "當月天數";
            sheet.Cells[row, col].Style.Border.BorderAround(ExcelBorderStyle.Thin);             //邊框
            sheet.Cells[row, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;    //水平對齊
            sheet.Cells[row, col].Style.VerticalAlignment = ExcelVerticalAlignment.Center;      //垂直對齊
            col++;

            sheet.Cells[row, col].Value = attendance.Mon_Day;
            sheet.Cells[row, col].Style.Border.BorderAround(ExcelBorderStyle.Thin);             //邊框
            sheet.Cells[row, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;    //水平對齊
            sheet.Cells[row, col].Style.VerticalAlignment = ExcelVerticalAlignment.Center;      //垂直對齊
            col++;

            sheet.Cells[row, col].Value = "日";
            sheet.Cells[row, col].Style.Border.BorderAround(ExcelBorderStyle.Thin);             //邊框
            sheet.Cells[row, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;    //水平對齊
            sheet.Cells[row, col].Style.VerticalAlignment = ExcelVerticalAlignment.Center;      //垂直對齊
            col++;

            sheet.Cells[row, col].Style.Border.BorderAround(ExcelBorderStyle.Thin);             //邊框
            row++; col = 1;

            sheet.Cells[row, col].Value = "本月加班時數*1.33";
            sheet.Cells[row, col].Style.Border.BorderAround(ExcelBorderStyle.Thin);             //邊框
            sheet.Cells[row, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;    //水平對齊
            sheet.Cells[row, col].Style.VerticalAlignment = ExcelVerticalAlignment.Center;      //垂直對齊
            col++;

            sheet.Cells[row, col].Value = _workOvertimeRecordService.GetSum133HourByDate(employee.EmployeeID, item.PayMon);
            sheet.Cells[row, col].Style.Border.BorderAround(ExcelBorderStyle.Thin);             //邊框
            sheet.Cells[row, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;    //水平對齊
            sheet.Cells[row, col].Style.VerticalAlignment = ExcelVerticalAlignment.Center;      //垂直對齊
            col++;

            sheet.Cells[row, col].Value = "小時";
            sheet.Cells[row, col].Style.Border.BorderAround(ExcelBorderStyle.Thin);             //邊框
            sheet.Cells[row, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;    //水平對齊
            sheet.Cells[row, col].Style.VerticalAlignment = ExcelVerticalAlignment.Center;      //垂直對齊
            col++;

            sheet.Cells[row, col].Style.Border.BorderAround(ExcelBorderStyle.Thin);             //邊框
            row++; col = 1;

            sheet.Cells[row, col].Value = "本月加班時數*1.66";
            sheet.Cells[row, col].Style.Border.BorderAround(ExcelBorderStyle.Thin);             //邊框
            sheet.Cells[row, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;    //水平對齊
            sheet.Cells[row, col].Style.VerticalAlignment = ExcelVerticalAlignment.Center;      //垂直對齊
            col++;

            sheet.Cells[row, col].Value = _workOvertimeRecordService.GetSum166HourByDate(employee.EmployeeID, item.PayMon);
            sheet.Cells[row, col].Style.Border.BorderAround(ExcelBorderStyle.Thin);             //邊框
            sheet.Cells[row, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;   //水平對齊
            sheet.Cells[row, col].Style.VerticalAlignment = ExcelVerticalAlignment.Center;      //垂直對齊
            col++;

            sheet.Cells[row, col].Value = "小時";
            sheet.Cells[row, col].Style.Border.BorderAround(ExcelBorderStyle.Thin);             //邊框
            sheet.Cells[row, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;    //水平對齊
            sheet.Cells[row, col].Style.VerticalAlignment = ExcelVerticalAlignment.Center;      //垂直對齊
            col++;

            sheet.Cells[row, col].Style.Border.BorderAround(ExcelBorderStyle.Thin);             //邊框
            row++; col = 1;

            sheet.Cells[row, col].Value = "本月加班時數*2.00";
            sheet.Cells[row, col].Style.Border.BorderAround(ExcelBorderStyle.Thin);             //邊框
            sheet.Cells[row, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;    //水平對齊
            sheet.Cells[row, col].Style.VerticalAlignment = ExcelVerticalAlignment.Center;      //垂直對齊
            col++;

            sheet.Cells[row, col].Value = _workOvertimeRecordService.GetSum200HourByDate(employee.EmployeeID, item.PayMon);
            sheet.Cells[row, col].Style.Border.BorderAround(ExcelBorderStyle.Thin);             //邊框
            sheet.Cells[row, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;   //水平對齊
            sheet.Cells[row, col].Style.VerticalAlignment = ExcelVerticalAlignment.Center;      //垂直對齊
            col++;

            sheet.Cells[row, col].Value = "小時";
            sheet.Cells[row, col].Style.Border.BorderAround(ExcelBorderStyle.Thin);             //邊框
            sheet.Cells[row, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;    //水平對齊
            sheet.Cells[row, col].Style.VerticalAlignment = ExcelVerticalAlignment.Center;      //垂直對齊
            col++;

            sheet.Cells[row, col].Style.Border.BorderAround(ExcelBorderStyle.Thin);             //邊框
            row++; col = 1;

            sheet.Cells[row, col].Value = "本月請假時數";
            sheet.Cells[row, col].Style.Border.BorderAround(ExcelBorderStyle.Thin);             //邊框
            sheet.Cells[row, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;    //水平對齊
            sheet.Cells[row, col].Style.VerticalAlignment = ExcelVerticalAlignment.Center;      //垂直對齊
            col++;

            sheet.Cells[row, col].Value = attendance.Thing_Hour + attendance.Sick_Hour_NoPay;
            sheet.Cells[row, col].Style.Border.BorderAround(ExcelBorderStyle.Thin);             //邊框
            sheet.Cells[row, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;   //水平對齊
            sheet.Cells[row, col].Style.VerticalAlignment = ExcelVerticalAlignment.Center;      //垂直對齊
            col++;

            sheet.Cells[row, col].Value = "小時";
            sheet.Cells[row, col].Style.Border.BorderAround(ExcelBorderStyle.Thin);             //邊框
            sheet.Cells[row, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;    //水平對齊
            sheet.Cells[row, col].Style.VerticalAlignment = ExcelVerticalAlignment.Center;      //垂直對齊
            col++;

            sheet.Cells[row, col].Style.Border.BorderAround(ExcelBorderStyle.Thin);             //邊框
            row++; col = 1;

            sheet.Cells[row, col].Value = "本月請假半薪時數";
            sheet.Cells[row, col].Style.Border.BorderAround(ExcelBorderStyle.Thin);             //邊框
            sheet.Cells[row, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;    //水平對齊
            sheet.Cells[row, col].Style.VerticalAlignment = ExcelVerticalAlignment.Center;      //垂直對齊
            col++;

            sheet.Cells[row, col].Value = attendance.Sick_Hour_Pay;
            sheet.Cells[row, col].Style.Border.BorderAround(ExcelBorderStyle.Thin);             //邊框
            sheet.Cells[row, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;   //水平對齊
            sheet.Cells[row, col].Style.VerticalAlignment = ExcelVerticalAlignment.Center;      //垂直對齊
            col++;

            sheet.Cells[row, col].Value = "小時";
            sheet.Cells[row, col].Style.Border.BorderAround(ExcelBorderStyle.Thin);             //邊框
            sheet.Cells[row, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;    //水平對齊
            sheet.Cells[row, col].Style.VerticalAlignment = ExcelVerticalAlignment.Center;      //垂直對齊
            col++;

            sheet.Cells[row, col].Style.Border.BorderAround(ExcelBorderStyle.Thin);             //邊框
            row++; col = 1;

            DateTime arrivalDate = employee.ArrivalDate ?? DateTime.Now;
            int SLHour_Total = CountSpecialLeaveHour(arrivalDate, new DateTime(item.Year + 1911, item.Month, 1).AddDays(-1)); // 當年度可使用特休總時數
            decimal SLHour_Use;// 當年度已使用特休時數(不含當月)
            if (item.Month > arrivalDate.Month)
            {
                SLHour_Use = _leaveRecordService.GetSumSLHourByDate(new DateTime(item.Year + 1911, arrivalDate.Month, arrivalDate.Day),
                    new DateTime(item.Year + 1911, item.Month, 1).AddDays(-1), employee.EmployeeID);
            }
            else
            {
                SLHour_Use = _leaveRecordService.GetSumSLHourByDate(new DateTime(item.Year + 1910, arrivalDate.Month, arrivalDate.Day),
                    new DateTime(item.Year + 1911, item.Month, 1).AddDays(-1), employee.EmployeeID); 
            }
            decimal SLHour_Now = _leaveRecordService.GetSumSLHourByDate(new DateTime(item.Year + 1911, item.Month, 1),
                new DateTime(item.Year + 1911, item.Month, 1).AddMonths(1).AddDays(-1), employee.EmployeeID); //當月已使用特休時數

            sheet.Cells[row, col].Value = "上月剩餘特休時數";
            sheet.Cells[row, col].Style.Border.BorderAround(ExcelBorderStyle.Thin);             //邊框
            sheet.Cells[row, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;    //水平對齊
            sheet.Cells[row, col].Style.VerticalAlignment = ExcelVerticalAlignment.Center;      //垂直對齊
            col++;

            sheet.Cells[row, col].Value = SLHour_Total - SLHour_Use;
            sheet.Cells[row, col].Style.Border.BorderAround(ExcelBorderStyle.Thin);             //邊框
            sheet.Cells[row, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;   //水平對齊
            sheet.Cells[row, col].Style.VerticalAlignment = ExcelVerticalAlignment.Center;      //垂直對齊
            col++;

            sheet.Cells[row, col].Value = "小時";
            sheet.Cells[row, col].Style.Border.BorderAround(ExcelBorderStyle.Thin);             //邊框
            sheet.Cells[row, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;    //水平對齊
            sheet.Cells[row, col].Style.VerticalAlignment = ExcelVerticalAlignment.Center;      //垂直對齊
            col++;

            sheet.Cells[row, col].Style.Border.BorderAround(ExcelBorderStyle.Thin);             //邊框
            row++; col = 1;

            sheet.Cells[row, col].Value = "當月剩餘特休時數";
            sheet.Cells[row, col].Style.Border.BorderAround(ExcelBorderStyle.Thin);             //邊框
            sheet.Cells[row, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;    //水平對齊
            sheet.Cells[row, col].Style.VerticalAlignment = ExcelVerticalAlignment.Center;      //垂直對齊
            col++;

            sheet.Cells[row, col].Value = SLHour_Total - SLHour_Use - SLHour_Now;
            sheet.Cells[row, col].Style.Border.BorderAround(ExcelBorderStyle.Thin);             //邊框
            sheet.Cells[row, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;   //水平對齊
            sheet.Cells[row, col].Style.VerticalAlignment = ExcelVerticalAlignment.Center;      //垂直對齊
            col++;

            sheet.Cells[row, col].Value = "小時";
            sheet.Cells[row, col].Style.Border.BorderAround(ExcelBorderStyle.Thin);             //邊框
            sheet.Cells[row, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;    //水平對齊
            sheet.Cells[row, col].Style.VerticalAlignment = ExcelVerticalAlignment.Center;      //垂直對齊
            col++;

            sheet.Cells[row, col].Style.Border.BorderAround(ExcelBorderStyle.Thin);             //邊框
            row++; col = 1;

            sheet.Cells[row, col].Value = "每日工作時數";
            sheet.Cells[row, col].Style.Border.BorderAround(ExcelBorderStyle.Thin);             //邊框
            sheet.Cells[row, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;    //水平對齊
            sheet.Cells[row, col].Style.VerticalAlignment = ExcelVerticalAlignment.Center;      //垂直對齊
            col++;

            sheet.Cells[row, col].Value = 8;
            sheet.Cells[row, col].Style.Border.BorderAround(ExcelBorderStyle.Thin);             //邊框
            sheet.Cells[row, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;   //水平對齊
            sheet.Cells[row, col].Style.VerticalAlignment = ExcelVerticalAlignment.Center;      //垂直對齊
            col++;

            sheet.Cells[row, col].Value = "小時";
            sheet.Cells[row, col].Style.Border.BorderAround(ExcelBorderStyle.Thin);             //邊框
            sheet.Cells[row, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;    //水平對齊
            sheet.Cells[row, col].Style.VerticalAlignment = ExcelVerticalAlignment.Center;      //垂直對齊
            col++;

            sheet.Cells[row, col].Style.Border.BorderAround(ExcelBorderStyle.Thin);             //邊框
            row++; col = 1;

            #endregion

            #region 薪資細目

            sheet.Cells[row, col, row, col + 3].Merge = true;
            sheet.Cells[row, col, row, col + 3].Value = $"薪資細目";
            sheet.Cells[row, col, row, col + 3].Style.Fill.PatternType = ExcelFillStyle.Solid;                //設定背景顏色的前置設定
            sheet.Cells[row, col, row, col + 3].Style.Fill.BackgroundColor.SetColor(Color.LightGray);         //背景顏色
            sheet.Cells[row, col, row, col + 3].Style.Border.BorderAround(ExcelBorderStyle.Thin);             //邊框
            sheet.Cells[row, col, row, col + 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;  //水平對齊
            sheet.Cells[row, col, row, col + 3].Style.VerticalAlignment = ExcelVerticalAlignment.Center;      //垂直對齊
            row++;

            sheet.Cells[row, col].Value = "本月應發工作費";
            sheet.Cells[row, col].Style.Border.BorderAround(ExcelBorderStyle.Thin);             //邊框
            sheet.Cells[row, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;    //水平對齊
            sheet.Cells[row, col].Style.VerticalAlignment = ExcelVerticalAlignment.Center;      //垂直對齊
            col++;

            sheet.Cells[row, col, row, col + 1].Merge = true;
            sheet.Cells[row, col, row, col + 1].Style.Border.BorderAround(ExcelBorderStyle.Thin);             //邊框
            sheet.Cells[row, col, row, col + 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;   //水平對齊
            sheet.Cells[row, col, row, col + 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;      //垂直對齊
            col+= 2;

            sheet.Cells[row, col].Formula = $"=B10";
            sheet.Cells[row, col].Style.Border.BorderAround(ExcelBorderStyle.Thin);             //邊框
            sheet.Cells[row, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;   //水平對齊
            sheet.Cells[row, col].Style.VerticalAlignment = ExcelVerticalAlignment.Center;      //垂直對齊
            row++; col = 1;

            sheet.Cells[row, col].Value = "本月加班費";
            sheet.Cells[row, col].Style.Border.BorderAround(ExcelBorderStyle.Thin);             //邊框
            sheet.Cells[row, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;    //水平對齊
            sheet.Cells[row, col].Style.VerticalAlignment = ExcelVerticalAlignment.Center;      //垂直對齊
            col++;

            sheet.Cells[row, col, row, col + 1].Merge = true;
            sheet.Cells[row, col, row, col + 1].Style.Border.BorderAround(ExcelBorderStyle.Thin);             //邊框
            sheet.Cells[row, col, row, col + 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;   //水平對齊
            sheet.Cells[row, col, row, col + 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;      //垂直對齊
            col += 2;

            sheet.Cells[row, col].Formula = $"=(B10/B13/B21) * (B14*1.33 + B15* 1.66 + B16 * 2)";
            sheet.Cells[row, col].Style.Numberformat.Format = "0";                              //去除小數點
            sheet.Cells[row, col].Style.Border.BorderAround(ExcelBorderStyle.Thin);             //邊框
            sheet.Cells[row, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;   //水平對齊
            sheet.Cells[row, col].Style.VerticalAlignment = ExcelVerticalAlignment.Center;      //垂直對齊
            row++; col = 1;

            sheet.Cells[row, col].Value = "年終獎金";
            sheet.Cells[row, col].Style.Border.BorderAround(ExcelBorderStyle.Thin);             //邊框
            sheet.Cells[row, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;    //水平對齊
            sheet.Cells[row, col].Style.VerticalAlignment = ExcelVerticalAlignment.Center;      //垂直對齊
            col++;

            sheet.Cells[row, col, row, col + 1].Merge = true;
            sheet.Cells[row, col, row, col + 1].Style.Border.BorderAround(ExcelBorderStyle.Thin);             //邊框
            sheet.Cells[row, col, row, col + 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;   //水平對齊
            sheet.Cells[row, col, row, col + 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;      //垂直對齊
            col += 2;

            if ((item.Year + 1910) == arrivalDate.Year && item.Month == 1)
            {
                int 當年天數 = 0;
                if (arrivalDate.Month < 7) //如果今年 7 月前上工
                {
                    DateTime beginDate = new DateTime(arrivalDate.Year, item.Month, 1);
                    if (DateTime.IsLeapYear(arrivalDate.Year))
                        當年天數 = 366;
                    else
                        當年天數 = 365;
                    TimeSpan ts = arrivalDate - beginDate;
                    sheet.Cells[row, col].Formula = $"={當年天數 - ts.TotalDays}/{當年天數} *B10";
                }
                else //如果今年 7 月後上工
                {
                    DateTime endDate = new DateTime(arrivalDate.Year + 1, item.Month, 1).AddDays(-1);
                    當年天數 = 31 + 31 + 30 + 31 + 30 + 31;
                    TimeSpan ts = endDate - arrivalDate;
                    sheet.Cells[row, col].Formula = $"={ts.TotalDays}/{當年天數} *B10";
                }
            }
            else 
            {
                sheet.Cells[row, col].Formula = $"=(B10) * {item.Year_End}";
            }
            sheet.Cells[row, col].Style.Numberformat.Format = "0";                              //去除小數點
            sheet.Cells[row, col].Style.Border.BorderAround(ExcelBorderStyle.Thin);             //邊框
            sheet.Cells[row, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;   //水平對齊
            sheet.Cells[row, col].Style.VerticalAlignment = ExcelVerticalAlignment.Center;      //垂直對齊
            row++; col = 1;

            sheet.Cells[row, col].Value = "駐點獎金";
            sheet.Cells[row, col].Style.Border.BorderAround(ExcelBorderStyle.Thin);             //邊框
            sheet.Cells[row, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;    //水平對齊
            sheet.Cells[row, col].Style.VerticalAlignment = ExcelVerticalAlignment.Center;      //垂直對齊
            col++;

            sheet.Cells[row, col, row, col + 1].Merge = true;
            sheet.Cells[row, col, row, col + 1].Style.Border.BorderAround(ExcelBorderStyle.Thin);             //邊框
            sheet.Cells[row, col, row, col + 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;   //水平對齊
            sheet.Cells[row, col, row, col + 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;      //垂直對齊
            col += 2;

            sheet.Cells[row, col].Value = item.Stay;
            sheet.Cells[row, col].Style.Border.BorderAround(ExcelBorderStyle.Thin);             //邊框
            sheet.Cells[row, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;   //水平對齊
            sheet.Cells[row, col].Style.VerticalAlignment = ExcelVerticalAlignment.Center;      //垂直對齊
            row++; col = 1;

            sheet.Cells[row, col].Value = "專案獎金";
            sheet.Cells[row, col].Style.Border.BorderAround(ExcelBorderStyle.Thin);             //邊框
            sheet.Cells[row, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;    //水平對齊
            sheet.Cells[row, col].Style.VerticalAlignment = ExcelVerticalAlignment.Center;      //垂直對齊
            col++;

            sheet.Cells[row, col, row, col + 1].Merge = true;
            sheet.Cells[row, col, row, col + 1].Style.Border.BorderAround(ExcelBorderStyle.Thin);             //邊框
            sheet.Cells[row, col, row, col + 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;   //水平對齊
            sheet.Cells[row, col, row, col + 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;      //垂直對齊
            col += 2;

            sheet.Cells[row, col].Value = item.Project;
            sheet.Cells[row, col].Style.Border.BorderAround(ExcelBorderStyle.Thin);             //邊框
            sheet.Cells[row, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;   //水平對齊
            sheet.Cells[row, col].Style.VerticalAlignment = ExcelVerticalAlignment.Center;      //垂直對齊
            row++; col = 1;

            sheet.Cells[row, col].Value = "本月請假扣費";
            sheet.Cells[row, col].Style.Border.BorderAround(ExcelBorderStyle.Thin);             //邊框
            sheet.Cells[row, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;    //水平對齊
            sheet.Cells[row, col].Style.VerticalAlignment = ExcelVerticalAlignment.Center;      //垂直對齊
            col++;

            sheet.Cells[row, col, row, col + 1].Merge = true;
            sheet.Cells[row, col, row, col + 1].Style.Border.BorderAround(ExcelBorderStyle.Thin);             //邊框
            sheet.Cells[row, col, row, col + 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;   //水平對齊
            sheet.Cells[row, col, row, col + 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;      //垂直對齊
            col += 2;

            sheet.Cells[row, col].Formula = $"=(B10/B13/B21) * (B18 * 0.5 + B17)";
            sheet.Cells[row, col].Style.Numberformat.Format = "0";                              //去除小數點
            sheet.Cells[row, col].Style.Border.BorderAround(ExcelBorderStyle.Thin);             //邊框
            sheet.Cells[row, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;   //水平對齊
            sheet.Cells[row, col].Style.VerticalAlignment = ExcelVerticalAlignment.Center;      //垂直對齊
            row++; col = 1;

            sheet.Cells[row, col].Value = "代扣勞保費";
            sheet.Cells[row, col].Style.Border.BorderAround(ExcelBorderStyle.Thin);             //邊框
            sheet.Cells[row, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;    //水平對齊
            sheet.Cells[row, col].Style.VerticalAlignment = ExcelVerticalAlignment.Center;      //垂直對齊
            col++;

            sheet.Cells[row, col, row, col + 1].Merge = true;
            sheet.Cells[row, col, row, col + 1].Style.Border.BorderAround(ExcelBorderStyle.Thin);             //邊框
            sheet.Cells[row, col, row, col + 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;   //水平對齊
            sheet.Cells[row, col, row, col + 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;      //垂直對齊
            col += 2;

            sheet.Cells[row, col].Value = item.Labor;
            sheet.Cells[row, col].Style.Border.BorderAround(ExcelBorderStyle.Thin);             //邊框
            sheet.Cells[row, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;   //水平對齊
            sheet.Cells[row, col].Style.VerticalAlignment = ExcelVerticalAlignment.Center;      //垂直對齊
            row++; col = 1;

            sheet.Cells[row, col].Value = "代扣健保費";
            sheet.Cells[row, col].Style.Border.BorderAround(ExcelBorderStyle.Thin);             //邊框
            sheet.Cells[row, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;    //水平對齊
            sheet.Cells[row, col].Style.VerticalAlignment = ExcelVerticalAlignment.Center;      //垂直對齊
            col++;

            sheet.Cells[row, col, row, col + 1].Merge = true;
            sheet.Cells[row, col, row, col + 1].Style.Border.BorderAround(ExcelBorderStyle.Thin);             //邊框
            sheet.Cells[row, col, row, col + 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;   //水平對齊
            sheet.Cells[row, col, row, col + 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;      //垂直對齊
            col += 2;

            sheet.Cells[row, col].Value = item.Health;
            sheet.Cells[row, col].Style.Border.BorderAround(ExcelBorderStyle.Thin);             //邊框
            sheet.Cells[row, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;   //水平對齊
            sheet.Cells[row, col].Style.VerticalAlignment = ExcelVerticalAlignment.Center;      //垂直對齊
            row++; col = 1;

            sheet.Cells[row, col].Value = "應發";
            sheet.Cells[row, col].Style.Border.BorderAround(ExcelBorderStyle.Thin);             //邊框
            sheet.Cells[row, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;    //水平對齊
            sheet.Cells[row, col].Style.VerticalAlignment = ExcelVerticalAlignment.Center;      //垂直對齊
            col++;

            sheet.Cells[row, col, row, col + 1].Merge = true;
            sheet.Cells[row, col, row, col + 1].Style.Border.BorderAround(ExcelBorderStyle.Thin);             //邊框
            sheet.Cells[row, col, row, col + 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;   //水平對齊
            sheet.Cells[row, col, row, col + 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;      //垂直對齊
            col += 2;

            sheet.Cells[row, col].Formula = "=D23+D24+D25+D26+D27-D28-D29-D30";
            sheet.Cells[row, col].Style.Numberformat.Format = "0";                              //去除小數點
            sheet.Cells[row, col].Style.Border.BorderAround(ExcelBorderStyle.Thin);             //邊框
            sheet.Cells[row, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;   //水平對齊
            sheet.Cells[row, col].Style.VerticalAlignment = ExcelVerticalAlignment.Center;      //垂直對齊
            row++; col = 1;

            #endregion

            sheet.Cells[sheet.Dimension.Address].AutoFitColumns(15); //自動調整列寬
            //sheet.Cells[sheet.Dimension.Address].Style.Font.Size = 12; // 設定 EXCEL 字型大小
            return ep.GetAsByteArray();
            
        }

        /// <summary>
        /// 計算特休總時數
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        private int CountSpecialLeaveHour(DateTime startDate, DateTime endDate)
        {
            int leapYearCount = 0;
            for (int i = startDate.Year; i <= endDate.Year; i++)
            {
                if (DateTime.IsLeapYear(i))
                {
                    if (i == startDate.Year) //如果是第一年，判斷是否有度過 2/29
                    {
                        if (startDate.Month < 2 || (startDate.Month == 2 && startDate.Day == 29))
                            leapYearCount++;
                    }
                    else if (i == endDate.Year) //如果是最後一年，判斷是否有度過 2/29 
                    {
                        if (startDate.Month > 2 || (startDate.Month == 2 && startDate.Day == 29))
                            leapYearCount++;
                    }
                    else 
                    {
                        leapYearCount++;
                    }
                }
                    
            }
            TimeSpan ts = endDate - startDate;
            int SLHour = 0, workDay = (int)ts.TotalDays - leapYearCount;
            if (workDay < 183) //工作未滿半年
            {
                SLHour = 0;
            }
            else if (workDay < 365) //工作滿半年，未滿一年
            {
                SLHour = 24;
            }
            else if (workDay < 365 * 2) //工作滿 1 年，未滿 2 年
            {
                SLHour = 56;
            }
            else if (workDay < 365 * 3) //工作滿 2 年，未滿 3 年
            {
                SLHour = 80;
            }
            else if (workDay < 365 * 5) //工作滿 3~4 年，未滿 5 年
            {
                SLHour = 112;
            }
            else if (workDay < 365 * 10) //工作滿 5~9 年，未滿 10 年
            {
                SLHour = 120;
            }
            else if (workDay < 365 * 11)//工作滿 10 年，未滿 11 年
            {
                SLHour = 128;
            }
            else if (workDay < 365 * 12)//工作滿 11 年，未滿 12 年
            {
                SLHour = 136;
            }
            else if (workDay < 365 * 13)//工作滿 12 年，未滿 13 年
            {
                SLHour = 144;
            }
            else if (workDay < 365 * 14)//工作滿 13 年，未滿 14 年
            {
                SLHour = 152;
            }
            else if (workDay < 365 * 15)//工作滿 14 年，未滿 15 年
            {
                SLHour = 160;
            }
            else if (workDay < 365 * 16)//工作滿 15 年，未滿 16 年
            {
                SLHour = 168;
            }
            else if (workDay < 365 * 17)//工作滿 16 年，未滿 17 年
            {
                SLHour = 176;
            }
            else if (workDay < 365 * 18)//工作滿 17 年，未滿 18 年
            {
                SLHour = 184;
            }
            else if (workDay < 365 * 19)//工作滿 18 年，未滿 19 年
            {
                SLHour = 192;
            }
            else if (workDay < 365 * 20)//工作滿 19 年，未滿 20 年
            {
                SLHour = 200;
            }
            else if (workDay < 365 * 21)//工作滿 20 年，未滿 21 年
            {
                SLHour = 208;
            }
            else if (workDay < 365 * 22)//工作滿 21 年，未滿 22 年
            {
                SLHour = 216;
            }
            else if (workDay < 365 * 23)//工作滿 22 年，未滿 23 年
            {
                SLHour = 224;
            }
            else if (workDay < 365 * 24)//工作滿 23 年，未滿 24 年
            {
                SLHour = 232;
            }
            else //工作滿 24 年以上
            {
                SLHour = 240;
            }

            return SLHour;
        }

        #region 未實作項目

        public IEnumerable<Salary> GetAllDatas()
        {
            throw new NotImplementedException();
        }

        public byte[] ExportExcel(List<SalaryViewModel> items, string fileName)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
