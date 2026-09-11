using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SD.LLBLGen.Pro.ORMSupportClasses;
using System.Data;

public partial class UserControl_DropDownList_Ticket : System.Web.UI.UserControl
{
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
        if (ddlTicket.Items.Count == 0)
            PopulateDropDownList();
    }

    #region public methods

    /// <summary>
    /// populdat drop down list
    /// </summary>
    public void PopulateDropDownList()
    {
        if (ClientId.HasValue)
        {
            #region data table of open tickets for user

            //get data table of open tickets for user
            DataTable dtTicket = DesktopShared.Ticket.GetAllClientTickets(ClientId.Value, true);

            //add new column for display
            dtTicket.Columns.Add("Display");

            //iterate through rows and combine data for display column
            for (int i = dtTicket.Rows.Count - 1; i >= 0; i--)
            {
                if (ExcludeTicketId.HasValue && (ExcludeTicketId.Value == BitByBit.Utility.DataBase.ConvertHelper.ConvertToInteger(dtTicket.Rows[i]["Id"], -1)))
                {
                    dtTicket.Rows[i].Delete();
                    continue;
                }
                DataRow dr = dtTicket.Rows[i];

                System.Text.StringBuilder sb = new System.Text.StringBuilder();

                //client name -> max of 10 characters 
                string _clientName = dr["CompanyName"].ToString().Trim();
                if (_clientName.Length > 10)
                    _clientName = _clientName.Substring(0, 10);
                sb.Append(_clientName + ": ");

                //ticket id -> pad left 5
                sb.Append(dr["Id"].ToString().Trim().PadLeft(5, '0') + " - ");

                //ticket summary -> max of 57
                string _summary = dr["Summary"].ToString().Trim();
                if (_summary.Length > 57)
                    _summary = _summary.Substring(0, 54) + " ...";
                sb.Append(_summary);

                dr["Display"] = sb.ToString();
            }

            dtTicket.AcceptChanges();

            //default sort by display
            dtTicket.DefaultView.Sort = "Display ASC";

            #endregion

            ddlTicket.DataSource = dtTicket;
            ddlTicket.DataTextField = "Display";
            ddlTicket.DataValueField = "Id";
            ddlTicket.DataBind();

            //select item
            if (_selectedId.HasValue)
            {
                ListItem li = ddlTicket.Items.FindByValue(_selectedId.Value.ToString());
                if (li != null)
                    li.Selected = true;
            }

            //show default list item
            if (_displayDefaultValue)
                ddlTicket.Items.Insert(0, new ListItem(_defaultText, _defaultValue));
        }

        //width / css class
        if (!String.IsNullOrEmpty(_cssClass))
            ddlTicket.CssClass = _cssClass;
    }

    #endregion

    #region public properties

    /// <summary>
    /// get/set client id
    /// </summary>
    public int? ClientId
    {
        get
        {
            object obj = this.ViewState["cid_tk"];
            return (obj == null) ? (int?)null : (int)obj;
        }
        set { this.ViewState["cid_tk"] = value; }
    }

    // <summary>
    /// get/set exclude ticket id
    /// </summary>
    public int? ExcludeTicketId
    {
        get
        {
            object obj = this.ViewState["etid_tk"];
            return (obj == null) ? (int?)null : (int)obj;
        }
        set { this.ViewState["etid_tk"] = value; }
    }

    /// <summary>
    /// get/set client ticket id
    /// </summary>
    public int? TicketId
    {
        get
        {
            if (String.IsNullOrWhiteSpace(ddlTicket.SelectedValue))
                return null;

            int _id = 0;
            if (Int32.TryParse(ddlTicket.SelectedValue.Trim(), out _id))
                return _id;
            else
                return (int?)null;
        }
        set { _selectedId = value; }
    }

    /// <summary>
    /// get selected value
    /// </summary>
    public string SelectedValue
    {
        get { return ddlTicket.SelectedValue; }
    }

    /// <summary>
    /// get client ticket name
    /// </summary>
    public string TicketName
    {
        get { return ddlTicket.SelectedItem.Text.Trim(); }
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
        set { ddlTicket.TabIndex = value; }
    }

    /// <summary>
    /// set validation group
    /// </summary>
    public string ValidationGroup
    {
        set { rfvTicket.ValidationGroup = value; }
    }

    /// <summary>
    /// set validation error message
    /// </summary>
    public string ErrorMessage
    {
        set { rfvTicket.ErrorMessage = value; }
    }

    /// <summary>
    /// set drop down selection is required
    /// </summary>
    public bool IsRequired
    {
        set { rfvTicket.Visible = value; }
    }

    /// <summary>
    /// set css class for drop down list
    /// </summary>
    public string CssClass
    {
        set { _cssClass = value; } // ddlTicket.CssClass = value; }
    }

    /// <summary>
    /// get drop down list
    /// </summary>
    /// <returns></returns>
    public DropDownList GetDropDownList()
    {
        return ddlTicket;
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
        sb.Append(String.Format("$('#{0}').chosen({{", ddlTicket.ClientID));
        sb.Append("allow_single_deselect: true,");
        sb.Append("placeholder_text_single: \"Select a Ticket ...\",");
        sb.Append("width: \"100%\"");
        sb.Append("});");
        sb.Append("}");
        sb.Append("</script>");
        litJs.Text = sb.ToString();
    }

    #endregion
}