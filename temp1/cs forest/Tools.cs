using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.IO;
using System.Net;
using System.Threading;

namespace cs_forest
{
    public class Tools
    {
        public void SendEmail(string EmailAddress, string EmailMessage, string subject)
        {
            try
            {
                string useremail = EmailAddress;
                StringBuilder strMessage = new StringBuilder();
                string SendFrom = "alert@forest-interactive.com";
                string SendTo = useremail;
                string newstxt = "<font face=\"Calibri\" size=\"3\">" + EmailMessage + "</font>";
                string newstitle = subject;


                SmtpClient client = new SmtpClient();
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.EnableSsl = true;
                client.Host = "smtp.office365.com";
                client.Port = 587;

                // setup Smtp authentication
                System.Net.NetworkCredential credentials = new System.Net.NetworkCredential("alert@forest-interactive.com", "Server9@9@","@forest-interactive.com");
                client.UseDefaultCredentials = false;
                client.Credentials = credentials;

                MailMessage msg = new MailMessage();
                msg.From = new MailAddress(SendFrom);
                msg.To.Add(new MailAddress(SendTo));

                msg.Subject = newstitle;
                msg.IsBodyHtml = true;
                msg.Body = newstxt;
                client.Send(msg);


            }
            catch (Exception ex)
            {

            }
        }

        public void process_log(string str, string logtype)
        {
            //curtype =mo  or mt  

            try
            {
                string myFile = null;
                string myDate = DateTime.Now.Date.ToString("yyyyMMdd");
                string myDir = null;
                switch (logtype)
                {
                    case "process":
                        myDir = @"C:\LOGS\CSForest\Process\\";
                        break;
                    case "err":
                        myDir = @"C:\LOGS\CSForest\Err\\";
                        break;
                }

                myFile = myDir + myDate + ".txt";


                StreamWriter sw = default(StreamWriter);
                DirectoryInfo dir = new DirectoryInfo(myDir);
                if (dir.Exists == false)
                {
                    Directory.CreateDirectory(myDir);
                }

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
                throw;
            }

        }

        public string WebReq(string urlStr)
        {
            string result = string.Empty;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(urlStr);
            try
            {
                request.Proxy = null;
                using (var response = request.GetResponse() as HttpWebResponse)
                {
                    if (request.HaveResponse && response != null)
                    {
                        using (var reader = new StreamReader(response.GetResponseStream()))
                        {
                            result = reader.ReadToEnd();
                        }
                    }
                }
            }
            catch (WebException ex)
            {
                if (ex.Status == WebExceptionStatus.ProtocolError)
                {
                    result = ((HttpWebResponse)ex.Response).StatusCode.ToString();
                }
            }

            return result;
        }

        public string MessageId()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(RandomInt.Instance.Next(1000000, 9999999));
            builder.Append(RandomInt.Instance.Next(100000, 999999));
            builder.Append(RandomInt.Instance.Next(10000, 99999));
            return builder.ToString();
        }

        public class RandomLetter
        {
            static Random randomChar = new Random();
            static Random randomCharNext = new Random((int)DateTime.Now.Ticks);
            public static string GetLetter(int size)
            {
                StringBuilder builder = new StringBuilder();
                int num = randomChar.Next(11, 26);
                char ch;
                for (int i = 0; i < size; i++)
                {
                    ch = Convert.ToChar(Convert.ToInt64(Math.Floor(num * randomCharNext.NextDouble() + 65)));
                    builder.Append(ch);
                }
                return builder.ToString().ToLower();
            }
        }

        public class RandomInt
        {
            private static int seed;
            private static ThreadLocal<Random> threadLocal = new ThreadLocal<Random>
                (() => new Random(Interlocked.Increment(ref seed)));

            static RandomInt()
            {
                seed = Environment.TickCount;
            }
            public static Random Instance { get { return threadLocal.Value; } }
        }
    }
}