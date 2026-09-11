using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace cs_forest
{
    public partial class SMPPAddHttpRoute : System.Web.UI.Page
    {
        private Model model = new Model();
        private string userid = string.Empty;
        private string api = string.Empty;
        private string prefix = string.Empty;
        private string oprator = string.Empty;
        private string country = string.Empty;
        private string route = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            userid = Request.QueryString["userid"] ?? string.Empty;
            api = Request.QueryString["api"] ?? string.Empty;
            prefix = Request.QueryString["prefix"] ?? string.Empty;
            oprator = Request.QueryString["oprator"] ?? string.Empty;
            country = Request.QueryString["country"] ?? string.Empty;
            route = Request.QueryString["route"] ?? string.Empty;

            if (!IsPostBack)
            {
                BindToSystemUser();
                txtCountry.Text = country;
                txtOperator.Text = oprator;
                DDLHTTPApi.SelectedValue = api;
                DDLSystemId.SelectedValue = userid;
                txtPrefix.Text = prefix;
                txtRoute.Text = route;
            }

            if (Request.QueryString["type"] == "update")
                btnSubmit.Text = "Update Route";
            else
            {
                btnSubmit.Text = "Add Route";
                txtRoute.Focus();
            }
        }

        public void BindToSystemUser()
        {
            string strSql = "select distinct system_id from smpp_info";

            try
            {
                using (MySqlConnection connection = new MySqlConnection("Server=210.5.41.112;userid=engrjahed;password=engrjahed123;Database=kannel"))
                {
                    connection.Open();
                    MySqlCommand cmd = new MySqlCommand(strSql, connection);
                    using (MySqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                            DDLSystemId.Items.Add(dr["system_id"].ToString().Trim());
                        dr.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string id = Request.QueryString["id"] ?? string.Empty;
            if (Request.QueryString["type"] == "update")
            {
                model.UpdateSMPPHttpRoute(id, txtRoute.Text, DDLSystemId.SelectedValue, DDLHTTPApi.SelectedValue, txtPrefix.Text, txtOperator.Text, txtCountry.Text);
                lblMessage.Text = "Route has been successfully update.";
            }
            else
            {
                model.AddSMPPHttpRoute(txtRoute.Text, DDLSystemId.SelectedValue, DDLHTTPApi.SelectedValue, txtPrefix.Text, txtOperator.Text, txtCountry.Text);
                lblMessage.Text = "New route has been successfully created.";
            }


        }
    }
}