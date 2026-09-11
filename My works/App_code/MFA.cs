using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

/// <summary>
/// Summary description for MFA
/// </summary>
public class MFA
{
    public MFA()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    public class KeyGenerator
    {
        internal static readonly char[] chars = "1234567890".ToCharArray();
        internal static readonly char[] charsWithAlpha = "1234567890abcdefghijklmnopqrstuvwxyz".ToCharArray();
        public static string GetUniqueKey(int size)
        {
            byte[] data = new byte[4 * size];
            using (RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider())
            {
                crypto.GetBytes(data);
            }
            StringBuilder result = new StringBuilder(size);
            for (int i = 0; i < size; i++)
            {
                var rnd = BitConverter.ToUInt32(data, i * 4);
                var idx = rnd % chars.Length;

                result.Append(chars[idx]);
            }

            return result.ToString();
        }

        public static string EncryptSSN(string SSN)
        {
            string BaseString = ConfigurationManager.AppSettings["BaseString"].ToString();

            string EncryptedSSN = BitByBit.Utility.EncryptData(BaseString, SSN);

            return EncryptedSSN;
        }
    }
}