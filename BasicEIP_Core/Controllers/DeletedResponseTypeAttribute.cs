using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BasicEIP_Core.Controllers
{
    /// <summary>
    /// 自訂屬性，用於設定方法的回應類型，其中包括 `204 No Content` 狀態碼以及其他常見狀態碼。
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class DeletedResponseTypeAttribute : Attribute, IFilterMetadata
    {
        /// <summary>
        /// 定義 API 回應的類型。
        /// </summary>
        public Type ResponseType { get; set; }  // 屬性是公開的並具有 getter 和 setter
        /// <summary>
        /// 建構函式，初始化屬性。
        /// </summary>
        public DeletedResponseTypeAttribute(Type responseType)
        {
            ResponseType = responseType;
        }

        /// <summary>
        /// 在操作執行前觸發，為操作添加通用的 ProducesResponseType 特性。
        /// </summary>
        /// <param name="context">操作執行上下文，提供有關當前操作的資訊。</param>
        public void OnActionExecuting(ActionExecutingContext context)
        {
            // 添加通用的 ProducesResponseType 特性，指定常見的 HTTP 狀態碼
            context.Filters.Add(new ProducesResponseTypeAttribute(ResponseType, 204)); // 刪除成功，無內容
            context.Filters.Add(new ProducesResponseTypeAttribute(400)); // 錯誤的請求
            context.Filters.Add(new ProducesResponseTypeAttribute(401)); // 未經授權
            context.Filters.Add(new ProducesResponseTypeAttribute(404)); // 找不到資源
        }

        /// <summary>
        /// 在操作執行後觸發（本案例中未使用）。
        /// </summary>
        /// <param name="context">操作執行後的上下文，提供有關執行結果的資訊。</param>
        public void OnActionExecuted(ActionExecutedContext context)
        {
            // 本案例不需要在操作執行後執行任何邏輯，因此留空
        }
    }
}
