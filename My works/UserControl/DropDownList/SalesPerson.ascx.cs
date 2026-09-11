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

public partial class UserControl_DropDownList_SalesPerson : System.Web.UI.UserControl
{
    private short _tabIndex = 10;
    private bool _displayDefaultValue = true;
    private int? _selectedId = null;
    private string _defaultText = "";
    private string _defaultValue = "";
    private string _cssClass = "form-control select-chosen";

    /// <summary>
    /// page load
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        DisplayChosenPluginJs();
        if (ddlSalesPerson.Items.Count == 0)
            PopulateDropDownList();
    }

    #region public methods

    /// <summary>
    /// populate drop down list
    /// </summary>
    public void PopulateDropDownList()
    {
        #region bind drop down list
        
        ddlSalesPerson.DataSource = DesktopShared.StoredProcedureCallerClasses.RetrievalProcedures.ProcGetSalesperson(0, 0, 0, 0);
        ddlSalesPerson.DataTextField = "Name";
        ddlSalesPerson.DataValueField = "Id";
        ddlSalesPerson.DataBind();

        #endregion

        #region width / css class

        if (!String.IsNullOrEmpty(_cssClass))
            ddlSalesPerson.CssClass = _cssClass;

        #endregion

        #region selected item

        if (_selectedId.HasValue)
        {
            ListItem li = ddlSalesPerson.Items.FindByValue(_selectedId.Value.ToString());
            if (li != null)
                li.Selected = true;
        }

        #endregion

        #region show default list item

        if (_displayDefaultValue)
        {
            ListItem liDefault = new ListItem(_defaultText, _defaultValue);
            ddlSalesPerson.Items.Insert(0, liDefault);
        }

        #endregion
    }

    #endregion

    #region public properties

    /// <summary>
    /// set width for drop down list
    /// </summary>
    public Unit Width
    {
        set { ddlSalesPerson.Width = value; }
    }

    /// <summary>
    /// get/set employee id
    /// </summary>
    public int? EmployeeId
    {
        get
        {
            int _id = 0;
            if (Int32.TryParse(ddlSalesPerson.SelectedValue.Trim(), out _id))
                return _id;
            else
                return (int?)null;
        }
        set
        {
            _selectedId = value;
            if (value.HasValue)
            {
                ListItem _li = ddlSalesPerson.Items.FindByValue(value.Value.ToString());
                if (_li != null)
                {
                    ddlSalesPerson.ClearSelection();
                    _li.Selected = true;
                    return;
                }
            }
            PopulateDropDownList();
        }
    }

    /// <summary>
    /// get selected employee name
    /// </summary>
    public string EmployeeName
    {
        get { return ddlSalesPerson.SelectedItem.Text; }
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
        set { rfvSalesPerson.ValidationGroup = value; }
    }

    /// <summary>
    /// set validation error message
    /// </summary>
    public string ErrorMessage
    {
        set { rfvSalesPerson.ErrorMessage = value; }
    }

    /// <summary>
    /// set drop down selection is required
    /// </summary>
    public bool IsRequired
    {
        set { rfvSalesPerson.Visible = value; }
    }

    /// <summary>
    /// set css class for drop down list
    /// </summary>
    public string CssClass
    {
        set { ddlSalesPerson.CssClass = value; }
    }

    /// <summary>
    /// get drop down list
    /// </summary>
    /// <returns></returns>
    public DropDownList GetDropDownList()
    {
        return ddlSalesPerson;
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
        sb.Append(String.Format("SetChosenSalesPerson_{0}();", this.ClientID));
        sb.Append(String.Format("Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(SetChosenSalesPerson_{0})", this.ClientID));
        sb.Append("});");
        sb.Append(String.Format("function SetChosenSalesPerson_{0}(sender, args) {{", this.ClientID));
        sb.Append(String.Format("$('#{0}').chosen({{", ddlSalesPerson.ClientID));
        sb.Append("allow_single_deselect: true,");
        sb.Append("placeholder_text_single: \"Select a Sales Rep ...\",");
        sb.Append("width: \"100%\"");
        sb.Append("});");
        sb.Append("}");
        sb.Append("</script>");
        litJs.Text = sb.ToString();
    }

    #endregion
}
