using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace cs_forest
{
    public partial class SMPPListUser : System.Web.UI.Page
    {
        private Model model = new Model();
        private string module_id = string.Empty;
        private string source = string.Empty;
        private string name = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            module_id = Request.QueryString["id"] ?? string.Empty;
            source = Request.QueryString["source"] ?? string.Empty;
            name = Request.QueryString["name"] ?? string.Empty;
            
            BindToGrid();
        }

        public void BindToGrid()
        {
            DataSet ds = model.GetSMPPUser();
            if (ds.Tables.Count > 0)
            {
                ASPxGridView1.DataSource = ds;
                ASPxGridView1.DataBind();
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            List<object> fieldValues = ASPxGridView1.GetSelectedFieldValues(new string[] { "id", "system_id" });
            foreach (object[] item in fieldValues)
            {
                model.DeleteSMPPUser(item[0].ToString());
            }
            Response.Redirect("smpplistuser.aspx");
        }
    }
}