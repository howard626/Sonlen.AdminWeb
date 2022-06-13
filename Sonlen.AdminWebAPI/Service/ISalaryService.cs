using Sonlen.WebAdmin.Model;
using System.Data;

namespace Sonlen.AdminWebAPI.Service
{
    /// <summary>
    /// 薪資資料
    /// </summary>
    public interface ISalaryService :IDataService<Salary>, IPrintService<SalaryViewModel,SalaryViewModel>
    {
        /// <summary>
        /// 繪製 EXCEL
        /// </summary>
        /// <param name="item">薪資資料</param>
        /// <param name="fileName">檔案名稱</param>
        /// <param name="employee">員工資料</param>
        /// <param name="company">公司資料</param>
        /// <param name="attendance">考勤資料</param>
        /// <returns></returns>
        byte[] ExportExcel(SalaryViewModel item, string fileName, Employee employee, Company company, Attendance attendance);
    }
}