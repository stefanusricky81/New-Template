using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class OrderDetail : System.Web.UI.Page
{
    ClassConnectionDatabase conn = new ClassConnectionDatabase();
    DataTable dt = new DataTable();
    SqlCommand smd = new SqlCommand();
    SqlDataReader sdr = null;
    int vstatus = 0;
    string pstatus = string.Empty;
    string pstatusmsg = string.Empty;
    string VCacheKeyOrderDetail = "";

    protected void GetdataOrder()
    {
        conn.ConnectionDatabase();

        smd = new SqlCommand("GET_DETAIL_ORDER_PRODUCT", conn.getconn());
        smd.Parameters.AddWithValue("@NOORDER", Request.Params["NoOrder"].ToString());

        smd.CommandType = CommandType.StoredProcedure;
        sdr = smd.ExecuteReader();
        if (sdr.HasRows)
        {
            dt.Load(sdr);
        }

        if (dt.Rows.Count > 0)
        {
            GV.DataSource = dt;
            Cache.Insert(VCacheKeyOrderDetail, GV.DataSource, null, DateTime.Now.AddMinutes(60), TimeSpan.Zero);
        }
        
        GV.DataBind();

        conn.CloseConnection();
        conn.Dispose();
    }

    protected void GetDataCustOrder()
    {
        conn.ConnectionDatabase();

        smd = new SqlCommand("GET_DETAIL_ORDER_NO", conn.getconn());
        smd.Parameters.AddWithValue("@NOORDER", Request.Params["NoOrder"].ToString());

        smd.CommandType = CommandType.StoredProcedure;
        sdr = smd.ExecuteReader();
        if (sdr.Read())
        {
            txtName.Text = sdr["Nama"].ToString();
            txtAddress.Text = sdr["Alamat"].ToString();
            txtCity.Text = sdr["Kota"].ToString();
            txtTlp.Text = sdr["TELEPHONE"].ToString();
            txtNoOrder.Text = sdr["NO_ORDER"].ToString();
            txtTotal.Text = Convert.ToDouble(sdr["TOTAL_PRICE"].ToString()).ToString("#,##0");
            hfFlagPaid.Value = sdr["Flag_Paid"].ToString();
            hfFlagSend.Value = sdr["Flag_Send"].ToString();
        }

        conn.CloseConnection();
        conn.Dispose();

    }

    protected void Page_Load(object sender, EventArgs e)
    {
        GetdataOrder();
        GetDataCustOrder();

        if (!IsPostBack)
        {
            if (Session["USER_ID"] != null)
            {
                if (Session["USER_ID"].ToString().ToUpper() != "ADMIN")
                    Response.Redirect("Home.aspx");
            }
            else
                Response.Redirect("Home.aspx");

            if (hfFlagPaid.Value == "N")
            {
                btnSend.Visible = false;
            }
            else
            {
                if(hfFlagSend.Value=="N")
                    btnSend.Visible = true;
                else
                    btnSend.Visible = false;
            }
        }
    }

    protected void myGridView_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GV.PageIndex = e.NewPageIndex;
        GV.DataSource = Cache[VCacheKeyOrderDetail];
        GV.DataBind();
    }

    protected void btnSend_Click(object sender, EventArgs e)
    {
        conn.ConnectionDatabase();
        
        smd = new SqlCommand("UPDATE_FLAG_SEND", conn.getconn());
        smd.Parameters.AddWithValue("@NOORDER", txtNoOrder.Text);

        smd.Parameters.Add("@Pstatus", SqlDbType.VarChar, 255);
        smd.Parameters["@Pstatus"].Direction = ParameterDirection.Output;
        smd.Parameters.Add("@PstatusMsg", SqlDbType.VarChar, 255);
        smd.Parameters["@PstatusMsg"].Direction = ParameterDirection.Output;
        smd.CommandType = CommandType.StoredProcedure;
        smd.ExecuteNonQuery();

        pstatus = smd.Parameters["@Pstatus"].Value.ToString();
        pstatusmsg = smd.Parameters["@PstatusMsg"].Value.ToString();

        vstatus = Int32.Parse(pstatus);

        if (vstatus > 0)
        {
            Master.Status.Text = "[" + pstatus + "] " + pstatusmsg;
            Master.Status.ForeColor = System.Drawing.Color.Red;
            return;
        }
        else
        {
            Response.Redirect("OrderHeader.aspx");
        }

        conn.CloseConnection();
        conn.Dispose();
    }
}