namespace Sonlen.AdminWeb.Service
{
    /// <summary>
    /// 審核
    /// </summary>    
    /// <returns></returns>
    public interface IConfirmService<T>
    {
        /// <summary>
        /// 同意
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        Task<string> AgreeAsync(T item);

        /// <summary>
        /// 駁回
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        Task<string> DisagreeAsync(T item);
    }
}
