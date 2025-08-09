using Microsoft.AspNetCore.Mvc.Filters;

namespace AniwalkServer.Filters
{
    /// <summary>
    /// 行為紀錄器
    /// </summary>
    public class LogFilter : IActionFilter
    {
        /// <summary>
        /// 行為紀錄檔路徑
        /// </summary>
        const string LogFilePath = "LogFiles/ActionLog";

        public void OnActionExecuting(ActionExecutingContext context)
        {

        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            var Area = context.RouteData.Values["area"];
            var controller = context.RouteData.Values["controller"];
            var action = context.RouteData.Values["action"];
            var id = context.RouteData.Values["id"];

            var agent = context.HttpContext.Request.Headers["User-Agent"].ToString();
            var ip = context.HttpContext.Connection.RemoteIpAddress?.ToString();

            string user = "Guest";
            var time = DateTime.Now;


            string logMessage = $"{time.ToString("HH-mm-ss")}\t{user}\t{ip}\t{agent}\t{Area}\t{controller}/{action}/{id}";


            // 寫入日誌系統
            string File = $"{time.ToString("yyyy-MM-dd")}.txt";

            if (!Directory.Exists(LogFilePath))
            {
                Directory.CreateDirectory(LogFilePath);
            }

            //寫檔
            using (StreamWriter writer = new StreamWriter(Path.Combine(LogFilePath, File), true))
            {
                writer.WriteLine(logMessage);
            }
        }
    }
}
