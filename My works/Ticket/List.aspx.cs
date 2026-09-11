using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class Ticket_List : BasePage
{
    bool closed = false;
    /// <summary>
    /// page load
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        ConfigureForDevice();

        #region event handlers for grids

        for (int i = 1; i <= 5; i++)
        {
            (DesktopShared.Utility.ControlHelper.FindControlRecursive(pnlContainer, String.Format("gridTicket{0}", i)) as UserControl_Grid_TicketResponsive).evAwaitingResponseCleared += gridTicket_evAwaitingResponseCleared;
            (DesktopShared.Utility.ControlHelper.FindControlRecursive(pnlContainer, String.Format("gridTicket{0}", i)) as UserControl_Grid_TicketResponsive).evItemCommand += gridTicket_evItemCommand;
            (DesktopShared.Utility.ControlHelper.FindControlRecursive(pnlContainer, String.Format("gridTicket{0}", i)) as UserControl_Grid_TicketResponsive).evClosedTicket += gridTicket_evClosedTicket;
            //(DesktopShared.Utility.ControlHelper.FindControlRecursive(pnlContainer, String.Format("gridTicket{0}", i)) as UserControl_Grid_TicketResponsive).evBulkClosedTicket += gridTicket_evBulkClosedTicket;
        }

        #endregion

        if (!IsPostBack)
            SetUpPage();
    }

    #region private methods

    /// <summary>
    /// configure for device
    /// </summary>
    private void ConfigureForDevice()
    {
        bool _displayChosenScript = true;
        string _cssClass = "form-control select-chosen";
        if (DesktopShared.Utility.BrowserHelper.IsTabletOrSmartPhone(this.Request))
        {
            _displayChosenScript = false;
            _cssClass = "form-control";
        }

        lbTicketView.DisplayChosenScript = _displayChosenScript;
        lbTicketView.CssClass = _cssClass;
    }

    /// <summary>
    /// grid awating response click event handler
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void gridTicket_evAwaitingResponseCleared(object sender, EventArgs e)
    {
        DesktopShared.Bootstrap.Alert.DisplayMessage(litMessage, "Awating Response flag as been cleared.", DesktopShared.Bootstrap.Alert.AlertType.Success);
    }

    private void gridTicket_evClosedTicket(object sender, EventArgs e)
    {
        //DesktopShared.Bootstrap.Alert.DisplayMessage(litMessage, "Ticket Closed is complete.", DesktopShared.Bootstrap.Alert.AlertType.Success);
        closed = true;
    }

    //private void gridTicket_evBulkClosedTicket(object sender, EventArgs e)
    //{
    //    DesktopShared.Bootstrap.Alert.DisplayMessage(litMessage, "Bulk Closed Tickets is complete.", DesktopShared.Bootstrap.Alert.AlertType.Success);
    //}

    /// <summary>
    /// grid on item command
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void gridTicket_evItemCommand(object sender, EventArgs e)
    {
        RebinAllGrids();
    }

    /// <summary>
    /// set up for initial page load
    /// </summary>
    private void SetUpPage()
    {
        PopulateListBox();
        RebinAllGrids();
    }

    /// <summary>
    /// populate list box
    /// </summary>
    private void PopulateListBox()
    {
        lbTicketView.UserId = DesktopShared.User.UserID;
        lbTicketView.Populate(DesktopShared.User.Ticket.ActiveViewIds, -3, "Assigned To Me");
    }
    /// <summary>
    /// rebind all grids
    /// </summary>
    private void RebinAllGrids()
    {
        phGrid1.Visible = false;
        phGrid2.Visible = false;
        phGrid3.Visible = false;
        phGrid4.Visible = false;
        phGrid5.Visible = false;

        List<int> _viewIds = DesktopShared.User.Ticket.ActiveViewIds;
        List<int> _updatedIds = new List<int>();

        if ((_viewIds == null) || (_viewIds.Count == 0))
            return;

        int _count = 0;
        foreach (int _viewId in _viewIds)
        {
            _count++;
            if (_count > 5)
                break;


            RebindGrid(
                _viewId,
                DesktopShared.Utility.ControlHelper.FindControlRecursive(pnlContainer, String.Format("phGrid{0}", _count)) as PlaceHolder,
                DesktopShared.Utility.ControlHelper.FindControlRecursive(pnlContainer, String.Format("gridTicket{0}", _count)) as UserControl_Grid_TicketResponsive,
                DesktopShared.Utility.ControlHelper.FindControlRecursive(pnlContainer, String.Format("litGrid{0}Header", _count)) as Literal)
                ;
            _updatedIds.Add(_viewId);
        }

        if (_viewIds.Count > 5)
        {
            DesktopShared.User.Ticket.ActiveViewIds = _updatedIds;
            PopulateListBox();
            DesktopShared.Bootstrap.Alert.DisplayMessage(litMessage, "5 Views maximum.", DesktopShared.Bootstrap.Alert.AlertType.Warning);
        }
    }

    /// <summary>
    /// rebind grid
    /// </summary>
    /// <param name="ticketViewId"></param>
    /// <param name="gridPlaceHolder"></param>
    /// <param name="gridTicket"></param>
    /// <param name="litGridHeader"></param>
    private void RebindGrid(int ticketViewId, PlaceHolder gridPlaceHolder, UserControl_Grid_TicketResponsive gridTicket, Literal litGridHeader)
    {
        gridPlaceHolder.Visible = true;

        int? _ticketId = null;
        string _description = "";
        int? _clientId = null;
        int? _assigngedToUserId = null;
        int? _createdByUserId = null;
        DesktopShared.Ticket.Status _status = DesktopShared.Ticket.Status.Open;
        int? _dispositionId = null;
        int? _priorityId = null;
        string _tagName = "";
        bool _tagIsPublic = false;
        int? _teamId = null;
        int? _typeId = null;
        DateTime ? _createdStart = null;
        DateTime? _createdEnd = null;
        DateTime? _updatedStart = null;
        DateTime? _updatedEnd = null;
        int? _viewedTodayUserId = null;
        int? _createdLastDays = null;
        int? _updatedLastDays = null;
        int? _salesEmployeeId = null;
        bool _awatingResponse = false;
        bool _excludehelpdesk = false;


        if (ticketViewId == -3)
        {
            litGridHeader.Text = "Assigned To Me";
            _assigngedToUserId = DesktopShared.User.UserID;
        }
        else
        {
            var objView = new DesktopShared.EntityClasses.TicketViewEntity(ticketViewId);
            if (objView.Fields.State == SD.LLBLGen.Pro.ORMSupportClasses.EntityState.Fetched)
            {
                if (Enum.IsDefined(typeof(DesktopShared.Ticket.Status), objView.TicketStatus))
                    _status = (DesktopShared.Ticket.Status)Enum.Parse(typeof(DesktopShared.Ticket.Status), objView.TicketStatus.Trim(), true);
               
                litGridHeader.Text = objView.Name.Trim();
                _ticketId = objView.TicketId;
                _description = objView.TicketDescription.Trim();
                _clientId = objView.ClientId;
                _assigngedToUserId = objView.AssignedTo;
                _createdByUserId = objView.CreatedByUserId;
                _dispositionId = objView.DispositionId;
                _priorityId = objView.PriorityId;

                #region TAG
                List<DesktopShared.Ticket.Tagged.TicketTag> _selectedTags = new List<DesktopShared.Ticket.Tagged.TicketTag>();
                
                if (objView.TagName.Trim()!=string.Empty)
                {
                    List<string> result = objView.TagName.Trim().Split(new char[] { ',' }).ToList();

                    foreach (string _selected in result)
                    {
                        int _pos1 = _selected.IndexOf("|");
                        int _pos2 = _selected.LastIndexOf("|");

                        if ((_pos1 > 0) && (_pos2 > 0))
                            _selectedTags.Add(new DesktopShared.Ticket.Tagged.TicketTag { Name = _selected.Substring(0, _pos1).Trim(), Public = _selected.Substring(_pos1 + 1, 1) == "1", ClientPublic = _selected.Substring(_pos2 + 1, 1) == "1" });
                    }
                }

                string _tags = string.Empty;

                if (_selectedTags.Count > 0)
                {
                    for (int i = 0; i <= _selectedTags.Count - 1; i++)
                    {
                        if (_tags == string.Empty)
                        {
                            _tags = _selectedTags[i].Name;
                            //if (_selectedTags[i].Public && _selectedTags[i].ClientPublic)
                            //    _tags += " (PC)";
                            //else if (_selectedTags[i].Public && !_selectedTags[i].ClientPublic)
                            //    _tags += " (P)";
                            //else if (!_selectedTags[i].Public && _selectedTags[i].ClientPublic)
                            //    _tags += " (C)";
                        }
                        else
                        {
                            _tags += "," + _selectedTags[i].Name;
                            //if (_selectedTags[i].Public && _selectedTags[i].ClientPublic)
                            //    _tags += " (PC)";
                            //else if (_selectedTags[i].Public && !_selectedTags[i].ClientPublic)
                            //    _tags += " (P)";
                            //else if (!_selectedTags[i].Public && _selectedTags[i].ClientPublic)
                            //    _tags += " (C)";
                        }
                    }
                }
                _tagName = _tags;
                #endregion
                //_tagIsPublic = objView.TagIsPublic;
                _teamId = objView.TeamId;
                _typeId = objView.TicketTypeId;
                _createdStart = objView.CreatedStartDate;
                _createdEnd = objView.CreatedEndDate;
                _updatedStart = objView.UpdatedStartDate;
                _updatedEnd = objView.UpdatedEndDate;
                _createdLastDays = objView.CreatedLastDays;
                _updatedLastDays = objView.UpdatedLastDays;
                _salesEmployeeId = objView.SalesEmployeeeId;
                _awatingResponse = objView.AwaitingResponse;
                if (objView.ExcludeHelpdesk != null)
                    _excludehelpdesk = (bool)objView.ExcludeHelpdesk;
            }
        }

        if (closed == true)
        {
            if (gridTicket.NumberofClosed > 1)
                DesktopShared.Bootstrap.Alert.DisplayMessage(litMessage, "Bulk Closed Ticket is complete.", DesktopShared.Bootstrap.Alert.AlertType.Success);
            else if (gridTicket.NumberofClosed == 1)
                DesktopShared.Bootstrap.Alert.DisplayMessage(litMessage, "Ticket Closed is complete.", DesktopShared.Bootstrap.Alert.AlertType.Success);
        }
        else
            litMessage.Text = "";

        phSearchResults.Visible = true;

        gridTicket.ResetSearch = true;
        gridTicket.SearchTicketNumber = _ticketId;
        gridTicket.SearchDescription = _description;
        gridTicket.SearchClientId = _clientId;
        gridTicket.SearchAssignedToUserId = _assigngedToUserId;
        gridTicket.SearchCreatedByUserId = _createdByUserId;
        gridTicket.SearchStatus = _status;
        gridTicket.SearchDispositionId = _dispositionId;
        gridTicket.SearchCreatedStartDate = _createdStart;
        gridTicket.SearchCreatedEndDate = _createdEnd;
        gridTicket.SearchLastUpdatedStartDate = _updatedStart;
        gridTicket.SearchLastUpdatedEndDate = _updatedEnd;
        gridTicket.SearchViewedTodayUserId = _viewedTodayUserId;
        gridTicket.SearchPriorityId = _priorityId;
        gridTicket.SearchTag = _tagName;
        //gridTicket.SearchTagUserId = _tagIsPublic ? (int?)null : DesktopShared.User.UserID;
        gridTicket.SearchTeamId = _teamId;
        gridTicket.SearchTicketTypeId = _typeId;
        gridTicket.SearchCreatedLastDays = _createdLastDays;
        gridTicket.SearchUpdatedLastDays = _updatedLastDays;
        gridTicket.SearchSalesEmployeeId = _salesEmployeeId;
        gridTicket.SearchAwaitingResponse = _awatingResponse;
        gridTicket.SearchExcludeHelpdesk = _excludehelpdesk;
        //gridTicket.UseColumnPixelWidth = true;

        gridTicket.RebindGrid();
    }

    #endregion

    #region protected events

    /// <summary>
    /// submit button on click
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        DesktopShared.User.Ticket.ActiveViewIds = lbTicketView.SelectedValues;
        RebinAllGrids();
    }

    #endregion
}