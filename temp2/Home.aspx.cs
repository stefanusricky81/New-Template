using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
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

    protected void GetData()
    {
        conn.ConnectionDatabase();

        smd = new SqlCommand("GET_HOMEPICTURE", conn.getconn());
        smd.CommandType = CommandType.StoredProcedure;
        sdr = smd.ExecuteReader();
        if (sdr.HasRows)
        {
            dt.Load(sdr);
        }

        conn.CloseConnection();
        conn.Dispose();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        string[] filePaths = Directory.GetFiles(Server.MapPath("~/ImageHome/"));
        List<ListItem> files = new List<ListItem>();
        foreach (string filePath in filePaths)
        {
            string fileName = Path.GetFileName(filePath);
            files.Add(new ListItem(fileName, "ImageHome/" + fileName));
        }
        Repeater1.DataSource = files;
        Repeater1.DataBind();

        //if (!IsPostBack)
        //{
        //    GetData();
        //}
    }
}
