using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace cs_forest
{
    public partial class ForgotPassword : System.Web.UI.Page
    {
        private Model model = new Model();
        private Tools tools = new Tools();

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string password = model.GetPassword(txtEmail.Text);

            string message = "Hi there,<br>";
            message += "<p>Here the login details for Customer Service Portal.</p>";
            message += "<p><b>Username : " + txtEmail.Text.Trim() + "</b><br>";
            message += "<b>Password : " + password + "</b><br>";
            message += "<p>Kindly use the login credential to access - http://210.5.41.105/csforest/.</p>";
            message += "<p>Regards,<br/><b>Customer Service Team</p>";

            if (string.IsNullOrEmpty(password))
            {
                lblMessage.ForeColor = Color.IndianRed;
                lblMessage.Text = "The entered emails is not exists.";
            }
            else
            {
                tools.SendEmail(txtEmail.Text, message, "Customer Service - Login Credential");
                lblMessage.ForeColor = Color.Green;
                lblMessage.Text = "Your copy of login credential has been sent to your email address.";
                Response.AddHeader("REFRESH", "5;URL=login.aspx");
            }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("login.aspx", true);
        }
    }
}