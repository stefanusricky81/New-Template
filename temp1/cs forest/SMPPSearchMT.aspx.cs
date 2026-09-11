using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace cs_forest
{
    public partial class SMPPSearchMT : System.Web.UI.Page
    {
        private Model model = new Model();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                RBMsisdn.Checked = true;
                if (RBMsisdn.Checked)
                {
                    txtGlobeMsgId.Visible = false;
                    txtGlobeMsisdn.Visible = true;
                }
            }

            string type = Request.QueryString["type"] ?? string.Empty;
            string msisdn = Request.QueryString["value"] ?? string.Empty;
            if (!string.IsNullOrEmpty(type) && !string.IsNullOrEmpty(msisdn))
                BindToGridLog(type, msisdn);
        }

        public void BindToGridLog(string type, string value)
        {
            DataSet mysql = model.MySQLGlobe(type, value);
            if (mysql.Tables.Count > 0)
            {
                ASPxGridView1.DataSource = mysql;
                ASPxGridView1.DataBind();
            }
        }

        protected void btnSearchMysql_Click(object sender, EventArgs e)
        {
            SearchMT();
        }

        public void SearchMT()
        {
            if (RBMessageId.Checked == true && txtGlobeMsgId.Text != "Message Id (e.g 8hsg6ak3)")
                Response.Redirect("smppsearchmt.aspx?type=msgid&value=" + txtGlobeMsgId.Text);

            if (RBMsisdn.Checked == true && txtGlobeMsisdn.Text != "MSISDN (e.g 60127654321)")
                Response.Redirect("smppsearchmt.aspx?type=msisdn&value=" + txtGlobeMsisdn.Text);
        }

        protected void RBMsisdn_CheckedChanged(object sender, EventArgs e)
        {
            if (RBMsisdn.Checked)
            {
                txtGlobeMsgId.Visible = false;
                txtGlobeMsisdn.Visible = true;
            }
        }

        protected void RBMessageId_CheckedChanged(object sender, EventArgs e)
        {
            if (RBMessageId.Checked)
            {
                txtGlobeMsgId.Visible = true;
                txtGlobeMsisdn.Visible = false;
            }
        }

    }
}