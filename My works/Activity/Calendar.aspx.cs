using DesktopShared.CollectionClasses;
using SD.LLBLGen.Pro.ORMSupportClasses;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Graph;
using System.Threading.Tasks;
using Microsoft.Identity.Client;
using Microsoft.Graph.Core;
using Telerik.Web.UI;

public partial class Activity_Calendar : System.Web.UI.Page
{
    private DataTable newdt;
    private int? pemployee = null;
    private double totaldiffhour = 0;
    private double totaltimespent = 0;
    private bool submitclick = false;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ucCreatedStart.SelectedDate = DateTime.Now;
            ucEndDate.SelectedDate = DateTime.Now;
            ddlEmployee.EmployeeId = DesktopShared.User.UserID;

            string email = DesktopShared.User.GetEmail(DesktopShared.User.UserID);
            if (email == "sricky@bitxbit.com" || email == "rob@bitxbit.com" || email == "dxuereb@bitxbit.com")
            //if (email == "rob@bitxbit.com" || email == "dxuereb@bitxbit.com")
                ddlEmployee.Enabled = true;
        }        
    }

    protected void cvCreatedDates_ServerValidate(object source, ServerValidateEventArgs args)
    {
        args.IsValid = true;
        if (!ucCreatedStart.SelectedDate.HasValue || !ucEndDate.SelectedDate.HasValue)
            return;
        args.IsValid = ucEndDate.SelectedDate.Value.Date >= ucCreatedStart.SelectedDate.Value.Date;
    }

    protected System.Data.DataTable Createddt()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("Id", System.Type.GetType("System.String"));//0
        dt.Columns.Add("Email", System.Type.GetType("System.String"));//1
        dt.Columns.Add("Subject", System.Type.GetType("System.String"));//2
        dt.Columns.Add("AllDate", System.Type.GetType("System.String"));//3
        dt.Columns.Add("StartDate", System.Type.GetType("System.String"));//3
        dt.Columns.Add("EndDate", System.Type.GetType("System.String"));//4

        return dt;
    }

    protected async void BindGrid()
    {
        try
        {
            if (ucCreatedStart.SelectedDate.HasValue && ucEndDate.SelectedDate.HasValue && ddlEmployee.EmployeeId != 0)
            {
                DateTime startDate = DateTime.Parse(ucCreatedStart.SelectedDate.Value.ToString().Trim());
                DateTime endDate = DateTime.Parse(ucEndDate.SelectedDate.Value.ToString().Trim());

                newdt = Createddt();
                string userEmail = "";
                GetEmpEmail(ddlEmployee.EmployeeId, ref userEmail, ref pemployee);
                PempployeeId = pemployee;

                var calendarService = new GraphCalendarService();
                var events = await calendarService.GetUserCalendarEvents(userEmail, startDate, endDate);

                for (int i = 0; i <= events.Count - 1; i++)
                {
                    DataRow row = newdt.NewRow();
                    row["Id"] = i + 1;
                    row["Email"] = events[i].Organizer;
                    row["Subject"] = events[i].Subject;
                    row["AllDate"] = events[i].StartTime;
                    row["StartDate"] = events[i].StartTime;
                    row["EndDate"] = events[i].EndTime;

                    newdt.Rows.Add(row);

                }
                ViewState["StartDate"] = startDate;
                ViewState["EndDate"] = endDate;
                ViewState["GridData"] = newdt;
                ViewState["PageSize"] = 20;

                rgCalendarEvents.DataSource = newdt;
                rgCalendarEvents.DataBind();
            }
        }
        catch (Exception ex)
        {
            DisplayMessage(litMessage, ex.ToString(), Bootstrap.Alert.AlertType.Danger);
        }
    }

    protected void rgCalendarEvents_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        //BindGrid();
        //BindGrid();
    }
    
    protected void Decision_Command(object sender, CommandEventArgs e)
    {
        int _usercode = DesktopShared.User.UserID;
        int employeeId = DesktopShared.User.EmployeeID;
        string userNameOnly = DesktopShared.User.GetUserNameOnly();
        string str = e.CommandName.ToString().Trim();
        string dateTimeString = DesktopShared.Utility.Date.DateToString(DateTime.Now);
        int _count = 0;
        double _hours = 0;
        switch (str)
        {
            case "Add":
                #region for checking
                foreach (GridEditableItem item in rgCalendarEvents.MasterTableView.Items)
                {
                    CheckBox chk = (CheckBox)item.FindControl("cboxSelect");
                    if (chk.Checked == true)
                    {
                        DropDownList ddlProjectTask = item.FindControl("ddlProjectTask") as DropDownList;
                        UserControl_DropDownList_TaskAction ddlTaskAction = item.FindControl("ddlTaskAction") as UserControl_DropDownList_TaskAction;
                        UserControl_DropDownList_Time ddltimspent = item.FindControl("ddlTimeSpent") as UserControl_DropDownList_Time;
                        TextBox txtNotes = item.FindControl("txtNotes") as TextBox;

                        if (String.IsNullOrEmpty(ddlProjectTask.SelectedValue))
                        {
                            DisplayMessage("Please Select Client Project(s)", Bootstrap.Alert.AlertType.Warning);
                            return;
                        }
                    }
                }
                #endregion

                #region insert into database
                foreach (GridEditableItem item in rgCalendarEvents.MasterTableView.Items)
                {
                    CheckBox chk = (CheckBox)item.FindControl("cboxSelect");
                    if (chk.Checked == true)
                    {
                        DropDownList ddlProjectTask = item.FindControl("ddlProjectTask") as DropDownList;
                        UserControl_DropDownList_TaskAction ddlTaskAction = item.FindControl("ddlTaskAction") as UserControl_DropDownList_TaskAction;
                        UserControl_DropDownList_Time ddltimspent = item.FindControl("ddlTimeSpent") as UserControl_DropDownList_Time;
                        TextBox txtNotes = item.FindControl("txtNotes") as TextBox;

                        //if (!String.IsNullOrEmpty(ddlProjectTask.SelectedValue))
                        //{
                        _count++;
                        DesktopShared.EntityClasses.TimesheetEntity _timeSheet = new DesktopShared.EntityClasses.TimesheetEntity();

                        _timeSheet.Created = dateTimeString;
                        _timeSheet.Workid = userNameOnly.Trim();
                        _timeSheet.FkEmployee = employeeId;

                        #region null value not allowed
                        _timeSheet.Location = "";
                        _timeSheet.Clientcode = "";
                        _timeSheet.Jobno = "";
                        _timeSheet.Taskno = "";
                        _timeSheet.Fncode = "";
                        _timeSheet.Bilhrs = 0;
                        _timeSheet.Bilamt = 0;
                        _timeSheet.Ssman = "";
                        _timeSheet.Workdate = "";
                        _timeSheet.Timeid = "";
                        _timeSheet.Miles = 0;
                        _timeSheet.Tolls = 0;
                        _timeSheet.Parking = 0;
                        _timeSheet.Masstran = 0;
                        _timeSheet.Misc = 0;
                        _timeSheet.Commamt = 0;
                        #endregion

                        //DateTime _start = Convert.ToDateTime(item["StartDate"].Text);
                        DateTime dateTime = DateTime.Parse(item["StartDate"].Text);
                        _timeSheet.Date = dateTime;
                        _timeSheet.Time = dateTime.ToString("HH:mm").Replace(":", "");

                        int _intHourSpent = Convert.ToInt32(ddltimspent.Hour);
                        int _intMinuteSpent = Convert.ToInt32(ddltimspent.Minute);
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

                        _hours = Convert.ToDouble(_intHourSpent.ToString() + _strMinuteSpent);
                        totaltimespent += _hours;
                        //_timeSheet.Hrs = Convert.ToDouble(item["Duration"].Text);
                        _timeSheet.Hrs = _hours;
                        _timeSheet.Projtaskid = SelectedProjtaskId(ddlProjectTask.SelectedValue).ToString();
                        _timeSheet.FkProjtask = SelectedProjtaskId(ddlProjectTask.SelectedValue);
                        _timeSheet.FkClient = SelectedClientId(ddlProjectTask.SelectedValue);
                        _timeSheet.FkTaskaction = ddlTaskAction.TaskActionId;
                        _timeSheet.Descr1 = ddlTaskAction.TaskActionName;
                        _timeSheet.FkProject = SelectedProjectId(ddlProjectTask.SelectedValue);
                        _timeSheet.Memo = Server.HtmlEncode(txtNotes.Text);

                        _timeSheet.LastUpdated = dateTimeString;
                        _timeSheet.LastUpdatedby = DesktopShared.User.GetUserNameOnly();

                        _timeSheet.Save();
                        //}
                        //else
                        //{
                        //    DisplayMessage("Please select client project", Bootstrap.Alert.AlertType.Warning);
                        //    return;
                        //}
                    }
                }
                #endregion

                DisplayMessage(String.Format("{0} Events has been {1} - {2}, {3} timesheet hours added", _count, "Added", DateTime.Now, totaltimespent), Bootstrap.Alert.AlertType.Success);
                submitclick = true;
                BindGrid();
                break;
        }
    }

    protected void rgCalendarEvents_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            GridDataItem item = (GridDataItem)e.Item;

            DateTime startdate = Convert.ToDateTime(item["StartDate"].Text);
            DateTime estStartTime = TimeZoneInfo.ConvertTimeFromUtc(startdate, TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time"));
            item["StartDate"].Text = estStartTime.ToString("yyyy-MM-dd HH:mm");

            DateTime enddate = Convert.ToDateTime(item["EndDate"].Text);
            DateTime estEndTime = TimeZoneInfo.ConvertTimeFromUtc(enddate, TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time"));
            item["EndDate"].Text = estEndTime.ToString("yyyy-MM-dd HH:mm");

            item["AllDate"].Text = estStartTime.DayOfWeek + "<br/>" + item["StartDate"].Text + "<br/>" + item["EndDate"].Text;

            UserControl_DropDownList_TaskAction ddlTaskAction = item.FindControl("ddlTaskAction") as UserControl_DropDownList_TaskAction;
            int _pemployee = (int)PempployeeId;
            int? _taskaction = (int?)DesktopShared.Employee.GetDefaultActivityId(_pemployee);
            if(_taskaction > 0)
                ddlTaskAction.TaskActionId = (int)DesktopShared.Employee.GetDefaultActivityId(_pemployee);

            #region Timepent
            UserControl_DropDownList_Time ddltimspent = item.FindControl("ddlTimeSpent") as UserControl_DropDownList_Time;

            TimeSpan difference = RoundUp0Minutes(estEndTime - estStartTime);
            ddltimspent.Hour = Convert.ToInt32(difference.Hours);
            ddltimspent.Minute = Convert.ToInt32(difference.Minutes);

            CheckBox chk = (CheckBox)item.FindControl("cboxSelect");
            if (chk.Checked == true)
                totaldiffhour = totaldiffhour + difference.TotalHours;
            #endregion

            DataTable dtProjectTask = GetEmployeeProjects((int)_pemployee);
            Literal litjs = item.FindControl("litJs") as Literal;
            DropDownList list = item.FindControl("ddlProjectTask") as DropDownList;

            list.DataSource = dtProjectTask;
            list.DataTextField = "Display";
            list.DataValueField = "DropDownValue";
            list.DataBind();
            list.Items.Insert(0, new System.Web.UI.WebControls.ListItem(String.Empty, String.Empty));

            #region notes
            TextBox txtNotes = item.FindControl("txtNotes") as TextBox;
            string _projectid = item["Subject"].Text;
            if (item["Subject"].Text.IndexOf(":") > 0)
            {
                string projectid = item["Subject"].Text.Substring(0, item["Subject"].Text.IndexOf(":"));
                string subjecttonotes = item["Subject"].Text.Substring(item["Subject"].Text.IndexOf(":") + 1, (item["Subject"].Text.Length - item["Subject"].Text.IndexOf(":")) - 1);
                if (!String.IsNullOrEmpty(projectid))
                {
                    int _projtid = 0;
                    bool isNumeric = int.TryParse(projectid, out _projtid);
                    if (isNumeric)
                        list.SelectedValue = valueproject(_projtid);
                }
                txtNotes.Text = subjecttonotes;
            }
            else
                txtNotes.Text = item["Subject"].Text;
            #endregion

            #region checkbox
            if (submitclick)
            {
                CheckBox chksingle = (CheckBox)item.FindControl("cboxSelect");
                chksingle.Checked = false;
            }
            #endregion
        }
        else if (e.Item is GridHeaderItem)
        {
            GridHeaderItem item = (GridHeaderItem)e.Item;
            #region checkbox
            if (submitclick)
            {
                CheckBox chkAll = (CheckBox)item.FindControl("checkAll");
                chkAll.Checked = false;
            }
            #endregion

        }
        TimeSpan timeSpan = TimeSpan.FromHours(totaldiffhour);
        TimeSpan time1 = TimeSpan.FromDays(totaldiffhour);

        timespentsummary(totaldiffhour);
    }

    protected void rgCalendarEvents_SortCommand(object sender, GridSortCommandEventArgs e)
    {
        DataTable dt = ViewState["GridData"] as DataTable;
        if (dt != null)
        {
            // Get the sort expression (column name)
            string sortExpression = e.SortExpression;

            // Determine the sort direction
            string sortDirection = GetSortDirection(sortExpression);

            // Sort the DataTable
            DataView dv = dt.DefaultView;
            dv.Sort = sortExpression + " " + sortDirection;

            // Rebind the grid
            rgCalendarEvents.DataSource = dv;
            rgCalendarEvents.DataBind();

            // Store the sorted DataTable back in ViewState if needed
            ViewState["GridData"] = dv.ToTable();
        }
    }

    protected void cboxSelect_CheckedChanged(object sender, EventArgs e)
    {
        totaldiffhour = 0;
        foreach (GridEditableItem item in rgCalendarEvents.MasterTableView.Items)
        {
            CheckBox chk = (CheckBox)item.FindControl("cboxSelect");
            if (chk.Checked == true)
            {
                DateTime startdate = Convert.ToDateTime(item["StartDate"].Text);
                DateTime estStartTime = TimeZoneInfo.ConvertTimeFromUtc(startdate, TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time"));

                DateTime enddate = Convert.ToDateTime(item["EndDate"].Text);
                DateTime estEndTime = TimeZoneInfo.ConvertTimeFromUtc(enddate, TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time"));

                TimeSpan difference = estEndTime - estStartTime;
                totaldiffhour = totaldiffhour + difference.TotalHours;
            }
        }
        timespentsummary(totaldiffhour);
    }

    protected void rgCalendarEvents_PageIndexChanged(object sender, GridPageChangedEventArgs e)
    {
        //rgCalendarEvents.CurrentPageIndex = e.NewPageIndex;
        //BindGrid();
    }

    protected void rgCalendarEvents_PageSizeChanged(object sender, GridPageSizeChangedEventArgs e)
    {
        //DataTable dt = ViewState["GridData"] as DataTable;

        //rgCalendarEvents.DataSource = dt;
        //rgCalendarEvents.DataBind();

    }

    
    #region button
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        DateTime _startdate = Convert.ToDateTime(ucCreatedStart.SelectedDate);
        DateTime _enddate = Convert.ToDateTime(ucEndDate.SelectedDate);

        TimeSpan _diff = _enddate - _startdate;

        if (_diff.TotalDays > 30)
        {
            DisplayMessage("Date range only for 30 days", Bootstrap.Alert.AlertType.Warning);
            return;
        }

        if (Page.IsValid)
        {
            litMessage.Text = "";
            phSearchResults.Visible = true;
            BindGrid();
        }

    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        Response.Redirect("Calendar.aspx");
    }
    protected void lbRefresh_Click(object sender, EventArgs e)
    {
        try {
            TimeSpan totalTime = TimeSpan.Zero;

            foreach (GridEditableItem item in rgCalendarEvents.MasterTableView.Items)
            {
                CheckBox chk = (CheckBox)item.FindControl("cboxSelect");
                if (chk.Checked == true)
                {
                    UserControl_DropDownList_Time ddltimspent = item.FindControl("ddlTimeSpent") as UserControl_DropDownList_Time;
                    int hours = Convert.ToInt32(ddltimspent.Hour);
                    int minutes = Convert.ToInt32(ddltimspent.Minute);

                    TimeSpan time = new TimeSpan(hours, minutes, 0);
                    totalTime = totalTime.Add(time);
                }
            }
            timespentsummary(totalTime.TotalHours);
        }
        catch (Exception ex)
        {
            DisplayMessage(ex.ToString().Trim(), Bootstrap.Alert.AlertType.Danger);
        }
    }
    #endregion

    #region private method
    private void timespentsummary(double total)
    {
        TimeSpan timeSpan = TimeSpan.FromHours(total);

        if (timeSpan.Hours > 1)
        {
            if (timeSpan.Days > 0)
            {
                if (timeSpan.Days > 1)
                {
                    ltTopTimeSpent.Text = timeSpan.Days.ToString() + " Days " + timeSpan.Hours.ToString() + " Hours " + timeSpan.Minutes.ToString() + " Minutes";
                    ltBottomTimeSpent.Text = timeSpan.Days.ToString() + " Days " + timeSpan.Hours.ToString() + " Hours " + timeSpan.Minutes.ToString() + " Minutes";
                }
                else
                {
                    ltTopTimeSpent.Text = timeSpan.Days.ToString() + " Day " + timeSpan.Hours.ToString() + " Hours " + timeSpan.Minutes.ToString() + " Minutes";
                    ltBottomTimeSpent.Text = timeSpan.Days.ToString() + " Day " + timeSpan.Hours.ToString() + " Hours " + timeSpan.Minutes.ToString() + " Minutes";
                }
            }
            else
            {
                ltTopTimeSpent.Text = timeSpan.Hours.ToString() + " Hours " + timeSpan.Minutes.ToString() + " Minutes";
                ltBottomTimeSpent.Text = timeSpan.Hours.ToString() + " Hours " + timeSpan.Minutes.ToString() + " Minutes";
            }
        }
        else
        {
            ltTopTimeSpent.Text = timeSpan.Hours.ToString() + " Hour " + timeSpan.Minutes.ToString() + " Minutes";
            ltBottomTimeSpent.Text = timeSpan.Hours.ToString() + " Hour " + timeSpan.Minutes.ToString() + " Minutes";
        }
    }
    private void DisplayMessage(Literal ltName, string message, Bootstrap.Alert.AlertType alertType, bool dismissable = true, string overrideHeader = "")
    {
        ScriptManager.RegisterClientScriptBlock(this, Page.GetType(), "ToTheTop", "ToTopOfPage();", true);
        ltName.Visible = true;
        ltName.Text = Bootstrap.Alert.GetDivText(alertType, message, dismissable, true, overrideHeader);
    }
   
    private void GetEmpEmail(int puserID, ref string email, ref int? pemployee)
    {
        UsersCollection _pusers = new UsersCollection();
        IPredicateExpression expr = new PredicateExpression();
        expr.Add(DesktopShared.HelperClasses.UsersFields.Pusers == puserID);
        _pusers.GetMulti(expr);

        if (_pusers.Count > 0)
        {
            double pcc = Convert.ToUInt32(_pusers[0].FkClientContact.ToString().Trim());
            ClientContactCollection _pclientcontact = new ClientContactCollection();
            IPredicateExpression exprpcc = new PredicateExpression();
            exprpcc.Add(DesktopShared.HelperClasses.ClientContactFields.PclientContact == pcc);
            _pclientcontact.GetMulti(exprpcc);

            if (_pclientcontact.Count > 0)
            {
                double pemp = Convert.ToUInt32(_pclientcontact[0].FkEmployee.ToString().Trim());
                EmployeeCollection _pemployee = new EmployeeCollection();
                IPredicateExpression expremp = new PredicateExpression();
                expremp.Add(DesktopShared.HelperClasses.EmployeeFields.Pemployee == pemp);
                _pemployee.GetMulti(expremp);

                if (_pemployee.Count > 0)
                {
                    email = _pemployee[0].Email.ToString().Trim();
                    pemployee = Convert.ToInt32(_pemployee[0].Pemployee.ToString().Trim());
                }

            }
        }
    }
    private string GetSortDirection(string column)
    {
        // By default, sort ascending
        string sortDirection = "ASC";

        // Check if this is the same column as the last sort
        if (ViewState["SortExpression"] != null &&
            ViewState["SortExpression"].ToString() == column)
        {
            // If same column, toggle the direction
            if (ViewState["SortDirection"] != null &&
                ViewState["SortDirection"].ToString() == "ASC")
            {
                sortDirection = "DESC";
            }
        }

        // Save new values in ViewState
        ViewState["SortExpression"] = column;
        ViewState["SortDirection"] = sortDirection;

        return sortDirection;
    }

    private DataTable GetEmployeeProjects(int employeeId)
    {
        DesktopShared.EntityClasses.EmployeeEntity _employee = DesktopShared.Employee.GetEmployeeEntity(employeeId);
        DesktopShared.User.EmployeeType ep = DesktopShared.User.GetEmployeeType(_employee);

        return DesktopShared.Employee.GetEmployeeProjects(employeeId, ep, 0, null);
    }

    private int SelectedClientId(string value)
    {
        if (!String.IsNullOrEmpty(value))
        {
            int nPos = value.IndexOf("|");

            if (nPos > 0)
                return Convert.ToInt32(value.Substring(0, nPos));
            else
                return -1;
        }
        else
            return -1;
    }

    private int SelectedProjtaskId(string value)
    {
        if (!String.IsNullOrEmpty(value))
        {
            int nPos = value.IndexOf("|");
            int lastNpos = value.LastIndexOf("|");
            lastNpos--;

            if (nPos > 0 && lastNpos > 0)
                return Convert.ToInt32(value.Substring(nPos + 1, lastNpos - nPos));
            else
                return -1;
        }
        else
            return -1;
    }

    private int SelectedProjectId(string value)
    {
        if (!String.IsNullOrEmpty(value))
        {
            int nPos = value.LastIndexOf("|");

            if (nPos > 0)
                return Convert.ToInt32(value.Substring(nPos + 1));
            else
                return -1;
        }
        else
            return -1;
    }

    private string valueproject(int projectid)
    {
        ProjtaskCollection collection = new ProjtaskCollection();
        IPredicateExpression _filters = new PredicateExpression();

        if (projectid > 0)
            _filters.Add(DesktopShared.HelperClasses.ProjtaskFields.FkProject == projectid);

        collection.GetMulti(_filters);
        if (collection.Count > 0)
        {
            string client = collection[0].FkClient.ToString().Trim();
            string projtask = collection[0].Pprojtask.ToString().Trim();

            return client + "|" + projtask + "|" + projectid;
        }

        return "";
    }

    private void DisplayMessage(string message, Bootstrap.Alert.AlertType alertType, bool dismissable = true, string overrideHeader = "")
    {
        ScriptManager.RegisterClientScriptBlock(this, Page.GetType(), "ToTheTop", "ToTopOfPage();", true);
        litMessage.Visible = true;
        litMessage.Text = Bootstrap.Alert.GetDivText(alertType, message, dismissable, true, overrideHeader);
    }
    private TimeSpan RoundUp0Minutes(TimeSpan time)
    {
        int minutes = 0;
        if (time.Minutes > 0 && time.Minutes <= 15)
            minutes = (int)Math.Round(time.TotalMinutes / 15) * 15;
        else if (time.Minutes > 16 && time.Minutes <= 30)
            minutes = (int)Math.Round(time.TotalMinutes / 30) * 30;
        else if (time.Minutes > 31 && time.Minutes <= 45)
            minutes = (int)Math.Round(time.TotalMinutes / 45) * 45;
        else if (time.Minutes > 46 && time.Minutes <= 59)
            minutes = (int)Math.Round(time.TotalMinutes / 60) * 60;
        else if (time.Minutes == 0)
            minutes = (int)Math.Round(time.TotalMinutes / 60) * 60;

        return TimeSpan.FromMinutes(minutes);
    }
    #endregion

    #region private properties
    private int? PempployeeId
    {
        get
        {
            object obj = this.ViewState["pem_id"];
            return (obj == null) ? (int?)null : (int)obj;
        }
        set { this.ViewState["pem_id"] = value; }
    }

    #endregion

    #region remark
    /*
    protected CalendarEventCollection getAllData(DateTime? fromDate, DateTime? endDate, string email)
    {
        CalendarEventCollection collection = new CalendarEventCollection();

        IPredicateExpression expression = new PredicateExpression();

        if (! String.IsNullOrEmpty(email))
            expression.Add(DesktopShared.HelperClasses.CalendarEventFields.Email == email);

        if (fromDate != null)
            expression.Add(DesktopShared.HelperClasses.CalendarEventFields.StartDate >= fromDate);

        if (endDate != null)
            expression.Add(DesktopShared.HelperClasses.CalendarEventFields.EndDate <= endDate);


        collection.GetMulti(expression);
        return collection;
    }

    

    private void BindGrid()
    {
        phSearchResults.Visible = true;
        int id = 0;
        string _email = GetEmpEmail(ddlEmployee.EmployeeId);
        rgCalendarEvents.DataSource = getAllData(ucCreatedStart.SelectedDate, ucEndDate.SelectedDate, _email);
    }
    */
    #endregion
}
