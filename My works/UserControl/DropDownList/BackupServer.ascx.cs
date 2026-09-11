using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DesktopShared;
using SD.LLBLGen.Pro.ORMSupportClasses;

public partial class UserControl_DropDownList_BackupServer : System.Web.UI.UserControl
{
    private string _defaultText = "ALL";
    private string _defaultValue = "";
    private bool _displayDefaultValue = true;
    private string _selectedText = "";
    private string _cssClass = "form-control select-chosen";

    protected void Page_Load(object sender, EventArgs e)
    {
        DisplayChosenPluginJs();
        if (ddlBackupServer.Items.Count == 0)
            Populate();
    }

    #region public methods

    /// <summary>
    /// populdate drop down list
    /// </summary>
    public void Populate()
    {
        ddlBackupServer.DataSource = DesktopShared.Backup.Server.GetDistinctNames();
        ddlBackupServer.DataTextField = "BackupServer";
        ddlBackupServer.DataValueField = "BackupServer";
        ddlBackupServer.DataBind();

        if (!String.IsNullOrEmpty(_cssClass))
            ddlBackupServer.CssClass = _cssClass;

        if (!String.IsNullOrWhiteSpace(_selectedText))
        {
            ListItem li = ddlBackupServer.Items.FindByText(_selectedText);
            if (li != null)
            {
                ddlBackupServer.ClearSelection();
                li.Selected = true;
            }
        }
        
        if (_displayDefaultValue)
            ddlBackupServer.Items.Insert(0, new ListItem(_defaultText, _defaultValue));
    }

    #endregion

    #region public properties

    /// <summary>
    /// set width of drop down list control
    /// </summary>
    public Unit Width
    {
        set { ddlBackupServer.Width = value; }
    }

    /// <summary>
    /// get drop down list
    /// </summary>
    /// <returns></returns>
    public DropDownList GetDropDownList()
    {
        return ddlBackupServer;
    }

    /// <summary>
    /// set/set selected backup server name
    /// </summary>
    public string BackupServerName
    {
        get
        {
            return ddlBackupServer.SelectedItem.Text;
        }
        set
        { 
            _selectedText = value;
            Populate();
        }
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
        set { rfvBackupServer.ValidationGroup = value; }
    }

    /// <summary>
    /// set selection is required
    /// </summary>
    public bool IsRequired
    {
        set { rfvBackupServer.Visible = value; }
    }

    /// <summary>
    /// set css class for drop down list control
    /// </summary>

    public string CssClass
    {
        set { _cssClass = value; } // ddlClient.CssClass = value; }
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
        sb.Append(String.Format("$('#{0}').chosen({{", ddlBackupServer.SelectedValue));
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