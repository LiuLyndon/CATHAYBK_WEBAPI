namespace BasicEIP_Core.Security
{
    /// <summary>
    /// 密碼加解密
    /// </summary>
    public class PasswordEncrypt
    {
        /// <summary>
        /// 解密密碼
        /// </summary>
        /// <param name="password">密碼</param>
        /// <param name="userID">UserID</param>
        /// <param name="transactionTime">交易時間</param>
        /// <returns></returns>
        public static string ExtractPassword(string password, string userID, string transactionTime)
        {
            if (string.IsNullOrEmpty(password) || string.IsNullOrEmpty(userID) || string.IsNullOrEmpty(transactionTime))
            {
                return string.Empty;
            }

            try
            {
                string pw = DataEncrypt.Decrypt(password, userID, transactionTime);
                return (string.IsNullOrEmpty(pw) ? "" : Encrypt(userID, pw));
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="salt">加鹽</param>
        /// <param name="password">密碼</param>
        /// <returns></returns>
        public static string Encrypt(string salt, string password)
        {
            salt += Const.PasswordSlat;
            return EncryptHelper.Sha256(salt + password);
        }
    }
}
