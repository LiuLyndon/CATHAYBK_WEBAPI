using BasicEIP_Core.Connection;
using BasicEIP_Core.Controllers;
using BasicEIP_Core.Middleware;
using BasicEIP_Core.Swagger;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using NLog.Web;
using NLog;
using NLog.Extensions.Logging;

var logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();

try
{
    logger.Debug("Application starting...");

    var builder = WebApplication.CreateBuilder(args);

    // 設定 NLog 為 .NET Core 的日誌記錄提供者
    builder.Logging.ClearProviders(); // 清除預設的日誌記錄提供者
    builder.Logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace); // 設定最低記錄等級為 Trace
    builder.Logging.AddNLog(); // 加入 NLog 作為日誌記錄提供者

    // 配置應用程式設定（如連線字串、系統設定等）
    ConfigureAppSettings(builder);

    // 配置應用程式的依賴注入服務 (DI)
    ConfigureServices(builder);

    // 建立應用程式物件
    // ---------------------------------------------------------
    var app = builder.Build();

    // 配置中介軟體 (Middleware)
    ConfigureMiddleware(app);

    // 啟動應用程式
    app.Run();
}
catch (Exception ex)
{
    logger.Error(ex, "應用程式因例外終止。");
    throw;
}
finally
{
    LogManager.Shutdown();
}

/// <summary>
/// 配置應用程式設定
/// </summary>
/// <param name="builder">WebApplication 建構器</param>
void ConfigureAppSettings(WebApplicationBuilder builder)
{
    // 設定 ConnectionStrings 與 SystemSetting 至 IOptions
    builder.Services.Configure<ConnectionStrings>(builder.Configuration.GetSection("ConnectionStrings"));
}

/// <summary>
/// 配置應用程式的依賴注入服務 (DI)
/// </summary>
/// <param name="builder">WebApplication 建構器</param>
void ConfigureServices(WebApplicationBuilder builder)
{
    // 設定 CORS 政策
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowAllOrigins", policy =>
        {
            policy.AllowAnyOrigin() // 允許來自任何來源的請求
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });
    });

    // 配置 MVC 與 JSON 輸出格式
    builder.Services.AddControllers(options =>
    {
        options.RespectBrowserAcceptHeader = false; // 預設回傳 JSON 格式
        options.OutputFormatters.RemoveType<StringOutputFormatter>(); // 移除字串輸出格式
        options.OutputFormatters.RemoveType<HttpNoContentOutputFormatter>(); // 移除 NoContent 格式

        //options.OutputFormatters.Insert(0, new SystemTextJsonOutputFormatter(new JsonSerializerOptions
        //{
        //    TypeInfoResolver = new DefaultJsonTypeInfoResolver() // 使用系統預設的 JSON 格式化
        //}));
    }).AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = null; // 禁用命名策略，保持屬性名稱大小寫一致
    });

    // 關閉自動模型驗證錯誤的回應，改為自訂錯誤處理
    builder.Services.Configure<ApiBehaviorOptions>(options =>
    {
        options.SuppressModelStateInvalidFilter = true;
    });

    // 註冊應用程式自訂服務
    // builder.Services.AddRepositories(); // 註冊資料庫倉儲服務
    // builder.Services.AddAppLogging(); // 註冊應用程式日誌服務

    // 配置 Swagger，用於 API 文件生成與測試
    builder.Services.AddEndpointsApiExplorer();

    string xmlPath = Path.Combine(AppContext.BaseDirectory, $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml");
    builder.Services.AddSwaggerGen(options =>
    {
        // 設定 XML 註解檔案的位置
        options.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);

        // 啟用 Swagger 中的 Model 描述
        options.SchemaFilter<AddModelSummarySchemaFilter>(xmlPath);

        options.SwaggerDoc("v1", new OpenApiInfo { Title = "TMS API", Version = "v1" });
        // 
        options.OperationFilter<DefaultResponseTypeOperationFilter>();
        options.OperationFilter<QueriedResponseTypeOperationFilter>();
        options.OperationFilter<CreatedResponseTypeOperationFilter>();
        options.OperationFilter<UpdatedResponseTypeOperationFilter>();
        options.OperationFilter<DeletedResponseTypeOperationFilter>();

    });

    // 配置 Session 和快取
    builder.Services.AddSession();
    builder.Services.AddDistributedMemoryCache();

    // 註冊 HttpContextAccessor，用於訪問 HTTP 上下文
    builder.Services.AddHttpContextAccessor();
}

/// <summary>
/// 配置中介軟體
/// </summary>
/// <param name="app">WebApplication 實例</param>
void ConfigureMiddleware(WebApplication app)
{
    // 啟用 Swagger，用於 API 文件生成與測試
    if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
    {
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.RoutePrefix = string.Empty; // 設定 Swagger UI 路徑為根目錄
            string swaggerJsonBasePath = string.IsNullOrWhiteSpace(c.RoutePrefix) ? "." : "..";
            c.SwaggerEndpoint($"{swaggerJsonBasePath}/swagger/v1/swagger.json", "My API V1");
        });
    }

    // 啟用自訂中介軟體
    app.UseExceptionHandling(); // 設定例外處理中介軟體
    app.UseRequestLogging(); // 設定請求日誌中介軟體

    // 啟用 HTTPS、CORS、驗證與授權
    app.UseHttpsRedirection(); // 強制將 HTTP 請求重導到 HTTPS
    app.UseCors("AllowAllOrigins"); // 啟用允許所有來源的 CORS 政策
    app.UseAuthentication(); // 啟用身份驗證
    // app.UseJwtTokenMonitor(); // 啟用自訂的 JWT Token 監控中介軟體
    app.UseAuthorization(); // 啟用授權檢查
    app.UseSession(); // 啟用 Session 管理

    // 設定路由
    app.MapControllers(); // 將 Controller 類別映射為路由
}