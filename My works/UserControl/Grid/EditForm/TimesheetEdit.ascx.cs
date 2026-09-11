using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using SD.LLBLGen.Pro.ORMSupportClasses;

public partial class UserControl_Grid_EditForm_TimesheetEdit : AbstractTimesheetEdit
{
    private EditMode _editMode = EditMode.Grid;

    protected void Page_Load(object sender, EventArgs e)
    {
        //event handler for employee drop down list
        DropDownList ddlEmployee = ucEmployee.GetDropDownList();
        ddlEmployee.AutoPostBack = true;
        ddlEmployee.SelectedIndexChanged += new EventHandler(ddlEmployee_SelectedIndexChanged);

        ddlSmartClient.ClientChanged += new EventHandler(ucClient_ClientChanged);

        //Telerik.Web.UI.RadComboBox rcbClient = (Telerik.Web.UI.RadComboBox)ddlSmartClient.FindControl("ddlSmartClient");
        //if (rcbClient != null)
        //{
        //    rcbClient.AutoPostBack = true;
        //    rcbClient.SelectedIndexChanged += new RadComboBoxSelectedIndexChangedEventHandler(rcbClient_SelectedIndexChanged);
        //}

        SetUpControl();
    }

    #region private methods

    /// <summary>
    /// set user id for ticket drop down list & combo box
    /// </summary>
    /// <param name="userId"></param>
    private void SetUserIdForTickets(int userId)
    {
        ucEmployeeTicket.UserId = userId;
        ucOpenTicket.UserId = userId;
    }

    /// <summary>
    /// set up user control
    /// </summary>
    private void SetUpControl()
    {
        if (_editMode == EditMode.Grid)
        {
            mvButton.SetActiveView(viewGrid);
            mvButtonTop.SetActiveView(viewGridTop);
        }
        else
        {
            mvButton.SetActiveView(viewPage);
            mvButtonTop.SetActiveView(viewPageTop);

            divMain.Style.Add("border", "1px solid #3b5a82");
        }
    }

    #endregion

    #region protected methods / events
    private void ucClient_ClientChanged(object sender, EventArgs e)
    {
        ucProject.SelectedClientId = ddlSmartClient.SelectedClientId;
    }

    /// <summary>
    /// employee drop down list on selected index changed
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlEmployee_SelectedIndexChanged(object sender, EventArgs e)
    {
        int _employeeId = ucEmployee.EmployeeId > 0 ? ucEmployee.EmployeeId : DesktopShared.User.EmployeeID;
        int _userId = DesktopShared.Employee.GetUserId(_employeeId);

        SetUserIdForTickets(_userId);

        //update projects
        ucProject.EmployeeId = _employeeId;
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

    #endregion

    #region public methods

    /// <summary>
    /// load timesheet values
    /// </summary>
    /// <param name="timesheetID"></param>
    public override void LoadValues(int timesheetID)
    {
        if (timesheetID > 0)
        {
            DesktopShared.EntityClasses.TimesheetEntity _timeSheet =new DesktopShared.EntityClasses.TimesheetEntity(timesheetID);
            LoadValues(_timeSheet);
        }
        else
            LoadValues(null);
    }

    /// <summary>
    /// load timesheet values
    /// </summary>
    /// <param name="timesheet"></param>
    public override void LoadValues(DesktopShared.EntityClasses.TimesheetEntity timesheet)
    {
        btnInsert.Visible = (timesheet == null);
        btnInsertTop.Visible = (timesheet == null);

        btnUpdate.Visible = (timesheet != null);
        btnUpdateTop.Visible = (timesheet != null);

        int _employeeId = 0;

        #region existing record

        if (timesheet != null)
        {
            pnlExistingReadOnly.Visible = true;

            TimesheetId = timesheet.Ptimesheet;
            Created = DesktopShared.Utility.Date.StringToDate(timesheet.Created.Trim());
            LastUpdated = DesktopShared.Utility.Date.StringToDate(timesheet.LastUpdated.Trim());
            LastUpdatedBy = timesheet.LastUpdatedby.Trim();
            EmployeeCode = timesheet.Employee.Code;

            EnteredDate = timesheet.Date;
            StartTime = timesheet.Time.Trim();
            TimeSpent = DesktopShared.Utility.Time.TimeDoubleToString(timesheet.Hrs);
            if (timesheet.FkEmployee.HasValue)
            {
                ucProject.EmployeeId = timesheet.FkEmployee.Value;
                _employeeId = timesheet.FkEmployee.Value;
            }
            ProjtaskId = timesheet.FkProjtask.Value;
            if (timesheet.FkTaskaction.HasValue)
                ActivityId = timesheet.FkTaskaction.Value;
            if (timesheet.FkCscdefects.HasValue)
                TicketId = timesheet.FkCscdefects.Value;
            IsOvertime = timesheet.IsOvertime;
            ClientDetails = Server.HtmlDecode(timesheet.Memo.Trim());
            InternalNotes = Server.HtmlDecode(timesheet.InternalComments.Trim());
            if (timesheet.FkClient.HasValue)
            {
                ddlSmartClient.SelectedClientId = (int)timesheet.FkClient;
                ucClient_ClientChanged(this, EventArgs.Empty);
            }
            #region expenses

            Location = timesheet.Location.Trim();
            MilesExpense = timesheet.Miles;
            TollsExpense = timesheet.Tolls;
            ParkingExpense = timesheet.Parking;
            MassTranExpense = timesheet.Masstran;
            MiscExpense = timesheet.Misc;
            CommAmtExpense = timesheet.Commamt;

            #endregion

            if (_editMode == EditMode.Page)
            {
                btnUpdatePage.Text = "UPDATE";
                //btnUpdatePageTop.Text = "UPDATE";
            }

        }

        #endregion

        #region new record -> set defaults

        else
        {
            TimesheetId = 0;
            pnlExistingReadOnly.Visible = false;

            EnteredDate = DateTime.Now;
            ActivityId = DesktopShared.User.DefaultActivityId;

            ucProject.Populate();

            string _startTime = "0800";
            string _timeSpent = "0000";

            DesktopShared.Timesheet.GetStartTimeAndTimeSpent(ref _startTime, ref _timeSpent);

            StartTime = _startTime;
            TimeSpent = _timeSpent.ToString();

            #region expenses

            MilesExpense = 0;
            TollsExpense = 0;
            ParkingExpense = 0;
            MassTranExpense = 0;
            MiscExpense = 0;
            CommAmtExpense = 0;

            if (_editMode == EditMode.Page)
            {
                btnUpdatePage.Text = "ADD";
                //btnUpdatePageTop.Text = "ADD";
            }

            #endregion
        }

        #endregion

        //default employee id
        if (_employeeId == 0)
            _employeeId = DesktopShared.User.EmployeeID;

        #region admins -> ddl for employee

        if (DesktopShared.User.IsAdmin)
        {
            phAdmin.Visible = true;
            ucEmployee.EmployeeId = EmployeeId > 0 ? EmployeeId : DesktopShared.User.EmployeeID;

            _employeeId = ucEmployee.EmployeeId;
        }

        #endregion

        #region my tickets drop down list

        //get user id of employee
        int _userId = DesktopShared.Employee.GetUserId(_employeeId);

        //set user id for ticket drop down list & combo box
        SetUserIdForTickets(_userId);

        #endregion


    }

    /// <summary>
    /// save timesheet values
    /// </summary>
    /// <param name="timesheetID"></param>
    public override void SaveValues(int timesheetID)
    {
        DesktopShared.EntityClasses.TimesheetEntity _timeSheet = null;
        string dateTimeString = DesktopShared.Utility.Date.DateToString(DateTime.Now);
        bool ticketIdChanged = false;
        int employeeId = DesktopShared.User.EmployeeID;
        string userNameOnly = DesktopShared.User.GetUserNameOnly();


        if (timesheetID > 0)
        {
            _timeSheet = new DesktopShared.EntityClasses.TimesheetEntity(timesheetID);

            //check if ticket id has changed
            if (_timeSheet.FkCscdefects != TicketId)
                ticketIdChanged = true;

            if (phAdmin.Visible)
            {
                if (_timeSheet.FkEmployee.HasValue && (ucEmployee.EmployeeId != _timeSheet.FkEmployee.Value))
                {
                    employeeId = ucEmployee.EmployeeId;
                    _timeSheet.FkEmployee = employeeId;
                    DesktopShared.EntityClasses.EmployeeEntity _employee = DesktopShared.Employee.GetEmployeeEntity(employeeId);
                    if ((_employee != null) && (_employee.Fields.State == SD.LLBLGen.Pro.ORMSupportClasses.EntityState.Fetched))
                        _timeSheet.Workid = _employee.Code.Trim().ToLower();
                }
            }
        }
        else
        {
            _timeSheet = new DesktopShared.EntityClasses.TimesheetEntity();

            #region get employee id / user name only if admin is entering timesheet for other user

            if (phAdmin.Visible)
            {
                if (ucEmployee.EmployeeId != DesktopShared.User.EmployeeID)
                {
                    employeeId = ucEmployee.EmployeeId;
                    DesktopShared.EntityClasses.EmployeeEntity _employee = DesktopShared.Employee.GetEmployeeEntity(employeeId);
                    if ((_employee != null) && (_employee.Fields.State == SD.LLBLGen.Pro.ORMSupportClasses.EntityState.Fetched))
                        userNameOnly = _employee.Code.Trim().ToLower();
                }
            }

            #endregion

            _timeSheet.Created = dateTimeString;
            _timeSheet.Workid = userNameOnly.Trim();
            _timeSheet.FkEmployee = employeeId;

            #region null value not allowed
            
            _timeSheet.Clientcode = "";
            _timeSheet.Jobno = "";
            _timeSheet.Taskno = "";
            _timeSheet.Fncode = "";
            _timeSheet.Bilhrs = 0;
            _timeSheet.Bilamt = 0;
            _timeSheet.Ssman = "";
            _timeSheet.Workdate = "";
            _timeSheet.Timeid = "";

            #endregion
        }

        #region time spent

        int _intHourSpent = Convert.ToInt32(TimeSpent.Substring(0, 2));
        int _intMinuteSpent = Convert.ToInt32(TimeSpent.Substring(2));
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

        _timeSheet.Date = EnteredDate;
        _timeSheet.Time = StartTime;
        _timeSheet.Hrs = _hours;
        _timeSheet.Projtaskid = ProjtaskId.ToString();
        _timeSheet.FkProjtask = ProjtaskId;
        _timeSheet.FkClient = SelectedClientId;
        _timeSheet.Descr1 = ActivityName;
        _timeSheet.FkTaskaction = ActivityId;
        //_timeSheet.IsOvertime = IsOvertime;
        _timeSheet.Memo = Server.HtmlEncode(ClientDetails);
        _timeSheet.InternalComments = Server.HtmlEncode(InternalNotes);
        
        _timeSheet.FkCscdefects = TicketId;
        _timeSheet.FkProject = ProjectId;

        #region expenses

        _timeSheet.Location = Location;
        _timeSheet.Miles = MilesExpense;
        _timeSheet.Tolls = TollsExpense;
        _timeSheet.Parking = ParkingExpense;
        _timeSheet.Masstran = MassTranExpense;
        _timeSheet.Misc = MiscExpense;
        _timeSheet.Commamt = CommAmtExpense;

        #endregion

        _timeSheet.LastUpdated = dateTimeString;
        _timeSheet.LastUpdatedby = DesktopShared.User.GetUserNameOnly();
        _timeSheet.Save();

        #region check project hours, send email alert if past threshold

        int _clientIdForTimesheetCheck = SelectedClientId; //default to project client id
        
        if (TicketId != null) //if ticket selected, use client id assigned to ticket
        {
            DesktopShared.EntityClasses.CscDefectsEntity objTicket = new DesktopShared.EntityClasses.CscDefectsEntity(TicketId.Value);
            if ((objTicket.Fields.State == EntityState.Fetched) && (objTicket.FkClient.HasValue))
                _clientIdForTimesheetCheck = objTicket.FkClient.Value;
        }

        DesktopShared.Timesheet.CheckProjectHours(ProjtaskId, _timeSheet.Ptimesheet, _clientIdForTimesheetCheck);

        #endregion

        #region add history to ticket if ticked was selected in ddl or combo box AND (timesheet entry is new) OR (timesheet entry original TicketID does not equal new TicketID)

        if ((TicketId != null) && ((timesheetID <= 0) || (ticketIdChanged)))
        {
            //add history (email also sent out via shared function)
            DesktopShared.Ticket.History.Add(
                TicketId.Value, //ticket id
                DesktopShared.User.UserID, //user id, 
                ClientDetails.Trim(), //notes
                InternalNotes.Trim(), //internal notes
                false, // is new ticket
                true, // send email out
                DesktopShared.Ticket.History.Type.Id.UpdateByEmployee // history type
                );
        }

        #endregion
    }

    #endregion

    #region public properties

    #region override

    /// <summary>
    /// get/set employee id
    /// </summary>
    public override int EmployeeId
    {
        get
        {
            object obj = this.ViewState["EmployeeIdForEdit"];
            if (obj == null)
                return -1;
            else
                return (int)obj;
        }
        set { this.ViewState["EmployeeIdForEdit"] = value; }
    }

    #endregion

    /// <summary>
    /// get/set timesheet id
    /// </summary>
    public int TimesheetId
    {
        get
        {
            object obj = this.ViewState["TimesheetIdForEdit"];
            if (obj == null)
                return -1;
            else
                return (int)obj;
        }

        set
        {
            this.ViewState["TimesheetIdForEdit"] = value;
            litTimesheetId.Text = value.ToString();

        }
    }

    private int? ClientId
    {
        get
        {
            object obj = this.ViewState["cid_tse"];
            return (obj == null) ? (int?)null : (int)obj;
        }
        set { this.ViewState["cid_tse"] = value; }
    }
    #region read only

    /// <summary>
    /// set created date
    /// </summary>
    public DateTime Created
    {
        set { litCreated.Text = value.ToString(); }
    }

    /// <summary>
    /// set last updated date
    /// </summary>
    public DateTime LastUpdated
    {
        set { litLastUpdated.Text = value.ToString(); }
    }

    /// <summary>
    /// set last updated by
    /// </summary>
    public string LastUpdatedBy
    {
        set { litLastUpdatedBy.Text = value; }
    }

    /// <summary>
    /// set employee code
    /// </summary>
    public string EmployeeCode
    {
        set { litEmployeeCode.Text = value; }
    }

    #endregion

    /// <summary>
    /// get/set entered date
    /// </summary>
    public DateTime EnteredDate
    {
        get { return rdpDate.SelectedDate.Value; }
        set { rdpDate.SelectedDate = value; }
    }

    /// <summary>
    /// get/set start time
    /// </summary>
    public string StartTime
    {
        get
        {
            return ucStartDDL.Hour.ToString().PadLeft(2, '0') +
                ucStartDDL.Minute.ToString().PadLeft(2, '0');
        }
        set
        {
            if (!String.IsNullOrEmpty(value))
            {
                ucStartDDL.Hour = Convert.ToInt32(value.Substring(0, 2));
                ucStartDDL.Minute = Convert.ToInt32(value.Substring(2, 2));
            }
        }
    }

    /// <summary>
    /// get/set time spent
    /// </summary>
    public string TimeSpent
    {
        get
        {
            return ucTimeSpentDDL.Hour.ToString().PadLeft(2, '0') +
                ucTimeSpentDDL.Minute.ToString().PadLeft(2, '0');
        }
        set
        {
            ucTimeSpentDDL.Hour = Convert.ToInt32(value.Substring(0, 2));
            ucTimeSpentDDL.Minute = Convert.ToInt32(value.Substring(2, 2));
        }
    }

    /// <summary>
    /// set/set client id
    /// </summary>
    public int SelectedClientId
    {
        get { return ucProject.SelectedClientId; }
        set { ucProject.SelectedClientId = value; }
    }

    /// <summary>
    /// get/set project ask id
    /// </summary>
    public int ProjtaskId
    {
        get { return ucProject.SelectedProjtaskId; }
        set { ucProject.SelectedProjtaskId = value; }
    }

    /// <summary>
    /// get/set project id
    /// </summary>
    public int ProjectId
    {
        get { return ucProject.SelectedProjectId; }
        set { ucProject.SelectedProjectId = value; }
    }

    /// <summary>
    /// get/set ticket id
    /// </summary>
    public int ? TicketId
    {
        get
        {
            if (ucEmployeeTicket.TicketId > 0)
                return ucEmployeeTicket.TicketId;
            else if (ucOpenTicket.SelectedTicketId > 0)
                return ucOpenTicket.SelectedTicketId;
            else
                return null;
        }
        set
        {
            if (value.HasValue)
                ucOpenTicket.SelectedTicketId = value.Value;
        }
    }


    /// <summary>
    /// get/set is over time
    /// </summary>
    public bool IsOvertime
    {
        get { return chkIsOvertime.Checked; }
        set { chkIsOvertime.Checked = value; }
    }

    /// <summary>
    /// get/set activity id
    /// </summary>
    public int ActivityId
    {
        get { return ucActivity.TaskActionId; }
        set { ucActivity.TaskActionId = value; }
    }

    /// <summary>
    /// get activity name
    /// </summary>
    public string ActivityName
    {
        get { return ucActivity.TaskActionName; }
    }

    /// <summary>
    /// get/set client details
    /// </summary>
    public string ClientDetails
    {
        get { return tbClientDetails.Text; }
        set { tbClientDetails.Text = value; }
    }

    /// <summary>
    /// get/set internal notes
    /// </summary>
    public string InternalNotes
    {
        get { return tbInternalNotes.Text; }
        set { tbInternalNotes.Text = value; }
    }

    #region expenses

    //get/set expenses location
    public string Location
    {
        get { return tbLocationExpense.Text; }
        set { tbLocationExpense.Text = value; }
    }

    /// <summary>
    /// get/set expenses miles
    /// </summary>
    public double MilesExpense
    {
        get { return Convert.ToDouble(tbMilesExpense.Text); }
        set { tbMilesExpense.Text = value.ToString(); }
    }

    /// <summary>
    /// get/set expenses tolls
    /// </summary>
    public double TollsExpense
    {
        get { return Convert.ToDouble(tbTollsExpense.Text); }
        set { tbTollsExpense.Text = value.ToString(); }
    }

    /// <summary>
    /// get/set expenses parking
    /// </summary>
    public double ParkingExpense
    {
        get { return Convert.ToDouble(tbParkingExpense.Text); }
        set { tbParkingExpense.Text = value.ToString(); }
    }

    /// <summary>
    /// get/set expenses mass transit
    /// </summary>
    public double MassTranExpense
    {
        get { return Convert.ToDouble(tbMassTranExpense.Text); }
        set { tbMassTranExpense.Text = value.ToString(); }
    }

    /// <summary>
    /// get/set expenses misc
    /// </summary>
    public double MiscExpense
    {
        get { return Convert.ToDouble(tbMiscExpense.Text); }
        set { tbMiscExpense.Text = value.ToString(); }
    }

    //get/set expenses comm amount
    public double CommAmtExpense
    {
        get { return Convert.ToDouble(tbCommAmtExpense.Text); }
        set { tbCommAmtExpense.Text = value.ToString(); }
    }

    #endregion

    /// <summary>
    /// set edit moe
    /// </summary>
    public EditMode FormEditMode
    {
        set { _editMode = value; }
    }

    #endregion

    #region public event

    /// <summary>
    /// update button event click handler
    /// </summary>
    public event EventHandler evButtonClick;

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

    #region custom validators

    /// <summary>
    /// validate time spent
    /// </summary>
    /// <param name="source"></param>
    /// <param name="args"></param>
    protected void cvTimeSpent_ServerValidate(object source, ServerValidateEventArgs args)
    {
        args.IsValid = (TimeSpent != "0000");
    }

    /// <summary>
    /// validate notes entered
    /// </summary>
    /// <param name="source"></param>
    /// <param name="args"></param>
    protected void cvNotes_ServerValidate(object source, ServerValidateEventArgs args)
    {
        args.IsValid = (ClientDetails.Trim().Length>0 || InternalNotes.Trim().Length>0);
    }

    /// <summary>
    /// validate date entered is less than max allowed (stored in config file)
    /// </summary>
    /// <param name="source"></param>
    /// <param name="args"></param>
    protected void cvDate_ServerValidate(object source, ServerValidateEventArgs args)
    {
        //allow even if they fail validation ... just send email 

        //timesheet admin do not have to obey this rule
        //if (DesktopShared.User.IsTimesheetAdmin)
        //{
            //args.IsValid = true;
            //return;
        //}

        //check date entered is not greater than max days allowed
        int maxDays = TimesheetId > 0 ? Desktop.SiteHelper.Timesheet.MaxDaysForEdit : Desktop.SiteHelper.Timesheet.MaxDaysForAdd ;
        //string addOrEdit = TimesheetId > 0 ? "edit" : "add";

        DateTime minDate = BitByBit.Utility.Date.StartOfDayDate(DateTime.Now.AddDays(-maxDays));
        if (EnteredDate < minDate)
        {
            //email alert 
            Desktop.Email.Timesheet.SendWarning(
                DesktopShared.User.UserFullName.Trim(), //employee name making add / edit
                TimesheetId > 0 ? TimesheetId : (int?)null, //ticket if if available (for edit)
                EnteredDate //date employee tried to enter
                );

            //error message
            //cvDate.ErrorMessage = "You may not " + addOrEdit + " timesheet entries entered " + maxDays.ToString() + " days earlier than today.";

            //return false
            //args.IsValid = false;
            //return;
        }

        args.IsValid = true;
        
    }

    #endregion

}
