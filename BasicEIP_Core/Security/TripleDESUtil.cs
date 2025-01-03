using System.Security.Cryptography;

namespace BasicEIP_Core.Security
{
    /// <summary>
    /// (公用類別)提供TripleDES演算法編解碼的公用類別
    /// </summary>
    public class TripleDESUtil
    {

        #region (公用靜態方法)Encrypt(string,string,string),Encrypt(string,string,string,Encoding)
        /// <summary>
        /// (公用靜態方法)對.NET字串編碼
        /// </summary>
        /// <param name="mvKey">TripleDES演算法的秘密金鑰</param>
        /// <param name="mvIV">對稱演算法的初始化向量</param>
        /// <param name="mvClearData">要編碼的.NET字串</param>
        /// <returns>編碼後的Base64字串</returns>
        public static string Encrypt(string mvKey, string mvIV, string mvInputString)
        {
            return Encrypt(mvKey, mvIV, mvInputString, System.Text.Encoding.UTF8);
        }
        /// <summary>
        /// (公用靜態方法)對.NET字串編碼
        /// </summary>
        /// <param name="mvKey">TripleDES演算法的秘密金鑰</param>
        /// <param name="mvIV">對稱演算法的初始化向量</param>
        /// <param name="mvClearData">要編碼的.NET字串</param>
        /// <param name="mvEncoding">編碼方式</param>
        /// <returns>編碼後的Base64字串</returns>
        public static string Encrypt(string mvKey, string mvIV, string mvInputString, System.Text.Encoding mvEncoding)
        {
            MemoryStream objMS = null;
            CryptoStream objCryptoStream = null;
            string strResultString = "";
            try
            {
                TripleDESCryptoServiceProvider objTDES = new();
                objMS = new MemoryStream();

                objCryptoStream = new CryptoStream(objMS,
                    objTDES.CreateEncryptor(mvEncoding.GetBytes(mvKey),
                    mvEncoding.GetBytes(mvIV)),
                    CryptoStreamMode.Write);

                byte[] aryBytesInput = mvEncoding.GetBytes(mvInputString);
                objCryptoStream.Write(aryBytesInput, 0, aryBytesInput.Length);
                objCryptoStream.FlushFinalBlock();

                strResultString = Convert.ToBase64String(objMS.ToArray());
            }
            catch (Exception ee)
            {
                throw new Exception("TripleDES資料加密發生錯誤 !!", ee);
            }
            finally
            {
                objCryptoStream?.Close();
                objMS?.Close();
            }
            return strResultString;
        }
        #endregion (公用靜態方法)

        #region (公用靜態方法)Decrypt(string,string,string),Decrypt(string,string,string,Encoding)
        /// <summary>
        /// (公用靜態方法)對Base64字串解碼
        /// </summary>
        /// <param name="mvKey">TripleDES演算法的秘密金鑰</param>
        /// <param name="mvIV">對稱演算法的初始化向量</param>
        /// <param name="mvBase64InputString">要解碼的Base64字串</param>
        /// <returns>解碼後的.NET字串</returns>
        public static string Decrypt(string mvKey, string mvIV, string mvBase64InputString)
        {
            return Decrypt(mvKey, mvIV, mvBase64InputString, System.Text.Encoding.UTF8);
        }
        /// <summary>
        /// (公用靜態方法)對Base64字串解碼
        /// </summary>
        /// <param name="mvKey">TripleDES演算法的秘密金鑰</param>
        /// <param name="mvIV">對稱演算法的初始化向量</param>
        /// <param name="mvBase64InputString">要解碼的Base64字串</param>
        /// <param name="mvEncoding">編碼方式</param>
        /// <returns>解碼後的.NET字串</returns>
        public static string Decrypt(string mvKey, string mvIV, string mvBase64InputString, System.Text.Encoding mvEncoding)
        {
            MemoryStream objMS = null;
            CryptoStream objCryptoStream = null;
            string strResultString = "";
            try
            {
                TripleDESCryptoServiceProvider objTDES = new();
                objMS = new MemoryStream();
                objCryptoStream = new CryptoStream(objMS,
                    objTDES.CreateDecryptor(mvEncoding.GetBytes(mvKey),
                    mvEncoding.GetBytes(mvIV)),
                    CryptoStreamMode.Write);

                byte[] aryBytesInput = Convert.FromBase64String(mvBase64InputString);
                objCryptoStream.Write(aryBytesInput, 0, aryBytesInput.Length);
                objCryptoStream.FlushFinalBlock();

                strResultString = mvEncoding.GetString(objMS.ToArray());
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                //TODO: 發生錯誤時Middleware會捕捉不到
                //throw new Exception("TripleDES資料解密發生錯誤 !!", e);
            }
            finally
            {
                objCryptoStream?.Close();
                objMS?.Close();
            }
            return strResultString;
        }
        #endregion (公用靜態方法)
    }
}
