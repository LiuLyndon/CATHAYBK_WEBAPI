using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace BasicEIP_Core.Controllers
{
    /// <summary>
    /// 自訂的 OpenAPI 運行過濾器，用於為方法添加 `201 Created` 狀態碼和其他相關回應描述。
    /// </summary>
    public class CreatedResponseTypeOperationFilter : IOperationFilter
    {
        /// <summary>
        /// 修改 OpenAPI 操作以添加 `201 Created` 狀態碼和通用的回應描述。
        /// </summary>
        /// <param name="operation">OpenAPI 操作物件，表示 API 方法的描述。</param>
        /// <param name="context">操作過濾器的上下文，提供方法資訊和 Schema 生成工具。</param>
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            // 從方法描述中取得 CreatedResponseTypeAttribute 屬性
            var createdResponseTypeAttribute = context.MethodInfo
                .GetCustomAttributes(typeof(CreatedResponseTypeAttribute), false)
                .FirstOrDefault() as CreatedResponseTypeAttribute;

            // 如果找到屬性則處理回應
            if (createdResponseTypeAttribute != null)
            {
                // 使用 SchemaGenerator 生成指定 ResponseType 的 Schema
                var schema = context.SchemaGenerator.GenerateSchema(
                    createdResponseTypeAttribute.ResponseType,
                    context.SchemaRepository);

                // 移除不需要的 200 狀態碼回應（如果存在）
                if (operation.Responses.ContainsKey("200"))
                {
                    operation.Responses.Remove("200");
                }


                // 添加 `201 Created` 的回應
                AddResponseIfNotExists(operation, "201", "已建立", schema);

                // 添加其他通用狀態碼回應
                AddResponseIfNotExists(operation, "400", "請求無效");
                AddResponseIfNotExists(operation, "401", "權限無效");
            }
        }

        /// <summary>
        /// 如果回應中不存在指定的狀態碼，則添加新的回應。
        /// </summary>
        /// <param name="operation">OpenAPI 操作物件。</param>
        /// <param name="statusCode">HTTP 狀態碼。</param>
        /// <param name="description">狀態碼的描述。</param>
        /// <param name="schema">可選的回應 Schema，如果適用。</param>
        private void AddResponseIfNotExists(OpenApiOperation operation, string statusCode, string description, OpenApiSchema? schema = null)
        {
            if (!operation.Responses.ContainsKey(statusCode))
            {
                var response = new OpenApiResponse
                {
                    Description = description
                };

                // 如果提供了 Schema，則添加為 JSON 的回應內容
                if (schema != null)
                {
                    response.Content = new Dictionary<string, OpenApiMediaType>
                    {
                        ["application/json"] = new OpenApiMediaType
                        {
                            Schema = schema
                        }
                    };
                }

                operation.Responses.Add(statusCode, response);
            }
        }
    }
}

