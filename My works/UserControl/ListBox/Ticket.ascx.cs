using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SD.LLBLGen.Pro.ORMSupportClasses;
using System.Data;

public partial class UserControl_ListBox_Ticket : System.Web.UI.UserControl
{
    private string _defaultText = "";
    private string _cssClass = "form-control select-chosen";

    /// <summary>
    /// page load
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        DisplayChosenPluginJs();
        if (lbTicket.Items.Count == 0)
            Populate();
    }

    #region public methods

    /// <summary>
    /// populate list box
    /// </summary>
    /// <param name="selectedIds"></param>
    /// <param name="defaultId"></param>
    /// <param name="defaultText"></param>
    public void Populate(List<int> selectedIds = null, int? defaultId = null, string defaultText = "")
    {
        if (ClientId.HasValue)
        {
            #region data table of open tickets for user

            //get data table of open tickets for user
            DataTable dtTicket = DesktopShared.Ticket.GetAllClientTickets(ClientId.Value, true, true);

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

            lbTicket.DataSource = dtTicket;
            lbTicket.DataTextField = "Display";
            lbTicket.DataValueField = "Id";
            lbTicket.DataBind();

            if (defaultId.HasValue && !String.IsNullOrWhiteSpace(defaultText))
                lbTicket.Items.Insert(0, new ListItem(defaultText, defaultId.Value.ToString()));

            if (selectedIds != null)
            {
                foreach (int _id in selectedIds)
                {
                    ListItem li = lbTicket.Items.FindByValue(_id.ToString());
                    if (li != null)
                        li.Selected = true;
                }
            }
        }

        //css class
        if (!String.IsNullOrEmpty(_cssClass))
            lbTicket.CssClass = _cssClass;
    }

    /// <summary>
    /// remove specified item
    /// </summary>
    /// <param name="id"></param>
    public void RemoveItem(int id)
    {
        foreach (ListItem _li in lbTicket.Items)
        {
            if (Convert.ToInt32(_li.Value) == id)
            {
                lbTicket.Items.Remove(_li);
                return;
            }
        }
    }

    #endregion

    #region public properties

    /// <summary>
    /// get selected values
    /// </summary>
    public List<int> SelectedValues
    {
        get
        {
            List<int> _vals = new List<int>();
            foreach (ListItem _li in lbTicket.Items)
            {
                if (_li.Selected)
                {
                    int _id = 0;
                    if (int.TryParse(_li.Value.Trim(), out _id))
                        _vals.Add(_id);
                }
            }
            return _vals;
        }
    }

    /// <summary>
    /// get/set client id
    /// </summary>
    public int? ClientId
    {
        get
        {
            object obj = this.ViewState["cid_tlb"];
            return (obj == null) ? (int?)null : (int)obj;
        }
        set { this.ViewState["cid_tlb"] = value; }
    }

    /// <summary>
    /// get/set include place holder tickets
    /// </summary>
    public bool IncludePlaceHolderTickets
    {
        get
        {
            object obj = this.ViewState["ipht_tlb"];
            return (obj == null) ? true : (bool)obj;
        }
        set { this.ViewState["ipht_tlb"] = value; }
    }

    // <summary>
    /// get/set exclude ticket id
    /// </summary>
    public int? ExcludeTicketId
    {
        get
        {
            object obj = this.ViewState["etid_tlb"];
            return (obj == null) ? (int?)null : (int)obj;
        }
        set { this.ViewState["etid_tlb"] = value; }

    }
    /// <summary>
    /// set tab index for control
    /// </summary>
    public short TabIndex
    {
        set { lbTicket.TabIndex = value; }
    }

    /// <summary>
    /// set css class for control
    /// </summary>
    public string CssClass
    {
        set { _cssClass = value; }
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
    /// get list box
    /// </summary>
    /// <returns></returns>
    public ListBox GetListBox()
    {
        return lbTicket;
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
        sb.Append(String.Format("$('#{0}').chosen({{", lbTicket.ClientID));
        sb.Append("allow_single_deselect: true,");
        sb.Append("placeholder_text_multiple: \"Select Tickets ...\",");
        sb.Append("width: \"100%\"");
        sb.Append("});");
        sb.Append("}");
        sb.Append("</script>");
        litJs.Text = sb.ToString();
    }

    #endregion

}