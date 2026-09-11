using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using SD.LLBLGen.Pro.ORMSupportClasses;
using System.Data;

public partial class UserControl_ComboBox_Ticket : System.Web.UI.UserControl
{
    
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    #region protected  events

    /// <summary>
    /// combo box on item data bound -> combine fields for text display
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rcbTicket_ItemDataBound(object sender, Telerik.Web.UI.RadComboBoxItemEventArgs e)
    {
        RadComboBoxItem item = (RadComboBoxItem)e.Item;
        System.Data.DataRowView dr = (System.Data.DataRowView)e.Item.DataItem;

        System.Text.StringBuilder sb = new System.Text.StringBuilder();

        //client name -> max of 10 characters 
        string _clientName = dr["CompanyName"].ToString().Trim();
        if (_clientName.Length > 10)
            _clientName = _clientName.Substring(0, 10);
        sb.Append(_clientName + ": ");
        
        //ticket id -> pad left 5
        sb.Append(dr["Id"].ToString().Trim().PadLeft(5, '0') + " - ");

        //ticket summary -> max of 40
        string _summary = dr["Summary"].ToString().Trim();
        if (_summary.Length > 40)
            _summary = _summary.Substring(0, 37) + " ...";
        sb.Append(_summary + " (");
        
        #region assigned to -> truncate if current display text is greater than 63 / to lower

        //last initial
        string _last = dr["UserLastName"].ToString().Trim();
        if (_last.Length > 1)
            _last = _last.Substring(0, 1);
        
        //first name
        string _assignedTo = dr["UserFirstName"].ToString().Trim() + " ";

        _assignedTo += _last;
        
        //truncate name
        if ((sb.Length > 62) && (_assignedTo.Length > 10))
            _assignedTo = _assignedTo.Substring(0, 10);

        sb.Append(_assignedTo.ToLower() + ")");

        #endregion
        
        item.Text = sb.ToString();
        
    } 

    /// <summary>
    /// combo box on items requested
    /// </summary>
    /// <param name="o"></param>
    /// <param name="e"></param>
    protected void rcbTicket_ItemsRequested(object o, RadComboBoxItemsRequestedEventArgs e)
    {
        DataTable dtTicket = null;
        if ((SearchType == TicketType.Client) && (SelectedClientId.HasValue))
            dtTicket = DesktopShared.Ticket.GetAllClientTickets(SelectedClientId.Value);
        else
            dtTicket = DesktopShared.Ticket.GetAllOpenTickets();

        //apply search filter
        string filter = String.Format("convert(Id, 'System.String') like '{0}%' OR Summary like '%{0}%' OR CompanyName like '%{0}%'", e.Text.Trim());
        dtTicket.DefaultView.RowFilter = filter;

        //default sort by display
        dtTicket.DefaultView.Sort = "CompanyName ASC, Id ASC";
        
        rcbTicket.ClearSelection();
        rcbTicket.DataSource = dtTicket;
        rcbTicket.DataTextField = "CompanyName";
        rcbTicket.DataValueField = "Id";
        rcbTicket.DataBind();
    }

    #endregion

    #region public methods

    /// <summary>
    /// clear all items from combo box
    /// </summary>
    public void ClearItems()
    {
        rcbTicket.Text = "";
        rcbTicket.Items.Clear();
        rcbTicket.Items.Add(new RadComboBoxItem());
    }

    #endregion

    #region public properties

    /// <summary>
    /// set/set selected ticket id
    /// </summary>
    public int SelectedTicketId
    {
        get
        {
            if (String.IsNullOrEmpty(rcbTicket.SelectedValue))
                return -1;

            int _selectedId = 0;
            Int32.TryParse(rcbTicket.SelectedValue.Trim(), out _selectedId);
            return _selectedId > 0 ? _selectedId : -1;
        }
        set
        {
            rcbTicket.Text = "";
            rcbTicket.Items.Clear();

            DesktopShared.TypedListClasses.TicketRow ticket = DesktopShared.Ticket.GetTicketTypedListRow(value);
            if (ticket != null)
            {
                #region display text for selected item

                System.Text.StringBuilder sb = new System.Text.StringBuilder();

                //ticket id
                sb.Append(ticket.Pcscdefects.ToString().Trim() + " - ");

                //ticket summary (truncate if too long)
                string _summary = ticket.Summary.Trim();
                if (_summary.Length > 40)
                    _summary = _summary.Substring(0, 37) + " ...";
                sb.Append(_summary + " (");

                //client code
                sb.Append(ticket.ClientCode.Trim() + ") - ");

                //assigned to 
                string _assignedTo = ticket.AssignedToLast.Trim() + ", ";
                _assignedTo += ticket.AssignedToFirst.Trim();
                if (sb.Length > 65)
                    _assignedTo = _assignedTo.Substring(0, 7) + "...";
                sb.Append(_assignedTo);

                #endregion

                //insert rad combo box item
                rcbTicket.Items.Insert(0, new RadComboBoxItem(sb.ToString(), ticket.Pcscdefects.ToString()));
                rcbTicket.SelectedIndex = 0;
            }
        }
    }

    /// <summary>
    /// get selected ticket name
    /// </summary>
    public string SelectedTicketName
    {
        get
        {
            if (!String.IsNullOrEmpty(rcbTicket.SelectedValue))
                return rcbTicket.Text;
            else
                return "";
        }
    }

    /// <summary>
    /// set is required
    /// </summary>
    public bool IsRequired
    {
        set { cvTicket.Visible = value; }
    }

    /// <summary>
    /// set validation error message
    /// </summary>
    public string ValidationErrorMessage
    {
        set { cvTicket.ErrorMessage = value; }
    }

    /// <summary>
    /// set validation group
    /// </summary>
    public string ValidationGroup
    {
        set { cvTicket.ValidationGroup = value; }
    }

    /// <summary>
    /// set combo box control width
    /// </summary>
    public Unit Width
    {
        set { rcbTicket.Width = value; }
    }

    /// <summary>
    /// get/set search ticket type
    /// </summary>
    public TicketType SearchType
    {
        get
        {
            object obj = this.ViewState["SearchType"];
            if (obj == null)
                return TicketType.AllOpen;
            else
                return (TicketType)obj;
        }
        set { this.ViewState["SearchType"] = value; }
    }

    /// <summary>
    /// get/set user id
    /// </summary>
    public int UserId
    {
        get
        {
            object obj = this.ViewState["UserIDForTicketsCB"];
            if (obj == null)
                return DesktopShared.User.UserID;
            else
                return (int)obj;
        }
        set { this.ViewState["UserIDForTicketsCB"] = value; }
    }

    /// <summary>
    /// get/set selected client id 
    /// </summary>
    public int? SelectedClientId
    {
        get
        {
            object obj = this.ViewState["SelectedClientIdForTicketsCB"];
            if (obj == null)
                return DesktopShared.User.UserID;
            else
                return (int)obj;
        }
        set { this.ViewState["SelectedClientIdForTicketsCB"] = value; }
    }

    #endregion

    #region custom validators

    /// <summary>
    /// validate ticket selected from combo box
    /// </summary>
    /// <param name="source"></param>
    /// <param name="args"></param>
    protected void cvTicket_ServerValidate(object source, ServerValidateEventArgs args)
    {
        if (SelectedTicketId < 1)
            ClearItems();

        args.IsValid = SelectedTicketId > 0;
    }

    #endregion

    #region public enum

    /// <summary>
    /// public enum for ticket type
    /// </summary>
    public enum TicketType
    {
        AllOpen,
        Client
    }

    #endregion

}
