namespace BasicEIP_Core.ApiResponse
{
    public class ApiResponse<T>
    {
        /// <summary>
        /// 回傳內容
        /// </summary>
        public T RSPN_DATA { get; set; }

        /// <summary>
        /// 執行結果代碼 (1=成功, 0=失敗, E000~E002)
        /// </summary>
        public string RSPN_CODE { get; set; }

        /// <summary>
        /// 執行結果說明
        /// </summary>
        public string RSPN_MSG { get; set; }

        /// <summary>
        /// 初始化 ApiResponse 實例。
        /// </summary>
        /// <param name="data">回傳資料。</param>
        /// <param name="responseCode">執行結果代碼。</param>
        /// <param name="responseMessage">執行結果訊息。</param>
        public ApiResponse(
            T data,
            string responseCode,
            string responseMessage)
        {
            RSPN_DATA = data;
            RSPN_CODE = responseCode;
            RSPN_MSG = responseMessage;
        }
    }
}
