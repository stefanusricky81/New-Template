using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class UserControl_Grid_TicketResponsive : System.Web.UI.UserControl
{
    //prod not updated

    /// <summary>
    /// page load
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        ConfigureForDevice();
    }

    #region private methods

    /// <summary>
    /// configure for device
    /// </summary>
    private void ConfigureForDevice()
    {
        if (DesktopShared.Utility.BrowserHelper.IsTabletOrSmartPhone(this.Request))
        {
            rgTicket.RenderMode = RenderMode.Lightweight;
            phTabletPagerCss.Visible = true;
        }
    }

    /// <summary>
    /// configure grid for export
    /// </summary>
    private void ConfigureExport()
    {
        rgTicket.ExportSettings.IgnorePaging = true;
        rgTicket.ExportSettings.OpenInNewWindow = true;
        rgTicket.ExportSettings.ExportOnlyData = true;
        rgTicket.ExportSettings.HideStructureColumns = true;

        rgTicket.ExportSettings.FileName = "Ticket_" + DateTime.Now.ToString("yyyyMMdd-hhmmss");

        //hide command item template
        foreach (GridItem commandItem in this.rgTicket.MasterTableView.GetItems(GridItemType.CommandItem))
        {
            commandItem.Visible = false;
            commandItem.Display = false;
        }

        //hide columns
        rgTicket.MasterTableView.Columns.FindByUniqueName("ActionColumn").Visible = false;
        rgTicket.MasterTableView.Columns.FindByUniqueName("Id").Visible = false;
        rgTicket.MasterTableView.Columns.FindByUniqueName("ClientName").Visible = false;
        rgTicket.MasterTableView.Columns.FindByUniqueName("AssignedToUserName").Visible = false;
        rgTicket.MasterTableView.Columns.FindByUniqueName("UpdateBy").Visible = false;

        //show export columns
        rgTicket.MasterTableView.Columns.FindByUniqueName("ExportId").Visible = true;
        rgTicket.MasterTableView.Columns.FindByUniqueName("ExportClientName").Visible = true;
        rgTicket.MasterTableView.Columns.FindByUniqueName("ExportAssignedToUserName").Visible = true;
    }

    /// <summary>
    /// close and then show ticket merge modal
    /// calling both methods to resolve issue with postbaclks
    /// </summary>
    private void CloseAndShowTicketMerge()
    {
        ScriptManager.RegisterStartupScript(this, this.GetType(), "Close", "CloseMergeTicket();", true);
        ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowMergeTicket", "$('#modal-ticket-merge').modal('show');", true);
    }

    #endregion

    #region public methods

    /// <summary>
    /// rebind grid
    /// </summary>
    public void RebindGrid()
    {
        rgTicket.Visible = true;
        rgTicket.EditIndexes.Clear();
        rgTicket.DataSource = null;
        rgTicket.Rebind();
    }

    #endregion

    #region protected events

    /// <summary>
    /// grid on need data source
    /// </summary>
    /// <param name="source"></param>
    /// <param name="e"></param>
    protected void rgTicket_NeedDataSource(object source, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        int _currentPage = ResetSearch ? 1 : rgTicket.CurrentPageIndex + 1;
        int _resultCount = 0;
        int _resultsPerPage = DesktopShared.User.Ticket.RecordsPerPage;
        rgTicket.PageSize = _resultsPerPage;
        int _excludehelpdesk = 0;
        int? _excludeviews = null;

        if (SearchExcludeHelpdesk == false)
            _excludehelpdesk = 1;
        if (SearchExcludeViews == true)
            _excludeviews = 0;

        string _searchTicketStatuses = "";
        if (SearchStatus == DesktopShared.Ticket.Status.Open)
            _searchTicketStatuses = "76,58";
        else if (SearchStatus == DesktopShared.Ticket.Status.Closed)
            _searchTicketStatuses = "77,59";

        var _dtTicketCount = DesktopShared.StoredProcedureCallerClasses.RetrievalProcedures.ProcSearchTicketsCount(
            SearchTicketNumber, //ticket number
            SearchDescription.Trim(), //ticket description
            SearchClientId, //client id
            SearchAssignedToUserId, //assigned to
            SearchCreatedByUserId, //created by
            _searchTicketStatuses, //ticket status
            SearchDispositionId, //ticket dispostion
            SearchCreatedStartDate, //created start
            SearchCreatedEndDate, //created end
            SearchLastUpdatedStartDate, //last updated start
            SearchLastUpdatedEndDate, //last updated end
            SearchViewedTodayUserId, //viewed today by user id
            SearchPriorityId, //priority id
            SearchTag, //tag value
            SearchTagUserId, //tag user id 
            SearchTeamId, //team id
            SearchTicketTypeId, //type id
            SearchCreatedLastDays, //created last days
            SearchUpdatedLastDays, //updated last days
            SearchSalesEmployeeId, // sales employee id
            SearchAwaitingResponse, //awaiting response
            SearchUpdatedByUserId, // updated by
            SearchCategoryId,
            _excludehelpdesk,
            SearchClientLocationId, 
            SearchClientContactId, 
            SearchReportedby,
            _excludeviews,
            SearchVIP,
            SearchTimeSpent
            );
        if (_dtTicketCount.Rows.Count > 0)
            _resultCount = Convert.ToInt32(_dtTicketCount.Rows[0][0].ToString());

        var _dtTicket = DesktopShared.StoredProcedureCallerClasses.RetrievalProcedures.ProcSearchTickets(
           SearchTicketNumber, //ticket number
            SearchDescription.Trim(), //ticket description
            SearchClientId, //client id
            SearchAssignedToUserId, //assigned to
            SearchCreatedByUserId, //created by
            _searchTicketStatuses, //ticket status
            SearchDispositionId, //ticket dispostion
            SearchCreatedStartDate, //created start
            SearchCreatedEndDate, //created end
            SearchLastUpdatedStartDate, //last updated start
            SearchLastUpdatedEndDate, //last update end
            SearchViewedTodayUserId, //viewed today by user id
            SearchPriorityId, //priority id
            SearchTag, //tag value
            SearchTagUserId, //tag user id 
            SearchTeamId, //team id
            SearchTicketTypeId, //type id
            SearchCreatedLastDays, //created last days
            SearchUpdatedLastDays, //updated last days
            SearchSalesEmployeeId,// sales employee id
            SearchAwaitingResponse, //awaiting response
           _resultsPerPage, //page size
           _currentPage, //current page
           SortColumnName, //sort by
           SortOperator == SD.LLBLGen.Pro.ORMSupportClasses.SortOperator.Ascending ? "asc" : "desc", //sort direction
           SearchUpdatedByUserId, // updated by
           SearchCategoryId,
           _excludehelpdesk,
            SearchClientLocationId,
            SearchClientContactId,
            SearchReportedby,
            _excludeviews,
            SearchVIP,
            SearchTimeSpent
           );

        rgTicket.VirtualItemCount = _resultCount;
        rgTicket.DataSource = _dtTicket;
        ResetSearch = false;
    }

    /// <summary>
    /// grid on pre render
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rgTicket_PreRender(object sender, EventArgs e)
    {
        if (DesktopShared.Utility.BrowserHelper.IsTabletOrSmartPhone(this.Request) || UseColumnPixelWidth)
        {
            int _colIndex = -1;
            rgTicket.MasterTableView.Columns[++_colIndex].HeaderStyle.Width = Unit.Pixel(30);
            rgTicket.MasterTableView.Columns[++_colIndex].HeaderStyle.Width = Unit.Pixel(80);
            rgTicket.MasterTableView.Columns[++_colIndex].HeaderStyle.Width = Unit.Pixel(80);
            rgTicket.MasterTableView.Columns[++_colIndex].HeaderStyle.Width = Unit.Pixel(30);
            rgTicket.MasterTableView.Columns[++_colIndex].HeaderStyle.Width = Unit.Pixel(200);
            rgTicket.MasterTableView.Columns[++_colIndex].HeaderStyle.Width = Unit.Pixel(125);
            rgTicket.MasterTableView.Columns[++_colIndex].HeaderStyle.Width = Unit.Pixel(125);
            rgTicket.MasterTableView.Columns[++_colIndex].HeaderStyle.Width = Unit.Pixel(125);
            rgTicket.MasterTableView.Columns[++_colIndex].HeaderStyle.Width = Unit.Pixel(60);
            rgTicket.MasterTableView.Columns[++_colIndex].HeaderStyle.Width = Unit.Pixel(60);
            rgTicket.MasterTableView.Columns[++_colIndex].HeaderStyle.Width = Unit.Pixel(100);
            rgTicket.MasterTableView.Columns[++_colIndex].HeaderStyle.Width = Unit.Pixel(100);
            rgTicket.MasterTableView.Columns[++_colIndex].HeaderStyle.Width = Unit.Pixel(60);
            rgTicket.MasterTableView.Columns[++_colIndex].HeaderStyle.Width = Unit.Pixel(60);
            rgTicket.MasterTableView.Columns[++_colIndex].HeaderStyle.Width = Unit.Pixel(500);
        }
    }

    /// <summary>
    /// grid on item data bound
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rgTicket_ItemDataBound(object sender, GridItemEventArgs e)
    {
        #region Item or AlternatingItem

        if (e.Item.ItemType == GridItemType.Item || e.Item.ItemType == GridItemType.AlternatingItem)
        {
            DataRowView _row = (DataRowView)e.Item.DataItem;
            GridDataItem _gdi = e.Item as GridDataItem;

            #region get values / format columns

            int _assignedToUserId = 0;
            int.TryParse(_row["AssignedToUserId"].ToString().Trim(), out _assignedToUserId);

            int _dispositionId = 0;
            int.TryParse(_row["TicketDispositionId"].ToString().Trim(), out _dispositionId);

            int _ticketId = Convert.ToInt32(_row["Id"].ToString().Trim());
            string _ticketUrl = String.Format("/Ticket/Detail2.aspx?Id={0}", _ticketId);

            //action column
            LinkButton btnMakeActive = e.Item.FindControl("btnMakeActive") as LinkButton;
            if ((_dispositionId != DesktopShared.SiteHelper.Ticket.Dispostion.Id.Active) || (_assignedToUserId != DesktopShared.User.UserID))
                btnMakeActive.Visible = true;
            

                //ticket id label
            Label lblTicketId = e.Item.FindControl("lblTicketId") as Label;
            lblTicketId.Text = String.Format("<a href=\"{0}\">{1}</a>", _ticketUrl, _ticketId);
            lblTicketId.ToolTip = _ticketId.ToString();

            //awaiting response
            int _awaitingResponseUserId = 0;
            if (int.TryParse(_row["AwaitingResponseUserId"].ToString().Trim(), out _awaitingResponseUserId))
            {
                bool _internalWaitingResponse = false;
                bool.TryParse(_row["AwaitingResponseInternal"].ToString().Trim(), out _internalWaitingResponse);

                LinkButton btnClearAwaitingResponse = e.Item.FindControl("btnClearAwaitingResponse") as LinkButton;
                btnClearAwaitingResponse.Visible = true;
                btnClearAwaitingResponse.Attributes.CssStyle.Add("color", _internalWaitingResponse ? "#3498db" : "#e74c3c");

                lblTicketId.Attributes.CssStyle.Add("color", _internalWaitingResponse ? "#3498db" : "#e74c3c");
                _gdi["Id"].Attributes.CssStyle.Add("background-color", _internalWaitingResponse ? "#dae8f2" : "#ffd1cc");
            }

            string _clientName = _row["ClientName"].ToString().Trim();

            bool _clientPriority = false;
            bool.TryParse(_row["ClientPriority"].ToString().Trim(), out _clientPriority);

            bool _clientUseHelpDeskp = false;
            bool.TryParse(_row["ClientUseHelpDesk"].ToString().Trim(), out _clientUseHelpDeskp);

            string _clientNameDisplay = "";
            if (_clientPriority || _clientUseHelpDeskp)
            {

                if (_clientPriority)
                    _clientNameDisplay += "<i class=\"gi gi-star\" title=\"Priority Client\" style=\"padding-right:5px; color:gold\"></i>";
                if (_clientUseHelpDeskp)
                    _clientNameDisplay += "<i class=\"gi gi-star\" title=\"Helpdesk Client\" style=\"padding-right:5px; color:red\"></i>";
                _clientNameDisplay += String.Format("<strong>{0}</strong>", _clientName);
            }
            else
                _clientNameDisplay = _clientName;

            string _assignedToUserName = _row["AssignedToUserName"].ToString().Trim();
            string _assignedToUserNameDisplay = _assignedToUserName;
            int _assignedToUserClientId = -1;
            int.TryParse(_row["AssignedToUserClientId"].ToString().Trim(), out _assignedToUserClientId);
            if (_assignedToUserClientId != Desktop.SiteHelper.Client.Id.BBB)
                _assignedToUserNameDisplay = String.Format("<strong>{0}</strong><i class=\"gi gi-star\" title=\"Helpdesk Client\" style=\"padding-left:5px; color:Blue\"></i>", _assignedToUserName);

            string _reportedByToolTip = _row["ReportedBy"].ToString().Trim();
            string _reportedBy = _reportedByToolTip;
            int _pos = _reportedBy.IndexOf(" ");
            if (_pos > 0)
                _reportedBy = String.Format("{0} {1}", _reportedBy.Substring(0, 1), _reportedBy.Substring(_pos));
            _reportedBy = DesktopShared.Utility.String.Truncate(_reportedBy, 15);

            /*bool _reportedByVip = false;
            bool.TryParse(_row["ReportedByVip"].ToString().Trim(), out _reportedByVip);
            if (_reportedByVip)
                _reportedBy = String.Format("<strong>{0}</strong> {1}", _reportedBy, DesktopShared.User.VipIcon);*/

            DateTime _tempDate = DateTime.MinValue;
            string _enteredDateDisplay = "";
            if (DateTime.TryParse(_row["DateEntered"].ToString().Trim(), out _tempDate))
                _enteredDateDisplay = _tempDate.ToString("MM/dd/yy HH:mm");

            DateTime _lastupdateDate = DateTime.MinValue;
            string _updatedDateDisplay = "";
            if (DateTime.TryParse(_row["LastUpdated"].ToString().Trim(), out _lastupdateDate))
                _updatedDateDisplay = _lastupdateDate.ToString("MM/dd/yy HH:mm");

            string _summaryAndDescription = String.Format("Summary: {0}\n\nDescription: {1}", _row["Summary"].ToString().Trim(), _row["Description"].ToString().Trim());

            string _status = _row["DispositionName"].ToString().Trim();
            string _statusToolTip = _status;
            string _lastExternalUpdate = _row["LastExternalUpdate"].ToString().Trim();
            if (!String.IsNullOrWhiteSpace(_lastExternalUpdate))
                _statusToolTip += String.Format("{0}{1}", Environment.NewLine, _lastExternalUpdate);
            #endregion

            DesktopShared.Utility.Grid.TelerikHelper.AddLabelToCell(_row["Id"].ToString().Trim(), _gdi["ExportId"]);
            DesktopShared.Utility.Grid.TelerikHelper.AddLabelToCell(_row["PriorityName"].ToString().Trim(), _gdi["PriorityName"]);
            DesktopShared.Utility.Grid.TelerikHelper.AddHyperLinkToCell(_row["Summary"].ToString().Trim(), _summaryAndDescription, _ticketUrl, _gdi["Summary"]);
            DesktopShared.Utility.Grid.TelerikHelper.AddLabelToCell(_clientNameDisplay, _clientName, _gdi["ClientName"]);
            DesktopShared.Utility.Grid.TelerikHelper.AddLabelToCell(_clientName, _gdi["ExportClientName"]);
            DesktopShared.Utility.Grid.TelerikHelper.AddLabelToCell(_reportedBy, _reportedByToolTip, _gdi["ReportedBy"]);
            DesktopShared.Utility.Grid.TelerikHelper.AddLabelToCell(_status, _statusToolTip, _gdi["DispositionName"]);
            DesktopShared.Utility.Grid.TelerikHelper.AddLabelToCell(_row["TicketTypeName"].ToString().Trim(), _gdi["TicketTypeName"]);
            //DesktopShared.Utility.Grid.TelerikHelper.AddLabelToCell(_enteredDateDisplay, _gdi["DateEntered"]);
            TelerikHelper.ToolTip.Ticket.AddLabelAndToolTipToCellForDate(Convert.ToInt32(_ticketId),_tempDate,_row["Description"].ToString().Trim(),_row["ReportedBy"].ToString().Trim(), "","Entered", _gdi["DateEntered"]);

            //DesktopShared.Utility.Grid.TelerikHelper.AddLabelToCell(_updatedDateDisplay, _gdi["LastUpdated"]);
            TelerikHelper.ToolTip.Ticket.AddLabelAndToolTipToCellForDate(Convert.ToInt32(_ticketId), _lastupdateDate, _row["LastExternalUpdate"].ToString().Trim(),_row["UpdateBy"].ToString().Trim(), "", "Updated",_gdi["LastUpdated"] );
            DesktopShared.Utility.Grid.TelerikHelper.AddLabelToCell(_assignedToUserNameDisplay, _assignedToUserName, _gdi["AssignedToUserName"]);
            DesktopShared.Utility.Grid.TelerikHelper.AddLabelToCell(_assignedToUserName, _gdi["ExportAssignedToUserName"]);
            DesktopShared.Utility.Grid.TelerikHelper.AddLabelToCell(_row["Description"].ToString().Trim(), _gdi["Description"]);
        }

        #endregion

        #region grid pager item

        else if (e.Item is GridPagerItem)
        {
            RadComboBox _rcb = (RadComboBox)e.Item.FindControl("PageSizeComboBox");
            RadComboBoxItem _rcbItem = _rcb.Items.FindItemByText(DesktopShared.User.Ticket.RecordsPerPage.ToString());
            if (_rcbItem != null)
                _rcbItem.Selected = true;
        }

        #endregion

    }

    /// <summary>
    /// grid on sort command
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rgTicket_SortCommand(object sender, GridSortCommandEventArgs e)
    {
        GridSortExpression sortExpression = new GridSortExpression();

        //these columns will sort descending on first click
        var _nonAscList = new List<string> { "Id" };
        bool _sortAscending = !_nonAscList.Contains(e.SortExpression.Trim());

        SortColumnName = e.SortExpression.Trim();
        switch (e.OldSortOrder)
        {
            case GridSortOrder.None:
                sortExpression.FieldName = e.SortExpression;
                sortExpression.SortOrder = _sortAscending ? GridSortOrder.Ascending : GridSortOrder.Descending;
                e.Item.OwnerTableView.SortExpressions.AddSortExpression(sortExpression);
                break;
            case GridSortOrder.Ascending:

                sortExpression.FieldName = e.SortExpression;
                sortExpression.SortOrder = _sortAscending ? GridSortOrder.Descending : GridSortOrder.None;
                e.Item.OwnerTableView.SortExpressions.AddSortExpression(sortExpression);
                break;
            case GridSortOrder.Descending:

                sortExpression.FieldName = e.SortExpression;
                sortExpression.SortOrder = _sortAscending ? GridSortOrder.None : GridSortOrder.Ascending;
                e.Item.OwnerTableView.SortExpressions.AddSortExpression(sortExpression);
                break;
        }
        SortOperator = (sortExpression.SortOrder == GridSortOrder.Ascending) ? SD.LLBLGen.Pro.ORMSupportClasses.SortOperator.Ascending : SD.LLBLGen.Pro.ORMSupportClasses.SortOperator.Descending;

        e.Canceled = true;
        rgTicket.CurrentPageIndex = 0;
        rgTicket.Rebind();

        if (!e.Item.OwnerTableView.SortExpressions.ContainsExpression(e.SortExpression))
        {
            sortExpression = new GridSortExpression();
            sortExpression.FieldName = e.SortExpression;
            sortExpression.SortOrder = GridSortOrder.Ascending;
            e.Item.OwnerTableView.SortExpressions.AddSortExpression(sortExpression);
        }
    }

    /// <summary>
    /// grid on item command
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rgTicket_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            bool _rebind = false;
            bool _clearAwaitingResponse = false;
            bool _closedtickets = false;

            #region clear awaiting response click

            if (e.CommandName == "ClearAwaitingResponse")
            {
                int cscDefectID = Convert.ToInt32((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["Id"].ToString());

                DesktopShared.EntityClasses.CscDefectsEntity ticket = new DesktopShared.EntityClasses.CscDefectsEntity(cscDefectID);
                ticket.AwaitingResponseUserId = null;
                ticket.AwaitingResponseInternal = true;
                ticket.Save();

                DesktopShared.Ticket.History.Add(
                      cscDefectID, //ticket id
                      DesktopShared.User.UserID, //user id, 
                      "",//notes
                      "Awaiting Response Tag cleared",//internal notes
                      false, // is new ticket
                      "", // client email addresses
                      "", // employee email addresses
                      false, // send email out
                      DesktopShared.Ticket.History.Type.Id.AwaitingResponseCleared //history type
                      );

                _rebind = true;
                _clearAwaitingResponse = true;
            }

            #endregion

            #region make active ticket

            else if (e.CommandName == "MakeActive")
            {
                int _id = Convert.ToInt32((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["Id"].ToString());
                //int? _currentActiveTicketId = DesktopShared.Ticket.GetActiveId(DesktopShared.User.UserID);
                int _assignedTo = Convert.ToInt32((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["AssignedToUserId"].ToString());
                bool _assignmentChanged = (_assignedTo != DesktopShared.User.UserID);

                //alert current active employee if another employee took over ticket
                string _emailTo = "";
                //if (_assignmentChanged)
                //_emailTo = (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["AssignedToUserEmail"].ToString();

                //make active for employee that clicked make active link
                DesktopShared.Ticket.MakeActive(_id, DesktopShared.User.UserID, DesktopShared.User.UserID, true, _emailTo);

                //if ticket assigned to other employee, call inactive method for that employee
                if (_assignmentChanged)
                    DesktopShared.Ticket.MakeInactive(_id, DesktopShared.User.UserID, _assignedTo, false);

                //set session values used for client combo box on master page header
                //Session["ClientText"] = (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["CompanyName"].ToString();
                //Session["ClientID"] = Convert.ToInt32((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["FkClient"].ToString());

                Response.Redirect(String.Format("/Ticket/Detail2.aspx?Id={0}", _id));//redirect to ticket  
            }

            #endregion

            #region merge tickets click

            else if (e.CommandName == "TicketMerge")
            {
                _rebind = true;
                string _allIds = "";
                foreach (GridDataItem _itemToMerge in rgTicket.SelectedItems)
                {
                    if (_allIds.Trim().Length > 0)
                        _allIds += ",";

                    _allIds += _itemToMerge.GetDataKeyValue("Id");
                }
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowTicketMerge", "$('#modal-ticket-merge').modal('show');", true);

                if (_allIds.Trim().Length > 0)
                {
                    litTicketsToMerge.Text = _allIds;
                    mvTicketMerge.SetActiveView(viewTicketMergeMain);
                    btnTicketMerge.Visible = true;
                }
                else
                {
                    mvTicketMerge.SetActiveView(viewTicketMergeError);
                    btnTicketMerge.Visible = false;
                }
            }

            #endregion

            #region Closed Ticket
            else if (e.CommandName == "Closed")
            {
                int countclosed = 0;
                _rebind = true;
                string _allClosedIds = "";
                foreach (GridDataItem _itemToMerge in rgTicket.SelectedItems)
                {
                    countclosed++;

                    if (_allClosedIds.Trim().Length > 0)
                        _allClosedIds += ",";

                    _allClosedIds += _itemToMerge.GetDataKeyValue("Id");
                    DesktopShared.EntityClasses.CscDefectsEntity _ticket = new DesktopShared.EntityClasses.CscDefectsEntity(Convert.ToInt32(_itemToMerge.GetDataKeyValue("Id")));

                    _ticket.FkStatus = 77;
                    _ticket.FkDisposition = 93;
                    _ticket.Lastupdated = DateTime.Now;
                    _ticket.Save();

                    DesktopShared.EntityClasses.CscHistoryEntity objHistory = new DesktopShared.EntityClasses.CscHistoryEntity();
                    objHistory.CscdefectsId = Convert.ToInt32(_itemToMerge.GetDataKeyValue("Id"));
                    objHistory.Notes = "Closed bulk ticket";
                    objHistory.UpdatedBy = DesktopShared.User.UserID;
                    objHistory.FkStatus = 77;

                    objHistory.Save();
                }
                NumberofClosed = countclosed;
                //_rebind = true;
                _closedtickets = true;
            }
            #endregion

            if (_rebind)
                RebindGrid();

            if (_closedtickets)
                evClosedTicket(this, e);

            if (_clearAwaitingResponse && (evAwaitingResponseCleared != null))
                evAwaitingResponseCleared(this, e);

            if (evItemCommand != null)
                evItemCommand(this, e);
        }
        catch (Exception ex)
        {
            return;
        }
    }

    /// <summary>
    /// grid on item created
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rgTicket_ItemCreated(object sender, GridItemEventArgs e)
    {
        if (MergeTicketsActive)
        {
            if (e.Item.ItemType == GridItemType.CommandItem)
            {
                ((LinkButton)e.Item.FindControl("btnMerge")).Visible = true;
            }
        }
    }

    /// <summary>
    /// grid on page size changed
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rgTicket_PageSizeChanged(object sender, GridPageSizeChangedEventArgs e)
    {
        DesktopShared.User.Ticket.RecordsPerPage = e.NewPageSize;
    }

    /// <summary>
    /// export button click
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnExport_Click(object sender, System.EventArgs e)
    {
        ConfigureExport();
        rgTicket.MasterTableView.ExportToCSV();
    }

    /// <summary>
    /// ticket merge on button click
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnTicketMerge_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            int masterTicketId = Convert.ToInt32(txtMasterTicketNumber.Text.Trim());

            string[] allValues = litTicketsToMerge.Text.Trim().Split(',');
            foreach (string currentValue in allValues)
            {
                int currentTicketId = -1;
                if (int.TryParse(currentValue, out currentTicketId))
                {
                    string errorMessage = "";
                    if (DesktopShared.Ticket.Merge.IsValid(currentTicketId, masterTicketId, ref errorMessage))
                        DesktopShared.Ticket.Merge.CompleteMerge(currentTicketId, masterTicketId, DesktopShared.User.UserID);
                }
            }

            RebindGrid();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Close", "CloseMergeTicket();", true);
            if (evTicketsMerged != null)
                evTicketsMerged(this, e);
        }
        else
        {
            CloseAndShowTicketMerge();
        }
    }


    #region custom validators

    /// <summary>
    /// validate merge master ticket exists
    /// </summary>
    /// <param name="source"></param>
    /// <param name="args"></param>
    protected void cvMasterTicketNumber_ServerValidate(object source, ServerValidateEventArgs args)
    {
        int _mergeMasterTicketId = Convert.ToInt32(txtMasterTicketNumber.Text.Trim());
        DesktopShared.EntityClasses.CscDefectsEntity _masterTicket = new DesktopShared.EntityClasses.CscDefectsEntity(_mergeMasterTicketId);
        args.IsValid = (_masterTicket.Fields.State == SD.LLBLGen.Pro.ORMSupportClasses.EntityState.Fetched);
    }

    #endregion

    #endregion

    #region private properties

    /// <summary>
    /// get/set sort column name
    /// </summary>
    private string SortColumnName
    {
        get
        {
            object obj = this.ViewState["scn_rtg"];
            return (obj == null) ? "Id" : (string)obj;
        }
        set { this.ViewState["scn_rtg"] = value; }
    }

    /// <summary>
    /// get/set sort operator
    /// </summary>
    private SD.LLBLGen.Pro.ORMSupportClasses.SortOperator SortOperator
    {
        get
        {
            object obj = this.ViewState["so_rtg"];
            return (obj == null) ? SD.LLBLGen.Pro.ORMSupportClasses.SortOperator.Descending : (SD.LLBLGen.Pro.ORMSupportClasses.SortOperator)obj;
        }
        set { this.ViewState["so_rtg"] = value; }
    }

    #endregion

    #region public properties

    /// <summary>
    /// get/set merge ticket functionality active status
    /// </summary>
    public bool MergeTicketsActive
    {
        get
        {
            object obj = this.ViewState["mta_rtg"];
            return (obj == null) ? false : (bool)obj;
        }
        set
        {
            this.ViewState["mta_rtg"] = value;
            rgTicket.ClientSettings.Selecting.AllowRowSelect = value;
            rgTicket.AllowMultiRowSelection = value;
        }
    }
    public bool BulkClosedTickets
    {
        get
        {
            object obj = this.ViewState["bulkct_rtg"];
            return (obj == null) ? false : (bool)obj;
        }
        set
        {
            this.ViewState["bulkct_rtg"] = value;
            rgTicket.ClientSettings.Selecting.AllowRowSelect = value;
            rgTicket.AllowMultiRowSelection = value;
        }
    }

    /// <summary>
    /// get/set reset search
    /// </summary>
    public bool UseColumnPixelWidth
    {
        get
        {
            object obj = this.ViewState["ucpw_rtg"];
            return (obj == null) ? false : (bool)obj;
        }
        set { this.ViewState["ucpw_rtg"] = value; }
    }

    /// <summary>
    /// get/set reset search
    /// </summary>
    public bool ResetSearch
    {
        get
        {
            object obj = this.ViewState["rs_rtg"];
            return (obj == null) ? false : (bool)obj;
        }
        set { this.ViewState["rs_rtg"] = value; }
    }

    /// <summary>
    /// get/set search ticket number
    /// </summary>
    public int? SearchTicketNumber
    {
        get
        {
            object obj = this.ViewState["stn_rtg"];
            return (obj == null) ? (int?)null : (int)obj;
        }
        set { this.ViewState["stn_rtg"] = value; }
    }

    /// <summary>
    /// get/set search description
    /// </summary>
    public string SearchDescription
    {
        get
        {
            object obj = this.ViewState["sdes_rtg"];
            return (obj == null) ? "" : (string)obj;
        }
        set { this.ViewState["sdes_rtg"] = value; }
    }

    /// <summary>
    /// get/set search client id
    /// </summary>
    public int? SearchClientId
    {
        get
        {
            object obj = this.ViewState["scid_rtg"];
            return (obj == null) ? (int?)null : (int)obj;
        }
        set { this.ViewState["scid_rtg"] = value; }
    }

    /// <summary>
    /// get/set search assigned to user id
    /// </summary>
    public int? SearchAssignedToUserId
    {
        get
        {
            object obj = this.ViewState["satuid_rtg"];
            return (obj == null) ? (int?)null : (int)obj;
        }
        set { this.ViewState["satuid_rtg"] = value; }
    }

    /// <summary>
    /// get/set search  created by user id 
    /// </summary>
    public int? SearchCreatedByUserId
    {
        get
        {
            object obj = this.ViewState["scbuid_rtg"];
            return (obj == null) ? (int?)null : (int)obj;

        }
        set { this.ViewState["scbuid_rtg"] = value; }
    }

    /// <summary>
    /// get/set search ticket status
    /// </summary>
    public DesktopShared.Ticket.Status SearchStatus
    {
        get
        {
            object obj = this.ViewState["ss_rtg"];
            return (obj == null) ? DesktopShared.Ticket.Status.All : (DesktopShared.Ticket.Status)obj;
        }
        set { this.ViewState["ss_rtg"] = value; }
    }

    /// <summary>
    /// get/set search disposition id
    /// </summary>
    public int? SearchDispositionId
    {
        get
        {
            object obj = this.ViewState["sdid_rtg"];
            return (obj == null) ? (int?)null : (int)obj;

        }
        set { this.ViewState["sdid_rtg"] = value; }
    }

    /// <summary>
    /// get/set search created start date
    /// </summary>
    public DateTime? SearchCreatedStartDate
    {
        get
        {
            object obj = this.ViewState["scst_rtg"];
            return (obj == null) ? (DateTime?)null : (DateTime)obj;

        }
        set { this.ViewState["scst_rtg"] = value; }
    }

    /// <summary>
    /// get/set search created end date
    /// </summary>
    public DateTime? SearchCreatedEndDate
    {
        get
        {
            object obj = this.ViewState["scet_rtg"];
            return (obj == null) ? (DateTime?)null : (DateTime)obj;

        }
        set { this.ViewState["scet_rtg"] = value; }
    }

    /// <summary>
    /// get/set search last updated start date
    /// </summary>
    public DateTime? SearchLastUpdatedStartDate
    {
        get
        {
            object obj = this.ViewState["slust_rtg"];
            return (obj == null) ? (DateTime?)null : (DateTime)obj;

        }
        set { this.ViewState["slust_rtg"] = value; }
    }

    /// <summary>
    /// get/set search last updated end date
    /// </summary>
    public DateTime? SearchLastUpdatedEndDate
    {
        get
        {
            object obj = this.ViewState["sluet_rtg"];
            return (obj == null) ? (DateTime?)null : (DateTime)obj;

        }
        set { this.ViewState["sluet_rtg"] = value; }
    }

    /// <summary>
    /// get/set search viewed today user id
    /// </summary>
    public int? SearchViewedTodayUserId
    {
        get
        {
            object obj = this.ViewState["svtuid_rtg"];
            return (obj == null) ? (int?)null : (int)obj;

        }
        set { this.ViewState["svtuid_rtg"] = value; }
    }

    /// <summary>
    /// get/set search priority id
    /// </summary>
    public int? SearchPriorityId
    {
        get
        {
            object obj = this.ViewState["spid_rtg"];
            return (obj == null) ? (int?)null : (int)obj;

        }
        set { this.ViewState["spid_rtg"] = value; }
    }

    public int? NumberofClosed
    {
        get
        {
            object obj = this.ViewState["numberoc_rtg"];
            return (obj == null) ? (int?)null : (int)obj;

        }
        set { this.ViewState["numberoc_rtg"] = value; }
    }

    // <summary>
    /// get/set search ticket type id
    /// </summary>
    public int? SearchTicketTypeId
    {
        get
        {
            object obj = this.ViewState["sttid_rtg"];
            return (obj == null) ? (int?)null : (int)obj;

        }
        set { this.ViewState["sttid_rtg"] = value; }
    }

    /// <summary>
    /// get/set search tag user id
    /// </summary>
    public int? SearchTagUserId
    {
        get
        {
            object obj = this.ViewState["stuid_rtg"];
            return (obj == null) ? (int?)null : (int)obj;

        }
        set { this.ViewState["stuid_rtg"] = value; }
    }

    /// <summary>
    /// get/set search tag
    /// </summary>
    public string SearchTag
    {
        get
        {
            object obj = this.ViewState["stag_rtg"];
            return (obj == null) ? "" : (string)obj;

        }
        set { this.ViewState["stag_rtg"] = value; }
    }

    /// <summary>
    /// get/set search team id
    /// </summary>
    public int? SearchTeamId
    {
        get
        {
            object obj = this.ViewState["steid_rtg"];
            return (obj == null) ? (int?)null : (int)obj;

        }
        set { this.ViewState["steid_rtg"] = value; }
    }

    /// <summary>
    /// get/set search sales employee id
    /// </summary>
    public int? SearchSalesEmployeeId
    {
        get
        {
            object obj = this.ViewState["sseid_rtg"];
            return (obj == null) ? (int?)null : (int)obj;
        }
        set { this.ViewState["sseid_rtg"] = value; }
    }


    /// <summary>
    /// get/set search created last days
    /// </summary>
    public int? SearchCreatedLastDays
    {
        get
        {
            object obj = this.ViewState["scld_rtg"];
            return (obj == null) ? (int?)null : (int)obj;
        }
        set { this.ViewState["scld_rtg"] = value; }
    }

    /// <summary>
    /// get/set search updated last days
    /// </summary>
    public int? SearchUpdatedLastDays
    {
        get
        {
            object obj = this.ViewState["suld_rtg"];
            return (obj == null) ? (int?)null : (int)obj;
        }
        set { this.ViewState["suld_rtg"] = value; }
    }

    /// <summary>
    /// get/set search awaiting response
    /// </summary>
    public bool SearchAwaitingResponse
    {
        get
        {
            object obj = this.ViewState["sar_rtg"];
            return (obj == null) ? false : (bool)obj;

        }
        set { this.ViewState["sar_rtg"] = value; }

    }

    /// <summary>
    /// get/set search  created by user id 
    /// </summary>
    public int? SearchUpdatedByUserId
    {
        get
        {
            object obj = this.ViewState["updatedbyuserid"];
            return (obj == null) ? (int?)null : (int)obj;

        }
        set { this.ViewState["updatedbyuserid"] = value; }
    }

    /// <summary>
    /// get/set search  created by user id 
    /// </summary>
    public int? SearchCategoryId
    {
        get
        {
            object obj = this.ViewState["categoryid"];
            return (obj == null) ? (int?)null : (int)obj;

        }
        set { this.ViewState["categoryid"] = value; }
    }

    /// <summary>
    /// get/set search awaiting response
    /// </summary>
    public bool SearchExcludeHelpdesk
    {
        get
        {
            object obj = this.ViewState["src_exlhd"];
            return (obj == null) ? false : (bool)obj;

        }
        set { this.ViewState["src_exlhd"] = value; }

    }

    /// <summary>
    /// get/set search  Client location
    /// </summary>
    public int? SearchClientLocationId
    {
        get
        {
            object obj = this.ViewState["sc_clloc"];
            return (obj == null) ? (int?)null : (int)obj;

        }
        set { this.ViewState["sc_clloc"] = value; }
    }

    /// <summary>
    /// get/set search  Client Contact
    /// </summary>
    public int? SearchClientContactId
    {
        get
        {
            object obj = this.ViewState["sc_clcont"];
            return (obj == null) ? (int?)null : (int)obj;

        }
        set { this.ViewState["sc_clcont"] = value; }
    }

    public double? SearchTimeSpent
    {
        get
        {
            object obj = this.ViewState["tm_spent"];
            return (obj == null) ? (double?)null : (double)obj;

        }
        set { this.ViewState["tm_spent"] = value; }
    }

    /// <summary>
    /// get/set search  Reported by
    /// </summary>
    public string SearchReportedby
    {
        get
        {
            object obj = this.ViewState["sc_repby"];
            return (obj == null) ? string.Empty : (string)obj;

        }
        set { this.ViewState["sc_repby"] = value; }
    }

    public bool SearchExcludeViews
    {
        get
        {
            object obj = this.ViewState["sc_exvw"];
            return (obj == null) ? false : (bool)obj;

        }
        set { this.ViewState["sc_exvw"] = value; }
    }

    public bool SearchVIP
    {
        get
        {
            object obj = this.ViewState["sct_vip"];
            return (obj == null) ? false : (bool)obj;

        }
        set { this.ViewState["sct_vip"] = value; }
    }
    #endregion

    #region public events

    /// <summary>
    /// event handler for when awating response is cleared
    /// </summary>
    public event EventHandler evAwaitingResponseCleared;

    /// <summary>
    /// event handler for any item command
    /// </summary>
    public event EventHandler evItemCommand;

    /// <summary>
    /// event handler for when ticket merge is completed
    /// </summary>
    public event EventHandler evTicketsMerged;

    public event EventHandler evClosedTicket;

    #endregion
}

    
