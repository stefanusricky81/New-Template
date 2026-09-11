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

public partial class UserControl_DropDownList_TicketDispositionResponsive : System.Web.UI.UserControl
{
    private bool _displayDefaultValue = true;
    private bool _displayNewDisposition = false;
    private bool? _active = null;
    private int? _selectedId = null;
    private string _defaultText = "";
    private string _defaultValue = "";
    private string _cssClass = "form-control select-chosen";
    private string _chosenDisplayName = "Disposition";

    /// <summary>
    /// page load
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        DisplayChosenPluginJs();
        if (ddlTicketDisposition.Items.Count == 0)
            PopulateDropDownList();
    }

    #region public methods

    /// <summary>
    /// populate drop down list
    /// </summary>
    public void PopulateDropDownList()
    {
        //clear items
        ddlTicketDisposition.ClearSelection();
        ddlTicketDisposition.Items.Clear();

        DesktopShared.CollectionClasses.CscFieldsCollection dispositions = DesktopShared.Ticket.CscField.GetDispositions();

        ddlTicketDisposition.DataSource = dispositions;
        ddlTicketDisposition.DataTextField = "Name";
        ddlTicketDisposition.DataValueField = "Pcscfields";
        ddlTicketDisposition.DataBind();

        //width / css class
        if (!String.IsNullOrEmpty(_cssClass))
            ddlTicketDisposition.CssClass = _cssClass;

        //select item
        if (_selectedId.HasValue)
        {
            ListItem li = ddlTicketDisposition.Items.FindByValue(_selectedId.Value.ToString());
            if (li != null)
                li.Selected = true;

            //if ticket disposition does not equal "new", user cannot change disposition to new
            if ((_selectedId != DesktopShared.SiteHelper.Ticket.Dispostion.Id.New) && !_displayNewDisposition)
            {
                ListItem liNew = ddlTicketDisposition.Items.FindByValue(DesktopShared.SiteHelper.Ticket.Dispostion.Id.New.ToString());
                if (liNew != null)
                    ddlTicketDisposition.Items.Remove(liNew);
            }
        }

        //show default list item
        if (_displayDefaultValue)
        {
            ListItem liDefault = new ListItem(_defaultText, _defaultValue);
            ddlTicketDisposition.Items.Insert(0, liDefault);
        }
    }

    #endregion

    #region public properties

    /// <summary>
    /// set width for drop down list
    /// </summary>
    public Unit Width
    {
        set { ddlTicketDisposition.Width = value; }
    }

    /// <summary>
    /// get/set ticket disposition id
    /// </summary>
    public int? TicketDispositionId
    {
        get
        {
            int _id = 0;
            if (Int32.TryParse(ddlTicketDisposition.SelectedValue.Trim(), out _id))
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
    /// get selected ticket disposition name name
    /// </summary>
    public string TicketDispositionName
    {
        get { return ddlTicketDisposition.SelectedItem.Text; }
    }

    /// <summary>
    /// set display name to use in chosen drop down
    /// </summary>
    public string ChosenDisplayName
    {
        set { _chosenDisplayName = value;  }
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
    /// set display "new" list item
    /// </summary>
    public bool DisplayNewDispositionOption
    {
        set { _displayNewDisposition = value; }
    }

    /// <summary>
    /// set tab index for control
    /// </summary>
    public short TabIndex
    {
        set { ddlTicketDisposition.TabIndex = value; }
    }

    /// <summary>
    /// set validation group
    /// </summary>
    public string ValidationGroup
    {
        set { rfvTicketDisposition.ValidationGroup = value; }
    }

    /// <summary>
    /// set validation error message
    /// </summary>
    public string ErrorMessage
    {
        set { rfvTicketDisposition.ErrorMessage = value; }
    }

    /// <summary>
    /// set drop down selection is required
    /// </summary>
    public bool IsRequired
    {
        set { rfvTicketDisposition.Visible = value; }
    }

    /// <summary>
    /// set css class for drop down list
    /// </summary>
    public string CssClass
    {
        set { _cssClass = value; } // ddlTicketDisposition.CssClass = value; }
    }

    /// <summary>
    /// get drop down list
    /// </summary>
    /// <returns></returns>
    public DropDownList GetDropDownList()
    {
        return ddlTicketDisposition;
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
        sb.Append(String.Format("$('#{0}').chosen({{", ddlTicketDisposition.ClientID));
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