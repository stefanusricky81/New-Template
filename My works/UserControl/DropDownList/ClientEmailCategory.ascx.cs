using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class UserControl_DropDownList_ClientEmailCategory : System.Web.UI.UserControl
{
    private string _available = string.Empty;
    private short _tabIndex = 10;
    private bool _displayDefaultValue = true;
    private bool _active;
    private bool _setSize = true;
    private int? _selectedId = null;
    private string _defaultText = "";
    private string _defaultValue = "";
    private string _cssClass = "form-control select-chosen";

    protected void Page_Load(object sender, EventArgs e)
    {
        DisplayChosenPluginJs();
        if (ddlClientEmailCategory.Items.Count == 0)
            PopulateDropDownList(Available,Active);
    }

    #region public methods

    /// <summary>
    /// populate drop down list
    /// </summary>
    public void PopulateDropDownList(string _available, bool _activecategory)
    {
        #region bind drop down list

        ddlClientEmailCategory.DataSource = DesktopShared.ClientEmailCategory.GetDataDropdownlist(_available, _activecategory);
        ddlClientEmailCategory.DataTextField = "CategoryName";
        ddlClientEmailCategory.DataValueField = "Id";
        ddlClientEmailCategory.DataBind();

        #endregion

        #region width / css class

        if (!String.IsNullOrEmpty(_cssClass))
            ddlClientEmailCategory.CssClass = _cssClass;

        #endregion

        #region selected item

        if (_selectedId.HasValue)
        {
            ListItem li = ddlClientEmailCategory.Items.FindByValue(_selectedId.Value.ToString());
            if (li != null)
                li.Selected = true;
        }

        #endregion

        #region show default list item

        if (_displayDefaultValue)
        {
            ListItem liDefault = new ListItem(_defaultText, _defaultValue);
            ddlClientEmailCategory.Items.Insert(0, liDefault);
        }

        #endregion

        //fixed height
        if (_setSize)
            ddlClientEmailCategory.Height = Unit.Pixel(20);
    }

    #endregion

    #region public properties

    /// <summary>
    /// set fixed size / height
    /// </summary>
    public bool SetSize
    {
        set { _setSize = value; }
    }

    /// <summary>
    /// set width for drop down list
    /// </summary>
    public Unit Width
    {
        set { ddlClientEmailCategory.Width = value; }
    }

    /// <summary>
    /// get/set ticket type id
    /// </summary>
    public int? ClientEmailCategoryId
    {
        get
        {
            int _id = 0;
            if (Int32.TryParse(ddlClientEmailCategory.SelectedValue.Trim(), out _id))
                return _id;
            else
                return (int?)null;
        }
        set
        {
            _selectedId = value;
            PopulateDropDownList(Available,Active);
        }
    }

    /// <summary>
    /// get selected ticket type name
    /// </summary>
    public string ClientEmailCategoryName
    {
        get
        {
            if (ddlClientEmailCategory.Items.Count == 0)
                return string.Empty;
            else
                return ddlClientEmailCategory.SelectedItem.Text.Trim();
        }
    }

    /// <summary>
    /// set active value
    /// </summary>
    public bool Active
    {
        get { return _active; }
        set { _active = value; }
    }

    public string Available
    {
        get { return _available; }
        set { _available = value; }
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
    /// set tab index for control
    /// </summary>
    public short TabIndex
    {
        set { _tabIndex = value; }
    }

    /// <summary>
    /// set validation group
    /// </summary>
    public string ValidationGroup
    {
        set { rfvClientEmailCategory.ValidationGroup = value; }
    }

    /// <summary>
    /// set validation error message
    /// </summary>
    public string ErrorMessage
    {
        set { rfvClientEmailCategory.ErrorMessage = value; }
    }

    /// <summary>
    /// set drop down selection is required
    /// </summary>
    public bool IsRequired
    {
        set { ddlClientEmailCategory.Visible = value; }
    }

    public bool IsEnabled
    {
        set { ddlClientEmailCategory.Enabled = value; }
    }

    /// <summary>
    /// set css class for drop down list
    /// </summary>
    public string CssClass
    {
        set
        {
            ddlClientEmailCategory.CssClass = value;
            _cssClass = value;
        }
    }

    /// <summary>
    /// get drop down list
    /// </summary>
    /// <returns></returns>
    public DropDownList GetDropDownList()
    {
        return ddlClientEmailCategory;
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
        sb.Append(String.Format("SetChosenTicketType_{0}();", this.ClientID));
        sb.Append(String.Format("Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(SetChosenTicketType_{0})", this.ClientID));
        sb.Append("});");
        sb.Append(String.Format("function SetChosenTicketType_{0}(sender, args) {{", this.ClientID));
        sb.Append(String.Format("$('#{0}').chosen({{", ddlClientEmailCategory.ClientID));
        sb.Append("allow_single_deselect: true,");
        sb.Append("placeholder_text_single: \"Select a Client Email Category ...\",");
        sb.Append("width: \"100%\"");
        sb.Append("});");
        sb.Append("}");
        sb.Append("</script>");
        litJs.Text = sb.ToString();
    }

    #endregion
}