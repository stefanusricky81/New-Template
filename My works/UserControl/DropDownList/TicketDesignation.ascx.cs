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

public partial class UserControl_DropDownList_TicketDesignation : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        
    }

    public Unit Width
    {
        set { ddlTicketDesignation.Width = value; }
    }

    public DropDownList GetDropDownList()
    {
        return ddlTicketDesignation;
    }

    public string Designation
    {
        get { return ddlTicketDesignation.SelectedValue; }
        set
        {
            ListItem li = ddlTicketDesignation.Items.FindByValue(value);
            if (li != null)
            {
                ddlTicketDesignation.ClearSelection();
                li.Selected = true;
            }
        }
    }

    public string ValidationGroup
    {
        set
        {
            rfvDesignation.Visible = true;
            rfvDesignation.ValidationGroup = value;
        }
    }

    public string CssClass
    {
        set { ddlTicketDesignation.CssClass = value; }
    }
}
