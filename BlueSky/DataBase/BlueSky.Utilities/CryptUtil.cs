using System;
using System.Security.Cryptography;
using System.Text;

namespace BlueSky.Utilities
{
    public class CryptUtil
    {
        public static string MD5Encrypt(string _strSource)
        {
            if (string.IsNullOrEmpty(_strSource))
                return "";
            MD5 md5Factory = MD5.Create();
            byte[] byteSource = Encoding.Default.GetBytes(_strSource);
            byte[] byteMd5 = md5Factory.ComputeHash(byteSource);
            string strResult = BitConverter.ToString(byteMd5).Replace("-", "");
            return strResult;
        }
    }
}
