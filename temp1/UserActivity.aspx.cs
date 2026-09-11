using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace cs_forest
{
    public partial class UserActivity : System.Web.UI.Page
    {
        private Model model = new Model();
        private string[] useridentity = HttpContext.Current.User.Identity.Name.Split(';');
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ddlUser();
                DDLDateFrom.Date = Convert.ToDateTime(DateTime.Now.ToString("M/d/yyyy"));
                DDLDateTo.Date = Convert.ToDateTime(DateTime.Now.ToString("M/d/yyyy"));
            }
        }

        protected void BtnSubmit_Click(object sender, EventArgs e)
        {
            DateTime date_from = DateTime.ParseExact(DDLDateFrom.Value.ToString(), "M/d/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture);
            DateTime date_to = DateTime.ParseExact(DDLDateTo.Value.ToString(), "M/d/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture);

            DataSet activity = model.SearchActivity(DDLUser.SelectedValue, date_from, date_to);

            if (activity.Tables.Count > 0)
            {
                ASPxGridView1.DataSource = activity;
                ASPxGridView1.DataBind();
            }
        }

        protected void ddlUser()
        {
            string role = useridentity[1].ToString();
            DataSet ddluser = model.ddlUser(role);
            DDLUser.DataSource = ddluser;

            DDLUser.DataTextField = "Username";
            DDLUser.DataValueField = "UserId";
            DDLUser.DataBind();
            DDLUser.Items.Insert(0, "Select User");
        }


    }
}