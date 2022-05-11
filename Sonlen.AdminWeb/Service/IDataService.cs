namespace Sonlen.AdminWeb.Service
{
    /// <summary>
    /// DB 基礎CRUD
    /// </summary>
    /// <returns></returns>
    public interface IDataService<T>
    {
        /// <summary>
        /// 新增資料
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        Task<string> AddDataAsync(T item);

        /// <summary>
        /// 刪除資料
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        Task<string> DeleteDataAsync(T item);

        /// <summary>
        /// 取得全部資料
        /// </summary>
        /// <returns></returns>
        Task<List<T>> GetAllDataAsync();

        /// <summary>
        /// 以 ID 取得資料
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<T?> GetDataByIDAsync(string id);

        /// <summary>
        /// 更新資料
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        Task<string> UpdateDataAsync(T item);
    }
}
