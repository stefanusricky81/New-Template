using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class VerifyEmail : System.Web.UI.Page
{
    ClassConnectionDatabase conn = new ClassConnectionDatabase();
    DataTable dt = new DataTable();
    SqlCommand smd = new SqlCommand();
    SqlDataReader sdr = null;
    int vstatus = 0;
    string pstatus = string.Empty;
    string pstatusmsg = string.Empty;
    string VCacheKey = "";

    protected void UpdateEmail()
    {
        conn.ConnectionDatabase();

        try
        {
            smd = new SqlCommand("VERIFY_EMAIL", conn.getconn());

            smd.Parameters.AddWithValue("@Email", Request["email"].ToString());

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
                Master.Status.Text = "[" + pstatus + "] " + pstatusmsg;
                Master.Status.ForeColor = System.Drawing.Color.Blue;
                Response.Redirect("SignIn.aspx?sts=" + pstatus);
            }
        }
        catch(SqlException se)
        {
            throw se;
        }
        finally
        {
            conn.CloseConnection();
            conn.Dispose();
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            UpdateEmail();
        }
    }
}