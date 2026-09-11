using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace cs_forest
{
    public partial class Login : System.Web.UI.Page
    {
        Model model = new Model();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                FormsAuthentication.SignOut();
                if (Request.QueryString["type"] == "logout")
                {
                    Session.Clear();
                    Response.Cookies.Clear();
                    Response.Redirect("login.aspx");
                }
                if (Request.QueryString["ReturnUrl"] != null || Request.QueryString["aspxerrorpath"] != null)
                {
                    Response.Redirect("login.aspx");
                }
            }
        }

        protected void BtnLogin_Click(object sender, EventArgs e)
        {
            string[] login_details = model.Login(txtUsername.Text.Trim(), txtPassword.Text.Trim());
            string status = login_details[0];
            string roles = login_details[1];
            string locked = login_details[2];
            string userid = login_details[3];

            if (status == "success")
            {
                if (locked == "True")
                {
                    lblMessage.Text = "Your account has been locked.";
                    return;
                }


                model.UpdateLogin(txtUsername.Text, 0);
                //Application["roles"] = roles;
                FormsAuthentication.SetAuthCookie(userid + ";" + roles, true);

                if (roles == "ocs" || roles == "admin")
                    Response.Redirect("menu.aspx");
                else if (roles == "telco")
                {
                    string[] menu = model.GetTelcoMenu(userid);
                    if (string.IsNullOrEmpty(menu[0]))
                        lblMessage.Text = "Your account is not assign to any modules.";
                    else
                        Response.Redirect("menu.aspx"); //Response.Redirect("dashboard.aspx?id=" + menu[0] + "&source=" + menu[2] + "&name=" + menu[1]);
                }
            }
            else
            {
                lblMessage.Text = "The username or password you entered is incorrect.";
                model.UpdateLogin(txtUsername.Text, 1);
            }
        }

        protected void BtnForgot_Click(object sender, EventArgs e)
        {
            Response.Redirect("forgotpassword.aspx", true);
        }
    }
}