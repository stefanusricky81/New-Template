using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace cs_forest
{
    public partial class MOMT : System.Web.UI.Page
    {
        private Model model = new Model();

        protected void Page_Load(object sender, EventArgs e)
        {
            string source = Request.QueryString["source"] ?? string.Empty;
            string msisdn = Request.QueryString["msisdn"] ?? string.Empty;
            string type = Request.QueryString["type"] ?? string.Empty;
            string name = Request.QueryString["name"] ?? string.Empty;

            if (type == "mt")
            {

                lblHeader.Text = "MT Listing";
                DataSet mt = model.SearchMT(source, msisdn,name);

                if (mt.Tables.Count > 0)
                {
                    if (source == "101")
                    {
                        ASPxGridView3.DataSource = mt;
                        ASPxGridView3.DataBind();
                        ASPxGridView1.Visible = false;
                    }
                    else
                    {
                        ASPxGridView1.DataSource = mt;
                        ASPxGridView1.DataBind();
                        ASPxGridView3.Visible = false;
                    }
                    
                }
                ASPxGridView2.Visible = false;
            }
            else if (type == "mo")
            {
                lblHeader.Text = "MO Listing";
                DataSet mo = model.SearchMO(source,msisdn,name);

                if (mo.Tables.Count > 0)
                {
                    if (source == "101")
                    {
                        ASPxGridView4.DataSource = mo;
                        ASPxGridView4.DataBind();
                        ASPxGridView1.Visible = false;
                        ASPxGridView3.Visible = false;
                    }
                    else
                    {
                        ASPxGridView1.DataSource = mo;
                        ASPxGridView1.DataBind();
                        ASPxGridView3.Visible = false;
                        ASPxGridView4.Visible = false;
                    }
                }
                ASPxGridView2.Visible = false;
            }
            else if (type == "globeph")
            {
                lblHeader.Text = "MO Listing";
                DataSet mo = model.SearchMOGlobePH(source, msisdn);

                if (mo.Tables.Count > 0)
                {
                    ASPxGridView2.DataSource = mo;
                    ASPxGridView2.DataBind();
                }
                ASPxGridView1.Visible = false;
                ASPxGridView3.Visible = false;
                ASPxGridView4.Visible = false;
            }
            else if (type == "globephmt")
            {
                lblHeader.Text = "Globe PH MT Listing";
                DataSet globe = model.SearchMTGlobePH(source,msisdn);

                if (globe.Tables.Count > 0)
                {
                    ASPxGridView1.DataSource = globe;
                    ASPxGridView1.DataBind();
                    ASPxGridView3.Visible = false;
                    ASPxGridView4.Visible = false;
                }
                ASPxGridView2.Visible = false;
            }
            else if (type == "globe")
            {
                lblHeader.Text = "Globe MT Listing";
                DataSet globe = model.SearchGlobe(msisdn);

                if (globe.Tables.Count > 0)
                {
                    ASPxGridView1.DataSource = globe;
                    ASPxGridView1.DataBind();
                }
                ASPxGridView2.Visible = false;
            }
        }
    }
}