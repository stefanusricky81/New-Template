using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Data;
using System.Data.SqlClient;

public partial class AdminEkpedisi : System.Web.UI.Page
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
        txtName.Text = string.Empty;
        txtPrice.Text = string.Empty;
    }

    protected void myGridView_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        CekSession();
        GV.PageIndex = e.NewPageIndex;
        GV.DataSource = Cache[VCacheKey];
        GV.DataBind();
    }

    protected string Validation()
    {
        if (txtName.Text == string.Empty) return "Ekpedisi Name Can't be Empty";
        if (txtPrice.Text == string.Empty) return "Price Can't be Empty";

        return string.Empty;
    }

    protected void loaddata()
    {
        conn.ConnectionDatabase();

        smd = new SqlCommand("GET_DETAILEKSPEDISI", conn.getconn());
        smd.Parameters.AddWithValue("@ID", Request.Params["ID"].ToString());

        smd.CommandType = CommandType.StoredProcedure;
        sdr = smd.ExecuteReader();
        if (sdr.Read())
        {
            txtName.Text = sdr["NAMA_EKSPEDISI"].ToString();
            txtPrice.Text = sdr["PRICE_PER_KILO"].ToString();
        }

        conn.CloseConnection();
        conn.Dispose();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        CekSession();

        if (!IsPostBack)
        {
            if (Request.Params["type"] == "edit")
            {
                loaddata();
            }

            conn.ConnectionDatabase();

            smd = new SqlCommand("GET_EKSPEDISI", conn.getconn());
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

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        CekSession();
        Response.Redirect("AdminEkpedisi.aspx");
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        string CekValidation = string.Empty;

        CekSession();

        conn.ConnectionDatabase();

        CekValidation = Validation();

        if (CekValidation == string.Empty)
        {

            if (Request.Params["type"] == "edit")
            {
                smd = new SqlCommand("UPDATE_EKPEDISI", conn.getconn());
                smd.Parameters.AddWithValue("@Id", Request.Params["ID"]);
            }
            else
            {
                smd = new SqlCommand("SP_INSERTEKSPEDISI", conn.getconn());
            }

            smd.Parameters.AddWithValue("@Name", txtName.Text);
            smd.Parameters.AddWithValue("@Price", txtPrice.Text);

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
        Response.Redirect("AdminEkpedisi.aspx");
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

                smd = new SqlCommand("DELETE_EKSPEDISI", conn.getconn());
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

        Response.Redirect("AdminEkpedisi.aspx");
    }
}