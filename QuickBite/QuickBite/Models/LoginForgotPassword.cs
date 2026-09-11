using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;

namespace QuickBite.Models
{
    public class LoginForgotPassword
    {
        public string status { get; set; }

        public static List<LoginForgotPassword> ResetPassword(string username)
        {
            List<LoginForgotPassword> Register = new List<LoginForgotPassword>();
            LoginForgotPassword details = new LoginForgotPassword();

            int countuser = new LoginForgotPassword().SelectDbLogin(username);
            if (countuser > 0)
            {
                sendGroupNotification(username, "", countuser);
                details.status = "success";
            }
            else
            {
                details.status = "not success";
            }
            Register.Add(details);
        
            return Register;
        }

        private static void sendGroupNotification(string username1, string noteMsg, int pwd)
        {
            try
            {
                string AA = "http://cms.lawaapp.com/QuickBite/resetpassword.aspx?c=" + pwd;
                string name = new LoginForgotPassword().SelectName(username1);
                string notemsg = "";

                notemsg = notemsg + "Hi " + name + ",<br/><br/>";

                notemsg = notemsg + "<b>Forgot your password?</b><br/>";
                notemsg = notemsg + "Quickbite received a request to reset the password for this email address.<br/><br/>";
                notemsg = notemsg + "To reset your password, please Click on the link below:<br/>";
                notemsg = notemsg + AA + "<br/>";
                notemsg = notemsg + "This link takes you to a secure page where you can change your password.<br/>  ";
                notemsg = notemsg + "If you did not request a password reset, then you can safely ignore this email. Your password will not be reset.<br/><br/>";

                notemsg = notemsg + "For general enquiries or request support with your account, please email: support@forest-interactive.com<br/><br/>";

                notemsg = notemsg + "Cheerfully yours,<br/>";
                notemsg = notemsg + "Quickbite Team<br/>";


                string useremail = username1;
                StringBuilder strMessage = new StringBuilder();
                string SendFrom = "alert@forest-interactive.com";
                string SendTo = useremail;
                string newstitle = "Reset Password QuickBite";


                SmtpClient client = new SmtpClient();
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.EnableSsl = true;
                client.Host = "smtp.office365.com";
                client.Port = 587;


                // setup Smtp authentication
                System.Net.NetworkCredential credentials = new System.Net.NetworkCredential("alert@forest-interactive.com", "Server9@9@");
                client.UseDefaultCredentials = true;
                client.Credentials = credentials;

                MailMessage msg = new MailMessage();
                msg.From = new MailAddress(SendFrom);
                msg.To.Add(new MailAddress(SendTo));

                msg.Subject = newstitle;
                msg.IsBodyHtml = true;
                //msg.AlternateViews.Add(view);
                //msg.Body = notemsg;
                msg.Body = notemsg;


                client.Send(msg);

            }
            catch (Exception ex)
            {
                //Tools.write_log(DateTime.Now.ToString() + "," + MethodBase.GetCurrentMethod().Name + "," + ex.Message, "Err");
            }
        }

        #region connection
        public SqlConnection ServerDB = null;
        public static string SqlConnection
        {
            get
            {
                return "Data Source=localhost;Initial Catalog=QuickBite;Persist Security Info=True;User ID=dbuser;Password=celcom2@13";
            }
        }

        public static SqlConnection GetConnection()
        {
            return new SqlConnection(SqlConnection);
        }
        #endregion
        public int SelectDbLogin(string username)
        {
            int returnvalue = 0;
            using (ServerDB = GetConnection())
            {
                if (ServerDB.State == ConnectionState.Closed)
                {

                    try
                    {
                        ServerDB.Open();
                        SqlCommand cmdInsert = new SqlCommand(@"select userid from [QuickBite].[dbo].[Users] where [UserName]=@username", ServerDB);
                        cmdInsert.Parameters.AddWithValue("@username", username);
                        int dd = (int)cmdInsert.ExecuteScalar();
                        return returnvalue = dd;

                    }
                    catch (Exception ex)
                    {
                        return returnvalue = 0;
                    }
                }

                if (ServerDB.State == ConnectionState.Open)
                {
                    ServerDB.Close();
                }

            }
            return returnvalue;
        }

        public string SelectName(string username)
        {
            using (ServerDB = GetConnection())
            {
                if (ServerDB.State == ConnectionState.Closed)
                {

                    try
                    {
                        ServerDB.Open();
                        SqlCommand cmdInsert = new SqlCommand(@"select FullName from [QuickBite].[dbo].[Users] where [UserName]=@username", ServerDB);
                        cmdInsert.Parameters.AddWithValue("@username", username);
                        string ee = (string)cmdInsert.ExecuteScalar();
                        return ee;

                    }
                    catch (Exception ex)
                    {
                        return null;
                    }
                }

                if (ServerDB.State == ConnectionState.Open)
                {
                    ServerDB.Close();
                }

            }
            return null;
        }
    }
}