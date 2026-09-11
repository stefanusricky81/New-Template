using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class MyAccount : System.Web.UI.Page
{
    ClassConnectionDatabase conn = new ClassConnectionDatabase();
    DataTable dt = new DataTable();
    SqlCommand smd = new SqlCommand();
    SqlDataReader sdr = null;
    int vstatus = 0;
    string pstatus = string.Empty;
    string pstatusmsg = string.Empty;
    string VCacheKey = "";

    protected void GetAccount()
    {
        conn.ConnectionDatabase();

        smd = new SqlCommand("GET_ORDERHEADER", conn.getconn());
        smd.Parameters.AddWithValue("@UserID", Session["USER_ID"].ToString());
        smd.CommandType = CommandType.StoredProcedure;
        sdr = smd.ExecuteReader();
        if (sdr.HasRows)
        {
            dt.Load(sdr);
        }

        if (dt.Rows.Count > 0)
        {
            gvAccount.DataSource = dt;
            Cache.Insert(VCacheKey, gvAccount.DataSource, null, DateTime.Now.AddMinutes(60), TimeSpan.Zero);
        }
        gvAccount.DataBind();

        conn.CloseConnection();
        conn.Dispose();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["USER_ID"] == null || Session["USER_ID"].ToString() == "" || Session["USER_ID"] == string.Empty)
            Response.Redirect("Login.aspx");

        if (!IsPostBack)
        {
            GetAccount();
        }
    }
}