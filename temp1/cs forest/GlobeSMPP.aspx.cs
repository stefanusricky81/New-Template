using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace cs_forest
{
    public partial class GlobeSMPP : System.Web.UI.Page
    {
        private Model model = new Model();

        protected void Page_Load(object sender, EventArgs e)
        {
            string type = Request.QueryString["type"];
            string msisdn = Request.QueryString["value"];
            BindToGridLog(type,msisdn);
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
    }
}