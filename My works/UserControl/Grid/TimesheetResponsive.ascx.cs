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

public partial class UserControl_Grid_TimesheetResponsive : System.Web.UI.UserControl
{
    private double _totalHours = 0;

    /// <summary>
    /// page load
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        ConfigureForDevice();
    }

    #region protected methods / events

    #region grid events

    /// <summary>
    /// grid on need data source
    /// </summary>
    /// <param name="source"></param>
    /// <param name="e"></param>
    protected void rgTimesheet_NeedDataSource(object source, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        int _currentPage = ResetSearch ? 1 : rgTimesheet.CurrentPageIndex + 1;
        int _resultCount = 0;
        int _resultsPerPage = rgTimesheet.PageSize;

        int? _clientId = SearchClientId > 0 ? SearchClientId : (int?)null;
        int? _employeeId = SearchEmployeeId > 0 ? SearchEmployeeId : (int?)null;
        DateTime? _startDate = SearchStartDate > DateTime.MinValue ? SearchStartDate : (DateTime?)null;
        DateTime? _endDate = SearchEndDate > DateTime.MinValue ? SearchEndDate : (DateTime?)null;
        int? _taskId = SearchActivityId > 0 ? SearchActivityId : (int?)null;
        int? _ticketId = SearchTicketId > 0 ? SearchTicketId : (int?)null;
        int? _projectId = SearchProjectId > 0 ? SearchProjectId : (int?)null;
        bool? _unbilled = SearchUnbilledHoursOnly ? true : (bool?)null;

        System.Text.StringBuilder _sbExcludeClientCode = new System.Text.StringBuilder();
        if (!String.IsNullOrWhiteSpace(SearchExcludeClientCode))
        {
            string _codes = SearchExcludeClientCode.Replace(";", ",").Replace(".", ",").Replace(":", ",");
            foreach (string _code in _codes.Split(','))
            {
                if (_sbExcludeClientCode.Length > 0)
                    _sbExcludeClientCode.Append(",");
                _sbExcludeClientCode.Append(String.Format("'{0}'", _code));
            }
        }

        string _excludeActivity = ((SearchExcludeActivity != null) && (SearchExcludeActivity.Count > 0)) ? string.Join(",", SearchExcludeActivity.ToArray()) : "";

        #region count

        var _dtTimesheetCount = DesktopShared.StoredProcedureCallerClasses.RetrievalProcedures.ProcSearchTimesheetsCount(
            _clientId, //client id
            _employeeId, //employee id
            _startDate, //start date
            _endDate, //end date
            _taskId, //task id
            _ticketId, //ticket id
            _sbExcludeClientCode.ToString(), //exclude client code
            SearchTeamId, //team id
            _projectId, //project id
            _excludeActivity, //exclude activity
            _unbilled, // search unbilled
            Mileage
            ); ;
        if (_dtTimesheetCount.Rows.Count > 0)
            _resultCount = Convert.ToInt32(_dtTimesheetCount.Rows[0][0].ToString());

        #endregion

        #region data table

        var _dtTimesheet = DesktopShared.StoredProcedureCallerClasses.RetrievalProcedures.ProcSearchTimesheets(
            _clientId, //client id
            _employeeId, //employee id
            _startDate, //start date
            _endDate, //end date
            _taskId, //task id
            _ticketId, //ticket id
            _sbExcludeClientCode.ToString(), //exclude client code
            SearchTeamId, //team id
            _projectId, //project id
            _excludeActivity, //exclude activity
            _unbilled, // search unbilled
            _resultsPerPage, //page size
           _currentPage, //current page
           SortColumnName, //sort by
           SortOperator == SD.LLBLGen.Pro.ORMSupportClasses.SortOperator.Ascending ? "asc" : "desc", //sort direction
           Mileage
            );

        #endregion

        rgTimesheet.VirtualItemCount = _resultCount;
        rgTimesheet.DataSource = _dtTimesheet;
        ResetSearch = false;

        #region grand total

        var _dtTimesheetHours = DesktopShared.StoredProcedureCallerClasses.RetrievalProcedures.ProcSearchTimesheetsHours(
            _clientId, //client id
            _employeeId, //employee id
            _startDate, //start date
            _endDate, //end date
            _taskId, //task id
            _ticketId, //ticket id
            _sbExcludeClientCode.ToString(), //exclude client code
            SearchTeamId, //team id
            _projectId, //project id
            _excludeActivity, //exclude activity
            _unbilled, // search unbilled
            Mileage
            ); ;
        if (_dtTimesheetHours.Rows.Count > 0)
            litGrandTotal.Text = _dtTimesheetHours.Rows[0][0].ToString();

        #endregion
    }

    /// <summary>
    /// grid on sort command
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rgTimesheet_SortCommand(object sender, GridSortCommandEventArgs e)
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
        rgTimesheet.CurrentPageIndex = 0;
        rgTimesheet.Rebind();

        if (!e.Item.OwnerTableView.SortExpressions.ContainsExpression(e.SortExpression))
        {
            sortExpression = new GridSortExpression();
            sortExpression.FieldName = e.SortExpression;
            sortExpression.SortOrder = GridSortOrder.Ascending;
            e.Item.OwnerTableView.SortExpressions.AddSortExpression(sortExpression);
        }
    }

    /// <summary>
    /// grid on item databound
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rgTimesheet_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        #region Item or AlternatingItem

        if (e.Item.ItemType == GridItemType.Item || e.Item.ItemType == GridItemType.AlternatingItem)
        {
            GridEditableItem item = e.Item as GridEditableItem;
            DataRowView drv = (DataRowView)e.Item.DataItem;

            //ticket id
            item["FkCscdefects"].Text = "";
            item["FkCscdefects"].Controls.Clear();
            int _ticketId = 0;
            int.TryParse(drv["FkCscdefects"].ToString(), out _ticketId);

            //view ticket
            HyperLink btnViewTicket = e.Item.FindControl("btnViewTicket") as HyperLink;
            if (_ticketId > 0)
            {
                btnViewTicket.Visible = true;
                btnViewTicket.NavigateUrl = String.Format("/Ticket/Detail2.aspx?Id={0}", _ticketId);
            }
            else
                btnViewTicket.Visible = false;

            //employee code 
            TelerikHelper.AddLabelToCell(drv["EmployeeCode"].ToString().Trim(), item["EmployeeCode"]);

            //date 
            DateTime _date = DateTime.MinValue;
            if (DateTime.TryParse(drv["Date"].ToString(), out _date))
                TelerikHelper.AddLabelToCell(_date.ToString("MM/dd/yyyy"), item["Date"]);

            //time
            TelerikHelper.AddLabelToCell(drv["Time"].ToString().Trim(), item["Time"]);

            //hours
            TelerikHelper.AddLabelToCell(drv["Hrs"].ToString().Trim(), item["Hrs"]);
            //_totalHours += objTimesheet.Hrs;

            //client
            TelerikHelper.AddLabelToCell(drv["ClientCompany"].ToString().Trim(), item["ClientCompany"]);

            //activity
            TelerikHelper.AddLabelToCell(drv["Descr1"].ToString().Trim(), item["Descr1"]);

            //ticket
            if (_ticketId > 0)
            {
                TelerikHelper.AddHyperLinkToCell(
                    String.Format("{0} - {1}", _ticketId.ToString().Trim(), drv["CscDefectsSummary"].ToString().Trim()), //display text
                    String.Format("{0} - {1}", _ticketId.ToString().Trim(), drv["CscDefectsSummary"].ToString().Trim()), //tool tip
                    String.Format("/Ticket/Detail2.aspx?Id={0}", _ticketId.ToString()), //navigate url
                    item["FkCscdefects"]);
            }

            //timesheet details
            string _memo = drv["Memo"].ToString().Trim();
            string _details = _memo.Length > 0 ? _memo : drv["InternalComments"].ToString().Trim();
            if(SearchDetailsOnly==false)
                TelerikHelper.AddHyperLinkToCell(
                    _details, //display text
                    String.Format("{0}{1}", _details, (_memo.Trim().Length == 0) ? " (Internal Note)" : ""), //tool tip
                    String.Format("/Timesheet/Detail2.aspx?TimesheetID={0}", drv["Ptimesheet"].ToString()), //navigate url
                    item["Memo"]);
            else
                TelerikHelper.AddLabelToCell(_details, item["Memo"]);

            //project
            TelerikHelper.AddLabelToCell(drv["ProjectDescr"].ToString().Trim(), item["ProjectDescr"]);


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
        if (DesktopShared.Utility.BrowserHelper.IsTabletOrSmartPhone(this.Request))
        {
            int _colIndex = -1;
            rgTimesheet.MasterTableView.Columns[++_colIndex].HeaderStyle.Width = Unit.Pixel(80);
            rgTimesheet.MasterTableView.Columns[++_colIndex].HeaderStyle.Width = Unit.Pixel(100);
            rgTimesheet.MasterTableView.Columns[++_colIndex].HeaderStyle.Width = Unit.Pixel(100);
            rgTimesheet.MasterTableView.Columns[++_colIndex].HeaderStyle.Width = Unit.Pixel(100);
            rgTimesheet.MasterTableView.Columns[++_colIndex].HeaderStyle.Width = Unit.Pixel(100);
            rgTimesheet.MasterTableView.Columns[++_colIndex].HeaderStyle.Width = Unit.Pixel(150);
            rgTimesheet.MasterTableView.Columns[++_colIndex].HeaderStyle.Width = Unit.Pixel(150);
            rgTimesheet.MasterTableView.Columns[++_colIndex].HeaderStyle.Width = Unit.Pixel(200);
            rgTimesheet.MasterTableView.Columns[++_colIndex].HeaderStyle.Width = Unit.Pixel(250);
            rgTimesheet.MasterTableView.Columns[++_colIndex].HeaderStyle.Width = Unit.Pixel(200);
        }

        bool _detailonly = false;
        _detailonly = SearchDetailsOnly;

        if (_detailonly == true)
        {
            rgTimesheet.MasterTableView.Columns[0].Visible = false;
            rgTimesheet.MasterTableView.GetColumn("EmployeeCode").Visible = false;
            rgTimesheet.MasterTableView.GetColumn("Date").Visible = false;
            rgTimesheet.MasterTableView.GetColumn("Time").Visible = false;
            rgTimesheet.MasterTableView.GetColumn("Hrs").Visible = false;
            rgTimesheet.MasterTableView.GetColumn("ClientCompany").Visible = false;
            rgTimesheet.MasterTableView.GetColumn("Descr1").Visible = false;
            rgTimesheet.MasterTableView.GetColumn("FkCscdefects").Visible = false;
            rgTimesheet.MasterTableView.GetColumn("ProjectDescr").Visible = false;
        }
        else
        {
            rgTimesheet.MasterTableView.Columns[0].Visible = true;
            rgTimesheet.MasterTableView.GetColumn("EmployeeCode").Visible = true;
            rgTimesheet.MasterTableView.GetColumn("Date").Visible = true;
            rgTimesheet.MasterTableView.GetColumn("Time").Visible = true;
            rgTimesheet.MasterTableView.GetColumn("Hrs").Visible = true;
            rgTimesheet.MasterTableView.GetColumn("ClientCompany").Visible = true;
            rgTimesheet.MasterTableView.GetColumn("Descr1").Visible = true;
            rgTimesheet.MasterTableView.GetColumn("FkCscdefects").Visible = true;
            rgTimesheet.MasterTableView.GetColumn("ProjectDescr").Visible = true;
        }
    }

    /// <summary>
    /// grid on delete command
    /// </summary>
    /// <param name="source"></param>
    /// <param name="e"></param>
    protected void rgTimesheet_DeleteCommand(object source, GridCommandEventArgs e)
    {
        int Id = Convert.ToInt32((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["Ptimesheet"]);
        DesktopShared.EntityClasses.TimesheetEntity _timesheet = new DesktopShared.EntityClasses.TimesheetEntity(Id);
        _timesheet.Delete();
        //DisplayMessage()
    }

    #endregion

    /// <summary>
    /// export button click
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnExport_Click(object sender, System.EventArgs e)
    {
        ConfigureExport();
        rgTimesheet.MasterTableView.ExportToCSV();
    }

    #endregion

    #region private methods

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
    /// configure for device
    /// </summary>
    private void ConfigureForDevice()
    {
        if (DesktopShared.Utility.BrowserHelper.IsTabletOrSmartPhone(this.Request))
        {
            rgTimesheet.RenderMode = RenderMode.Lightweight;
            phTabletPagerCss.Visible = true;
        }
    }

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
        rgTimesheet.MasterTableView.Columns.FindByUniqueName("ActionColumn").Visible = false;

    }

    #endregion

    #region public methods

    /// <summary>
    /// reset/hide grid
    /// </summary>
    public void ResetGrid()
    {
        rgTimesheet.Visible = true;
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
        phGrandTotal.Visible = false;
    }

    #endregion

    #region private properties

    /// <summary>
    /// get/set sort column name
    /// </summary>
    private string SortColumnName
    {
        get
        {
            object obj = this.ViewState["scn_ts"];
            return (obj == null) ? "Date" : (string)obj;
        }
        set { this.ViewState["scn_ts"] = value; }
    }

    /// <summary>
    /// get/set sort operator
    /// </summary>
    private SD.LLBLGen.Pro.ORMSupportClasses.SortOperator SortOperator
    {
        get
        {
            object obj = this.ViewState["so_ts"];
            return (obj == null) ? SD.LLBLGen.Pro.ORMSupportClasses.SortOperator.Descending : (SD.LLBLGen.Pro.ORMSupportClasses.SortOperator)obj;
        }
        set { this.ViewState["so_ts"] = value; }
    }

    #endregion

    #region public properties

    /// <summary>
    /// get/set reset search
    /// </summary>
    public bool ResetSearch
    {
        get
        {
            object obj = this.ViewState["rs_ts"];
            return (obj == null) ? false : (bool)obj;
        }
        set { this.ViewState["rs_ts"] = value; }
    }


    /// <summary>
    /// get/set search client id 
    /// </summary>
    public int SearchClientId
    {
        get
        {
            object obj = this.ViewState["SearchClientIdForTimesheet"];
            return (obj == null) ? -1 : (int)obj;
        }
        set { this.ViewState["SearchClientIdForTimesheet"] = value; }
    }

    /// <summary>
    /// get/set search employee id
    /// </summary>
    public int SearchEmployeeId
    {
        get
        {
            object obj = this.ViewState["SearchEmployeeIdForTimesheet"];
            return (obj == null) ? -1 : (int)obj;
        }
        set { this.ViewState["SearchEmployeeIdForTimesheet"] = value; }
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
    /// get/set search Mileage
    /// </summary>
    public decimal? Mileage
    {
        get
        {
            object obj = this.ViewState["SearchMileageForTimesheet"];
            return (obj == null) ? (decimal?)null : (decimal)obj;
        }
        set { this.ViewState["SearchMileageForTimesheet"] = value; }
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
    /// get/set search start date 
    /// </summary>
    public DateTime SearchStartDate
    {
        get
        {
            object obj = this.ViewState["SearchStartDateForTimesheet"];
            return (obj == null) ? DateTime.MinValue : (DateTime)obj;
        }
        set { this.ViewState["SearchStartDateForTimesheet"] = value; }
    }

    /// <summary>
    /// get/set search end date 
    /// </summary>
    public DateTime SearchEndDate
    {
        get
        {
            object obj = this.ViewState["SearchEndDateForTimesheet"];
            return (obj == null) ? DateTime.MinValue : (DateTime)obj;
        }
        set { this.ViewState["SearchEndDateForTimesheet"] = value; }
    }

    /// <summary>
    /// get/set search activity id 
    /// </summary>
    public int SearchActivityId
    {
        get
        {
            object obj = this.ViewState["SearchActivityIdForTimesheet"];
            return (obj == null) ? -1 : (int)obj;
        }
        set { this.ViewState["SearchActivityIdForTimesheet"] = value; }
    }

    /// <summary>
    /// get/set search ticket id 
    /// </summary>
    public int SearchTicketId
    {
        get
        {
            object obj = this.ViewState["SearchTicketIdForTimesheet"];
            return (obj == null) ? -1 : (int)obj;
        }
        set { this.ViewState["SearchTicketIdForTimesheet"] = value; }
    }

    /// <summary>
    /// get/set search project id 
    /// </summary>
    public int SearchProjectId
    {
        get
        {
            object obj = this.ViewState["SearchProjectIdForTimesheet"];
            return (obj == null) ? -1 : (int)obj;
        }

        set { this.ViewState["SearchProjectIdForTimesheet"] = value; }
    }

    /// <summary>
    /// get/set search exclude activity array list 
    /// </summary>
    public ArrayList SearchExcludeActivity
    {
        get
        {
            object obj = this.ViewState["SearchExcludeActivityForTimesheet"];
            return (obj == null) ? null : (ArrayList)obj;
        }
        set { this.ViewState["SearchExcludeActivityForTimesheet"] = value; }
    }

    /// <summary>
    /// get/set search unbilled hours 
    /// </summary>
    public bool SearchUnbilledHoursOnly
    {
        get
        {
            object obj = this.ViewState["SearchUnbilledHoursOnlyForTimesheet"];
            return (obj == null) ? false : (bool)obj;
        }
        set { this.ViewState["SearchUnbilledHoursOnlyForTimesheet"] = value; }
    }

    /// <summary>
    /// set show grand total of hours
    /// </summary>
    public bool ShowGrandTotal
    {
        set { phGrandTotal.Visible = value; }
    }

    /// <summary>
    /// get/set search unbilled hours 
    /// </summary>
    public bool SearchDetailsOnly
    {
        get
        {
            object obj = this.ViewState["SearchDetailsOnlyForTimesheet"];
            return (obj == null) ? false : (bool)obj;
        }
        set { this.ViewState["SearchDetailsOnlyForTimesheet"] = value; }
    }

    #endregion

}