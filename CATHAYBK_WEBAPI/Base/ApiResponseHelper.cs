using BasicEIP_Core.ApiResponse;
using BasicEIP_Core.Enum;

namespace TMSAPP_WEBAPI.Base
{
    /// <summary>
    /// 1=成功
    /// 0=失敗, E000 = 查無會員資料, E001=參數欄位不完整, E002 = 參數內容有誤
    /// </summary>
    public class ApiResponseHelper
    {
        /// <summary>
        /// 建立成功的 API 響應
        /// </summary>
        public static ApiResponse<T> Success<T>(string funcId, string accountNo, T data, string message = "成功")
        {
            return new ApiResponse<T>(
                data: data,
                responseCode: $"{(int)RspnCode.Success}",
                responseMessage: message
            );
        }

        /// <summary>
        /// 建立失敗的 API 響應
        /// </summary>
        public static ApiResponse<string> Error(string funcId, string accountNo, RspnCode rspnCode, string message = "錯誤")
        {
            return new ApiResponse<string>(
                data: null,
                responseCode: rspnCode.Equals(RspnCode.Failure) ? $"{(int)RspnCode.Failure}" : ((int)rspnCode).ToString("D4"), // 格式化成4位數，例如 1000 -> E000
                responseMessage: message
            );
        }
    }
}
