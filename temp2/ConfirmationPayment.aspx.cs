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
using System.IO;

public partial class LssCulture : System.Web.UI.Page
{
    ClassConnectionDatabase conn = new ClassConnectionDatabase();
    DataTable dt = new DataTable();
    SqlCommand smd = new SqlCommand();
    SqlDataReader sdr = null;
    int vstatus = 0;
    string pstatus = string.Empty;
    string pstatusmsg = string.Empty;
    string VCacheKey = "";
    string _CustomerEmail = "";
    string _CustomerId = "";

    protected void Isi_Bank()
    {
        conn.ConnectionDatabase();

        smd = new SqlCommand("GET_DDL_BANK", conn.getconn());
        smd.CommandType = CommandType.StoredProcedure;
        SqlDataAdapter da = new SqlDataAdapter(smd);
        DataSet ds = new DataSet();
        da.Fill(ds);
        ddlBank.DataSource = ds;

        ddlBank.DataTextField = "Bank_Name";
        ddlBank.DataValueField = "ID";
        ddlBank.DataBind();
        ddlBank.Items.Insert(0, "");

        conn.CloseConnection();
        conn.Dispose();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Session["USER_ID"] == null || Session["USER_ID"] == "" || Session["USER_ID"] == string.Empty)
                Response.Redirect("SignIn.aspx");

            if (!IsPostBack)
            {
                Isi_Bank();
            }
        }
    }

    protected void txtOrderID_TextChanged(object sender, EventArgs e)
    {
        conn.ConnectionDatabase();

        smd = new SqlCommand("GET_DETAIL_ORDER", conn.getconn());
        smd.Parameters.AddWithValue("@NOORDER", txtOrderID.Text);

        smd.CommandType = CommandType.StoredProcedure;
        sdr = smd.ExecuteReader();

        if (sdr.Read())
        {
            txtName.Text = sdr["NAMA"].ToString();
            txtAmount.Text = Convert.ToDouble(sdr["TOTAL_PRICE"].ToString()).ToString("#,##0");
            _CustomerEmail = sdr["USER_ID"].ToString();
            _CustomerId = sdr["ID"].ToString();
        }

        conn.CloseConnection();
        conn.Dispose();
    }

    protected void sendMailToAdmin()
    {
        string MsgTo = "littlesuperstarfashion@gmail.com";

        string MailHost = "little-superstar.com";

        string MailPort = "25";
        string MailFrom = "no-reply@little-superstar.com";
        string MailUser = "no-reply@little-superstar.com";
        string MailPassword = "Justinsonja123!";

        SmtpClient theClient = new SmtpClient(MailHost, Convert.ToInt32(MailPort));
        theClient.UseDefaultCredentials = true;
        theClient.Credentials = new NetworkCredential(MailUser, MailPassword);

        MailMessage theMessage = new MailMessage(MailFrom, MsgTo);

        theMessage.Subject = "Notification Customer Payment";
        theMessage.Body = "Customer Id : " + _CustomerId + "<br />" + " Customer Name : " + txtName.Text + "<br />" + " Customer Email : " + _CustomerEmail + "<br />";
        theMessage.Body += "Already paid for Order Number :" + txtOrderID.Text + "<br />";
        theMessage.Body += "Please check and send the item(s).";
        theMessage.IsBodyHtml = true;

        theClient.Send(theMessage);
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        string filename = Path.GetFileName(fileuploadimages.PostedFile.FileName);

        if (fileuploadimages.HasFile != true)
        {
            lblStatus.Text = "Please Provide Your Receipt of Payment";
            lblStatus.ForeColor = System.Drawing.Color.Red;
            return;
        }
        else
        {
            fileuploadimages.SaveAs(Server.MapPath("ImgPayment/" + filename));
        }

        conn.ConnectionDatabase();

        smd = new SqlCommand("UpdatePayment", conn.getconn());
        smd.Parameters.AddWithValue("@UserID", Session["USER_ID"].ToString());
        smd.Parameters.AddWithValue("@NoOrder", txtOrderID.Text);
        smd.Parameters.AddWithValue("@AccCustName", txtAccountName.Text);
        smd.Parameters.AddWithValue("@AccCustNo", txtAccountNumber.Text);
        smd.Parameters.AddWithValue("@Message", txtMessage.Text);
        smd.Parameters.AddWithValue("@Image", "ImgPayment/" + filename);

        smd.Parameters.Add("@Pstatus", SqlDbType.VarChar, 255);
        smd.Parameters["@Pstatus"].Direction = ParameterDirection.Output;
        smd.Parameters.Add("@PstatusMsg", SqlDbType.VarChar, 255);
        smd.Parameters["@PstatusMsg"].Direction = ParameterDirection.Output;

        smd.CommandType = CommandType.StoredProcedure;
        smd.ExecuteNonQuery();
        pstatus = smd.Parameters["@Pstatus"].Value.ToString();
        pstatusmsg = smd.Parameters["@PstatusMsg"].Value.ToString();

        vstatus = Int32.Parse(pstatus);
        if (vstatus > 0)
        {
            lblStatus.Text = "[" + pstatus + "] " + pstatusmsg;
            lblStatus.ForeColor = System.Drawing.Color.Red;
            return;
        }
        else
        {
            sendMailToAdmin();

            Response.Redirect("About.aspx");
        }

        conn.CloseConnection();
        conn.Dispose();
    }
}