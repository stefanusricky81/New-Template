using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DesktopShared;
using SD.LLBLGen.Pro.ORMSupportClasses;

public partial class UserControl_DropDownList_SupportingTable : System.Web.UI.UserControl
{
    private string _defaultText = "";
    private string _defaultValue = "";
    private string _cssClass = "form-control select-chosen";
    private string _chosenDisplayName = "Type";
    private bool _displayDefaultValue = true;
    private int? _selectedId = null;
    private short _tabIndex = 10;

    /// <summary>
    /// page load
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        DisplayChosenPluginJs();
        if (ddlSupportingTable.Items.Count == 0)
            PopulateDropDownList();
    }

    #region public methods

   
    /// <summary>
    /// populate drop down list
    /// </summary>
    public void PopulateDropDownList()
    {
        //clear items
        ddlSupportingTable.ClearSelection();
        ddlSupportingTable.Items.Clear();

        //tab index
        ddlSupportingTable.TabIndex = _tabIndex;

        if (String.IsNullOrWhiteSpace(TableType))
            return;

        //get collection
        DesktopShared.CollectionClasses.SupportingTableCollection _collection = new DesktopShared.CollectionClasses.SupportingTableCollection();
        IPredicateExpression _filter = new PredicateExpression();
        _filter.Add(DesktopShared.HelperClasses.SupportingTableFields.TableType == TableType.Trim());
        ISortExpression _sort = new SortExpression();
        _sort.Add(DesktopShared.HelperClasses.SupportingTableFields.Description | SortOperator.Ascending);
        _collection.GetMulti(_filter, 0, _sort);
        
        //bind drop down list
        ddlSupportingTable.DataSource = _collection;
        ddlSupportingTable.DataTextField = "Description";
        ddlSupportingTable.DataValueField = "pSupportingTable";
        ddlSupportingTable.DataBind();
        ddlSupportingTable.DataSource = null;

        //width / css class
        if (!String.IsNullOrEmpty(_cssClass))
            ddlSupportingTable.CssClass = _cssClass;

        //selected item
        if (_selectedId.HasValue)
        {
            ListItem li = ddlSupportingTable.Items.FindByValue(_selectedId.ToString());
            if (li != null)
                li.Selected = true;
        }

        //default item
        if (_displayDefaultValue)
            ddlSupportingTable.Items.Insert(0, new ListItem(_defaultText, _defaultValue));
    }

    #endregion

    #region public properties

    /// <summary>
    /// get/set table type
    /// </summary>
    public string TableType
    {
        get
        {
            object obj = this.ViewState["tt_st"];
            return (obj == null) ? "": (string)obj;
        }
        set { this.ViewState["tt_st"] = value; }
    }

    /// <summary>
    /// set/set SupportingTable id
    /// </summary>
    public int? SupportingTableId
    {
        get
        {
            int _id = 0;
            if (Int32.TryParse(ddlSupportingTable.SelectedValue.Trim(), out _id)) 
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
    /// get selected SupportingTable name
    /// </summary>
    public string SupportingTableName
    {
        get { return ddlSupportingTable.SelectedIndex >= 0 ? ddlSupportingTable.SelectedItem.Text : ""; }
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
    /// set display default list item
    /// </summary>
    public bool DisplayDefaultValue
    {
        set { _displayDefaultValue = value; }
    }

    /// <summary>
    /// set validation group for required field validator
    /// </summary>
    public string ValidationGroup
    {
        set { rfvSupportingTable.ValidationGroup = value; }
    }

    /// <summary>
    /// set selection is required
    /// </summary>
    public bool IsRequired
    {
        set { rfvSupportingTable.Visible = value; }
    }

    /// <summary>
    /// set validation error message
    /// </summary>
    public string ErrorMessage
    {
        set { rfvSupportingTable.ErrorMessage = value; }
    }

    /// <summary>
    /// set css class for drop down list control
    /// </summary>
    public string CssClass
    {
        set
        {
            ddlSupportingTable.CssClass = value;
            _cssClass = value;
        }
    }

    /// <summary>
    /// set tab index for control
    /// </summary>
    public short TabIndex
    {
        set
        {
            ddlSupportingTable.TabIndex = value;
            _tabIndex = value;
        }
    }

    /// <summary>
    /// get drop down list
    /// </summary>
    /// <returns></returns>
    public DropDownList GetDropDownList()
    {
        return ddlSupportingTable;
    }

    /// <summary>
    /// set show chosen script
    /// </summary>
    public bool DisplayChosenScript
    {
        set { litJs.Visible = value; }
    }

    /// <summary>
    /// set display name to use in chosen drop down
    /// </summary>
    public string ChosenDisplayName
    {
        set { _chosenDisplayName = value; }
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
        sb.Append(String.Format("SetChosenSupportingTable_{0}();", this.ClientID));
        sb.Append(String.Format("Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(SetChosenSupportingTable_{0})", this.ClientID));
        sb.Append("});");
        sb.Append(String.Format("function SetChosenSupportingTable_{0}(sender, args) {{", this.ClientID));
        sb.Append(String.Format("$('#{0}').chosen({{", ddlSupportingTable.ClientID));
        sb.Append("allow_single_deselect: true,");
        sb.Append(String.Format("placeholder_text_single: \"Select a {0} ...\",", _chosenDisplayName));
        sb.Append("width: \"100%\"");
        sb.Append("});");
        sb.Append("}");
        sb.Append("</script>");
        litJs.Text = sb.ToString();
    }

    #endregion
}