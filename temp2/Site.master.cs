using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class SiteMaster : System.Web.UI.MasterPage
{
    ClassConnectionDatabase conn = new ClassConnectionDatabase();
    DataTable dt = new DataTable();
    SqlCommand smd = new SqlCommand();
    SqlDataReader sdr = null;
    int vstatus = 0;
    string pstatus = string.Empty;
    string pstatusmsg = string.Empty;
    string VCacheKey = "";

    protected void GetTotalCart()
    {
        conn.ConnectionDatabase();

        smd = new SqlCommand("GetTotalCart", conn.getconn());
        smd.Parameters.AddWithValue("@UserID", Session["USER_ID"].ToString());

        smd.CommandType = CommandType.StoredProcedure;
        sdr = smd.ExecuteReader();
        if (sdr.Read())
        {
            lblItemCart.Text =sdr["QTY"].ToString();
            lblPrice.Text = sdr["TOTAL"].ToString();
        }

        conn.CloseConnection();
        conn.Dispose();
    }
   
    public Label User
    {
        get { return lblUserId; }
        set { lblUserId = value; }
    }
    
    public Label Status
    {
        get { return tStatus; }
        set { tStatus = value; }
    }

    public Label ItemCart
    {
        get { return lblItemCart; }
        set { lblItemCart = value; }
    }

    public Label Price
    {
        get { return lblPrice; }
        set { lblPrice = value; }
    }

    protected void cekAdmin()
    {
        if (Session["USER_ID"].ToString().ToUpper() == "ADMIN")
            pnlAdmin.Visible = true;
        else
            pnlAdmin.Visible = false;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["USER_ID"] != null && Session["USER_ID"] != string.Empty && Session["USER_ID"] != "")
        {
            lblUserId.Text = Session["USER_NAME"].ToString();
            lblSign.Text = "LOG OUT";

            cekAdmin();
        }
        else
        {
            lblUserId.Text = "Guest";
        }
        if (!IsPostBack)
        {
            if (Session["USER_ID"] != null)
                GetTotalCart(); 

            //if (Session["Cart_Quantity"]!=null)
            //    lblItemCart.Text = Session["Cart_Quantity"].ToString();

            //if (Session["Cart_Price"]!=null)
            //    lblPrice.Text =Session["Cart_Price"].ToString();
        }
    }
}
