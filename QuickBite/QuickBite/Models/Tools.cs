using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace QuickBite.Models
{
    public class Tools
    {
        public static string Encryption(string input)
        {
            char[] input_temp = input.ToCharArray();
            string[] key = new string[input_temp.Length];

            StringBuilder builder = new StringBuilder();
            Random random = new Random();

            for (int i = 0; i < input_temp.Length; i++)
            {
                string random_int = new string(Enumerable.Repeat("0123456789", 3).Select(s => s[random.Next(s.Length)]).ToArray());
                byte[] asciiBytes = Encoding.ASCII.GetBytes(input_temp[i].ToString());

                if (asciiBytes[0].ToString().Length == 2)
                    key[i] = "0" + asciiBytes[0].ToString() + random_int;
                else
                    key[i] = asciiBytes[0].ToString() + random_int;

                builder.Append(key[i]);
            }
            byte[] plainTextBytes = System.Text.Encoding.UTF8.GetBytes(builder.ToString());
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public static string Decryption(string input)
        {
            byte[] base64EncodedBytes = System.Convert.FromBase64String(input);
            string x = System.Text.Encoding.UTF8.GetString(base64EncodedBytes);

            List<string> a = new List<string>();
            for (int i = 0; i < x.Length; i += 6)
            {
                if ((i + 6) < x.Length)
                    a.Add(x.Substring(i, 6));
                else
                    a.Add(x.Substring(i));
            }

            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < a.Count; i++)
            {
                builder.Append(Char.ConvertFromUtf32(Convert.ToInt32(a[i].Substring(0, 3))));
            }

            return builder.ToString();

        }
    }
}