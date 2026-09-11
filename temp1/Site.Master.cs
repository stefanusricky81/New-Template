using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace cs_forest
{
    public partial class Site : System.Web.UI.MasterPage
    {
        private Model model = new Model();
        private string module_id = string.Empty;
        private string source = string.Empty;
        private string name = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!string.IsNullOrEmpty(HttpContext.Current.User.Identity.Name))
            {
                string[] useridentity = HttpContext.Current.User.Identity.Name.Split(';');
                string page = Path.GetFileName(Request.PhysicalPath).ToLower();
                module_id = Request.QueryString["id"] ?? string.Empty;
                source = Request.QueryString["source"] ?? string.Empty;
                name = Request.QueryString["name"] ?? string.Empty;

                if (page == "menu.aspx" || page == "changepassword.aspx")
                {
                    menu_manageuser.Visible = false;
                    menu_dashboard.Visible = false;
                    menu_home.Visible = false;
                }
                else if (page == "createuser.aspx" || page == "manageuser.aspx" || page == "useractivity.aspx")
                {
                    if (useridentity[1] != "admin")
                        Response.Redirect("login.aspx?type=logout");
                    menu_dashboard.Visible = false;
                }
                else if (page == "manual.aspx")
                {
                    menu_manageuser.Visible = false;
                    menu_dashboard.Visible = false;
                    menu_home.Visible = true;
                }
                else
                {
                    menu_manageuser.Visible = false;
                    if (!model.UserAcccess(module_id, useridentity[0], name, source))
                        Response.Redirect("login.aspx?type=logout");
                }

                if (useridentity[1] == "admin" && page != "dashboard.aspx" && page != "momt.aspx" && page != "blacklist.aspx" && page != "subscriber.aspx" && page != "managesmppaccount.aspx" && page != "createsmppaccount.aspx" && page != "addsmpproute.aspx")
                {
                    menu_manageuser.Visible = true;
                }

                if (useridentity[1] == "ocs")
                    model.AddActivity(useridentity[0], Request.RawUrl);
            }
            else
            {
                Response.Redirect("login.aspx?type=logout");
            }
        }
    }
}