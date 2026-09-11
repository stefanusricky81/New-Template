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
using SD.LLBLGen.Pro.ORMSupportClasses;

public partial class UserControl_DropDownList_TicketStatus : System.Web.UI.UserControl
{
    private string _defaultText = "All";
    private string _defaultValue = "-1";
    private bool _displayDefaultValue = true;
    private bool _isLoaded = false;
    private int _selectedId = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack && !_isLoaded)
            Populate();
    }

    public void Populate()
    {
        DesktopShared.CollectionClasses.CscFieldsCollection statuses =
            DesktopShared.Ticket.CscField.GetStatuses();

        ddlTicketStatus.DataSource = statuses;
        ddlTicketStatus.DataTextField = "Name";
        ddlTicketStatus.DataValueField = "Pcscfields";
        ddlTicketStatus.DataBind();

        if (_selectedId > 0)
        {
            ListItem li = ddlTicketStatus.Items.FindByValue(_selectedId.ToString());
            if (li != null)
                li.Selected = true;
        }
        
        if (_displayDefaultValue)
            ddlTicketStatus.Items.Insert(0, new ListItem(_defaultText, _defaultValue));

        _isLoaded = true;
    }

    public Unit Width
    {
        set { ddlTicketStatus.Width = value; }
    }

    public DropDownList GetDropDownList()
    {
        return ddlTicketStatus;
    }

    public int StatusId
    {
        get { return Convert.ToInt32(ddlTicketStatus.SelectedValue); }
        set
        {
            _selectedId = value;
            Populate();
        }
    }

    public string DefaultText
    {
        set { _defaultText = value; }
    }

    public string DefaultValue
    {
        set { _defaultValue = value; }
    }

    public bool DisplayDefaultValue
    {
        set { _displayDefaultValue = value; }
    }

    public string ValidationGroup
    {
        set
        {
            rfvStatus.Visible = true;
            rfvStatus.ValidationGroup = value;
        }
    }

    public string CssClass
    {
        set { ddlTicketStatus.CssClass = value; }
    }
}
