using BasicEIP_Core.ApiResponse;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;

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
            var code = HttpStatusCode.InternalServerError; // 預設為 500
            string responseMessage = "伺服器內部錯誤"; // 預設錯誤訊息
            object responseData = null;

            // 根據異常類型調整狀態碼和訊息
            switch (exception)
            {
                case ArgumentNullException:
                    code = HttpStatusCode.BadRequest;
                    responseMessage = "請求參數錯誤";
                    break;

                case UnauthorizedAccessException:
                    code = HttpStatusCode.Unauthorized;
                    responseMessage = "未經授權的請求";
                    break;

                // 其他特定異常類型處理...
                default:
                    _logger.LogError(exception, "未預期的錯誤");
                    break;
            }

            // 統一的 API 回應格式
            var response = new ApiResponse<object>(responseData, responseMessage);

            // 設置 HTTP 狀態碼和回應格式
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;

            // 將回應序列化為 JSON 並回傳
            await context.Response.WriteAsync(JsonSerializer.Serialize(response, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase, // 使用駝峰命名策略
                WriteIndented = true // 格式化輸出
            }));
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

