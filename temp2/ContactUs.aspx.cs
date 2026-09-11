using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class contact : System.Web.UI.Page
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

        smd = new SqlCommand("GET_DETAILCONTACTUS", conn.getconn());

        smd.CommandType = CommandType.StoredProcedure;
        sdr = smd.ExecuteReader();
        if (sdr.Read())
        {
            lblOfficeDay.Text = sdr["OFFICE_DAY"].ToString().ToUpper();
            lblOfficeHour.Text = sdr["OFFICE_HOURS"].ToString().ToUpper();
            lblPhone1.Text = sdr["PHONE1"].ToString().ToUpper();
            lblPhone2.Text = sdr["PHONE2"].ToString().ToUpper();
            lblSms.Text = sdr["SMS"].ToString().ToUpper();
            lblEmail.Text = sdr["EMAIL"].ToString().ToUpper();
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