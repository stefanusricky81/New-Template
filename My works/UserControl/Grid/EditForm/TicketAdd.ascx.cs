using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using SD.LLBLGen.Pro.ORMSupportClasses;
using System.Text;

public partial class UserControl_Grid_EditForm_TicketAdd : AbstractTicketAdd
{
    private AddMode _addMode = AddMode.Grid;
    private string _defaultTextAddTag = "--select--";

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

        //collapse all other panels for grid add in place
        bool collapsePanel = (_addMode == AddMode.Grid);

        cpeDetails.Collapsed = collapsePanel;
        cpeEmployeeEmail.Collapsed = collapsePanel;
        cpeClientEmail.Collapsed = collapsePanel;
    }

    /// <summary>
    /// set up user control
    /// </summary>
    private void SetUpControl()
    {
        SetUpCollapsiblePanels();

        if (_addMode == AddMode.Grid)
        {
            mvTopButtons.SetActiveView(viewTopGrid);
            mvBottomButtons.SetActiveView(viewBottomGrid);
        }
        else
        {
            mvTopButtons.SetActiveView(viewTopPage);
            mvBottomButtons.SetActiveView(viewBottomPage);

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

        chkEnteredByRecieveTicket.InputAttributes.Add("class", "checkbox");
        chkAddTimesheet.InputAttributes.Add("class", "checkbox");
        chkScheduledSendInvite.InputAttributes.Add("class", "checkbox");
        chkTag.InputAttributes.Add("class", "checkbox");
        chkTagPublic.InputAttributes.Add("class", "checkbox");
        chkTagClientPublic.InputAttributes.Add("class", "checkbox");
        chkSilentMode.InputAttributes.Add("class", "checkbox");

        #endregion

    }

    /// <summary>
    /// set priortity selection based on client / client user selection
    /// </summary>
    private void SetPrioritySelection()
    {
        DropDownList _ddl = ucTicketPriority.GetDropDownList();
        if (!ClientPriorityValue.HasValue && !ClientUserPriorityValue.HasValue)
        {
            _ddl.ClearSelection();
            return;
        }

        int _clientVal = ClientPriorityValue.HasValue ? ClientPriorityValue.Value : 100;
        int _clientUserVal = ClientUserPriorityValue.HasValue ? ClientUserPriorityValue.Value : 100;
        int _finalVal = (_clientVal <= _clientUserVal) ? _clientVal : _clientUserVal;

        ListItem _li = _ddl.Items.Cast<ListItem>().FirstOrDefault(item => item.Text.Trim().Contains(_finalVal.ToString().Trim()));
        if (_li != null)
        {
            _ddl.ClearSelection();
            _li.Selected = true;
        }
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
        /*DesktopShared.CollectionClasses.TicketCheckListDetailCollection _details = DesktopShared.Ticket.CheckList.Detail.GetActive(ddlTicketCheckListMaster.TicketCheckListMasterId.Value);
        if (_details.Count > 0)
        {
            System.Text.StringBuilder _sb = new StringBuilder();
            if (!String.IsNullOrWhiteSpace(tbNotes.Text.Trim()))
                _sb.Append(String.Format("{0}\n\n", tbNotes.Text.Trim()));

            _sb.AppendLine("[checklist]");
            foreach (DesktopShared.EntityClasses.TicketCheckListDetailEntity objDetail in _details)
            {
                string _task = objDetail.Task.Trim();
                if (!String.IsNullOrWhiteSpace(_task))
                    _sb.AppendLine(String.Format("~{0}", _task));
            }

            _sb.AppendLine("[/checklist]");
            tbNotes.Text = _sb.ToString();
        }*/
        ddlTicketCheckListMaster.TicketCheckListMasterId = null;
    }


    /// <summary>
    /// client combo box on selected index changed
    /// </summary>
    /// <param name="o"></param>
    /// <param name="e"></param>
    protected void rcbClient_SelectedIndexChanged(object o, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
    {
        if (!ddlTicketType.TicketTypeId.HasValue)
            ddlTicketType.TicketTypeId = DesktopShared.Ticket.TypeHelper.Id.HelpDesk;

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

        //check for on hold status .... also returns priority value
        int? _clientPriorityVaue = null;
        phClientOnHoldWarning.Visible = DesktopShared.Client.Status.IsOnHold(objClient, ref _clientPriorityVaue);
        ClientPriorityValue = _clientPriorityVaue;
        SetPrioritySelection();

        //default ticket type / email cc
        if ((objClient.Fields.State == EntityState.Fetched) && objClient.TicketTypeId.HasValue)
            ddlTicketType.TicketTypeId = objClient.TicketTypeId;

        //category required
        bool _categoryRequired = false;
        if (objClient.Fields.State == EntityState.Fetched)
            _categoryRequired = objClient.TicketCategoryRequired;
        ddlTicketCategory.IsRequired = _categoryRequired;
        litCategoryHeader.Text = _categoryRequired ? "<b>Category</b>" : "Category";
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

            ReportedByFirstName = _user.First.Trim();
            ReportedByLastName = _user.Last.Trim();
            ReportedByPhone = _user.Phone.Trim();
            ReportedByEmail = _user.Email.Trim();

            var _clientContact = new DesktopShared.EntityClasses.ClientContactEntity(_user.FkClientContact.HasValue ? _user.FkClientContact.Value : -1);
            if (_clientContact.Fields.State == EntityState.Fetched)
            {
                txtCellPhone.Text = _clientContact.Cellphone.Trim();
                txtHomePhone.Text = _clientContact.Homephone.Trim();
            }

            ClientUserPriorityValue = null;
            if (_user.RankingId.HasValue)
            {
                int _clientUserVal = -1;
                if (int.TryParse(_user.RankingSupportingTable.Description.Trim(), out _clientUserVal))
                    ClientUserPriorityValue = _clientUserVal;
            }
            SetPrioritySelection();
        }
    }

    /// <summary>
    /// add button click
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnAddPage_Click(object sender, EventArgs e)
    {
        evButtonClick(this, e);
    }

    #endregion

    #region public methods

    #region override 

    /// <summary>
    /// set up page
    /// </summary>
    public override void SetUpPage()
    {
        #region general

        AssignedTo = DesktopShared.User.UserID;

        //user tags
        ucUserTicketTagsDDL.UserId = DesktopShared.User.UserID;
        ucUserTicketTagsDDL.DefaultText = _defaultTextAddTag;
        ucUserTicketTagsDDL.Populate();

        DropDownList _ddl = ddlTicketCheckListMaster.GetDropDownList();
        if (_ddl.Items.Count == 0)
            ddlTicketCheckListMaster.Populate();

        #region timesheet

        TimesheetDate = DateTime.Now;
        TimesheetActivityId = DesktopShared.User.DefaultActivityId;
        ucTimesheetProject.SelectedClientId = SelectedClientId;

        string _startTime = "0800";
        string _timeSpent = "0000";

        DesktopShared.Timesheet.GetStartTimeAndTimeSpent(ref _startTime, ref _timeSpent);

        TimesheetStartTime = _startTime;
        TimesheetTimeSpent = _timeSpent;

        #endregion

        ucTicketPriority.Populate();

        ReceivedDateTime = DateTime.Now;
        
        #endregion

        #region details

        Designation = BitByBit.Configuration.GetConfigString("TicketDesignation_NetworkId", "N");
        ucTicketProduct.Populate();
        ReceivedMethod = "P";
        DispositionId = BitByBit.Configuration.GetConfigInt("TicketDisposition_BBBNewId", 99);
        Timer = null;

        #endregion

    }

    /// <summary>
    /// save ticket values
    /// </summary>
    /// <param name="schedulingError"></param>
    /// <param name="emailError"></param>
    /// <param name="emailErrorTrackingIds"></param>
    /// <returns></returns>
    public override int SaveValues(out bool schedulingError, out bool emailError, out List<int> emailErrorTrackingIds)
    {
        schedulingError = false;
        emailError = false;
        emailErrorTrackingIds = new List<int>();

        DesktopShared.EntityClasses.CscDefectsEntity ticket = new DesktopShared.EntityClasses.CscDefectsEntity();

        string _clientEmails = "";
        string _assignedToClientEmail = "";
        string _employeeEmails = "";

        #region general

        ticket.Summary = Summary.Trim();
        ticket.Assignedto = AssignedTo;

        //status / disposition
        if (DispositionId > 0)
        {
            //do not allow user to add active ticket for other user
            if ((DispositionId == DesktopShared.SiteHelper.Ticket.Dispostion.Id.Active) && (AssignedTo != DesktopShared.User.UserID))
                ticket.FkDisposition = DesktopShared.SiteHelper.Ticket.Dispostion.Id.New;
            else
                ticket.FkDisposition = DispositionId;
        }
        else
            ticket.FkDisposition = null;

        int ticketStatusID = DispositionId == 93 ? 77 : 76;
        ticket.FkStatus = ticketStatusID;

        //check list tasks
        string _notes = "";
        List<DesktopShared.Ticket.CheckList.Node> _checkListTasks = DesktopShared.Ticket.CheckList.ParseItems(tbNotes.Text.Trim(), ref _notes);
        bool _checkListAdded = _checkListTasks.Count > 0;
        if (!_checkListAdded)
            _notes = Notes.Trim();

        ticket.TicketTypeId = ddlTicketType.TicketTypeId;
        ticket.TicketCategoryId = ddlTicketCategory.TicketCategoryId;
        ticket.ProductId = ProductId > 0 ? ProductId : (int?)null;

        ticket.HoursToFix = HoursToFix.Trim();
        ticket.Description = Server.HtmlEncode(_notes);

        ticket.FkClient = SelectedClientId;

        ticket.ReportedByFirst = ReportedByFirstName.Trim();
        ticket.ReportedByLast = ReportedByLastName.Trim();
        ticket.Email = ReportedByEmail.Trim();
        ticket.Phone = ReportedByPhone.Trim();

        string _reportedByFullName = ReportedByFirstName.Trim() + " " + ReportedByLastName.Trim();
        if (_reportedByFullName.Length > 50)
            _reportedByFullName = _reportedByFullName.Substring(0, 50);
        ticket.Reportedby = _reportedByFullName;

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
                _clientContact.Password = "123456*";

            }

            _clientContact.First = DesktopShared.Utility.String.Truncate(ReportedByFirstName, 12);
            _clientContact.Last = DesktopShared.Utility.String.Truncate(ReportedByLastName, 18);
            _clientContact.Email = ReportedByEmail.Trim();
            _clientContact.Busphone = DesktopShared.Utility.String.Truncate(ReportedByPhone, 17);
            _clientContact.Cellphone = DesktopShared.Utility.String.Truncate(txtCellPhone.Text, 17);
            _clientContact.Homephone = DesktopShared.Utility.String.Truncate(txtHomePhone.Text, 17);
            _clientContact.LastUpdated = DesktopShared.Utility.Date.DateToString(DateTime.Now);
            _clientContact.LastUpdatedby = DesktopShared.User.UserName;

            
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

        //priority
        if (PriorityId > 0)
            ticket.FkPriority = PriorityId;
        else
            ticket.FkPriority = null;

        //scheduled
        ticket.ScheduledDate = ScheduledDateTime != DateTime.MinValue ? ScheduledDateTime : (DateTime?)null;

        //scheduled duration
        ticket.ScheduledDuration = DesktopShared.Utility.Time.TimeStringToDouble(ScheduledDuration.Trim());

        //need completion by
        ticket.NeedCompletionDate = NeedCompletedByDateTime != DateTime.MinValue ? NeedCompletedByDateTime : (DateTime?)null;

        //received date
        ticket.ReceiveDate = ReceivedDateTime;

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
        ticket.TimerInterval = Timer;

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

        #region assigned to employee = bit by bit employee

        else
        {
            DesktopShared.EntityClasses.UsersEntity _userEmployee =
                new DesktopShared.EntityClasses.UsersEntity(AssignedTo);

            if (!String.IsNullOrEmpty(_userEmployee.Email))
            {
                if (!_employeeEmails.Contains(_userEmployee.Email.Trim()))
                {
                    if (_employeeEmails.Length > 0)
                        _employeeEmails += "; ";

                    _employeeEmails += _userEmployee.Email.Trim();
                }
            }
        }

        #endregion

        #endregion

        #region client email

        if (!InternalOnly)
        {
            //client contacts selected via check box list
            _clientEmails += SelectedClientEmails;

            #region entered by client email

            if (chkEnteredByRecieveTicket.Checked)
            {
                if (!String.IsNullOrEmpty(ReportedByEmail))
                {
                    if (!_clientEmails.Contains(ReportedByEmail.Trim()))
                    {
                        if (_clientEmails.Length > 0)
                            _clientEmails += "; ";

                        _clientEmails += ReportedByEmail.Trim();
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

        #endregion

        ticket.FkCscprojects = 117;
        ticket.EnteredBy = DesktopShared.User.UserID;
        ticket.Lastupdated = DateTime.Now;

        if (AssignedTo != DesktopShared.User.UserID)
            ticket.AwaitingResponseUserId = AssignedTo;

        ticket.Save();

        #region tag ticket

        //user id (public tags are not assigned to user)
        int? userId = TagIsPublic ? (int?)null : DesktopShared.User.UserID;

        //strip out (P) or (PC)
        string _tagValue = TagValue.Replace("(P)", "").Replace("(PC)", "").Trim();

        if (IsTagged)
            DesktopShared.Ticket.Tagged.AddTagValue(ticket.Pcscdefects, _tagValue, userId, TagIsPublic, TagIsClientPublic);

        #endregion

        #region add history to ticket and send emails

        List<int> failedMessageTrackingIds = new List<int>();
        bool _externalMailSuccess = true;
        int? _externalMessageTrackingId = null;
        bool _internalMailSuccess = true;
        int? _internalMessageTrackingId = null;
        int _ticketHistoryId = 0;

        DesktopShared.Ticket.History.Add(
            ticket.Pcscdefects, //ticket id
            DesktopShared.User.UserID, //user id, 
            "Ticket Added: Desktop", //notes
            InternalNotes.Trim(), //internal notes
            true, // is new ticket
            _clientEmails, // client email addresses
            _employeeEmails, // employee email addresses
            true, // send email out
            DesktopShared.Ticket.History.Type.Id.UpdateByEmployee, //history type
             null,  //date/time stamp to user for history note created
            null, //ids for file to attach to email
            "", //update by override
            null, //internal ticket history type id (override history type id)
            ref _ticketHistoryId, //entity id
            ref _externalMailSuccess, //email successfully sent to client
            ref _externalMessageTrackingId, //message tracking id for email to client
            ref _internalMailSuccess, //email successfully sent to employees
            ref _internalMessageTrackingId //message tracking id for email to employee
            );

        #endregion

        //if (AssignedTo != DesktopShared.User.UserID)
            //DesktopShared.Ticket.Slack.SendMessage(ticket.Pcscdefects, DesktopShared.User.UserID);

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
            _timeSheet.Memo = Server.HtmlEncode(_notes);
            _timeSheet.InternalComments = Server.HtmlEncode(InternalNotes);
            _timeSheet.Workid = DesktopShared.User.GetUserNameOnly();
            _timeSheet.FkEmployee = DesktopShared.User.EmployeeID;
            _timeSheet.FkCscdefects = ticket.Pcscdefects;
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

        //calender entry
        if (ScheduledDateTime != DateTime.MinValue)
            schedulingError = !DesktopShared.Exchange.Calendar.CreateAppointment(ticket.Pcscdefects, AssignedTo, ScheduledSendInvite, DesktopShared.User.UserID);

        //ticket is set to active, all other active tickets need to be set to idle
        if ((DispositionId == DesktopShared.SiteHelper.Ticket.Dispostion.Id.Active) && (AssignedTo == DesktopShared.User.UserID))
            DesktopShared.Ticket.MakeActive(ticket.Pcscdefects, DesktopShared.User.UserID, AssignedTo, false, "");

        //check list tasks
        if (_checkListAdded)
            DesktopShared.Ticket.CheckList.AddByList(ticket.Pcscdefects, _checkListTasks, DesktopShared.User.UserID);

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

        return ticket.Pcscdefects;
    }

    #endregion

    #endregion

    #region public properties

    #region general

    /// <summary>
    /// get/set summary
    /// </summary>
    public string Summary
    {
        get { return tbSummary.Text; }
        set { tbSummary.Text = value; }
    }

    /// <summary>
    /// get/set selected client id
    /// </summary>
    public int SelectedClientId
    {
        get { return ucClient.SelectedClientId; }
        set { ucClient.SelectedClientId = value; }
    }

    /// <summary>
    /// get/set client contact id
    /// </summary>
    public int ClientContactId
    {
        get { return ucClientContact.ClientContactId; }
        set { ucClientContact.ClientContactId = value; }
    }

    /// <summary>
    /// get/set reported by first name
    /// </summary>
    public string ReportedByFirstName
    {
        get { return tbReportedByFirstName.Text; }
        set { tbReportedByFirstName.Text = value; }
    }

    /// <summary>
    /// get/set reported by last name
    /// </summary>
    public string ReportedByLastName
    {
        get { return tbReportedByLastName.Text; }
        set { tbReportedByLastName.Text = value; }
    }

    /// <summary>
    /// get/set reported by email
    /// </summary>
    public string ReportedByEmail
    {
        get { return tbReportedByEmail.Text; }
        set { tbReportedByEmail.Text = value; }
    }

    /// <summary>
    /// get/set reported by phone
    /// </summary>
    public string ReportedByPhone
    {
        get { return tbReportedByPhone.Text; }
        set { tbReportedByPhone.Text = value; }
    }

    /// <summary>
    /// get/set is tagged
    /// </summary>
    public bool IsTagged
    {
        get { return chkTag.Checked; }
        set { chkTag.Checked = value; }
    }

    /// <summary>
    /// get tag value from either text box or drop down list
    /// </summary>
    public string TagValue
    {
        get
        {
            //text box
            string tagValue = tbTagText.Text.Trim();

            if (tagValue.Trim().Length == 0)
            {
                //drop down list
                if (ucUserTicketTagsDDL.TagName != _defaultTextAddTag)
                    tagValue = ucUserTicketTagsDDL.TagName.Trim();
            }

            //default text if no entry in text box or ddl
            if (tagValue.Length == 0)
                tagValue = DesktopShared.Ticket.Tagged.DefaultTagText;

            return tagValue;
        }
    }

    /// <summary>
    /// get/set tag is public 
    /// </summary>
    public bool TagIsPublic
    {
        get
        {
            if (tbTagText.Text.Trim().Length > 0) //new value via text box
                return chkTagPublic.Checked;
            else //value via drop down list
            {
                if (ucUserTicketTagsDDL.TagName != _defaultTextAddTag)
                    return ucUserTicketTagsDDL.IsPublic;
                else
                    return false;
            }
        }
        set { chkTagPublic.Checked = value; }
    }

    /// <summary>
    /// get/set tag is client public
    /// </summary>
    public bool TagIsClientPublic
    {
        get
        {
            if (tbTagText.Text.Trim().Length > 0) //new value via text box
                return chkTagClientPublic.Checked;
            else //value via drop down list
            {
                if (ucUserTicketTagsDDL.TagName != _defaultTextAddTag)
                    return ucUserTicketTagsDDL.IsClientPublic;
                else
                    return false;
            }

        }
        set { chkTagClientPublic.Checked = value; }
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
    /// get timesheet project task id
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

    /// <summary>
    /// /get/set client priority value
    /// </summary>
    public int ? ClientPriorityValue
    {
        get
        {
            object obj = this.ViewState["ta_cpv"];
            return (obj == null) ? (int?)null : (int)obj;
        }
        set { this.ViewState["ta_cpv"] = value; }
    }

    /// <summary>
    /// /get/set client user priority value
    /// </summary>
    public int? ClientUserPriorityValue
    {
        get
        {
            object obj = this.ViewState["ta_cupv"];
            return (obj == null) ? (int?)null : (int)obj;
        }
        set { this.ViewState["ta_cupv"] = value; }
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
    /// set add mode for control
    /// </summary>
    public override AddMode FormAddMode
    {
        set { _addMode = value; }
    }

    #endregion

    #endregion

    #region public events

    /// <summary>
    /// add button event handler click
    /// </summary>
    public event EventHandler evButtonClick;

    #endregion

    #region custom validators

    /// <summary>
    /// server validation for add new tag
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cvAddTag_ServerValidate(object sender, ServerValidateEventArgs args)
    {
        cvAddTag.ErrorMessage = "Tag already exists for ticket";

        //client public must also be public
        if (chkTagClientPublic.Checked && !chkTagPublic.Checked)
        {
            cvAddTag.ErrorMessage = "Client Public tags must also be flagged as Public";
            args.IsValid = false;
            return;
        }

        //characters "|", "(P)", "(PC)" not allowed
        string _tagText = tbTagText.Text.Trim();
        if (_tagText.Length > 0)
        {
            if (_tagText.Contains("|") || _tagText.Contains("(P)") || _tagText.Contains("(PC)"))
            {
                cvAddTag.ErrorMessage = "Tags may not contain \"|\", \"(P)\", or \"(PC)\"";
                args.IsValid = false;
                return;
            }
        }
    }

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

            if (!DesktopShared.Utility.IsValidEmailAddress(ReportedByEmail))
            {
                cvClientContactEmail.ErrorMessage = "Invalid Client Contact Email Address";
                args.IsValid = false;
                return;
            }

            if (!DesktopShared.User.IsUniqueEmail(ReportedByEmail, _userId))
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

    #endregion
      
}
