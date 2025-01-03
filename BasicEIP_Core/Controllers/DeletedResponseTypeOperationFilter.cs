using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace BasicEIP_Core.Controllers
{
    /// <summary>
    /// 自訂的 OpenAPI 運行過濾器，用於根據方法上的 DeletedResponseTypeAttribute 屬性設定回應的預設 Schema 和狀態碼描述。
    /// </summary>
    public class DeletedResponseTypeOperationFilter : IOperationFilter
    {
        /// <summary>
        /// 修改 OpenAPI 操作以添加預設的回應狀態碼和 Schema。
        /// </summary>
        /// <param name="operation">OpenAPI 操作物件，表示 API 方法的描述。</param>
        /// <param name="context">操作過濾器的上下文，提供方法資訊和 Schema 生成工具。</param>
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            // 獲取 DeletedResponseTypeAttribute 屬性
            var deletedResponseTypeAttribute = context.MethodInfo
                .GetCustomAttributes(typeof(DeletedResponseTypeAttribute), false)
                .FirstOrDefault() as DeletedResponseTypeAttribute;

            // 如果找到屬性則繼續處理
            if (deletedResponseTypeAttribute != null)
            {
                // 使用 SchemaGenerator 生成指定 ResponseType 的 Schema
                var schema = context.SchemaGenerator.GenerateSchema(
                    deletedResponseTypeAttribute.ResponseType,
                    context.SchemaRepository);

                // 處理 200 狀態碼的回應
                if (operation.Responses.ContainsKey("200"))
                {
                    // 更新已有的 200 回應
                    operation.Responses["200"] = new OpenApiResponse
                    {
                        Description = "成功", // 回應的描述
                        Content = new Dictionary<string, OpenApiMediaType>
                        {
                            ["application/json"] = new OpenApiMediaType
                            {
                                Schema = schema // 指定回應的 Schema
                            }
                        }
                    };
                }
                else
                {
                    // 添加新的 200 回應
                    operation.Responses.Add("200", new OpenApiResponse
                    {
                        Description = "成功", // 回應的描述
                        Content = new Dictionary<string, OpenApiMediaType>
                        {
                            ["application/json"] = new OpenApiMediaType
                            {
                                Schema = schema // 指定回應的 Schema
                            }
                        }
                    });
                }

                // 確保存在其他常見的狀態碼描述
                AddResponseIfNotExists(operation, "400", "請求無效"); // 錯誤的請求
                AddResponseIfNotExists(operation, "401", "權限無效"); // 未經授權
                AddResponseIfNotExists(operation, "404", "資源未找到"); // 找不到資源
            }
        }

        /// <summary>
        /// 如果回應中不存在指定的狀態碼，則添加新的回應。
        /// </summary>
        /// <param name="operation">OpenAPI 操作物件。</param>
        /// <param name="statusCode">HTTP 狀態碼。</param>
        /// <param name="description">狀態碼的描述。</param>
        private void AddResponseIfNotExists(OpenApiOperation operation, string statusCode, string description)
        {
            if (!operation.Responses.ContainsKey(statusCode))
            {
                operation.Responses.Add(statusCode, new OpenApiResponse
                {
                    Description = description
                });
            }
        }
    }
}

