using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class AdminSize : System.Web.UI.Page
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
        txtSize.Text = "";
    }

    protected string Validation()
    {
        if (txtSize.Text == string.Empty) return "Size Can't Empty";

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

        smd = new SqlCommand("GET_DETAILSIZE", conn.getconn());
        smd.Parameters.AddWithValue("@ID", Request.Params["ID"].ToString());

        smd.CommandType = CommandType.StoredProcedure;
        sdr = smd.ExecuteReader();
        if (sdr.Read())
        {
            txtSize.Text = sdr["SIZE"].ToString();
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
        }

        if (!IsPostBack)
        {
            conn.ConnectionDatabase();

            smd = new SqlCommand("GET_SIZE", conn.getconn());
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
        Response.Redirect("AdminSize.aspx");
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
                smd = new SqlCommand("UPDATE_SIZE", conn.getconn());
                smd.Parameters.AddWithValue("@Id", Request.Params["ID"]);
            }
            else
            {
                smd = new SqlCommand("SP_INSERTSIZE", conn.getconn());
            }

            smd.Parameters.AddWithValue("@Size", txtSize.Text);

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
        Response.Redirect("AdminSize.aspx");
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

                smd = new SqlCommand("DELETE_SIZE", conn.getconn());
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

        Response.Redirect("AdminBankAccount.aspx");
    }
}