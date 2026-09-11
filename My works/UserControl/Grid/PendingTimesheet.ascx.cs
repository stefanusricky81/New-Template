using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using SD.LLBLGen.Pro.ORMSupportClasses;
using System.Collections;

public partial class UserControl_Grid_PendingTimesheet : System.Web.UI.UserControl
{
    private int _totalMinutes = 0;

    /// <summary>
    /// page load
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
    }

    #region private methods

    /// <summary>
    /// /save pending timesheet 
    /// </summary>
    /// <param name="pendingTimesheetId"></param>
    /// <param name="gridEditableItem"></param>
    /// <param name="validate"></param>
    /// <returns></returns>
    private bool SaveTimesheet(int? pendingTimesheetId, GridEditableItem gridEditableItem, bool validate = false)
    {
        TableCell _tc = null;

        //date
        _tc = gridEditableItem["Date"];
        UserControl_DateTime_DatePicker ucDate = _tc.FindControl("ucDate") as UserControl_DateTime_DatePicker;

        //client
        _tc = gridEditableItem["ClientName"];
        UserControl_DropDownList_Client ddlClient = _tc.FindControl("ddlClient") as UserControl_DropDownList_Client;

        //project
        _tc = gridEditableItem["ProjectName"];
        UserControl_DropDownList_ProjectTask ddlProjectTask = _tc.FindControl("ddlProjectTask") as UserControl_DropDownList_ProjectTask;

        //activity 
        _tc = gridEditableItem["TaskActionName"];
        UserControl_DropDownList_TaskAction ddlTaskAction = _tc.FindControl("ddlTaskAction") as UserControl_DropDownList_TaskAction;

        //time
        _tc = gridEditableItem["TimeSpentMinutes"];
        TextBox txtTimeSpent = _tc.FindControl("txtTimeSpent") as TextBox;
        RangeValidator rvTimeSpent = _tc.FindControl("rvTimeSpent") as RangeValidator;

        //internal notes
        _tc = gridEditableItem["InternalNotes"];
        TextBox txtInternalNotes = (_tc.FindControl("txtInternalNotes") as TextBox);

        //description
        _tc = gridEditableItem["Description"];
        TextBox txtDescription = (_tc.FindControl("txtDescription") as TextBox);

        #region validate for text changed event

        if (validate && (DataType == PendingType.Consolidated))
        {
            if (!ucDate.SelectedDate.HasValue)
            {
                DisplayMessage("Date is required", Bootstrap.Alert.AlertType.Danger);
                return false;
            }
            if (ddlProjectTask.SelectedProjtaskId < 1)
            {
                DisplayMessage("Project is required", Bootstrap.Alert.AlertType.Danger);
                return false;
            }
            if (ddlTaskAction.TaskActionId < 1)
            {
                DisplayMessage("Activity is required", Bootstrap.Alert.AlertType.Danger);
                return false;
            }
            if (String.IsNullOrWhiteSpace(txtTimeSpent.Text))
            {
                DisplayMessage("Time is required", Bootstrap.Alert.AlertType.Danger);
                return false;
            }
            int _time = 0;
            if (!int.TryParse(txtTimeSpent.Text, out _time))
            {
                DisplayMessage("Invalid Time", Bootstrap.Alert.AlertType.Danger);
                return false;
            }
            int _min = DataType == PendingType.Open ? 5 : 15;
            if ((_time < _min) || (_time > 1000))
            {
                DisplayMessage("Invalid Time", Bootstrap.Alert.AlertType.Danger);
                return false;
            }
            if (String.IsNullOrWhiteSpace(txtDescription.Text))
            {
                DisplayMessage("Description is required", Bootstrap.Alert.AlertType.Danger);
                return false;
            }
        }

        #endregion

        DesktopShared.EntityClasses.PendingTimesheetEntity objPendingTimesheet = null;
        DateTime _auditDate = DateTime.Now;
        if (!pendingTimesheetId.HasValue)
        {
            objPendingTimesheet = new DesktopShared.EntityClasses.PendingTimesheetEntity();
            objPendingTimesheet.Created = _auditDate;
            objPendingTimesheet.EmployeeId = DesktopShared.User.EmployeeID;
        }
        else
            objPendingTimesheet = new DesktopShared.EntityClasses.PendingTimesheetEntity(pendingTimesheetId.Value);

        objPendingTimesheet.Date = ucDate.SelectedDate;
        if (ddlProjectTask.SelectedProjectId > 0)
        {
            objPendingTimesheet.ClientId = ddlProjectTask.SelectedClientId;
            objPendingTimesheet.ProjTaskId = ddlProjectTask.SelectedProjtaskId;
            objPendingTimesheet.ProjectId = ddlProjectTask.SelectedProjectId;
        }
        if (ddlTaskAction.TaskActionId > 0)
            objPendingTimesheet.TaskActionId = ddlTaskAction.TaskActionId;
        if (String.IsNullOrWhiteSpace(txtTimeSpent.Text))
            objPendingTimesheet.TimeSpentMinutes = null;
        else
            objPendingTimesheet.TimeSpentMinutes = DesktopShared.Utility.Time.RoundUp(Convert.ToInt32(txtTimeSpent.Text), DataType == PendingType.Open ? 5 : 15);
        objPendingTimesheet.InternalNotes = txtInternalNotes.Text.Trim();
        objPendingTimesheet.Description = txtDescription.Text.Trim();
        objPendingTimesheet.LastUpdated = _auditDate;
        objPendingTimesheet.Save();

        return true;
    }

    /// <summary>
    /// display message
    /// </summary>
    /// <param name="message"></param>
    /// <param name="alertType"></param>
    /// <param name="overrideHeader"></param>
    private void DisplayMessage(string message, Bootstrap.Alert.AlertType alertType, bool dismissable = true, string overrideHeader = "")
    {
        ScriptManager.RegisterClientScriptBlock(this, Page.GetType(), "ToTheTop", "ToTopOfPage();", true);
        litMessage.Visible = true;
        litMessage.Text = Bootstrap.Alert.GetDivText(alertType, message, dismissable, true, overrideHeader);
    }

    /// <summary>
    /// format for display of hours/mins
    /// </summary>
    /// <param name="timeSpent"></param>
    /// <returns></returns>
    private string GetTimeDisplay(int timeSpent)
    {
        string _timeDisplay = "";
        TimeSpan _tsTimeSpent = TimeSpan.FromMinutes(timeSpent);
        if (timeSpent > 59)
        {
            int _hours = _tsTimeSpent.Hours;
            _timeDisplay = String.Format("{0} hour{1} ", _hours, _hours > 1 ? "s" : "");
        }

        int _minutes = _tsTimeSpent.Minutes;
        if (_minutes > 0)
            _timeDisplay += String.Format("{0} min", _minutes);

        return _timeDisplay;
    }

    #endregion

    #region protected events

    /// <summary>
    /// description text box on text changed
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void txtDescription_TextChanged(object sender, EventArgs e)
    {
        Page.Validate("vgTimesheet");
        if (Page.IsValid)
        {
            TextBox txtDescription = sender as TextBox;
            GridEditableItem _editedItem = txtDescription.NamingContainer as GridEditableItem;
            bool _success = SaveTimesheet(null, _editedItem, true);
            RebindGrid(_success);
            _totalMinutes = 0;
            if (_success)
            {
                if (InsertCompleted != null)
                    InsertCompleted(sender, e);
            }
        }
    }

    /// <summary>
    /// description text box on text changed
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void txtDescriptionEdit_TextChanged(object sender, EventArgs e)
    {
        Page.Validate("vgTimesheet");
        if (Page.IsValid)
        {
            TextBox txtDescription = sender as TextBox;
            GridEditableItem _editedItem = txtDescription.NamingContainer as GridEditableItem;
            int _id = int.Parse(_editedItem.OwnerTableView.DataKeyValues[_editedItem.ItemIndex]["Id"].ToString());
            bool _success = SaveTimesheet(_id, _editedItem, true);
            RebindGrid(_success);
            _totalMinutes = 0;
            if (_success)
            {
                if (UpdateCompleted != null)
                    UpdateCompleted(sender, e);
            }
        }
    }

    /// <summary>
    /// submit edit button on click
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSubmitEdit_Click(object sender, EventArgs e)
    {
        LinkButton btnSubmit = sender as LinkButton;
        GridEditableItem _editedItem = btnSubmit.NamingContainer as GridEditableItem;
        int _id = int.Parse(_editedItem.OwnerTableView.DataKeyValues[_editedItem.ItemIndex]["Id"].ToString());
        SaveTimesheet(_id, _editedItem);
    }

    #region rad grid events

    /// <summary>
    /// grid on need data source
    /// </summary>
    /// <param name="source"></param>
    /// <param name="e"></param>
    protected void rgTimesheet_NeedDataSource(object source, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        if (DataType == PendingType.Open)
            rgTimesheet.DataSource = DesktopShared.Timesheet.Pending.GetOpen(DesktopShared.User.EmployeeID);
        else if (DataType == PendingType.Consolidated)
            rgTimesheet.DataSource = DesktopShared.Timesheet.Pending.Consolidated.GetOpen(DesktopShared.User.EmployeeID);
        else
            rgTimesheet.DataSource = null;
    }

    /// <summary>
    /// grid on item databound
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rgTimesheet_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        #region GridEditFormItem and IsInEditMode

        if (e.Item is GridEditableItem && e.Item.IsInEditMode)
        {
            GridEditableItem _gei = e.Item as GridEditableItem;

            #region user control

            if (UserControlForInsert == true)
            {
                /*if (!e.Item.OwnerTableView.IsItemInserted) //edit/view
                {
                DesktopShared.TypedListClasses.PendingTimesheetsRow objPendingTimesheet = (DesktopShared.TypedListClasses.PendingTimesheetsRow)((DataRowView)e.Item.DataItem).Row;
                ucEditForm.LoadValues(objPendingTimesheet);
                }*/
                UserControl_Grid_EditForm_PendingTimesheetEdit ucEditForm = e.Item.FindControl(GridEditFormItem.EditFormUserControlID) as UserControl_Grid_EditForm_PendingTimesheetEdit;
                ucEditForm.IsConsolidated = DataType == PendingType.Consolidated;
                if (e.Item.OwnerTableView.IsItemInserted)
                {
                    ucEditForm.LoadValues(null);
                }
            }

            #endregion

            #region inplace 

            else
            {
                TableCell _tc = null;
                bool _isRequired = (DataType == PendingType.Consolidated);
                //action buttons
                _tc = _gei["ActionColumn"];
                (_tc.FindControl("btnEdit") as LinkButton).Visible = false;
                (_tc.FindControl("btnDelete") as LinkButton).Visible = false;
                (_tc.FindControl("btnSubmitEdit") as LinkButton).Visible = true;
                (_tc.FindControl("btnCancel") as LinkButton).Visible = true;

                //date
                _tc = _gei["Date"];
                UserControl_DateTime_DatePicker ucDate = _tc.FindControl("ucDate") as UserControl_DateTime_DatePicker;
                ucDate.IsRequired = _isRequired;

                //client
                //_tc = _gei["ClientName"];
                //UserControl_DropDownList_Client ddlClient = _tc.FindControl("ddlClient") as UserControl_DropDownList_Client;
                //<ddl:Client ID="ddlClient" runat="server" IsRequired="true" ValidationGroup="vgTimesheet" DisplayDefaultValue="true" DefaultText="" DefaultValue="" CssClass="form-control" DisplayChosenScript="false" />

                //project
                _tc = _gei["ProjectName"];
                UserControl_DropDownList_ProjectTask ddlProjectTask = _tc.FindControl("ddlProjectTask") as UserControl_DropDownList_ProjectTask;
                ddlProjectTask.IsRequired = _isRequired;
                ddlProjectTask.Focus();

                //activity 
                _tc = _gei["TaskActionName"];
                UserControl_DropDownList_TaskAction ddlTaskAction = _tc.FindControl("ddlTaskAction") as UserControl_DropDownList_TaskAction;
                ddlTaskAction.IsRequired = _isRequired;

                //time
                _tc = _gei["TimeSpentMinutes"];
                TextBox txtTimeSpent = _tc.FindControl("txtTimeSpent") as TextBox;
                (_tc.FindControl("rfvTimeSpent") as RequiredFieldValidator).Visible = _isRequired;

                //internal notes
                _tc = _gei["InternalNotes"];
                TextBox txtInternalNotes = (_tc.FindControl("txtInternalNotes") as TextBox);

                //description
                _tc = _gei["Description"];
                TextBox txtDescription = (_tc.FindControl("txtDescription") as TextBox);

                #region edit existing

                if (!e.Item.OwnerTableView.IsItemInserted)
                {
                    DesktopShared.TypedListClasses.PendingTimesheetsRow objPendingTimesheet = (DesktopShared.TypedListClasses.PendingTimesheetsRow)((DataRowView)e.Item.DataItem).Row;

                    ucDate.SelectedDate = objPendingTimesheet.IsDateNull() ? (DateTime?)null : objPendingTimesheet.Date;
                    ddlProjectTask.SelectedProjtaskId = objPendingTimesheet.ProjTaskId;
                    ddlTaskAction.TaskActionId = objPendingTimesheet.TaskActionId;
                    txtTimeSpent.Text = objPendingTimesheet.IsTimeSpentMinutesNull() ? "" : objPendingTimesheet.TimeSpentMinutes.ToString();
                    txtInternalNotes.Text = objPendingTimesheet.InternalNotes.Trim();
                    txtDescription.Text = objPendingTimesheet.Description.Trim();
                }

                #endregion

                #region insert new

                else
                {
                    (_tc.FindControl("btnSubmitEdit") as LinkButton).CommandName = "PerformInsert";
                    (_tc.FindControl("btnCancel") as LinkButton).Visible = false;
                    DateTime? _lastDate = DesktopShared.Timesheet.Pending.GetLastDate(DesktopShared.User.EmployeeID);
                    ucDate.SelectedDate = _lastDate.HasValue ? _lastDate.Value : DateTime.Now;
                    ddlProjectTask.Populate();
                    ddlTaskAction.TaskActionId = DesktopShared.User.DefaultActivityId;
                    ddlTaskAction.Populate();
                }

                #endregion
            }

            #endregion
        }

        #endregion

        #region Item or AlternatingItem

        else if (e.Item.ItemType == GridItemType.Item || e.Item.ItemType == GridItemType.AlternatingItem)
        {
            GridEditableItem _gdi = e.Item as GridEditableItem;
            DesktopShared.TypedListClasses.PendingTimesheetsRow objPendingTimesheet = (DesktopShared.TypedListClasses.PendingTimesheetsRow)((DataRowView)e.Item.DataItem).Row;

            TelerikHelper.AddLabelToCell(objPendingTimesheet.IsDateNull() ? "" : objPendingTimesheet.Date.ToString("MM/dd/yy"), _gdi["Date"]);
            TelerikHelper.AddLabelToCell(objPendingTimesheet.ClientName, _gdi["ClientName"]);
            TelerikHelper.AddLabelToCell(objPendingTimesheet.ProjectName, _gdi["ProjectName"]);
            TelerikHelper.AddLabelToCell(objPendingTimesheet.TaskActionName, _gdi["TaskActionName"]);
            string _internalNotes = objPendingTimesheet.InternalNotes.Trim();
            string _gridCellInternalNotes = _internalNotes.Replace(Environment.NewLine, "<br />").Replace("\r\n", "<br />").Replace("\n", "<br />").Replace("\r", "<br />");
            TelerikHelper.AddLabelToCell(_gridCellInternalNotes, _internalNotes, _gdi["InternalNotes"]);
            string _description = objPendingTimesheet.Description.Trim();
            string _gridCellDescription = _description.Replace(Environment.NewLine, "<br />").Replace("\r\n", "<br />").Replace("\n", "<br />").Replace("\r", "<br />");
            TelerikHelper.AddLabelToCell(_gridCellDescription, _description, _gdi["Description"]);
            TelerikHelper.AddLabelToCell(GetTimeDisplay(objPendingTimesheet.TimeSpentMinutes), _gdi["TimeSpentMinutes"]);
            _totalMinutes += objPendingTimesheet.TimeSpentMinutes;
        }

        #endregion

        #region footer

        else if (e.Item is GridFooterItem)
        {
            GridFooterItem footerItem = e.Item as GridFooterItem;
            footerItem["TimeSpentMinutes"].Text = String.Format("<strong>{0}</strong>", GetTimeDisplay(_totalMinutes));
        }

        #endregion
    }

    /// <summary>
    /// grid on update command
    /// </summary>
    /// <param name="source"></param>
    /// <param name="e"></param>
    protected void rgTimesheet_UpdateCommand(object source, GridCommandEventArgs e)
    {
        GridEditableItem _editedItem = (GridEditableItem)e.Item;
        int _id = int.Parse(_editedItem.OwnerTableView.DataKeyValues[_editedItem.ItemIndex]["Id"].ToString());
        SaveTimesheet(_id, _editedItem);

        RebindGrid();
        _totalMinutes = 0;
        if (UpdateCompleted != null)
            UpdateCompleted(source, e);
    }

    /// <summary>
    /// grid on insert command
    /// </summary>
    /// <param name="source"></param>
    /// <param name="e"></param>
    protected void rgTimesheet_InsertCommand(object source, GridCommandEventArgs e)
    {
        if (UserControlForInsert)
        {
            UserControl_Grid_EditForm_PendingTimesheetEdit ucEditForm = e.Item.FindControl(GridEditFormItem.EditFormUserControlID) as UserControl_Grid_EditForm_PendingTimesheetEdit;
            ucEditForm.SaveValues(null);
            e.Canceled = true;
            if (e.CommandArgument.ToString() == "SaveAdd")
            {
                RebindGrid();
                _totalMinutes = 0;
                RebindGrid();
                return;
            }
        }
        else
        {
            GridEditableItem _editedItem = (GridEditableItem)e.Item;
            SaveTimesheet(null, _editedItem);
        }

        UserControlForInsert = false;
        e.Item.OwnerTableView.EditMode = GridEditMode.InPlace;

        RebindGrid();
        _totalMinutes = 0;
        if (InsertCompleted != null)
            InsertCompleted(source, e);

        rgTimesheet.MasterTableView.IsItemInserted = true;
        RebindGrid();
    }

    /// <summary>
    /// grid on delete command
    /// </summary>
    /// <param name="source"></param>
    /// <param name="e"></param>
    protected void rgTimesheet_DeleteCommand(object source, GridCommandEventArgs e)
    {
        int _id = Convert.ToInt32((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["Id"]);
        DesktopShared.EntityClasses.PendingTimesheetEntity objPendingTimesheet = new DesktopShared.EntityClasses.PendingTimesheetEntity(_id);
        objPendingTimesheet.Delete();
        RebindGrid();
        _totalMinutes = 0;
        if (DeleteCompleted != null)
            DeleteCompleted(source, e);
    }

    /// <summary>
    /// grid on pre render
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rgTimesheet_PreRender(object sender, EventArgs e)
    {
        bool _isAddEdit = rgTimesheet.EditItems.Count > 0 || rgTimesheet.MasterTableView.IsItemInserted;
        int _descriptionWidth = 14;
        if (DataType != PendingType.Consolidated)
        {
            _descriptionWidth = 16;
            rgTimesheet.MasterTableView.Columns[0].Visible = false;
        }

        bool _usePercentage = true;
        if (DesktopShared.Utility.BrowserHelper.IsTabletOrSmartPhone(this.Request))
        {
            #region hide grid if adding via user control on tablet or smart phone

            if (rgTimesheet.MasterTableView.IsItemInserted && UserControlForInsert)
            {
                //add link button 
                GridCommandItem commandItem = null;
                if (rgTimesheet.MasterTableView.Items.Count > 0)
                {
                    commandItem = (GridCommandItem)rgTimesheet.MasterTableView.GetItems(GridItemType.CommandItem)[0];
                    commandItem.Visible = true;
                }

                foreach (GridDataItem item in rgTimesheet.MasterTableView.Items)
                {
                    item.Visible = false; //hide all rows not being edited
                }

                rgTimesheet.ShowHeader = false; //hide grid header 
                rgTimesheet.ShowFooter = false; //hide grid footer

                //hide grid pager
                rgTimesheet.PagerStyle.AlwaysVisible = false;
                rgTimesheet.PagerStyle.Visible = false;

                //hide add ssl certificate link button
                if (commandItem != null)
                    commandItem.Visible = false;
            }

            #endregion

            else
            {
                rgTimesheet.MasterTableView.Columns[0].HeaderStyle.Width = Unit.Pixel(80);
                rgTimesheet.MasterTableView.Columns[1].HeaderStyle.Width = Unit.Pixel(60);
                rgTimesheet.MasterTableView.Columns[2].HeaderStyle.Width = Unit.Pixel(100);
                rgTimesheet.MasterTableView.Columns[3].HeaderStyle.Width = Unit.Pixel(200);
                rgTimesheet.MasterTableView.Columns[4].HeaderStyle.Width = Unit.Pixel(200);
                rgTimesheet.MasterTableView.Columns[5].HeaderStyle.Width = Unit.Pixel(200);
                rgTimesheet.MasterTableView.Columns[6].HeaderStyle.Width = Unit.Pixel(115);
                rgTimesheet.MasterTableView.Columns[7].HeaderStyle.Width = Unit.Pixel(400);
                rgTimesheet.MasterTableView.Columns[8].HeaderStyle.Width = Unit.Pixel(400);
                _usePercentage = false;
            }
        }
        
        if (_usePercentage)
        {
            rgTimesheet.MasterTableView.Columns[0].HeaderStyle.Width = Unit.Percentage(4);
            rgTimesheet.MasterTableView.Columns[1].HeaderStyle.Width = Unit.Percentage(6);
            rgTimesheet.MasterTableView.Columns[2].HeaderStyle.Width = Unit.Percentage(12);
            rgTimesheet.MasterTableView.Columns[3].HeaderStyle.Width = Unit.Percentage(12);
            rgTimesheet.MasterTableView.Columns[4].HeaderStyle.Width = Unit.Percentage(14);
            rgTimesheet.MasterTableView.Columns[5].HeaderStyle.Width = Unit.Percentage(12);
            rgTimesheet.MasterTableView.Columns[6].HeaderStyle.Width = Unit.Percentage(12);
            rgTimesheet.MasterTableView.Columns[7].HeaderStyle.Width = Unit.Percentage(_descriptionWidth);
            rgTimesheet.MasterTableView.Columns[8].HeaderStyle.Width = Unit.Percentage(_descriptionWidth);
        }
    }

    /// <summary>
    /// grid on item created
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rgTimesheet_ItemCreated(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridCommandItem)
        {
            if (DataType != PendingType.Open)
            {
                GridCommandItem _gci = (GridCommandItem)e.Item;
                LinkButton btnAdd = (LinkButton)_gci.FindControl("btnAdd");
                btnAdd.Visible = false;
            }
        }

        //if (e.Item is GridDataItem)
        //{
        //    GridDataItem item = (GridDataItem)e.Item;
        //    UserControl_DropDownList_SmartClient ucClient = (UserControl_DropDownList_SmartClient)item.FindControl("ddlSmartClient");
        //    ucClient.ClientChanged += new EventHandler(ucClient_ClientChanged);
        //    //ucClient.AutoPostback = true;
        //    //ucClient.ClientChanged += new EventHandler(ucClient_ClientChanged);
        //}
        if (e.Item is GridEditableItem && e.Item.IsInEditMode)
        {
            GridEditableItem editItem = (GridEditableItem)e.Item;

            var smartClient = editItem.FindControl("ddlSmartClient") as UserControl_DropDownList_SmartClient;
            if (smartClient != null)
            {
                smartClient.ClientChanged += new EventHandler(ucClient_ClientChanged);
            }
        }
    }

    /// <summary>
    /// grid on item command
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rgTimesheet_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName == RadGrid.EditCommandName)
        {
            UserControlForInsert = false;
            e.Item.OwnerTableView.IsItemInserted = false;
        }
        else if ((e.CommandName == RadGrid.CancelCommandName))
        {
            UserControlForInsert = false;
            e.Item.OwnerTableView.EditMode = GridEditMode.InPlace;
            if (DataType == PendingType.Open)
            {
                e.Canceled = true;
                e.Item.OwnerTableView.IsItemInserted = true;
                e.Item.OwnerTableView.InsertItem();
                RebindGrid();
            }
        }
        else if (e.CommandName == "AddViaUserControl")
        {
            UserControlForInsert = true;
            e.Canceled = true;
            rgTimesheet.EditIndexes.Clear();
            e.Item.OwnerTableView.EditMode = GridEditMode.EditForms;
            e.Item.OwnerTableView.EditFormSettings.EditFormType = GridEditFormType.WebUserControl;
            e.Item.OwnerTableView.EditFormSettings.UserControlName = "/UserControl/Grid/EditForm/PendingTimesheetEdit.ascx";
            e.Item.OwnerTableView.InsertItem();
        }
    }

    private void ucClient_ClientChanged(object sender, EventArgs e)
    {
        var uc = (UserControl_DropDownList_SmartClient)sender;
        GridDataItem row = (GridDataItem)uc.NamingContainer;
        UserControl_DropDownList_ProjectTask ddlprojtask = (UserControl_DropDownList_ProjectTask)row.FindControl("ddlProjectTask");
        ddlprojtask.SelectedClientId = uc.SelectedClientId;
    }

    #endregion

    #endregion

    #region public methods

    /// <summary>
    /// rebind grid
    /// </summary>
    public void RebindGrid(bool clearMessage = true)
    {
        _totalMinutes = 0;
        if (clearMessage)
            litMessage.Text = "";
        rgTimesheet.Visible = true;
        rgTimesheet.EditIndexes.Clear();
        rgTimesheet.DataSource = null;
        rgTimesheet.MasterTableView.IsItemInserted = ((DataType == PendingType.Open) && (rgTimesheet.EditItems.Count == 0));
        rgTimesheet.Rebind();
    }

    /// <summary>
    /// clear grid
    /// </summary>
    public void ClearGrid()
    {
        _totalMinutes = 0;
        rgTimesheet.CurrentPageIndex = 0;
        rgTimesheet.EditIndexes.Clear();
        rgTimesheet.Visible = false;
    }

    #endregion

    #region public events

    public event EventHandler InsertCompleted;
    public event EventHandler UpdateCompleted;
    public event EventHandler DeleteCompleted;

    #endregion

    #region public enum

    /// <summary>
    /// enum for pending type
    /// </summary>
    public enum PendingType
    {
        Consolidated,
        Open,
        Unknown
    }

    #endregion

    #region public properties

    /// <summary>
    /// get/set pending data type
    /// </summary>
    public PendingType DataType
    {
        get
        {
            object obj = this.ViewState["dt"];
            return (obj == null) ? PendingType.Unknown : (PendingType)obj;
        }
        set { this.ViewState["dt"] = value; }
    }

    /// <summary>
    /// get record count
    /// </summary>
    public int RecordCount
    {
        get { return rgTimesheet.Items.Count;  }
    }

    /// <summary>
    /// get list of selected ids
    /// </summary>
    public List<int> SelectedIds
    {
        get
        {
            List<int> _ids = new List<int>();
            foreach (GridDataItem _gdi in rgTimesheet.MasterTableView.Items)
            {
                CheckBox chkSubmit = (CheckBox)_gdi.FindControl("chkSubmit");
                if (chkSubmit.Checked)
                    _ids.Add(Convert.ToInt32(_gdi.GetDataKeyValue("Id")));
            }

            return _ids;
        }
    }

    #endregion

    #region private properties

    /// <summary>
    /// get/set use user control for insert
    /// </summary>
    private bool UserControlForInsert
    {
        get
        {
            object obj = this.ViewState["ucfi_pt"];
            return (obj == null) ? false : (bool)obj;
        }
        set { this.ViewState["ucfi_pt"] = value; }
    }

    #endregion
}
