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

public partial class UserControl_Grid_Timesheet : System.Web.UI.UserControl
{
    private double _totalHours = 0;
    bool _isExport = false;

    protected void Page_Load(object sender, EventArgs e)
    {
        //records per page event handler
        DropDownList ddlRecordsPerPage = ucRecordsPerPage.GetDropDownList();
        ddlRecordsPerPage.SelectedIndexChanged += new EventHandler(ddlRecordsPerPage_SelectedChange);

        //ajax manager, set conditional postback script to cancel ajax on grid export
        Telerik.Web.UI.RadAjaxManager.GetCurrent(this.Page).ClientEvents.OnRequestStart = "conditionalPostback";

        if (!this.IsPostBack)
        {
            SetShowHide(); //hide/show grid click
            SetUpRecordsPerPage(); //set value for drop down list
        }
    }

    #region protected methods / events

    #region rad grid events

    /// <summary>
    /// grid on need data source
    /// </summary>
    /// <param name="source"></param>
    /// <param name="e"></param>
    protected void rgTimesheet_NeedDataSource(object source, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        DesktopShared.TypedListClasses.TimesheetTypedList _timesheets = new DesktopShared.TypedListClasses.TimesheetTypedList();

        #region predicate expression

        IPredicateExpression _timesheetsFilter = new PredicateExpression();

        //client id
        if (SearchClientId > 0)
        {
            _timesheetsFilter.Add(DesktopShared.HelperClasses.TimesheetFields.FkClient
                == SearchClientId);
        }

        //employee id
        if (SearchEmployeeId > 0)
        {
            _timesheetsFilter.Add(DesktopShared.HelperClasses.TimesheetFields.FkEmployee
                == SearchEmployeeId);
        }

        //start date
        if (SearchStartDate != DateTime.MinValue)
        {
            _timesheetsFilter.Add(DesktopShared.HelperClasses.TimesheetFields.Date
                >= DesktopShared.Utility.Date.StartOfDayDate(SearchStartDate));
        }

        //end date
        if (SearchEndDate != DateTime.MinValue)
        {
            _timesheetsFilter.Add(DesktopShared.HelperClasses.TimesheetFields.Date
                <= DesktopShared.Utility.Date.EndOfDayDate(SearchEndDate));
        }

        //activity id
        if (SearchActivityId > 0)
        {
            _timesheetsFilter.Add(DesktopShared.HelperClasses.TimesheetFields.FkTaskaction
                == SearchActivityId);
        }

        //ticked id
        if (SearchTicketId > 0)
        {
            _timesheetsFilter.Add(DesktopShared.HelperClasses.TimesheetFields.FkCscdefects
                == SearchTicketId);
        } 

        //exclude client code
        if (!String.IsNullOrEmpty(SearchExcludeClientCode.Trim()))
        {
            char[] _delimiterChars = { ';', ',', '.', ':' };
            string[] _codes = SearchExcludeClientCode.Trim().Split(_delimiterChars);
            _timesheetsFilter.Add(DesktopShared.HelperClasses.ClientFields.Code != _codes);
        }

        //team id
        if (SearchTeamId.HasValue)
            _timesheetsFilter.Add(DesktopShared.HelperClasses.UsersFields.TeamId == SearchTeamId.Value);

        //project id
        if (SearchProjectId > 0)
        {
            _timesheetsFilter.Add(DesktopShared.HelperClasses.TimesheetFields.FkProject
                == SearchProjectId);
        }

        //exclude activity
        if (SearchExcludeActivity!=null)
        {
            _timesheetsFilter.Add(new FieldCompareRangePredicate
                (DesktopShared.HelperClasses.TimesheetFields.FkTaskaction, true, SearchExcludeActivity));
        }

        //unbilled hours only
        if (SearchUnbilledHoursOnly)
            _timesheetsFilter.Add(DesktopShared.HelperClasses.TimesheetFields.Bilhrs == 0);

        //overtime
        if (SearchOvertime.HasValue)
            _timesheetsFilter.Add(DesktopShared.HelperClasses.TimesheetFields.IsOvertime == SearchOvertime);

        #endregion

        #region sort

        #region sort on date only for TimesheetFields.Date field.  Ignore time

        IExpression datePart = new DbFunctionCall("CAST({0} AS DATE)", new object[] { DesktopShared.HelperClasses.TimesheetFields.Date });
        IEntityField datePartField = DesktopShared.HelperClasses.TimesheetFields.Date.SetExpression(datePart);

        ISortClause datePartSortClause = new SortClause(datePartField, null, SortOperator.Descending);
        datePartSortClause.EmitAliasForExpressionAggregateField = false;

        #endregion

        ISortExpression _timeSheetsSort = new SD.LLBLGen.Pro.ORMSupportClasses.SortExpression(datePartSortClause);
        //_timeSheetsSort.Add(DesktopShared.HelperClasses.TimesheetFields.Date | SortOperator.Descending);
        _timeSheetsSort.Add(DesktopShared.HelperClasses.TimesheetFields.Time | SortOperator.Ascending);

        #endregion

        _timesheets.Fill(0, _timeSheetsSort, false, _timesheetsFilter);
        rgTimesheet.DataSource = _timesheets;

        #region paging

        if (TicketsPerPage > 0)
            rgTimesheet.PageSize = TicketsPerPage;
        else
        {
            if (_timesheets.Rows.Count > 0)
                rgTimesheet.PageSize = _timesheets.Rows.Count;
        }

        #endregion

        #region grand total

        double _grandTotal = 0;
        foreach (DesktopShared.TypedListClasses.TimesheetRow _row in _timesheets)
            _grandTotal += Convert.ToDouble(_row.Hrs);

        litGrandTotal.Text = _grandTotal.ToString();
        
        #endregion

    }

    /// <summary>
    /// grid on item databound
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rgTimesheet_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        #region GridEditFormItem and IsInEditMode

        if (e.Item is GridEditFormItem && e.Item.IsInEditMode)
        {
            
            rgTimesheet.MasterTableView.NoMasterRecordsText = "";

            AbstractTimesheetEdit ucTimesheetEdit = e.Item.FindControl(GridEditFormItem.EditFormUserControlID) as AbstractTimesheetEdit;
            ucTimesheetEdit.EmployeeId = SearchEmployeeId; //used for admin only 

            #region edit existing

            if (!e.Item.OwnerTableView.IsItemInserted)
            {
                DataRowView drv = (DataRowView)e.Item.DataItem;
                ucTimesheetEdit.LoadValues(Convert.ToInt32(drv["Ptimesheet"].ToString().Trim()));
            }

            #endregion

            #region add new item

            else
            {
                ucTimesheetEdit.LoadValues(0);
            }

            #endregion

        }

        #endregion

        #region Item or AlternatingItem

        if (e.Item.ItemType == GridItemType.Item || e.Item.ItemType == GridItemType.AlternatingItem)
        {
            GridEditableItem item = e.Item as GridEditableItem;
            DesktopShared.TypedListClasses.TimesheetRow objTimesheet = (DesktopShared.TypedListClasses.TimesheetRow)((DataRowView)e.Item.DataItem).Row;

            //employee code 
            TelerikHelper.AddLabelToCell(objTimesheet.EmployeeCode.Trim(), item["EmployeeCode"]);

            //date 
            TelerikHelper.AddLabelToCell(objTimesheet.Date.ToString("MM/dd/yyyy"), item["Date"]);

            //time
            TelerikHelper.AddLabelToCell(objTimesheet.Time.Trim(), item["Time"]);

            //hours
            TelerikHelper.AddLabelToCell(objTimesheet.Hrs.ToString().Trim(), item["Hrs"]);
            _totalHours += objTimesheet.Hrs;

            //client
            TelerikHelper.AddLabelToCell(objTimesheet.ClientCompany.Trim(), item["ClientCompany"]);

            //activity
            TelerikHelper.AddLabelToCell(objTimesheet.Descr1.Trim(), item["Descr1"]);

            //ticket # 
            if ((objTimesheet.FkCscdefects > 0) && (!objTimesheet.IsFkCscdefectsNull()))
            {
            TelerikHelper.AddHyperLinkToCell(
                String.Format("{0} - {1}", objTimesheet.FkCscdefects.ToString().Trim(), objTimesheet.CscDefectsSummary.Trim()), //display text
                String.Format("{0} - {1}", objTimesheet.FkCscdefects.ToString().Trim(), objTimesheet.CscDefectsSummary.Trim()), //tool tip
                String.Format("/Ticket/Detail2.aspx?Id={0}", objTimesheet.FkCscdefects.ToString()), //navigate url
                item["FkCscdefects"]);
            }
            else
            {
                item["FkCscdefects"].Text = "";
                item["FkCscdefects"].Controls.Clear();
            }

            //timesheet details
            string _details = objTimesheet.Memo.Trim().Length > 0 ? objTimesheet.Memo.Trim() : objTimesheet.InternalComments.Trim();
            TelerikHelper.AddHyperLinkToCell(
                _details, //display text
                String.Format("{0}{1}", _details, (objTimesheet.Memo.Trim().Length ==0) ? " (Internal Note)" : ""), //tool tip
                String.Format("/Timesheet/Detail.aspx?TimesheetID={0}", objTimesheet.Ptimesheet.ToString()), //navigate url
                item["Memo"]);

            //project
            TelerikHelper.AddLabelToCell(objTimesheet.ProjectDescr.Trim(), item["ProjectDescr"]);
        }

        if (e.Item.ItemType == GridItemType.Footer)
        {
            GridFooterItem _footerItem = e.Item as GridFooterItem;
            _footerItem["Hrs"].Text = _totalHours.ToString();
        }

        #endregion

        #region exporting

        if (_isExport && e.Item is GridDataItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;

            //employee code
            ((GridDataItem)e.Item)["EmployeeCode"].Width = Unit.Pixel(75);
            //client (display full text - is truncated in grid)
            ((GridDataItem)e.Item)["ClientCompany"].Text = drv["ClientCompany"].ToString().Trim();
            //activity (display full text - is truncated in grid)
            ((GridDataItem)e.Item)["Descr1"].Text = drv["Descr1"].ToString().Trim();
            //memo / details (display full text - is truncated in grid)
            ((GridDataItem)e.Item)["Memo"].Text = drv["Memo"].ToString().Trim();
            //project (display full text - is truncated in grid)
            ((GridDataItem)e.Item)["ProjectDescr"].Text = drv["ProjectDescr"].ToString().Trim();

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

        if (_isExport && e.Item is GridFooterItem)
        {
            GridFooterItem footerItem = (GridFooterItem)e.Item;
            footerItem.Style["font-size"] = "9pt";
            footerItem.Style["font-family"] = "Verdana";
            footerItem.Style["font-weight"] = "bold";
        }

        #endregion

    }

    /// <summary>
    /// grid on pre render
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rgTimesheet_PreRender(object sender, EventArgs e)
    {
        rgTimesheet.ShowHeader = true;
        rgTimesheet.ShowFooter = true;

        rgTimesheet.PagerStyle.AlwaysVisible = true;
        rgTimesheet.PagerStyle.Visible = true;
        
        rgTimesheet.MasterTableView.NoMasterRecordsText = "No records found.";

        phGrandTotal.Visible = true;

        GridCommandItem commandItem = null;

        if (rgTimesheet.MasterTableView.Items.Count > 0 & !_isExport)
        {
            commandItem = (GridCommandItem)rgTimesheet.MasterTableView.GetItems(GridItemType.CommandItem)[0];
            commandItem.Visible = true;
        }

        #region add / editing ticket

        if (rgTimesheet.EditItems.Count > 0 || rgTimesheet.MasterTableView.IsItemInserted)
        {
            foreach (GridDataItem item in rgTimesheet.MasterTableView.Items)
            {
                item.Visible = false;
            }

            rgTimesheet.ShowHeader = false;
            rgTimesheet.ShowFooter = false;

            rgTimesheet.PagerStyle.AlwaysVisible = false;
            rgTimesheet.PagerStyle.Visible = false;

            phGrandTotal.Visible = false;

            if (commandItem!=null)
                commandItem.Visible = false;
        }

        #endregion

        #region command item template -> populate export to type drop down list

        UserControl_DropDownList_GridExportType ucGridExportType =
            rgTimesheet.MasterTableView.GetItems(GridItemType.CommandItem)[0].FindControl("ucGridExportType")
            as UserControl_DropDownList_GridExportType;

        ucGridExportType.Populate();

        #endregion
    }

    /// <summary>
    /// timesheet grid on item created
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rgTimesheet_ItemCreated(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        #region command item -> hide for export

        if (e.Item is GridCommandItem && _isExport)
            e.Item.Display = false;

        #endregion

    }

    /// <summary>
    /// grid on update command
    /// </summary>
    /// <param name="source"></param>
    /// <param name="e"></param>
    protected void rgTimesheet_UpdateCommand(object source, GridCommandEventArgs e)
    {
        if (Page.IsValid)
        {
            GridEditableItem editedItem = e.Item as GridEditableItem;

            AbstractTimesheetEdit ucTimesheetEdit = e.Item.FindControl
                (GridEditFormItem.EditFormUserControlID) as AbstractTimesheetEdit;

            int Id = Convert.ToInt32(editedItem.OwnerTableView.DataKeyValues
                [editedItem.ItemIndex]["Ptimesheet"]);

            ucTimesheetEdit.SaveValues(Id);
        }
    }

    /// <summary>
    /// grid on insert command
    /// </summary>
    /// <param name="source"></param>
    /// <param name="e"></param>
    protected void rgTimesheet_InsertCommand(object source, GridCommandEventArgs e)
    {
        if (Page.IsValid)
        {
            GridEditableItem editedItem = e.Item as GridEditableItem;

            AbstractTimesheetEdit ucTimesheeEdit = e.Item.FindControl
                (GridEditFormItem.EditFormUserControlID) as AbstractTimesheetEdit;

            ucTimesheeEdit.SaveValues(0);
        }
    }

    /// <summary>
    /// grid on delete command
    /// </summary>
    /// <param name="source"></param>
    /// <param name="e"></param>
    protected void rgTimesheet_DeleteCommand(object source, GridCommandEventArgs e)
    {
        int Id = Convert.ToInt32((e.Item as GridDataItem).OwnerTableView.DataKeyValues
            [e.Item.ItemIndex]["Ptimesheet"]);

        DesktopShared.EntityClasses.TimesheetEntity _timesheet =
            new DesktopShared.EntityClasses.TimesheetEntity(Id);

        _timesheet.Delete();
    }

    #endregion

    /// <summary>
    /// records per page drop list on selected index changed
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlRecordsPerPage_SelectedChange(object sender, EventArgs e)
    {
        TicketsPerPage = ucRecordsPerPage.SelectedValue;
        rgTimesheet.Rebind();
    }

    /// <summary>
    /// export button click
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnExport_Click(object sender, System.EventArgs e)
    {
        ConfigureExport();

        UserControl_DropDownList_GridExportType ucGridExportType =
           rgTimesheet.MasterTableView.GetItems(GridItemType.CommandItem)[0].FindControl("ucGridExportType")
           as UserControl_DropDownList_GridExportType;

        if (ucGridExportType.Mode == UserControl_DropDownList_GridExportType.ExportMode.Csv)
            rgTimesheet.MasterTableView.ExportToCSV();
        else if (ucGridExportType.Mode == UserControl_DropDownList_GridExportType.ExportMode.Excel)
            rgTimesheet.MasterTableView.ExportToExcel();
        else if (ucGridExportType.Mode == UserControl_DropDownList_GridExportType.ExportMode.Word)
            rgTimesheet.MasterTableView.ExportToWord();
        else if (ucGridExportType.Mode == UserControl_DropDownList_GridExportType.ExportMode.Pdf)
            rgTimesheet.MasterTableView.ExportToPdf();
    }

    #endregion

    #region private methods

    /// <summary>
    /// configure grid for export
    /// </summary>
    private void ConfigureExport()
    {
        rgTimesheet.ExportSettings.IgnorePaging = true;
        rgTimesheet.ExportSettings.OpenInNewWindow = true;
        rgTimesheet.ExportSettings.ExportOnlyData = true;
        rgTimesheet.ExportSettings.HideStructureColumns = true;

        rgTimesheet.ExportSettings.FileName = "Timesheet_" + DateTime.Now.ToString("yyyyMMdd-hhmmss");

        //hide command item template
        foreach (GridItem commandItem in this.rgTimesheet.MasterTableView.GetItems(GridItemType.CommandItem))
        {
            commandItem.Visible = false;
            commandItem.Display = false;
        }

        //hide link columns
        rgTimesheet.MasterTableView.Columns.FindByUniqueName("Edit").Visible = false;
        rgTimesheet.MasterTableView.Columns.FindByUniqueName("Delete").Visible = false;

        _isExport = true;
    }

    /// <summary>
    /// set up records per page
    /// </summary>
    private void SetUpRecordsPerPage()
    {
        ucRecordsPerPage.SelectedValue = TicketsPerPage;
    }

    /// <summary>
    /// set show/hide of grid via click
    /// </summary>
    private void SetShowHide()
    {
        //<a href="#" class="hide" onclick="showHideInfo(this, '<%=this.ClientID %>'); return false;"></a>

        if (!SetShowHideDone)
        {
            string _linkClass = "hide";
            string _divDisplay = "";

            if (HideGrid)
            {
                _linkClass = "show";
                _divDisplay = "none";
            }

            hlShowHide.Attributes.Add("href", "#");
            hlShowHide.Attributes.Add("class", _linkClass);

            string _onClickFunction = "showHideInfo(this, '" + divTimesheet.ClientID + "');";
            _onClickFunction += "return false;";
            hlShowHide.Attributes.Add("onclick", _onClickFunction);

            divTimesheet.Attributes.CssStyle.Add("display", _divDisplay);

            SetShowHideDone = true;
        }
    }

    #endregion

    #region public methods

    /// <summary>
    /// reset/hide grid
    /// </summary>
    public void ResetGrid()
    {
        rgTimesheet.Visible = true;
        pnlHeader.Visible = true;
        phGrandTotal.Visible = true;

        rgTimesheet.EditIndexes.Clear();
        rgTimesheet.DataSource = null;
        rgTimesheet.Rebind();
    }

    /// <summary>
    /// clear grid
    /// </summary>
    public void ClearGrid()
    {
        rgTimesheet.CurrentPageIndex = 0;
        rgTimesheet.EditIndexes.Clear();
        rgTimesheet.Visible = false;
        pnlHeader.Visible = false;
        phGrandTotal.Visible = false;
    }

    #endregion

    #region private properties

    /// <summary>
    /// get/set tickets per page (stored in session)
    /// </summary>
    private int TicketsPerPage
    {
        get
        {
            object obj = this.Session["TicketsPerPage"];
            if (obj == null)
                return 25;
            else
                return (int)obj;
        }

        set
        {
            this.Session["TicketsPerPage"] = value;
        }
    }

    #endregion

    #region public properties

    /// <summary>
    /// get/set show hide done (stored in viewstate)
    /// </summary>
    public bool SetShowHideDone
    {
        get
        {
            object obj = this.ViewState["SetShowHideDoneForTimesheet"];
            if (obj == null)
                return false;
            else
                return (bool)obj;
        }

        set
        {
            this.ViewState["SetShowHideDoneForTimesheet"] = value;
        }
    }

    /// <summary>
    /// get/set hide grid (stored in viewstate)
    /// </summary>
    public bool HideGrid
    {
        get
        {
            object obj = this.ViewState["HideGridForTimesheet"];
            if (obj == null)
                return false;
            else
                return (bool)obj;
        }

        set
        {
            this.ViewState["HideGridForTimesheet"] = value;
        }
    }

    /// <summary>
    /// get/set search client id (stored in viewstate)
    /// </summary>
    public int SearchClientId
    {
        get
        {
            object obj = this.ViewState["SearchClientIdForTimesheet"];
            if (obj == null)
                return -1;
            else
                return (int)obj;
        }

        set
        {
            this.ViewState["SearchClientIdForTimesheet"] = value;
        }
    }

    /// <summary>
    /// get/set search employee id
    /// </summary>
    public int SearchEmployeeId
    {
        get
        {
            object obj = this.ViewState["SearchEmployeeIdForTimesheet"];
            if (obj == null)
                return -1;
            else
                return (int)obj;
        }

        set
        {
            this.ViewState["SearchEmployeeIdForTimesheet"] = value;
        }
    }

    /// <summary>
    /// get/set search team id
    /// </summary>
    public int? SearchTeamId
    {
        get
        {
            object obj = this.ViewState["SearchTeamIdForTimesheet"];
            return (obj == null) ? (int?)null : (int)obj;
        }
        set { this.ViewState["SearchTeamIdForTimesheet"] = value; }
    }

    /// <summary>
    /// get/set search exclude client code
    /// </summary>
    public string SearchExcludeClientCode
    {
        get
        {
            object obj = this.ViewState["SearchExcludeClientCode"];
            return (obj == null) ? "" : (string)obj;
        }
        set { this.ViewState["SearchExcludeClientCode"] = value; }
    }

    /// <summary>
    /// get/set search start date (stored in viewstate)
    /// </summary>
    public DateTime SearchStartDate
    {
        get
        {
            object obj = this.ViewState["SearchStartDateForTimesheet"];
            if (obj == null)
                return DateTime.MinValue;
            else
                return (DateTime)obj;
        }

        set
        {
            this.ViewState["SearchStartDateForTimesheet"] = value;
        }
    }

    /// <summary>
    /// get/set search end date (stored in viewstate)
    /// </summary>
    public DateTime SearchEndDate
    {
        get
        {
            object obj = this.ViewState["SearchEndDateForTimesheet"];
            if (obj == null)
                return DateTime.MinValue;
            else
                return (DateTime)obj;
        }

        set
        {
            this.ViewState["SearchEndDateForTimesheet"] = value;
        }
    }

    /// <summary>
    /// get/set search activity id (stored in viewstate)
    /// </summary>
    public int SearchActivityId
    {
        get
        {
            object obj = this.ViewState["SearchActivityIdForTimesheet"];
            if (obj == null)
                return -1;
            else
                return (int)obj;
        }

        set
        {
            this.ViewState["SearchActivityIdForTimesheet"] = value;
        }
    }

    /// <summary>
    /// get/set search ticket id (stored in viewstate)
    /// </summary>
    public int SearchTicketId
    {
        get
        {
            object obj = this.ViewState["SearchTicketIdForTimesheet"];
            if (obj == null)
                return -1;
            else
                return (int)obj;
        }

        set
        {
            this.ViewState["SearchTicketIdForTimesheet"] = value;
        }
    }

    /// <summary>
    /// get/set search project id (stored in viewstate)
    /// </summary>
    public int SearchProjectId
    {
        get
        {
            object obj = this.ViewState["SearchProjectIdForTimesheet"];
            if (obj == null)
                return -1;
            else
                return (int)obj;
        }

        set
        {
            this.ViewState["SearchProjectIdForTimesheet"] = value;
        }
    }

    /// <summary>
    /// get/set search exclude activity array list (stored in viewstate)
    /// </summary>
    public ArrayList SearchExcludeActivity
    {
        get
        {
            object obj = this.ViewState["SearchExcludeActivityForTimesheet"];
            if (obj == null)
                return null;
            else
                return (ArrayList)obj;
        }

        set
        {
            this.ViewState["SearchExcludeActivityForTimesheet"] = value;
        }
    }

    /// <summary>
    /// get/set search unbilled hours (stored in viewstate)
    /// </summary>
    public bool SearchUnbilledHoursOnly
    {
        get
        {
            object obj = this.ViewState["SearchUnbilledHoursOnlyForTimesheet"];
            if (obj == null)
                return false;
            else
                return (bool)obj;
        }

        set
        {
            this.ViewState["SearchUnbilledHoursOnlyForTimesheet"] = value;
        }
    }

    /// <summary>
    /// set show grand total of hours
    /// </summary>
    public bool ShowGrandTotal
    {
        set { phGrandTotal.Visible = value; }
    }

    /// <summary>
    /// get/set search overtime hours (stored in viewstate)
    /// </summary>
    public bool? SearchOvertime
    {
        get
        {
            object obj = this.ViewState["SearchOvertimeForTimesheet"];
            if (obj == null)
                return null;
            else
                return (bool)obj;
        }

        set
        {
            this.ViewState["SearchOvertimeForTimesheet"] = value;
        }
    }

    #endregion

}
