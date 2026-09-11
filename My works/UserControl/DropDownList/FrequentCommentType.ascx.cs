using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class UserControl_DropDownList_FrequentCommentType : System.Web.UI.UserControl
{
    private int _ddWidth = 0;
    private short _tabIndex = 10;
    private bool _showDefault = false;
    private int? _selectedId = null;
    private string _defaultValue = "";
    private string _defaultText = "";
    private string _cssClass = "";

    /// <summary>
    /// page load
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        DisplayChosenPluginJs();
        if (ddlFrequentCommentType.Items.Count == 0)
            PopulateDropDownList();
    }

    #region public methods

    /// <summary>
    /// bind FrequentCommentType drop down list
    /// </summary>
    public void PopulateDropDownList()
    {
        //clear items
        ddlFrequentCommentType.ClearSelection();
        ddlFrequentCommentType.Items.Clear();

        //tab index
        ddlFrequentCommentType.TabIndex = _tabIndex;

        #region bind drop down list

        ddlFrequentCommentType.DataSource = DesktopShared.FrequentComment.Type.GetActive();
        ddlFrequentCommentType.DataTextField = "Name";
        ddlFrequentCommentType.DataValueField = "Id";
        ddlFrequentCommentType.DataBind();
        ddlFrequentCommentType.DataSource = null;

        #endregion

        #region width / css class

        if (_ddWidth > 0)
            ddlFrequentCommentType.Width = _ddWidth;
        if (!String.IsNullOrEmpty(_cssClass))
            ddlFrequentCommentType.CssClass = _cssClass;

        #endregion

        #region selected item

        if (_selectedId.HasValue)
        {
            ListItem li = ddlFrequentCommentType.Items.FindByValue(_selectedId.Value.ToString());
            if (li != null)
                li.Selected = true;
        }

        #endregion

        #region show default list item

        if (_showDefault)
        {
            ListItem liDefault = new ListItem(_defaultText, _defaultValue);
            ddlFrequentCommentType.Items.Insert(0, liDefault);
            if (!_selectedId.HasValue)
                ddlFrequentCommentType.SelectedIndex = 0;
        }

        #endregion
    }

    #endregion

    #region public properties

    /// <summary>
    /// get/set selected frequent comment type ID
    /// </summary>
    public int? FrequentCommentTypeId
    {
        get
        {
            int _id = 0;
            if (Int32.TryParse(ddlFrequentCommentType.SelectedValue.Trim(), out _id))
                return _id;
            else
                return (int?)null;
        }
        set
        {
            _selectedId = value;
            PopulateDropDownList();
        }
    }

    /// <summary>
    /// get selected frequent comment type name
    /// </summary>
    public string FrequentCommentTypeName
    {
        get { return ddlFrequentCommentType.SelectedItem.Text; }
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
        set { rfvFrequentCommentType.ValidationGroup = value; }
    }

    /// <summary>
    /// set drop down selection is required
    /// </summary>
    public bool IsRequired
    {
        set { rfvFrequentCommentType.Visible = value; }
    }

    /// <summary>
    /// set validation error message
    /// </summary>
    public string ErrorMessage
    {
        set { rfvFrequentCommentType.ErrorMessage = value; }
    }

    /// <summary>
    /// set validator display type
    /// </summary>
    public ValidatorDisplay validatorDisplay
    {
        set { rfvFrequentCommentType.Display = value; }
    }

    /// <summary>
    /// set validator fore color
    /// </summary>
    public System.Drawing.Color ValidatorForeColor
    {
        set { rfvFrequentCommentType.ForeColor = value; }
    }

    /// <summary>
    /// get drop down list control
    /// </summary>
    /// <returns></returns>
    public DropDownList GetDropDownList()
    {
        return ddlFrequentCommentType;
    }

    /// <summary>
    /// get bool of whether drop down list has any records
    /// </summary>
    public bool HasRecords
    {
        get
        {
            int _count = _showDefault ? 1 : 0;
            return ddlFrequentCommentType.Items.Count > _count;
        }
    }

    /// <summary>
    /// set drop down list enabled
    /// </summary>
    public bool Enabled
    {
        set { ddlFrequentCommentType.Enabled = value; }
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
        sb.Append(String.Format("SetChosenFrequentCommentType_{0}();", this.ClientID));
        sb.Append(String.Format("Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(SetChosenFrequentCommentType_{0})", this.ClientID));
        sb.Append("});");
        sb.Append(String.Format("function SetChosenFrequentCommentType_{0}(sender, args) {{", this.ClientID));
        sb.Append(String.Format("$('#{0}').chosen({{", ddlFrequentCommentType.ClientID));
        sb.Append("allow_single_deselect: true,");
        sb.Append(String.Format("placeholder_text_single: \"{0}\",", "Select a Type ..."));
        sb.Append("width: \"100%\"");
        sb.Append("});");
        sb.Append("}");
        sb.Append("</script>");
        litJs.Text = sb.ToString();
    }

    #endregion
}