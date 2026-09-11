using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using SD.LLBLGen.Pro.ORMSupportClasses;

public partial class UserControl_Grid_EditForm_PendingTimesheetEdit : System.Web.UI.UserControl
{
    /// <summary>
    /// page load
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        //UserControl_DropDownList_SmartClient ucClient = (UserControl_DropDownList_SmartClient)ddlSmartClient.FindControl("ddlSmartClient");
        //ucClient.AutoPostback = true;
        //ucClient.ClientChanged += new EventHandler(ucClient_ClientChanged);
        ddlSmartClient.ClientChanged += new EventHandler(ucClient_ClientChanged);


        //Telerik.Web.UI.RadComboBox rcbClient = (Telerik.Web.UI.RadComboBox)ddlSmartClient.FindControl("ddlSmartClient");
        //if (rcbClient != null)
        //{
        //    rcbClient.AutoPostBack = true;
        //    rcbClient.SelectedIndexChanged += new RadComboBoxSelectedIndexChangedEventHandler(ucClient_ClientChanged);
        //}

        ConfigureForDevice();
        PopulateDropDownLists();
    }

    #region private methods
    private void ucClient_ClientChanged(object sender, EventArgs e)
    {
        //UserControl_DropDownList_ProjectTask ddlprojtask = (UserControl_DropDownList_ProjectTask)ddlProjectTask.FindControl("ddlProjectTask");
        ddlProjectTask.SelectedClientId = ddlSmartClient.SelectedClientId;
    }

    /// <summary>
    /// populate hour and minute drop down lists
    /// </summary>
    private void PopulateDropDownLists()
    {
        if (ddlTimeMinute.Items.Count == 0)
        {
            ddlTimeHour.Items.Insert(0, new ListItem("", ""));
            for (int i = 0; i <= 12; i++)
            {
                string _val = i.ToString();
                string _display = String.Format("{0} hour{1}", _val, i == 1 ? "" : "s");
                ddlTimeHour.Items.Insert(i + 1, new ListItem(_display, _val));
            }
        }

        if (ddlTimeMinute.Items.Count == 0)
        {
            ddlTimeMinute.Items.Insert(0, new ListItem("", ""));
            int _count = 0;
            for (int i = 0; i < 60; i++)
            {
                int _operator = IsConsolidated ? 15 : 5;
                if (i % _operator != 0)
                    continue;

                _count++;
                string _val = i.ToString();
                string _display = String.Format("{0} min{1}", _val, i == 1 ? "" : "s");
                ddlTimeMinute.Items.Insert(_count, new ListItem(_display, _val));
            }
        }
    }

    /// <summary>
    /// configure for device
    /// </summary>
    private void ConfigureForDevice()
    {
        string _ddlClass = "form-control";
        bool _displayChosen = false;
        /*if (DesktopShared.Utility.BrowserHelper.IsTabletOrSmartPhone(this.Request))
        {
            _ddlClass = "form-control";
            _displayChosen = false;
        }
        else
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append("<script type='text/javascript'>");
            sb.Append("$(document).ready(function() {");
            sb.Append(String.Format("SetChosenTimeHour_{0}();", this.ClientID));
            sb.Append(String.Format("Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(SetChosenTimeHour_{0})", this.ClientID));
            sb.Append("});");
            sb.Append(String.Format("function SetChosenTimeHour_{0}(sender, args) {{", this.ClientID));
            sb.Append(String.Format("$('#{0}').chosen({{", ddlTimeHour.ClientID));
            sb.Append("allow_single_deselect: true,");
            sb.Append("placeholder_text_single: \"Select Hours ...\",");
            sb.Append("width: \"100%\"");
            sb.Append("});");
            sb.Append("}");
            sb.Append("</script>");
            litChosenJsTimeHour.Text = sb.ToString();

            sb = new System.Text.StringBuilder();
            sb.Append("<script type='text/javascript'>");
            sb.Append("$(document).ready(function() {");
            sb.Append(String.Format("SetChosenTimeMinute_{0}();", this.ClientID));
            sb.Append(String.Format("Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(SetChosenTimeMinute_{0})", this.ClientID));
            sb.Append("});");
            sb.Append(String.Format("function SetChosenTimeMinute_{0}(sender, args) {{", this.ClientID));
            sb.Append(String.Format("$('#{0}').chosen({{", ddlTimeMinute.ClientID));
            sb.Append("allow_single_deselect: true,");
            sb.Append("placeholder_text_single: \"Select Minutes ...\",");
            sb.Append("width: \"100%\"");
            sb.Append("});");
            sb.Append("}");
            sb.Append("</script>");
            litChosenJsTimeMinute.Text = sb.ToString();
        }*/

        //ddlClient.CssClass = _ddlClass;
        //ddlClient.DisplayChosenScript = _displayChosen;
        ddlProjectTask.CssClass = _ddlClass;
        ddlProjectTask.DisplayChosenScript = _displayChosen;
        ddlTaskAction.CssClass = _ddlClass;
        ddlTaskAction.DisplayChosenScript = _displayChosen;
        ddlTimeHour.CssClass = _ddlClass;
        ddlTimeMinute.CssClass = _ddlClass;
    }

    #endregion

    #region protected events

    #region custom validators

    /// <summary>
    /// validate time entered
    /// </summary>
    /// <param name="source"></param>
    /// <param name="args"></param>
    protected void cvTime_ServerValidate(object source, ServerValidateEventArgs args)
    {
        args.IsValid = Time > 0;
    }

    #endregion

    #endregion

    #region public methods

    /// <summary>
    /// load timesheet values
    /// </summary>
    /// <param name="objPendingTimesheet"></param>
    public void LoadValues(DesktopShared.TypedListClasses.PendingTimesheetsRow objPendingTimesheet)
    {
        ucDate.Focus();
        btnAdd.Visible = objPendingTimesheet == null;
        btnEdit.Visible = !btnAdd.Visible;

        //ddlClient.PopulateDropDownList();
        ddlProjectTask.Populate(DesktopShared.User.EmployeeID);
        ddlTaskAction.Populate();

        #region existing record

        if (objPendingTimesheet != null)
        {
            if (IsConsolidated)
            {
                ddlTimeHour.Items.Clear();
                ddlTimeMinute.Items.Clear();
                PopulateDropDownLists();
            }

            litHeader.Text = "Edit Pending Timesheet";
            ucDate.SelectedDate = objPendingTimesheet.Date;
            //ddlClient.ClientId = objPendingTimesheet.ClientId;
            ddlProjectTask.SelectedProjtaskId = objPendingTimesheet.ProjTaskId;
            ddlTaskAction.TaskActionId = objPendingTimesheet.TaskActionId;
            txtInternalNotes.Text = objPendingTimesheet.InternalNotes.Trim();
            txtDescription.Text = objPendingTimesheet.Description.Trim();

            #region  time spent
            if (objPendingTimesheet.TimeSpentMinutes > 0)
            {
                ListItem _li = null;
                TimeSpan _tsTimeSpent = TimeSpan.FromMinutes(objPendingTimesheet.TimeSpentMinutes);
                int _hours = _tsTimeSpent.Hours;
                int _mins = _tsTimeSpent.Minutes;

                if (_hours > 0)
                {
                    _li = ddlTimeHour.Items.FindByValue(_hours.ToString());
                    if (_li != null)
                        _li.Selected = true;
                }
                if (_mins > 0)
                {
                    _li = ddlTimeMinute.Items.FindByValue(_mins.ToString());
                    if (_li != null)
                        _li.Selected = true;
                }
            }
            #endregion
        }

        #endregion

        #region new record -> set defaults

        else
        {
            DateTime? _lastDate = DesktopShared.Timesheet.Pending.GetLastDate(DesktopShared.User.EmployeeID);
            ucDate.SelectedDate = _lastDate.HasValue ? _lastDate.Value : DateTime.Now;
            ddlTaskAction.TaskActionId = DesktopShared.User.DefaultActivityId;
        }

        #endregion

        #region hide required

        ucDate.IsRequired = false;
        ddlProjectTask.IsRequired = false;
        ddlTaskAction.IsRequired = false;
        cvTime.Visible = false;

        #endregion
    }

    /// <summary>
    /// save timesheet values
    /// </summary>
    /// <param name="pendingTimesheetID"></param>
    public void SaveValues(int? pendingTimesheetID)
    {
        DesktopShared.EntityClasses.PendingTimesheetEntity objPendingTimesheet = null;
        DateTime _auditDate = DateTime.Now;
        if (!pendingTimesheetID.HasValue)
        {
            objPendingTimesheet = new DesktopShared.EntityClasses.PendingTimesheetEntity();
            objPendingTimesheet.Created = _auditDate;
            objPendingTimesheet.EmployeeId = DesktopShared.User.EmployeeID;
        }
        else
            objPendingTimesheet = new DesktopShared.EntityClasses.PendingTimesheetEntity(pendingTimesheetID.Value);

        objPendingTimesheet.Date = ucDate.SelectedDate;
        if (ddlProjectTask.SelectedProjtaskId > 0)
        {
            objPendingTimesheet.ClientId = ddlProjectTask.SelectedClientId;
            objPendingTimesheet.ProjTaskId = ddlProjectTask.SelectedProjtaskId;
            objPendingTimesheet.ProjectId = ddlProjectTask.SelectedProjectId;
        }
        if (ddlTaskAction.TaskActionId > 0)
            objPendingTimesheet.TaskActionId = ddlTaskAction.TaskActionId;
        if (Time > 0)
            objPendingTimesheet.TimeSpentMinutes = Time;
        objPendingTimesheet.InternalNotes = txtInternalNotes.Text.Trim();
        objPendingTimesheet.Description = txtDescription.Text.Trim();
        objPendingTimesheet.LastUpdated = _auditDate;
        objPendingTimesheet.Save();
    }

    #endregion

    #region private properties

    /// <summary>
    /// get time spent
    /// </summary>
    private int Time
    {
        get
        {
            int _retValue = 0;
            string _selectedValue = "";
            int _parsedValue = 0;

            _selectedValue = ddlTimeHour.SelectedValue.Trim();
            if (!String.IsNullOrWhiteSpace(_selectedValue))
            {
                if (int.TryParse(_selectedValue, out _parsedValue))
                    _retValue = _parsedValue * 60;
            }
            _selectedValue = ddlTimeMinute.SelectedValue.Trim();
            if (!String.IsNullOrWhiteSpace(_selectedValue))
            {
                if (int.TryParse(_selectedValue, out _parsedValue))
                    _retValue += _parsedValue;
            }

            return _retValue;
        }
    }

    #endregion

    #region public properites

    /// <summary>
    /// get/set is consolidated entry
    /// </summary>
    public bool IsConsolidated
    {
        get
        {
            object obj = this.ViewState["ic_pte"];
            return (obj == null) ? false : (bool)obj;
        }
        set { this.ViewState["ic_pte"] = value; }
    }

    #endregion
}
