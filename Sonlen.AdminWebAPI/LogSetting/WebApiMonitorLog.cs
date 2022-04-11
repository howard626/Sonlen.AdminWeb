namespace Sonlen.AdminWebAPI.LogSetting
{
    /// <summary>
    /// 監控日誌對象
    /// </summary>
    public class WebApiMonitorLog
    {
        /// <summary>
        /// 控制器名稱
        /// </summary>
        public string ControllerName { get; set; } = string.Empty;

        /// <summary>
        /// 動作名稱
        /// </summary>
        public string ActionName { get; set; } = string.Empty;

        /// <summary>
        /// 執行時間(起)
        /// </summary>
        public DateTime ExecuteStartTime { get; set; }

        /// <summary>
        /// 執行時間(迄)
        /// </summary>
        public DateTime ExecuteEndTime { get; set; }

        /// <summary>
        /// 請求的Action 參數
        /// </summary>
        public string ActionParams { get; set; } = string.Empty;

        /// <summary>
        /// Http請求頭
        /// </summary>
        public string HttpRequestHeaders { get; set; } = string.Empty;

        /// <summary>
        /// 請求方式
        /// </summary>
        public string HttpMethod { get; set; } = string.Empty;

        /// <summary>
        /// 獲取監控指標日誌
        /// </summary>
        /// <returns></returns>
        public string GetLoginfo()
        {
            return $@"
            Action執行時間監控：
            ControllerName: {ControllerName}
            ActionName:{ActionName}
            Method:{HttpMethod}
            開始時間：{ExecuteStartTime}
            結束時間：{ExecuteEndTime}
            總 時 間：{(ExecuteEndTime - ExecuteStartTime).TotalSeconds}秒
            Action參數：{ActionParams}";
        }
    }
}
