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
using SD.LLBLGen.Pro.ORMSupportClasses;

public partial class UserControl_DropDownList_ClientLocation : System.Web.UI.UserControl
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
        if (ddlClientLocation.Items.Count == 0)
            PopulateDropDownList();
    }

    #region public methods

    /// <summary>
    /// populate drop down list
    /// </summary>
    public void PopulateDropDownList()
    {
        //clear items
        ddlClientLocation.ClearSelection();
        ddlClientLocation.Items.Clear();

        if (ClientIdForLocation.HasValue)
        {
            ddlClientLocation.DataSource = DesktopShared.Client.Location.Get(ClientIdForLocation.Value, _active);
            ddlClientLocation.DataTextField = "Name";
            ddlClientLocation.DataValueField = "Id";
            ddlClientLocation.DataBind();
        }
        else if (UserIdForLocation.HasValue)
        {
            var _locations = DesktopShared.Client.Location.GetForUser(UserIdForLocation.Value);
            ddlClientLocation.DataSource = _locations;
            ddlClientLocation.DataTextField = "ClientLocationName";
            ddlClientLocation.DataValueField = "ClientLocationId";
            ddlClientLocation.DataBind();

            if (!_selectedId.HasValue)
            {
                foreach (var objLocation in _locations)
                {
                    if (objLocation.Primary)
                    {
                        _selectedId = objLocation.ClientLocationId;
                        break;
                    }
                }
            }
        }

        //css class
        if (!String.IsNullOrEmpty(_cssClass))
            ddlClientLocation.CssClass = _cssClass;

        //selected item
        if (_selectedId.HasValue)
        {
            ListItem li = ddlClientLocation.Items.FindByValue(_selectedId.Value.ToString());
            if (li != null)
                li.Selected = true;
        }

        //show default list item
        if (_displayDefaultValue)
        {
            ListItem liDefault = new ListItem(_defaultText, _defaultValue);
            ddlClientLocation.Items.Insert(0, liDefault);
        }
    }

    #endregion

    #region public properties

    /// <summary>
    /// get/set client id
    /// </summary>
    public int? ClientIdForLocation
    {
        get
        {
            object obj = this.ViewState["cid_cl"];
            return (obj == null) ? (int?)null : (int)obj;
        }
        set { this.ViewState["cid_cl"] = value; }
    }

    /// <summary>
    /// get/set user id
    /// </summary>
    public int? UserIdForLocation
    {
        get
        {
            object obj = this.ViewState["uid_cl"];
            return (obj == null) ? (int?)null : (int)obj;
        }
        set { this.ViewState["uid_cl"] = value; }
    }


    /// <summary>
    /// get/set ticket status id
    /// </summary>
    public int? ClientLocationId
    {
        get
        {
            int _id = 0;
            if (Int32.TryParse(ddlClientLocation.SelectedValue.Trim(), out _id))
                return _id;
            else
                return (int?)null;
        }
        set { _selectedId = value; }
    }

    /// <summary>
    /// get selected ticket status name name
    /// </summary>
    public string ClientLocationName
    {
        get { return ddlClientLocation.SelectedItem.Text; }
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
        set { ddlClientLocation.TabIndex = value; }
    }

    /// <summary>
    /// set validation group
    /// </summary>
    public string ValidationGroup
    {
        set { rfvClientLocation.ValidationGroup = value; }
    }

    /// <summary>
    /// set validation error message
    /// </summary>
    public string ErrorMessage
    {
        set { rfvClientLocation.ErrorMessage = value; }
    }

    /// <summary>
    /// set drop down selection is required
    /// </summary>
    public bool IsRequired
    {
        set { rfvClientLocation.Visible = value; }
    }

    /// <summary>
    /// set css class for drop down list
    /// </summary>
    public string CssClass
    {
        set { _cssClass = value; } 
    }

    /// <summary>
    /// get drop down list
    /// </summary>
    /// <returns></returns>
    public DropDownList GetDropDownList()
    {
        return ddlClientLocation;
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
        sb.Append(String.Format("SetChosenTicket_{0}();", this.ClientID));
        sb.Append(String.Format("Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(SetChosenTicket_{0})", this.ClientID));
        sb.Append("});");
        sb.Append(String.Format("function SetChosenTicket_{0}(sender, args) {{", this.ClientID));
        sb.Append(String.Format("$('#{0}').chosen({{", ddlClientLocation.ClientID));
        sb.Append("allow_single_deselect: true,");
        sb.Append("placeholder_text_single: \"Select a Location ...\",");
        sb.Append("width: \"100%\"");
        sb.Append("});");
        sb.Append("}");
        sb.Append("</script>");
        litJs.Text = sb.ToString();
    }

    #endregion
}

