using SD.LLBLGen.Pro.ORMSupportClasses;
using System;
using System.Web.UI.WebControls;

public partial class UserControl_DropDownList_EndpointPlatform : System.Web.UI.UserControl
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
        if (ddlEndpointPlatform.Items.Count == 0)
            PopulateDropDownList();
    }

    #region public methods

    /// <summary>
    /// bind EndpointPlatform drop down list
    /// </summary>
    public void PopulateDropDownList()
    {
        //clear items
        ddlEndpointPlatform.ClearSelection();
        ddlEndpointPlatform.Items.Clear();

        //tab index
        ddlEndpointPlatform.TabIndex = _tabIndex;
        
        //get collection
        var endpointPlatforms = new DesktopShared.CollectionClasses.EndpointPlatformCollection();
        var endpointPlatformsFilter = new PredicateExpression();
        endpointPlatformsFilter.Add(DesktopShared.HelperClasses.EndpointPlatformFields.Active == true);
        var endpointPlatformsSort = new SortExpression();
        endpointPlatformsSort.Add(DesktopShared.HelperClasses.EndpointPlatformFields.Name | SortOperator.Ascending);
        endpointPlatforms.GetMulti(endpointPlatformsFilter, 0, endpointPlatformsSort);

        //bind drop down list
        ddlEndpointPlatform.DataSource = endpointPlatforms;
        ddlEndpointPlatform.DataTextField = "Name";
        ddlEndpointPlatform.DataValueField = "Id";
        ddlEndpointPlatform.DataBind();
        ddlEndpointPlatform.DataSource = null;

        //width / css class
        if (_ddWidth > 0)
            ddlEndpointPlatform.Width = _ddWidth;
        if (!String.IsNullOrEmpty(_cssClass))
            ddlEndpointPlatform.CssClass = _cssClass;

        //selected item
        if (_selectedId.HasValue)
        {
            ListItem li = ddlEndpointPlatform.Items.FindByValue(_selectedId.Value.ToString());
            if (li != null)
                li.Selected = true;
        }

        //show default list item
        if (_showDefault)
        {
            ListItem liDefault = new ListItem(_defaultText, _defaultValue);
            ddlEndpointPlatform.Items.Insert(0, liDefault);
            if (!_selectedId.HasValue)
                ddlEndpointPlatform.SelectedIndex = 0;
        }
    }

    #endregion

    #region public properties

    /// <summary>
    /// get/set selected endpoint platform  ID
    /// </summary>
    public int? EndpointPlatformId
    {
        get
        {
            int _id = 0;
            if (Int32.TryParse(ddlEndpointPlatform.SelectedValue.Trim(), out _id))
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
    /// get selected endpoint platform text
    /// </summary>
    public string EndpointPlatformText
    {
        get { return ddlEndpointPlatform.SelectedItem.Text; }
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
        set { rfvEndpointPlatform.ValidationGroup = value; }
    }

    /// <summary>
    /// set drop down selection is required
    /// </summary>
    public bool IsRequired
    {
        set { rfvEndpointPlatform.Visible = value; }
    }

    /// <summary>
    /// set validation error message
    /// </summary>
    public string ErrorMessage
    {
        set { rfvEndpointPlatform.ErrorMessage = value; }
    }

    /// <summary>
    /// set validator display 
    /// </summary>
    public ValidatorDisplay validatorDisplay
    {
        set { rfvEndpointPlatform.Display = value; }
    }

    /// <summary>
    /// set validator fore color
    /// </summary>
    public System.Drawing.Color ValidatorForeColor
    {
        set { rfvEndpointPlatform.ForeColor = value; }
    }

    /// <summary>
    /// get drop down list control
    /// </summary>
    /// <returns></returns>
    public DropDownList GetDropDownList()
    {
        return ddlEndpointPlatform;
    }

    /// <summary>
    /// get bool of whether drop down list has any records
    /// </summary>
    public bool HasRecords
    {
        get
        {
            int _count = _showDefault ? 1 : 0;
            return ddlEndpointPlatform.Items.Count > _count;
        }
    }

    /// <summary>
    /// set drop down list enabled
    /// </summary>
    public bool Enabled
    {
        set { ddlEndpointPlatform.Enabled = value; }
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
        sb.Append("<script ='text/javascript'>");
        sb.Append("$(document).ready(function() {");
        sb.Append(String.Format("SetChosenEndpointPlatform_{0}();", this.ClientID));
        sb.Append(String.Format("Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(SetChosenEndpointPlatform_{0})", this.ClientID));
        sb.Append("});");
        sb.Append(String.Format("function SetChosenEndpointPlatform_{0}(sender, args) {{", this.ClientID));
        sb.Append(String.Format("$('#{0}').chosen({{", ddlEndpointPlatform.ClientID));
        sb.Append("allow_single_deselect: true,");
        sb.Append(String.Format("placeholder_text_single: \"{0}\",", "Select an Endpoint ..."));
        sb.Append("width: \"100%\"");
        sb.Append("});");
        sb.Append("}");
        sb.Append("</script>");
        litJs.Text = sb.ToString();
    }

    #endregion
}