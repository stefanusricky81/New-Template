using System;
using System.Collections;
using System.Collections.Generic;
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
using Telerik.Web.UI;
using SD.LLBLGen.Pro.ORMSupportClasses;
using System.Text;

public partial class UserControl_Grid_EditForm_TicketEdit : AbstractTicketEdit
{
    private EditMode _editMode = EditMode.Grid;
    private System.Text.StringBuilder sbToolTipJs = new System.Text.StringBuilder();
    private string _ticketCheckListHeader = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        ConfigurePhoneValidations();
        SetUpControl();
    }

    #region private methods

    /// <summary>
    /// confirue phone validation
    /// </summary>
    private void ConfigurePhoneValidations()
    {
        revPhone.ValidationExpression = DesktopShared.Utility.ValidPhoneNumberRegEx;
        revPhone.ErrorMessage = DesktopShared.Utility.InvalidPhoneNumberMessage("Phone");
        revCellPhone.ValidationExpression = DesktopShared.Utility.ValidPhoneNumberRegEx;
        revCellPhone.ErrorMessage = DesktopShared.Utility.InvalidPhoneNumberMessage("Cell Phone");
        revHomePhone.ValidationExpression = DesktopShared.Utility.ValidPhoneNumberRegEx;
        revHomePhone.ErrorMessage = DesktopShared.Utility.InvalidPhoneNumberMessage("Home Phone");
    }

    /// <summary>
    /// set up ajax collapsible panels
    /// </summary>
    private void SetUpCollapsiblePanels()
    {
        //always show general panel as open
        cpeGeneral.Collapsed = false;

        //always show check list as open
        cpeCheckList.Collapsed = false;

        //always show details panel as open
        cpeDetails.Collapsed = false;

        //always show history panel as open
        cpeHistory.Collapsed = false;

        //collapse all other panels for grid edit in place
        bool collapsePanel = (_editMode == EditMode.Grid);

        cpeClient.Collapsed = collapsePanel;
        cpeClientHistory.Collapsed = collapsePanel;
        cpeEmployeeEmail.Collapsed = collapsePanel;
        cpeClientEmail.Collapsed = collapsePanel;
        cpeTags.Collapsed = collapsePanel;
        cpeFiles.Collapsed = collapsePanel;
    }

    /// <summary>
    /// set up user control
    /// </summary>
    private void SetUpControl()
    {
        SetUpCollapsiblePanels();

        if (_editMode == EditMode.Grid)
        {
            mvTopButtons.SetActiveView(viewTopGrid);
            mvBottomButtons.SetActiveView(viewBottomGrid);

            litTicketNumber.Visible = false;
            hlTicketNumber.Visible = true;
        }
        else
        {
            mvTopButtons.SetActiveView(viewTopPage);
            mvBottomButtons.SetActiveView(viewBottomPage);

            litTicketNumber.Visible = true;
            hlTicketNumber.Visible = false;

            divMain.Style.Add("border", "1px solid #3b5a82");
        }

        #region wire up rcbClient SelectedIndexChanged

        Telerik.Web.UI.RadComboBox rcbClient = (Telerik.Web.UI.RadComboBox)ucClient.FindControl("rcbClient");
        if (rcbClient != null)
        {
            rcbClient.AutoPostBack = true;
            rcbClient.SelectedIndexChanged += new RadComboBoxSelectedIndexChangedEventHandler(rcbClient_SelectedIndexChanged);
        }

        #endregion

        #region wire up ucClientContact SelectedIndexChanged

        DropDownList ddlClientContact = ucClientContact.GetDropDownList();
        if (ddlClientContact != null)
        {
            ddlClientContact.AutoPostBack = true;
            ddlClientContact.SelectedIndexChanged += new EventHandler(ucClientContact_SelectedIndexChanged);
        }

        #endregion

        #region css class for check boxes

        chkNotifyClient.InputAttributes.Add("class", "checkbox");
        chkEnteredByRecieveTicketOnClose.InputAttributes.Add("class", "checkbox");
        chkAddTimesheet.InputAttributes.Add("class", "checkbox");
        chkScheduledSendInvite.InputAttributes.Add("class", "checkbox");
        chkSilentMode.InputAttributes.Add("class", "checkbox");
        chkHistoryExcludeAlerts.InputAttributes.Add("class", "checkbox");
        chkHistoryExcludeViews.InputAttributes.Add("class", "checkbox");

        #endregion
    }

    #endregion

    #region protected methods / events

    /// <summary>
    /// copy check list master to ticket notes
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnCopyCheckList_Click(object sender, EventArgs e)
    {
        if (!ddlTicketCheckListMaster.TicketCheckListMasterId.HasValue)
            return;

        DesktopShared.EntityClasses.TicketCheckListMasterEntity objTicketCheckListMaster = new DesktopShared.EntityClasses.TicketCheckListMasterEntity(ddlTicketCheckListMaster.TicketCheckListMasterId.Value);
        if (objTicketCheckListMaster.Fields.State == EntityState.Fetched)
            tbNotes.Text = objTicketCheckListMaster.CheckListCode.Trim();

        ddlTicketCheckListMaster.TicketCheckListMasterId = null;
    }

    /// <summary>
    /// view client history grid on button click
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnViewClientHistoryGrid_Click(object sender, EventArgs e)
    {
        mvClientHistory.SetActiveView(viewClientHistoryGrid);
        ucTicketClientHistoryGrid.ResetGrid();
    }

    /// <summary>
    /// history exclude views on checked changed
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void chkHistoryExcludeViews_CheckedChanged(object sender, EventArgs e)
    {
        ucTicketHistoryGrid.ExcludeViews = chkHistoryExcludeViews.Checked;
        ucTicketHistoryGrid.RebindGrid();
    }

    /// <summary>
    /// history exclude alerts on checked changed
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void chkHistoryExcludeAlerts_CheckedChanged(object sender, EventArgs e)
    {
        ucTicketHistoryGrid.ExcludeAlerts = chkHistoryExcludeAlerts.Checked;
        ucTicketHistoryGrid.RebindGrid();
    }

    /// <summary>
    /// client combo box on selected index changed
    /// </summary>
    /// <param name="o"></param>
    /// <param name="e"></param>
    protected void rcbClient_SelectedIndexChanged(object o, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
    {
        int selectedId = 0;

        if (e.Value.Length > 0)
        {
            try { selectedId = Convert.ToInt32(e.Value); }
            catch { selectedId = -1; }
        }
        else
            ucClient.ClearItems();

        DesktopShared.EntityClasses.ClientEntity objClient = new DesktopShared.EntityClasses.ClientEntity(selectedId);

        ucClientContactCBL.SelectedClientId = selectedId;
        ucTimesheetProject.SelectedClientId = selectedId;
        ucClientContact.SelectedClientId = selectedId;

        ucEmployeeAssignedTo.HelpDeskClientId = selectedId;
        AssignedTo = AssignedTo;
        ucEmployeeAssignedTo.Populate();

        //check for on hold status
        phClientOnHoldWarning.Visible = DesktopShared.Client.Status.IsOnHold(objClient);

        //default ticket type
        if (!ddlTicketType.TicketTypeId.HasValue)
        {
            if (objClient.Fields.State == EntityState.Fetched)
                ddlTicketType.TicketTypeId = objClient.TicketTypeId;
        }

        //category required
        bool _categoryRequired = false;
        if (objClient.Fields.State == EntityState.Fetched)
            _categoryRequired = objClient.TicketCategoryRequired;
        ddlTicketCategory.IsRequired = _categoryRequired;
        litCategoryHeader.Text = _categoryRequired ? "<b>Category</b>" : "Category";

        #region notifications modals

        System.Text.StringBuilder _sbNotifications = new StringBuilder();

        //employees
        List<DesktopShared.Ticket.Notification.Contact> _notificationsEmployee = DesktopShared.Ticket.Notification.GetEmployees(TicketId);
        if (_notificationsEmployee.Count > 0)
        {
            _sbNotifications.Append("<strong>Employees</strong>");
            foreach (var _contact in _notificationsEmployee)
                _sbNotifications.Append(String.Format("<br />{0}", _contact.FullName));
        }

        //client contacts
        /*List<DesktopShared.Ticket.Notification.Contact> _notificationsClientContact = DesktopShared.Ticket.Notification.GetClientContacts(TicketId);
        if (_notificationsClientContact.Count > 0)
        {
            if (_sbNotifications.Length > 0)
                _sbNotifications.Append("<br /><br />");

            _sbNotifications.Append("<strong>Client</strong>");
            foreach (var _contact in _notificationsClientContact)
                _sbNotifications.Append(String.Format("<br />{0}", _contact.FullName));
        }*/

        //email cc for tooltip
        List<string> _autoCcEmails = DesktopShared.Client.Contact.GetTicketAutoCc(selectedId);
        if (_autoCcEmails.Count > 0)
        {
            if (_sbNotifications.Length > 0)
                _sbNotifications.Append("<br /><br />");

            _sbNotifications.Append("<strong>Auto Email CC</strong>");
            foreach (string _autoCcEmail in _autoCcEmails)
                _sbNotifications.Append(String.Format("<br />{0}", _autoCcEmail));
        }

        //tool tip to display who will be notified
        if (_sbNotifications.Length > 0)
        {
            hlNotifications.Visible = true;
            rtpNotifications.Text = _sbNotifications.ToString();
        }
        else
            hlNotifications.Visible = false;

        #endregion
    }

    /// <summary>
    /// client contact drop down list on selected index changed
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ucClientContact_SelectedIndexChanged(object sender, System.EventArgs e)
    {
        if (ucClientContact.ClientContactId > 0)
        {
            DesktopShared.EntityClasses.UsersEntity _user =
                new DesktopShared.EntityClasses.UsersEntity(ucClientContact.ClientContactId);

            ClientContactFirstName = _user.First.Trim();
            ClientContactLastName = _user.Last.Trim();
            ClientContactPhone = _user.Phone.Trim();
            ClientContactEmail = _user.Email.Trim();

            var _clientContact = new DesktopShared.EntityClasses.ClientContactEntity(_user.FkClientContact.HasValue ? _user.FkClientContact.Value : -1);
            if (_clientContact.Fields.State == EntityState.Fetched)
            {
                ClientContactCellPhone = _clientContact.Cellphone.Trim();
                ClientContactHomePhone = _clientContact.Homephone.Trim();
            }
        }
    }

    /// <summary>
    /// update button click
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnUpdatePage_Click(object sender, EventArgs e)
    {
        evButtonClick(this, e);
    }

    /// <summary>
    /// ticket check list repeater on item databound
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rptCheckList_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        DesktopShared.EntityClasses.TicketCheckListEntity objTicketCheckList = (DesktopShared.EntityClasses.TicketCheckListEntity)e.Item.DataItem;
        
        if (!String.Equals(_ticketCheckListHeader, objTicketCheckList.Header.Trim(), StringComparison.OrdinalIgnoreCase))
        {
            PlaceHolder phHeader = (PlaceHolder)e.Item.FindControl("phHeader");
            phHeader.Visible = true;

            Literal litCheckListHeader = (Literal)e.Item.FindControl("litCheckListHeader");
            litCheckListHeader.Text = objTicketCheckList.Header.Trim();
        }

        CheckBox chkCheckListCompleted = (CheckBox)e.Item.FindControl("chkCheckListCompleted");
        bool _completed = objTicketCheckList.Completed;
        chkCheckListCompleted.Checked = _completed;
        if (_completed)
        {
            chkCheckListCompleted.Enabled = false;
        }

        Literal litCheckListName = (Literal)e.Item.FindControl("litCheckListName");
        litCheckListName.Text = objTicketCheckList.Task.Trim();
        if (_completed && objTicketCheckList.CompletedDate.HasValue)
            litCheckListName.Text += String.Format(" <em>[Completed {0}]</em>", objTicketCheckList.CompletedDate.Value);

        Literal litCheckListId = (Literal)e.Item.FindControl("litCheckListId");
        litCheckListId.Text = objTicketCheckList.Id.ToString();

        _ticketCheckListHeader = objTicketCheckList.Header.Trim();
    }

    #endregion

    #region public methods

    /// <summary>
    /// load ticket values via ticked id
    /// </summary>
    /// <param name="ticketID"></param>
    public override void LoadValues(int ticketID)
    {
        DesktopShared.TypedListClasses.TicketTypedList tickets =
            new DesktopShared.TypedListClasses.TicketTypedList();

        IPredicateExpression ticketsFilter = new PredicateExpression();
        ticketsFilter.Add(DesktopShared.HelperClasses.CscDefectsFields.Pcscdefects == ticketID);

        tickets.Fill(0, null, false, ticketsFilter);

        LoadValues(tickets[0], true);
    }

    /// <summary>
    /// load ticket values via ticked id
    /// </summary>
    /// <param name="ticketID"></param>
    public override void LoadValues(int ticketID, bool insertViewHistory)
    {
        DesktopShared.TypedListClasses.TicketTypedList tickets =
            new DesktopShared.TypedListClasses.TicketTypedList();

        IPredicateExpression ticketsFilter = new PredicateExpression();
        ticketsFilter.Add(DesktopShared.HelperClasses.CscDefectsFields.Pcscdefects == ticketID);

        tickets.Fill(0, null, false, ticketsFilter);

        LoadValues(tickets[0], insertViewHistory);
    }

    /// <summary>
    /// load ticket values ticket typed list row
    /// </summary>
    /// <param name="ticket"></param>
    public override void LoadValues(DesktopShared.TypedListClasses.TicketRow ticket)
    {
        LoadValues(ticket, true);
    }

    /// <summary>
    /// load ticket values ticket typed list row
    /// </summary>
    /// <param name="ticket"></param>
    public override void LoadValues(DesktopShared.TypedListClasses.TicketRow ticket, bool insertViewHistory)
    {
        #region general

        //on hold warning
        phClientOnHoldWarning.Visible = DesktopShared.Client.Status.IsOnHold(ticket.FkClient);

        TicketId = ticket.Pcscdefects;
        hlDetail2.NavigateUrl = String.Format("/Ticket/Detail2.aspx?Id={0}", TicketId);
        Summary = ticket.Summary.Trim();

        ucEmployeeAssignedTo.HelpDeskClientId = ticket.FkClient;
        AssignedTo = ticket.Assignedto;
        OriginalAssignedToName = String.Format("{0} {1}", ticket.AssignedToFirst.Trim(), ticket.AssignedToLast.Trim());

        HoursToFix = ticket.HoursToFix.Trim();
        ReportedByName = ticket.Reportedby.Trim();
        ReportedDate = ticket.DateEntered;
        rtpDetails.Text = ticket.Description.Trim().Replace("\n", "<br />");

        //disposition
        if (ticket.FkDisposition > 0)
            DispositionId = ticket.FkDisposition;
        else
            ucTicketDisposition.Populate();

        //ticket type id
        ddlTicketType.TicketTypeId = ticket.TicketTypeId;

        //ticket categroy id
        ddlTicketCategory.TicketCategoryId = ticket.TicketCategoryId;
        ddlTicketCategory.IsRequired = ticket.TicketCategoryRequired;
        litCategoryHeader.Text = ticket.TicketCategoryRequired ? "<b>Category</b>" : "Category";

        //product
        if (ticket.ProductId > 0)
            ProductId = ticket.ProductId;
        else
            ucTicketProduct.Populate();
        
        Notes = "";
        InternalNotes = "";
        chkNotifyClient.Checked = true;

        DropDownList _ddl = ddlTicketCheckListMaster.GetDropDownList();
        if (_ddl.Items.Count == 0)
            ddlTicketCheckListMaster.Populate();

        #region time spent so far on ticket

        DesktopShared.CollectionClasses.TimesheetCollection _timeSheetsSum =
            new DesktopShared.CollectionClasses.TimesheetCollection();

        IPredicateExpression _timeSheetsSumFilter = new PredicateExpression();
        _timeSheetsSumFilter.Add(DesktopShared.HelperClasses.TimesheetFields.FkCscdefects == TicketId);
        _timeSheetsSumFilter.Add(DesktopShared.HelperClasses.TimesheetFields.FkProject > 0);
        _timeSheetsSumFilter.Add(DesktopShared.HelperClasses.TimesheetFields.FkClient > 0);

        double _hoursSpent = 0;
        object result = _timeSheetsSum.GetScalar(
             DesktopShared.TimesheetFieldIndex.Hrs, null, AggregateFunction.Sum, _timeSheetsSumFilter);
        if (result != DBNull.Value)
            _hoursSpent = (double)result;

        hlTimeSpent.Text = _hoursSpent.ToString();
        hlTimeSpent.NavigateUrl = "/Timesheet/Default.aspx?TicketId=" + TicketId.ToString();

        #endregion

        #region timesheet

        AddTimesheet = true;
        TimesheetIsOvertime = false;
        TimesheetDate = DateTime.Now;
        TimesheetActivityId = DesktopShared.User.DefaultActivityId;
        ucTimesheetProject.SelectedClientId = ticket.FkClient;

        string _startTime = "0800";
        string _timeSpent = "0000";

        DesktopShared.Timesheet.GetStartTimeAndTimeSpent(ref _startTime, ref _timeSpent);

        TimesheetStartTime = _startTime;
        TimesheetTimeSpent = _timeSpent;

        #endregion

        //priority
        if (ticket.FkPriority > 0)
            PriorityId = ticket.FkPriority;
        else
            ucTicketPriority.Populate();

        //scheduled
        if (ticket.ScheduledDate != DateTime.MinValue)
            ScheduledDateTime = ticket.ScheduledDate;

        //scheduled duration
        ScheduledDuration = DesktopShared.Utility.Time.TimeDoubleToString(ticket.ScheduledDuration);

        //need completed by
        if (ticket.NeedCompletionDate != DateTime.MinValue)
            NeedCompletedByDateTime = ticket.NeedCompletionDate;

        //received
        ReceivedDateTime = ticket.ReceiveDate;

        #endregion

        #region check lists tasks

        phCheckList.Visible = false;
        DesktopShared.CollectionClasses.TicketCheckListCollection _tclCollection = DesktopShared.Ticket.CheckList.Get(ticket.Pcscdefects);
        if (_tclCollection.Count > 0)
        {
            phCheckList.Visible = true;
            rptCheckList.DataSource = _tclCollection;
            rptCheckList.DataBind();
        }

        #endregion

        #region client

        if (ticket.FkClient > 0)
        {
            SelectedClientId = ticket.FkClient;
            ucClientContact.SelectedClientId = ticket.FkClient;
        }

        ClientContactFirstName = ticket.ReportedByFirst.Trim();
        ClientContactLastName = ticket.ReportedByLast.Trim();
        ClientContactPhone = ticket.Phone.Trim();
        ClientContactEmail = ticket.Email.Trim();
        ClientContactId = ticket.FkUser;

        if (!ticket.IsFkUserNull())
        {
            var objUser = new DesktopShared.EntityClasses.UsersEntity(ticket.FkUser);
            if (objUser.Fields.State == EntityState.Fetched)
            {
                var objClientContact = new DesktopShared.EntityClasses.ClientContactEntity(objUser.FkClientContact.HasValue ? objUser.FkClientContact.Value : -1);
                if (objClientContact.Fields.State == EntityState.Fetched)
                {
                    ClientContactCellPhone = objClientContact.Cellphone.Trim();
                    ClientContactHomePhone = objClientContact.Homephone.Trim();
                    ClientContactFirstName = objClientContact.First.Trim();
                    ClientContactLastName = objClientContact.Last.Trim();
                    ClientContactEmail = objClientContact.Email.Trim();
                }
            }
        }

        //copy quick summary to clipboard
        //Client - Subject - Assigned to John Doe - Link to Ticket
        string _assignedTo = "";
        if (ucEmployeeAssignedTo.EmployeeName.Trim().Length > 0)
            _assignedTo = String.Format("- Assigned to {0}", ucEmployeeAssignedTo.EmployeeName.Trim());
        string _textToCopy = String.Format("{0} - {1}{2} - {3}", ticket.ClientCompany.Trim(), ticket.Summary.Trim(), _assignedTo, HttpContext.Current.Request.Url.AbsoluteUri.Trim());
        litCopySummary.Text = String.Format("<a class=\"copy-btn\" data-clipboard-text=\"{0}\" href=\"javascript:void(0);\">Copy summary to clipboard</a>", _textToCopy);

        #endregion

        #region details

        //designation
        if (!String.IsNullOrEmpty(ticket.Designation) && ticket.Designation != "U")
            Designation = ticket.Designation;
        else
            Designation = "N"; // default to network

        //received method
        ReceivedMethod = ticket.ReceiveMethod.Trim();

        //is recurring ticket
        IsRecurring = ticket.IsRecurring;

        //auto close
        AutoClose = ticket.AllowAutoClose == "Y" ? true : false;

        //archive
        Archive = ticket.Archive;

        //review
        Review = ticket.Review;

        //internal only
        InternalOnly = ticket.InternalOnly;

        //use for reporting
        UseForReporting = ticket.UseForReporting;

        //silent mode
        SilentMode = ticket.SilentMode;

        //description
        Description = ticket.Description.Trim();

        //timer
        Timer = ticket.IsTimerIntervalNull() ? (int?)null : ticket.TimerInterval;

        //merge ticket
        if (ticket.Merged)
        {
            //hide textbox, display literal
            tbMasterTicketId.Visible = false;
            litMasterticketId.Visible = true;

            MergeMasterTicketId = ticket.MergedMasterTicketId;
        }

        #endregion

        ucTicketClientHistoryGrid.SearchClientId = ticket.FkClient;
        //ucTicketClientHistoryGrid.ResetGrid();

        System.Text.StringBuilder _sbNotifications = new StringBuilder();

        //employee check box list
        List<DesktopShared.Ticket.Notification.Contact> _notificationsEmployee = DesktopShared.Ticket.Notification.GetEmployees(ticket.Pcscdefects);
        ucEmployeeCBL.ClearSelected();
        ucEmployeeCBL.SetSelectedViaTaggedTicket(ticket.Pcscdefects);
        ucEmployeeCBL.SetSelectedViaNotification(_notificationsEmployee);
        if (_notificationsEmployee.Count > 0)
        {
            _sbNotifications.Append("<strong>Employees</strong>");
            foreach (var _contact in _notificationsEmployee)
                _sbNotifications.Append(String.Format("<br />{0}", _contact.FullName));
        }

        //client contact check box list
        bool _rebindClientContactCbl = false;
        List<DesktopShared.Ticket.Notification.Contact> _notificationsClientContact = DesktopShared.Ticket.Notification.GetClientContacts(ticket.Pcscdefects);
        if (_notificationsClientContact.Count > 0)
        {
            if (_sbNotifications.Length > 0)
                _sbNotifications.Append("<br /><br />");

            _sbNotifications.Append("<strong>Client</strong>");
            foreach (var _contact in _notificationsClientContact)
            {
                _sbNotifications.Append(String.Format("<br />{0}", _contact.FullName));

                //check if user/client contact exists. if not, need to create one. this can occur if ticket was updated via email
                /*if (!String.IsNullOrWhiteSpace(_contact.Email))
                {
                    if (DesktopShared.Client.Contact.GetForEmail(_contact.Email, ticket.FkClient).Rows.Count == 0)
                    {
                        //litDebug.Text += _contact.Email.Trim();
                        DesktopShared.Client.Contact.QuickAdd(_contact.Email, ticket.FkClient);
                        _rebindClientContactCbl = true;
                    }
                }*/
            }
        }
        ucClientContactCBL.ClearSelected();
        if (_rebindClientContactCbl)
            ucClientContactCBL.LoadUsers();
        ucClientContactCBL.SetSelectedViaNotification(_notificationsClientContact);

        //email cc for tooltip
        List<string> _autoCcEmails = DesktopShared.Client.Contact.GetTicketAutoCc(ticket.FkClient);
        if (_autoCcEmails.Count > 0)
        {
            if (_sbNotifications.Length > 0)
                _sbNotifications.Append("<br /><br />");

            _sbNotifications.Append("<strong>Auto Email CC</strong>");
            foreach (string _autoCcEmail in _autoCcEmails)
                _sbNotifications.Append(String.Format("<br />{0}", _autoCcEmail));
        }

        //tool tip to display who will be notified
        if (_sbNotifications.Length > 0)
        {
            hlNotifications.Visible = true;
            rtpNotifications.Text = _sbNotifications.ToString();
        }
        else
            hlNotifications.Visible = false;

        //ticket tags
        ucTicketTagsGrid.TicketId = ticket.Pcscdefects;

        if (insertViewHistory)
        {
            //add history note indicating employee has viewed ticket
            DesktopShared.Ticket.History.AddView(ticket.Pcscdefects, DesktopShared.User.UserID, DesktopShared.User.UserFullName.Trim());
        }
    }

    /// <summary>
    /// save ticket values via ticked id
    /// </summary>
    /// <param name="ticketID"></param>
    /// <param name="emailError"></param>
    /// <param name="emailErrorTrackingIds"></param>
    public override void SaveValues(int ticketID, out bool schedulingError, out bool emailError, out List<int> emailErrorTrackingIds)
    {
        DesktopShared.EntityClasses.CscDefectsEntity ticket =
            new DesktopShared.EntityClasses.CscDefectsEntity(ticketID);

        SaveValues(ticket, out schedulingError, out emailError, out emailErrorTrackingIds);
    }

    /// <summary>
    /// save ticket values via ticket entity
    /// </summary>
    /// <param name="ticket"></param>
    public override void SaveValues(DesktopShared.EntityClasses.CscDefectsEntity ticket, out bool schedulingError, out bool emailError, out List<int> emailErrorTrackingIds)
    {
        schedulingError = false;
        emailError = false;
        emailErrorTrackingIds = new List<int>();

        //merge ticket -> if ticket is being merged, ignore all other actions (updates / ts entry etc)
        if (MergeMasterTicketId.HasValue)
        {
            DesktopShared.Ticket.Merge.CompleteMerge(TicketId, MergeMasterTicketId.Value, DesktopShared.User.UserID);
            return;
        }

        string _clientEmails = "";
        string _assignedToClientEmail = "";
        string _employeeEmails = "";
        bool _assignedToChanged = false;
        bool _summaryChanged = false;
        string _oldSummary = "";
        int _ticketHistoryTypeId = DesktopShared.Ticket.History.Type.Id.UpdateByEmployee;
        bool _dipositionChanged = false;
        bool _ticketWasClosed = false;
        string _oldDisposition = "";
        int _originalDispositionId = ticket.FkDisposition.HasValue ? ticket.FkDisposition.Value : -1;

        #region exchange calender update/delete appointment

        bool _addCalendarAppointment = false;
        int _addCalendarAppointmentUserId = 0;

        bool _deleteCalendarAppointment = false;

        bool _updateCalendarAppointment = false;

        if ((ticket.ScheduledDate.HasValue) && (ScheduledDateTime == DateTime.MinValue))
        {
            //was scheduled, now removed -> delete all future appointments for ticket
            _deleteCalendarAppointment = true;
        }
        else if ((!ticket.ScheduledDate.HasValue) && (ScheduledDateTime != DateTime.MinValue))
        {
            //not scheduled originally, now has scheduled -> create appointment for assigned to user
            _addCalendarAppointment = true;
            _addCalendarAppointmentUserId = AssignedTo;
        }
        else if (ticket.ScheduledDate.HasValue)
        {
            if ((ticket.Assignedto.HasValue) && (ticket.Assignedto.Value != AssignedTo))
            {
                //asignment changed -> 

                //1. delete all future appointments for ticket
                _deleteCalendarAppointment = true;

                //2.  create new appointment for new assigned to employee
                _addCalendarAppointment = true;
                _addCalendarAppointmentUserId = AssignedTo;
            }
            else
            {
                if ((ticket.ScheduledDate != ScheduledDateTime) || (DesktopShared.Utility.Time.TimeStringToDouble(ScheduledDuration.Trim()) != ticket.ScheduledDuration))
                {
                    //scheduled date / time has been updated -> update all future appointments for ticket with new scheduled date
                    _updateCalendarAppointment = true;
                }
            }
        }

        #endregion

        #region general

        #region logic to flag ticket as awaiting response

        if ((ticket.Assignedto.Value != AssignedTo) && (AssignedTo != DesktopShared.User.UserID)) //assigning to different user
            ticket.AwaitingResponseUserId = AssignedTo;
        else if (AssignedTo != DesktopShared.User.UserID) //user is making update to ticket assigned to other user
            ticket.AwaitingResponseUserId = AssignedTo;
        else if (DesktopShared.User.UserID == AssignedTo)//user awaiting response is updating ticket
        {
            ticket.AwaitingResponseUserId = null;
            ticket.AwaitingResponseInternal = true;
        }

        #endregion

        //summary
        if (ticket.Summary.Trim() != Summary.Trim())
        {
            _summaryChanged = true;
            _oldSummary = ticket.Summary.Trim();
        }
        ticket.Summary = Summary.Trim();

        
        _assignedToChanged = (ticket.Assignedto != AssignedTo);
        ticket.Assignedto = AssignedTo;

        DateTime? _dateForHistoryNote = null;
        //status / disposition
        if (DispositionId > 0)
        {
            //ticket was changed to active, all other active tickets need to be set to idle
            if ((DispositionId == DesktopShared.SiteHelper.Ticket.Dispostion.Id.Active) && (ticket.FkDisposition != DesktopShared.SiteHelper.Ticket.Dispostion.Id.Active))
                DesktopShared.Ticket.MakeActive(ticket.Pcscdefects, DesktopShared.User.UserID, AssignedTo, false, "", ref _dateForHistoryNote);
            else if ((DispositionId != DesktopShared.SiteHelper.Ticket.Dispostion.Id.Active) && (ticket.FkDisposition == DesktopShared.SiteHelper.Ticket.Dispostion.Id.Active)) //was active, changed to alternate disposition
                DesktopShared.Ticket.MakeInactive(ticket.Pcscdefects, DesktopShared.User.UserID, AssignedTo, AddTimesheet, ref _dateForHistoryNote);

            //check if changed .. history note will be addded if changed
            if ((DispositionId != ticket.FkDisposition) && (ticket.FkDisposition.HasValue))
            {
                _dipositionChanged = true;
                DesktopShared.EntityClasses.CscFieldsEntity objOldDispostion = new DesktopShared.EntityClasses.CscFieldsEntity(ticket.FkDisposition.Value);
                if (objOldDispostion.Fields.State == EntityState.Fetched)
                    _oldDisposition = objOldDispostion.Name.Trim();

                if (DispositionId == DesktopShared.Ticket.Disposition.Id.Closed)
                    _ticketWasClosed = true;
            }

            //if status is active and assignment has changed and user making assignment change is assigning to other user .... set status to idle, run make inactive 
            if ((_assignedToChanged) && (DispositionId == DesktopShared.SiteHelper.Ticket.Dispostion.Id.Active) && (AssignedTo != DesktopShared.User.UserID))
            {
                DesktopShared.Ticket.MakeInactive(ticket.Pcscdefects, DesktopShared.User.UserID, DesktopShared.User.UserID, AddTimesheet, ref _dateForHistoryNote);
                ticket.FkDisposition = DesktopShared.SiteHelper.Ticket.Dispostion.Id.Idle;
            }
            else
                ticket.FkDisposition = DispositionId;
            /*else //if user is updating a ticket with a status of new, switch to idle if they don't manually update the status themselves
            {
                //do not set to idle if "~[user]" is making change ... i.e. "~ Triage Queue" 
                if ((OriginalAssignedToName.Trim().Length > 0) && (OriginalAssignedToName.Substring(0,1) != "~"))
                    ticket.FkDisposition = (DispositionId == DesktopShared.SiteHelper.Ticket.Dispostion.Id.New) ? DesktopShared.SiteHelper.Ticket.Dispostion.Id.Idle : DispositionId;
                else
                    ticket.FkDisposition = DispositionId;
            }*/
        }
        else
            ticket.FkDisposition = null;

        int ticketStatusID = DispositionId == 93 ? 77 : 76;
        ticket.FkStatus = ticketStatusID;

        ticket.TicketTypeId = ddlTicketType.TicketTypeId;
        ticket.TicketCategoryId = ddlTicketCategory.TicketCategoryId;
        ticket.ProductId = ProductId > 0 ? ProductId : (int?)null;

        ticket.HoursToFix = HoursToFix.Trim();

        #region timesheet entry

        if (AddTimesheet)
        {
            DesktopShared.EntityClasses.TimesheetEntity _timeSheet =
                new DesktopShared.EntityClasses.TimesheetEntity();

            #region time spent

            int _intHourSpent = Convert.ToInt32(TimesheetTimeSpent.Substring(0, 2));
            int _intMinuteSpent = Convert.ToInt32(TimesheetTimeSpent.Substring(2));
            string _strMinuteSpent = "";

            switch (_intMinuteSpent)
            {
                case 15:
                    _strMinuteSpent = ".25";
                    break;
                case 30:
                    _strMinuteSpent = ".5";
                    break;
                case 45:
                    _strMinuteSpent = ".75";
                    break;
            }

            double _hours = Convert.ToDouble(_intHourSpent.ToString() + _strMinuteSpent);

            #endregion

            _timeSheet.Date = TimesheetDate;
            _timeSheet.Time = TimesheetStartTime;
            //_timeSheet.IsOvertime = TimesheetIsOvertime;
            _timeSheet.Hrs = _hours;
            _timeSheet.Projtaskid = TimesheetProjtaskId.ToString();
            _timeSheet.FkProjtask = TimesheetProjtaskId;
            _timeSheet.FkClient = TimesheetClientId;
            _timeSheet.Descr1 = TimesheetActivityName;
            _timeSheet.FkTaskaction = TimesheetActivityId;
            _timeSheet.Memo = Server.HtmlEncode(Notes);
            _timeSheet.InternalComments = Server.HtmlEncode(InternalNotes);
            _timeSheet.Workid = DesktopShared.User.GetUserNameOnly();
            _timeSheet.FkEmployee = DesktopShared.User.EmployeeID;
            _timeSheet.FkCscdefects = TicketId;
            _timeSheet.FkProject = TimesheetProjectId;

            #region null value not allowed

            _timeSheet.Clientcode = "";
            _timeSheet.Jobno = "";
            _timeSheet.Taskno = "";
            _timeSheet.Fncode = "";
            _timeSheet.Bilhrs = 0;
            _timeSheet.Location = "";
            _timeSheet.Miles = 0;
            _timeSheet.Tolls = 0;
            _timeSheet.Parking = 0;
            _timeSheet.Masstran = 0;
            _timeSheet.Misc = 0;
            _timeSheet.Bilamt = 0;
            _timeSheet.Commamt = 0;
            _timeSheet.Ssman = "";
            _timeSheet.Workdate = "";
            _timeSheet.Timeid = "";

            #endregion

            string dateTimeString = DesktopShared.Utility.Date.DateToString(DateTime.Now);
            _timeSheet.Created = dateTimeString;
            _timeSheet.LastUpdated = dateTimeString;
            _timeSheet.LastUpdatedby = DesktopShared.User.GetUserNameOnly();
            _timeSheet.Save();

            //check project hours, send email alert if past threshold
            DesktopShared.Timesheet.CheckProjectHours(TimesheetProjtaskId, _timeSheet.Ptimesheet, SelectedClientId);

        }

        #endregion

        //priority
        if (PriorityId > 0)
            ticket.FkPriority = PriorityId;
        else
            ticket.FkPriority = null;

        //scheduled
        ticket.ScheduledDate = ScheduledDateTime != DateTime.MinValue ? ScheduledDateTime : (DateTime?)null;

        //scheduled duration
        if (ScheduledDateTime != DateTime.MinValue)
            ticket.ScheduledDuration = DesktopShared.Utility.Time.TimeStringToDouble(ScheduledDuration.Trim());
        else
            ticket.ScheduledDuration = 0;

        //need completion by
        ticket.NeedCompletionDate = NeedCompletedByDateTime != DateTime.MinValue ? NeedCompletedByDateTime : (DateTime?)null;

        //received date
        ticket.ReceiveDate = ReceivedDateTime;

        #endregion

        #region client

        ticket.FkClient = SelectedClientId;

        int _clientContactId = ClientContactId;

        #region update / add client contact

        string clientContactAction = ddlClientContactAction.SelectedValue;
        if (!String.IsNullOrEmpty(clientContactAction))
        {
            int _userID = 0;
            DesktopShared.EntityClasses.ClientContactEntity _clientContact = null;
            DesktopShared.CollectionClasses.UsersCollection _users = null;
            IPredicateExpression _usersFilter = null;

            if (clientContactAction == "A")//add
            {
                _clientContact = new DesktopShared.EntityClasses.ClientContactEntity();

                _clientContact.Created = DateTime.Now.ToString();
                _clientContact.FkClient = SelectedClientId;

            }
            else if (clientContactAction == "U")//update
            {
                _users = new DesktopShared.CollectionClasses.UsersCollection();

                _usersFilter = new PredicateExpression();
                _usersFilter.Add(DesktopShared.HelperClasses.UsersFields.Pusers == ClientContactId);

                _users.GetMulti(_usersFilter);

                _clientContact = new DesktopShared.EntityClasses.ClientContactEntity(
                    _users[0].FkClientContact.Value);

                _userID = ClientContactId;

            }

            _clientContact.First = DesktopShared.Utility.String.Truncate(ClientContactFirstName, 12);
            _clientContact.Last = DesktopShared.Utility.String.Truncate(ClientContactLastName, 18);
            _clientContact.Email = ClientContactEmail.Trim();
            _clientContact.Busphone = DesktopShared.Utility.String.Truncate(ClientContactPhone, 17);
            _clientContact.Cellphone = DesktopShared.Utility.String.Truncate(txtCellPhone.Text, 17);
            _clientContact.Homephone = DesktopShared.Utility.String.Truncate(txtHomePhone.Text, 17);
            _clientContact.LastUpdated = DesktopShared.Utility.Date.DateToString(DateTime.Now);
            _clientContact.LastUpdatedby = DesktopShared.User.UserName;

            _clientContact.Password = "123456*";
            _clientContact.Save();

            if (_userID < 1)
            {
                _users = new DesktopShared.CollectionClasses.UsersCollection();

                _usersFilter = new PredicateExpression();
                _usersFilter.Add(DesktopShared.HelperClasses.UsersFields.FkClientContact == _clientContact.PclientContact);

                _users.GetMulti(_usersFilter);
                if (_users.Count > 0)
                    _userID = _users[0].Pusers;
            }

            if (_userID > 0)
                _clientContactId = _userID;
        }

        #endregion

        if (_clientContactId > 0)
            ticket.FkUser = _clientContactId;

        #endregion

        #region details

        //designation
        if (!String.IsNullOrEmpty(Designation))
            ticket.Designation = Designation;
        else
            ticket.Designation = null;

        //received method
        if (!String.IsNullOrEmpty(ReceivedMethod))
            ticket.ReceiveMethod = ReceivedMethod;
        else
            ticket.ReceiveMethod = null;

        //is recurring ticket
        ticket.IsRecurring = IsRecurring;

        //auto close
        ticket.AllowAutoClose = AutoClose ? "Y" : "N";

        //archive
        ticket.Archive = Archive;

        //review
        ticket.Review = Review;

        //internal only
        ticket.InternalOnly = InternalOnly;

        //use for reporting
        ticket.UseForReporting = UseForReporting;

        //silent mode
        ticket.SilentMode = SilentMode;

        //timer
        bool _clearTimerFlag = !Timer.HasValue;
        ticket.TimerInterval = Timer;

        #endregion

        #region external & internal notes

        int? _internalTicketHistoryTypeId = null;

        //external notes
        string _notes = "";
        if (Notes.Length > 0)
        {
            _notes = Notes.Trim();

            if (_notes.Length > 8000)
                _notes = _notes.Substring(0, 8000);
            string _parsedNotes = "";
            if (DesktopShared.Ticket.CheckList.AddByText(ticket.Pcscdefects, _notes, ref _parsedNotes, DesktopShared.User.UserID))
                _notes = _parsedNotes;
        }

        //internal notes
        string _internalNotes = "";
        if (InternalNotes.Length > 0)
        {
            _internalNotes = InternalNotes.Trim();
            if (_internalNotes.Length > 8000)
                _internalNotes = _internalNotes.Substring(0, 8000);

        }

        #endregion

        #region employee email

        _employeeEmails = SelectedEmployeeEmails;

        #region assigned to "employee" = helpdesk client user

        if (ucEmployeeAssignedTo.HelpDeskClientId != Desktop.SiteHelper.Client.Id.BBB)
        {
            DesktopShared.EntityClasses.UsersEntity _userEmployee =
                new DesktopShared.EntityClasses.UsersEntity(AssignedTo);

            if (!String.IsNullOrEmpty(_userEmployee.Email))
                _assignedToClientEmail = _userEmployee.Email.Trim();
        }

        #endregion

        #endregion

        #region client email

        if (!InternalOnly)
        {
            //client contacts selected via check box list
            _clientEmails += SelectedClientEmails;

            #region entered by client email

            if (chkNotifyClient.Checked ||
                (DispositionId == DesktopShared.Ticket.Disposition.Id.Closed && chkEnteredByRecieveTicketOnClose.Checked))
            {
                if (!String.IsNullOrEmpty(ClientContactEmail))
                {
                    if (!_clientEmails.Contains(ClientContactEmail.Trim()))
                    {
                        if (_clientEmails.Length > 0)
                            _clientEmails += "; ";

                        _clientEmails += ClientContactEmail.Trim();
                    }
                }
            }

            #endregion

            #region assigned to client email

            if (_assignedToClientEmail.Trim().Length > 0)
            {
                if (!_clientEmails.Contains(_assignedToClientEmail.Trim()))
                {
                    if (_clientEmails.Length > 0)
                        _clientEmails += "; ";

                    _clientEmails += _assignedToClientEmail.Trim();
                }
            }

            #endregion
        }
        else // regardless of selected email addresses, don't send to client as ticket is internal only
            _clientEmails = "";

        if (!chkNotifyClient.Checked) //if notify client not checked, no client emails
            _clientEmails = "";

        #endregion

        //assigned to text 
        bool _assignedToOnlyUpdate = false;
        string _assignedToText = "";
        if (_assignedToChanged)
        {
            if (ScheduledDateTime == DateTime.MinValue)
                _assignedToText = String.Format("Assigned to {0} by {1}.", ucEmployeeAssignedTo.EmployeeName.Trim(), DesktopShared.User.UserFullName.Trim());
            else
            {
                _assignedToText = String.Format("Scheduled for {0} on {1} by {2}.", ucEmployeeAssignedTo.EmployeeName.Trim(), ScheduledDateTime.ToString(), DesktopShared.User.UserFullName.Trim());
                if (!_addCalendarAppointment)
                {
                    StringBuilder _sbAlertMessage = new StringBuilder();
                    _sbAlertMessage.Append("Scheduled for text added to ticket history but Add Calendar Appointment logic is false");
                    _sbAlertMessage.Append(String.Format("<br />Ticket Id:{0}", ticket.Pcscdefects));
                    _sbAlertMessage.Append(String.Format("<br />ticket.ScheduledDate.HasValue:{0}", ticket.ScheduledDate.HasValue));
                    _sbAlertMessage.Append(String.Format("<br />(ScheduledDateTime == DateTime.MinValue):{0}", (ScheduledDateTime == DateTime.MinValue).ToString()));
                    _sbAlertMessage.Append(String.Format("<br />(ticket.Assignedto.HasValue):{0}", (ticket.Assignedto.HasValue).ToString()));
                    _sbAlertMessage.Append(String.Format("<br />(ticket.Assignedto.Value != AssignedTo):{0}", (ticket.Assignedto.Value != AssignedTo).ToString()));

                    DesktopShared.Exchange.SendEmailAlert("TicketEditUserControl", _sbAlertMessage.ToString());
                }
            }

            if ((_notes.Length == 0) && (_internalNotes.Trim().Length == 0))
                _assignedToOnlyUpdate = true;
        }

        //save ticket
        ticket.Lastupdated = DateTime.Now;
        ticket.Save();

        #region add history to ticket and send emails

        bool _externalMailSuccess = true;
        int? _externalMessageTrackingId = null;
        bool _internalMailSuccess = true;
        int? _internalMessageTrackingId = null;

        List<int> _filesToEmail = ucTicketFileGrid.FileIdsToEmail;
        int _mainTicketHistoryId = -1;

        if (!_assignedToOnlyUpdate)
        { 
            DesktopShared.Ticket.History.Add(
                TicketId, //ticket id
                DesktopShared.User.UserID, //user id, 
                _notes.Trim(), //notes
                _internalNotes.Trim(), //internal notes
                false, // is new ticket
                _clientEmails, // client email addresses
                _employeeEmails, // employee email addresses
                !SilentMode,  // send email out
                _ticketHistoryTypeId,  // history type id
                _dateForHistoryNote,  //date/time stamp to user for history note created
                _filesToEmail, //ids for file to attach to email
                "", //update by override
                _internalTicketHistoryTypeId, //internal ticket history type id (override history type id)
                ref _mainTicketHistoryId, //entity id
                ref _externalMailSuccess, //email successfully sent to client
                ref _externalMessageTrackingId, //message tracking id for email to client
                ref _internalMailSuccess, //email successfully sent to employees
                ref _internalMessageTrackingId //message tracking id for email to employee
                );
        }
        #endregion

        #region add history if summary has been changed 

        if (_summaryChanged)
        {
            _notes = String.Format("SUMMARY WAS: {0}{2}NEW SUMMARY: {1}", _oldSummary.Trim(), Summary.Trim(), Environment.NewLine);

            DesktopShared.Ticket.History.Add(
            TicketId, //ticket id
            DesktopShared.User.UserID, //user id, 
            _notes.Trim(), //notes
            "", //internal notes
            false, // is new ticket
            "", // client email addresses
            "", // employee email addresses
            false, // send email out
            DesktopShared.Ticket.History.Type.Id.SummaryChange, //historty type
            _dateForHistoryNote //date/time stamp to user for history note created
            );
        }

        #endregion

        #region add history if disposition has been changed

        if (_dipositionChanged)
        {
            _notes = String.Format("STATUS WAS: {0}{2}NEW STATUS: {1}", _oldDisposition.Trim(), ucTicketDisposition.DispositionName.Trim(), Environment.NewLine);
            int _statusChangeHistoryId = -1;

            DesktopShared.Ticket.History.Add(
            ticket.Pcscdefects, //ticket id
            DesktopShared.User.UserID, //user id, 
            _notes.Trim(), //notes
            "", //internal notes
            false, // is new ticket
            "", // client email addresses
            "", // employee email addresses
            false, // send email out
            DesktopShared.Ticket.History.Type.Id.DispositionChange, //history type
            _dateForHistoryNote, //date/time stamp to user for history note created
            null, //ids for file to attach to email
            "", //update by override
            null, //internal ticket history type id (override history type id)
            ref _statusChangeHistoryId //entity id
            );

            if (_mainTicketHistoryId == -1)
                _mainTicketHistoryId = _statusChangeHistoryId;
        }

        #endregion

        #region assignment changed updated

        if (_assignedToChanged)
        {
            //only send email if assigned to is not user making update
            string _assignedToEmails = "";
            if (DesktopShared.User.UserID != AssignedTo)
            {
                DesktopShared.EntityClasses.UsersEntity _userEmployee = new DesktopShared.EntityClasses.UsersEntity(AssignedTo);
                if (!String.IsNullOrEmpty(_userEmployee.Email))
                    _assignedToEmails = _userEmployee.Email.Trim();
            }

            DesktopShared.Ticket.History.Add(
                ticket.Pcscdefects, //ticket id
                DesktopShared.User.UserID, //user id, 
                "", //notes
                _assignedToText, //internal notes
                false, // is new ticket
                "", // client email addresses
                _assignedToEmails, // employee email addresses
                true,  // send email out
                null,  // history type id
                _dateForHistoryNote,  //date/time stamp to user for history note created
                null, //ids for file to attach to email
                "", //update by override
                DesktopShared.Ticket.History.Type.Id.AssignmentChange, //internal ticket history type id (override history type id)
                ref _mainTicketHistoryId, //entity id
                true //is responsive page
           );
        }

        #endregion

        #region calendar

        //delete future appointments
        if (_deleteCalendarAppointment)
            DesktopShared.Exchange.Calendar.DeleteAllFuture(TicketId, DesktopShared.User.UserID, ScheduledSendInvite);

        //add calendar appointment for specified user id    
        if (_addCalendarAppointment)
            schedulingError = !DesktopShared.Exchange.Calendar.CreateAppointment(TicketId, _addCalendarAppointmentUserId, ScheduledSendInvite, DesktopShared.User.UserID);
       
        //edit future appointments
        if (_updateCalendarAppointment)
            DesktopShared.Exchange.Calendar.UpdateAllFuture(TicketId, DesktopShared.User.UserID, ScheduledSendInvite);

        #endregion

        //slack notification
        //if ((_assignedToChanged) && (ticket.Assignedto.HasValue) && (ticket.Assignedto.Value != DesktopShared.User.UserID))
            //DesktopShared.Ticket.Slack.SendMessage(ticket.Pcscdefects, DesktopShared.User.UserID);

        //3b exam specific ticket -> update 3b exam via api
        if (SelectedClientId == DesktopShared.BbbExam.ClientId)
        {
            if (!_assignedToOnlyUpdate)
            {
                string _errorMessage = "";
                DesktopShared.BbbExam.Api.Support.Detail.Create(DesktopShared.Ticket.History.GetTypedListRow(_mainTicketHistoryId), _originalDispositionId, DispositionId, ref _errorMessage);
            }
        }

        //delete ticket timer tag if assigned to user is updated ticket or timer was reset to null
        if (!_clearTimerFlag)
        {
            if (ticket.TimerInterval.HasValue && (DesktopShared.User.UserID == AssignedTo))
            {
                if (!String.IsNullOrWhiteSpace(InternalNotes) || !String.IsNullOrWhiteSpace(Notes))
                    _clearTimerFlag = true;
            }
        }
        if (_clearTimerFlag)
            DesktopShared.Ticket.Tagged.DeleteTagValue(TicketId, DesktopShared.Ticket.Tagged.TicketTimerTagText, null);

        //checklist tasks
        if (phCheckList.Visible)
        {
            System.Text.StringBuilder sbCheckListItemsCompleted = new StringBuilder();
            List<int> _updatedIds = new List<int>();

            foreach (RepeaterItem _repeaterItem in rptCheckList.Items)
            {
                CheckBox chkCheckListCompleted = (CheckBox)_repeaterItem.FindControl("chkCheckListCompleted");
                if (chkCheckListCompleted.Enabled && chkCheckListCompleted.Checked)
                {
                    Literal litCheckListId = (Literal)_repeaterItem.FindControl("litCheckListId");
                    int _ticketCheckListId = -1;
                    if (int.TryParse(litCheckListId.Text.Trim(), out _ticketCheckListId))
                    {
                        _updatedIds.Add(_ticketCheckListId);
                        Literal litCheckListName = (Literal)_repeaterItem.FindControl("litCheckListName");
                        if (sbCheckListItemsCompleted.Length > 0)
                            sbCheckListItemsCompleted.Append(", ");
                        sbCheckListItemsCompleted.Append(litCheckListName.Text.Trim());
                    }
                }
            }

            if (_updatedIds.Count > 0)
            {
                DesktopShared.Ticket.CheckList.SetAsCompleted(_updatedIds, DesktopShared.User.UserID);

                DesktopShared.Ticket.History.Add(
                   ticket.Pcscdefects, //ticket id
                   DesktopShared.User.UserID, //user id, 
                   String.Format("The following Check List items were completed: {0}", sbCheckListItemsCompleted.ToString()), //notes
                   "",//internal notes
                   false, // is new ticket
                   "", // client email addresses
                   "", // employee email addresses
                   false, // send email out
                   DesktopShared.Ticket.History.Type.Id.UpdateByEmployee, //history type
                   _dateForHistoryNote //date/time stamp to user for history note created
              );
            }
        }

        //if ticket closed, user has mananger, and checklist items open -> send alert to manager
        if (_ticketWasClosed)
        {
            int? _managerId = DesktopShared.User.ManagerId;
            if (_managerId.HasValue)
            {
                if (DesktopShared.Ticket.CheckList.HasOpenItems(ticket.Pcscdefects))
                    DesktopShared.Email.Ticket.SendClosedWithOpenChecklist(ticket.Pcscdefects, DesktopShared.User.UserID, _managerId.Value);
            }
        }

        //update sticky notifications for employee / client contact
        ucEmployeeCBL.UpdateTicketNotification(ticket.Pcscdefects);
        ucClientContactCBL.UpdateTicketNotification(ticket.Pcscdefects);

        emailError = !_externalMailSuccess || !_internalMailSuccess;
        if (emailError)
        {
            if (_externalMessageTrackingId.HasValue)
                emailErrorTrackingIds.Add(_externalMessageTrackingId.Value);
            if (_internalMessageTrackingId.HasValue)
                emailErrorTrackingIds.Add(_internalMessageTrackingId.Value);
        }
    }

    #endregion

    #region public properties

    /// <summary>
    /// get/set ticket id
    /// </summary>
    public int TicketId
    {
        get
        {
            object obj = this.ViewState["TicketIdForEdit"];
            if (obj == null)
                return -1;
            else
                return (int)obj;
        }

        set
        {
            this.ViewState["TicketIdForEdit"] = value;

            litTicketNumber.Text = value.ToString();

            hlTicketNumber.Text = value.ToString();
            hlTicketNumber.NavigateUrl = "/Ticket/Detail2.aspx?Id=" + value.ToString();

            //history grid  
            ucTicketHistoryGrid.TicketId = value;

            //file grid
            ucTicketFileGrid.ShowUploadLink = true;
            ucTicketFileGrid.TicketId = value;
        }
    }

    #region general

    /// <summary>
    /// set reported by name
    /// </summary>
    public string ReportedByName
    {
        set { litReportedByName.Text = value; }
    }

    /// <summary>
    /// set reported date
    /// </summary>
    public DateTime ReportedDate
    {
        set { litReportedDate.Text = value.ToString("MM/dd/yy HH:mm:ss"); }
    }

    /// <summary>
    /// get/set summary
    /// </summary>
    public string Summary
    {
        get { return tbSummary.Text; }
        set { tbSummary.Text = value; }
    }

    /// <summary>
    /// get/set assigned to
    /// </summary>
    public int AssignedTo
    {
        get { return ucEmployeeAssignedTo.EmployeeId; }
        set { ucEmployeeAssignedTo.EmployeeId = value; }
    }

    /// <summary>
    /// get/set original assigned to anme
    /// </summary>
    public string OriginalAssignedToName
    {
        get
        {
            object obj = this.ViewState["OriginalAssignedToNameForEdit"];
            return (obj == null) ? "" : (string)obj;
        }
        set { this.ViewState["OriginalAssignedToNameForEdit"] = value; }
    }

    /// <summary>
    /// get/set disposition id
    /// </summary>
    public int DispositionId
    {
        get { return ucTicketDisposition.DispositionId; }
        set { ucTicketDisposition.DispositionId = value; }
    }

    /// <summary>
    /// get/set product id
    /// </summary>
    public int? ProductId
    {
        get { return ucTicketProduct.ProductId; }
        set
        {
            if (value.HasValue)
                ucTicketProduct.ProductId = value.Value;
        }
    }

    /// <summary>
    /// get set tagged ticket id (stored in viewstate)
    /// </summary>
    public int TaggedTicketId
    {
        get
        {
            object obj = this.ViewState["TaggedTicketIdForTicketEdit"];
            if (obj == null)
                return -1;
            else
                return (int)obj;
        }
        set
        {
            this.ViewState["TaggedTicketIdForTicketEdit"] = value;
        }
    }

    /// <summary>
    /// get/set hours to fix
    /// </summary>
    public string HoursToFix
    {
        get { return txtHoursToFix.Text; }
        set { txtHoursToFix.Text = value; }
    }

    /// <summary>
    /// get/set notes
    /// </summary>
    public string Notes
    {
        get { return tbNotes.Text; }
        set { tbNotes.Text = value; }
    }

    /// <summary>
    /// get/set internal notes
    /// </summary>
    public string InternalNotes
    {
        get { return tbInternalNotes.Text; }
        set { tbInternalNotes.Text = value; }
    }

    #region timesheet

    /// <summary>
    /// get/set add timesheet
    /// </summary>
    public bool AddTimesheet
    {
        get { return chkAddTimesheet.Checked; }
        set { chkAddTimesheet.Checked = value; }
    }

    /// <summary>
    /// get/set timesheet date
    /// </summary>
    public DateTime TimesheetDate
    {
        get
        {
            if (rdpTimesheet.SelectedDate.HasValue)
                return rdpTimesheet.SelectedDate.Value;
            else
                return DateTime.MinValue;
        }
        set { rdpTimesheet.SelectedDate = value; }
    }

    /// <summary>
    /// get/set timesheet start time
    /// </summary>
    public string TimesheetStartTime
    {
        get
        {
            return ucTimesheetStartDDL.Hour.ToString().PadLeft(2, '0') +
                ucTimesheetStartDDL.Minute.ToString().PadLeft(2, '0');
        }
        set
        {
            ucTimesheetStartDDL.Hour = Convert.ToInt32(value.Substring(0, 2));
            ucTimesheetStartDDL.Minute = Convert.ToInt32(value.Substring(2, 2));
        }
    }

    /// <summary>
    /// get/set timesheet time spent
    /// </summary>
    public string TimesheetTimeSpent
    {
        get
        {
            return ucTimesheetSpentDDL.Hour.ToString().PadLeft(2, '0') +
                ucTimesheetSpentDDL.Minute.ToString().PadLeft(2, '0');
        }
        set
        {
            ucTimesheetSpentDDL.Hour = Convert.ToInt32(value.Substring(0, 2));
            ucTimesheetSpentDDL.Minute = Convert.ToInt32(value.Substring(2, 2));
        }
    }

    /// <summary>
    /// get/set timesheet is over time
    /// </summary>
    public bool TimesheetIsOvertime
    {
        get { return chkTimesheetIsOvertime.Checked; }
        set { chkTimesheetIsOvertime.Checked = value; }
    }

    /// <summary>
    /// get timesheet client id
    /// </summary>
    public int TimesheetClientId
    {
        get { return ucTimesheetProject.SelectedClientId; }
    }

    /// <summary>
    /// get timesheet project task id
    /// </summary>
    public int TimesheetProjtaskId
    {
        get { return ucTimesheetProject.SelectedProjtaskId; }
    }

    /// <summary>
    /// get timesheet project id
    /// </summary>
    public int TimesheetProjectId
    {
        get { return ucTimesheetProject.SelectedProjectId; }
    }

    /// <summary>
    /// get/set timesheet activity id
    /// </summary>
    public int TimesheetActivityId
    {
        get { return ucTimesheetActivity.TaskActionId; }
        set { ucTimesheetActivity.TaskActionId = value; }
    }

    /// <summary>
    /// get timesheet activity name
    /// </summary>
    public string TimesheetActivityName
    {
        get { return ucTimesheetActivity.TaskActionName; }
    }

    #endregion

    /// <summary>
    /// get/set priority id
    /// </summary>
    public int PriorityId
    {
        get { return ucTicketPriority.PriorityId; }
        set { ucTicketPriority.PriorityId = value; }
    }

    /// <summary>
    /// get/set scheduled date & time
    /// </summary>
    public DateTime ScheduledDateTime
    {
        get { return ucDateTimeScheduled.SelctedDateTime; }
        set { ucDateTimeScheduled.SelctedDateTime = value; }
    }

    /// <summary>
    /// get/set scheduled duration
    /// </summary>
    public string ScheduledDuration
    {
        get { return ucTimeDDLScheduledDuration.Hour.ToString().PadLeft(2, '0') + ucTimeDDLScheduledDuration.Minute.ToString().PadLeft(2, '0'); }
        set
        {
            ucTimeDDLScheduledDuration.Hour = Convert.ToInt32(value.Substring(0, 2));
            ucTimeDDLScheduledDuration.Minute = Convert.ToInt32(value.Substring(2, 2));
        }
    }

    /// <summary>
    /// get/set send invite for scheduled calendar item
    /// </summary>
    public bool ScheduledSendInvite
    {
        get { return chkScheduledSendInvite.Checked; }
        set { chkScheduledSendInvite.Checked = value; }
    }

    /// <summary>
    /// get/set need completed by date & time
    /// </summary>
    public DateTime NeedCompletedByDateTime
    {
        get { return ucDateTimeNeedCompletedBy.SelctedDateTime; }
        set { ucDateTimeNeedCompletedBy.SelctedDateTime = value; }
    }

    /// <summary>
    /// get/set received date & time
    /// </summary>
    public DateTime ReceivedDateTime
    {
        get { return ucDateTimeReceived.SelctedDateTime; }
        set { ucDateTimeReceived.SelctedDateTime = value; }
    }

    #endregion

    #region client

    /// <summary>
    /// get/set selected client id
    /// </summary>
    public int SelectedClientId
    {
        get { return ucClient.SelectedClientId; }
        set
        {
            ucClient.SelectedClientId = value;
            ucClientContactCBL.SelectedClientId = value; // client email check box list
        }
    }

    /// <summary>
    /// get/set client contact id
    /// </summary>
    public int ClientContactId
    {
        get { return ucClientContact.ClientContactId; }
        set { ucClientContact.ClientContactId = value; }
    }

    #endregion

    #region general AND client

    /// <summary>
    /// get/set client contact first name
    /// </summary>
    public string ClientContactFirstName
    {
        get { return tbClientContactFirstName.Text; }
        set
        {
            tbClientContactFirstName.Text = value;
            litContactFirst.Text = value;
        }
    }

    /// <summary>
    /// get/set client contact last name
    /// </summary>
    public string ClientContactLastName
    {
        get { return tbClientContactLastName.Text; }
        set
        {
            tbClientContactLastName.Text = value;
            litContactLast.Text = value;
        }
    }

    /// <summary>
    /// get/set client contact phone
    /// </summary>
    public string ClientContactPhone
    {
        get { return tbClientContactPhone.Text; }
        set
        {
            tbClientContactPhone.Text = value;
            string spacer = "";

            if (!String.IsNullOrEmpty(value))
            {
                if (!String.IsNullOrEmpty(litContactFirst.Text.Trim()) ||
                    !String.IsNullOrEmpty(litContactLast.Text.Trim())
                    )
                    spacer = " | ";

                litContactPhone.Text = spacer + "w: " + value;
            }
        }
    }

    /// <summary>
    /// get/set client contact cell phone
    /// </summary>
    public string ClientContactCellPhone
    {
        get { return txtCellPhone.Text; }
        set
        {
            txtCellPhone.Text = value;
            string spacer = "";

            if (!String.IsNullOrEmpty(value))
            {
                if (!String.IsNullOrEmpty(litContactFirst.Text.Trim()) ||
                    !String.IsNullOrEmpty(litContactLast.Text.Trim()) ||
                    !String.IsNullOrEmpty(litContactPhone.Text.Trim())
                    )
                    spacer = " | ";

                litContactCellPhone.Text = spacer + "c: " + value;
            }
        }
    }

    /// <summary>
    /// get/set client contact home phone
    /// </summary>
    public string ClientContactHomePhone
    {
        get { return txtHomePhone.Text; }
        set
        {
            txtHomePhone.Text = value;
            string spacer = "";

            if (!String.IsNullOrEmpty(value))
            {
                if (!String.IsNullOrEmpty(litContactFirst.Text.Trim()) ||
                    !String.IsNullOrEmpty(litContactLast.Text.Trim()) ||
                    !String.IsNullOrEmpty(litContactPhone.Text.Trim()) ||
                    !String.IsNullOrEmpty(litContactCellPhone.Text.Trim())
                    )
                    spacer = " | ";

                litContactHomePhone.Text = spacer + "h: " + value;
            }
        }
    }

    /// <summary>
    /// get/set client contact email
    /// </summary>
    public string ClientContactEmail
    {
        get { return tbClientContactEmail.Text; }
        set
        {
            tbClientContactEmail.Text = value;
            if (!String.IsNullOrEmpty(value))
            {
                string mailTo = "<a href=\"mailto:" + value.Trim() + "\">" + value + "</a>";
                string spacer = "";

                if (!String.IsNullOrEmpty(litContactFirst.Text.Trim()) ||
                    !String.IsNullOrEmpty(litContactLast.Text.Trim()))
                    spacer = " | ";
                litContactEmail.Text = spacer + "email: " + mailTo;
            }
        }
    }

    #endregion

    #region details

    /// <summary>
    /// get/set designation
    /// </summary>
    public string Designation
    {
        get { return ucTicketDesignation.Designation; }
        set { ucTicketDesignation.Designation = value; }
    }


    /// <summary>
    /// get/set received method
    /// </summary>
    public string ReceivedMethod
    {
        get { return ucTicketReceivedMethod.ReceivedMethod; }
        set { ucTicketReceivedMethod.ReceivedMethod = value; }
    }

    /// <summary>
    /// get/set is recurring ticket
    /// </summary>
    public bool IsRecurring
    {
        get { return chkIsRecurring.Checked; }
        set { chkIsRecurring.Checked = value; }
    }

    /// <summary>
    /// get/set auto close
    /// </summary>
    public bool AutoClose
    {
        get { return chkAutoClose.Checked; }
        set { chkAutoClose.Checked = value; }
    }

    /// <summary>
    /// get/set archive
    /// </summary>
    public bool Archive
    {
        get { return chkArchive.Checked; }
        set { chkArchive.Checked = value; }
    }

    /// <summary>
    /// get/set review
    /// </summary>
    public bool Review
    {
        get { return chkReview.Checked; }
        set { chkReview.Checked = value; }
    }

    /// <summary>
    /// get/set internal only
    /// </summary>
    public bool InternalOnly
    {
        get { return chkInternalOnly.Checked; }
        set { chkInternalOnly.Checked = value; }
    }

    /// <summary>
    /// get/set use for reporting
    /// </summary>
    public bool UseForReporting
    {
        get { return chkUseForReporting.Checked; }
        set { chkUseForReporting.Checked = value; }
    }

    /// <summary>
    /// get/set  silent mode (true = no email alerts sent out)
    /// </summary>
    public bool SilentMode
    {
        get { return chkSilentMode.Checked; }
        set { chkSilentMode.Checked = value; }
    }

    /// <summary>
    /// set description
    /// </summary>
    public string Description
    {
        //get { return tbDescription.Text; }
        set
        {
            lblDescription.Text = value.Replace("\n", "<br />");
            //tbDescription.Text = value.Replace("\n", "<br />");
        }
    }

    /// <summary>
    /// get/set timer
    /// </summary>
    public int? Timer
    {
        get { return ddlTimer.SelectedValue == "" ? (int?)null : Convert.ToInt32(ddlTimer.SelectedValue); }
        set
        {
            if (value.HasValue)
            {
                ListItem _li = ddlTimer.Items.FindByValue(value.Value.ToString());
                if (_li != null)
                    _li.Selected = true;
            }
        }
    }

    /// <summary>
    /// get/set merge master ticket id
    /// </summary>
    public int? MergeMasterTicketId
    {
        get
        {
            if (tbMasterTicketId.Text.Trim().Length > 0)
                return Convert.ToInt32(tbMasterTicketId.Text);
            else
                return null;
        
        }
        set { litMasterticketId.Text = value.Value.ToString(); }
    }


    #endregion

    #region client email

    /// <summary>
    /// get selected client email addresses
    /// </summary>
    public string SelectedClientEmails
    {
        get { return ucClientContactCBL.GetSelectedEmailAddresses(); }
    }

    #endregion

    #region employee email

    /// <summary>
    /// get selected employee emails
    /// </summary>
    public string SelectedEmployeeEmails
    {
        get { return ucEmployeeCBL.GetSelectedEmailAddresses(); }

    }

    #endregion

    #region misc

    /// <summary>
    /// set edit mode for control
    /// </summary>
    public EditMode FormEditMode
    {
        set { _editMode = value; }
    }

    #endregion

    #endregion

    #region public enum

    /// <summary>
    /// enum for edit mode
    /// </summary>
    public enum EditMode
    {
        Grid,
        Page
    }

    #endregion

    #region public events

    /// <summary>
    /// update button event handler click
    /// </summary>
    public event EventHandler evButtonClick;

    #endregion

    #region custom validators

    /// <summary>
    /// validate client contact email if update/add action selected
    /// </summary>
    /// <param name="source"></param>
    /// <param name="args"></param>
    protected void cvClientContactEmail_ServerValidate(object source, ServerValidateEventArgs args)
    {
        args.IsValid = true;
        string action = ddlClientContactAction.SelectedValue;
        if (!String.IsNullOrEmpty(action))
        {
            int? _userId = null;
            if (action == "U")
                _userId = ClientContactId;

            if (!DesktopShared.Utility.IsValidEmailAddress(ClientContactEmail))
            {
                cvClientContactEmail.ErrorMessage = "Invalid Client Contact Email Address";
                args.IsValid = false;
                return;
            }

            if (!DesktopShared.User.IsUniqueEmail(ClientContactEmail, _userId))
            {
                cvClientContactEmail.ErrorMessage = "Client Contact Email Address is already in use";
                args.IsValid = false;
                return;
            }
        }
    }

    /// <summary>
    /// validate time sheet entry
    /// </summary>
    /// <param name="source"></param>
    /// <param name="args"></param>
    protected void cvTimesheet_ServerValidate(object source, ServerValidateEventArgs args)
    {
        if (AddTimesheet)
        {
            StringBuilder sbTimesheetError = new StringBuilder();

            if (TimesheetDate == DateTime.MinValue)
                sbTimesheetError.Append("Date");

            if (TimesheetTimeSpent == "0000")
            {
                if (sbTimesheetError.Length > 0)
                    sbTimesheetError.Append(", ");

                sbTimesheetError.Append("Time Spent");
            }

            if (TimesheetClientId < 1)
            {
                if (sbTimesheetError.Length > 0)
                    sbTimesheetError.Append(", ");

                sbTimesheetError.Append("Project");
            }

            if (TimesheetActivityId < 1)
            {
                if (sbTimesheetError.Length > 0)
                    sbTimesheetError.Append(", ");

                sbTimesheetError.Append("Activity");
            }

            #region validate date for timesheet does not exceed threshold
            //allow even if they fail validation ... just send email 
            
            //timesheet admin do not have to obey this rule
            //if (!DesktopShared.User.IsTimesheetAdmin)
            //{
            //check date entered is not greater than max days allowed
            int maxDays = Desktop.SiteHelper.Timesheet.MaxDaysForAdd;
            //string addOrEdit = "add";

            DateTime minDate = BitByBit.Utility.Date.StartOfDayDate(DateTime.Now.AddDays(-maxDays));
            if (TimesheetDate < minDate)
            {
                //email alert 
                Desktop.Email.Timesheet.SendWarning(
                    DesktopShared.User.UserFullName.Trim(), //employee name making add / edit
                    (int?)null, //ticket if if available (for edit)
                    TimesheetDate //date employee tried to enter
                    );

                //error message
                //sbTimesheetError.Append("You may not " + addOrEdit + " timesheet entries entered "
                    //+ maxDays.ToString() + " days earlier than today.");
            }
            //}
            

            #endregion

            string errorMessage = "";
            if (sbTimesheetError.Length > 0)
            {
                errorMessage = "The following Timesheet fields are required: ";
                errorMessage += sbTimesheetError.ToString();
            }

            cvTimesheet.ErrorMessage = errorMessage;
            args.IsValid = sbTimesheetError.Length == 0;

            return;
        }

        args.IsValid = true;

    }

    /// <summary>
    /// validate scheduled duration is selected if scheduled date is entered
    /// </summary>
    /// <param name="source"></param>
    /// <param name="args"></param>
    protected void cvScheduledDuration_ServerValidate(object source, ServerValidateEventArgs args)
    {
        args.IsValid = (ScheduledDateTime == DateTime.MinValue) || (ScheduledDuration != "0000");
    }

    /// <summary>
    /// validate merge master ticket is valid
    /// </summary>
    /// <param name="source"></param>
    /// <param name="args"></param>
    protected void cvMasterTicketId_ServerValidate(object source, ServerValidateEventArgs args)
    {
        if (!MergeMasterTicketId.HasValue)
        {
            args.IsValid = true;
            return;
        }

        string errorMessage = "";
        bool isValid = DesktopShared.Ticket.Merge.IsValid(TicketId, MergeMasterTicketId.Value, ref errorMessage);

        cvMasterTicketId.ErrorMessage = "Merge Master Ticket Error: " + errorMessage;
        args.IsValid = isValid;
    }


    #endregion

    #region client ticket history

    /*<asp:PlaceHolder ID="phClientHistory" runat="server">
     <ajaxToolkit:CollapsiblePanelExtender ID="cpeClientHistory" 
        runat="Server"
        TargetControlID="pnlClientHistory"
        ExpandControlID="pnlShowHideClientHistory"
        CollapseControlID="pnlShowHideClientHistory" 
        Collapsed="True"
        BehaviorID="cpeBehaviorClientHistory"
        TextLabelID="lblClientHistoryHeader"
        ImageControlID="imgClientHistoryHeader"    
        ExpandedText="CLIENT PREVIOUS TICKETS (click to hide)"
        CollapsedText="CLIENT PREVIOUS TICKETS (click to show)"
        ExpandedImage="~/images/collapse_blue.jpg"
        CollapsedImage="~/Images/expand_blue.jpg"
        SuppressPostBack="true"
    />

    <asp:Panel ID="pnlShowHideClientHistory" runat="server" CssClass="collapsePanelHeader" Height="25px">
        <div style="cursor: pointer; vertical-align: middle; padding:5px;">
            <div style="float: left;">
                <asp:Label ID="lblClientHistoryHeader" runat="server">Show Client Previous Tickets</asp:Label>
            </div>
            <div style="float: right; vertical-align: middle;">
                <asp:ImageButton ID="imgClientHistoryHeader" runat="server" ImageUrl="~/images/expand_blue.jpg" 
                    AlternateText="CLIENT PREVIOUS TICKETS (click to show)"/>
            </div>
        </div>
    </asp:Panel>

    <asp:Panel ID="pnlClientHistory" runat="server">

        <table border="0" class="gridEditForm">
            <tr>
                <td style="width:100%">
                    <uc:TicketGrid ID="ucTicketClientHistoryGrid" runat="server" Type="ClientHistory" />
                </td>
            </tr>
        </table>
    </asp:Panel>
    </asp:PlaceHolder>*/
    #endregion

}
