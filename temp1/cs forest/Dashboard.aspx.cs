using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace cs_forest
{
    public partial class Dashboard : System.Web.UI.Page
    {
        private Model model = new Model();
        private string[] useridentity = HttpContext.Current.User.Identity.Name.Split(';');
        private string module_id = string.Empty;
        private string source = string.Empty;
        private string name = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            module_id = Request.QueryString["id"] ?? string.Empty;
            source = Request.QueryString["source"] ?? string.Empty;
            name = Request.QueryString["name"] ?? string.Empty;

            if (useridentity[1] == "telco")
            {
                panel_blacklist.Visible = true;
                panelMO.Visible = false;
                panelMT.Visible = false;
                pnlDescription.Visible = false;
                if (module_id == "11")
                {
                    panelMT.Visible = true;
                    panelMO.Visible = true;
                }
            }

            if (name.ToLower() == "smpp")
            {
                Response.Redirect("smppsearchmt.aspx");
            }
            else if (name.ToLower() == "13999")
            {
                panel_subscriber.Visible = false;
                pnlDescription.Visible = false;
            }

            if (string.IsNullOrEmpty(source) || string.IsNullOrEmpty(module_id) || string.IsNullOrEmpty(name))
                Response.Redirect("login.aspx?type=logout");

            if (module_id != "1" && module_id != "2" && module_id != "3" && module_id != "4" && module_id != "8" && module_id != "9")
                panel_blacklist.Visible = false;

            if (!IsPostBack)
            {
                RBSearchBlack.Checked=true;
            }

            if (!model.UserAcccess(module_id, useridentity[0], name, source))
                Response.Redirect("login.aspx?type=logout");

            if (name == "GlobePH")
            {
                panel_blacklist.Visible = true;
                panelMT.Visible = true;
                panelMO.Visible = true;
                pnlDescription.Visible = true;
                //lblBlackListExample.Text = "(e.g 63127654321)";
                //lblMOExample.Text = "(e.g 63127654321)";
                //lblMTExample.Text = "(e.g 63127654321)";
                //lblSubExample.Text = "(e.g 63127654321)";
            }
            else
            {
                lblBlackListExample.Text = "(e.g 60127654321)";
                lblMOExample.Text = "(e.g 60127654321)";
                lblMTExample.Text = "(e.g 60127654321)";
                lblSubExample.Text = "(e.g 60127654321)";
            }
        }

        protected void btnSearchMT_Click(object sender, EventArgs e)
        {
            string type = string.Empty;
            if (name.ToLower() == "globe")
                type = "globe";
            else if (name.ToLower() == "globeph")
                type = "globephmt";
            else
                type = "mt";

            Search("momt.aspx", module_id, type, txtMsisdnMT.Text);
        }

        protected void btnSearchMO_Click(object sender, EventArgs e)
        {
            if (name == "GlobePH")
                Search("momt.aspx", module_id, "globeph", txtMsisdnMO.Text);
            else
                Search("momt.aspx", module_id, "mo", txtMsisdnMO.Text);
        }

        protected void btnSearchSubs_Click(object sender, EventArgs e)
        {
            Search("subscriber.aspx", module_id, "subs", txtMsisdnSubs.Text);
        }

        public void Search(string page, string module_id, string type, string msisdn)
        {
            if (!string.IsNullOrEmpty(msisdn) && msisdn != "MSISDN (e.g 60127654321)")
            {
                if (name == "GlobePH")
                {
                    if (msisdn.Substring(0, 2) != "63")
                    {
                        msisdn = msisdn.Remove(0, 1);
                        msisdn = "63" + msisdn;
                    }
                }
                else
                {
                    if (msisdn.Substring(0, 1) != "6")
                        msisdn = "6" + msisdn;
                }
                if (!string.IsNullOrEmpty(msisdn))
                    Response.Redirect(page + "?id=" + module_id + "&source=" + source + "&name=" + name + "&type=" + type + "&msisdn=" + msisdn);
            }
        }

        protected void btnBlacklist_Click(object sender, EventArgs e)
        {
            string msisdn = string.Empty;

            if (!string.IsNullOrEmpty(txtBlacklist.Text))
            {
                if (name == "GlobePH")
                {
                    msisdn = txtBlacklist.Text;
                    if (msisdn.Substring(0, 2) != "63")
                    {
                        msisdn = msisdn.Remove(0, 1);
                        msisdn = "63" + msisdn;
                    }
                }
                else if (txtBlacklist.Text.Substring(0, 1) != "6")
                    msisdn = "6" + txtBlacklist.Text;
                else
                    msisdn = txtBlacklist.Text;
            }
            else
            {
                return;
            }

            if (RBAddBlack.Checked)
            {
                if (name == "GlobePH")
                    model.AddBlaclistPH(msisdn, useridentity[0], source);
                else
                    model.AddBlaclist(msisdn, useridentity[0], model.GetTelco(module_id), source);

                lblMsgBlack.Text = "Sucessfully addded.";
            }
            else if (RBSearchBlack.Checked)
            {
                Response.Redirect("blacklist.aspx?id=" + module_id + "&source=" + source + "&name=" + name + "&type=search&msisdn=" + msisdn);
            }
        }

        protected void RBSearchBlack_CheckedChanged(object sender, EventArgs e)
        {
            if (RBSearchBlack.Checked)
            {
                btnBlacklist.Text = "Search MSISDN";
                lblMsgBlack.Text = "";
            }
        }

        protected void RBAddBlack_CheckedChanged(object sender, EventArgs e)
        {
            if (RBAddBlack.Checked)
            {
                btnBlacklist.Text = "Add MSISDN";
                lblMsgBlack.Text = "";
            }
        }
       


    }
}