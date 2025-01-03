using Microsoft.Extensions.Logging;
using System.Security.Cryptography;
using System.Text;

namespace BasicEIP_Core.Security
{
    /// <summary>
    /// 加密 MD5 (https://emn178.github.io/online-tools/md5.html)
    /// </summary>
    public class TXNDATAHASHMD5Encrypt
    {
        // 從 appsettings.json 讀取金鑰和 IV
        private readonly ILogger<TXNDATAHASHMD5Encrypt> _logger;

        public TXNDATAHASHMD5Encrypt(ILogger<TXNDATAHASHMD5Encrypt> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="plainText">明文</param>
        /// <returns>加密後的字串 (Base64)</returns>
        public static string Encrypt(string input)
        {
            try
            {
                using (MD5 md5 = MD5.Create())
                {
                    byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                    byte[] hashBytes = md5.ComputeHash(inputBytes);

                    StringBuilder sb = new StringBuilder();
                    foreach (byte b in hashBytes)
                    {
                        sb.Append(b.ToString("x2")); // 轉換為16進位字串
                    }

                    return sb.ToString();
                }
            }
            catch (Exception)
            {

                throw;
            }

        }
    }
}
