using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class UserControl_Grid_EditForm_TicketActive : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
            SetUpPage();
    }

    #region private methods

    /// <summary>
    /// set up page for initial page load -> display list of selected tickets to be merged
    /// </summary>
    private void SetUpPage()
    {
        //original active ticket id
        string _oldActiveTicketId = BitByBit.Web.Request.GetString("TicketId");
        int _ticketId = 0;
        int.TryParse(_oldActiveTicketId, out _ticketId);

        //new active ticket id
        string _newActiveTicketId = BitByBit.Web.Request.GetString("NewTicketId");
        int _newTicketId = -1;
        if (int.TryParse(_newActiveTicketId, out _newTicketId))
            NewTicketId = _newTicketId;

        if (_ticketId > 0)
        {
            mvMain.SetActiveView(viewMainDefault);  
            TicketId = _ticketId;

            //ticket entity
            DesktopShared.EntityClasses.CscDefectsEntity objTicket = new DesktopShared.EntityClasses.CscDefectsEntity(TicketId.Value);
            if (objTicket.Fields.State != SD.LLBLGen.Pro.ORMSupportClasses.EntityState.Fetched)
            {
                DisplayError("Unable to fetch cscDefects entity.");
                return;
            }

            //last active ticket entity
            DesktopShared.EntityClasses.ActiveTicketEntity objActiveTicket = DesktopShared.Ticket.GetLastActiveTicketForToday(TicketId.Value, DesktopShared.User.UserID);
            if ((objActiveTicket == null) || (objActiveTicket.Fields.State != SD.LLBLGen.Pro.ORMSupportClasses.EntityState.Fetched))
            {
                DisplayError("There is no active ticket from today to be updated.");
                return;
            }
            
            #region start/end times

            string _startTime = "";
            string _endTime = "";
            int _hour = 0;
            int _minute = 0;

            //start
            if (objActiveTicket.StartTime.HasValue)
            {
                _hour = objActiveTicket.StartTime.Value.Hour;
                _minute = objActiveTicket.StartTime.Value.Minute;

                DesktopShared.Utility.Time.RoundDownBy15(ref _hour, ref _minute);
                _startTime = String.Format("{0}:{1}", 
                    _hour.ToString().PadLeft(2, '0'), 
                    _minute.ToString().PadLeft(2, '0')
                    );
            }

            //end
            if (objActiveTicket.EndTime.HasValue)
            {
                _hour = objActiveTicket.EndTime.Value.Hour;
                _minute = objActiveTicket.EndTime.Value.Minute;

                DesktopShared.Utility.Time.RoundUpBy15(ref _hour, ref _minute);
                _endTime = String.Format("{0}:{1}",
                    _hour.ToString().PadLeft(2, '0'),
                    _minute.ToString().PadLeft(2, '0')
                    );
            }

            if ((_startTime.Trim().Length == 0) || (_endTime.Trim().Length == 0))
            {
                DisplayError("Unable to get valid start or end time.");
                return;
            }

            if (_startTime == _endTime)
            {
                DisplayError("Start time = end time.  Not enought time spent to enter timesheet.");
                return;
            }

            //display
            litTimeSpent.Text = String.Format("{0} - {1}", _startTime, _endTime);

            //values held in view state for entry
            ActiveTicketId = objActiveTicket.Id;
            ActiveTicketStartTime = _startTime.Replace(":", "");

            #endregion

            //ticket description
            litTicketBeingUpdated.Text = String.Format("{0} - {1} ({2}", TicketId, objTicket.Summary.Trim(), objTicket.Client.Company.Trim());

            //populate project drop down list
            ddlTimesheetProject.SelectedClientId = objTicket.FkClient.HasValue ? objTicket.FkClient.Value : -1;

            //default activity id
            int? _defaultActivityId = DesktopShared.Employee.GetDefaultActivityId(DesktopShared.User.EmployeeID);
            if (_defaultActivityId.HasValue)
                ddlActivity.TaskActionId = _defaultActivityId.Value;
        }
        else
            DisplayError("Missing Ticket ID.");
    }

    /// <summary>
    /// call close window function which also refreshes parent page
    /// </summary>
    private void CloseWindow()
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        if ((NewTicketId.HasValue) && (NewTicketId.Value > 0))
            sb.Append(String.Format("GetRadWindow().BrowserWindow.location.href = '/Ticket/Detail2.aspx?Id={0}';", NewTicketId.Value));
        else
            sb.Append("GetRadWindow().BrowserWindow.location.reload();");
        sb.Append("GetRadWindow().close();");
        Page.ClientScript.RegisterStartupScript(Page.GetType(), "mykey", sb.ToString(), true);
        //Page.ClientScript.RegisterStartupScript(Page.GetType(), "mykey", "Close();", true);
        
    }

    /// <summary>
    /// display error message
    /// </summary>
    /// <param name="errorMessage"></param>
    private void DisplayError(string errorMessage)
    {
        mvMain.SetActiveView(viewMainError);
        btnSubmit.Visible = false;
        litErrorMessage.Text = errorMessage.Trim();
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
        if (this.IsValid)
        {
            DateTime _now = DateTime.Now;
            int _projtaskId = ddlTimesheetProject.SelectedProjtaskId; 
            DesktopShared.EntityClasses.CscDefectsEntity objTicket = new DesktopShared.EntityClasses.CscDefectsEntity(TicketId.Value);
            DesktopShared.EntityClasses.ActiveTicketEntity objActiveTicket = new DesktopShared.EntityClasses.ActiveTicketEntity(ActiveTicketId.Value);

            //use exact end date for active ticket close for history created
            DateTime? _noteCreatedDate = null;
            if ((objActiveTicket.Fields.State == SD.LLBLGen.Pro.ORMSupportClasses.EntityState.Fetched) && (objActiveTicket.EndTime.HasValue))
                _noteCreatedDate = objActiveTicket.EndTime.Value;

            #region add history note

            #region external & internal notes

            //external notes
            string _notes = tbExternalNotes.Text.Trim();
            if (_notes.Length > 8000)
                _notes = _notes.Substring(0, 8000);           

            //internal notes
            string _internalNotes = tbInternalNotes.Text.Trim();           
            if (_internalNotes.Length > 8000)
                _internalNotes = _internalNotes.Substring(0, 8000);

            #endregion

            #region client email adddress if external note

            string _clientEmail = "";
            if (_notes.Trim().Length > 0)
            {
                if (objTicket.Email.Trim().Length > 0)
                    _clientEmail = objTicket.Email.Trim();
            }

            #endregion

            DesktopShared.Ticket.History.Add(
                TicketId.Value, //ticket id
                DesktopShared.User.UserID, //user id, 
                _notes, //notes
                _internalNotes.Trim(), //internal notes
                false, // is new ticket
                _clientEmail, // client email addresses
                "", // employee email addresses
                true,  // send email out
                DesktopShared.Ticket.History.Type.Id.UpdateByEmployee, // history type id
                _noteCreatedDate //history note created
            );

            #endregion

            #region timesheet entry
           
            DesktopShared.EntityClasses.TimesheetEntity objTimesheet = new DesktopShared.EntityClasses.TimesheetEntity();

            objTimesheet.Date = DateTime.Now;
            objTimesheet.Time = ActiveTicketStartTime;
            objTimesheet.Hrs = DesktopShared.Utility.Time.CalculateTimeSpent(objActiveTicket.StartTime.Value, objActiveTicket.EndTime.Value, true);
            objTimesheet.Projtaskid = _projtaskId.ToString();
            objTimesheet.FkProjtask = _projtaskId;
            objTimesheet.FkClient = objTicket.FkClient.Value;
            objTimesheet.Descr1 = ddlActivity.TaskActionName.Trim();
            objTimesheet.FkTaskaction = ddlActivity.TaskActionId;
            objTimesheet.Memo = Server.HtmlEncode(_notes);
            objTimesheet.InternalComments = Server.HtmlEncode(_internalNotes);
            objTimesheet.Workid = DesktopShared.User.GetUserNameOnly();
            objTimesheet.FkEmployee = DesktopShared.User.EmployeeID;
            objTimesheet.FkCscdefects = TicketId;
            objTimesheet.FkProject = ddlTimesheetProject.SelectedProjectId;

            #region null value not allowed

            objTimesheet.Clientcode = "";
            objTimesheet.Jobno = "";
            objTimesheet.Taskno = "";
            objTimesheet.Fncode = "";
            objTimesheet.Bilhrs = 0;
            objTimesheet.Location = "";
            objTimesheet.Miles = 0;
            objTimesheet.Tolls = 0;
            objTimesheet.Parking = 0;
            objTimesheet.Masstran = 0;
            objTimesheet.Misc = 0;
            objTimesheet.Bilamt = 0;
            objTimesheet.Commamt = 0;
            objTimesheet.Ssman = "";
            objTimesheet.Workdate = "";
            objTimesheet.Timeid = "";

            #endregion

            string dateTimeString = DesktopShared.Utility.Date.DateToString(DateTime.Now);
            objTimesheet.Created = dateTimeString;
            objTimesheet.LastUpdated = dateTimeString;
            objTimesheet.LastUpdatedby = DesktopShared.User.GetUserNameOnly();
            objTimesheet.Save();

            //check project hours, send email alert if past threshold
            DesktopShared.Timesheet.CheckProjectHours(_projtaskId, objTimesheet.Ptimesheet, objTicket.FkClient);

            #endregion

            #region 3.  update TS entered flag for ActiveTicket
            
            if (objActiveTicket.Fields.State == SD.LLBLGen.Pro.ORMSupportClasses.EntityState.Fetched)
            {
                objActiveTicket.TimeSheetEntered = true;
                objActiveTicket.LastUpdated = _now;
                objActiveTicket.Save();
            }
     
            #endregion

            //remove awaiting response flag as user has entered update if they hit submit
            if (objTicket.AwaitingResponseUserId.HasValue)
            {
                objTicket.AwaitingResponseUserId = null;
                objTicket.AwaitingResponseInternal = true;
                objTicket.Save();
            }

            CloseWindow();
        }
    }

    /// <summary>
    /// close button on click
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnClose_Click(object sender, EventArgs e)
    {
        CloseWindow();
    }

    #region custom validators

    /// <summary>
    /// validate merge master ticket is valid
    /// </summary>
    /// <param name="source"></param>
    /// <param name="args"></param>
    protected void cvActiveTicket_ServerValidate(object source, ServerValidateEventArgs args)
    {
        args.IsValid = (tbExternalNotes.Text.Trim().Length > 0) || (tbInternalNotes.Text.Trim().Length > 0);    
    }

    #endregion

    #endregion

    #region private properties

    /// <summary>
    /// get/set ticket id (the original active one)
    /// </summary>
    private int? TicketId
    {
        get
        {
            object obj = this.ViewState["TicketIdForTicketActive"];
            return (obj == null) ? -1 : (int)obj;
        }
        set { this.ViewState["TicketIdForTicketActive"] = value; }
    }

    /// <summary>
    /// get/set new ticket id (the one switching to active)
    /// </summary>
    private int? NewTicketId
    {
        get
        {
            object obj = this.ViewState["NewTicketIdForTicketActive"];
            return (obj == null) ? -1 : (int)obj;
        }
        set { this.ViewState["NewTicketIdForTicketActive"] = value; }
    }

    /// <summary>
    /// get/set active ticket id
    /// </summary>
    private int? ActiveTicketId
    {
        get
        {
            object obj = this.ViewState["ActiveTicketIdForTicketActive"];
            return (obj == null) ? -1 : (int)obj;
        }
        set { this.ViewState["ActiveTicketIdForTicketActive"] = value; }
    }

    /// <summary>
    /// get/set active ticket start time
    /// </summary>
    private string ActiveTicketStartTime
    {
        get
        {
            object obj = this.ViewState["ActiveTicketStartTimeForTicketActive"];
            return (obj == null) ? "" : (string)obj;
        }
        set { this.ViewState["ActiveTicketStartTimeForTicketActive"] = value; }
    }

    #endregion

}