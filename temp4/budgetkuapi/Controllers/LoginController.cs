using budgetkuapi.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Net.Mime;
using System.Web.Http;
using ZXing;
using ZXing.Common;

namespace budgetkuapi.Controllers
{
    public class LoginController : ApiController
    {
        DataTable resAllowUser, resEmail = null;
        private static Random random = new Random();
        string param, loginuser, email, companycode, username, resSendEmail, token = string.Empty;
        int resUpdateLoginApp = 0;

        public object Post(HttpRequestMessage request)
        {
            var statuscode = new Status.StatusCode();
            if (request.Headers != null && request.Headers.Authorization != null && request.Headers.Authorization.Scheme == "Basic")
            {
                string auth_scheme = request.Headers.Authorization.Scheme;
                string auth_parameter = request.Headers.Authorization.Parameter;
                string content = request.Content.ReadAsStringAsync().Result; // body

                if (auth_parameter == "admin")
                {
                    var objects = JObject.Parse(content);
                    loginuser = (string)objects["loginuser"];

                    if (loginuser == null || loginuser == string.Empty)
                    {
                        return Status.statuscode(405);
                    }
                    else
                    {
                        resAllowUser = DataAccess.GetAllowUser(loginuser);
                        if (resAllowUser.Rows.Count > 0)
                        {
                            companycode = resAllowUser.Rows[0]["CompanyCode"].ToString();
                            string[] strLoginUser = loginuser.Split('@'); 
                            username = strLoginUser[0];
                            resEmail = DataAccess.GetEmail(username, companycode);
                            if (resEmail.Rows.Count > 0)
                            {
                                email = resEmail.Rows[0]["mail"].ToString();
                                resSendEmail = sendmail(email);
                                if (resSendEmail == "200")
                                {
                                    resUpdateLoginApp = DataAccess.UpdateLoginApp(loginuser, token);
                                    if (resUpdateLoginApp == 0)
                                    {
                                        return Status.statuscode(200);
                                    }
                                    else
                                    {
                                        return Status.statuscode(400);
                                    }
                                }
                                else
                                {
                                    return resSendEmail;
                                    //return Status.statuscode(400);
                                }

                            }
                            else
                            {
                                return Status.statuscode(409);
                            }
                        }
                        else
                        {
                            return Status.statuscode(408);
                        }
                    }
                }
                else
                {
                    return Status.statuscode(401);
                }
            }
            else
            {
                return Status.statuscode(402);
            }
        }

        protected string generateqrcode()
        {
            var qrWriter = new BarcodeWriter()
            {
                Format = BarcodeFormat.QR_CODE,
                Options = new EncodingOptions() { Height = 500, Width = 500, Margin = 5 }
            };
            token = RandomToken(20);
            param = token+"*"+loginuser;
            using (var q = qrWriter.Write(param))
            {
                using (var ms = new MemoryStream())
                {
                    q.Save(ms, ImageFormat.Png);

                    return Convert.ToBase64String(ms.ToArray());
                }
            }
        }
        public static string RandomToken(int length)
        {
            string chars = "12345678901234567ABCDEFGHIJKLMNOPQRSTUVWYXZabcdefghijklmnopqrstuvwyxz";
            return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
        }
        protected string sendmail(string emailaddress)
        {
            SmtpClient smtp = new SmtpClient();
            string filename = string.Empty;
            string attachmentPath = string.Empty;

            try
            {
                MailMessage mm = new MailMessage();
                mm.From = new MailAddress("Lonsum.e-Apps@londonsumatra.com");
                mm.Subject = "Authorization";
                filename = username + DateTime.Now.ToString("ddMMMyyyyhhmmss");

                string base64 = generateqrcode();
                byte[] bytes = Convert.FromBase64String(base64);
                using (Image image = Image.FromStream(new MemoryStream(bytes)))
                {
                    image.Save(@"C:\BudgetKuQRCode\" + filename + ".jpg", ImageFormat.Jpeg);
                    attachmentPath = @"C:\BudgetKuQRCode\" + filename + ".jpg";
                    LinkedResource LinkedImage = new LinkedResource(attachmentPath);
                    LinkedImage.ContentId = "MyPic";
                    //Added the patch for Thunderbird as suggested by Jorge
                    LinkedImage.ContentType = new ContentType(MediaTypeNames.Image.Jpeg);
                    AlternateView htmlView = AlternateView.CreateAlternateViewFromString(
                      "Please scan qrcode below: " + "<BR/> <img width='250' height='250' src=cid:MyPic>",
                      null, "text/html");

                    htmlView.LinkedResources.Add(LinkedImage);
                    mm.AlternateViews.Add(htmlView);
                }

                mm.IsBodyHtml = false;
                mm.To.Add(new MailAddress(emailaddress));
                //smtp.Host = "mail.simp.co.id";
                smtp.Host = "mail.londonsumatra.com";
                smtp.EnableSsl = false; //Depending on server SSL Settings true/false
                System.Net.NetworkCredential NetworkCred = new System.Net.NetworkCredential();
                NetworkCred.UserName = "e-apps";
                NetworkCred.Password = "54321";
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = NetworkCred;
                smtp.Port = 25;//Specify your port No;
                smtp.Send(mm);
                //
                return "200";
            }
            catch (Exception ex)
            {
                //return ex.Message;
                return "400";
            }
        }
    }
}
