using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace BasicEIP_Core.Middleware
{
    /// <summary>
    /// 自訂的請求日誌記錄中介軟體
    /// </summary>
    public class RequestLoggingMiddleware
    {
        // 問題診斷：詳細記錄每個請求和響應，幫助開發人員快速定位問題。
        // 安全審計：可以追蹤所有進入和退出系統的請求，這在安全和合規要求下非常重要。
        // 性能監控：通過記錄每個請求的處理時間，幫助識別和解決性能瓶頸。
        // 操作記錄：記錄關鍵操作的請求，有助於數據的追蹤和變更歷史的保存。
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestLoggingMiddleware> _logger;

        // 中介軟體的建構子，注入 RequestDelegate 和 Logger
        public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        // 處理 HTTP 請求的主要方法
        public async Task InvokeAsync(HttpContext context)
        {
            // 記錄請求訊息
            await LogRequest(context);


            // 使用 Response.OnStarting 來記錄響應
            //context.Response.OnStarting(async () =>
            //{
            //    await LogResponse(context);
            //});

            // 記錄響應訊息
            await LogResponse(context);
        }

        // 記錄請求訊息
        private async Task LogRequest(HttpContext context)
        {
            context.Request.EnableBuffering(); // 允許重複讀取 Request.Body

            var request = context.Request;
            var buffer = new byte[Convert.ToInt32(request.ContentLength)];
            await request.Body.ReadAsync(buffer, 0, buffer.Length);
            var requestBody = System.Text.Encoding.UTF8.GetString(buffer);
            request.Body.Position = 0; // 重置 Request.Body 位置

            _logger.LogInformation($"HTTP Request Information: {request.Method} {request.Path} {request.QueryString} Body: {requestBody}");
        }

        // 記錄響應訊息
        private async Task LogResponse(HttpContext context)
        {
            var originalBodyStream = context.Response.Body;
            try
            {
                using (var responseBody = new MemoryStream())
                {
                    context.Response.Body = responseBody;

                    // 先讓後續中介軟體處理響應
                    await _next(context);

                    // 記錄響應
                    context.Response.Body.Seek(0, SeekOrigin.Begin);
                    var responseText = await new StreamReader(context.Response.Body).ReadToEndAsync();
                    context.Response.Body.Seek(0, SeekOrigin.Begin);

                    _logger.LogInformation($"HTTP Response Information: Status Code: {context.Response.StatusCode} Body: {responseText}");

                    // 把內容寫回原始流
                    await responseBody.CopyToAsync(originalBodyStream);
                }
            }
            finally
            {
                //未作此步驟會讓context.Response.Body關閉導致錯誤
                context.Response.Body = originalBodyStream;
            }
        }
    }

    // 擴充方法，用於將中介軟體加入到應用程式的請求管道中
    public static class RequestLoggingMiddlewareExtensions
    {
        public static IApplicationBuilder UseRequestLogging(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RequestLoggingMiddleware>();
        }
    }
}

