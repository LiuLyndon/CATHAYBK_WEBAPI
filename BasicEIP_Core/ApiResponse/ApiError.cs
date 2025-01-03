namespace BasicEIP_Core.ApiResponse
{
    /// <summary>
    /// API 錯誤格式結構
    /// </summary>
    public class ApiError
    {
        /// <summary>
        /// 定義的 Code
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 錯誤訊息
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 詳細訊息
        /// </summary>
        public string Details { get; set; }
    }
}
