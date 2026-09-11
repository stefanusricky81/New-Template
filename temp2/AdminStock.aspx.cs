using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Data;
using System.Data.SqlClient;

public partial class AdminStock : System.Web.UI.Page
{
    ClassConnectionDatabase conn = new ClassConnectionDatabase();
    DataTable dt = new DataTable();
    SqlCommand smd = new SqlCommand();
    SqlDataReader sdr = null;
    int vstatus = 0;
    string pstatus = string.Empty;
    string pstatusmsg = string.Empty;
    string VCacheKey = "";

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

    protected void EmptyField()
    {
        ddlColor.SelectedIndex = -1;
        ddlSize.SelectedIndex = -1;
        ddlProductName.SelectedIndex = -1;
        txtStock.Text = "";
    }

    protected string Validation()
    {
        if (ddlColor.SelectedValue == string.Empty) return "Color Can't be Empty";
        if (ddlSize.SelectedValue == string.Empty) return "Size Can't be Empty";
        if (ddlProductName.SelectedValue == string.Empty) return "Product Can't be Empty";
        if (txtStock.Text == string.Empty) return "Stock Can't be Empty";

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

        smd = new SqlCommand("GET_DETAILSTOCK", conn.getconn());
        smd.Parameters.AddWithValue("@ID", Request.Params["ID"].ToString());

        smd.CommandType = CommandType.StoredProcedure;
        sdr = smd.ExecuteReader();
        if (sdr.Read())
        {
            ddlProductName.SelectedValue = sdr["PRODUCT_ID"].ToString();
            ddlSize.SelectedValue = sdr["SIZE_ID"].ToString();
            ddlColor.SelectedValue = sdr["COLOR_ID"].ToString();
            txtStock.Text = sdr["STOCK"].ToString();

            ddlProductName.Enabled = false;
            ddlProductName.ForeColor = System.Drawing.Color.Gray;
            ddlSize.Enabled = false;
            ddlSize.ForeColor = System.Drawing.Color.Gray;
            ddlColor.Enabled = false;
            ddlColor.ForeColor = System.Drawing.Color.Gray;
        }

        conn.CloseConnection();
        conn.Dispose();
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

            Isi_Size();
            Isi_Color();
            Isi_Product();
        }

        if (!IsPostBack)
        {
            conn.ConnectionDatabase();

            smd = new SqlCommand("GET_STOCK", conn.getconn());
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

        CekSession();

        conn.ConnectionDatabase();

        CekValidation = Validation();

        if (CekValidation == string.Empty)
        {

            if (Request.Params["type"] == "edit")
            {
                smd = new SqlCommand("UPDATE_STOCK", conn.getconn());
                smd.Parameters.AddWithValue("@Id", Request.Params["ID"]);
            }
            else
            {
                smd = new SqlCommand("SP_INSERTSTOCK", conn.getconn());
                smd.Parameters.AddWithValue("@Product", ddlProductName.SelectedValue);
                smd.Parameters.AddWithValue("@Size", ddlSize.SelectedValue);
                smd.Parameters.AddWithValue("@Color", ddlColor.SelectedValue);
            }

            smd.Parameters.AddWithValue("@Stock", txtStock.Text);

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
        Response.Redirect("AdminStock.aspx");
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        CekSession();
        Response.Redirect("AdminStock.aspx");
    }

    protected void Isi_Size()
    {
        conn.ConnectionDatabase();

        smd = new SqlCommand("GET_DDL_SIZESTOCK", conn.getconn());
        smd.CommandType = CommandType.StoredProcedure;
        SqlDataAdapter da = new SqlDataAdapter(smd);
        DataSet ds = new DataSet();
        da.Fill(ds);
        ddlSize.DataSource = ds;

        ddlSize.DataTextField = "Size";
        ddlSize.DataValueField = "ID";
        ddlSize.DataBind();
        ddlSize.Items.Insert(0, "");

        conn.CloseConnection();
        conn.Dispose();
    }

    protected void Isi_Color()
    {
        conn.ConnectionDatabase();

        smd = new SqlCommand("GET_DDL_COLORSTOCK", conn.getconn());
        smd.CommandType = CommandType.StoredProcedure;
        SqlDataAdapter da = new SqlDataAdapter(smd);
        DataSet ds = new DataSet();
        da.Fill(ds);
        ddlColor.DataSource = ds;

        ddlColor.DataTextField = "Color";
        ddlColor.DataValueField = "ID";
        ddlColor.DataBind();
        ddlColor.Items.Insert(0, "");

        conn.CloseConnection();
        conn.Dispose();
    }

    protected void Isi_Product()
    {
        conn.ConnectionDatabase();

        smd = new SqlCommand("GET_DDL_PRODUCTSTOCK", conn.getconn());
        smd.CommandType = CommandType.StoredProcedure;
        SqlDataAdapter da = new SqlDataAdapter(smd);
        DataSet ds = new DataSet();
        da.Fill(ds);
        ddlProductName.DataSource = ds;

        ddlProductName.DataTextField = "Name";
        ddlProductName.DataValueField = "ID";
        ddlProductName.DataBind();
        ddlProductName.Items.Insert(0, "");

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

                smd = new SqlCommand("DELETE_STOCK", conn.getconn());
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
                    Master.Status.Text = "[" + pstatus + "] " + pstatusmsg;
                    Master.Status.ForeColor = System.Drawing.Color.Red;
                    return;
                }

                conn.CloseConnection();
                conn.Dispose();
            }
        }

        Response.Redirect("AdminStock.aspx");
    }
}