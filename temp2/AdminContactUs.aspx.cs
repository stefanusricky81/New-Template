using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class AdminContactUs : System.Web.UI.Page
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
        txtOfficeDay1.Text = string.Empty;
        txtOfficeDay2.Text = string.Empty;
        txtPhone1.Text = string.Empty;
        txtPhone2.Text = string.Empty;
        txtHours1.Text = string.Empty;
        txtHours2.Text = string.Empty;
        txtSms.Text = string.Empty;
        txtEmail.Text = string.Empty;
    }

    protected void loaddata()
    {
        string _hours = string.Empty;
        string _days= string.Empty;
        conn.ConnectionDatabase();

        smd = new SqlCommand("GET_MASTERCONTACTUS", conn.getconn());
        smd.Parameters.AddWithValue("@ID", Request.Params["ID"].ToString());

        smd.CommandType = CommandType.StoredProcedure;
        sdr = smd.ExecuteReader();
        if (sdr.Read())
        {
            _hours =sdr["OFFICE_HOURS"].ToString();
            _days= sdr["OFFICE_DAY"].ToString();

            txtHours1.Text = _hours.Substring(0, _hours.IndexOf("-"));
            txtHours2.Text = _hours.Substring(_hours.IndexOf("-") + 1, ((_hours.Length - _hours.IndexOf("-")) - 1));

            txtOfficeDay1.Text = _days.Substring(0, _days.IndexOf("-"));
            txtOfficeDay2.Text = _days.Substring(_days.IndexOf("-") + 1, ((_days.Length - _days.IndexOf("-")) - 1));

            txtPhone1.Text = sdr["PHONE1"].ToString();
            txtPhone2.Text = sdr["PHONE2"].ToString();
            txtSms.Text = sdr["SMS"].ToString();
            txtEmail.Text = sdr["EMAIL"].ToString();
        }

        conn.CloseConnection();
        conn.Dispose();
    }

    protected string Validation()
    {
        if (txtOfficeDay1.Text == string.Empty && txtOfficeDay2.Text==string.Empty) return "Office Day Can't Empty";
        if (txtHours1.Text == string.Empty && txtHours2.Text == string.Empty) return "Office Hours Can't Empty";
        if (txtPhone1.Text == string.Empty || txtPhone2.Text == string.Empty) return "Phone Can't Empty";
        if (txtSms.Text == string.Empty) return "Sms Can't Empty";
        if (txtEmail.Text == string.Empty) return "Email Can't Empty";

        return string.Empty;
    }

    protected void myGridView_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        CekSession();
        GV.PageIndex = e.NewPageIndex;
        GV.DataSource = Cache[VCacheKey];
        GV.DataBind();
    }

    protected void Get_Data()
    {
        conn.ConnectionDatabase();

        smd = new SqlCommand("GET_CONTACTUS", conn.getconn());
        smd.CommandType = CommandType.StoredProcedure;
        sdr = smd.ExecuteReader();
        if (sdr.HasRows)
        {
            dt.Load(sdr);
        }

        if (dt.Rows.Count > 0)
        {
            GV.DataSource = dt;
            Cache.Insert(VCacheKey, GV.DataSource, null, DateTime.Now.AddMinutes(60), TimeSpan.Zero);
        }
        else if (dt.Rows.Count == 0)
        {
            //bDelete.Visible = false;
        }
        GV.DataBind();

        conn.CloseConnection();
        conn.Dispose();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        CekSession();
        Get_Data();

        if (!IsPostBack)
        {
            if (Request.Params["type"] == "edit")
            {
                loaddata();
            }
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        CekSession();
        Response.Redirect("AdminContactUs.aspx");
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
                smd = new SqlCommand("UPDATE_CONTACTUS", conn.getconn());
                smd.Parameters.AddWithValue("@Id", Request.Params["ID"]);
            }
            else
            {
                smd = new SqlCommand("SP_INSERTCONTACTUS", conn.getconn());
            }

            smd.Parameters.AddWithValue("@OfficeDay", txtOfficeDay1.Text +"-"+txtOfficeDay2.Text);
            smd.Parameters.AddWithValue("@HourDay", txtHours1.Text + "-" + txtHours2.Text);
            smd.Parameters.AddWithValue("@Phone1", txtPhone1.Text);
            smd.Parameters.AddWithValue("@Phone2", txtPhone2.Text);
            smd.Parameters.AddWithValue("@Sms", txtSms.Text);
            smd.Parameters.AddWithValue("@Email", txtEmail.Text);

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
        Response.Redirect("AdminContactUs.aspx");
    }
}