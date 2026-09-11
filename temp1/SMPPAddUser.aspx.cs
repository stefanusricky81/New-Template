using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace cs_forest
{
    public partial class SMPPAddUser : System.Web.UI.Page
    {
        private Model model = new Model();
        private string module_id = string.Empty;
        private string source = string.Empty;
        private string name = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            string type = Request.QueryString["type"] ?? string.Empty;
            string system_id = Request.QueryString["system_id"] ?? string.Empty;
            string password = Request.QueryString["password"] ?? string.Empty;
            string system_type = Request.QueryString["system_type"] ?? string.Empty;
            string allowed_id = Request.QueryString["allowed_id"] ?? string.Empty;
            module_id = Request.QueryString["id"] ?? string.Empty;
            source = Request.QueryString["source"] ?? string.Empty;
            name = Request.QueryString["name"] ?? string.Empty;

            if (!IsPostBack)
            {
                if (type.ToLower() == "update")
                {
                    btnSubmit.Text = "Update";
                    txtSystemId.Text = system_id;
                    txtPassword.Text = password;
                    txtSystemType.Text = system_type;
                    txtAllowedId.Text = allowed_id;
                }
                else
                {
                    txtSystemId.Focus();
                    btnSubmit.Text = "Create";
                }
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {

            string type = Request.QueryString["type"] ?? string.Empty;
            string id = Request.QueryString["id"] ?? string.Empty;
            if (type.ToLower() == "update")
            {
                if (!string.IsNullOrEmpty(id))
                {
                    model.UpdateSMPPAccount(id, txtSystemId.Text, txtPassword.Text, txtSystemType.Text, txtAllowedId.Text);
                    lblMessage.Text = "User detail has been successfully update.";
                }
            }
            else
            {
                model.AddSMPPAccount(txtSystemId.Text, txtPassword.Text, txtSystemType.Text, txtAllowedId.Text);
                lblMessage.Text = "User has been successfully created.";
                //Response.AddHeader("REFRESH", "5;URL=smpplistuser.aspx");
            }
        }
    }
}