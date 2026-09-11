using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Net.Mail;
using System.Net;
using System.Text.RegularExpressions;

public partial class Registration : System.Web.UI.Page
{
    DataTable dt = new DataTable();
    SqlCommand smd = new SqlCommand();
    SqlDataReader sdr = null;
    int vstatus = 0;
    string pstatus = string.Empty;
    string pstatusmsg = string.Empty;

    public static bool emailIsValid(string email)
    {
        string expresion;
        expresion = "\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*";
        if (Regex.IsMatch(email, expresion))
        {
            if (Regex.Replace(email, expresion, string.Empty).Length == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //if (Session["emailsignup"] == null) 
            //{
            //    Response.Redirect("SignIn.aspx");
            //    Session["emailsignup"] = string.Empty;
            //}
            //else
            //    if (Session["emailsignup"] != null && Session["emailsignup"] != string.Empty && Session["emailsignup"] != "")
            //        txtEmail.Text = Session["emailsignup"].ToString();
            //    else
            //        txtEmail.Text = string.Empty;
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("Registration.aspx");
    }

    protected void sendMail()
    {
        string MsgTo = txtEmail.Text;

        //string MailHost = "smtp.gmail.com";  //klo gmail pake "smtp.gmail.com" ; kalo yahoo pake "smtp.yahoo.com"            
        //string MailHost = "smtp.gmail.com";
        //string MailHost = "secure16.win.hostgator.com";
        string MailHost = "little-superstar.com";

        //string MailPort = "587";465
        string MailPort = "25";
        string MailFrom = "no-reply@little-superstar.com";
        string MailUser = "no-reply@little-superstar.com";
        string MailPassword = "Justinsonja123!";
        //string MailSsl = "1";

        SmtpClient theClient = new SmtpClient(MailHost, Convert.ToInt32(MailPort));
        theClient.UseDefaultCredentials = true;
        theClient.Credentials = new NetworkCredential(MailUser, MailPassword);

        //if (MailSsl == "0") { theClient.EnableSsl = false; }
        //else { theClient.EnableSsl = true; }


        MailMessage theMessage = new MailMessage(MailFrom, MsgTo);

        theMessage.Subject = "You're Registration";
        //theMessage.Body = "<a href='little-superstar.com/VerifyEmail.aspx?email=" + txtEmail.Text + "'><u> Please Click Here To Confirm Your's Email </u></a>";
        theMessage.Body = "This Is Youre Email Confirmation ";
        theMessage.Body = theMessage.Body + "<br /><a href = '" + Request.Url.AbsoluteUri.Replace("Registration.aspx", "VerifyEmail.aspx?email=" + txtEmail.Text) + "'><u> Please Click Here To Confirm Your's Email </u></a>";
        //theMessage.Body = theMessage.Body + "<br /><a href = 'localhost:1210/New Lss/VerifyEmail.aspx?email=" + txtEmail.Text + "'><u> Please Click Here To Confirm Your's Email </u></a>";
        theMessage.IsBodyHtml = true;

        theClient.Send(theMessage); 
    }

    protected void btnSignIn_Click(object sender, EventArgs e)
    {
        ClassConnectionDatabase conn = new ClassConnectionDatabase();

        if (!emailIsValid(txtEmail.Text))
        {
            Master.Status.Text= "You're Email Address Is Wrong";
            Master.Status.ForeColor = System.Drawing.Color.Red;
            return;
        }
        conn.ConnectionDatabase();

        smd = new SqlCommand("sp_InsertRegistration", conn.getconn());
        smd.Parameters.AddWithValue("@Name", txtName.Text);
        smd.Parameters.AddWithValue("@Address", txtAddress.Text);
        smd.Parameters.AddWithValue("@Tlp", txtTlp.Text);
        smd.Parameters.AddWithValue("@Email", txtEmail.Text);
        smd.Parameters.AddWithValue("@Pass", txtPass.Text);
        smd.Parameters.AddWithValue("@Kota", txtKota.Text);

        smd.Parameters.Add("@Pstatus", SqlDbType.VarChar, 255);
        smd.Parameters["@Pstatus"].Direction = ParameterDirection.Output;
        smd.Parameters.Add("@PstatusMsg", SqlDbType.VarChar, 255);
        smd.Parameters["@PstatusMsg"].Direction = ParameterDirection.Output;
        smd.CommandType = CommandType.StoredProcedure;
        smd.ExecuteNonQuery();
        pstatus = smd.Parameters["@Pstatus"].Value.ToString();
        pstatusmsg = smd.Parameters["@PstatusMsg"].Value.ToString();

        conn.CloseConnection();
        conn.Dispose();

        vstatus = Int32.Parse(pstatus);
        if (vstatus > 0)
        {
            Master.Status.Text = "[" + pstatus + "] " + pstatusmsg;
            Master.Status.ForeColor = System.Drawing.Color.Red;
            return;
        }
        else
        {
            sendMail();
            Response.Redirect("SIgnIn.aspx");
        }

    }
}