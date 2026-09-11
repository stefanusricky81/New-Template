using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Net.Mail;
using System.Net;
using System.Text.RegularExpressions;

public partial class VerifyOrder : System.Web.UI.Page
{
    ClassConnectionDatabase conn = new ClassConnectionDatabase();
    DataTable dt = new DataTable();
    SqlCommand smd = new SqlCommand();
    SqlDataReader sdr = null;
    int vstatus = 0;
    string pstatus = string.Empty;
    string pstatusmsg = string.Empty;
    string _OrderNo = string.Empty;
    string VCacheKey = "";
    decimal _Weight = 0;

    protected void GV_RowDataBound(object sender, GridViewRowEventArgs ge)
    {
        double _totalPrice = 0;
        double _TotalPayment = 0;
        ViewState["Weight"] = "0";
        

        foreach (GridViewRow gv in gvCart.Rows)
        {
            _TotalPayment = Convert.ToDouble(gv.Cells[3].Text) * Convert.ToDouble(gv.Cells[4].Text);
            gv.Cells[6].Text = Convert.ToDouble(_TotalPayment.ToString()).ToString("#,##0");

            _totalPrice = _totalPrice + System.Convert.ToDouble(gv.Cells[6].Text);

            ViewState["Weight"] = Convert.ToDecimal(ViewState["Weight"]) + (Convert.ToDecimal(gv.Cells[2].Text) * Convert.ToDecimal(gv.Cells[4].Text));
        }

        txtTotalOfOrder.Text = Convert.ToDouble(_totalPrice.ToString()).ToString("#,##0");
        txtGrandTotal.Text = Convert.ToDouble(txtTotalOfOrder.Text).ToString("#,##0");
    }

    protected void Isi_Ekpedisi()
    {
        conn.ConnectionDatabase();

        smd = new SqlCommand("GET_EKSPEDISI", conn.getconn());
        smd.CommandType = CommandType.StoredProcedure;
        SqlDataAdapter da = new SqlDataAdapter(smd);
        DataSet ds = new DataSet();
        da.Fill(ds);
        ddlEkspedisi.DataSource = ds;

        ddlEkspedisi.DataTextField = "NAMA_EKSPEDISI";
        ddlEkspedisi.DataValueField = "ID";
        ddlEkspedisi.DataBind();
        ddlEkspedisi.Items.Insert(0, "");

        conn.CloseConnection();
        conn.Dispose();
    }

    protected void Isi_BankAcc()
    {
        conn.ConnectionDatabase();

        smd = new SqlCommand("GET_BANKACCOUNT", conn.getconn());
        smd.CommandType = CommandType.StoredProcedure;
        SqlDataAdapter da = new SqlDataAdapter(smd);
        DataSet ds = new DataSet();
        da.Fill(ds);
        ddlBankAcc.DataSource = ds;

        ddlBankAcc.DataTextField = "BANK_NAME";
        ddlBankAcc.DataValueField = "ID";
        ddlBankAcc.DataBind();
        ddlBankAcc.Items.Insert(0, "");

        conn.CloseConnection();
        conn.Dispose();
    }

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

    protected void GetMemberData()
    {
        conn.ConnectionDatabase();

        smd = new SqlCommand("GET_DETAILMEMBERDATA", conn.getconn());
        smd.Parameters.AddWithValue("@USER", Session["USER_ID"].ToString());

        smd.CommandType = CommandType.StoredProcedure;
        sdr = smd.ExecuteReader();
        if (sdr.Read())
        {
            lblName.Text = sdr["NAMA"].ToString();
            lblIdCust.Text = sdr["ID"].ToString();
            lblAddress.Text = sdr["ALAMAT"].ToString();
            lblCity.Text = sdr["KOTA"].ToString();
            lblPhone.Text = sdr["TELEPHONE"].ToString();
            lblEmail.Text = sdr["EMAIL"].ToString();
            lblDateOrder.Text = DateTime.Now.ToString("dd-MM-yyyy");
        }

        conn.CloseConnection();
        conn.Dispose();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        lblStatus.Text = string.Empty;

        if (!IsPostBack)
        {
            if (Session["USER_ID"] == null || Session["USER_ID"].ToString() == string.Empty)
                Response.Redirect("SignIn.aspx");

            GetCart();
            Isi_Ekpedisi();
            Isi_BankAcc();
            GetMemberData();
        }
        
    }

    protected void ddlEkspedisi_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlEkspedisi.SelectedValue != string.Empty)
        {
            conn.ConnectionDatabase();

            smd = new SqlCommand("GET_DETAILPRICEPERKILO", conn.getconn());
            smd.Parameters.AddWithValue("@ID", ddlEkspedisi.SelectedValue);

            smd.CommandType = CommandType.StoredProcedure;
            sdr = smd.ExecuteReader();
            if (sdr.Read())
            {
                txtTotalOfShipping.Text = (System.Convert.ToDecimal(sdr["PRICE_PER_KILO"]) * System.Convert.ToDecimal(ViewState["Weight"])).ToString("#,##0");
                txtGrandTotal.Text = (System.Convert.ToDouble(txtTotalOfOrder.Text) + System.Convert.ToDouble(txtTotalOfShipping.Text)).ToString("#,##0");
            }

            conn.CloseConnection();
            conn.Dispose();
        }
        else
        {
            txtTotalOfShipping.Text = "0";
            txtGrandTotal.Text = (System.Convert.ToDouble(txtTotalOfOrder.Text) - System.Convert.ToDouble(txtTotalOfShipping.Text)).ToString("#,##0");
        }
        
    }

    protected void btnConfirmOrder_Click(object sender, EventArgs e)
    {
        if (ddlEkspedisi.SelectedValue == string.Empty)
        {
            lblStatus.Text = "Please Choose Courier First";
            lblStatus.ForeColor = System.Drawing.Color.Red;
            return;
        }
        
        if (ddlBankAcc.SelectedValue == string.Empty)
        {
            lblStatus.Text = "Please Choose Bank Account First";
            lblStatus.ForeColor = System.Drawing.Color.Red;
            return;
        }   

        conn.ConnectionDatabase();

        smd = new SqlCommand("InsertOrder", conn.getconn());
        smd.Parameters.AddWithValue("@UserID", Session["USER_ID"].ToString());
        smd.Parameters.AddWithValue("@RekID", ddlBankAcc.SelectedValue);
        smd.Parameters.AddWithValue("@EkpedisiID", ddlEkspedisi.SelectedValue);
        smd.Parameters.AddWithValue("@Total", Convert.ToDouble(txtGrandTotal.Text));

        smd.Parameters.Add("@Pstatus", SqlDbType.VarChar, 255);
        smd.Parameters["@Pstatus"].Direction = ParameterDirection.Output;
        smd.Parameters.Add("@PstatusMsg", SqlDbType.VarChar, 255);
        smd.Parameters["@PstatusMsg"].Direction = ParameterDirection.Output;
        smd.Parameters.Add("@OrderNo", SqlDbType.VarChar, 255);
        smd.Parameters["@OrderNo"].Direction = ParameterDirection.Output;

        smd.CommandType = CommandType.StoredProcedure;
        smd.ExecuteNonQuery();
        pstatus = smd.Parameters["@Pstatus"].Value.ToString();
        pstatusmsg = smd.Parameters["@PstatusMsg"].Value.ToString();
        _OrderNo = smd.Parameters["@OrderNo"].Value.ToString();

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
            sendMail();

            sendMailToAdmin();

            Response.Redirect("About.aspx");
        }

        conn.CloseConnection();
        conn.Dispose();
    }

    protected void sendMail()
    {
        string MsgTo = lblEmail.Text;

        string MailHost = "little-superstar.com";

        string MailPort = "25";
        string MailFrom = "no-reply@little-superstar.com";
        string MailUser = "no-reply@little-superstar.com";
        string MailPassword = "Justinsonja123!";

        SmtpClient theClient = new SmtpClient(MailHost, Convert.ToInt32(MailPort));
        theClient.UseDefaultCredentials = true;
        theClient.Credentials = new NetworkCredential(MailUser, MailPassword);

        MailMessage theMessage = new MailMessage(MailFrom, MsgTo);

        theMessage.Subject = "You're Order No";
        theMessage.Body = "Thank you for buying from us, this is your Order Number :" + _OrderNo + "<br />";
        theMessage.Body += "Total Payment : IDR " + txtGrandTotal.Text + "<br />";
        theMessage.Body += "Please Transfer to Account Number :" + lblAccNo.Text + "<br /><br /><br />";
        theMessage.Body += "Your time limit payment is 2 x 24 hours from (" + DateTime.Now.ToString("dd-MMM-yyyy") + ")" + "<br /><br /><br />";
        theMessage.Body += "Notes" + "<br />";
        theMessage.Body += "If you already fulfill the payment make sure you're confirm your payment via website or we couldn't send your item(s)."+ "<br /><br /><br />";
        theMessage.Body += "This is no-reply message. If you have any further question please contact via our email littlesuperstarfashion@gmail.com"+ "<br /><br /><br />";
        theMessage.Body += "Regards," + "<br /><br /><br />";
        theMessage.Body += "LittleSuperstar";

        theMessage.IsBodyHtml = true;

        theClient.Send(theMessage);
    }
    
    protected void sendMailToAdmin()
    {
        string MsgTo = "littlesuperstarfashion@gmail.com";

        string MailHost = "little-superstar.com";

        string MailPort = "25";
        string MailFrom = "no-reply@little-superstar.com";
        string MailUser = "no-reply@little-superstar.com";
        string MailPassword = "Justinsonja123!";

        SmtpClient theClient = new SmtpClient(MailHost, Convert.ToInt32(MailPort));
        theClient.UseDefaultCredentials = true;
        theClient.Credentials = new NetworkCredential(MailUser, MailPassword);

        MailMessage theMessage = new MailMessage(MailFrom, MsgTo);

        theMessage.Subject = "You have a new order(s) from :";
        theMessage.Body = "Customer Id : " + lblIdCust.Text + "<br/>" + " Customer Name : " + lblName.Text + "<br/>" + " Customer Email : " + lblEmail.Text + "<br />";
        theMessage.Body += "Customer Order No :" + _OrderNo + "<br />";
        theMessage.Body += "Total Payment : IDR " + txtGrandTotal.Text + "<br />";
        theMessage.Body += "Will Transfer to BCA :" + lblAccNo.Text + "<br />";
        theMessage.Body += "Please wait for the payment.";

        theMessage.IsBodyHtml = true;

        theClient.Send(theMessage);
    }

    protected void ddlBankAcc_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlBankAcc.SelectedValue != string.Empty)
        {
            conn.ConnectionDatabase();

            smd = new SqlCommand("GET_DETAILACC", conn.getconn());
            smd.Parameters.AddWithValue("@ID", ddlBankAcc.SelectedValue);

            smd.CommandType = CommandType.StoredProcedure;
            sdr = smd.ExecuteReader();
            if (sdr.Read())
            {
                lblAccNo.Text = sdr["Bank_Account"].ToString() + " a/n " + sdr["ACCOUNT_NAME"].ToString();
                hfAccountName.Value = sdr["ACCOUNT_NAME"].ToString();
            }

            conn.CloseConnection();
            conn.Dispose();
        }
        else
            lblAccNo.Text = string.Empty;
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
            Response.Redirect("VerifyOrder.aspx");
        }

        conn.CloseConnection();
        conn.Dispose();
        
    }
}