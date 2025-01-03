namespace BasicEIP_Core.Enum
{
    /// <summary>
    /// API 響應代碼枚舉
    /// </summary>
    public enum RspnCode
    {
        /// <summary>
        /// 成功
        /// </summary>
        Success = 1,

        /// <summary>
        /// 失敗
        /// </summary>
        Failure = 0,

        /// <summary>
        /// 查無會員資料
        /// </summary>
        NoMemberData = 1000, // E000 可對應到 1000

        /// <summary>
        /// 參數欄位不完整
        /// </summary>
        MissingParameter = 1001, // E001 可對應到 1001

        /// <summary>
        /// 參數內容有誤
        /// </summary>
        InvalidParameter = 1002 // E002 可對應到 1002
    }
}
