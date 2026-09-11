using DevExpress.Utils;
using DevExpress.Web;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Web;

namespace cs_forest
{
    public partial class Subscriber : System.Web.UI.Page
    {
        private Model model = new Model();
        private AddOn addon = new AddOn();
        private Tools tools = new Tools();
        private string[] useridentity = HttpContext.Current.User.Identity.Name.Split(';');
        private string msisdn = string.Empty;
        private string name = string.Empty;
        private string source = string.Empty;
        private string module_id = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            source = Request.QueryString["source"] ?? string.Empty;
            msisdn = Request.QueryString["msisdn"] ?? string.Empty;
            name = Request.QueryString["name"] ?? string.Empty;
            module_id = Request.QueryString["id"] ?? string.Empty;

            //if (string.IsNullOrEmpty(source) || string.IsNullOrEmpty(msisdn) || string.IsNullOrEmpty(name))
            //    Response.Redirect("login.aspx?type=logout");

            if (useridentity[1] == "telco")
            {
                btnBlacklist.Visible = false;
            }

            if (!IsPostBack)
            {
                if (source == "101")
                {
                    this.BindToGrid101(source, msisdn);
                    section2.Visible = true;
                    section3.Visible = false;
                }
                else if (source == "104")
                {
                    this.BindToGridGlobePH(source, msisdn);
                    section1.Visible = false;
                    section2.Visible = false;
                    section3.Visible = true;
                }
                else
                {
                    this.BindToGrid(source, msisdn, name);
                    section1.Visible = true;
                    section3.Visible = false;
                }
            }
            else
            {
                if (source == "101")
                {
                    this.BindToGrid101(source, msisdn);
                    section2.Visible = true;
                }
                else if (source == "104")
                {
                    this.BindToGridGlobePH(source, msisdn);
                    section1.Visible = false;
                    section2.Visible = false;
                    section3.Visible = true;
                }
                else
                {
                    this.BindToGrid(source, msisdn, name);
                    section1.Visible = true;
                }
            }
        }

        public void BindToGrid(string source, string msisdn, string name)
        {
            DataSet subs = SearchSubscriber(source, msisdn, name);

            if (subs.Tables.Count > 0)
            {
                ASPxGridView1.DataSource = subs;
                ASPxGridView1.DataBind();
            }
            else
            {
                dvStopAll.Visible = false;
            }
        }

        public void BindToGrid101(string source, string msisdn)
        {
            DataSet subs = SearchSubscriber101(source, msisdn);

            if (subs.Tables.Count > 0)
            {
                ASPxGridView2.DataSource = subs;
                ASPxGridView2.DataBind();
            }
            else
            {
                dvStopAll.Visible = false;
            }
        }

        public void BindToGridGlobePH(string source, string msisdn)
        {
            DataSet subs = SearchSubscriberGlobePH(source, msisdn);

            if (subs.Tables.Count > 0)
            {
                ASPxGridView3.DataSource = subs;
                ASPxGridView3.DataBind();
            }
            else
            {
                dvStopAll.Visible = false;
            }
        }

        protected void ASPxGridView1_CustomButtonCallback(object sender, DevExpress.Web.ASPxGridViewCustomButtonCallbackEventArgs e)
        {
            int userid = (int)ASPxGridView1.GetRowValues(e.VisibleIndex, ASPxGridView1.KeyFieldName);
            string keyword = (string)ASPxGridView1.GetRowValues(e.VisibleIndex, "keyword");
            string msisdn = Request.QueryString["msisdn"];
            string source = Request.QueryString["source"] ?? string.Empty;
            string name = Request.QueryString["name"];

            if (keyword == "CHITIK")
            {
                addon.StopChitik(msisdn);
            }

            if (e.ButtonID == "btnUnsub")
                model.Unsubscribe(source, userid, useridentity[0], name);

            if (name == "20000")
            {
                SendMT(keyword, msisdn);
            }

            this.BindToGrid(source, msisdn, name);
        }

        protected void ASPxGridView1_CustomButtonInitialize(object sender, ASPxGridViewCustomButtonEventArgs e)
        {
            if (e.VisibleIndex == -1) return;

            if (e.ButtonID == "btnUnsub")
            {
                if (((ASPxGridView)sender).GetRowValues(e.VisibleIndex, "status").ToString() == "STOP")
                    e.Visible = DefaultBoolean.False;
            }
        }

        protected void ASPxGridView1_DataBound(object sender, EventArgs e)
        {
            if (useridentity[1] == "telco")
            {
                this.ASPxGridView1.Columns["Revenue (RM)"].Visible = false;
            }
        }

        protected void ASPxGridView2_CustomButtonCallback(object sender, DevExpress.Web.ASPxGridViewCustomButtonCallbackEventArgs e)
        {
            int subid = (int)ASPxGridView2.GetRowValues(e.VisibleIndex, ASPxGridView2.KeyFieldName);
            string keyword = (string)ASPxGridView2.GetRowValues(e.VisibleIndex, "KeywordName");

            if (e.ButtonID == "btnUnsub2")
                model.Unsubscribe101(source, subid, useridentity[0]);
            this.BindToGrid101(source, msisdn);
        }

        protected void ASPxGridView2_CustomButtonInitialize(object sender, ASPxGridViewCustomButtonEventArgs e)
        {
            if (e.VisibleIndex == -1) return;

            if (e.ButtonID == "btnUnsub2")
            {
                if (((ASPxGridView)sender).GetRowValues(e.VisibleIndex, "Status").ToString() == "STOP")
                    e.Visible = DefaultBoolean.False;
            }
        }

        protected void ASPxGridView3_CustomButtonCallback(object sender, DevExpress.Web.ASPxGridViewCustomButtonCallbackEventArgs e)
        {
            int subid = (int)ASPxGridView3.GetRowValues(e.VisibleIndex, ASPxGridView3.KeyFieldName);

            if (e.ButtonID == "btnUnsub3")
            {
                model.UnsubscribeGlobePH(source, subid, useridentity[0]);
                tools.SendMOGlobePH(msisdn);
            }
            this.BindToGridGlobePH(source, msisdn);
        }

        protected void ASPxGridView3_CustomButtonInitialize(object sender, ASPxGridViewCustomButtonEventArgs e)
        {
            if (e.VisibleIndex == -1) return;

            if (e.ButtonID == "btnUnsub3")
            {
                if (((ASPxGridView)sender).GetRowValues(e.VisibleIndex, "Status").ToString() == "Unactive")
                    e.Visible = DefaultBoolean.False;
            }
        }

        protected void btnStopALL_Click(object sender, EventArgs e)
        {
            model.UnsubscribeAll(source, msisdn, useridentity[0], name);
            if (name == "20000")
            {
                SendMT("All", msisdn);
                addon.StopChitik(msisdn);
            }
            this.BindToGrid(source, msisdn, name);
        }

        public DataSet SearchSubscriber(string source, string msisdn, string name)
        {
            DataSet ds = new DataSet();

            string tblMO = string.Empty;
            string tblMT = string.Empty;
            string shortcode = "92325";

            #region old
            //if (name == "2345")
            //    tblMO = "tblMO_tunetalk";
            //else if (name == "28282")
            //    tblMO = "tblMO_umobile";
            //else
            //    tblMO = "tblMO_" + name;
            #endregion
            tblMO = "tblMO_" + name;
            #region old
            //if (name == "2345")
            //    tblMT = "views_tunetalk";
            //else if (name == "28282")
            //    tblMT = "tblMT_umobile";
            //else if (name == "20088")
            //    tblMT = "tblMT_20088_All";
            //else
            //    tblMT = "tblMT_" + name;
            #endregion
            tblMT = "tblMT_" + name;
            string strSql = "SELECT s.userid,s.msisdn,p.shortcode,p.keyword,s.mo_id,s.groupid,isnull(s.unsubby,'n/a') as unsubby,CASE WHEN s.status = 0 THEN 'REG' ELSE 'STOP' END AS status,s.date_reg,s.date_unsubs,p.provider_id,s.provid,s.telconame as Gateway,(case when s.unsubby is null then 'SMS' else 'Portal' end) as UnsubType  ";
            //strSql += ",(select count(a.msisdn) from [MOMT_NEW].[dbo].[" + tblMO + "] as a where a.msisdn='" + msisdn + "' and a.provid=s.provid) as mo";
            //strSql += ",(select count(a.msisdn) from [MOMT_NEW].[dbo].[" + tblMT + "] as a where a.msisdn='" + msisdn + "' and a.provid=s.provid) as mt";

            if (useridentity[1] != "telco")
            {
                strSql += ",s.regtype ";
                if (name == "22355")
                    strSql += ",(select CAST(SUM(a.price) AS NVARCHAR) + '.00' from [MOMT_NEW].[dbo].[" + tblMT + "] AS A INNER JOIN [MOMT_NEW].[dbo].[DN_22355] AS B ON A.msg_id=B.msgid WHERE A.msisdn='" + msisdn + "' AND B.Status='Delivered' and A.provid=s.provid) as revenue ";
                else if (name == "20088")
                    strSql += ",(select CAST(SUM(a.price) AS NVARCHAR) + '.00' from [MOMTV2].[dbo].[" + tblMT + "] AS A WHERE A.msisdn='" + msisdn + "' AND A.DNStatus='Delivered' and A.provid=s.provid) as revenue ";
                else
                    strSql += ",(select CAST(SUM(a.price) AS NVARCHAR) + '.00' from [MOMT_NEW].[dbo].[" + tblMT + "] as a where a.msisdn='" + msisdn + "' and a.DNStatus='Delivered' and a.provid=s.provid) as revenue ";
            }
            else
            {
                strSql += ",(case when s.regtype = 'SMS' then 'SMS' else 'WAP' end) AS regtype ";
            }
            #region old
            //if (name == "20000")
            //{
            //    strSql += "FROM [smsadminv2].[dbo].[tblSubscriber_20000] AS s INNER JOIN [smsadminv2].[dbo].[tblProvDet] AS p ON s.provid = p.provid ";
            //}
            //else if (name == "20088")
            //{
            //    strSql += "FROM [smsadminv2].[dbo].[tblSubscriber20088] AS s INNER JOIN [smsadminv2].[dbo].[tblProvDet] AS p ON s.provid = p.provid ";
            //}
            //else
            //{
            //    strSql += "FROM [smsadminv2].[dbo].[tblSubscriber] AS s INNER JOIN [smsadminv2].[dbo].[tblProvDet] AS p ON s.provid = p.provid ";
            //}
            //strSql += "FROM [smsadminv2].[dbo].[tblSubscriber] AS s INNER JOIN [smsadminv2].[dbo].[tblProvDet] AS p ON s.provid = p.provid ";
            #endregion
            strSql += "FROM [smsadminv2].[dbo].[tblSubscriber] AS s INNER JOIN [smsadminv2].[dbo].[tblProvDet] AS p ON s.provid = p.provid ";
            strSql += "WHERE (LTRIM(RTRIM(s.msisdn)) = '" + msisdn + "') AND p.shortcode='" + shortcode + "'  ORDER BY s.date_unsubs DESC";

            try
            {
                using (SqlConnection cn = new SqlConnection(model.ConnStr(source, "smsadminv2")))
                {
                    cn.Open();
                    using (SqlDataAdapter adp = new SqlDataAdapter(strSql, cn))
                    {
                        ds.Reset();
                        adp.Fill(ds);
                    }
                }
            }
            catch (SqlException ex)
            {
                //ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('" + ex.Message + "');", true);
                if (ex.ErrorCode == -2146232060)
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Connection timeout, please try again.');", true);
                else
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Error " + ex.ErrorCode + ". Please submit this error to the system admin.');", true);
            }

            return ds;
        }

        public DataSet SearchSubscriber101(string source, string msisdn)
        {
            DataSet ds = new DataSet();

            string strSql = "SELECT [SubId], [KeywordName], [Msisdn],(CASE WHEN [Status]=0 THEN 'STOP' ELSE 'ACTIVE' END) AS [Status], [MoId], [DateRegister], [DateUnsubscribe] FROM [SubscriberList] WHERE [Msisdn]='" + msisdn + "'";
            try
            {
                using (SqlConnection cn = new SqlConnection(model.ConnStr(source, "SMSAdmin")))
                {
                    cn.Open();
                    using (SqlDataAdapter adp = new SqlDataAdapter(strSql, cn))
                    {
                        ds.Reset();
                        adp.Fill(ds);
                    }
                }
            }
            catch (SqlException ex)
            {
                //ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('" + ex.Message + "');", true);
                if (ex.ErrorCode == -2146232060)
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Connection timeout, please try again.');", true);
                else
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Error " + ex.ErrorCode + ". Please submit this error to the system admin.');", true);
            }

            return ds;
        }

        public DataSet SearchSubscriberGlobePH(string source, string msisdn)
        {
            DataSet ds = new DataSet();

            string strSql = "SELECT [SubsId], [DateCreated], [Msisdn], CASE WHEN [Status2] = 1 THEN 'Active' WHEN [Status2] = 0 THEN 'Unactive' ELSE 'PreSubs' END AS [Status] , [TotalDownload], [RenewDate], [LastRenewal] FROM [tblSubscriber] WHERE ([Msisdn] = '" + msisdn + "')";
            try
            {
                using (SqlConnection cn = new SqlConnection(model.ConnStr(source, "GameWorld")))
                {
                    cn.Open();
                    using (SqlDataAdapter adp = new SqlDataAdapter(strSql, cn))
                    {
                        ds.Reset();
                        adp.Fill(ds);
                    }
                }
            }
            catch (SqlException ex)
            {
                //ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('" + ex.Message + "');", true);
                if (ex.ErrorCode == -2146232060)
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Connection timeout, please try again.');", true);
                else
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Error " + ex.ErrorCode + ". Please submit this error to the system admin.');", true);
            }

            return ds;
        }

        protected void btnBlacklist_Click(object sender, EventArgs e)
        {
            try
            {
                model.AddBlaclist(msisdn, useridentity[0], model.GetTelco(module_id), source);
                model.UnsubscribeAll(source, msisdn, useridentity[0], name);
                lblMsg.Text = msisdn + " has been add into blacklist and stop all services.";
            }
            catch (SqlException ex)
            {
                if (ex.ErrorCode == -2146232060)
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Connection timeout, please try again.');", true);
                else
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Error " + ex.ErrorCode + ". Please submit this error to the system admin.');", true);
            }
        }

        public string SendMT(string keyword, string msisdn)
        {
            string urlStr = "http://localhost/digi2000v2/MT-SMS-20000.aspx";
            urlStr += "?msisdn=" + msisdn;
            if (keyword != "All")
                urlStr += "&reply=Langganan anda telah ditamatkan. Utk kembali melanggan, hantar ON " + keyword.Trim() + " ke 20000";
            else
                urlStr += "&reply=Semua langganan anda telah ditamatkan.";
            urlStr += "&p=0";
            urlStr += "&k=" + keyword.Trim();
            urlStr += "&s=20000";
            urlStr += "&msgid=" + tools.MessageId();
            return tools.WebReq(urlStr);
        }
    }
}