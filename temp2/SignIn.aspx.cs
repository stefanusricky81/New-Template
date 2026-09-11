using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class SignIn : System.Web.UI.Page
{
    int vstatus = 0;
    string pstatus = string.Empty;
    string pstatusmsg = string.Empty;
    string _username = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if(Session["USER_ID"]!= null || Session["USER_ID"]!=string.Empty || Session["USER_ID"] != "")
                Session["USER_ID"] = string.Empty;

            if (Session["emailsignup"] != null || Session["emailsignup"] != string.Empty || Session["emailsignup"] != "")
                Session["emailsignup"] = string.Empty;

            if (Request["sts"] != null)
                Master.Status.Text = "Youre Verify Email Have Been Success";

            txtEmail.Attributes.Add("onKeyPress", "doClick('" + btnSignIn.ClientID + "',event)");
            txtPass.Attributes.Add("onKeyPress", "doClick('" + btnSignIn.ClientID + "',event)");
        }
    }

    protected bool validation()
    {
        if (txtEmail.Text.Trim() == string.Empty)
        {
            Master.Status.Text = "Username must be filled";
            Master.Status.ForeColor = System.Drawing.Color.Red;
            return false;
        }
        if (txtPass.Text.Trim() == string.Empty)
        {
            Master.Status.Text = "Password must be filled";
            Master.Status.ForeColor = System.Drawing.Color.Red;
            return false;
        }
        return true;
    }

    protected void btnSignIn_Click(object sender, EventArgs e)
    {
        ClassConnectionDatabase conn = new ClassConnectionDatabase();
        DataTable dt = new DataTable();
        SqlCommand smd = new SqlCommand();

        if (!validation())
        {
            return;
        }

        conn.ConnectionDatabase();

        smd = new SqlCommand("sp_checkLogin", conn.getconn());
        smd.Parameters.AddWithValue("@Email", txtEmail.Text);
        smd.Parameters.AddWithValue("@Password", txtPass.Text);

        smd.Parameters.Add("@Pstatus", SqlDbType.VarChar, 255);
        smd.Parameters["@Pstatus"].Direction = ParameterDirection.Output;
        smd.Parameters.Add("@PstatusMsg", SqlDbType.VarChar, 255);
        smd.Parameters["@PstatusMsg"].Direction = ParameterDirection.Output;
        smd.Parameters.Add("@UserName", SqlDbType.VarChar, 255);
        smd.Parameters["@UserName"].Direction = ParameterDirection.Output;
        smd.CommandType = CommandType.StoredProcedure;
        smd.ExecuteNonQuery();

        pstatus = smd.Parameters["@Pstatus"].Value.ToString();
        pstatusmsg = smd.Parameters["@PstatusMsg"].Value.ToString();
        _username = smd.Parameters["@UserName"].Value.ToString();

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
            Session["USER_ID"] = txtEmail.Text;
            Session["USER_NAME"] = _username.ToString();
            Response.Redirect("About.aspx");
        }
    }

    protected void btnCreate_Click(object sender, EventArgs e)
    {
        Session["emailsignup"] = txtEmailNew.Text;
        Response.Redirect("Registration.aspx");
    }
}