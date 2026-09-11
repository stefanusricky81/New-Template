using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Data;
using System.Data.SqlClient;

public partial class AdminUploadProduct : System.Web.UI.Page
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
        txtProductName.Text = "";
        txtBrand.Text = "";
        ddlCategory.SelectedIndex = -1;
        txtPrice.Text = "";
        //fileuploadimages.PostedFile.FileName = "";
        ddlProductCategory.SelectedIndex = -1;
        ddlProductGender.SelectedIndex = -1;
        ddlProductStatus.SelectedIndex = -1;
    }

    protected void GV_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[3].Text = e.Row.Cells[3].Text.Replace("&lt;\br&gt;", "\r\n");
        }
    }

    protected string Validation()
    {
        if (txtProductName.Text == string.Empty) return "Product Name Can't Empty";
        if (txtBrand.Text == string.Empty) return "Brand Can't Empty";
        if (txtPrice.Text == string.Empty) return "Price Can't Empty";
        if (ddlCategory.SelectedValue == string.Empty) return "Product Type Can't Empty";
        if (ddlProductCategory.SelectedValue == string.Empty) return "Product Category Can't Empty";
        if (ddlProductGender.SelectedValue == string.Empty) return "Product Gender Can't Empty";
        if (ddlProductStatus.SelectedValue == string.Empty) return "Product Status Can't Empty";
        
        return string.Empty;
    }

    protected void myGridView_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        CekSession();
        GV.PageIndex = e.NewPageIndex;
        GV.DataSource = Cache[VCacheKey];
        GV.DataBind();
    }
    
    protected void loaddata()
    {
        conn.ConnectionDatabase();

        smd = new SqlCommand("GET_DETAILPRODUCT", conn.getconn());
        smd.Parameters.AddWithValue("@ID", Request.Params["ID"].ToString());

        smd.CommandType = CommandType.StoredProcedure;
        sdr = smd.ExecuteReader();
        if (sdr.Read())
        {
            txtProductName.Text = sdr["PRODUCTNAME"].ToString();
            txtBrand.Text = sdr["BRAND"].ToString();
            txtPrice.Text = sdr["PRICE"].ToString();
            ddlCategory.SelectedValue = sdr["CATEGORY"].ToString();
            ddlProductCategory.SelectedValue = sdr["PRODUCT_CATEGORY"].ToString();
            ddlProductGender.SelectedValue = sdr["GENDER"].ToString();
            ddlProductStatus.SelectedValue = sdr["PRODUCT_STATUS"].ToString();
            txtDesc.Text = sdr["DESCRIP"].ToString();
            txtMaterial.Text = sdr["MATERIAL"].ToString();
            txtCare.Text = sdr["CARE"].ToString();
            hfImage.Value = sdr["IMAGE"].ToString();
        }

        conn.CloseConnection();
        conn.Dispose();
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

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            CekSession();

            if (Request.Params["type"] == "edit")
            {
                loaddata();
            }
        }

        if (!IsPostBack)
        {
            Isi_Category();
            Isi_ProductCategory();
            Isi_Gender();
            Isi_Status();

            conn.ConnectionDatabase();

            smd = new SqlCommand("GET_PRODUCT", conn.getconn());
            smd.CommandType = CommandType.StoredProcedure;
            sdr = smd.ExecuteReader();
            if (sdr.HasRows)
            {
                dt.Load(sdr);
            }

            if (dt.Rows.Count > 0)
            {
                bDelete.Visible = true;
                GV.DataSource = dt;
                Cache.Insert(VCacheKey, GV.DataSource, null, DateTime.Now.AddMinutes(60), TimeSpan.Zero);
            }
            else if (dt.Rows.Count == 0)
            {
                bDelete.Visible = false;
            }
            GV.DataBind();

            conn.CloseConnection();
            conn.Dispose();
        }
    }
    
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        string CekValidation = string.Empty;
        string _image = string.Empty;

        CekSession();

        string filename = Path.GetFileName(fileuploadimages.PostedFile.FileName);

        if (fileuploadimages.HasFile == false)
            _image = hfImage.Value;
        else
        {
            _image = "ImgProduct/" + filename;
            fileuploadimages.SaveAs(Server.MapPath(_image));
        }

        CekValidation = Validation();

        ClassConnectionDatabase conn = new ClassConnectionDatabase();
        conn.ConnectionDatabase();
        if (CekValidation == string.Empty)
        {

            if (Request.Params["type"] == "edit")
            {
                smd = new SqlCommand("UPDATE_PRODUCT", conn.getconn());
                smd.Parameters.AddWithValue("@Id", Request.Params["ID"]);
            }
            else
            {
                smd = new SqlCommand("SP_INSERTPRODUCT", conn.getconn());
            }
            
            smd.Parameters.AddWithValue("@ProductName", txtProductName.Text);
            smd.Parameters.AddWithValue("@Brand", txtBrand.Text);
            smd.Parameters.AddWithValue("@Category", ddlCategory.SelectedValue);
            smd.Parameters.AddWithValue("@Price", txtPrice.Text);
            smd.Parameters.AddWithValue("@Image", _image);
            smd.Parameters.AddWithValue("@ProductCategory", ddlProductCategory.SelectedValue);
            smd.Parameters.AddWithValue("@ProductGender", ddlProductGender.SelectedValue);
            smd.Parameters.AddWithValue("@ProductStatus", ddlProductStatus.SelectedValue);
            smd.Parameters.AddWithValue("@Desc", txtDesc.Text.Replace("\r\n", "<\br>"));
            smd.Parameters.AddWithValue("@Material", txtMaterial.Text);
            smd.Parameters.AddWithValue("@Care", txtCare.Text);

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

        Response.Redirect("AdminUploadProduct.aspx");
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        CekSession();

        Response.Redirect("AdminUploadProduct.aspx");
    }

    protected void Isi_Category()
    {
        conn.ConnectionDatabase();

        smd = new SqlCommand("GET_CATEGORY", conn.getconn());
        smd.CommandType = CommandType.StoredProcedure;
        SqlDataAdapter da = new SqlDataAdapter(smd);
        DataSet ds = new DataSet();
        da.Fill(ds);
        ddlCategory.DataSource = ds;

        ddlCategory.DataTextField = "Category_Name";
        ddlCategory.DataValueField = "ID";
        ddlCategory.DataBind();
        ddlCategory.Items.Insert(0, "");

        conn.CloseConnection();
        conn.Dispose();
    }

    protected void Isi_ProductCategory()
    {
        conn.ConnectionDatabase();

        smd = new SqlCommand("GET_PRODUCT_CATEGORY", conn.getconn());
        smd.CommandType = CommandType.StoredProcedure;
        SqlDataAdapter da = new SqlDataAdapter(smd);
        DataSet ds = new DataSet();
        da.Fill(ds);
        ddlProductCategory.DataSource = ds;

        ddlProductCategory.DataTextField = "Product_Category";
        ddlProductCategory.DataValueField = "ID";
        ddlProductCategory.DataBind();
        ddlProductCategory.Items.Insert(0, "");

        conn.CloseConnection();
        conn.Dispose();
    }

    protected void Isi_Gender()
    {
        conn.ConnectionDatabase();

        smd = new SqlCommand("GET_PRODUCT_GENDER", conn.getconn());
        smd.CommandType = CommandType.StoredProcedure;
        SqlDataAdapter da = new SqlDataAdapter(smd);
        DataSet ds = new DataSet();
        da.Fill(ds);
        ddlProductGender.DataSource = ds;

        ddlProductGender.DataTextField = "Gender";
        ddlProductGender.DataValueField = "ID";
        ddlProductGender.DataBind();
        ddlProductGender.Items.Insert(0, "");

        conn.CloseConnection();
        conn.Dispose();
    }

    protected void Isi_Status()
    {
        conn.ConnectionDatabase();

        smd = new SqlCommand("GET_PRODUCT_STATUS", conn.getconn());
        smd.CommandType = CommandType.StoredProcedure;
        SqlDataAdapter da = new SqlDataAdapter(smd);
        DataSet ds = new DataSet();
        da.Fill(ds);
        ddlProductStatus.DataSource = ds;

        ddlProductStatus.DataTextField = "Status";
        ddlProductStatus.DataValueField = "ID";
        ddlProductStatus.DataBind();
        ddlProductStatus.Items.Insert(0, "");

        conn.CloseConnection();
        conn.Dispose();
    }

    protected void bDelete_Click(object sender, EventArgs e)
    {
        CheckBox cb;
        int count = 0;

        CekSession();

        foreach (GridViewRow dvr in GV.Rows)
        {
            cb = (CheckBox)dvr.FindControl("chkBxSelect");
            if (cb != null && cb.Checked)
            {
                count++;
                conn.ConnectionDatabase();

                double idVal = (double)Convert.ToDouble(GV.DataKeys[dvr.RowIndex].Value);

                smd = new SqlCommand("DELETE_PRODUCT", conn.getconn());
                smd.Parameters.AddWithValue("@ID", idVal);
                smd.Parameters.AddWithValue("@UserId", Session["USER_ID"].ToString());

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

                conn.CloseConnection();
                conn.Dispose();
            }
        }

        Response.Redirect("AdminUploadProduct.aspx");
    }

}