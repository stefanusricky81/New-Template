using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class UserControl_Grid_TicketDashboardResponsive : System.Web.UI.UserControl
{
    private DesktopShared.Ticket.GridType _type = DesktopShared.Ticket.GridType.Employee;

    private UserControl_Grid_TicketDashboardResponsive _taggedTicketUserControl = null;
    private UserControl_Grid_TicketDashboardResponsive _viewTicketUserControl = null;
    private UserControl_Grid_TicketDashboardResponsive _helpDeskTicketUserControl = null;
    private UserControl_Grid_TicketDashboardResponsive _backupTicketUserControl = null;
    private UserControl_Grid_TicketDashboardResponsive _level2QueueTicketUserControl = null;
    private UserControl_Grid_TicketDashboardResponsive _priorityUserTicketUserControl = null;
    private UserControl_Grid_TicketDashboardResponsive _employeeTicketUserControl = null;
    private UserControl_Grid_TicketDashboardResponsive _recentTicketUserControl = null;
    private UserControl_Grid_TicketDashboardResponsive _priorityClientTicketUserControl = null;
    private UserControl_Grid_TicketDashboardResponsive _helpDeskClientTicketUserControl = null;
    private UserControl_Grid_TicketDashboardResponsive _scheduledTicketUserControl = null;
    private UserControl_Grid_TicketDashboardResponsive _designationAlertTicketUserControl = null;

    private string _highlight1 = "#FFC1C1";
    private string _highlight2 = "#FEE5AC";

    private string _gridId = "rgTicket";

    bool _isExport = false;

    protected void Page_Load(object sender, EventArgs e)
    {
        #region event handler for drop down lists

        //set event handler for user ticket tags
        DropDownList ddlUserTicketTags = ucUserTicketTags.GetDropDownList();
        ddlUserTicketTags.AutoPostBack = true;
        ddlUserTicketTags.SelectedIndexChanged += new EventHandler(ddlUserTicketTags_SelectedIndexChanged);

        //set event handler for views
        DropDownList ddlTicketView = ucTicketView.GetDropDownList();
        ddlTicketView.AutoPostBack = true;
        ddlTicketView.SelectedIndexChanged += new EventHandler(ddlTicketView_SelectedIndexChanged);

        #endregion

        //ajax manager, set conditional postback script to cancel ajax on grid export
        Telerik.Web.UI.RadAjaxManager.GetCurrent(this.Page).ClientEvents.OnRequestStart = "conditionalPostback";

        if (!this.IsPostBack)
        {
            SetUpHeader();//set up grid header

            //set saved ticket view selected value
            if (DesktopShared.User.Ticket.TicketViewId.HasValue && SetViewDropDownListSelection && (_type == DesktopShared.Ticket.GridType.View1 || _type == DesktopShared.Ticket.GridType.View))
                ucTicketView.TicketViewId = DesktopShared.User.Ticket.TicketViewId;
            else if (DesktopShared.User.Ticket.TicketView2Id.HasValue && SetViewDropDownListSelection && _type == DesktopShared.Ticket.GridType.View2)
                ucTicketView.TicketViewId = DesktopShared.User.Ticket.TicketView2Id;
            else if (DesktopShared.User.Ticket.TicketView3Id.HasValue && SetViewDropDownListSelection && _type == DesktopShared.Ticket.GridType.View3)
                ucTicketView.TicketViewId = DesktopShared.User.Ticket.TicketView3Id;
        }
    }

    #region Datagrid
    protected void rgTicket_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        //tag value in drop down list
        SetUpTagValue();

        DataTable dtResult = null;
        int currentPage = rgTicket.CurrentPageIndex + 1;
        int resultCount = 0;

        //tag user id
        DesktopShared.Ticket.UserIDForTaggedTicket = SearchTagUserId;

        //max of 500 records
        int _ticketsPerPage = (DesktopShared.User.Ticket.RecordsPerPage == 0) ? DesktopShared.SiteHelper.Grid.MaximumNumberOfRecords : DesktopShared.User.Ticket.RecordsPerPage;

        #region get data table depending on grid type

        bool? _recurringTicketFilter = DesktopShared.User.Ticket.GetRecurring(_type.ToString().Trim());

        if (_type == DesktopShared.Ticket.GridType.Employee)
            dtResult = DesktopShared.Ticket.GetUserTickets(DesktopShared.User.UserID, _recurringTicketFilter, _ticketsPerPage, currentPage, ref resultCount);
        else if (_type == DesktopShared.Ticket.GridType.Active)
            dtResult = DesktopShared.Ticket.GetHotTickets(_recurringTicketFilter, _ticketsPerPage, currentPage, ref resultCount);
        else if (_type == DesktopShared.Ticket.GridType.HelpDesk)
            dtResult = DesktopShared.Ticket.GetHelpDeskTickets(_recurringTicketFilter, _ticketsPerPage, currentPage, ref resultCount);
        else if (_type == DesktopShared.Ticket.GridType.Backup)
            dtResult = DesktopShared.Ticket.GetBackupTickets(_recurringTicketFilter, _ticketsPerPage, currentPage, ref resultCount);
        else if (_type == DesktopShared.Ticket.GridType.Level2Queue)
            dtResult = DesktopShared.Ticket.GetLevel2QueueTickets(_recurringTicketFilter, _ticketsPerPage, currentPage, ref resultCount);
        else if (_type == DesktopShared.Ticket.GridType.HelpDeskClient)
            dtResult = DesktopShared.Ticket.GetHelpDeskClientTickets(_recurringTicketFilter, _ticketsPerPage, currentPage, ref resultCount);
        else if (_type == DesktopShared.Ticket.GridType.Scheduled)
            dtResult = DesktopShared.Ticket.GetScheduledTickets(_recurringTicketFilter, _ticketsPerPage, currentPage, ref resultCount);
        else if (_type == DesktopShared.Ticket.GridType.PriorityClient)
            dtResult = DesktopShared.Ticket.GetPriorityClientTickets(_recurringTicketFilter, _ticketsPerPage, currentPage, ref resultCount);
        else if (_type == DesktopShared.Ticket.GridType.PriorityUser)
            dtResult = DesktopShared.Ticket.GetPriorityUserTickets(_recurringTicketFilter, _ticketsPerPage, currentPage, ref resultCount);
        else if (_type == DesktopShared.Ticket.GridType.ClientHistory)
        {
            _ticketsPerPage = 25;
            resultCount = -1;
            dtResult = DesktopShared.Ticket.GetClientHistoryTickets(SearchClientId, _ticketsPerPage);
            resultCount = 25;
        }
        else if (_type == DesktopShared.Ticket.GridType.Tagged)
            dtResult = DesktopShared.Ticket.GetUserTaggedTickets(DesktopShared.User.UserID, CurrentTag, _recurringTicketFilter, _ticketsPerPage, currentPage, ref resultCount);
        else if (_type == DesktopShared.Ticket.GridType.DesignationAlert)
            dtResult = DesktopShared.Ticket.GetDesignationAlertTickets(_recurringTicketFilter, _ticketsPerPage, currentPage, ref resultCount);

        #region search

        else if (_type == DesktopShared.Ticket.GridType.Search)
        {
            //strip out (P) or (PC)
            string _tagValue = SearchTag.Replace("(P)", "").Replace("(PC)", "").Trim();
            //bool _awaiting =

            dtResult = DesktopShared.Ticket.GetTicketsFromSearch(
                SearchClientId, SearchTicketNumber, SearchEmployeeId, SearchStatus,
                SearchDispositionId, SearchDescription, SearchProductId, SearchTagUserId,
                SearchLastUpdatedStartDate, SearchLastUpdatedEndDate, SearchCreatedStartDate,
                SearchCreatedEndDate, SearchPriorityClient, SearchArchive, SearchReview,
                SearchAwaitingResponse, null, SearchDesignation, _tagValue, SearchTeamId, SearchTierId,
                SearchCreatedLastDays, SearchLastUpdatedLastDays,
                SearchUserViewedLastDaysUserId.HasValue ? SearchUserViewedLastDaysUserId.Value : DesktopShared.User.UserID, //user id for viewed last x days
                SearchUserCreatedLastDaysUserId.HasValue ? SearchUserCreatedLastDaysUserId.Value : DesktopShared.User.UserID, //user id for created last x days
                SearchUserLastUpdatedLastDaysUserId.HasValue ? SearchUserLastUpdatedLastDaysUserId.Value : DesktopShared.User.UserID, //user id for updated last x days
                SearchUserViewedLastDays, //viewed last x days
                SearchUserCreatedLastDays, //created last x days
                SearchUserLastUpdatedLastDays, //updated last x days
                SearchUpdatedByUserStartDate, //updated by user start date
                SearchUpdatedByUserEndDate, //updated by user end date
                SearchUpdatedByUserUserId, //updated by user user id
                SearchScheduledStartDate, //scheduled start date
                SearchScheduledEndDate, //scheduled end date
                SearchScheduledLastDays, //scheduled last x days
                SearchPriorityId, //priority id
                SearchSalesEmployeeId, //sales employee id
                _recurringTicketFilter, // recurring ticket 
                SearchCreatedByClientUserId, //created by client user id
                null, //disposition ids to exclude
                SearchMsp, //msp
                null, //proj mgr id
                null, //tech lead employee id
                IncludeTimeSpent, //include time spent column
                SearchHistory, //search history collection
                SearchTicketTypeId, //ticket type id
                _ticketsPerPage,
                currentPage,
                ref resultCount
                );
        }

        #endregion

        #region view

        else if (IsViewGrid)
        {
            bool showNoResults = false;

            if (ucTicketView.TicketViewId.HasValue && ucTicketView.TicketViewId.Value > 0)
            {
                DesktopShared.EntityClasses.TicketViewEntity _ticketView = new DesktopShared.EntityClasses.TicketViewEntity(ucTicketView.TicketViewId.Value);

                #region entity is fetched, set search criteria

                if (_ticketView.Fields.State == SD.LLBLGen.Pro.ORMSupportClasses.EntityState.Fetched)
                {
                    #region ticket status enum

                    DesktopShared.Ticket.Status _status = DesktopShared.Ticket.Status.Open;
                    if (_ticketView.TicketStatus.Trim().Length > 0)
                    {
                        try
                        {
                            _status = (DesktopShared.Ticket.Status)Enum.Parse(typeof(DesktopShared.Ticket.Status),
                                _ticketView.TicketStatus.Trim(), true);
                        }
                        catch { _status = DesktopShared.Ticket.Status.Open; }
                    }

                    #endregion

                    //list of dispositions ids to exclude
                    List<int> _dispositionIdsToExclude = new List<int>();
                    foreach (string _stringId in _ticketView.TicketDispositionsToExclude.Split(','))
                    {
                        int _id = 0;
                        if (int.TryParse(_stringId, out _id))
                            _dispositionIdsToExclude.Add(_id);
                    }

                    DesktopShared.Ticket.UserIDForTaggedTicket = _ticketView.TagUserId;
                    if (IncludeTimeSpent != _ticketView.IncludeTimeSpent)
                    {
                        IncludeTimeSpent = _ticketView.IncludeTimeSpent;
                    }

                    dtResult = DesktopShared.Ticket.GetTicketsFromSearch(
                        _ticketView.ClientId.HasValue ? _ticketView.ClientId.Value : -1, //client id
                        _ticketView.TicketId.HasValue ? _ticketView.TicketId.Value : -1, //ticket number
                        _ticketView.AssignedTo.HasValue ? _ticketView.AssignedTo.Value : -1, //employee id
                        _status, //status
                        _ticketView.DispositionId.HasValue ? _ticketView.DispositionId.Value : -1, //disposition id
                        _ticketView.TicketDescription.Trim(), //description
                        _ticketView.ProductId.HasValue ? _ticketView.ProductId.Value : -1, //product id
                        _ticketView.TagUserId, //tagged user id 
                        _ticketView.UpdatedStartDate.HasValue ? _ticketView.UpdatedStartDate.Value : DateTime.MinValue, //updated start date
                        _ticketView.UpdatedEndDate.HasValue ? _ticketView.UpdatedEndDate.Value : DateTime.MinValue, //updated end date
                        _ticketView.CreatedStartDate.HasValue ? _ticketView.CreatedStartDate.Value : DateTime.MinValue, //created start date
                        _ticketView.CreatedEndDate.HasValue ? _ticketView.CreatedEndDate.Value : DateTime.MinValue, //created end date
                        _ticketView.PriorityClient, //priority client
                        _ticketView.ArchiveLibrary, //archived
                        _ticketView.ReviewCase, //review
                        _ticketView.AwaitingResponse,  //awaiting reponse 
                        null, //internal only
                        _ticketView.Designation, //designation
                        _ticketView.TagName.Trim(), //tag name
                        _ticketView.TeamId, //team id
                        _ticketView.TierId, //tier id
                        _ticketView.CreatedLastDays, //creatd in last number of days
                        _ticketView.UpdatedLastDays, //last updated in last number of days
                        _ticketView.ViewedByUserLastDaysUserId.HasValue ? _ticketView.ViewedByUserLastDaysUserId.Value : DesktopShared.User.UserID, //viewed by user last number of days user id
                        _ticketView.CreatedByUserLastDaysUserId.HasValue ? _ticketView.CreatedByUserLastDaysUserId.Value : DesktopShared.User.UserID, //created by user last number of days user id
                        _ticketView.UpdatedByUserLastDaysUserId.HasValue ? _ticketView.UpdatedByUserLastDaysUserId.Value : DesktopShared.User.UserID, //updated by user last number of days user id
                        _ticketView.ViewedByUserLastDays, //viewed by user in last number of days
                        _ticketView.CreatedByUserLastDays, //created by user in last number of days
                        _ticketView.UpdatedByUserLastDays, //last updated in user in last number of days
                        _ticketView.UpdatedByUserStartDate, //updated by user start date
                        _ticketView.UpdatedByUserEndDate, //updated by user end date
                        _ticketView.UpdatedByUserUserId, //updated by user user id
                        _ticketView.ScheduledStartDate, //scheduled start date 
                        _ticketView.ScheduledEndDate, //scheduled end date 
                        _ticketView.ScheduledLastDays, //scheduled last x days
                        _ticketView.PriorityId, //priority id
                        _ticketView.SalesEmployeeeId, //sales employee id
                        _recurringTicketFilter, //recurring ticket
                        _ticketView.CreatedByClientUserId, //search created by client user id
                        _dispositionIdsToExclude, //disposition ids to exclude
                        _ticketView.Msp, //msp
                        null, //proj mgr id
                        null, //lead employee id
                        _ticketView.IncludeTimeSpent, //include time spent,
                        _ticketView.SearchHistory, //search history
                        _ticketView.TicketTypeId, //ticket type
                         _ticketsPerPage,
                        currentPage,
                        ref resultCount
                        );
                }

                #endregion

                else //entity is not fetched - do not display any records
                    showNoResults = true;
            }
            else //no value selected in view drop down list
                showNoResults = true;


            #region do not show any records

            if (showNoResults)
            {
                dtResult = new DataTable();
                resultCount = 0;
            }

            #endregion
        }

        #endregion

        #endregion

        #region sort

        string _fieldName = DesktopShared.User.Ticket.GridSort.GetFieldName(_type).Trim(); //sort field name stored in cookie
        if ((_fieldName.Length > 0) && (!this.Page.IsPostBack))
        {
            //sort operator
            Telerik.Web.UI.GridSortOrder _gridSortOrder = GridSortOrder.Ascending;
            string _enum = DesktopShared.User.Ticket.GridSort.GetOperator(_type);
            if (_enum.Length > 0)
            {
                try { _gridSortOrder = (Telerik.Web.UI.GridSortOrder)Enum.Parse(typeof(Telerik.Web.UI.GridSortOrder), _enum, true); }
                catch { _gridSortOrder = GridSortOrder.Ascending; }
            }

            GridSortExpression sortExperssion = new GridSortExpression();
            sortExperssion.FieldName = _fieldName;
            sortExperssion.SortOrder = _gridSortOrder;
            rgTicket.MasterTableView.SortExpressions.AddSortExpression(sortExperssion);
        }

        if (rgTicket.MasterTableView.SortExpressions.Count == 0)
        {
            GridSortExpression sortExperssion = new GridSortExpression();
            sortExperssion.FieldName = "DateEntered";
            sortExperssion.SortOrder = GridSortOrder.Descending;
            rgTicket.MasterTableView.SortExpressions.AddSortExpression(sortExperssion);
        }


        #endregion

        //rgTicket.VirtualItemCount = dtResult.Rows.Count;
        rgTicket.DataSource = dtResult;

        if (DesktopShared.User.Ticket.RecordsPerPage > 0)
            rgTicket.PageSize = DesktopShared.User.Ticket.RecordsPerPage;
        else
        {
            if (dtResult.Rows.Count > 0)
                rgTicket.PageSize = dtResult.Rows.Count;
        }

        litLastUpdated.Text = DateTime.Now.ToString();
    }

    protected void rgTicket_ItemDataBound(object sender, GridItemEventArgs e)
    {
        #region GridEditFormItem and IsInEditMode - add ticket, edit 1 ticket, or batch edit
        if (e.Item is GridEditFormItem && e.Item.IsInEditMode)
        {
            rgTicket.MasterTableView.NoMasterRecordsText = "";

            #region edit existing

            if (!e.Item.OwnerTableView.IsItemInserted)
            {
                #region default edit one item

                if (!IsBatchEditMode)
                {
                    AbstractTicketEdit ucTicketEdit = e.Item.FindControl(GridEditFormItem.EditFormUserControlID) as AbstractTicketEdit;

                    DataRowView drv = (DataRowView)e.Item.DataItem;
                    ucTicketEdit.LoadValues(Convert.ToInt32(drv["Id"].ToString().Trim()));
                }

                #endregion

                #region batch edit all

                else
                {
                    AbstractTicketBatchEdit ucTicketEdit = e.Item.FindControl(GridEditFormItem.EditFormUserControlID) as AbstractTicketBatchEdit;

                    DataRowView drv = (DataRowView)e.Item.DataItem;
                    ucTicketEdit.LoadValues(Convert.ToInt32(drv["Id"].ToString().Trim()));
                }

                #endregion

            }

            #endregion

            #region add new item

            else
            {
                AbstractTicketAdd ucTicketAdd = e.Item.FindControl(GridEditFormItem.EditFormUserControlID) as AbstractTicketAdd;

                ucTicketAdd.FormAddMode = AbstractTicketAdd.AddMode.Grid;
                ucTicketAdd.SetUpPage();
            }

            #endregion
        }
        #endregion

        #region Item or AlternatingItem
        if (e.Item.ItemType == GridItemType.Item || e.Item.ItemType == GridItemType.AlternatingItem)
        {
            GridDataItem dataItem = (GridDataItem)e.Item;
            DataRowView drv = (DataRowView)dataItem.DataItem;

            int ticketId = BitByBit.Utility.DataBase.ConvertHelper.ConvertToInteger(dataItem.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["Id"], -1);
            int _dispositionId = BitByBit.Utility.DataBase.ConvertHelper.ConvertToInteger(dataItem.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["FkDisposition"], -1);
            int _assignedTo = BitByBit.Utility.DataBase.ConvertHelper.ConvertToInteger(dataItem.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["AssignedTo"], -1);

            //Action column
            #region Action
            LinkButton lnkbtnActive = (e.Item.FindControl("btnMakeActive") as LinkButton);
            if ((_dispositionId != DesktopShared.SiteHelper.Ticket.Dispostion.Id.Active) || (_assignedTo != DesktopShared.User.UserID))
            {
                lnkbtnActive.Text = "Active";
                lnkbtnActive.ToolTip = "Make this ticket Active. Any other active tickets assigned will be changed to idle.";
            }
            else
                lnkbtnActive.Text = "Edit";
            #endregion

            //Awaiting Response
            #region awaiting response

            int _awaitingResponseUserId = BitByBit.Utility.DataBase.ConvertHelper.ConvertToInteger(drv["AwaitingResponseUserId"], 0);
            //int _awaitingResponseUserId = 0;
            //try { _awaitingResponseUserId = Convert.ToInt32(dataItem.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["AwaitingResponseUserId"].ToString()); }
            //catch { _awaitingResponseUserId = -1; }
            LinkButton linkClearAwaitingResponse = (e.Item.FindControl("btnClearAwaitingResponse") as LinkButton);
            Label lblID = (e.Item.FindControl("lblTicketNo") as Label);
            if (_awaitingResponseUserId > 0)
            {
                bool _internalWaitingResponse = BitByBit.Utility.DataBase.ConvertHelper.ConvertToBoolean(drv["AwaitingResponseInternal"], false);
                //dataItem["Id"].Attributes.CssStyle.Add("background-color", _internalWaitingResponse ? "#1E90FF" : "#EE0000");

                //Literal litId = new Literal();
                //litId.Text = String.Format("<span style=\"color:White;\">{0}</span>&nbsp;&nbsp;&nbsp;", dataItem.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["Id"].ToString());
                //dataItem["Id"].Controls.Clear();
                //dataItem["Id"].Controls.Add(litId);

                //linkClearAwaitingResponse.Attributes.CssStyle.Add("color", "White");
                //linkClearAwaitingResponse.CausesValidation = false;
                linkClearAwaitingResponse.Text = "X";
                linkClearAwaitingResponse.Visible = true;
                linkClearAwaitingResponse.Attributes.CssStyle.Add("color", _internalWaitingResponse ? "#3498db" : "#e74c3c");

                lblID.Attributes.CssStyle.Add("color", _internalWaitingResponse ? "#3498db" : "#e74c3c");
                dataItem["Id"].Attributes.CssStyle.Add("background-color", _internalWaitingResponse ? "#dae8f2" : "#ffd1cc");
            }
            else
               linkClearAwaitingResponse.Visible = false;

            #endregion

            //Company Name
            #region Company Name
            #region priority client and/or helpdesk client
            if (HighlightPriorityClient || HighlightDesktopClient)
            {  
                //company name
                string companyName = dataItem.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["CompanyName"].ToString().Trim();

                bool isPriority = Convert.ToBoolean((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["Priority"].ToString());
                bool isHelpDesk = Convert.ToBoolean((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["UseHelpDesk"].ToString());


                if (isPriority || isHelpDesk)
                {
                    dataItem["CompanyName"].Attributes.CssStyle.Add("font-weight", "bold");

                    //company name in bold
                    Literal litName = new Literal();
                    litName.Text = companyName + " ";
                    dataItem["CompanyName"].Controls.Add(litName);

                    //priority icon
                    if (isPriority)
                    {
                        Image imgStar = new Image();
                        imgStar.ImageUrl = "/Images/gold-star.png";
                        imgStar.AlternateText = "Priority Client";
                        imgStar.Attributes.Add("Title", "Priority Client");
                        dataItem["CompanyName"].Controls.Add(imgStar);

                        Literal litSpacer = new Literal();
                        litSpacer.Text = "&nbsp;";
                        dataItem["CompanyName"].Controls.Add(litSpacer);
                    }

                    //help desk icon
                    if (isHelpDesk)
                    {
                        Image imgRedStar = new Image();
                        imgRedStar.ImageUrl = "/Images/red-star.png";
                        imgRedStar.AlternateText = "Help Desk Client";
                        imgRedStar.Attributes.Add("Title", "Help Desk Client");
                        dataItem["CompanyName"].Controls.Add(imgRedStar);
                    }
                }
            }
            #endregion
            #endregion

            #region entered - description tooltip

            DateTime _enteredDate = DateTime.MinValue;
            DateTime.TryParse(drv["DateEntered"].ToString(), out _enteredDate);
            if (_enteredDate != DateTime.MinValue)
            {
                string _enteredtooltip = "Entered By " + drv["EnteredByFirstName"].ToString().Trim() + " " + drv["EnteredByLastName"].ToString().Trim() + " (" + _enteredDate.ToString("dddd MM/dd/yy HH:mm") + ")";
                string _finalenteredttooltip = String.Format("{0} \n {1}", _enteredtooltip, drv["Description"].ToString().Trim());
                dataItem["DateEntered"].ToolTip = _finalenteredttooltip;
                dataItem["DateEntered"].Text = _enteredDate.ToString();
                //TelerikHelper.ToolTip.Ticket.AddLabelAndToolTipToCellForDate(
                //    Convert.ToInt32(ticketId), //ticket id
                //    _enteredDate, //ticket entered date
                //    drv["Description"].ToString().Trim(), //ticket entered description
                //    drv["EnteredByFirstName"].ToString().Trim(),  //entered by first name
                //    drv["EnteredByLastName"].ToString().Trim(), //entered by last name
                //    "Entered",  //action type
                //    dataItem["DateEntered"] //telerik cell
                //    );
            }
            else
                dataItem["DateEntered"].Text = "";

            #endregion

            #region last updated
            DateTime _updatedDate = DateTime.MinValue;
            DateTime.TryParse(drv["LastUpdated"].ToString(), out _updatedDate);
            if (_updatedDate != DateTime.MinValue)
            {
                string _updatetooltip = "Updated By " + drv["UpdatedByFirstName"].ToString().Trim() + " " + drv["UpdatedByLastName"].ToString().Trim() + " (" + _updatedDate.ToString("dddd MM/dd/yy HH:mm") + ")";
                string _finalupdatetooltip = String.Format("{0} \n {1}", _updatetooltip, drv["LastUpdatedNotes"].ToString().Trim());
                dataItem["LastUpdated"].ToolTip = _finalupdatetooltip;
                dataItem["LastUpdated"].Text = _updatedDate.ToString().Trim();
                //TelerikHelper.ToolTip.Ticket.AddLabelAndToolTipToCellForDate(
                //Convert.ToInt32(ticketId), //ticket id
                //_updatedDate, //ticket updated date
                //drv["LastUpdatedNotes"].ToString().Trim(), //ticket updated description
                //drv["UpdatedByFirstName"].ToString().Trim(),  //updated by first name
                //drv["UpdatedByLastName"].ToString().Trim(), //updated by last name
                //"Updated", //action type
                //dataItem["LastUpdated"] //telerik cell
                //);
            }
            else
                dataItem["LastUpdated"].Text = "";

            #region higlight cell based on last updated

            if (LastUpdatedWarning > 0)
            {
                DateTime entered = Convert.ToDateTime(dataItem["DateEntered"].Text);
                DateTime lastUpdated = Convert.ToDateTime(dataItem["LastUpdated"].Text);

                bool highlightCell = false;
                TimeSpan ts = DateTime.Now.Subtract(lastUpdated);

                if (entered == lastUpdated)
                {
                    if (ts.TotalHours > LastUpdatedWarning)
                        highlightCell = true;
                }

                if (HighlightBasedOnLastUpdated)
                {
                    if (highlightCell)
                        dataItem["LastUpdated"].Attributes.CssStyle.Add("background-color", _highlight2);
                }

                if (HighlightNotUpdatedFor24Hours)
                {
                    if (ts.TotalHours > 24)
                        dataItem["LastUpdated"].Attributes.CssStyle.Add("background-color", _highlight1);
                }
            }

            #endregion

            #endregion
            //Assigned
            #region Assigned

            //if(dataItem.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["UserFirstName"])
            string _assignedToName = dataItem.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["UserFirstName"].ToString().Trim();
            string _lastName = dataItem.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["UserLastName"].ToString().Trim();
            _assignedToName += String.Format("{0}", _lastName.Length > 0 ? String.Format(" {0}", _lastName.Substring(0, 1).ToUpper()) : "");
            dataItem["UserFirstName"].Text = _assignedToName;

            int assignedToClientId = 0;
            try { assignedToClientId = Convert.ToInt32(dataItem.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["UserClientId"].ToString()); }
            catch { assignedToClientId = -1; }
            if (assignedToClientId != Desktop.SiteHelper.Client.Id.BBB)
            {
                //user name in bold
                string userName = dataItem.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["UserFirstName"].ToString().Trim();
                dataItem["UserFirstName"].Attributes.CssStyle.Add("font-weight", "bold");

                Literal litName = new Literal();
                litName.Text = userName + " ";
                dataItem["UserFirstName"].Controls.Add(litName);

                //icon
                Image imgBlueStar = new Image();
                imgBlueStar.ImageUrl = "/Images/blue-star.png";
                imgBlueStar.AlternateText = "Help Desk Client User";
                imgBlueStar.Attributes.Add("Title", "Help Desk Client User");
                dataItem["UserFirstName"].Controls.Add(imgBlueStar);
            }
            #endregion

            //Tag
            #region Tag
            LinkButton lnkbtnTag = (e.Item.FindControl("lbTagged") as LinkButton);
            int taggedTicketId = 0;
            try { taggedTicketId = Convert.ToInt32(dataItem.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["TaggedTicketId"].ToString()); }
            catch { taggedTicketId = -1; }

            bool addDeleteTag = false;
            if (taggedTicketId > 0)
            {
                var _tagValue = ucUserTicketTags.TagName.Replace("(P)", "").Replace("(PC)", "").Trim();
                if (!String.IsNullOrWhiteSpace(_tagValue))
                {
                    //check tag text vaue vs current selection in drop down list
                    addDeleteTag = DesktopShared.Ticket.Tagged.TicketIsTaggedByValue(ticketId, TagIsPublic ? (int?)null : DesktopShared.User.UserID, _tagValue);
                }
            }
            if (addDeleteTag)
            {
                lnkbtnTag.Text = "X";
                lnkbtnTag.ToolTip = "Delete Tag";
                lnkbtnTag.CommandName = "RemoveTag";
                lnkbtnTag.Attributes.CssStyle.Add("color", "#DB2929");
            }
            else
            {
                lnkbtnTag.Text = "+";
                lnkbtnTag.ToolTip = "Add Tag";
                lnkbtnTag.CommandName = "AddTag";
            }
            #endregion
        }
        #endregion

        #region Remark
        /*
        #region GridEditFormItem and IsInEditMode - add ticket, edit 1 ticket, or batch edit

        if (e.Item is GridEditFormItem && e.Item.IsInEditMode)
        {
            rgTicket.MasterTableView.NoMasterRecordsText = "";

            #region edit existing

            if (!e.Item.OwnerTableView.IsItemInserted)
            {
                #region default edit one item

                if (!IsBatchEditMode)
                {
                    AbstractTicketEdit ucTicketEdit = e.Item.FindControl
                        (GridEditFormItem.EditFormUserControlID) as AbstractTicketEdit;

                    DataRowView drv = (DataRowView)e.Item.DataItem;
                    ucTicketEdit.LoadValues(Convert.ToInt32(drv["Id"].ToString().Trim()));
                }

                #endregion

                #region batch edit all

                else
                {
                    AbstractTicketBatchEdit ucTicketEdit = e.Item.FindControl
                        (GridEditFormItem.EditFormUserControlID) as AbstractTicketBatchEdit;

                    DataRowView drv = (DataRowView)e.Item.DataItem;
                    ucTicketEdit.LoadValues(Convert.ToInt32(drv["Id"].ToString().Trim()));
                }

                #endregion

            }

            #endregion

            #region add new item

            else
            {
                AbstractTicketAdd ucTicketAdd = e.Item.FindControl
                    (GridEditFormItem.EditFormUserControlID) as AbstractTicketAdd;

                ucTicketAdd.FormAddMode = AbstractTicketAdd.AddMode.Grid;
                ucTicketAdd.SetUpPage();
            }

            #endregion

        }

        #endregion

        #region Item or AlternatingItem

        if (e.Item.ItemType == GridItemType.Item || e.Item.ItemType == GridItemType.AlternatingItem)
        {
            GridEditableItem item = e.Item as GridEditableItem;
            DataRowView drv = (DataRowView)e.Item.DataItem;

            bool isTagged = BitByBit.Utility.DataBase.ConvertHelper.ConvertToInteger(drv["TaggedTicketId"], -1) > 0;
            string _ticketId = item["Id"].Text.Trim();

            #region tag link

            TableCell tcTag = item["Tag"];

            LinkButton linkTag = new LinkButton();
            linkTag.ID = "linkTag";
            linkTag.CausesValidation = false;

            bool addDeleteTag = false;
            if (isTagged)
            {
                var _tagValue = ucUserTicketTags.TagName.Replace("(P)", "").Replace("(PC)", "").Trim();
                if (!String.IsNullOrWhiteSpace(_tagValue))
                {
                    //check tag text vaue vs current selection in drop down list
                    addDeleteTag = DesktopShared.Ticket.Tagged.TicketIsTaggedByValue(
                        BitByBit.Utility.DataBase.ConvertHelper.ConvertToInteger(_ticketId, -1),
                        TagIsPublic ? (int?)null : DesktopShared.User.UserID,
                        _tagValue);
                }
            }

            if (addDeleteTag)
            {
                linkTag.Text = "X";
                linkTag.ToolTip = "Delete Tag";
                linkTag.CommandName = "RemoveTag";
                linkTag.Attributes.CssStyle.Add("color", "#DB2929");
            }
            else
            {
                linkTag.Text = "+";
                linkTag.ToolTip = "Add Tage";
                linkTag.CommandName = "AddTag";
            }

            tcTag.Controls.Add(linkTag);

            #endregion

            //priority
            bool _highlightPriority = drv["PriorityName"].ToString().Trim() == "0";
            string _priority = String.Format("{0}{1}{2}", _highlightPriority ? "<strong>" : "", drv["PriorityName"].ToString().Trim(), _highlightPriority ? "</strong>" : "");
            TelerikHelper.AddLabelToCell(_priority, item["PriorityName"]);

            #region ticket id col -> ticket awaiting response / recurring ticket

            TableCell tcId = item["Id"];
            tcId.Controls.Clear();

            Label lblId = new Label();
            lblId.Text = _ticketId;
            lblId.ID = "lblId_" + _ticketId;
            tcId.Controls.Add(lblId);

            #region awaiting response

            int _awaitingResponseUserId = BitByBit.Utility.DataBase.ConvertHelper.ConvertToInteger(drv["AwaitingResponseUserId"], 0);

            if (_awaitingResponseUserId > 0)
            {
                bool _internalWaitingResponse = BitByBit.Utility.DataBase.ConvertHelper.ConvertToBoolean(drv["AwaitingResponseInternal"], false);
                item["Id"].Attributes.CssStyle.Add("background-color", _internalWaitingResponse ? "#1E90FF" : "#EE0000");

                Literal litId = new Literal();
                litId.Text = String.Format("<span style=\"color:White;\">{0}</span>&nbsp;&nbsp;&nbsp;", _ticketId);
                tcId.Controls.Clear();
                tcId.Controls.Add(litId);

                LinkButton linkClearAwaitingResponse = new LinkButton();
                linkClearAwaitingResponse.Attributes.CssStyle.Add("color", "White");
                linkClearAwaitingResponse.ID = "linkClearAwaitingResponse";
                linkClearAwaitingResponse.CausesValidation = false;
                linkClearAwaitingResponse.Text = "X";
                linkClearAwaitingResponse.ToolTip = "Clear Awaiting Reponse Flag";
                linkClearAwaitingResponse.CommandName = "ClearClearAwaitingResponse";

                tcId.Controls.Add(linkClearAwaitingResponse);
            }

            #endregion

            #region recurring ticket

            if (BitByBit.Utility.DataBase.ConvertHelper.ConvertToBoolean(drv["IsRecurring"], false))
            {
                tcId.Controls.Add(new LiteralControl("&nbsp;"));

                Image imgRecurringTicket = new Image();
                imgRecurringTicket.ImageUrl = DesktopShared.SiteHelper.Icon.RecurringTicket;
                imgRecurringTicket.AlternateText = "Recurring Ticket";
                imgRecurringTicket.Attributes.Add("Title", "Recurring Ticket");
                imgRecurringTicket.ToolTip = "Recurring Ticket";
                imgRecurringTicket.ImageAlign = ImageAlign.AbsBottom;
                tcId.Controls.Add(imgRecurringTicket);
            }

            #endregion

            #endregion

            #region priority client and/or helpdesk client

            if (HighlightPriorityClient || HighlightDesktopClient)
            {
                TableCell tcCompanyName = item["CompanyName"];
                bool isPriority = BitByBit.Utility.DataBase.ConvertHelper.ConvertToBoolean(drv["Priority"], false);
                bool isHelpDesk = BitByBit.Utility.DataBase.ConvertHelper.ConvertToBoolean(drv["UseHelpDesk"], false);

                if (isPriority || isHelpDesk)
                {
                    //company name in bold
                    tcCompanyName.Attributes.CssStyle.Add("font-weight", "bold");
                    Literal litName = new Literal();
                    litName.Text = drv["CompanyName"].ToString().Trim() + " ";
                    tcCompanyName.Controls.Add(litName);

                    //priority icon
                    if (isPriority)
                    {
                        Image imgStar = new Image();
                        imgStar.ImageUrl = "/Images/gold-star.png";
                        imgStar.AlternateText = "Priority Client";
                        imgStar.Attributes.Add("Title", "Priority Client");
                        tcCompanyName.Controls.Add(imgStar);

                        Literal litSpacer = new Literal();
                        litSpacer.Text = "&nbsp;";
                        tcCompanyName.Controls.Add(litSpacer);
                    }

                    //help desk icon
                    if (isHelpDesk)
                    {
                        Image imgRedStar = new Image();
                        imgRedStar.ImageUrl = "/Images/red-star.png";
                        imgRedStar.AlternateText = "Help Desk Client";
                        imgRedStar.Attributes.Add("Title", "Help Desk Client");
                        tcCompanyName.Controls.Add(imgRedStar);
                    }
                }
            }

            #endregion

            #region entered - description tooltip

            DateTime _enteredDate = DateTime.MinValue;
            DateTime.TryParse(drv["DateEntered"].ToString(), out _enteredDate);
            if (_enteredDate != DateTime.MinValue)
            {

                string _enteredtooltip = "Entered By " + drv["EnteredByFirstName"].ToString().Trim() + " " + drv["EnteredByLastName"].ToString().Trim() + " (" + _enteredDate.ToString("dddd MM/dd/yy HH:mm") + ")";
                string _finalenteredttooltip = String.Format("{0} \n {1}", _enteredtooltip, drv["Description"].ToString().Trim());
                item["DateEntered"].ToolTip = _finalenteredttooltip;
                item["DateEntered"].Text = _enteredDate.ToString();
                //TelerikHelper.ToolTip.Ticket.AddLabelAndToolTipToCellForDate(
                //    Convert.ToInt32(_ticketId), //ticket id
                //    _enteredDate, //ticket entered date
                //    drv["Description"].ToString().Trim(), //ticket entered description
                //    drv["EnteredByFirstName"].ToString().Trim(),  //entered by first name
                //    drv["EnteredByLastName"].ToString().Trim(), //entered by last name
                //    "Entered",  //action type
                //    item["DateEntered"] //telerik cell
                //    );
            }
            else
                item["DateEntered"].Text = "";

            #endregion

            #region last updated

            TableCell tcLastUpdated = item["LastUpdated"];

            DateTime _updatedDate = DateTime.MinValue;
            DateTime.TryParse(drv["LastUpdated"].ToString(), out _updatedDate);
            if (_updatedDate != DateTime.MinValue)
            {
                string _updatetooltip = "Updated By " + drv["UpdatedByFirstName"].ToString().Trim() + " " + drv["UpdatedByLastName"].ToString().Trim() + " (" + _updatedDate.ToString("dddd MM/dd/yy HH:mm") + ")";
                string _finalupdatetooltip = String.Format("{0} \n {1}", _updatetooltip, drv["LastUpdatedNotes"].ToString().Trim());
                item["LastUpdated"].ToolTip = _finalupdatetooltip;
                item["LastUpdated"].Text = _updatedDate.ToString();
                //TelerikHelper.ToolTip.Ticket.AddLabelAndToolTipToCellForDate(
                //Convert.ToInt32(_ticketId), //ticket id
                //_updatedDate, //ticket updated date
                //drv["LastUpdatedNotes"].ToString().Trim(), //ticket updated description
                //drv["UpdatedByFirstName"].ToString().Trim(),  //updated by first name
                //drv["UpdatedByLastName"].ToString().Trim(), //updated by last name
                //"Updated", //action type
                //item["LastUpdated"] //telerik cell
                //);
            }
            else
                item["LastUpdated"].Text = "";

            #region higlight cell based on last updated

            if (LastUpdatedWarning > 0)
            {
                DateTime entered = Convert.ToDateTime(item["DateEntered"].Text);
                DateTime lastUpdated = Convert.ToDateTime(item["LastUpdated"].Text);

                bool highlightCell = false;
                TimeSpan ts = DateTime.Now.Subtract(lastUpdated);

                if (entered == lastUpdated)
                {
                    if (ts.TotalHours > LastUpdatedWarning)
                        highlightCell = true;
                }

                if (HighlightBasedOnLastUpdated)
                {
                    if (highlightCell)
                        item["LastUpdated"].Attributes.CssStyle.Add("background-color", _highlight2);
                }

                if (HighlightNotUpdatedFor24Hours)
                {
                    if (ts.TotalHours > 24)
                        item["LastUpdated"].Attributes.CssStyle.Add("background-color", _highlight1);
                }
            }

            #endregion

            #endregion

            #region scheduled date highlight if past due

            TableCell tcScheduledDate = item["ScheduledDate"];
            if (tcScheduledDate != null)
            {
                DateTime scheduled = DateTime.MinValue;
                try { scheduled = Convert.ToDateTime(drv["ScheduledDate"].ToString().Trim()); }
                catch { scheduled = DateTime.MinValue; }

                if ((scheduled != DateTime.MinValue) && (scheduled <= DateTime.Now))
                    tcScheduledDate.Attributes.CssStyle.Add("background-color", _highlight1);
            }

            #endregion

            #region assigned to

            TableCell tcUserFirstName = item["UserFirstName"];
            if (tcUserFirstName != null)
            {
                string _assignedToName = drv["UserFirstName"].ToString().Trim();
                string _lastName = drv["UserLastName"].ToString().Trim();
                _assignedToName += String.Format("{0}", _lastName.Length > 0 ? String.Format(" {0}", _lastName.Substring(0, 1).ToUpper()) : "");
                tcUserFirstName.Text = _assignedToName;

                int _assignedToClientId = BitByBit.Utility.DataBase.ConvertHelper.ConvertToInteger(drv["UserClientId"], 0);

                if (_assignedToClientId != Desktop.SiteHelper.Client.Id.BBB)
                {
                    //user name in bold
                    tcUserFirstName.Attributes.CssStyle.Add("font-weight", "bold");
                    Literal litName = new Literal();
                    litName.Text = drv["UserFirstName"].ToString().Trim() + " ";
                    tcUserFirstName.Controls.Add(litName);

                    //icon
                    Image imgBlueStar = new Image();
                    imgBlueStar.ImageUrl = "/Images/blue-star.png";
                    imgBlueStar.AlternateText = "Help Desk Client User";
                    imgBlueStar.Attributes.Add("Title", "Help Desk Client User");
                    tcUserFirstName.Controls.Add(imgBlueStar);
                }
            }

            #endregion

            //disposition
            TelerikHelper.AddLabelToCell(drv["DispositionName"].ToString().Trim(), item["DispositionName"]);

            //ticket type
            TelerikHelper.AddLabelToCell(drv["TicketTypeName"].ToString().Trim(), item["TicketTypeName"]);
        }

        #endregion

        #region exporting

        if (_isExport && e.Item is GridDataItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;

            //ticket id
            ((GridDataItem)e.Item)["Id"].Width = Unit.Pixel(45);
            //summary
            ((GridDataItem)e.Item)["Summary"].Text = drv["Summary"].ToString().Trim();
            //company name
            ((GridDataItem)e.Item)["CompanyName"].Text = drv["CompanyName"].ToString().Trim();

            foreach (TableCell _tc in e.Item.Cells)
            {
                e.Item.Style["font-size"] = "9pt";
                e.Item.Style["font-family"] = "Verdana";
            }
        }

        if (_isExport && e.Item is GridHeaderItem)
        {
            GridHeaderItem headerItem = (GridHeaderItem)e.Item;
            headerItem.Style["font-size"] = "9pt";
            headerItem.Style["font-family"] = "Verdana";
        }

        #endregion

        #region header

        if (e.Item.ItemType == GridItemType.Header)
        {
            GridHeaderItem item = e.Item as GridHeaderItem;
            int _totalCellCount = item.Cells.Count;
            int _currentCellCount = 0;

            foreach (TableCell cell in item.Cells)
            {
                _currentCellCount++;
                if ((cell.Controls.Count > 0))
                {
                    if (_currentCellCount == _totalCellCount)
                    {
                        System.Text.StringBuilder _sbToolTip = new System.Text.StringBuilder();
                        _sbToolTip.Append("0 = Highest");
                        _sbToolTip.Append("<br />4 = Lowest");

                        Telerik.Web.UI.RadToolTip _toolTipDescription = new RadToolTip();
                        _toolTipDescription.Text = _sbToolTip.ToString();
                        _toolTipDescription.TargetControlID = cell.ClientID;
                        _toolTipDescription.IsClientID = true;
                        _toolTipDescription.AutoCloseDelay = 5000;
                        _toolTipDescription.Width = System.Web.UI.WebControls.Unit.Pixel(150);
                        _toolTipDescription.Skin = "BitByBit";
                        _toolTipDescription.EnableEmbeddedSkins = false;
                        _toolTipDescription.EnableEmbeddedBaseStylesheet = false;
                        _toolTipDescription.Title = "Priority Description";
                        cell.Controls.Add(_toolTipDescription);
                    }
                }
            }
        }

        #endregion
        */
        #endregion
    }

    protected void rgTicket_PreRender(object sender, EventArgs e)
    {
        rgTicket.ShowHeader = true;
        rgTicket.MasterTableView.PagerStyle.AlwaysVisible = true;
        rgTicket.MasterTableView.PagerStyle.Visible = true;
        rgTicket.MasterTableView.NoMasterRecordsText = "No records found.";

        if (_type == DesktopShared.Ticket.GridType.Employee)
            rgTicket.MasterTableView.GetColumn("UserFirstName").Display = false;

        GridCommandItem commandItem = null;

        if (rgTicket.MasterTableView.Items.Count > 0 & !_isExport)
        {
            commandItem = (GridCommandItem)rgTicket.MasterTableView.GetItems(GridItemType.CommandItem)[0];
            commandItem.Visible = true;
        }

        #region add / editing ticket

        if (rgTicket.EditItems.Count > 0 || rgTicket.MasterTableView.IsItemInserted)
        {
            //hide all other line items while in edit mode
            foreach (GridDataItem item in rgTicket.MasterTableView.Items)
                item.Visible = false;

            //hide header if in edit mode
            //rgTicket.ShowHeader = IsBatchEditMode;
            rgTicket.ShowHeader = false;

            //hide footer/pager
            rgTicket.MasterTableView.PagerStyle.AlwaysVisible = false;
            rgTicket.MasterTableView.PagerStyle.Visible = false;

            if (commandItem != null)
            {
                if (!IsBatchEditMode)
                    commandItem.Visible = false; //hide command template (add ticket, refresh etc) if editing one item
                else
                {
                    //batch edit -> set active view in command item template that has update all / cancel buttons
                    MultiView mv = (MultiView)commandItem.FindControl("mvCommandTemplate");
                    mv.ActiveViewIndex = 1;
                }
            }
        }

        #endregion

        #region allow rows drag drop

        if (_type == DesktopShared.Ticket.GridType.HelpDesk || _type == DesktopShared.Ticket.GridType.Backup || _type == DesktopShared.Ticket.GridType.Level2Queue || _type == DesktopShared.Ticket.GridType.Employee || _type == DesktopShared.Ticket.GridType.PriorityUser)
        {
            rgTicket.ClientSettings.AllowRowsDragDrop = true;
        }

        #endregion

        #region command item template -> populate export to type drop down list

        if (rgTicket.MasterTableView.GetItems(GridItemType.CommandItem).Length > 0)
        {
            UserControl_DropDownList_GridExportType ucGridExportType = rgTicket.MasterTableView.GetItems(GridItemType.CommandItem)[0].FindControl("ucGridExportType") as UserControl_DropDownList_GridExportType;
            ucGridExportType.Populate();
        }

        #endregion

        #region client history grid viewed in ticked edit form -> hide header / footers

        if (_type == DesktopShared.Ticket.GridType.ClientHistory)
        {
            cpeTicket.Enabled = false;
            pnlShowHideTicket.Visible = false;
            pnlHeader.Visible = false;

            if (commandItem != null)
                commandItem.Visible = false;

            rgTicket.MasterTableView.PagerStyle.AlwaysVisible = false;
            rgTicket.MasterTableView.PagerStyle.Visible = false;
        }

        #endregion

        #region remark
        /*
        rgTicket.ShowHeader = true;
        rgTicket.MasterTableView.PagerStyle.AlwaysVisible = true;
        rgTicket.MasterTableView.PagerStyle.Visible = true;
        rgTicket.MasterTableView.NoMasterRecordsText = "No records found.";

        GridCommandItem commandItem = null;

        if (rgTicket.MasterTableView.Items.Count > 0 & !_isExport)
        {
            commandItem = (GridCommandItem)rgTicket.MasterTableView.GetItems(GridItemType.CommandItem)[0];
            commandItem.Visible = true;
        }

        #region add / editing ticket

        if (rgTicket.EditItems.Count > 0 || rgTicket.MasterTableView.IsItemInserted)
        {
            //hide all other line items while in edit mode
            foreach (GridDataItem item in rgTicket.MasterTableView.Items)
                item.Visible = false;

            //hide header if in edit mode
            //rgTicket.ShowHeader = IsBatchEditMode;
            rgTicket.ShowHeader = false;

            //hide footer/pager
            rgTicket.MasterTableView.PagerStyle.AlwaysVisible = false;
            rgTicket.MasterTableView.PagerStyle.Visible = false;

            if (commandItem != null)
            {
                if (!IsBatchEditMode)
                    commandItem.Visible = false; //hide command template (add ticket, refresh etc) if editing one item
                else
                {
                    //batch edit -> set active view in command item template that has update all / cancel buttons
                    MultiView mv = (MultiView)commandItem.FindControl("mvCommandTemplate");
                    mv.ActiveViewIndex = 1;
                }
            }
        }

        #endregion

        #region hide / show columns and set col width based on grid type

        #region default (NOT edit all)

        if (!IsBatchEditMode)
        {
            //hide employee column for certain types.  update summary width
            bool includeEmployeeCol = true;
            int summaryWidth = 15;

            if (_type == DesktopShared.Ticket.GridType.HelpDesk || _type == DesktopShared.Ticket.GridType.Backup || _type == DesktopShared.Ticket.GridType.Employee || _type == DesktopShared.Ticket.GridType.Level2Queue || _type == DesktopShared.Ticket.GridType.PriorityUser)
            {
                includeEmployeeCol = false;
                summaryWidth = 25;
            }

            //show scheduled / need completion by.  date cols width
            bool includeScheduledAndCompleted = false;
            int dateWidth = 5;
            if (_type == DesktopShared.Ticket.GridType.Scheduled)
            {
                includeScheduledAndCompleted = true;
                dateWidth = 4;
                summaryWidth = 4;
            }

            foreach (GridColumn col in rgTicket.MasterTableView.RenderColumns)
            {
                //assigned to user
                if (col.UniqueName == "UserFirstName")
                    col.Visible = includeEmployeeCol;

                //date entered
                if (col.UniqueName == "DateEntered")
                    col.HeaderStyle.Width = Unit.Percentage(dateWidth);

                //date updated
                if (col.UniqueName == "LastUpdated")
                    col.HeaderStyle.Width = Unit.Percentage(dateWidth);

                //date scheduled
                if (col.UniqueName == "ScheduledDate")
                {
                    col.Visible = includeScheduledAndCompleted;
                    col.HeaderStyle.Width = Unit.Percentage(dateWidth);
                }

                //date need completion by
                if (col.UniqueName == "NeedCompletionDate")
                {
                    col.Visible = includeScheduledAndCompleted;
                    col.HeaderStyle.Width = Unit.Percentage(dateWidth);
                }

                //summary
                if (col.UniqueName == "Summary")
                    col.HeaderStyle.Width = Unit.Percentage(summaryWidth);

                //client history grid
                if (_type == DesktopShared.Ticket.GridType.ClientHistory)
                {
                    if (col.UniqueName == "ActionColumn")
                        col.Visible = false;
                    if (col.UniqueName == "Tag")
                        col.Visible = false;
                }
            }

        }

        #region edit all mode

        else
        {
            foreach (GridColumn col in rgTicket.MasterTableView.RenderColumns)
            {
                //width of each column should match width used in BatchEditTicket control table cells

                //hide edit column
                if (col.UniqueName == "ActionColumn")
                    col.Visible = false;

                //width of No. col
                if (col.UniqueName == "Id")
                    col.HeaderStyle.Width = Unit.Percentage(5);

                //summary width
                if (col.UniqueName == "Summary")
                    col.HeaderStyle.Width = Unit.Percentage(20);

                //company width
                if (col.UniqueName == "CompanyName")
                    col.HeaderStyle.Width = Unit.Percentage(20);

                //status width
                if (col.UniqueName == "DispositionName")
                    col.HeaderStyle.Width = Unit.Percentage(10);

                //priority width
                if (col.UniqueName == "PriorityName")
                    col.HeaderStyle.Width = Unit.Percentage(10);

                //show assigned to user.  set width
                if (col.UniqueName == "UserFirstName")
                {
                    col.Visible = true;
                    col.HeaderStyle.Width = Unit.Percentage(19);
                }

                //hide entered
                if (col.UniqueName == "DateEntered")
                    col.Visible = false;

                //hide date scheduled
                if (col.UniqueName == "ScheduledDate")
                    col.Visible = false;

                //hide date need completion by
                if (col.UniqueName == "NeedCompletionDate")
                    col.Visible = false;

                //hide last updated
                if (col.UniqueName == "LastUpdated")
                    col.Visible = false;

                //hide time spent col
                if (col.UniqueName == "TotalTimeSpent")
                    col.Visible = false;

                //hide estimate col
                if (col.UniqueName == "HoursToFix")
                    col.Visible = false;

                //hide tag column
                if (col.UniqueName == "Tag")
                    col.Visible = false;

                //show product col.  set width
                if (col.UniqueName == "Product")
                {
                    col.Visible = true;
                    col.HeaderStyle.Width = Unit.Percentage(16);
                }
            }
        }

        #endregion

        #endregion

        #endregion

        #region allow rows drag drop

        if (_type == DesktopShared.Ticket.GridType.HelpDesk || _type == DesktopShared.Ticket.GridType.Backup || _type == DesktopShared.Ticket.GridType.Level2Queue || _type == DesktopShared.Ticket.GridType.Employee || _type == DesktopShared.Ticket.GridType.PriorityUser)
        {
            rgTicket.ClientSettings.AllowRowsDragDrop = true;
        }

        #endregion

        #region command item template -> populate export to type drop down list

        if (rgTicket.MasterTableView.GetItems(GridItemType.CommandItem).Length > 0)
        {
            UserControl_DropDownList_GridExportType ucGridExportType =
                rgTicket.MasterTableView.GetItems(GridItemType.CommandItem)[0].FindControl("ucGridExportType")
                as UserControl_DropDownList_GridExportType;

            ucGridExportType.Populate();
        }

        #endregion

        #region client history grid viewed in ticked edit form -> hide header / footers

        if (_type == DesktopShared.Ticket.GridType.ClientHistory)
        {
            cpeTicket.Enabled = false;
            pnlShowHideTicket.Visible = false;
            pnlHeader.Visible = false;

            if (commandItem != null)
                commandItem.Visible = false;

            rgTicket.MasterTableView.PagerStyle.AlwaysVisible = false;
            rgTicket.MasterTableView.PagerStyle.Visible = false;
        }

        #endregion
        */
        #endregion
    }

    protected void rgTicket_ItemCreated(object sender, GridItemEventArgs e)
    {
        #region remark
        /*
        #region Item or AlternatingItem

        //action link buttons needs to be added for both ItemCreated & ItemDataBound to raise postback events
        if (e.Item.ItemType == GridItemType.Item || e.Item.ItemType == GridItemType.AlternatingItem)
        {
            GridDataItem item = e.Item as GridDataItem;

            int ticketId = BitByBit.Utility.DataBase.ConvertHelper.ConvertToInteger((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["Id"], -1);

            #region add "make active" link if assigned to employee

            int _assignedTo = BitByBit.Utility.DataBase.ConvertHelper.ConvertToInteger((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["AssignedTo"], -1);
            int _dispositionId = BitByBit.Utility.DataBase.ConvertHelper.ConvertToInteger((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["FkDisposition"], -1);

            TableCell tcEditCommandColumn = item["ActionColumn"];
            tcEditCommandColumn.Controls.Clear();

            LinkButton lbMakeActive = new LinkButton();
            lbMakeActive.CommandName = "MakeActive";
            lbMakeActive.CommandArgument = ticketId.ToString();
            if ((_dispositionId != DesktopShared.SiteHelper.Ticket.Dispostion.Id.Active) || (_assignedTo != DesktopShared.User.UserID))
            {
                lbMakeActive.Text = "Active";
                lbMakeActive.ToolTip = "Make this ticket Active. Any other active tickets assigned will be changed to idle.";
            }
            else
                lbMakeActive.Text = "Edit";
            tcEditCommandColumn.Controls.Add(lbMakeActive);
            #endregion

            #region tag link

            TableCell tcTag = item["Tag"];

            LinkButton linkTag = new LinkButton();
            linkTag.ID = "linkTag";
            linkTag.CausesValidation = false;

            int taggedTicketId = 0;
            try { taggedTicketId = Convert.ToInt32((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["TaggedTicketId"].ToString()); }
            catch { taggedTicketId = -1; }

            bool addDeleteTag = false;
            if (taggedTicketId > 0)
            {
                var _tagValue = ucUserTicketTags.TagName.Replace("(P)", "").Replace("(PC)", "").Trim();
                if (!String.IsNullOrWhiteSpace(_tagValue))
                {
                    //check tag text vaue vs current selection in drop down list
                    addDeleteTag = DesktopShared.Ticket.Tagged.TicketIsTaggedByValue(
                    ticketId,
                    TagIsPublic ? (int?)null : DesktopShared.User.UserID,
                    _tagValue);
                }
            }

            if (addDeleteTag)
            {
                linkTag.Text = "X";
                linkTag.ToolTip = "Delete Tag";
                linkTag.CommandName = "RemoveTag";
                linkTag.Attributes.CssStyle.Add("color", "#DB2929");
            }
            else
            {
                linkTag.Text = "+";
                linkTag.ToolTip = "Add Tag";
                linkTag.CommandName = "AddTag";
            }

            tcTag.Controls.Add(linkTag);

            #endregion

            #region ticket ID column - > ticket awaiting response / recurring ticket

            TableCell tcId = item["Id"];

            #region awaiting response

            int _awaitingResponseUserId = 0;
            try { _awaitingResponseUserId = Convert.ToInt32((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["AwaitingResponseUserId"].ToString()); }
            catch { _awaitingResponseUserId = -1; }

            if (_awaitingResponseUserId > 0)
            {
                bool _internalWaitingResponse = Convert.ToBoolean((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["AwaitingResponseInternal"].ToString());
                item["Id"].Attributes.CssStyle.Add("background-color", _internalWaitingResponse ? "#1E90FF" : "#EE0000");

                Literal litId = new Literal();
                litId.Text = String.Format("<span style=\"color:White;\">{0}</span>&nbsp;&nbsp;&nbsp;", (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["Id"].ToString());
                tcId.Controls.Clear();
                tcId.Controls.Add(litId);

                LinkButton linkClearAwaitingResponse = new LinkButton();
                linkClearAwaitingResponse.Attributes.CssStyle.Add("color", "White");
                linkClearAwaitingResponse.ID = "linkClearAwaitingResponse";
                linkClearAwaitingResponse.CausesValidation = false;
                linkClearAwaitingResponse.Text = "X";
                linkClearAwaitingResponse.ToolTip = "Clear Awaiting Reponse Flag";
                linkClearAwaitingResponse.CommandName = "ClearClearAwaitingResponse";

                tcId.Controls.Add(linkClearAwaitingResponse);
            }

            #endregion

            #region recurring ticket

            bool _isRecurringTicket = false;
            bool.TryParse((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["IsRecurring"].ToString(), out _isRecurringTicket);

            if (_isRecurringTicket)
            {
                tcId.Controls.Add(new LiteralControl("&nbsp;"));

                Image imgRecurringTicket = new Image();
                imgRecurringTicket.ImageUrl = DesktopShared.SiteHelper.Icon.RecurringTicket;
                imgRecurringTicket.AlternateText = "Recurring Ticket";
                imgRecurringTicket.Attributes.Add("Title", "Recurring Ticket");
                imgRecurringTicket.ToolTip = "Recurring Ticket";
                imgRecurringTicket.ImageAlign = ImageAlign.AbsBottom;
                tcId.Controls.Add(imgRecurringTicket);
            }

            #endregion

            #endregion

            #region priority client and/or helpdesk client

            if (HighlightPriorityClient || HighlightDesktopClient)
            {
                //company name
                string companyName = (e.Item as GridDataItem).OwnerTableView.DataKeyValues
                    [e.Item.ItemIndex]["CompanyName"].ToString().Trim();

                //is priority client
                bool isPriority = false;
                try
                {
                    isPriority = Convert.ToBoolean((e.Item as GridDataItem).OwnerTableView.DataKeyValues
                    [e.Item.ItemIndex]["Priority"].ToString());
                }
                catch { isPriority = false; }

                //is help desk client
                bool isHelpDesk = false;
                try
                {
                    isHelpDesk = Convert.ToBoolean((e.Item as GridDataItem).OwnerTableView.DataKeyValues
                    [e.Item.ItemIndex]["UseHelpDesk"].ToString());
                }
                catch { isHelpDesk = false; }

                if (isPriority || isHelpDesk)
                {
                    TableCell tcCompanyName = item["CompanyName"];
                    tcCompanyName.Attributes.CssStyle.Add("font-weight", "bold");

                    //company name in bold
                    Literal litName = new Literal();
                    litName.Text = companyName + " ";
                    tcCompanyName.Controls.Add(litName);

                    //priority icon
                    if (isPriority)
                    {
                        Image imgStar = new Image();
                        imgStar.ImageUrl = "/Images/gold-star.png";
                        imgStar.AlternateText = "Priority Client";
                        imgStar.Attributes.Add("Title", "Priority Client");
                        tcCompanyName.Controls.Add(imgStar);

                        Literal litSpacer = new Literal();
                        litSpacer.Text = "&nbsp;";
                        tcCompanyName.Controls.Add(litSpacer);
                    }

                    //help desk icon
                    if (isHelpDesk)
                    {
                        Image imgRedStar = new Image();
                        imgRedStar.ImageUrl = "/Images/red-star.png";
                        imgRedStar.AlternateText = "Help Desk Client";
                        imgRedStar.Attributes.Add("Title", "Help Desk Client");
                        tcCompanyName.Controls.Add(imgRedStar);
                    }
                }
            }

            #endregion

            #region assigned to

            TableCell tcUserFirstName = item["UserFirstName"];
            if (tcUserFirstName != null)
            {
                string _assignedToName = (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["UserFirstName"].ToString().Trim();
                string _lastName = (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["UserLastName"].ToString().Trim();
                _assignedToName += String.Format("{0}", _lastName.Length > 0 ? String.Format(" {0}", _lastName.Substring(0, 1).ToUpper()) : "");
                tcUserFirstName.Text = _assignedToName;

                int assignedToClientId = 0;
                try { assignedToClientId = Convert.ToInt32((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["UserClientId"].ToString()); }
                catch { assignedToClientId = -1; }

                if (assignedToClientId != Desktop.SiteHelper.Client.Id.BBB)
                {
                    //user name in bold
                    string userName = (e.Item as GridDataItem).OwnerTableView.DataKeyValues
                       [e.Item.ItemIndex]["UserFirstName"].ToString().Trim();
                    tcUserFirstName.Attributes.CssStyle.Add("font-weight", "bold");
                    Literal litName = new Literal();
                    litName.Text = userName + " ";
                    tcUserFirstName.Controls.Add(litName);

                    //icon
                    Image imgBlueStar = new Image();
                    imgBlueStar.ImageUrl = "/Images/blue-star.png";
                    imgBlueStar.AlternateText = "Help Desk Client User";
                    imgBlueStar.Attributes.Add("Title", "Help Desk Client User");
                    tcUserFirstName.Controls.Add(imgBlueStar);
                }
            }

            #endregion
        }

        #endregion

        #region command item -> hide for export

        if (e.Item is GridCommandItem && _isExport)
            e.Item.Display = false;

        #endregion
    */
        #endregion
    }

    protected void rgTicket_UpdateCommand(object sender, GridCommandEventArgs e)
    {
        if (Page.IsValid)
        {
            GridEditableItem editedItem = e.Item as GridEditableItem;
            AbstractTicketEdit ucTicketEdit = e.Item.FindControl(GridEditFormItem.EditFormUserControlID) as AbstractTicketEdit;
            int Id = Convert.ToInt32(editedItem.OwnerTableView.DataKeyValues[editedItem.ItemIndex]["Id"]);
            DesktopShared.EntityClasses.CscDefectsEntity ticket = new DesktopShared.EntityClasses.CscDefectsEntity(Id);
            bool _emailError;
            bool _schedulingError;
            List<int> _emailErrorTrackingIds;
            ucTicketEdit.SaveValues(ticket, out _schedulingError, out _emailError, out _emailErrorTrackingIds);
            if (_emailError && _emailErrorTrackingIds.Count > 0)
                Response.Redirect(String.Format("/Ticket/EmailError.aspx?Id={0}", String.Join(",", _emailErrorTrackingIds.ToArray())));
            else if (_schedulingError)
            {
                rnConfirmation.Text = "Ticket has been updated. Unable to add to exchange calendar so no scheduling occured.";
                rnConfirmation.Skin = "Sunset";
                rnConfirmation.Title = "Warning";
                rnConfirmation.AutoCloseDelay = 0;
                rnConfirmation.VisibleTitlebar = true;
                rnConfirmation.ContentIcon = "warning";
                rnConfirmation.TitleIcon = "warning";
                rnConfirmation.Height = 150;
            }
            else
                rnConfirmation.Text = "Ticket has been updated.";
            rnConfirmation.Show();
        }
    }

    protected void rgTicket_InsertCommand(object sender, GridCommandEventArgs e)
    {
        if (Page.IsValid)
        {
            GridEditableItem editedItem = e.Item as GridEditableItem;
            AbstractTicketAdd ucTicketAdd = e.Item.FindControl(GridEditFormItem.EditFormUserControlID) as AbstractTicketAdd;
            bool _emailError;
            bool _schedulingError;
            List<int> _emailErrorTrackingIds;
            int ticketId = ucTicketAdd.SaveValues(out _schedulingError, out _emailError, out _emailErrorTrackingIds);
            if (_emailError && _emailErrorTrackingIds.Count > 0)
                Response.Redirect(String.Format("/Ticket/EmailError.aspx?Id={0}", String.Join(",", _emailErrorTrackingIds.ToArray())));

            //confirmation message
            rnConfirmation.Text = "Ticket has been added.";
            rnConfirmation.Show();
        }
    }

    protected void rgTicket_ItemCommand(object sender, GridCommandEventArgs e)
    {
        bool doRebind = false;
        RadGrid grid = (sender as RadGrid);

        #region add ticket click

        if (e.CommandName == "AddNewTicket")
        {
            Response.Redirect("/Ticket/Add2.aspx");
        }

        #endregion

        #region edit ticket click

        else if (e.CommandName == RadGrid.EditCommandName)
        {
            IsBatchEditMode = false;

            e.Item.OwnerTableView.IsItemInserted = false;
            e.Item.OwnerTableView.EditFormSettings.UserControlName = "/UserControl/Grid/EditForm/TicketEdit.ascx";
        }

        #endregion

        #region refresh grid click

        else if (e.CommandName == "RefreshGrid")
        {
            doRebind = true;
        }

        #endregion

        #region merge tickets click

        else if (e.CommandName == "TicketMerge")
        {
            string _allIds = "";

            #region get ids of each selected item

            //foreach (GridDataItem _itemToMerge in rgTicket.SelectedItems)
            //{
            //    if (_allIds.Trim().Length > 0)
            //        _allIds += ",";

            //    _allIds += _itemToMerge.GetDataKeyValue("Id");
            //}

            foreach (GridDataItem _itemToMerge in rgTicket.MasterTableView.Items)
            {
                CheckBox chk = (CheckBox)_itemToMerge.FindControl("cbCheck");
                if (chk.Checked == true) 
                {
                    if (_allIds.Trim().Length > 0)
                        _allIds += ",";

                    _allIds += _itemToMerge.GetDataKeyValue("Id");
                }
            }

            #endregion

            if (_allIds.Trim().Length > 0)
            {
                RadWindow rwMerge = new RadWindow();
                rwMerge.NavigateUrl = String.Format("/UserControl/Grid/EditForm/TicketMerge.aspx?Ids={0}", _allIds);
                rwMerge.VisibleOnPageLoad = true;
                rwMerge.DestroyOnClose = true;
                rwMerge.Height = Unit.Pixel(320);
                rwMerge.Width = Unit.Pixel(450);
                rwMerge.Left = Unit.Pixel(150);
                rwMerge.AutoSize = false;
                rwMerge.Modal = true;
                rwmTicketMerge.VisibleTitlebar = false;

                rwmTicketMerge.Windows.Add(rwMerge);
            }
        }

        #endregion

        #region close selected tickets click

        else if (e.CommandName == "CloseSelected")
        {
            int? _managerId = DesktopShared.User.ManagerId;
            foreach (GridDataItem _itemToClose in rgTicket.MasterTableView.Items)
            {
                CheckBox chk = (CheckBox)_itemToClose.FindControl("cbCheck");
                if (chk.Checked == true)
                {
                    #region fetch ticket & save

                    //ticked it
                    int _ticketID = (int)_itemToClose.GetDataKeyValue("Id");

                    //fetch ticket entity
                    DesktopShared.EntityClasses.CscDefectsEntity ticket =
                        new DesktopShared.EntityClasses.CscDefectsEntity(_ticketID);

                    //disposition
                    ticket.FkDisposition = Desktop.SiteHelper.Ticket.Dispostion.Id.Closed;
                    //status
                    ticket.FkStatus = Desktop.SiteHelper.Ticket.Status.Id.Closed;
                    //awaiting response
                    ticket.AwaitingResponseUserId = null;
                    ticket.AwaitingResponseInternal = true;
                    //last updated
                    ticket.Lastupdated = DateTime.Now;
                    //save ticket
                    ticket.Save();

                    #endregion

                    #region create internal note (Batch Close By Employee)

                    string _internalNotes = Desktop.SiteHelper.Message.TicketBatchClose;

                    DesktopShared.EntityClasses.CscHistoryEntity _internalHistory =
                       new DesktopShared.EntityClasses.CscHistoryEntity();

                    _internalHistory.CscdefectsId = ticket.Pcscdefects;
                    string _htmlEncodedInternalNotes = Server.HtmlEncode(_internalNotes);
                    if (_htmlEncodedInternalNotes.Length > 8000)
                        _htmlEncodedInternalNotes = _htmlEncodedInternalNotes.Substring(0, 8000);
                    _internalHistory.Notes = _htmlEncodedInternalNotes;
                    _internalHistory.UpdatedBy = DesktopShared.User.UserID;
                    _internalHistory.InternalUsage = "Y";
                    _internalHistory.FkStatus = ticket.FkStatus;
                    _internalHistory.InternalRecipients = "";
                    _internalHistory.TicketHistoryTypeId = DesktopShared.Ticket.History.Type.Id.BatchUpdate;
                    _internalHistory.Save();

                    #endregion

                    //if ticket closed, user has mananger, and checklist items open -> send alert to manager
                    if (_managerId.HasValue)
                    {
                        if (DesktopShared.Ticket.CheckList.HasOpenItems(ticket.Pcscdefects))
                            DesktopShared.Email.Ticket.SendClosedWithOpenChecklist(ticket.Pcscdefects, DesktopShared.User.UserID, _managerId.Value);
                    }
                }
            }

            doRebind = true;
        }

        #endregion

        #region edit all

        else if (e.CommandName == "EditAll")
        {
            IsBatchEditMode = true;

            e.Item.OwnerTableView.IsItemInserted = false;
            e.Item.OwnerTableView.EditFormSettings.UserControlName = "/UserControl/Grid/EditForm/TicketBatchEdit.ascx";

            foreach (GridItem _gridItem in rgTicket.MasterTableView.Items)
            {
                if (_gridItem is GridEditableItem)
                {
                    GridEditableItem _gridEditableItem = _gridItem as GridDataItem;
                    _gridEditableItem.Edit = true;
                }
            }

            doRebind = true;
        }

        #endregion

        #region update all

        else if (e.CommandName == "UpdateAll")
        {
            if (Page.IsValid)
            {
                if (rgTicket.EditIndexes.Count == 0)
                    return;

                int? _managerId = DesktopShared.User.ManagerId;

                //iterate through grid items in edit mode
                foreach (GridDataItem dataItem in rgTicket.EditItems)
                {
                    int Id = Convert.ToInt32(dataItem.OwnerTableView.DataKeyValues[dataItem.ItemIndex]["Id"]);
                    GridEditableItem editedItem = (GridEditableItem)dataItem.EditFormItem;
                    AbstractTicketBatchEdit ucTicketEdit = editedItem.FindControl(GridEditFormItem.EditFormUserControlID) as AbstractTicketBatchEdit;
                    DesktopShared.EntityClasses.CscDefectsEntity ticket = new DesktopShared.EntityClasses.CscDefectsEntity(Id);

                    if (ucTicketEdit != null)
                        ucTicketEdit.SaveValues(ticket, _managerId);
                }

                IsBatchEditMode = false;
                rgTicket.EditIndexes.Clear();
                doRebind = true;
            }
        }

        #endregion

        #region cancel update all

        else if (e.CommandName == "CancelUpdateAll")
        {
            if (rgTicket.EditIndexes.Count == 0)
                return;

            IsBatchEditMode = false;
            rgTicket.EditIndexes.Clear();
            doRebind = true;
        }

        #endregion

        #region add tag click

        else if (e.CommandName == "AddTag")
        {
            int cscDefectID = Convert.ToInt32((e.Item as GridDataItem).OwnerTableView.DataKeyValues
                 [e.Item.ItemIndex]["Id"].ToString());

            if (CurrentTag == "")
                CurrentTag = BitByBit.Configuration.GetConfigString("DefaultTagText", "Default");

            //public tags do not have user id assigned to them
            int? userId = TagIsPublic ? (int?)null : DesktopShared.User.UserID;

            //add tag
            DesktopShared.Ticket.Tagged.AddTagValue(cscDefectID, CurrentTagStripPublic, userId, TagIsPublic, TagIsClientPublic);

            doRebind = true;
        }

        #endregion

        #region remove tag click

        else if (e.CommandName == "RemoveTag")
        {
            int cscDefectID = Convert.ToInt32((e.Item as GridDataItem).OwnerTableView.DataKeyValues
                [e.Item.ItemIndex]["Id"].ToString());

            //public tags do not have user id assigned to them
            int? userId = TagIsPublic ? (int?)null : DesktopShared.User.UserID;

            //delete ticket
            DesktopShared.Ticket.Tagged.DeleteTagValue(cscDefectID, CurrentTagStripPublic, userId);

            doRebind = true;
        }

        #endregion

        #region clear awaiting response click

        else if (e.CommandName == "ClearClearAwaitingResponse")
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

            doRebind = true;
        }

        #endregion

        #region make active ticket

        else if (e.CommandName == "MakeActive")
        {
            int cscDefectID = Convert.ToInt32((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["Id"].ToString());
            int? _currentActiveTicketId = DesktopShared.Ticket.GetActiveId(DesktopShared.User.UserID);
            int _assignedTo = Convert.ToInt32((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["AssignedTo"].ToString());
            bool _assignmentChanged = (_assignedTo != DesktopShared.User.UserID);

            //alert current active employee if another employee took over ticket
            string _emailTo = "";
            if (_assignmentChanged)
                _emailTo = (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["AssignedToUserEmail"].ToString();

            //make active for employee that clicked make active link
            DesktopShared.Ticket.MakeActive(cscDefectID, DesktopShared.User.UserID, DesktopShared.User.UserID, true, _emailTo);

            //if ticket assigned to other employee, call inactive method for that employee
            if (_assignmentChanged)
                DesktopShared.Ticket.MakeInactive(cscDefectID, DesktopShared.User.UserID, _assignedTo, false);

            //set session values used for client combo box on master page header
            Session["ClientText"] = (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["CompanyName"].ToString();
            Session["ClientID"] = Convert.ToInt32((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["FkClient"].ToString());

            if (!_currentActiveTicketId.HasValue) //no current active ticket for employee = no old ticket to update
                Response.Redirect(String.Format("/Ticket/Detail.aspx?TicketID={0}", cscDefectID));//redirect to ticket  
            else //prompt user to add external/internal notes and timesheet entry for ticket that was just active
            {
                RadWindow rwMerge = new RadWindow();
                rwMerge.NavigateUrl = String.Format("/UserControl/Grid/EditForm/TicketActive.aspx?TicketId={0}&NewTicketId={1}", _currentActiveTicketId.Value, cscDefectID);
                rwMerge.VisibleOnPageLoad = true;
                rwMerge.DestroyOnClose = true;
                rwMerge.Height = Unit.Pixel(500);
                rwMerge.Width = Unit.Pixel(650);
                rwMerge.Left = Unit.Pixel(150);
                rwMerge.AutoSize = false;
                rwMerge.Modal = true;
                rwmTicketMerge.VisibleTitlebar = false;

                rwmTicketMerge.Windows.Add(rwMerge);
            }
        }

        #endregion

        #region do rebind

        if (doRebind)
        {
            //rebind sender grid
            RebindGrid(rgTicket);

            #region rebind tagged ticket grid

            Telerik.Web.UI.RadGrid taggedTicketGrid = GetGrid(DesktopShared.Ticket.GridType.Tagged);
            if (taggedTicketGrid != null)
                RebindGrid(taggedTicketGrid);

            #endregion

            #region rebind help desk grid

            Telerik.Web.UI.RadGrid helpdeskTicketGrid = GetGrid(DesktopShared.Ticket.GridType.HelpDesk);
            if (helpdeskTicketGrid != null)
                RebindGrid(helpdeskTicketGrid);

            #endregion

            #region rebind backup grid

            Telerik.Web.UI.RadGrid backupTicketGrid = GetGrid(DesktopShared.Ticket.GridType.Backup);
            if (backupTicketGrid != null)
                RebindGrid(backupTicketGrid);

            #endregion

            #region rebind priorit user grid

            Telerik.Web.UI.RadGrid priorityUserTicketGrid = GetGrid(DesktopShared.Ticket.GridType.PriorityUser);
            if (priorityUserTicketGrid != null)
                RebindGrid(priorityUserTicketGrid);

            #endregion

            #region rebind level 2 queue grid

            Telerik.Web.UI.RadGrid level2QueueTicketGrid = GetGrid(DesktopShared.Ticket.GridType.Level2Queue);
            if (level2QueueTicketGrid != null)
                RebindGrid(level2QueueTicketGrid);

            #endregion

            #region rebind recent tickets grid

            Telerik.Web.UI.RadGrid recentTicketGrid = GetGrid(DesktopShared.Ticket.GridType.Active);
            if (recentTicketGrid != null)
                RebindGrid(recentTicketGrid);

            #endregion

            #region rebind scheduled tickets grid

            Telerik.Web.UI.RadGrid scheduledTicketGrid = GetGrid(DesktopShared.Ticket.GridType.Scheduled);
            if (scheduledTicketGrid != null)
                RebindGrid(scheduledTicketGrid);

            #endregion

            #region rebind employee tickets grid

            Telerik.Web.UI.RadGrid employeeTicketGrid = GetGrid(DesktopShared.Ticket.GridType.Employee);
            if (employeeTicketGrid != null)
                RebindGrid(employeeTicketGrid);

            #endregion

            #region rebind priority client tickets grid

            Telerik.Web.UI.RadGrid priorityClientTicketGrid = GetGrid(DesktopShared.Ticket.GridType.PriorityClient);
            if (priorityClientTicketGrid != null)
                RebindGrid(priorityClientTicketGrid);

            #endregion

            #region rebind helpdesk client tickets grid

            Telerik.Web.UI.RadGrid helpDeskClientTicketGrid = GetGrid(DesktopShared.Ticket.GridType.HelpDeskClient);
            if (helpDeskClientTicketGrid != null)
                RebindGrid(helpDeskClientTicketGrid);

            #endregion

            #region rebind alert designation tickets grid

            Telerik.Web.UI.RadGrid designationAlertTicketGrid = GetGrid(DesktopShared.Ticket.GridType.DesignationAlert);
            if (designationAlertTicketGrid != null)
                RebindGrid(designationAlertTicketGrid);

            #endregion

        }

        #endregion
    }

    protected void rgTicket_RowDrop(object sender, GridDragDropEventArgs e)
    {
        if (string.IsNullOrEmpty(e.HtmlElement))
        {

            //assigned to
            int? _assignedTo = null;

            #region get destination grid & user to assign to

            Telerik.Web.UI.RadGrid _destinationGrid = null;

            if (e.DestinationGrid.ClientID.IndexOf("ucTicketHelpDesk") > -1) //assign to helpdesk
            {
                _assignedTo = DesktopShared.SiteHelper.User.Id.HelpDesk;
                _destinationGrid = GetGrid(DesktopShared.Ticket.GridType.HelpDesk);
            }
            else if (e.DestinationGrid.ClientID.IndexOf("ucTicketBackup") > -1) //assign to backups
            {
                _assignedTo = DesktopShared.SiteHelper.User.Id.Backup;
                _destinationGrid = GetGrid(DesktopShared.Ticket.GridType.Backup);
            }
            else if (e.DestinationGrid.ClientID.IndexOf("ucTicketLevel2Queue") > -1) //assign to level 2
            {
                _assignedTo = DesktopShared.SiteHelper.User.Id.Level2Queue;
                _destinationGrid = GetGrid(DesktopShared.Ticket.GridType.Level2Queue);
            }
            else if (e.DestinationGrid.ClientID.IndexOf("ucTicketEmployee") > -1) //assign to logged in employee
            {
                _assignedTo = DesktopShared.User.UserID;
                _destinationGrid = GetGrid(DesktopShared.Ticket.GridType.Employee);
            }
            else if (e.DestinationGrid.ClientID.IndexOf("ucTicketPriorityUser") > -1) //assign to priority user
            {
                _assignedTo = DesktopShared.SiteHelper.User.Id.Priority;
                _destinationGrid = GetGrid(DesktopShared.Ticket.GridType.PriorityUser);
            }

            #endregion

            if (_assignedTo.HasValue)
            {

                #region iterate through each selected row and update assigned to

                foreach (GridDataItem _draggedItem in e.DraggedItems)
                {
                    //ticket id
                    int _ticketID = (int)_draggedItem.GetDataKeyValue("Id");

                    //fetch ticket entity
                    DesktopShared.EntityClasses.CscDefectsEntity ticket =
                        new DesktopShared.EntityClasses.CscDefectsEntity(_ticketID);

                    //update ticket if current assigned to != new assigned to
                    if (ticket.Assignedto != _assignedTo)
                    {
                        #region update assigned to / awaiting response

                        //logic to flag ticket as awaiting response
                        if ((ticket.Assignedto.Value != _assignedTo.Value) && (_assignedTo.Value != DesktopShared.User.UserID))
                            ticket.AwaitingResponseUserId = _assignedTo.Value;

                        ticket.Assignedto = _assignedTo.Value;

                        #endregion

                        #region email to assign to

                        string _employeeEmails = "";

                        if (DesktopShared.User.UserID != _assignedTo)
                        {
                            DesktopShared.EntityClasses.UsersEntity _userEmployee =
                                new DesktopShared.EntityClasses.UsersEntity(_assignedTo.Value);

                            if (!String.IsNullOrEmpty(_userEmployee.Email))
                                _employeeEmails += _userEmployee.Email.Trim();
                        }

                        #endregion

                        #region save ticket

                        ticket.Lastupdated = DateTime.Now;
                        ticket.Save();

                        #endregion

                        #region add history to ticket and send emails    

                        DesktopShared.Ticket.History.Add(
                            ticket.Pcscdefects, //ticket id
                            DesktopShared.User.UserID, //user id, 
                            "", //notes
                            Desktop.SiteHelper.Message.TicketAssignmentChanged.Trim(), //internal notes 
                            false, // is new ticket
                            "", // client email addresses
                            _employeeEmails, // employee email addresses
                            true, // send email out 
                            DesktopShared.Ticket.History.Type.Id.AssignmentChange //history type
                            );

                        #endregion
                    }
                }

                #endregion

                #region rebind source & destination grid

                //source grid
                RebindGrid(rgTicket);

                //destination grid
                if (_destinationGrid != null)
                    RebindGrid(_destinationGrid);

                #endregion
            }
        }
    }

    protected void rgTicket_SortCommand(object sender, GridSortCommandEventArgs e)
    {
        GridSortExpression sortExpression = new GridSortExpression();
        DesktopShared.User.Ticket.GridSort.SetFieldName(e.SortExpression, _type);

        //these columns will sort descending on first click
        var _nonAscList = new List<string> { "Id", "DateEntered", "LastUpdated", "ScheduledDate", "NeedCompletionDate", "HoursToFix", "TotalTimeSpent" };
        bool _sortAscending = !_nonAscList.Contains(e.SortExpression.Trim());


        switch (e.OldSortOrder)
        {
            case GridSortOrder.None:
                sortExpression.FieldName = e.SortExpression;
                sortExpression.SortOrder = _sortAscending ? GridSortOrder.Ascending : GridSortOrder.Descending;
                e.Item.OwnerTableView.SortExpressions.AddSortExpression(sortExpression);

                DesktopShared.User.Ticket.GridSort.SetOperator(
                    _sortAscending ? GridSortOrder.Ascending.ToString() : GridSortOrder.Descending.ToString(),
                    _type);

                break;
            case GridSortOrder.Ascending:

                sortExpression.FieldName = e.SortExpression;
                sortExpression.SortOrder = _sortAscending ? GridSortOrder.Descending : GridSortOrder.None;
                e.Item.OwnerTableView.SortExpressions.AddSortExpression(sortExpression);

                DesktopShared.User.Ticket.GridSort.SetOperator(
                    _sortAscending ? GridSortOrder.Descending.ToString() : GridSortOrder.None.ToString(),
                    _type);

                break;
            case GridSortOrder.Descending:

                sortExpression.FieldName = e.SortExpression;
                sortExpression.SortOrder = _sortAscending ? GridSortOrder.None : GridSortOrder.Ascending;
                e.Item.OwnerTableView.SortExpressions.AddSortExpression(sortExpression);

                DesktopShared.User.Ticket.GridSort.SetOperator(
                    _sortAscending ? GridSortOrder.None.ToString() : GridSortOrder.Ascending.ToString(),
                    _type);

                break;
        }

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
    #endregion

    #region Protected method
    /// <summary>
    /// user ticket tags on selected index changed
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlUserTicketTags_SelectedIndexChanged(object sender, EventArgs e)
    {
        CurrentTag = ucUserTicketTags.TagName.Trim();
        rgTicket.Rebind();

        #region rebind tagged ticket grid

        Telerik.Web.UI.RadGrid taggedTicketGrid = GetGrid(DesktopShared.Ticket.GridType.Tagged);
        if (taggedTicketGrid != null)
            RebindGrid(taggedTicketGrid);

        #endregion
    }

    /// <summary>
    /// ticket views drop down list on selected index changed
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlTicketView_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (_type == DesktopShared.Ticket.GridType.View || _type == DesktopShared.Ticket.GridType.View1)
            DesktopShared.User.Ticket.TicketViewId = ucTicketView.TicketViewId;
        else if (_type == DesktopShared.Ticket.GridType.View2)
            DesktopShared.User.Ticket.TicketView2Id = ucTicketView.TicketViewId;
        else if (_type == DesktopShared.Ticket.GridType.View3)
            DesktopShared.User.Ticket.TicketView3Id = ucTicketView.TicketViewId;
        rgTicket.Rebind();
    }

    protected void btnExport_Click(object sender, EventArgs e)
    {
        ConfigureExport();

        UserControl_DropDownList_GridExportType ucGridExportType =
           rgTicket.MasterTableView.GetItems(GridItemType.CommandItem)[0].FindControl("ucGridExportType")
           as UserControl_DropDownList_GridExportType;

        if (ucGridExportType.Mode == UserControl_DropDownList_GridExportType.ExportMode.Csv)
            rgTicket.MasterTableView.ExportToCSV();
        else if (ucGridExportType.Mode == UserControl_DropDownList_GridExportType.ExportMode.Excel)
            rgTicket.MasterTableView.ExportToExcel();
        else if (ucGridExportType.Mode == UserControl_DropDownList_GridExportType.ExportMode.Word)
            rgTicket.MasterTableView.ExportToWord();
        else if (ucGridExportType.Mode == UserControl_DropDownList_GridExportType.ExportMode.Pdf)
            rgTicket.MasterTableView.ExportToPdf();
    }
    #endregion

    #region public properties

    /// <summary>
    /// set grid type
    /// </summary>
    public DesktopShared.Ticket.GridType Type
    {
        set { _type = value; }
    }

    /// <summary>
    /// set help desk client ticket user control
    /// </summary>
    public UserControl_Grid_TicketDashboardResponsive HelpDeskClientTicketUserControl
    {
        set { _helpDeskClientTicketUserControl = value; }
    }

    /// <summary>
    /// set priority ticket user control
    /// </summary>
    public UserControl_Grid_TicketDashboardResponsive PriorityClientTicketUserControl
    {
        set { _priorityClientTicketUserControl = value; }
    }

    /// <summary>
    /// set priority user ticket user control
    /// </summary>
    public UserControl_Grid_TicketDashboardResponsive PriorityUserTicketUserControl
    {
        set { _priorityUserTicketUserControl = value; }
    }

    /// <summary>
    /// set tagged ticket user control
    /// </summary>
    public UserControl_Grid_TicketDashboardResponsive TaggedTicketUserControl
    {
        set { _taggedTicketUserControl = value; }
    }

    /// <summary>
    /// set view ticket user control
    /// </summary>
    public UserControl_Grid_TicketDashboardResponsive ViewTicketUserControl
    {
        set { _viewTicketUserControl = value; }
    }

    /// <summary>
    /// set help desk ticket user control
    /// </summary>
    public UserControl_Grid_TicketDashboardResponsive HelpDeskTicketUserControl
    {
        set { _helpDeskTicketUserControl = value; }
    }

    /// <summary>
    /// set backup ticket user control
    /// </summary>
    public UserControl_Grid_TicketDashboardResponsive BackupTicketUserControl
    {
        set { _backupTicketUserControl = value; }
    }

    /// <summary>
    /// set level 2 queue ticket user control
    /// </summary>
    public UserControl_Grid_TicketDashboardResponsive Level2QueueTicketUserControl
    {
        set { _level2QueueTicketUserControl = value; }
    }

    /// <summary>
    /// set recent ticket user control
    /// </summary>
    public UserControl_Grid_TicketDashboardResponsive RecentTicketUserControl
    {
        set { _recentTicketUserControl = value; }
    }

    /// <summary>
    /// set scheduled ticket user control
    /// </summary>
    public UserControl_Grid_TicketDashboardResponsive ScheduledTicketUserControl
    {
        set { _scheduledTicketUserControl = value; }
    }

    /// <summary>
    /// set employee ticket user control
    /// </summary>
    public UserControl_Grid_TicketDashboardResponsive EmployeeTicketUserControl
    {
        set { _employeeTicketUserControl = value; }
    }

    /// <summary>
    /// set designation alert user control
    /// </summary>
    public UserControl_Grid_TicketDashboardResponsive DesignationAlertTicketUserControl
    {
        set { _designationAlertTicketUserControl = value; }
    }

    #endregion

    #region public methods

    /// <summary>
    /// reset grid
    /// </summary>
    public void ResetGrid()
    {
        rgTicket.Visible = true;
        pnlHeader.Visible = true;

        rgTicket.EditIndexes.Clear();
        rgTicket.DataSource = null;
        rgTicket.Rebind();
    }

    /// <summary>
    /// clear/hide grid
    /// </summary>
    public void ClearGrid()
    {
        rgTicket.CurrentPageIndex = 0;
        rgTicket.EditIndexes.Clear();
        rgTicket.Visible = false;
        pnlHeader.Visible = false;
    }

    #endregion

    #region search properties

    /// <summary>
    /// get/set search tagged
    /// </summary>
    public bool SearchTagged
    {
        get
        {
            object obj = this.ViewState["SearchTaggedForTicket"];
            if (obj == null)
                return false;
            else
                return (bool)obj;
        }

        set
        {
            this.ViewState["SearchTaggedForTicket"] = value;
        }
    }

    /// <summary>
    /// get/set search ticket number
    /// </summary>
    public int SearchTicketNumber
    {
        get
        {
            object obj = this.ViewState["SearchTicketNumberForTicket"];
            if (obj == null)
                return -1;
            else
                return (int)obj;
        }

        set
        {
            this.ViewState["SearchTicketNumberForTicket"] = value;
        }
    }

    /// <summary>
    /// get/set search client id
    /// </summary>
    public int SearchClientId
    {
        get
        {
            object obj = this.ViewState["SearchClientIdForTicket"];
            if (obj == null)
                return -1;
            else
                return (int)obj;
        }

        set
        {
            this.ViewState["SearchClientIdForTicket"] = value;
        }
    }

    /// <summary>
    /// get/set search employee id
    /// </summary>
    public int SearchEmployeeId
    {
        get
        {
            object obj = this.ViewState["SearchEmployeeIdForTicket"];
            if (obj == null)
                return -1;
            else
                return (int)obj;
        }

        set
        {
            this.ViewState["SearchEmployeeIdForTicket"] = value;
        }
    }

    /// <summary>
    /// <summary>
    /// get/set search project manager employee id
    /// </summary>
    public int? SearchProjectManagerEmployeeId
    {
        get
        {
            object obj = this.ViewState["SearchProjectManagerEmployeeIdForTicket"];
            return (obj == null) ? (int?)null : (int)obj;
        }
        set { this.ViewState["SearchProjectManagerEmployeeIdForTicket"] = value; }
    }

    /// <summary>
    /// <summary>
    /// get/set search tech lead employee id
    /// </summary>
    public int? SearchTechLeadEmployeeId
    {
        get
        {
            object obj = this.ViewState["SearchTechLeadEmployeeIdForTicket"];
            return (obj == null) ? (int?)null : (int)obj;
        }
        set { this.ViewState["SearchTechLeadEmployeeIdForTicket"] = value; }
    }

    /// <summary>
    /// get/set search ticket status
    /// </summary>
    public DesktopShared.Ticket.Status SearchStatus
    {
        get
        {
            object obj = this.ViewState["SearchStatusForTicket"];
            if (obj == null)
                return DesktopShared.Ticket.Status.Closed;
            else
                return (DesktopShared.Ticket.Status)obj;
        }
        set
        {
            this.ViewState["SearchStatusForTicket"] = value;
        }
    }

    /// <summary>
    /// get/set search disposition id
    /// </summary>
    public int SearchDispositionId
    {
        get
        {
            object obj = this.ViewState["SearchDispositionIdForTicket"];
            if (obj == null)
                return -1;
            else
                return (int)obj;
        }

        set
        {
            this.ViewState["SearchDispositionIdForTicket"] = value;
        }
    }

    /// <summary>
    /// get/set search description
    /// </summary>
    public string SearchDescription
    {
        get
        {
            object obj = this.ViewState["SearchDescriptionForTicket"];
            if (obj == null)
                return "";
            else
                return (string)obj;
        }

        set
        {
            this.ViewState["SearchDescriptionForTicket"] = value;
        }
    }

    /// <summary>
    /// get/set search designation
    /// </summary>
    public string SearchDesignation
    {
        get
        {
            object obj = this.ViewState["SearchDesignationForTicket"];
            if (obj == null)
                return "";
            else
                return (string)obj;
        }

        set
        {
            this.ViewState["SearchDesignationForTicket"] = value;
        }
    }

    /// <summary>
    /// get/set search created start date
    /// </summary>
    public DateTime SearchCreatedStartDate
    {
        get
        {
            object obj = this.ViewState["SearchCreatedStartDateForTicket"];
            if (obj == null)
                return DateTime.MinValue;
            else
                return (DateTime)obj;
        }

        set
        {
            this.ViewState["SearchCreatedStartDateForTicket"] = value;
        }
    }

    /// <summary>
    /// get/set search created end date
    /// </summary>
    public DateTime SearchCreatedEndDate
    {
        get
        {
            object obj = this.ViewState["SearchCreatedEndDateForTicket"];
            if (obj == null)
                return DateTime.MinValue;
            else
                return (DateTime)obj;
        }

        set
        {
            this.ViewState["SearchCreatedEndDateForTicket"] = value;
        }
    }

    /// <summary>
    /// get/set search last updated start date
    /// </summary>
    public DateTime SearchLastUpdatedStartDate
    {
        get
        {
            object obj = this.ViewState["SearchLastUpdatedStartDateForTicket"];
            if (obj == null)
                return DateTime.MinValue;
            else
                return (DateTime)obj;
        }

        set
        {
            this.ViewState["SearchLastUpdatedStartDateForTicket"] = value;
        }
    }

    /// <summary>
    /// get/set search last updated end date
    /// </summary>
    public DateTime SearchLastUpdatedEndDate
    {
        get
        {
            object obj = this.ViewState["SearchLastUpdatedEndDateForTicket"];
            if (obj == null)
                return DateTime.MinValue;
            else
                return (DateTime)obj;
        }

        set
        {
            this.ViewState["SearchLastUpdatedEndDateForTicket"] = value;
        }
    }

    /// <summary>
    /// get/set search product id
    /// </summary>
    public int? SearchProductId
    {
        get
        {
            object obj = this.ViewState["SearchProductIdForTicket"];
            if (obj == null)
                return null;
            else
                return (int)obj;
        }

        set
        {
            this.ViewState["SearchProductIdForTicket"] = value;
        }
    }

    /// <summary>
    /// get/set search priority client
    /// </summary>
    public bool SearchPriorityClient
    {
        get
        {
            object obj = this.ViewState["SearchPriorityClientForTicket"];
            if (obj == null)
                return false;
            else
                return (bool)obj;
        }

        set
        {
            this.ViewState["SearchPriorityClientForTicket"] = value;
        }
    }

    /// <summary>
    /// get/set search archive
    /// </summary>
    public bool SearchArchive
    {
        get
        {
            object obj = this.ViewState["SearchArchiveForTicket"];
            if (obj == null)
                return false;
            else
                return (bool)obj;
        }

        set
        {
            this.ViewState["SearchArchiveForTicket"] = value;
        }
    }

    /// <summary>
    /// get/set search review
    /// </summary>
    public bool SearchReview
    {
        get
        {
            object obj = this.ViewState["SearchReviewForTicket"];
            if (obj == null)
                return false;
            else
                return (bool)obj;
        }

        set
        {
            this.ViewState["SearchReviewForTicket"] = value;
        }
    }

    /// <summary>
    /// get/set search awaiting response
    /// </summary>
    public bool SearchAwaitingResponse
    {
        get
        {
            object obj = this.ViewState["SearchAwaitingResponseForTicket"];
            if (obj == null)
                return false;
            else
                return (bool)obj;
        }

        set
        {
            this.ViewState["SearchAwaitingResponseForTicket"] = value;
        }
    }

    /// <summary>
    /// get/set search msp
    /// </summary>
    public bool SearchMsp
    {
        get
        {
            object obj = this.ViewState["SearchMspForTicket"];
            if (obj == null)
                return false;
            else
                return (bool)obj;
        }

        set
        {
            this.ViewState["SearchMspForTicket"] = value;
        }
    }

    /// <summary>
    /// get/set search team id
    /// </summary>
    public int? SearchTeamId
    {
        get
        {
            object obj = this.ViewState["SearchTeamIdForTicket"];
            if (obj == null)
                return null;
            else
                return (int)obj;
        }

        set
        {
            this.ViewState["SearchTeamIdForTicket"] = value;
        }
    }

    /// <summary>
    /// get/set search tier id
    /// </summary>
    public int? SearchTierId
    {
        get
        {
            object obj = this.ViewState["SearchTierIdForTicket"];
            if (obj == null)
                return null;
            else
                return (int)obj;
        }

        set
        {
            this.ViewState["SearchTierIdForTicket"] = value;
        }
    }

    /// <summary>
    /// get/set search created last number of days
    /// </summary>
    public int? SearchCreatedLastDays
    {
        get
        {
            object obj = this.ViewState["SearchCreatedLastDaysForTicket"];
            if (obj == null)
                return null;
            else
                return (int)obj;
        }
        set { this.ViewState["SearchCreatedLastDaysForTicket"] = value; }
    }

    /// <summary>
    /// get/set search last updated last number of days
    /// </summary>
    public int? SearchLastUpdatedLastDays
    {
        get
        {
            object obj = this.ViewState["SearchLastUpdatedLastDaysForTicket"];
            if (obj == null)
                return null;
            else
                return (int)obj;
        }
        set { this.ViewState["SearchLastUpdatedLastDaysForTicket"] = value; }
    }

    /// <summary>
    /// get/set search user created last number of days
    /// </summary>
    public int? SearchUserCreatedLastDays
    {
        get
        {
            object obj = this.ViewState["SearchUserCreatedLastDaysForTicket"];
            if (obj == null)
                return null;
            else
                return (int)obj;
        }
        set { this.ViewState["SearchUserCreatedLastDaysForTicket"] = value; }
    }

    /// <summary>
    /// get/set search user created last number of days user id
    /// </summary>
    public int? SearchUserCreatedLastDaysUserId
    {
        get
        {
            object obj = this.ViewState["SearchUserCreatedLastDaysUserIdForTicket"];
            if (obj == null)
                return null;
            else
                return (int)obj;
        }
        set { this.ViewState["SearchUserCreatedLastDaysUserIdForTicket"] = value; }
    }

    /// <summary>
    /// get/set search user last updated last number of days
    /// </summary>
    public int? SearchUserLastUpdatedLastDays
    {
        get
        {
            object obj = this.ViewState["SearchUserLastUpdatedLastDaysForTicket"];
            if (obj == null)
                return null;
            else
                return (int)obj;
        }
        set { this.ViewState["SearchUserLastUpdatedLastDaysForTicket"] = value; }
    }

    /// <summary>
    /// get/set search user last updated last number of days user id
    /// </summary>
    public int? SearchUserLastUpdatedLastDaysUserId
    {
        get
        {
            object obj = this.ViewState["SearchUserLastUpdatedLastDaysUserIdForTicket"];
            if (obj == null)
                return null;
            else
                return (int)obj;
        }
        set { this.ViewState["SearchUserLastUpdatedLastDaysUserIdForTicket"] = value; }
    }

    /// <summary>
    /// get/set search user viewed last number of days
    /// </summary>
    public int? SearchUserViewedLastDays
    {
        get
        {
            object obj = this.ViewState["SearchUserViewedLastDaysForTicket"];
            if (obj == null)
                return null;
            else
                return (int)obj;
        }
        set { this.ViewState["SearchUserViewedLastDaysForTicket"] = value; }
    }

    /// <summary>
    /// get/set search user viewed last number of days user id
    /// </summary>
    public int? SearchUserViewedLastDaysUserId
    {
        get
        {
            object obj = this.ViewState["SearchUserViewedLastDaysUserIdForTicket"];
            if (obj == null)
                return null;
            else
                return (int)obj;
        }
        set { this.ViewState["SearchUserViewedLastDaysUserIdForTicket"] = value; }
    }

    /// <summary>
    /// get/set search tag
    /// </summary>
    public string SearchTag
    {
        get
        {
            object obj = this.ViewState["SearchTagForTicket"];
            if (obj == null)
                return "";
            else
                return (string)obj;
        }
        set { this.ViewState["SearchTagForTicket"] = value; }
    }

    /// <summary>
    /// get/set search updated by user start date
    /// </summary>
    public DateTime? SearchUpdatedByUserStartDate
    {
        get
        {
            object obj = this.ViewState["SearchUpdatedByUserStartDateForTicket"];
            if (obj == null)
                return null;
            else
                return (DateTime)obj;
        }

        set
        {
            this.ViewState["SearchUpdatedByUserStartDateForTicket"] = value;
        }
    }

    /// <summary>
    /// get/set search updated by user end date
    /// </summary>
    public DateTime? SearchUpdatedByUserEndDate
    {
        get
        {
            object obj = this.ViewState["SearchUpdatedByUserEndDateForTicket"];
            if (obj == null)
                return null;
            else
                return (DateTime)obj;
        }

        set
        {
            this.ViewState["SearchUpdatedByUserEndDateForTicket"] = value;
        }
    }

    /// <summary>
    /// get/set search updated by user user id
    /// </summary>
    public int? SearchUpdatedByUserUserId
    {
        get
        {
            object obj = this.ViewState["SearchUpdatedByUserUserIdForTicket"];
            if (obj == null)
                return null;
            else
                return (int)obj;
        }
        set { this.ViewState["SearchUpdatedByUserUserIdForTicket"] = value; }
    }

    /// <summary>
    /// get/set search tag user id
    /// </summary>
    public int? SearchTagUserId
    {
        get
        {
            object obj = this.ViewState["SearchTagUserIdForTicket"];
            if (obj == null)
                return null;
            else
                return (int)obj;
        }
        set { this.ViewState["SearchTagUserIdForTicket"] = value; }
    }

    /// <summary>
    /// get/set search scheduled start date
    /// </summary>
    public DateTime? SearchScheduledStartDate
    {
        get
        {
            object obj = this.ViewState["SearchScheduledStartDateForTicket"];
            if (obj == null)
                return null;
            else
                return (DateTime)obj;
        }
        set { this.ViewState["SearchScheduledStartDateForTicket"] = value; }
    }

    /// <summary>
    /// get/set search scheduled end date
    /// </summary>
    public DateTime? SearchScheduledEndDate
    {
        get
        {
            object obj = this.ViewState["SearchScheduledEndDateForTicket"];
            if (obj == null)
                return null;
            else
                return (DateTime)obj;
        }
        set { this.ViewState["SearchScheduledEndDateForTicket"] = value; }
    }

    /// <summary>
    /// get/set search scheduled last number of days
    /// </summary>
    public int? SearchScheduledLastDays
    {
        get
        {
            object obj = this.ViewState["SearchScheduledLastDaysForTicket"];
            if (obj == null)
                return null;
            else
                return (int)obj;
        }
        set { this.ViewState["SearchScheduledLastDaysForTicket"] = value; }
    }

    /// <summary>
    /// get/set search priority id
    /// </summary>
    public int? SearchPriorityId
    {
        get
        {
            object obj = this.ViewState["SearchPriorityIdForTicket"];
            if (obj == null)
                return null;
            else
                return (int)obj;
        }
        set { this.ViewState["SearchPriorityIdForTicket"] = value; }
    }

    /// <summary>
    /// get/set search sales employee id
    /// </summary>
    public int? SearchSalesEmployeeId
    {
        get
        {
            object obj = this.ViewState["SearchSalesEmployeeIdForTicket"];
            return (obj == null) ? (int?)null : (int)obj;
        }
        set { this.ViewState["SearchSalesEmployeeIdForTicket"] = value; }
    }

    /// <summary>
    /// get/set search created by user id (CLIENT)
    /// </summary>
    public int? SearchCreatedByClientUserId
    {
        get
        {
            object obj = this.ViewState["SearchCreatedByClientUserIdForTicket"];
            return (obj == null) ? (int?)null : (int)obj;
        }
        set { this.ViewState["SearchCreatedByClientUserIdForTicket"] = value; }
    }

    // <summary>
    /// get/set search ticket type id
    /// </summary>
    public int? SearchTicketTypeId
    {
        get
        {
            object obj = this.ViewState["SearchTicketTypeIdForTicket"];
            return (obj == null) ? (int?)null : (int)obj;
        }
        set { this.ViewState["SearchTicketTypeIdForTicket"] = value; }
    }

    #endregion

    #region stored in ViewState

    //TODO: move properties from viewstate for better performance
    //store in cookie?

    /// <summary>
    /// get/set show hide done
    /// </summary>
    public bool SetShowHideDone
    {
        get
        {
            object obj = this.ViewState["SetShowHideDoneForTicket"];
            if (obj == null)
                return false;
            else
                return (bool)obj;
        }

        set
        {
            this.ViewState["SetShowHideDoneForTicket"] = value;
        }
    }

    /// <summary>
    /// get/set hide grid
    /// </summary>
    public bool HideGrid
    {
        get
        {
            object obj = this.ViewState["HideGridForTicket"];
            if (obj == null)
                return false;
            else
                return (bool)obj;
        }

        set
        {
            this.ViewState["HideGridForTicket"] = value;
        }
    }

    /// <summary>
    /// get/set highlight based on last updated
    /// </summary>
    public bool HighlightBasedOnLastUpdated
    {
        get
        {
            object obj = this.ViewState["HighlightBasedOnLastUpdatedForTicket"];
            if (obj == null)
                return true;
            else
                return (bool)obj;
        }

        set
        {
            this.ViewState["HighlightBasedOnLastUpdatedForTicket"] = value;
        }
    }

    /// <summary>
    /// get/set highlight priority client
    /// </summary>
    public bool HighlightPriorityClient
    {
        get
        {
            object obj = this.ViewState["HighlightPriorityClientForTicket"];
            if (obj == null)
                return true;
            else
                return (bool)obj;
        }

        set
        {
            this.ViewState["HighlightPriorityClientForTicket"] = value;
        }
    }

    /// <summary>
    /// get/set highlight desktop client
    /// </summary>
    public bool HighlightDesktopClient
    {
        get
        {
            object obj = this.ViewState["HighlightDesktopClientForTicket"];
            if (obj == null)
                return true;
            else
                return (bool)obj;
        }

        set
        {
            this.ViewState["HighlightDesktopClientForTicket"] = value;
        }
    }

    /// <summary>
    /// get/set highligh not updated for 24 hours
    /// </summary>
    public bool HighlightNotUpdatedFor24Hours
    {
        get
        {
            object obj = this.ViewState["HighlightNotUpdatedFor24Hours"];
            if (obj == null)
                return false;
            else
                return (bool)obj;
        }

        set
        {
            this.ViewState["HighlightNotUpdatedFor24Hours"] = value;
        }
    }

    /// <summary>
    /// get/set include Time spent column
    /// </summary>
    public bool IncludeTimeSpent
    {
        get
        {
            object obj = this.ViewState["IncludeTimeSpent"];
            if (obj == null)
                return false;
            else
                return (bool)obj;
        }

        set
        {
            this.ViewState["IncludeTimeSpent"] = value;
        }
    }

    /// <summary>
    /// get/set grid is in add or edit mode
    /// </summary>
    public bool IsAddEditMode
    {
        get { return (rgTicket.EditItems.Count > 0) || (rgTicket.MasterTableView.IsItemInserted); }
    }

    /// <summary>
    /// get/set is edit all mode
    /// </summary>
    public bool IsBatchEditMode
    {
        get
        {
            object obj = this.ViewState["IsBatchEditMode"];
            if (obj == null)
                return false;
            else
                return (bool)obj;
        }

        set
        {
            this.ViewState["IsBatchEditMode"] = value;
        }
    }

    /// <summary>
    /// get/set current tag
    /// </summary>
    public string CurrentTag
    {
        get
        {
            object obj = this.Session["CurrentTag" + rgTicket.ClientID.ToString()];
            if (obj == null)
                return "";
            else
                return (string)obj;
        }

        set
        {
            this.Session["CurrentTag" + rgTicket.ClientID.ToString()] = value;
            CurrentTagStripPublic = value;
        }
    }

    /// <summary>
    /// get/set search tag and strip out public ("(P)" or "(PC)") text
    /// </summary>
    public string CurrentTagStripPublic
    {
        get
        {
            object obj = this.Session["CurrentTagStripPublic" + rgTicket.ClientID.ToString()];
            if (obj == null)
                return "";
            else
                return obj.ToString().Replace("(P)", "").Replace("(PC)", "");
        }
        set { this.Session["CurrentTagStripPublic" + rgTicket.ClientID.ToString()] = value.Replace("(P)", "").Replace("(PC)", ""); }
    }

    /// <summary>
    /// get/set tag is public 
    /// </summary>
    public bool TagIsPublic
    {
        get
        {
            if (ucUserTicketTags.TagName != BitByBit.Configuration.GetConfigString("DefaultTagText", "Default"))
                return ucUserTicketTags.IsPublic;
            else
                return false;
        }
    }

    /// <summary>
    /// get/set tag is client public
    /// </summary>
    public bool TagIsClientPublic
    {
        get
        {
            if (ucUserTicketTags.TagName != BitByBit.Configuration.GetConfigString("DefaultTagText", "Default"))
                return ucUserTicketTags.IsClientPublic;
            else
                return false;
        }
    }

    /// <summary>
    /// get ticket view drop down list user control
    /// </summary>
    public UserControl_DropDownList_TicketView TicketViewUserControl
    {
        get { return ucTicketView; }
    }

    /// <summary>
    /// get/set drop down selection in view control if id is present in cookie
    /// </summary>
    public bool SetViewDropDownListSelection
    {
        get
        {
            object obj = this.ViewState["SetViewDropDownListSelectionForTicket"];
            return (obj == null) ? true : (bool)obj;
        }
        set { this.ViewState["SetViewDropDownListSelectionForTicket"] = value; }
    }

    #endregion

    #region private properties

    /// <summary>
    /// get/set last updated warning
    /// </summary>
    private int LastUpdatedWarning
    {
        get
        {
            object obj = this.Session["LastUpdatedWarning"];
            if (obj == null)
                return 4;
            else
                return (int)obj;
        }

        set
        {
            this.Session["LastUpdatedWarning"] = value;
        }
    }
    /// <summary>
    /// get/set search should inclue csc history collection
    /// true values decreases perfomrance of query
    /// </summary>
    public bool SearchHistory
    {
        get
        {
            object obj = this.ViewState["t_sih"];
            return (obj == null) ? false : (bool)obj;
        }
        set { this.ViewState["t_sih"] = value; }
    }

    /// <summary>
    /// get is view grid
    /// </summary>
    private bool IsViewGrid
    {
        get
        {
            return _type == DesktopShared.Ticket.GridType.View || _type == DesktopShared.Ticket.GridType.View1 || _type == DesktopShared.Ticket.GridType.View2 || _type == DesktopShared.Ticket.GridType.View3;
        }
    }

    #endregion

    #region private Method
    /// <summary>
    /// configure grid for export
    /// </summary>
    private void ConfigureExport()
    {
        rgTicket.ExportSettings.IgnorePaging = true;
        rgTicket.ExportSettings.OpenInNewWindow = true;
        rgTicket.ExportSettings.ExportOnlyData = true;
        rgTicket.ExportSettings.HideStructureColumns = true;

        rgTicket.ExportSettings.FileName = "Tickets_" + DateTime.Now.ToString("yyyyMMdd-hhmmss");

        //hide command item template
        foreach (GridItem commandItem in this.rgTicket.MasterTableView.GetItems(GridItemType.CommandItem))
        {
            commandItem.Visible = false;
            commandItem.Display = false;
        }

        //hide link columns
        rgTicket.MasterTableView.Columns.FindByUniqueName("ActionColumn").Visible = false;
        rgTicket.MasterTableView.Columns.FindByUniqueName("Tag").Visible = false;

        //show description
        rgTicket.MasterTableView.Columns.FindByUniqueName("Description").Visible = true;

        _isExport = true;
    }

    /// <summary>
    /// set up grid header
    /// </summary>
    private void SetUpHeader()
    {
        string _show = " [click to display]";
        string _hide = " [click to hide]";
        string _start = "";

        if (_type == DesktopShared.Ticket.GridType.Employee)
        {
            _start = "Tickets Assigned to " + BitByBit.Utility.String.FirstUpper
                (DesktopShared.User.GetUserNameOnly());

            cpeTicket.ExpandedText = _start + _hide;
            cpeTicket.CollapsedText = _start + _show;
        }
        else if (_type == DesktopShared.Ticket.GridType.Active)
        {
            _start = "Recent Tickets (past two days)";

            cpeTicket.ExpandedText = _start + _hide;
            cpeTicket.CollapsedText = _start + _show;
        }
        else if (_type == DesktopShared.Ticket.GridType.Scheduled)
        {
            _start = "Scheduled Tickets";

            cpeTicket.ExpandedText = _start + _hide;
            cpeTicket.CollapsedText = _start + _show;
        }
        else if (_type == DesktopShared.Ticket.GridType.HelpDesk)
        {
            _start = "Triage Queue Tickets";

            cpeTicket.ExpandedText = _start + _hide;
            cpeTicket.CollapsedText = _start + _show;

        }
        else if (_type == DesktopShared.Ticket.GridType.Backup)
        {
            _start = "Backup Tickets";

            cpeTicket.ExpandedText = _start + _hide;
            cpeTicket.CollapsedText = _start + _show;

        }
        else if (_type == DesktopShared.Ticket.GridType.Level2Queue)
        {
            _start = "To Be Determined (TBD) Queue Tickets";

            cpeTicket.ExpandedText = _start + _hide;
            cpeTicket.CollapsedText = _start + _show;

        }
        else if (_type == DesktopShared.Ticket.GridType.PriorityUser)
        {
            _start = "Priority Queue Tickets";

            cpeTicket.ExpandedText = _start + _hide;
            cpeTicket.CollapsedText = _start + _show;

        }
        else if (_type == DesktopShared.Ticket.GridType.Search)
        {
            _start = "Search Tickets";

            cpeTicket.ExpandedText = _start + _hide;
            cpeTicket.CollapsedText = _start + _show;
        }
        else if (_type == DesktopShared.Ticket.GridType.PriorityClient)
        {
            _start = "Priority Client Tickets";

            cpeTicket.ExpandedText = _start + _hide;
            cpeTicket.CollapsedText = _start + _show;
        }
        else if (_type == DesktopShared.Ticket.GridType.HelpDeskClient)
        {
            _start = "Help Desk Client Tickets";

            cpeTicket.ExpandedText = _start + _hide;
            cpeTicket.CollapsedText = _start + _show;
        }
        else if (_type == DesktopShared.Ticket.GridType.DesignationAlert)
        {
            _start = "Alert (Designation) Tickets";

            cpeTicket.ExpandedText = _start + _hide;
            cpeTicket.CollapsedText = _start + _show;
        }
        else if (_type == DesktopShared.Ticket.GridType.Tagged)
        {
            _start = "My Tags";

            cpeTicket.ExpandedText = _start + _hide;
            cpeTicket.CollapsedText = _start + _show;

            //do not display public tags in tagged ticket grid
            ucUserTicketTags.DisplayUserTagsOnly = true;
        }
        else if (IsViewGrid)
        {
            if (_type == DesktopShared.Ticket.GridType.View || _type == DesktopShared.Ticket.GridType.View1)
                _start = "Ticket View";
            else if (_type == DesktopShared.Ticket.GridType.View2)
                _start = "Ticket View 2";
            else if (_type == DesktopShared.Ticket.GridType.View3)
                _start = "Ticket View 3";

            cpeTicket.ExpandedText = _start + _hide;
            cpeTicket.CollapsedText = _start + _show;
        }

        SetUpTableHeader();
    }

    /// <summary>
    /// rebind grid
    /// </summary>
    /// <param name="grid"></param>
    private void RebindGrid(Telerik.Web.UI.RadGrid grid)
    {
        if (grid != null)
        {
            grid.DataSource = null;
            grid.Rebind();
        }
    }

    /// <summary>
    /// set up main table header (Last Updated, Employee Tag, Records per page etc).
    /// main point is to display view ddl for view ticket and updated width of cells
    /// </summary>
    private void SetUpTableHeader()
    {
        if (IsViewGrid)
        {
            //width of cells need to be changed from 20 to 16 %
            tblGridHeaderCell1.Width = Unit.Percentage(16);
            //tblGridHeaderCell2.Width = Unit.Percentage(16);
            tblGridHeaderCell4.Width = Unit.Percentage(16);
            //tblGridHeaderCell5.Width = Unit.Percentage(16);
            //tblGridHeaderCell6.Width = Unit.Percentage(16);

            //display cell containing views ddl & views ddl
            tblGridHeaderCell3.Visible = true;
            ucTicketView.Visible = true;
        }

        //text alignment
        //tblGridHeaderCell2.Attributes.CssStyle.Add("text-align", "center");
        //tblGridHeaderCell5.Attributes.CssStyle.Add("text-align", "right");
        //tblGridHeaderCell6.Attributes.CssStyle.Add("text-align", "right");
    }

    /// <summary>
    /// set employee tag drop down list selected value
    /// </summary>
    private void SetUpTagValue()
    {
        //set default current tag if no value present
        DropDownList ddlUserTicketTags = ucUserTicketTags.GetDropDownList();
        ListItem li = ddlUserTicketTags.Items.FindByText(CurrentTag);
        if (li == null)
            CurrentTag = ucUserTicketTags.TagName;
        else
        {
            ddlUserTicketTags.ClearSelection();
            li.Selected = true;
        }

        //set tag user id for grid.  not used by view or search types
        //use user id for private tags, null for public tags
        if ((!IsViewGrid) && (_type != DesktopShared.Ticket.GridType.Search))
            SearchTagUserId = ucUserTicketTags.IsPublic ? (int?)null : DesktopShared.User.UserID;
    }
    private Telerik.Web.UI.RadGrid GetGrid(DesktopShared.Ticket.GridType gridType)
    {
        Telerik.Web.UI.RadGrid _grid = null;

        switch (gridType)
        {
            case DesktopShared.Ticket.GridType.Active:
                {
                    if (_recentTicketUserControl != null)
                        _grid = (Telerik.Web.UI.RadGrid)_recentTicketUserControl.FindControl(_gridId);

                    break;
                }
            case DesktopShared.Ticket.GridType.Backup:
                {
                    if (_backupTicketUserControl != null)
                        _grid = (Telerik.Web.UI.RadGrid)_backupTicketUserControl.FindControl(_gridId);

                    break;
                }
            case DesktopShared.Ticket.GridType.DesignationAlert:
                {
                    if (_designationAlertTicketUserControl != null)
                        _grid = (Telerik.Web.UI.RadGrid)_designationAlertTicketUserControl.FindControl(_gridId);

                    break;
                }
            case DesktopShared.Ticket.GridType.Employee:
                {
                    if (_employeeTicketUserControl != null)
                        _grid = (Telerik.Web.UI.RadGrid)_employeeTicketUserControl.FindControl(_gridId);

                    break;
                }
            case DesktopShared.Ticket.GridType.HelpDesk:
                {
                    if (_helpDeskTicketUserControl != null)
                        _grid = (Telerik.Web.UI.RadGrid)_helpDeskTicketUserControl.FindControl(_gridId);

                    break;
                }
            case DesktopShared.Ticket.GridType.HelpDeskClient:
                {
                    if (_helpDeskClientTicketUserControl != null)
                        _grid = (Telerik.Web.UI.RadGrid)_helpDeskClientTicketUserControl.FindControl(_gridId);

                    break;
                }
            case DesktopShared.Ticket.GridType.Level2Queue:
                {
                    if (_level2QueueTicketUserControl != null)
                        _grid = (Telerik.Web.UI.RadGrid)_level2QueueTicketUserControl.FindControl(_gridId);

                    break;
                }

            case DesktopShared.Ticket.GridType.PriorityClient:
                {
                    if (_priorityClientTicketUserControl != null)
                        _grid = (Telerik.Web.UI.RadGrid)_priorityClientTicketUserControl.FindControl(_gridId);

                    break;
                }
            case DesktopShared.Ticket.GridType.PriorityUser:
                {
                    if (_priorityUserTicketUserControl != null)
                        _grid = (Telerik.Web.UI.RadGrid)_priorityUserTicketUserControl.FindControl(_gridId);

                    break;
                }
            case DesktopShared.Ticket.GridType.Scheduled:
                {
                    if (_scheduledTicketUserControl != null)
                        _grid = (Telerik.Web.UI.RadGrid)_scheduledTicketUserControl.FindControl(_gridId);

                    break;
                }
            case DesktopShared.Ticket.GridType.Tagged:
                {
                    if (_taggedTicketUserControl != null)
                        _grid = (Telerik.Web.UI.RadGrid)_taggedTicketUserControl.FindControl(_gridId);

                    break;
                }
            case DesktopShared.Ticket.GridType.View:
                {
                    if (_viewTicketUserControl != null)
                        _grid = (Telerik.Web.UI.RadGrid)_viewTicketUserControl.FindControl(_gridId);

                    break;
                }

        }

        return _grid;
    }
    #endregion
}