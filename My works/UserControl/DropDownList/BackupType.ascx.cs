using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DesktopShared;
using SD.LLBLGen.Pro.ORMSupportClasses;

public partial class UserControl_DropDownList_BackupType : System.Web.UI.UserControl
{
    private string _defaultText = "ALL";
    private string _defaultValue = "";
    private bool _displayDefaultValue = true;
    private bool _isLoaded = false;
    private int _selectedId = 0;
    private string _cssClass = "form-control select-chosen";

    protected void Page_Load(object sender, EventArgs e)
    {
        DisplayChosenPluginJs();
        if (!this.IsPostBack && !_isLoaded)
            Populate();
    }

    #region public methods

    /// <summary>
    /// populdate drop down list
    /// </summary>
    //public void Populate()
    //{
    //    //get collection
    //    DesktopShared.CollectionClasses.BackupMonitorStatusCollection _statuses = DesktopShared.Backup.Monitor.Status.Get(Active);

    //    #region bind drop down list

    //    ddlStatus.DataSource = _statuses;
    //    ddlStatus.DataTextField = "Name";
    //    ddlStatus.DataValueField = "Id";
    //    ddlStatus.DataBind();
        
    //    #endregion
                
    //    #region find by selected id

    //    if (_selectedId > 0)
    //    {
    //        ListItem li = ddlStatus.Items.FindByValue(_selectedId.ToString());
    //        if (li != null)
    //        {
    //            ddlStatus.ClearSelection();
    //            li.Selected = true;
    //        }
    //    }
        
    //    #endregion

    //    #region display default value

    //    if (_displayDefaultValue)
    //        //ddlStatus.Items.Insert(0, new ListItem(_defaultText, _defaultValue));

    //    #endregion

    //    _isLoaded = true;
    //}

    public void Populate()
    {

        IPredicateExpression _filter = new PredicateExpression();
        _filter.Add(DesktopShared.HelperClasses.SupportingTableFields.TableType == "BACKUP");
               //sort expression
        ISortExpression _statusesSort = new SortExpression();
        _statusesSort.Add(DesktopShared.HelperClasses.SupportingTableFields.Description | SortOperator.Ascending);
        DesktopShared.CollectionClasses.SupportingTableCollection _statuses = new DesktopShared.CollectionClasses.SupportingTableCollection();
        _statuses.GetMulti(_filter, 0, _statusesSort);

            ddlStatus.DataSource = _statuses;
            ddlStatus.DataTextField = "Description";
            ddlStatus.DataValueField = "pSupportingTable";
            ddlStatus.DataBind();

            if (_selectedId > 0)
            {
                ListItem li = ddlStatus.Items.FindByValue(_selectedId.ToString());
                if (li != null)
                    li.Selected = true;
            }

            if (_displayDefaultValue)
                ddlStatus.Items.Insert(0, new ListItem(_defaultText, _defaultValue));

        if (!String.IsNullOrEmpty(_cssClass))
            ddlStatus.CssClass = _cssClass;

        _isLoaded = true;
    }

    #endregion

    #region public properties

    /// <summary>
    /// get/set active flag for filter
    /// </summary>
    public bool? Active
    {
        get
        {
            object obj = this.ViewState["ActiveForStatus"];
            if (obj == null)
                return null;
            else
                return (bool)obj;
        }
        set { this.ViewState["ActiveForStatus"] = value; }
    }

    /// <summary>
    /// set width of drop down list control
    /// </summary>
    public Unit Width
    {
        set { ddlStatus.Width = value; }
    }

    /// <summary>
    /// get drop down list
    /// </summary>
    /// <returns></returns>
    public DropDownList GetDropDownList()
    {
        return ddlStatus;
    }

    /// <summary>
    /// set/set status id
    /// </summary>
    public int StatusId
    {
        get
        {
            try
            {
                if (ddlStatus.SelectedIndex > -1)
                    return Convert.ToInt32(ddlStatus.SelectedItem.Value);
                else
                    return -1;
            }
            catch { return -1; }
        }
        set { 
            _selectedId = value;
            Populate();
        }
    }    

    /// <summary>
    /// get selected status name
    /// </summary>
    public string StatusName
    {
        get { return ddlStatus.SelectedIndex >= 0 ? ddlStatus.SelectedItem.Text : ""; }
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
        set { rfvStatus.ValidationGroup = value; }
    }

    /// <summary>
    /// set selection is required
    /// </summary>
    public bool IsRequired
    {
        set { rfvStatus.Visible = value; }
    }

    /// <summary>
    /// set css class for drop down list control
    /// </summary>
    public string CssClass
    {
        set { _cssClass = value; }
    }

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
        sb.Append(String.Format("$('#{0}').chosen({{", ddlStatus.SelectedValue));
        sb.Append("allow_single_deselect: true,");
        sb.Append("placeholder_text_single: \"Select a Ticket Type ...\",");
        sb.Append("width: \"100%\"");
        sb.Append("});");
        sb.Append("}");
        sb.Append("</script>");
        litJs.Text = sb.ToString();
    }

    #endregion
}