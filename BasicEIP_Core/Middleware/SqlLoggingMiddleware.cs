using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace BasicEIP_Core.Middleware
{
    /// <summary>
    /// 記錄 SQL 語法
    /// </summary>
	public class SqlLoggingMiddleware : IMiddleware
    {
        private readonly ILogger<SqlLoggingMiddleware> _logger;

        public SqlLoggingMiddleware(ILogger<SqlLoggingMiddleware> logger)
        {
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            // 執行下一個 Middleware
            await next(context);

            stopwatch.Stop();

            // 在 UserRepository 中，你需要在每次執行 SQL 查詢之前將 SQL 語法保存到 HttpContext.Items
            // 這樣 SqlLoggingMiddleware 就能夠攔截並記錄這些 SQL 語法。

            // 假設你在某處使用 Dapper 執行 SQL 語法
            // 使用 Log Information 記錄 SQL 語法與執行時間
            // 這裡假設你有一個地方能夠存取到 SQL 語法
            var sql = context.Items["ExecutedSQL"] as string;
            if (!string.IsNullOrEmpty(sql))
            {
                _logger.LogInformation($"SQL: {sql} executed in {stopwatch.ElapsedMilliseconds}ms");
            }
        }
    }
}

