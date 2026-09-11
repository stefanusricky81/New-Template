using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SendEmailE_Recruitment.Classes
{
    class Tools
    {
        public static void write_log(string str, string curtype, string shortcode = "SendMailE-Recruitment")
        {
            DateTime Now = DateTime.Now;
            try
            {
                string myDate = Now.Date.ToString("yyyy-MM-dd");
                string myFile = "C:\\Logs\\e-Recruitment\\SendMailE-Recruitment\\" + curtype.ToLower() + "\\" + curtype.ToLower() + "-" + myDate + ".txt";

                if (!Directory.Exists("C:\\Logs\\e-Recruitment\\SendMailE-Recruitment"))
                    Directory.CreateDirectory("C:\\Logs\\e-Recruitment\\SendMailE-Recruitment");

                if (!Directory.Exists(@"C:\Logs\e-Recruitment\SendMailE-Recruitment\" + curtype.ToLower()))
                    Directory.CreateDirectory(@"C:\Logs\e-Recruitment\SendMailE-Recruitment\" + curtype.ToLower());


                StreamWriter sw = default(StreamWriter);
                if (File.Exists(myFile))
                {
                    sw = File.AppendText(myFile);
                }
                else
                {
                    sw = File.CreateText(myFile);
                }
                sw.WriteLine(str);
                sw.Close();
            }
            catch (Exception ex)
            {
                string logSTR = Now.ToString("yyyy-MM-dd HH:mm:ss") + "," + str + "\t" + ex.Message + "\r\n\r\n";
                write_log(logSTR, "Err");
            }
        }
    }
}
