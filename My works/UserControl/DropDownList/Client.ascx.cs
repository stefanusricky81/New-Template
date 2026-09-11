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

public partial class UserControl_DropDownList_Client : System.Web.UI.UserControl
{
    private bool _displayDefaultValue = true;
    private bool? _active = null;
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
        if (ddlClient.Items.Count == 0)
            PopulateDropDownList();
    }

    #region public methods

    /// <summary>
    /// populate drop down list
    /// </summary>
    public void PopulateDropDownList()
    {
        //clear items
        ddlClient.ClearSelection();
        ddlClient.Items.Clear();

        #region bind drop down list

        ddlClient.DataSource = DesktopShared.Client.GetAllActive("", IsMasterClient);
        ddlClient.DataValueField = "pclient";
        ddlClient.DataTextField = "company";
        ddlClient.DataBind();

        #endregion

        #region width / css class

        if (!String.IsNullOrEmpty(_cssClass))
            ddlClient.CssClass = _cssClass;

        #endregion

        #region selected item

        if (_selectedId.HasValue)
        {
            ListItem li = ddlClient.Items.FindByValue(_selectedId.Value.ToString());
            if (li != null)
                li.Selected = true;
        }

        #endregion

        #region show default list item

        if (_displayDefaultValue)
        {
            ListItem liDefault = new ListItem(_defaultText, _defaultValue);
            ddlClient.Items.Insert(0, liDefault);
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
        set { ddlClient.Width = value; }
    }

    /// <summary>
    /// get/set client id
    /// </summary>
    public int? ClientId
    {
        get
        {
            int _id = 0;
            if (Int32.TryParse(ddlClient.SelectedValue.Trim(), out _id))
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
    /// get/set is master client filter
    /// </summary>
    public bool IsMasterClient
    {
        get
        {
            object obj = this.ViewState["imc_cl"];
            return (obj == null) ? false : (bool)obj;
        }
        set { this.ViewState["imc_cl"] = value; }
    }

    /// <summary>
    /// get selected client name name
    /// </summary>
    public string ClientName
    {
        get { return ddlClient.SelectedItem.Text; }
    }

    /// <summary>
    /// set active value
    /// </summary>
    public bool? Active
    {
        set { _active = value; }
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
        set { ddlClient.TabIndex = value; }
    }

    /// <summary>
    /// set validation group
    /// </summary>
    public string ValidationGroup
    {
        set { rfvClient.ValidationGroup = value; }
    }

    /// <summary>
    /// set validation error message
    /// </summary>
    public string ErrorMessage
    {
        set { rfvClient.ErrorMessage = value; }
    }

    /// <summary>
    /// set drop down selection is required
    /// </summary>
    public bool IsRequired
    {
        set { rfvClient.Visible = value; }
    }

    /// <summary>
    /// set css class for drop down list
    /// </summary>
    public string CssClass
    {
        set { _cssClass = value; } // ddlClient.CssClass = value; }
    }

    /// <summary>
    /// get drop down list
    /// </summary>
    /// <returns></returns>
    public DropDownList GetDropDownList()
    {
        return ddlClient;
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
        sb.Append(String.Format("SetChosenClient_{0}();", this.ClientID));
        sb.Append(String.Format("Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(SetChosenClient_{0})", this.ClientID));
        sb.Append("});");
        sb.Append(String.Format("function SetChosenClient_{0}(sender, args) {{", this.ClientID));
        sb.Append(String.Format("$('#{0}').chosen({{", ddlClient.ClientID));
        sb.Append("allow_single_deselect: true,");
        sb.Append("placeholder_text_single: \"Select a Client ...\",");
        sb.Append("width: \"100%\"");
        sb.Append("});");
        sb.Append("}");
        sb.Append("</script>");
        litJs.Text = sb.ToString();
    }

    #endregion
}
