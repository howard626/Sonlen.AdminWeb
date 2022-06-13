using Sonlen.WebAdmin.Model;

namespace Sonlen.AdminWeb.Service
{
    public interface ISalaryService : IPrintService<SalaryViewModel>
    {
        /// <summary>
        /// 取得上個月的薪資資料
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<SalaryViewModel?> GetDataByModelAsync(SalaryViewModel model);
    }
}