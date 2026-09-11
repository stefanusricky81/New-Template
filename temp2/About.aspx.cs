using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class About : System.Web.UI.Page
{
    ClassConnectionDatabase conn = new ClassConnectionDatabase();
    DataTable dt = new DataTable();
    SqlCommand smd = new SqlCommand();
    SqlDataReader sdr = null;
    int vstatus = 0;
    string pstatus = string.Empty;
    string pstatusmsg = string.Empty;
    string VCacheKey = "";

    protected void loaddata()
    {
        conn.ConnectionDatabase();

        smd = new SqlCommand("GET_DETAILABOUTUS", conn.getconn());

        smd.CommandType = CommandType.StoredProcedure;
        sdr = smd.ExecuteReader();
        if (sdr.Read())
        {
            lblLine1.Text = sdr["Line1"].ToString().ToUpper();
            lblLine2.Text = sdr["Line2"].ToString().ToUpper();
            lblLine3.Text = sdr["Line3"].ToString().ToUpper();
            lblLine4.Text = sdr["Line4"].ToString().ToUpper();
        }

        conn.CloseConnection();
        conn.Dispose();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            loaddata();
        }
    }
}
