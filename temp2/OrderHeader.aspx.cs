using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Data;
using System.Data.SqlClient;

public partial class OrderHeader : System.Web.UI.Page
{
    ClassConnectionDatabase conn = new ClassConnectionDatabase();
    DataTable dt = new DataTable();
    SqlCommand smd = new SqlCommand();
    SqlDataReader sdr = null;
    int vstatus = 0;
    string pstatus = string.Empty;
    string pstatusmsg = string.Empty;
    string VCacheKeyOrderHeader = "";

    protected void GV_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (e.Row.Cells[12].Text == "Y")
                e.Row.Cells[12].Text = "Paid";
            else
                e.Row.Cells[12].Text = "Not Paid";

            if (e.Row.Cells[14].Text == "Y")
                e.Row.Cells[14].Text = "Send";
            else
                e.Row.Cells[14].Text = "Not Send";
        }
    }

    protected void myGridView_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GV.PageIndex = e.NewPageIndex;
        GV.DataSource = Cache[VCacheKeyOrderHeader];
        GV.DataBind();
    }

    protected void Getdata()
    {
        conn.ConnectionDatabase();

        smd = new SqlCommand("GET_ORDER_HEADER", conn.getconn());
        smd.CommandType = CommandType.StoredProcedure;
        sdr = smd.ExecuteReader();
        if (sdr.HasRows)
        {
            dt.Load(sdr);
        }

        if (dt.Rows.Count > 0)
        {
            //bDelete.Visible = true;
            GV.DataSource = dt;
            Cache.Insert(VCacheKeyOrderHeader, GV.DataSource, null, DateTime.Now.AddMinutes(60), TimeSpan.Zero);
        }
        else if (dt.Rows.Count == 0)
        {
           // bDelete.Visible = false;
        }
        GV.DataBind();

        conn.CloseConnection();
        conn.Dispose();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["USER_ID"] != null)
        {
            if (Session["USER_ID"].ToString().ToUpper() != "ADMIN")
                Response.Redirect("Home.aspx");
        }
        else
            Response.Redirect("Home.aspx");

        Getdata();

        //if (!IsPostBack)
        //{
        //    Getdata();
        //}
    }
}