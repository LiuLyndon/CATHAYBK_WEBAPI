using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net;

namespace BasicEIP_Core.Middleware
{
    /// <summary>
    /// 自訂的異常處理中介軟體
    /// </summary>
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        /// <summary>
        /// 建構式，初始化請求委派和記錄器
        /// </summary>
        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        /// <summary>
        /// 異常處理主方法，捕捉未處理的異常並處理
        /// </summary>
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "未處理的異常發生");
                await HandleExceptionAsync(context, ex);
            }
        }

        /// <summary>
        /// 異常處理的輔助方法，根據異常類型生成適當的回應
        /// </summary>
        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            string message = "未知錯誤";
            string details = "未處理的異常類型";
            string funcId = context.Request.Headers["FUNC_ID"].ToString() ?? "未知";
            string accountNo = context.Request.Headers["ACNT_NO"].ToString() ?? "未知";
            string sysDate = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
            object responseData = null;
            string responseCode = "0"; // 默認為失敗
            string responseMessage = "發生內部伺服器錯誤";

            var code = HttpStatusCode.InternalServerError;

        }
    }

    // 擴充方法，用於將中介軟體加入到應用程式的請求管道中
    public static class ExceptionHandlingMiddlewareExtensions
    {
        public static IApplicationBuilder UseExceptionHandling(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionHandlingMiddleware>();
        }
    }
}

