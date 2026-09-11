using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class CCart : System.Web.UI.Page
{
    ClassConnectionDatabase conn = new ClassConnectionDatabase();
    DataTable dt = new DataTable();
    SqlCommand smd = new SqlCommand();
    SqlDataReader sdr = null;
    int vstatus = 0;
    string pstatus = string.Empty;
    string pstatusmsg = string.Empty;
    string VCacheKey = "";

    protected void GetCart()
    {
        conn.ConnectionDatabase();

        smd = new SqlCommand("Get_Cart", conn.getconn());
        smd.Parameters.AddWithValue("@UserID", Session["USER_ID"].ToString());
        smd.CommandType = CommandType.StoredProcedure;
        sdr = smd.ExecuteReader();
        if (sdr.HasRows)
        {
            dt.Load(sdr);
        }

        if (dt.Rows.Count > 0)
        {
            gvCart.DataSource = dt;
            Cache.Insert(VCacheKey, gvCart.DataSource, null, DateTime.Now.AddMinutes(60), TimeSpan.Zero);
        }
        gvCart.DataBind();

        conn.CloseConnection();
        conn.Dispose();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Session["USER_ID"] == null || Session["USER_ID"].ToString() == string.Empty)
                Response.Redirect("SignIn.aspx");
            GetCart();
        }
    
    }

    protected void GV_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        double _totalQty = 0;
        double _totalPrice = 0;
        double _TotalPayment = 0;

        foreach (GridViewRow gv in gvCart.Rows)
        {
            _TotalPayment = Convert.ToDouble(gv.Cells[3].Text) * Convert.ToDouble(gv.Cells[4].Text);
            gv.Cells[6].Text = Convert.ToDouble(_TotalPayment.ToString()).ToString("#,##0");

            _totalQty = _totalQty + System.Convert.ToDouble(gv.Cells[4].Text);
            _totalPrice = _totalPrice + System.Convert.ToDouble(gv.Cells[6].Text);
        }

        txtQuantity.Text = Convert.ToDouble(_totalPrice.ToString()).ToString("#,##0");
        txtPrice.Text = Convert.ToDouble(_totalPrice.ToString()).ToString("#,##0");

        Session["Cart_Quantity"] = txtQuantity.Text;
        Session["Cart_Price"] = txtPrice.Text;
    }

    protected void gvPurchasing_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        conn.ConnectionDatabase();
        //GV.DataKeys[e.RowIndex].Value.ToString();
        int idVal = (int)gvCart.DataKeys[e.RowIndex].Value;

        smd = new SqlCommand("DELETE_ITEMCART", conn.getconn());
        smd.Parameters.AddWithValue("@ID", idVal);

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
            //tStatus.Text = "[" + pstatus + "] " + pstatusmsg;
            return;
        }
        else
        {
            Response.Redirect("CCart.aspx");
        }

        conn.CloseConnection();
        conn.Dispose();

        //string _id = gvCart.Rows[e.RowIndex]
        //smd = new SqlCommand("Get_Cart", conn.getconn());
        //smd.Parameters.AddWithValue("@ID", Session["USER_ID"].ToString());
        //smd.CommandType = CommandType.StoredProcedure;
        //sdr = smd.ExecuteReader();
        //if (sdr.HasRows)
        //{
        //    dt.Load(sdr);
        //}

        //if (dt.Rows.Count > 0)
        //{
        //    gvCart.DataSource = dt;
        //    Cache.Insert(VCacheKey, gvCart.DataSource, null, DateTime.Now.AddMinutes(60), TimeSpan.Zero);
        //}
        //gvCart.DataBind();



        //SetRowData();
        //if (ViewState["CurrentTable"] != null)
        //{
        //    DataTable dt = (DataTable)ViewState["CurrentTable"];
        //    DataRow drCurrentRow = null;
        //    int rowIndex = Convert.ToInt32(e.RowIndex);
        //    if (dt.Rows.Count > 1)
        //    {
        //        dt.Rows.Remove(dt.Rows[rowIndex]);
        //        drCurrentRow = dt.NewRow();
        //        ViewState["CurrentTable"] = dt;
        //        gvPurchasing.DataSource = dt;
        //        gvPurchasing.DataBind();

        //        for (int i = 0; i < gvPurchasing.Rows.Count - 1; i++)
        //        {
        //            gvPurchasing.Rows[i].Cells[0].Text = Convert.ToString(i + 1);
        //        }
        //        SetPreviousData();
        //        CalculateTotal();
        //    }
        //    else
        //    {
        //        Response.Write("Sorry Can't Delete Last Record");
        //    }
        //}
    }
    
    protected void btnCheckOut_Click(object sender, EventArgs e)
    {
        conn.ConnectionDatabase();

        smd = new SqlCommand("InsertOrder", conn.getconn());
        smd.Parameters.AddWithValue("@UserID", Session["USER_ID"].ToString());

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
            //Master.Status.Text = "[" + pstatus + "] " + pstatusmsg;
            //Master.Status.ForeColor = System.Drawing.Color.Red;
            return;
            //tStatus.Text = "[" + pstatus + "] " + pstatusmsg;
        }
        else
        {
            Session["Cart_Quantity"] = null;
            Session["Cart_Price"] = null;
            Response.Redirect("About.aspx");
        }

        conn.CloseConnection();
        conn.Dispose();
    }

    protected void btnNext_Click(object sender, EventArgs e)
    {
        Response.Redirect("VerifyOrder.aspx");
    }

    protected void btnShopping_Click(object sender, EventArgs e)
    {
        Response.Redirect("Home.aspx");
    }
}