using BasicEIP_Core.ApiResponse;
using BasicEIP_Core.Connection;
using BasicEIP_Core.Controllers;
using BasicEIP_Core.Middleware;
using BasicEIP_Core.Security;
using BasicEIP_Core.Swagger;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Rendering;
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

    // �]�w NLog �� .NET Core ����x�O�����Ѫ�
    builder.Logging.ClearProviders(); // �M���w�]����x�O�����Ѫ�
    builder.Logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace); // �]�w�̧C�O�����Ŭ� Trace
    builder.Logging.AddNLog(); // �[�J NLog �@����x�O�����Ѫ�

    // �t�m���ε{���]�w�]�p�s�u�r��B�t�γ]�w���^
    ConfigureAppSettings(builder);

    // �t�m���ε{�����̿�`�J�A�� (DI)
    ConfigureServices(builder);

    // �إ����ε{������
    // ---------------------------------------------------------
    var app = builder.Build();

    // �t�m�����n�� (Middleware)
    ConfigureMiddleware(app);

    // �Ұ����ε{��
    app.Run();
}
catch (Exception ex)
{
    logger.Error(ex, "���ε{���]�ҥ~�פ�C");
    throw;
}
finally
{
    LogManager.Shutdown();
}

/// <summary>
/// �t�m���ε{���]�w
/// </summary>
/// <param name="builder">WebApplication �غc��</param>
void ConfigureAppSettings(WebApplicationBuilder builder)
{
    // �]�w ConnectionStrings �P SystemSetting �� IOptions
    builder.Services.Configure<ConnectionStrings>(builder.Configuration.GetSection("ConnectionStrings"));
}

/// <summary>
/// �t�m���ε{�����̿�`�J�A�� (DI)
/// </summary>
/// <param name="builder">WebApplication �غc��</param>
void ConfigureServices(WebApplicationBuilder builder)
{
    // �]�w CORS �F��
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowAllOrigins", policy =>
        {
            policy.AllowAnyOrigin() // ���\�Ӧۥ���ӷ����ШD
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });
    });

    // �t�m MVC �P JSON ��X�榡
    builder.Services.AddControllers(options =>
    {
        options.RespectBrowserAcceptHeader = false; // �w�]�^�� JSON �榡
        options.OutputFormatters.RemoveType<StringOutputFormatter>(); // �����r���X�榡
        options.OutputFormatters.RemoveType<HttpNoContentOutputFormatter>(); // ���� NoContent �榡

        //options.OutputFormatters.Insert(0, new SystemTextJsonOutputFormatter(new JsonSerializerOptions
        //{
        //    TypeInfoResolver = new DefaultJsonTypeInfoResolver() // �ϥΨt�ιw�]�� JSON �榡��
        //}));
    }).AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = null; // �T�ΩR�W�����A�O���ݩʦW�٤j�p�g�@�P
    });

    // �����۰ʼҫ����ҿ��~���^���A�אּ�ۭq���~�B�z
    builder.Services.Configure<ApiBehaviorOptions>(options =>
    {
        options.SuppressModelStateInvalidFilter = true;
    });

    // ���U���ε{���ۭq�A��
    // builder.Services.AddRepositories(); // ���U��Ʈw���x�A��
    // builder.Services.AddAppLogging(); // ���U���ε{����x�A��
    // builder.Services.AddSingleton<IJwtUtils, JwtTokenUtil>(); // ���U JWT �u�����O

    // �t�m Swagger�A�Ω� API ���ͦ��P����
    builder.Services.AddEndpointsApiExplorer();

    string xmlPath = Path.Combine(AppContext.BaseDirectory, $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml");
    builder.Services.AddSwaggerGen(options =>
    {
        // �]�w XML �����ɮת���m
        options.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);

        // �ҥ� Swagger ���� Model �y�z
        options.SchemaFilter<AddModelSummarySchemaFilter>(xmlPath);

        options.SwaggerDoc("v1", new OpenApiInfo { Title = "TMS API", Version = "v1" });
        options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header, // JWT ��b�ШD�� Header
            Description = "�п�J JWT Token�A�榡�� Bearer <token>",
            Name = "Authorization", // Header ���W��
            Type = SecuritySchemeType.Http, // ���v������ HTTP
            BearerFormat = "JWT", // ���w�ϥ� JWT �榡
            Scheme = "Bearer" // �ϥ� Bearer ����
        });
        options.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
                },
                Array.Empty<string>() // ���ݭn�B�~���d��
            }
        });
        // 
        options.OperationFilter<DefaultResponseTypeOperationFilter>();
        options.OperationFilter<QueriedResponseTypeOperationFilter>();
        options.OperationFilter<CreatedResponseTypeOperationFilter>();
        options.OperationFilter<UpdatedResponseTypeOperationFilter>();
        options.OperationFilter<DeletedResponseTypeOperationFilter>();

    });

    // �t�m Session �M�֨�
    builder.Services.AddSession();
    builder.Services.AddDistributedMemoryCache();

    // ���U HttpContextAccessor�A�Ω�X�� HTTP �W�U��
    builder.Services.AddHttpContextAccessor();
}

/// <summary>
/// �t�m�����n��
/// </summary>
/// <param name="app">WebApplication ���</param>
void ConfigureMiddleware(WebApplication app)
{
    // �ҥ� Swagger�A�Ω� API ���ͦ��P����
    if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
    {
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.RoutePrefix = string.Empty; // �]�w Swagger UI ���|���ڥؿ�
            string swaggerJsonBasePath = string.IsNullOrWhiteSpace(c.RoutePrefix) ? "." : "..";
            c.SwaggerEndpoint($"{swaggerJsonBasePath}/swagger/v1/swagger.json", "My API V1");
        });
    }

    // �ҥΦۭq�����n��
    app.UseExceptionHandling(); // �]�w�ҥ~�B�z�����n��
    app.UseRequestLogging(); // �]�w�ШD��x�����n��

    // �ҥ� HTTPS�BCORS�B���һP���v
    app.UseHttpsRedirection(); // �j��N HTTP �ШD���ɨ� HTTPS
    app.UseCors("AllowAllOrigins"); // �ҥΤ��\�Ҧ��ӷ��� CORS �F��
    app.UseAuthentication(); // �ҥΨ�������
    // app.UseJwtTokenMonitor(); // �ҥΦۭq�� JWT Token �ʱ������n��
    app.UseAuthorization(); // �ҥα��v�ˬd
    app.UseSession(); // �ҥ� Session �޲z

    // �]�w����
    app.MapControllers(); // �N Controller ���O�M�g������
}