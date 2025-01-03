using System.ComponentModel.DataAnnotations;

namespace BasicEIP_Core.ApiResponse
{
    public class ApiRequest
    {
        /// <summary>
        /// API功能代碼
        /// </summary>
        [Required(ErrorMessage = "FUNC_ID (功能代碼)是必填欄位")]
        [MaxLength(50, ErrorMessage = "FUNC_ID (功能代碼)長度不得超過50字元")]
        public string FUNC_ID { get; set; } = string.Empty;

        /// <summary>
        /// 用戶帳號
        /// </summary>
        [Required(ErrorMessage = "ACNT_NO (用戶帳號)是必填欄位")]
        [MaxLength(32, ErrorMessage = "ACNT_NO (用戶帳號)長度不得超過32字元")]
        public string ACNT_NO { get; set; } = string.Empty;

        /// <summary>
        /// 用戶上傳時間 YYYYMMDDHHNNSS
        /// </summary>
        [Required(ErrorMessage = "SYS_DATE (用戶上傳時間)是必填欄位")]
        [MaxLength(14, ErrorMessage = "SYS_DATE (用戶上傳時間)長度不得超過14字元")]
        public string SYS_DATE { get; set; } = string.Empty;

        /// <summary>
        /// API認證簽章
        /// </summary>
        [Required(ErrorMessage = "FUNC_SIGNATURE (認證簽章)是必填欄位")]
        public string FUNC_SIGNATURE { get; set; } = string.Empty;

        /// <summary>
        /// API參數
        /// </summary>
        [Required(ErrorMessage = "FUNC_DATA (參數)是必填欄位")]
        public object FUNC_DATA { get; set; } = default!;
    }
}
