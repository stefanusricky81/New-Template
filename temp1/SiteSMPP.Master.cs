using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace cs_forest
{
    public partial class SiteSMPP : System.Web.UI.MasterPage
    {
        private Model model = new Model();

        protected void Page_Load(object sender, EventArgs e)
        {
            string[] useridentity = HttpContext.Current.User.Identity.Name.Split(';');
            string page = Path.GetFileName(Request.PhysicalPath).ToLower();

            if (!model.UserAcccess("6", useridentity[0], "SMPP", "106"))
                Response.Redirect("login.aspx?type=logout");

            if (useridentity[1] != "admin")
                menu_manageuser.Visible = false;
        }

        protected void btnSearchMT_Click(object sender, EventArgs e)
        {
            Response.Redirect("smppsearchmt.aspx");
        }

        protected void btnAddRoute_Click(object sender, EventArgs e)
        {
            Response.Redirect("smppaddroute.aspx");
        }

        protected void btnRouteList_Click(object sender, EventArgs e)
        {
            Response.Redirect("smpplistroute.aspx");
        }

        protected void btnAddUser_Click(object sender, EventArgs e)
        {
            Response.Redirect("smppadduser.aspx");
        }

        protected void btnUserList_Click(object sender, EventArgs e)
        {
            Response.Redirect("smpplistuser.aspx");
        }

        protected void btnAddHttpRoute_Click(object sender, EventArgs e)
        {
            Response.Redirect("smppaddhttproute.aspx");
        }

        protected void btnRouteHttpList_Click(object sender, EventArgs e)
        {
            Response.Redirect("smpplisthttproute.aspx");
        }
    }
}