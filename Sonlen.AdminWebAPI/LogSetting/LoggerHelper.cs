using log4net;

namespace Sonlen.AdminWebAPI.LogSetting
{
    public class LoggerHelper
    {
        //一般紀錄
        private static readonly ILog loginfo = LogManager.GetLogger("loginfo");
        //錯誤紀錄
        private static readonly ILog logerror = LogManager.GetLogger("logerror");
        //監察紀錄
        private static readonly ILog logmonitor = LogManager.GetLogger("logmonitor");

        public static void Error(string ErrorMsg, Exception? ex = null)
        {
            if (ex != null)
            {
                logerror.Error(ErrorMsg, ex);
            }
            else
            {
                logerror.Error(ErrorMsg);
            }
        }

        public static void Info(string Msg)
        {
            loginfo.Info(Msg);
        }

        public static void Monitor(string Msg)
        {
            logmonitor.Info(Msg);
        }
    }
}
