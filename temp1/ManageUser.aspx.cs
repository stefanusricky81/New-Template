using DevExpress.Utils;
using DevExpress.Web;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace cs_forest
{
    public partial class ManageUser : System.Web.UI.Page
    {
        private Model model = new Model();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                
            }
        }

        protected void btnCreateUser_Click(object sender, EventArgs e)
        {
            Response.Redirect("createuser.aspx");
        }

        protected void btnUserActivity_Click(object sender, EventArgs e)
        {
            Response.Redirect("useractivity.aspx");
        }

        protected void ASPxGridView1_CustomButtonCallback(object sender, DevExpress.Web.ASPxGridViewCustomButtonCallbackEventArgs e)
        {
            int userid = (int)ASPxGridView1.GetRowValues(e.VisibleIndex, ASPxGridView1.KeyFieldName);

            if (e.ButtonID == "btnLock")
                model.UpdateLoginStatus(userid, "True");
            else if (e.ButtonID == "btnUnlock")
                model.UpdateLoginStatus(userid, "False");
            ASPxGridView1.DataBind();
        }

        protected void ASPxGridView1_CustomButtonInitialize(object sender, ASPxGridViewCustomButtonEventArgs e)
        {
            if (e.VisibleIndex == -1) return;

            if (e.ButtonID == "btnLock")
            {
                if (((ASPxGridView)sender).GetRowValues(e.VisibleIndex, "Locked").ToString() == "Lock")
                    e.Visible = DefaultBoolean.False;
            }

            if (e.ButtonID == "btnUnlock")
            {
                if (((ASPxGridView)sender).GetRowValues(e.VisibleIndex, "Locked").ToString() == "Active")
                    e.Visible = DefaultBoolean.False;
            }
        }
    }
}