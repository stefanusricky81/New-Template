using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SD.LLBLGen.Pro.ORMSupportClasses;
using System.Data;

public partial class UserControl_DropDownList_ClientContactResponsive : System.Web.UI.UserControl
{
    private bool _displayDefaultValue = true;
    private bool? _active = null;
    private bool _useEmailAsDataValue = false;
    private bool _useClientContactIdAsDataValue = false;
    private int? _selectedId = null;
    private string _defaultText = "";
    private string _defaultValue = "";
    private string _cssClass = "";

    /// <summary>
    /// page load
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        DisplayChosenPluginJs();
        if (ddlClientContact.Items.Count == 0)
            PopulateDropDownList();
    }

    #region public methods

    /// <summary>
    /// populdat drop down list
    /// </summary>
    public void PopulateDropDownList()
    {
        if (ClientId.HasValue)
        {
            DataTable dtClientContact = DesktopShared.Client.GetActiveContacts(ClientId.Value, IsSilent);
            //DataTable dtClientContact = DesktopShared.Client.GetActiveContacts(ClientId.Value);
            dtClientContact.DefaultView.Sort = "FirstName ASC";

            ddlClientContact.DataSource = dtClientContact;
            //ddlClientContact.DataTextField = "FullName";
            ddlClientContact.DataTextField = "FullNameVIP";
            if (_useEmailAsDataValue)
                ddlClientContact.DataValueField = "Email";
            else if (_useClientContactIdAsDataValue)
                ddlClientContact.DataValueField = "PclientContact";
            else
                ddlClientContact.DataValueField = "Pusers";
            ddlClientContact.DataBind();
            
            //select item
            if (_selectedId.HasValue)
            {
                ListItem li = ddlClientContact.Items.FindByValue(_selectedId.Value.ToString());
                if (li != null)
                    li.Selected = true;
            }

            //show default list item
            if (_displayDefaultValue)
                ddlClientContact.Items.Insert(0, new ListItem(_defaultText, _defaultValue));


            //width / css class
            if (!String.IsNullOrEmpty(_cssClass))
                ddlClientContact.CssClass = _cssClass;
        }
    }

    #endregion

    #region public properties

    /// <summary>
    /// get/set client id
    /// </summary>
    public int? ClientId
    {
        get
        {
            object obj = this.ViewState["cid_ccr"];
            return (obj == null) ? (int?)null : (int)obj;
        }
        set { this.ViewState["cid_ccr"] = value; }
    }

    public string IsSilent
    {
        get
        {
            object obj = this.ViewState["cc_slnt"];
            return (obj == null) ? "" : (string)obj;
        }
        set { this.ViewState["cc_slnt"] = value; }
    }

    /// <summary>
    /// get/set client contact id
    /// </summary>
    public int? ClientContactId
    {
        get
        {
            if (String.IsNullOrWhiteSpace(ddlClientContact.SelectedValue))
                return null;

            int _id = 0;
            if (Int32.TryParse(ddlClientContact.SelectedValue.Trim(), out _id))
                return _id;
            else
                return (int?)null;
        }
        set { _selectedId = value; }
    }

    /// <summary>
    /// get selected value
    /// </summary>
    public string SelectedValue
    {
        get { return ddlClientContact.SelectedValue; }
    }

    /// <summary>
    /// get client contact name
    /// </summary>
    public string ClientContactName
    {
        get { return ddlClientContact.SelectedItem.Text.Trim(); }
    }

    /// <summary>
    /// set text for default list item
    /// </summary>
    public string DefaultText
    {
        set { _defaultText = value; }
    }

    /// <summary>
    /// set value for default list item
    /// </summary>
    public string DefaultValue
    {
        set { _defaultValue = value; }
    }

    /// <summary>
    /// set dipslay default list item
    /// </summary>
    public bool DisplayDefaultValue
    {
        set { _displayDefaultValue = value; }
    }

    /// <summary>
    /// set use employee email as data value
    /// </summary>
    public bool UseEmailAsDataValue
    {
        set { _useEmailAsDataValue = value; }
    }

    /// <summary>
    /// set use client contact id as data value
    /// </summary>
    public bool UseClientContactIdAsDataValue
    {
        set { _useClientContactIdAsDataValue = value;  }
    }

    /// <summary>
    /// set tab index for control
    /// </summary>
    public short TabIndex
    {
        set { ddlClientContact.TabIndex = value; }
    }

    /// <summary>
    /// set validation group
    /// </summary>
    public string ValidationGroup
    {
        set { rfvClientContact.ValidationGroup = value; }
    }

    /// <summary>
    /// set validation error message
    /// </summary>
    public string ErrorMessage
    {
        set { rfvClientContact.ErrorMessage = value; }
    }

    /// <summary>
    /// set drop down selection is required
    /// </summary>
    public bool IsRequired
    {
        set { rfvClientContact.Visible = value; }
    }

    /// <summary>
    /// set css class for drop down list
    /// </summary>
    public string CssClass
    {
        set { _cssClass = value; } // ddlClientContact.CssClass = value; }
    }

    /// <summary>
    /// get drop down list
    /// </summary>
    /// <returns></returns>
    public DropDownList GetDropDownList()
    {
        return ddlClientContact;
    }

    /// <summary>
    /// set show chosen script
    /// </summary>
    public bool DisplayChosenScript
    {
        set { litJs.Visible = value; }
    }

    #endregion

    #region private methods

    /// <summary>
    /// display script used by chosen plugin
    /// </summary>
    private void DisplayChosenPluginJs()
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        sb.Append("<script type='text/javascript'>");
        sb.Append("$(document).ready(function() {");
        sb.Append(String.Format("SetChosenClientContact_{0}();", this.ClientID));
        sb.Append(String.Format("Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(SetChosenClientContact_{0})", this.ClientID));
        sb.Append("});");
        sb.Append(String.Format("function SetChosenClientContact_{0}(sender, args) {{", this.ClientID));
        sb.Append(String.Format("$('#{0}').chosen({{", ddlClientContact.ClientID));
        sb.Append("allow_single_deselect: true,");
        sb.Append("placeholder_text_single: \"Select a Contact ...\",");
        sb.Append("width: \"100%\"");
        sb.Append("});");
        sb.Append("}");
        sb.Append("</script>");
        litJs.Text = sb.ToString();
    }

    #endregion
}