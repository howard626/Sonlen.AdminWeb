using Sonlen.WebAdmin.Model;

namespace Sonlen.AdminWeb.Service
{
    /// <summary>
    /// 列印
    /// </summary>    
    /// <returns></returns>
    public interface IPrintService<T>
    {
        /// <summary>
        /// 列印
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        Task<UploadFile> PrintAsync(T viewModel);
    }
}
