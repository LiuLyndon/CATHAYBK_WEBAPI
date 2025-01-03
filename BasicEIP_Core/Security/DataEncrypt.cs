namespace BasicEIP_Core.Security
{
    public class DataEncrypt
    {
        /// <summary>
        /// 資料加密
        /// </summary>
        /// <param name="input">輸入</param>
        /// <param name="key">Key</param>
        /// <param name="iv">IV</param>
        /// <returns>加密結果</returns>
        public static string Encrypt(string input, string key = Const.PasswordKey, string iv = Const.PasswordIV)
        {
            key = key + iv + Const.PasswordKey;
            key = key[..16];
            string s = TripleDESUtil.Encrypt(key, iv, input);

            return s;
        }

        /// <summary>
        /// 資料解密
        /// </summary>
        /// <param name="input">輸入</param>
        /// <param name="key">Key</param>
        /// <param name="iv">IV</param>
        /// <returns>解密結果</returns>
        public static string Decrypt(string input, string key = Const.PasswordKey, string iv = Const.PasswordIV)
        {
            key = key + iv + Const.PasswordKey;
            key = key[..16];
            string s = TripleDESUtil.Decrypt(key, iv, input);

            return s;
        }
    }
}
