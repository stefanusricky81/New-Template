using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class Login : System.Web.UI.Page
{
    int vstatus = 0;
    string pstatus = string.Empty;
    string pstatusmsg = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Session["USER_ID"] = string.Empty;

            txtEmail.Attributes.Add("onKeyPress", "doClick('" + btnSignIn.ClientID + "',event)");
            txtPass.Attributes.Add("onKeyPress", "doClick('" + btnSignIn.ClientID + "',event)");
        }
    }

    protected bool validation()
    {
        if (txtEmail.Text.Trim() == string.Empty)
        {
            tStatus.Text = "Username must be filled";
            return false;
        }
        if (txtPass.Text.Trim() == string.Empty)
        {
            tStatus.Text = "Password must be filled";
            return false;
        }
        return true;
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("About.aspx");
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
        smd.CommandType = CommandType.StoredProcedure;
        smd.ExecuteNonQuery();

        pstatus = smd.Parameters["@Pstatus"].Value.ToString();
        pstatusmsg = smd.Parameters["@PstatusMsg"].Value.ToString();

        conn.CloseConnection();
        conn.Dispose();
        vstatus = Int32.Parse(pstatus);
        if (vstatus > 0)
        {
            tStatus.Text = "[" + pstatus + "] " + pstatusmsg;
        }
        else
        {
            Session["USER_ID"] = txtEmail.Text;
            Response.Redirect("About.aspx");
        }
    }
}