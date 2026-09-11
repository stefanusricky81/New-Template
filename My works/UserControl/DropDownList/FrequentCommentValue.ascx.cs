using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class UserControl_DropDownList_FrequentCommentValue : System.Web.UI.UserControl
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
        if (ddlFrequentCommentValue.Items.Count == 0)
            PopulateDropDownList();
    }

    #region public methods

    /// <summary>
    /// bind FrequentComment drop down list
    /// </summary>
    public void PopulateDropDownList()
    {
        //clear items
        ddlFrequentCommentValue.ClearSelection();
        ddlFrequentCommentValue.Items.Clear();

        //tab index
        ddlFrequentCommentValue.TabIndex = _tabIndex;

        #region bind drop down list

        ddlFrequentCommentValue.DataSource = DesktopShared.FrequentComment.Value.GetActive(FrequentCommentTypeId);
        ddlFrequentCommentValue.DataTextField = "Text";
        ddlFrequentCommentValue.DataValueField = TruncateTextMaxLength.HasValue ? "Text" : "Id";
        ddlFrequentCommentValue.DataBind();
        ddlFrequentCommentValue.DataSource = null;

        if (TruncateTextMaxLength.HasValue)
        {
            foreach (ListItem _li in ddlFrequentCommentValue.Items)
                _li.Text = DesktopShared.Utility.String.Truncate(_li.Text, TruncateTextMaxLength.Value, " ..");
        }

        #endregion

        #region width / css class

        if (_ddWidth > 0)
            ddlFrequentCommentValue.Width = _ddWidth;
        if (!String.IsNullOrEmpty(_cssClass))
            ddlFrequentCommentValue.CssClass = _cssClass;

        #endregion

        #region selected item

        if (_selectedId.HasValue)
        {
            ListItem li = ddlFrequentCommentValue.Items.FindByValue(_selectedId.Value.ToString());
            if (li != null)
                li.Selected = true;
        }

        #endregion

        #region show default list item

        if (_showDefault)
        {
            ListItem liDefault = new ListItem(_defaultText, _defaultValue);
            ddlFrequentCommentValue.Items.Insert(0, liDefault);
            if (!_selectedId.HasValue)
                ddlFrequentCommentValue.SelectedIndex = 0;
        }

        #endregion
    }

    #endregion

    #region public properties

    /// <summary>
    /// get/set selected frequent comment  ID
    /// </summary>
    public int? FrequentCommentId
    {
        get
        {
            int _id = 0;
            if (Int32.TryParse(ddlFrequentCommentValue.SelectedValue.Trim(), out _id))
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
    /// get selected frequent comment text
    /// </summary>
    public string FrequentCommentText
    {
        get { return ddlFrequentCommentValue.SelectedItem.Text; }
    }

    /// <summary>
    /// get selected comment values
    /// used if trunating actual display text
    /// </summary>
    public string FullExamCommentValue
    {
        get { return ddlFrequentCommentValue.SelectedItem.Value; }

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
        set { rfvFrequentCommentValue.ValidationGroup = value; }
    }

    /// <summary>
    /// set drop down selection is required
    /// </summary>
    public bool IsRequired
    {
        set { rfvFrequentCommentValue.Visible = value; }
    }

    /// <summary>
    /// set validation error message
    /// </summary>
    public string ErrorMessage
    {
        set { rfvFrequentCommentValue.ErrorMessage = value; }
    }

    /// <summary>
    /// set validator display 
    /// </summary>
    public ValidatorDisplay validatorDisplay
    {
        set { rfvFrequentCommentValue.Display = value; }
    }

    /// <summary>
    /// set validator fore color
    /// </summary>
    public System.Drawing.Color ValidatorForeColor
    {
        set { rfvFrequentCommentValue.ForeColor = value; }
    }

    /// <summary>
    /// get drop down list control
    /// </summary>
    /// <returns></returns>
    public DropDownList GetDropDownList()
    {
        return ddlFrequentCommentValue;
    }

    /// <summary>
    /// get bool of whether drop down list has any records
    /// </summary>
    public bool HasRecords
    {
        get
        {
            int _count = _showDefault ? 1 : 0;
            return ddlFrequentCommentValue.Items.Count > _count;
        }
    }

    /// <summary>
    /// set drop down list enabled
    /// </summary>
    public bool Enabled
    {
        set { ddlFrequentCommentValue.Enabled = value; }
    }

    /// <summary>
    /// set show chosen script
    /// </summary>
    public bool DisplayChosenScript
    {
        set { litJs.Visible = value; }
    }

    /// <summary>
    /// get/set frequent comment type id
    /// </summary>
    public int? FrequentCommentTypeId
    {
        get
        {
            object obj = this.ViewState["fcti"];
            return (obj == null) ? (int?)null : (int)obj;
        }
        set { this.ViewState["fcti"] = value; }
    }

    /// <summary>
    /// get/set max length for text if truncating
    /// </summary>
    public int? TruncateTextMaxLength
    {
        get
        {
            object obj = this.ViewState["ttml"];
            return (obj == null) ? (int?)null : (int)obj;
        }
        set { this.ViewState["ttml"] = value; }
    }

    #endregion

    #region private methods

    /// <summary>
    /// display script used by chosen plugin
    /// </summary>
    private void DisplayChosenPluginJs()
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        sb.Append("<script ='text/javascript'>");
        sb.Append("$(document).ready(function() {");
        sb.Append(String.Format("SetChosenFrequentComment_{0}();", this.ClientID));
        sb.Append(String.Format("Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(SetChosenFrequentComment_{0})", this.ClientID));
        sb.Append("});");
        sb.Append(String.Format("function SetChosenFrequentComment_{0}(sender, args) {{", this.ClientID));
        sb.Append(String.Format("$('#{0}').chosen({{", ddlFrequentCommentValue.ClientID));
        sb.Append("allow_single_deselect: true,");
        sb.Append(String.Format("placeholder_text_single: \"{0}\",", "Select a Comment ..."));
        sb.Append("width: \"100%\"");
        sb.Append("});");
        sb.Append("}");
        sb.Append("</script>");
        litJs.Text = sb.ToString();
    }

    #endregion
}