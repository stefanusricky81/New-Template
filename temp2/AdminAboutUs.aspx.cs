using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class AdminAboutUs : System.Web.UI.Page
{
    ClassConnectionDatabase conn = new ClassConnectionDatabase();
    DataTable dt = new DataTable();
    SqlCommand smd = new SqlCommand();
    SqlDataReader sdr = null;
    int vstatus = 0;
    string pstatus = string.Empty;
    string pstatusmsg = string.Empty;
    string VCacheKey = "";

    protected void EmptyField()
    {
        txtLine1.Text = string.Empty;
        txtLine2.Text = string.Empty;
        txtLine3.Text = string.Empty;
        txtLine4.Text = string.Empty;
    }

    protected void CekSession()
    {
        if (Session["USER_ID"] != null)
        {
            if (Session["USER_ID"].ToString().ToUpper() != "ADMIN")
                Response.Redirect("Home.aspx");
        }
        else
            Response.Redirect("Home.aspx");
    }

    protected void loaddata()
    {
        conn.ConnectionDatabase();

        smd = new SqlCommand("GET_DETAILABOUTUS", conn.getconn());
        //smd.Parameters.AddWithValue("@ID", Request.Params["ID"].ToString());

        smd.CommandType = CommandType.StoredProcedure;
        sdr = smd.ExecuteReader();
        if (sdr.Read())
        {
            txtLine1.Text = sdr["LINE1"].ToString();
            txtLine2.Text = sdr["LINE2"].ToString();
            txtLine3.Text = sdr["LINE3"].ToString();
            txtLine4.Text = sdr["LINE4"].ToString();
        }

        conn.CloseConnection();
        conn.Dispose();
    }

    protected void Get_Data()
    {
        conn.ConnectionDatabase();

        smd = new SqlCommand("GET_ABOUTUS", conn.getconn());
        smd.CommandType = CommandType.StoredProcedure;
        sdr = smd.ExecuteReader();
        if (sdr.HasRows)
        {
            dt.Load(sdr);
        }

        if (dt.Rows.Count > 0)
        {
            GV.DataSource = dt;
            Cache.Insert(VCacheKey, GV.DataSource, null, DateTime.Now.AddMinutes(60), TimeSpan.Zero);
        }
        GV.DataBind();

        conn.CloseConnection();
        conn.Dispose();
    }

    protected void myGridView_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GV.PageIndex = e.NewPageIndex;
        GV.DataSource = Cache[VCacheKey];
        GV.DataBind();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        CekSession();

        Get_Data();

        if (!IsPostBack)
        {
            if (Request.Params["type"] == "edit")
            {
                loaddata();
            }
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        CekSession();
        Response.Redirect("AdminAboutUs.aspx");
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        string CekValidation = string.Empty;
        
        CekSession();

        conn.ConnectionDatabase();

        if (Request.Params["type"] == "edit")
        {
            smd = new SqlCommand("UPDATE_ABOUTUS", conn.getconn());
            smd.Parameters.AddWithValue("@Id", Request.Params["ID"]);
        }
        else
        {
            smd = new SqlCommand("SP_INSERTABOUTUS", conn.getconn());
        }

        smd.Parameters.AddWithValue("@Line1", txtLine1.Text);
        smd.Parameters.AddWithValue("@Line2", txtLine2.Text);
        smd.Parameters.AddWithValue("@Line3", txtLine3.Text);
        smd.Parameters.AddWithValue("@Line4", txtLine4.Text);

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

            EmptyField();
        }

        Response.Redirect("AdminAboutUs.aspx");
    }
}