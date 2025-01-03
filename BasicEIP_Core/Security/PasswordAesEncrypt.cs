using Microsoft.Extensions.Logging;
using System.Security.Cryptography;
using System.Text;

namespace BasicEIP_Core.Security
{
    /// <summary>
    /// 密碼加解密
    /// </summary>
    public class PasswordAesEncrypt
    {
        // 從 appsettings.json 讀取金鑰和 IV
        private readonly ILogger<PasswordAesEncrypt> _logger;

        public PasswordAesEncrypt(ILogger<PasswordAesEncrypt> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="plainText">明文</param>
        /// <returns>加密後的字串 (Base64)</returns>
        public static string Encrypt(string password, string iv)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(iv);
                aes.IV = Encoding.UTF8.GetBytes(iv.Substring(0, 16)); // 16 字元 IV

                using (MemoryStream ms = new MemoryStream())
                using (CryptoStream cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    byte[] plainBytes = Encoding.UTF8.GetBytes(password);
                    cs.Write(plainBytes, 0, plainBytes.Length);
                    cs.FlushFinalBlock();
                    return Convert.ToBase64String(ms.ToArray());
                }
            }
        }

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="cipherText">加密字串 (Base64)</param>
        /// <returns>解密後的明文</returns>
        public static string Decrypt(string password, string iv)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(iv);
                aes.IV = Encoding.UTF8.GetBytes(iv.Substring(0, 16)); // 16 字元 IV

                using (MemoryStream ms = new MemoryStream())
                using (CryptoStream cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    byte[] cipherBytes = Convert.FromBase64String(password);
                    cs.Write(cipherBytes, 0, cipherBytes.Length);
                    cs.FlushFinalBlock();
                    return Encoding.UTF8.GetString(ms.ToArray());
                }
            }
        }


        /// <summary>
        /// 計算 SHA256 雜湊值 (https://emn178.github.io/online-tools/sha256.html)
        /// </summary>
        /// <param name="rawData">原始資料</param>
        /// <returns>SHA256 雜湊值（十六進位格式）</returns>
        public static string ComputeSha256Hash(string rawData)
        {
            // 若API功能代碼為「KGMBS_QUERY_MEMBER」
            // 用戶上傳時間為「20190701123000」
            // 用戶帳號為「0912345678」
            // 認證金鑰為「12345678」
            // 則簽章為「FUNC_ID=KGMBS_QUERY_MEMBER&SYS_DATE=20190701123000&ACNT_NO=0912345678&API_KEY=12345678」取 SHA256 運算結果
            using (SHA256 sha256 = SHA256.Create())
            {
                // 將字串轉換為位元組陣列
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                // 將位元組陣列轉換為十六進位字串
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
