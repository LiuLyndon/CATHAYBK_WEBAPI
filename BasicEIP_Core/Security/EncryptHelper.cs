using System.Security.Cryptography;
using System.Text;

namespace BasicEIP_Core.Security
{
    /// <summary>
    /// 加解密Helper
    /// </summary>
    public class EncryptHelper
    {
        /// <summary>
        /// Sha256
        /// </summary>
        /// <param name="input">輸入</param>
        /// <returns></returns>
        public static string Sha256(string input)
        {
            using SHA256 sha256 = SHA256.Create();

            // 將輸入轉換為位元組陣列，並計算哈希值
            byte[] bytes = Encoding.UTF8.GetBytes(input);
            byte[] hashBytes = sha256.ComputeHash(bytes);

            // 將哈希值轉換為十六進制字串
            StringBuilder builder = new();
            for (int i = 0; i < hashBytes.Length; i++)
            {
                builder.Append(hashBytes[i].ToString("x2"));
            }

            return builder.ToString();
        }

        /// <summary>
        /// Triple Des
        /// </summary>
        /// <param name="input">輸入</param>
        /// <param name="key">Key</param>
        /// <returns></returns>
        public static string EncryptTripleDes(string input, string key)
        {
            if (string.IsNullOrEmpty(input))
            {
                return string.Empty;
            }

            using var des = TripleDES.Create();
            des.Key = Encoding.UTF8.GetBytes(key);
            des.IV = Encoding.UTF8.GetBytes(Const.PasswordIV);

            using var encryptor = des.CreateEncryptor();
            byte[] plainBytes = Encoding.UTF8.GetBytes(input);
            byte[] encryptedBytes = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);

            return Convert.ToBase64String(encryptedBytes);
        }

        /// <summary>
        /// 解密TripleDes
        /// </summary>
        /// <param name="input">輸入</param>
        /// <param name="key">Key</param>
        /// <returns></returns>
        public static string DecryptTripleDes(string input, string key, string iv = Const.PasswordIV)
        {
            if (string.IsNullOrEmpty(input))
            {
                return string.Empty;
            }

            using var aesAlg = Aes.Create();
            // 設置金鑰和初始化向量
            aesAlg.Key = Encoding.UTF8.GetBytes(key);
            aesAlg.IV = Encoding.UTF8.GetBytes(iv);

            // 使用解密模式
            aesAlg.Mode = CipherMode.CBC;

            // 使用PKCS7填充模式
            aesAlg.Padding = PaddingMode.PKCS7;

            // 建立解密器
            using var decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
            // 將加密後的文字轉換為位元組
            byte[] encryptedBytes = Convert.FromBase64String(input);

            // 解密
            using var msDecrypt = new MemoryStream(encryptedBytes);
            using var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);
            using var srDecrypt = new StreamReader(csDecrypt);
            return srDecrypt.ReadToEnd();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="key"></param>
        /// <param name="iv"></param>
        /// <returns></returns>
        public static string ExtractPassword(string input, string key = Const.PasswordKey, string iv = Const.PasswordIV)
        {
            if (string.IsNullOrEmpty(input))
            {
                return string.Empty;
            }

            key = key + iv + Const.PasswordKey;
            key = key[..16];
            string s = TripleDESUtil.Decrypt(key, iv, input);

            return s;
        }
    }

}
