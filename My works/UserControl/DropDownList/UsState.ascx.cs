using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class UserControl_DropDownList_UsState : System.Web.UI.UserControl
{
    private int _ddWidth = 0;
    private short _tabIndex = 10;
    private bool _showDefault = false;
    private bool? _active = true;
    private int? _selectedId = null;
    private string _defaultValue = "";
    private string _defaultText = "";
    private string _cssClass = "";
    private string _chosentPlaceholderText = "Select a State ...";

    /// <summary>
    /// page load
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        DisplayChosenPluginJs();
        if (ddlUsState.Items.Count == 0)
            PopulateDropDownList();
    }

    #region public methods

    /// <summary>
    /// bind UsState drop down list
    /// </summary>
    public void PopulateDropDownList()
    {
        //clear items
        ddlUsState.ClearSelection();
        ddlUsState.Items.Clear();

        //tab index
        ddlUsState.TabIndex = _tabIndex;

        #region bind drop down list

        ddlUsState.DataSource = DesktopShared.UsState.Get();
        ddlUsState.DataTextField = "Name";
        ddlUsState.DataValueField = "Pstate";
        ddlUsState.DataBind();
        ddlUsState.DataSource = null;

        #endregion

        #region width / css class

        if (_ddWidth > 0)
            ddlUsState.Width = _ddWidth;
        if (!String.IsNullOrEmpty(_cssClass))
            ddlUsState.CssClass = _cssClass;

        #endregion

        #region selected item

        if (_selectedId.HasValue)
        {
            ListItem li = ddlUsState.Items.FindByValue(_selectedId.Value.ToString());
            if (li != null)
                li.Selected = true;
        }

        #endregion

        #region show default list item

        if (_showDefault)
        {
            ListItem liDefault = new ListItem(_defaultText, _defaultValue);
            ddlUsState.Items.Insert(0, liDefault);
            if (!_selectedId.HasValue)
                liDefault.Selected = true;
        }

        #endregion
    }

    #endregion

    #region public properties

    /// <summary>
    /// get/set selected us state ID
    /// </summary>
    /*public int? UsStateId
    {
        get
        {
            int _id = 0;
            if (Int32.TryParse(ddlUsState.SelectedValue.Trim(), out _id))
                return _id;
            else
                return (int?)null;
        }
        set
        {
            _selectedId = value;
            if (value.HasValue)
            {
                ListItem _li = ddlUsState.Items.FindByValue(value.Value.ToString());
                if (_li != null)
                {
                    ddlUsState.ClearSelection();
                    _li.Selected = true;
                    return;
                }
            }
            PopulateDropDownList();
        }
    }*/

    /// <summary>
    /// get/set selected us state code
    /// </summary>
    public string UsStateCode
    {
        get { return ddlUsState.SelectedValue.Trim(); }
        set
        {
            ListItem _li = ddlUsState.Items.FindByValue(value.Trim());
            if (_li != null)
            {
                ddlUsState.ClearSelection();
                _li.Selected = true;
                return;
            }
            PopulateDropDownList();
        }
    }

    /// <summary>
    /// get selected us state name
    /// </summary>
    public string UsStateName
    {
        get { return ddlUsState.SelectedItem.Text; }
    }

    /// <summary>
    /// set tab index for control
    /// </summary>
    public short TabIndex
    {
        set { _tabIndex = value; }
    }

    /// <summary>
    /// set default value for list item
    /// </summary>
    public string DefaultValue
    {
        set { _defaultValue = value; }
    }

    /// <summary>
    /// set default text for list item
    /// </summary>
    public string DefaultText
    {
        set { _defaultText = value; }
    }

    /// <summary>
    /// set display default list item
    /// </summary>
    public bool ShowDefaultEntry
    {
        set { _showDefault = value; }
    }

    /// <summary>
    /// set css class of drop down list
    /// </summary>
    public string CssClass
    {
        set { _cssClass = value; }
    }

    /// <summary>
    /// set width of drop down list
    /// </summary>
    public int ControlWidth
    {
        set { _ddWidth = value; }
    }

    /// <summary>
    /// set validation group
    /// </summary>
    public string ValidationGroup
    {
        set { rfvUsState.ValidationGroup = value; }
    }

    /// <summary>
    /// set drop down selection is required
    /// </summary>
    public bool IsRequired
    {
        set { rfvUsState.Visible = value; }
    }

    /// <summary>
    /// set validation error message
    /// </summary>
    public string ErrorMessage
    {
        set { rfvUsState.ErrorMessage = value; }
    }

    /// <summary>
    /// set display asterisk for error
    /// </summary>
    public bool DisplayErrorAsterisk
    {
        set { litError.Visible = value; }
    }

    /// <summary>
    /// set validator display type
    /// </summary>
    public ValidatorDisplay validatorDisplay
    {
        set { rfvUsState.Display = value; }
    }

    /// <summary>
    /// set validator fore color
    /// </summary>
    public System.Drawing.Color ValidatorForeColor
    {
        set { rfvUsState.ForeColor = value; }
    }

    /// <summary>
    /// set active status of collection
    /// </summary>
    public bool? Active
    {
        set { _active = value; }
    }
    /// <summary>
    /// get drop down list control
    /// </summary>
    /// <returns></returns>
    public DropDownList GetDropDownList()
    {
        return ddlUsState;
    }

    /// <summary>
    /// get bool of whether drop down list has any records
    /// </summary>
    public bool HasRecords
    {
        get
        {
            int _count = _showDefault ? 1 : 0;
            return ddlUsState.Items.Count > _count;
        }
    }

    /// <summary>
    /// set drop down list enabled
    /// </summary>
    public bool Enabled
    {
        set { ddlUsState.Enabled = value; }
    }

    /// <summary>
    /// set text to use for placeholder for chosen pluging
    /// </summary>
    public string ChosentPlaceholderText
    {
        set { _chosentPlaceholderText = value; }
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
        sb.Append(String.Format("SetChosenUsState_{0}();", this.ClientID));
        sb.Append(String.Format("Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(SetChosenUsState_{0})", this.ClientID));
        sb.Append("});");
        sb.Append(String.Format("function SetChosenUsState_{0}(sender, args) {{", this.ClientID));
        sb.Append(String.Format("$('#{0}').chosen({{", ddlUsState.ClientID));
        sb.Append("allow_single_deselect: true,");
        sb.Append(String.Format("placeholder_text_single: \"{0}\",", _chosentPlaceholderText));
        sb.Append("width: \"100%\"");
        sb.Append("});");
        sb.Append("}");
        sb.Append("</script>");
        litJs.Text = sb.ToString();
    }

    #endregion
}