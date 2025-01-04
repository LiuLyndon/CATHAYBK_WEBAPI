using BasicEIP_Core.ApiResponse;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.ComponentModel.DataAnnotations;

namespace BasicEIP_Core.Controllers
{
    /// <summary>W
    /// 自訂的 Action 過濾器屬性，用於在操作執行之前驗證模型狀態是否有效。
    /// </summary>
    /// <summary>
    /// 自訂的 Action 過濾器屬性，用於在操作執行之前驗證模型狀態是否有效。
    /// </summary>
    public class ValidateModelAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// 在操作執行之前觸發，檢查模型狀態是否有效。
        /// </summary>
        /// <param name="context">操作執行的上下文，提供當前請求的相關資訊。</param>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            // 獲取請求的路徑
            // var path = context.HttpContext.Request.Path.ToString().ToLower();

            // 獲取動作的第一個參數作為請求物件（假設只有一個參數並為模型）
            var request = context.ActionArguments.Values.FirstOrDefault() as dynamic;

            // 檢查模型是否為 null
            if (request == null)
            {
                context.Result = new BadRequestObjectResult(
                    CreateErrorResponse(
                        responseMessage: "Request cannot be null.")
                );
                return;
            }

            // 如果模型狀態無效，設定操作結果為 400 Bad Request 並返回錯誤訊息
            if (!context.ModelState.IsValid)
            {
                var validationContext = new ValidationContext(request);
                var validationResults = new List<ValidationResult>();

                Validator.TryValidateObject(request, validationContext, validationResults, true);

                context.Result = new BadRequestObjectResult(
                    CreateErrorResponse(
                        responseMessage: string.Join("; ", validationResults.Select(v => v.ErrorMessage).ToList())
                    )
                );
                return;
            }

            // 檢查模型是否為 null
            if (context.ActionArguments.Values.Any(v => v == null))
            {
                context.Result = new BadRequestObjectResult(
                        new ApiResponse<string>(
                        data: null,
                        responseMessage: "Request cannot be null."
                    )
                );
                return;
            }
        }

        /// <summary>
        /// 創建錯誤響應的輔助方法，根據路徑動態生成不同的 JSON 結構。
        /// </summary>
        private object CreateErrorResponse(string responseMessage)
        {
            // 預設錯誤格式
            return new
            {
                RSPN_DATA = string.Empty,
                RSPN_MSG = responseMessage
            };
        }
    }
}
