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
    string VCacheKeyLSSDetail = "";
    
    protected void EmptyField()
    {
        ddlColor.SelectedIndex = -1;
        ddlSize.SelectedIndex = -1;
        txtQty.Text = string.Empty;
    }

    protected string Validation()
    {
        if (ddlColor.SelectedValue == "---CHOSE COLOR---") return "Choose Color First";
        if (ddlSize.SelectedValue == "---CHOSE SIZE---") return "Choose Size First";

        return string.Empty;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            conn.ConnectionDatabase();

            smd = new SqlCommand("Get_Product_Detail", conn.getconn());
            smd.Parameters.AddWithValue("@ProductID", Request.Params["ID"].ToString());

            smd.CommandType = CommandType.StoredProcedure;
            sdr = smd.ExecuteReader();
            if (sdr.Read())
            {
                ImgProduct.ImageUrl = sdr["IMAGE"].ToString();
                lblBrand.Text = sdr["PRODUCTNAME"].ToString();
                lblPrice.Text = Convert.ToDouble(sdr["PRICE"].ToString()).ToString("#,##0");
                lblDesc.Text = sdr["Descrip"].ToString().Replace("<\br>", "\r\n");
                lblMaterial.Text = sdr["Material"].ToString();
                lblCare.Text = sdr["Care"].ToString();
                //lblSize.Text = sdr["SIZE_DESC"].ToString();
            }

            conn.CloseConnection();
            conn.Dispose();
        }

        if (!IsPostBack)
        {
            Isi_Size();
            Isi_Color();
            GetPictureDetail();
            if (Request["OOS"] != null)
            {
                if (Request["OOS"] == "Y")
                    btnAddToBag.Enabled = false;
                else
                    btnAddToBag.Enabled = true;
            }
        }
    }

    protected void GetPictureDetail()
    {
        conn.ConnectionDatabase();

        smd = new SqlCommand("Get_Detail_Picture", conn.getconn());
        smd.Parameters.AddWithValue("@ID", Request.Params["ID"].ToString());

        smd.CommandType = CommandType.StoredProcedure;
        sdr = smd.ExecuteReader();
        if (sdr.HasRows)
        {
            dt.Load(sdr);
        }

        if (dt.Rows.Count > 0)
        {
            dlDetailProductPic.DataSource = dt;
            Cache.Insert(VCacheKeyLSSDetail, dlDetailProductPic.DataSource, null, DateTime.Now.AddMinutes(60), TimeSpan.Zero);
        }
        dlDetailProductPic.DataBind();

        conn.CloseConnection();
        conn.Dispose();
    }

    protected void Isi_Size()
    {
        conn.ConnectionDatabase();

        smd = new SqlCommand("Get_Product_Detail_DdlSize", conn.getconn());
        smd.Parameters.AddWithValue("@ProductID", Request.Params["ID"].ToString());

        smd.CommandType = CommandType.StoredProcedure;
        SqlDataAdapter da = new SqlDataAdapter(smd);
        DataSet ds = new DataSet();
        da.Fill(ds);
        ddlSize.DataSource = ds;

        ddlSize.DataTextField = "Size";
        ddlSize.DataValueField = "Size_Id";
        ddlSize.DataBind();
        ddlSize.Items.Insert(0, "---CHOSE SIZE---");

        conn.CloseConnection();
        conn.Dispose();
    }

    protected void Isi_Color()
    {
        conn.ConnectionDatabase();

        smd = new SqlCommand("Get_Product_Detail_DdlColor", conn.getconn());
        smd.Parameters.AddWithValue("@ProductID", Request.Params["ID"].ToString());

        smd.CommandType = CommandType.StoredProcedure;
        SqlDataAdapter da = new SqlDataAdapter(smd);
        DataSet ds = new DataSet();
        da.Fill(ds);
        ddlColor.DataSource = ds;

        ddlColor.DataTextField = "Color";
        ddlColor.DataValueField = "Color_Id";
        ddlColor.DataBind();
        ddlColor.Items.Insert(0, "---CHOSE COLOR---");

        conn.CloseConnection();
        conn.Dispose();
    }

    protected void btnAddToBag_Click(object sender, EventArgs e)
    {
        if (Session["USER_ID"] == null || Session["USER_ID"].ToString() == "" || Session["USER_ID"] == string.Empty)
            Response.Redirect("SignIn.aspx");

        string CekValidation = string.Empty;

        conn.ConnectionDatabase();

        CekValidation = Validation();

        if (CekValidation == string.Empty)
        {
            smd = new SqlCommand("SP_INSERTCART", conn.getconn());

            smd.Parameters.AddWithValue("@Username", Session["USER_ID"].ToString());
            smd.Parameters.AddWithValue("@Productid", Request.Params["ID"].ToString());
            smd.Parameters.AddWithValue("@Color", ddlColor.SelectedValue);
            smd.Parameters.AddWithValue("@Size", ddlSize.SelectedValue);
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
                Master.Status.Text = "[" + pstatus + "] " + pstatusmsg;
                Master.Status.ForeColor = System.Drawing.Color.Blue;

                EmptyField();
            }
        }
        else
        {
            Master.Status.Text = CekValidation;
            Master.Status.ForeColor = System.Drawing.Color.Red;
            return;
        }

        conn.CloseConnection();
        conn.Dispose();
    }

    protected void btnCheckOut_Click(object sender, EventArgs e)
    {
        Response.Redirect("CCart.aspx");
    }
}