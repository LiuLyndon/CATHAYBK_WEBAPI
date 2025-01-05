using System.Security.Cryptography;

namespace BasicEIP_Core.Security
{
    public class AESService
    {
        private readonly string _key;
        private readonly string _iv;

        public AESService(string key, string iv)
        {
            _key = key;
            _iv = iv;
        }

        public string Encrypt(string plainText)
        {
            using (var aes = Aes.Create())
            {
                aes.Key = Convert.FromBase64String(_key);
                aes.IV = Convert.FromBase64String(_iv);

                using (var encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
                using (var ms = new MemoryStream())
                {
                    using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    using (var writer = new StreamWriter(cs))
                    {
                        writer.Write(plainText);
                    }
                    return Convert.ToBase64String(ms.ToArray());
                }
            }
        }

        public string Decrypt(string encryptedText)
        {
            using (var aes = Aes.Create())
            {
                aes.Key = Convert.FromBase64String(_key);
                aes.IV = Convert.FromBase64String(_iv);

                using (var decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
                using (var ms = new MemoryStream(Convert.FromBase64String(encryptedText)))
                using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                using (var reader = new StreamReader(cs))
                {
                    return reader.ReadToEnd();
                }
            }
        }
    }
}