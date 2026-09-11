using System;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace cs_forest
{
    public partial class CreateUser : System.Web.UI.Page
    {
        private Model model = new Model();
        private Tools tools = new Tools();
        private int n;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.BindModules();
                if (Request.QueryString["type"] == "update")
                {
                    this.GetCurrentModules(Request.QueryString["userid"]);
                    lblHeader.Text = "Update Roles";
                    DDLUser.SelectedValue = Request.QueryString["role"] ?? string.Empty;
                    txtEmail.Text = Request.QueryString["email"] ?? string.Empty;
                    txtPassword.Attributes.Add("value", Request.QueryString["password"] ?? string.Empty);
                    DDLUser.Enabled = false;
                    txtEmail.Enabled = false;
                    txtPassword.Enabled = false;
                    if (DDLUser.SelectedValue == "Admin")
                    {
                        CHKModule.Enabled = false;
                        for (int i = 0; i < CHKModule.Items.Count; i++)
                            CHKModule.Items[i].Selected = true;
                    }
                }
                else
                {
                    lblHeader.Text = "Create User";
                }
            }
        }

        protected void BtnSubmit_Click(object sender, EventArgs e)
        {
            string userid = string.Empty;

            if (Request.QueryString["type"] != "update")
                userid = model.CreateUser(txtEmail.Text, txtPassword.Text, DDLUser.SelectedValue, txtEmail.Text);
            else
                userid = Request.QueryString["userid"];

            bool isNumeric = int.TryParse(userid, out n);

            if (isNumeric)
            {
                for (int i = 0; i < CHKModule.Items.Count; i++)
                {
                    if (CHKModule.Items[i].Selected)
                        model.AddUserAccess(userid, CHKModule.Items[i].Value, i);
                }

                string message = "Hi there,<br>";

                if (Request.QueryString["type"] != "update")
                {
                    message += "<p>Your login credential for Customer Service Portal has successfully been created.</p>";
                    message += "<p><b>Username : " + txtEmail.Text.Trim() + "</b><br>";
                    message += "<b>Password : " + txtPassword.Text.Trim() + "</b><br>";
                    message += "<b>Role : " + DDLUser.SelectedValue + "</b><br>";
                    message += "<b>Email Password Recovery : " + txtEmail.Text.Trim() + "</b></p>";
                    message += "<p>Kindly use the login credential to access - http://210.5.41.105/csforest/.</p>";
                    message += "<p>Regards,<br/><b>Customer Service Team</p>";
                    tools.SendEmail(txtEmail.Text.Trim(), message, "Login Credential - Customer Service");
                }

                Response.Redirect("manageuser.aspx");
            }
            else
                lblMessage.Text = "Error: " + userid;
        }

        public void BindModules()
        {
            string strSql = "SELECT [Id],[Name],[Details],[Logo] FROM [Modules]";
            try
            {
                using (SqlConnection cn = new SqlConnection(model.ConnStr("105", "CustomerService")))
                {
                    cn.Open();
                    SqlCommand cmd = new SqlCommand(strSql, cn);
                    using (SqlDataReader rd = cmd.ExecuteReader())
                    {
                        while (rd.Read())
                        {
                            ListItem item = new ListItem();
                            item.Text = rd["Details"].ToString() + " " + rd["Name"].ToString();
                            item.Value = rd["Id"].ToString();
                            CHKModule.Items.Add(item);
                        }
                    }
                }
            }
            catch (SqlException ex)
            {

            }
        }

        protected void CHKModule_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DDLUser.SelectedValue == "Telco")
            {
                int count = 0;
                for (int i = 0; i < CHKModule.Items.Count; i++)
                {
                    if (CHKModule.Items[i].Selected)
                    {
                        count += 1;
                        if (count > 1)
                            CHKModule.Items[i].Selected = false;
                    }
                }
            }
        }

        protected void DDLUser_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DDLUser.SelectedValue == "Admin")
            {
                CHKModule.Enabled = false;
                for (int i = 0; i < CHKModule.Items.Count; i++)
                    CHKModule.Items[i].Selected = true;
            }
            else
            {
                CHKModule.Enabled = true;
                for (int i = 0; i < CHKModule.Items.Count; i++)
                    CHKModule.Items[i].Selected = false;
            }
            
        }

        public void GetCurrentModules(string userid)
        {
            for (int i = 0; i < CHKModule.Items.Count; i++)
                CHKModule.Items[i].Selected = false;

            string strSql = "SELECT [UserId],[Modules] FROM [UserAccess] WHERE [UserId]=" + userid;
            try
            {
                using (SqlConnection cn = new SqlConnection(model.ConnStr("105", "CustomerService")))
                {
                    cn.Open();
                    SqlCommand cmd = new SqlCommand(strSql, cn);
                    using (SqlDataReader rd = cmd.ExecuteReader())
                    {
                        while (rd.Read())
                        {
                            for (int i = 0; i < CHKModule.Items.Count; i++)
                            {
                                if (CHKModule.Items[i].Value == rd["Modules"].ToString())
                                    CHKModule.Items[i].Selected = true;
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {

            }
        }
    }
}