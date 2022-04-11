using System.Text;
using System.Text.Json;

namespace Sonlen.AdminWebAPI.LogSetting
{
    public class WebApiTrackerMiddleware
    {
        private readonly RequestDelegate _next;

        public WebApiTrackerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            string param = string.Empty;

            WebApiMonitorLog MonLog = new WebApiMonitorLog
            {
                ExecuteStartTime = DateTime.Now,
                HttpRequestHeaders = GetHeaders(context),
                HttpMethod = context.Request.Method
            };

            //操作Request.Body之前加上EnableBuffering即可
            context.Request.EnableBuffering();
            var reader = new StreamReader(context.Request.Body, Encoding.UTF8);
            MonLog.ActionParams = await reader.ReadToEndAsync();

            // 重置Position
            context.Request.Body.Seek(0, SeekOrigin.Begin);

            await _next(context);

            if (context.Request.Path.HasValue)
            {
                if (context.Request.Path.Value.Contains("/api"))
                {
                    if (context.Request.Method != "OPTIONS")
                    {
                        string[] path = context.Request.Path.Value.Split('/');
                        MonLog.ExecuteEndTime = DateTime.Now;
                        if (path.Length >= 4)
                        {
                            MonLog.ActionName = path[3];
                            MonLog.ControllerName = path[2];
                        }
                        LoggerHelper.Monitor(MonLog.GetLoginfo());
                    }
                }
            }
        }

        private string GetHeaders(HttpContext context) 
        {
            string result = string.Empty;

            // Put the names of all keys into a string array.
            foreach (var key in context.Request.Headers.Keys)
            {
                result += $"{key} : {context.Request.Headers[key]} \r\n";
            }

            return result;
        }
    }
}
