using DevExpress.Utils;
using DevExpress.Web;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace cs_forest
{
    public partial class Blacklist : System.Web.UI.Page
    {
        private Model model = new Model();
        private string[] useridentity = HttpContext.Current.User.Identity.Name.Split(';');
        string msisdn = string.Empty;
        string module_id = string.Empty;
        string source = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            msisdn = Request.QueryString["msisdn"] ?? string.Empty;
            module_id = Request.QueryString["id"] ?? string.Empty;
            source = Request.QueryString["source"] ?? string.Empty;

            if (!IsPostBack)
            {
                if (source == "104")
                {
                    BindToGridPH(msisdn, module_id);
                    ASPxGridView1.Visible = false;
                }
                else
                {
                    BindToGrid(msisdn, module_id);
                    ASPxGridView2.Visible = false;
                }
            }
        }

        public void BindToGrid(string msisdn, string id)
        {
            DataSet mt = model.SearchBlacklist(msisdn, model.GetTelco(id),source);

            if (mt.Tables.Count > 0)
            {
                ASPxGridView1.DataSource = mt;
                ASPxGridView1.DataBind();
            }
        }

        public void BindToGridPH(string msisdn, string id)
        {
            DataSet mt = model.SearchBlacklistPH(msisdn, source);

            if (mt.Tables.Count > 0)
            {
                ASPxGridView2.DataSource = mt;
                ASPxGridView2.DataBind();
            }
        }

        protected void ASPxGridView1_CustomButtonCallback(object sender, DevExpress.Web.ASPxGridViewCustomButtonCallbackEventArgs e)
        {
            int id = (int)ASPxGridView1.GetRowValues(e.VisibleIndex, ASPxGridView1.KeyFieldName);
            string source = Request.QueryString["source"] ?? string.Empty;

            if (e.ButtonID == "btnBlock")
                model.UpdateBlacklist(id, "blocked", useridentity[0],source);
            else if (e.ButtonID == "btnUnBlock")
                model.UpdateBlacklist(id, "unblocked", useridentity[0],source);

            this.BindToGrid(msisdn, module_id);
        }

        protected void ASPxGridView1_CustomButtonInitialize(object sender, ASPxGridViewCustomButtonEventArgs e)
        {
            if (e.VisibleIndex == -1) return;

            if (e.ButtonID == "btnBlock")
            {
                if (((ASPxGridView)sender).GetRowValues(e.VisibleIndex, "status").ToString() == "blocked")
                    e.Visible = DefaultBoolean.False;
            }
            else if (e.ButtonID == "btnUnBlock")
            {
                if (((ASPxGridView)sender).GetRowValues(e.VisibleIndex, "status").ToString() == "unblocked")
                    e.Visible = DefaultBoolean.False;
            }
        }

        protected void ASPxGridView2_CustomButtonCallback(object sender, DevExpress.Web.ASPxGridViewCustomButtonCallbackEventArgs e)
        {
            int id = (int)ASPxGridView1.GetRowValues(e.VisibleIndex, ASPxGridView1.KeyFieldName);
            string source = Request.QueryString["source"] ?? string.Empty;

            if (e.ButtonID == "btnBlock")
                model.UpdateBlacklist(id, "blocked", useridentity[0], source);
            else if (e.ButtonID == "btnUnBlock")
                model.UpdateBlacklist(id, "unblocked", useridentity[0], source);

            this.BindToGrid(msisdn, module_id);
        }

        protected void ASPxGridView2_CustomButtonInitialize(object sender, ASPxGridViewCustomButtonEventArgs e)
        {
            if (e.VisibleIndex == -1) return;

            if (e.ButtonID == "btnBlock")
            {
                if (((ASPxGridView)sender).GetRowValues(e.VisibleIndex, "status").ToString() == "blocked")
                    e.Visible = DefaultBoolean.False;
            }
            else if (e.ButtonID == "btnUnBlock")
            {
                if (((ASPxGridView)sender).GetRowValues(e.VisibleIndex, "status").ToString() == "unblocked")
                    e.Visible = DefaultBoolean.False;
            }
        }
    }
}