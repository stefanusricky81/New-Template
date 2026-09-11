using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class _Default : System.Web.UI.Page
{
    ClassConnectionDatabase conn = new ClassConnectionDatabase();
    DataTable dt = new DataTable();
    SqlCommand smd = new SqlCommand();
    SqlDataReader sdr = null;
    int vstatus = 0;
    string pstatus = string.Empty;
    string pstatusmsg = string.Empty;
    string VCacheKey = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

            conn.ConnectionDatabase();

            smd = new SqlCommand("GET_DATA_EVENTANDPROMO", conn.getconn());
            smd.CommandType = CommandType.StoredProcedure;
            sdr = smd.ExecuteReader();
            if (sdr.HasRows)
            {
                dt.Load(sdr);
            }

            if (dt.Rows.Count > 0)
            {
                dlProduct.DataSource = dt;
                Cache.Insert(VCacheKey, dlProduct.DataSource, null, DateTime.Now.AddMinutes(60), TimeSpan.Zero);
            }
            dlProduct.DataBind();

            conn.CloseConnection();
            conn.Dispose();
        }

    }
}
