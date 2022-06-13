using Sonlen.WebAdmin.Model;

namespace Sonlen.AdminWebAPI.Service
{
    /// <summary>
    /// 列印動作
    /// </summary>
    /// <typeparam name="T">接收 MODEL</typeparam>
    /// <typeparam name="T2">列印 MODEL</typeparam>
    public interface IPrintService<T, T2>
    {
        /// <summary>
        /// 列印資料
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        UploadFile Print(T model);

        /// <summary>
        /// 繪製 EXCEL
        /// </summary>
        /// <param name="items"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        byte[] ExportExcel(List<T2> items, string fileName);
    }
}
