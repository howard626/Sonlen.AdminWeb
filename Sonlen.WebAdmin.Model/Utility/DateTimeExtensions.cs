using System.Globalization;


namespace Sonlen.WebAdmin.Model.Utility
{
    public static class DateTimeExtensions
    {
        /// <summary>
        /// 日期轉換成民國年格式日期字串
        /// </summary>
        /// <param name="datetime">日期物件</param>
        /// <param name="format">格式</param>
        /// <returns></returns>
        public static string ToTWDateString(this DateTime datetime, string format = "yyy/MM/dd")
        {
            CultureInfo info = new CultureInfo("zh-TW");
            TaiwanCalendar calendar = new TaiwanCalendar();
            info.DateTimeFormat.Calendar = calendar;

            string tmpString;

            if (datetime.Year < 1912)
            {
                int offsetYear = 1912 - datetime.Year;
                datetime = datetime.AddYears(offsetYear * 2 - 1);
                tmpString = datetime.ToString(format, info);
                tmpString = "民國前" + tmpString;
            }
            else
            {
                tmpString = datetime.ToString(format, info);
            }

            return tmpString;
        }

        /// <summary>
        /// 日期轉換成民國年格式日期字串
        /// </summary>
        /// <param name="datetime">日期物件</param>
        /// <param name="format">格式</param>
        /// <returns></returns>
        public static string ToTWDateString(this DateTime? datetime, string format = "yyy/MM/dd")
        {
            if (datetime == null)
            {
                return "";
            }
            else
            {
                return datetime.Value.ToTWDateString(format);
            }
        }
    }
}
