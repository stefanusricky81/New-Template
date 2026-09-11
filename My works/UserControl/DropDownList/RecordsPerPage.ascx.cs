using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

public partial class UserControl_DropDownList_RecordsPerPage : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    public Unit Width
    {
        set { ddlRecordsPerPage.Width  = value; }
    }

    public DropDownList GetDropDownList()
    {
        return ddlRecordsPerPage;
    }

    public int SelectedValue
    {
        get { return Convert.ToInt32(ddlRecordsPerPage.SelectedValue); }
        set
        {
            ListItem li = ddlRecordsPerPage.Items.FindByValue(value.ToString());
            if (li != null)
            {
                ddlRecordsPerPage.ClearSelection();
                li.Selected = true;
            }
        }
    }
}
