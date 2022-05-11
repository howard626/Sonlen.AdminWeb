namespace Sonlen.AdminWebAPI.Service
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
        public string Agree(T item);

        /// <summary>
        /// 駁回
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public string Disagree(T item);
        
    }
}
