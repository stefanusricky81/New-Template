using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace cs_forest
{
    public partial class ChangePassword : System.Web.UI.Page
    {
        private Model model = new Model();
        private string[] useridentity = HttpContext.Current.User.Identity.Name.Split(';');

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void BtnSubmit_Click(object sender, EventArgs e)
        {
            string password = model.UpdatePassword(txtCurrentPassword.Text,txtNewPassword.Text,useridentity[0]);
            if (string.IsNullOrEmpty(password))
            {
                lblMessage.Text = "Please check you current password.";
                lblMessage.ForeColor = Color.IndianRed;
            }
            else
            {
                lblMessage.Text = "Your password has been updated.";
                lblMessage.ForeColor = Color.Green;
            }
        }
    }
}