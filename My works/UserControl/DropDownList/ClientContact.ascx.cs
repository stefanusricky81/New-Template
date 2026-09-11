using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SD.LLBLGen.Pro.ORMSupportClasses;
using System.Data;

public partial class UserControl_DropDownList_ClientContact : System.Web.UI.UserControl
{
    private string _defaultText = "";
    private string _defaultValue = "-1";
    private bool _displayDefaultValue = true;
    private bool _isLoaded = false;
    private int _selectedId = 0;
    private int _clientId = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack && !_isLoaded)
            Populate();
    }

    public void Populate()
    {
        if (_clientId > 0)
        {
            DataTable dtClientContact = DesktopShared.Client.GetActiveContacts(_clientId,"on");
            dtClientContact.DefaultView.Sort = "FirstName ASC";

            ddlClientContact.DataSource = dtClientContact;
            ddlClientContact.DataTextField = "FullName";
            ddlClientContact.DataValueField = "Pusers";
            ddlClientContact.DataBind();

            if (_selectedId > 0)
            {
                ListItem li = ddlClientContact.Items.FindByValue(_selectedId.ToString());
                if (li != null)
                    li.Selected = true;
            }

            if (_displayDefaultValue)
                ddlClientContact.Items.Insert(0, new ListItem(_defaultText, _defaultValue));

            _isLoaded = true;
        }
    }

    public Unit Width
    {
        set { ddlClientContact.Width = value; }
    }

    public DropDownList GetDropDownList()
    {
        return ddlClientContact;
    }

    public int SelectedClientId
    {
        set
        {
            _clientId = value;
            Populate();
        }
    }

    public int ClientContactId
    {
        get { return Convert.ToInt32(ddlClientContact.SelectedValue); }
        set
        {
            //_selectedId = value;
            //Populate();
            ListItem li = ddlClientContact.Items.FindByValue(value.ToString());
            if (li != null)
                li.Selected = true;
        }
    }

    public string ClientContactName
    {
        get { return ddlClientContact.SelectedItem.Text.Trim(); }
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
            rfvClientContact.Visible = true;
            rfvClientContact.ValidationGroup = value;
        }
    }

    public string CssClass
    {
        set { ddlClientContact.CssClass = value; }
    }
}

