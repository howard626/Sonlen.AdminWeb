namespace Sonlen.AdminWebAPI.Service
{
    public interface IDataService<T>
    {
        /// <summary>
        /// 取得全部資料
        /// </summary>
        /// <returns></returns>
        IEnumerable<T> GetAllDatas();

        /// <summary>
        /// 以 ID 取得資料
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        T? GetDataByID(string id);

        /// <summary>
        /// 新增資料
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        string InsertData(T item);

        /// <summary>
        /// 更新資料
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        string UpdateData(T item);

        /// <summary>
        /// 刪除資料
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        string DeleteData(T item);
    }
}
