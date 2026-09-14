using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SendEmailE_Recruitment.Classes;
using System.Net;
using System.IO;
using System.Xml;
//using SendEmailE_Recruitment.GetData;

namespace SendEmailE_Recruitment
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            backgroundWorker1.RunWorkerAsync();
        }
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            //string email = string.Empty;
            string status = string.Empty;
            //try
            //{
            //    GetData.WsRecruitmentSoap a = new GetData.WsRecruitmentSoapClient();
            //    DataTable dt = a.GetEmail();

            //    for (int i = 0; i < dt.Rows.Count; i++)
            //    {
            //        email = dt.Rows[i][0].ToString();
                    status = sendmail("com.com");//ganti alamat email disini
            //        if (status == "Success")
            //        {
            //            a.UpdateSendEmail(ref email);
            //            Tools.write_log(DateTime.Now.ToString() + "," + "RetrieveWebServices" + "," + email, "Send Email");
            //        }
            //        else
            //            Tools.write_log(DateTime.Now.ToString() + "," + "Failed Send Email" + "," + email, "Failed Send Email");
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Tools.write_log(DateTime.Now.ToString() + "," + "RetrieveWebServices" + "," + ex.Message, "errRetrieveWebServices");
            //}
        }

        private string sendmail(string to)
        {
            SmtpClient smtp = new SmtpClient();
            string url = string.Empty;
            url = "http://www.x.com/VerifyEmail.aspx?email=" + to;
            try
            {
                MailMessage mm = new MailMessage();
                //mm.From = new MailAddress("Tiara.fatwa@londonsumatra.com");
                //mm.From = new MailAddress("Recruitment@londonsumatra.com");

                mm.From = new MailAddress("x.com");
                mm.Subject = "[E-Recruitment] Verify Your Email";//untuk ganti subject email

                mm.Body = "Please Click To Confirm Your's Email";//untuk ganti isi body email
                mm.IsBodyHtml = true;
                mm.To.Add(new MailAddress(to));
                smtp.Host = "mail.x.com";//excarray.londonsumatra.com
                smtp.EnableSsl = false; //Depending on server SSL Settings true/false
                System.Net.NetworkCredential NetworkCred = new System.Net.NetworkCredential();
                NetworkCred.UserName = "x";
                NetworkCred.Password = "x";
                //NetworkCred.UserName = "stefanus.ricky@londonsumatra.com";
                //NetworkCred.Password = "Ricky123!";
                smtp.UseDefaultCredentials = true;
                smtp.Credentials = NetworkCred;
                smtp.Port = 25;//Specify your port No;
                smtp.Send(mm);
                return "Success";
            }

            #region unuse
            //MailMessage mail = new MailMessage();
            //SmtpClient SmtpServer = new SmtpClient();
            //mail.To.Add(to);
            //mail.From = new MailAddress("Recruitment@Londonsumatra.com");
            //mail.Subject = "[E-Recruitment] Verify Your Email";
            //mail.IsBodyHtml = true;
            //mail.Body = "Please Click <a href = '" + url + "'><u>Here</u></a> To Confirm Your's Email";
            //SmtpServer.Host = "vsexcjkt01.londonsumatra.com";
            //SmtpServer.Port = 25;
            //SmtpServer.Credentials = CredentialCache.DefaultNetworkCredentials;
            //try
            //{
            //    SmtpServer.Send(mail);
            //    return "Success";
            //}
            #endregion
            catch (Exception ex)
            {
                Tools.write_log(DateTime.Now.ToString() + "," + "SendEmail" + "," + ex.Message, "ErrSendEmail");
                return ex + "Error";
            }
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Close();
        }
    }
}
