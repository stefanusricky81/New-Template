using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class LssCulture : System.Web.UI.Page
{
    ClassConnectionDatabase conn = new ClassConnectionDatabase();
    DataTable dt = new DataTable();
    SqlCommand smd = new SqlCommand();
    SqlDataReader sdr = null;
    SqlDataAdapter dadapter = null;
    DataSet dset = null;
    PagedDataSource adsource;

    int vstatus = 0;
    string pstatus = string.Empty;
    string pstatusmsg = string.Empty;
    string VCacheKey = "";
    int pos;

    protected void Get_Data()
    {
        string _search = string.Empty;

        if (Request["f"] == null || Request["f"].ToString() == string.Empty)
            _search = "OO";
        else
            _search = Request["f"].ToString();

        conn.ConnectionDatabase();

        dadapter = new SqlDataAdapter("Get_Product_Couture", conn.getconn());
        dadapter.SelectCommand.CommandType = CommandType.StoredProcedure;
        dadapter.SelectCommand.Parameters.Add("@SEARCH", SqlDbType.VarChar).Value = _search;

        dset = new DataSet();
        adsource = new PagedDataSource();
        dadapter.Fill(dset);
        adsource.DataSource = dset.Tables[0].DefaultView;
        adsource.PageSize = 6;
        adsource.AllowPaging = true;
        adsource.CurrentPageIndex = pos;
        btnfirst.Enabled = !adsource.IsFirstPage;
        btnprevious.Enabled = !adsource.IsFirstPage;
        btnlast.Enabled = !adsource.IsLastPage;
        btnnext.Enabled = !adsource.IsLastPage;
        dlProduct.DataSource = adsource;
        dlProduct.DataBind();

        conn.CloseConnection();
        conn.Dispose();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            this.ViewState["vs"] = 0;
        }
        
        Get_Data();
    }

    protected void btnfirst_Click(object sender, EventArgs e)
    {
        pos = 1;
        this.ViewState["vs"] = pos;
        Get_Data();
    }

    protected void btnprevious_Click(object sender, EventArgs e)
    {
        pos = (int)this.ViewState["vs"];
        pos -= 1;
        this.ViewState["vs"] = pos;
        Get_Data();
    }

    protected void btnnext_Click(object sender, EventArgs e)
    {
        pos = (int)this.ViewState["vs"];
        pos += 1;
        this.ViewState["vs"] = pos;
        Get_Data();
    }

    protected void btnlast_Click(object sender, EventArgs e)
    {
        pos = adsource.PageCount - 1;
        this.ViewState["vs"] = pos;
        Get_Data();
    }

    public string ProcessMyDataItem(object myValue)
    {
        if (Convert.ToInt32(myValue) == 1)
        {
            return "New";
        }
        else if (Convert.ToInt32(myValue) == 2)
        {
            return "Out Of Stock";
        }
        else
        {
            return "Sale";
        }
    }
    //protected void Cart_Click(object sender, EventArgs e)
    //{
    //    //Button btn;

    //    conn.ConnectionDatabase();
    //    //foreach (DataListItem dl in dlProduct.Items)
    //    //{
    //    //    //btn = (Button)dl.FindControl("btnCart");
    //    //    //if (btn != null && btn.Click)
    //    //    //{ 
    //    //    //}
    //    //    string lbltest = (dl.FindControl("lblID") as Label).Text;

    //    //}
        
    //    smd = new SqlCommand("sp_InsertCart", conn.getconn());
    //    smd.Parameters.AddWithValue("@Username", Session["USER_ID"].ToString());
    //    smd.Parameters.AddWithValue("@Productid", Request["ID"]);
    //    smd.Parameters.AddWithValue("@Qty", ViewState["textQty"]);

    //    smd.Parameters.Add("@Pstatus", SqlDbType.VarChar, 255);
    //    smd.Parameters["@Pstatus"].Direction = ParameterDirection.Output;
    //    smd.Parameters.Add("@PstatusMsg", SqlDbType.VarChar, 255);
    //    smd.Parameters["@PstatusMsg"].Direction = ParameterDirection.Output;
    //    smd.CommandType = CommandType.StoredProcedure;
    //    smd.ExecuteNonQuery();
    //    pstatus = smd.Parameters["@Pstatus"].Value.ToString();
    //    pstatusmsg = smd.Parameters["@PstatusMsg"].Value.ToString();

    //    conn.CloseConnection();
    //    conn.Dispose();

    //    vstatus = Int32.Parse(pstatus);
    //    if (vstatus > 0)
    //    {
    //        //tStatus.Text = "[" + pstatus + "] " + pstatusmsg;
    //    }
    //    else
    //    {
    //        Response.Redirect("CCart.aspx");
    //    }
    //}
}