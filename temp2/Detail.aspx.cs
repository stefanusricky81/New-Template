using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class Detail : System.Web.UI.Page
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

            smd = new SqlCommand("Get_Product_Detail", conn.getconn());
            smd.Parameters.AddWithValue("@ProductID", Request.Params["ID"].ToString());

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

    protected string validasi()
    {
        try 
        { 
            int _qty = Convert.ToInt16(txtQty.Text);
        }
        catch 
        { 
            return "Quantity Must Numeric";
        }
        return string.Empty;
    }

    protected void btnAddCart_Click(object sender, EventArgs e)
    {
        if (Session["USER_ID"] == null)
            Response.Redirect("MyAccount.aspx");

        if (validasi() != string.Empty)
        {
            Master.Status.Text = validasi().ToString();
            Master.Status.ForeColor = System.Drawing.Color.Red;
            return;
        }

        conn.ConnectionDatabase();

        smd = new SqlCommand("sp_InsertCart", conn.getconn());
        smd.Parameters.AddWithValue("@Username", Session["USER_ID"].ToString());
        smd.Parameters.AddWithValue("@Productid", Request["ID"]);
        smd.Parameters.AddWithValue("@Qty", txtQty.Text);

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
            Response.Redirect("CCart.aspx");
        }
    }
}