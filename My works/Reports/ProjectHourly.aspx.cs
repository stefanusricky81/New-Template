using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class Reports_ProjectHourly : BasePage
{
    /// <summary>
    /// page load
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        ConfigureForDevice();
        if (!IsPostBack)
            SetUpPage();

        if (ModalIsOpen)
            ShowTimesheetModal();
    }

    #region private methods

    /// <summary>
    /// set up for initial page load
    /// </summary>
    private void SetUpPage()
    {
        ddlClient.Focus();

        short _tabIndex = 0;
        ddlClient.TabIndex = ++_tabIndex;
        ddlEmployee.TabIndex = ++_tabIndex;
        ucStartDate.TabIndex = ++_tabIndex;
        ucEndDate.TabIndex = ++_tabIndex;
        btnSubmit.TabIndex = ++_tabIndex;
        btnClear.TabIndex = ++_tabIndex;

        DateTime _now = DateTime.Now;
        ucEndDate.SelectedDate = _now;
        ucStartDate.SelectedDate = DesktopShared.Utility.Date.GetFirstDayOfMonth(_now);
    }

    /// <summary>
    /// show timesheet modal
    /// </summary>
    private void ShowTimesheetModal()
    {
        ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowModal", "ShowimesheetModal();", true);
    }

    /// <summary>
    /// configure for device
    /// </summary>
    private void ConfigureForDevice()
    {
        bool _displayChosenScript = true;
        string _cssClass = "form-control select-chosen";
        if (DesktopShared.Utility.BrowserHelper.IsTabletOrSmartPhone(this.Request))
        {
            _displayChosenScript = false;
            _cssClass = "form-control";

            rgReport.RenderMode = RenderMode.Lightweight;
            phTabletPagerCss.Visible = true;
        }

        ddlClient.DisplayChosenScript = _displayChosenScript;
        ddlClient.CssClass = _cssClass;
        ddlEmployee.DisplayChosenScript = _displayChosenScript;
        ddlEmployee.CssClass = _cssClass;
    }

    /// <summary>
    /// rebind document grid
    /// </summary>
    private void RebindGrid()
    {
        phSearchResults.Visible = true;
        ResetSearch = true;
        rgReport.Visible = true;
        rgReport.EditIndexes.Clear();
        rgReport.DataSource = null;
        rgReport.Rebind();
    }

    /// <summary>
    /// configure grid for export
    /// </summary>
    private void ConfigureExport()
    {
        rgReport.ExportSettings.IgnorePaging = true;
        rgReport.ExportSettings.OpenInNewWindow = true;
        rgReport.ExportSettings.ExportOnlyData = true;
        rgReport.ExportSettings.HideStructureColumns = true;

        rgReport.ExportSettings.FileName = "HourlyProjects_" + DateTime.Now.ToString("yyyyMMdd-hhmmss");

        //hide command item template
        foreach (GridItem commandItem in this.rgReport.MasterTableView.GetItems(GridItemType.CommandItem))
        {
            commandItem.Visible = false;
            commandItem.Display = false;
        }

        rgReport.MasterTableView.Columns.FindByUniqueName("HoursForExport").Visible = true;
        rgReport.MasterTableView.Columns.FindByUniqueName("Hours").Visible = false;
    }

    #endregion

    #region protected events

    #region telerik grid

    /// <summary>
    /// grid on need data source
    /// </summary>
    /// <param name="source"></param>
    /// <param name="e"></param>
    protected void rgReport_NeedDataSource(object source, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        int _currentPage = ResetSearch ? 1 : rgReport.CurrentPageIndex + 1;
        int _resultCount = 0;
        int _resultsPerPage = rgReport.PageSize;

        int? _clientId = ddlClient.ClientId;
        int? _employeeId = ddlEmployee.EmployeeId > 0 ? ddlEmployee.EmployeeId : (int?)null;
        DateTime? _startDate = ucStartDate.SelectedDate;
        DateTime? _endDate = ucEndDate.SelectedDate;
       

        #region count

        var _dtProjectCount = DesktopShared.StoredProcedureCallerClasses.RetrievalProcedures.ProcSearchHourlyProjectsCount(
            _clientId, //client id
            _employeeId, //employee id
            _startDate, //start date
            _endDate //end date
            ); ;
        if (_dtProjectCount.Rows.Count > 0)
            _resultCount = Convert.ToInt32(_dtProjectCount.Rows[0][0].ToString());

        #endregion

        #region data table

        var _dtProject = DesktopShared.StoredProcedureCallerClasses.RetrievalProcedures.ProcSearchHourlyProjects(
            _clientId, //client id
            _employeeId, //employee id
            _startDate, //start date
            _endDate, //end date           
            _resultsPerPage, //page size
           _currentPage, //current page
           SortColumnName, //sort by
           SortOperator == SD.LLBLGen.Pro.ORMSupportClasses.SortOperator.Ascending ? "asc" : "desc" //sort direction
            );

        #endregion

        rgReport.VirtualItemCount = _resultCount;
        rgReport.DataSource = _dtProject;
        ResetSearch = false;

        #region grand total
        
        var _dtProjectAmount = DesktopShared.StoredProcedureCallerClasses.RetrievalProcedures.ProcSearchHourlyProjectsBillTotal(
            _clientId, //client id
            _employeeId, //employee id
            _startDate, //start date
            _endDate //end date
            ); ;
        if (_dtProjectAmount.Rows.Count > 0)
        {
            decimal _total = 0;
            if (decimal.TryParse(_dtProjectAmount.Rows[0][0].ToString(), out _total))
                litGrandTotal.Text = _total.ToString("C");
        }

        #endregion
    }

    /// <summary>
    /// grid on sort command
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rgReport_SortCommand(object sender, GridSortCommandEventArgs e)
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
        rgReport.CurrentPageIndex = 0;
        rgReport.Rebind();

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
    protected void rgReport_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        #region Item or AlternatingItem

        if (e.Item.ItemType == GridItemType.Item || e.Item.ItemType == GridItemType.AlternatingItem)
        {
            GridEditableItem item = e.Item as GridEditableItem;
            DataRowView drv = (DataRowView)e.Item.DataItem;

            int _clientId = 0;
            int.TryParse(drv["fk_client"].ToString(), out _clientId);
            TelerikHelper.AddHyperLinkToCell(drv["ClientCompany"].ToString().Trim(), String.Format("/Client/clientdetail.aspx?ClientId={0}", _clientId), item["ClientCompany"]);

            int _projectId = 0;
            int.TryParse(drv["fk_project"].ToString(), out _projectId);
            TelerikHelper.AddHyperLinkToCell(drv["ProjectDescr"].ToString().Trim(), String.Format("/Client/ClientProjectAdd.aspx?ProjectID={0}&ClientId={1}", _projectId, _clientId), item["ProjectDescr"]);

            TelerikHelper.AddLabelToCell(drv["bilrate"].ToString().Trim(), item["bilrate"]);
            TelerikHelper.AddLabelToCell(drv["Dollars"].ToString().Trim(), item["Dollars"]);

            decimal _hours = 0;
            decimal.TryParse(drv["hours"].ToString(), out _hours);
            LinkButton btnViewHours = (LinkButton)e.Item.FindControl("btnViewHours");
            btnViewHours.Text = _hours.ToString();
            btnViewHours.ToolTip = _hours.ToString();
            btnViewHours.CommandArgument = _projectId.ToString();  
        }

        #endregion
    }

    /// <summary>
    /// grid on pre render
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rgReport_PreRender(object sender, EventArgs e)
    {
        if (DesktopShared.Utility.BrowserHelper.IsTabletOrSmartPhone(this.Request))
        {
            int _colIndex = -1;
            rgReport.MasterTableView.Columns[++_colIndex].HeaderStyle.Width = Unit.Pixel(300);
            rgReport.MasterTableView.Columns[++_colIndex].HeaderStyle.Width = Unit.Pixel(300);
            rgReport.MasterTableView.Columns[++_colIndex].HeaderStyle.Width = Unit.Pixel(75);
            rgReport.MasterTableView.Columns[++_colIndex].HeaderStyle.Width = Unit.Pixel(75);
            rgReport.MasterTableView.Columns[++_colIndex].HeaderStyle.Width = Unit.Pixel(75);
            rgReport.MasterTableView.Columns[++_colIndex].HeaderStyle.Width = Unit.Pixel(75);
        }
    }

    /// <summary>
    /// grid on item command
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rgReport_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName == "ViewHours")
        {
            ModalIsOpen = true;
            ShowTimesheetModal();
            
            litModalHeader.Text = (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["ProjectDescr"].ToString().Trim();

            int _projectId = int.Parse((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["fk_project"].ToString());
            gridTimesheet.Visible = true;
            gridTimesheet.SearchStartDate = ucStartDate.SelectedDate.HasValue ? ucStartDate.SelectedDate.Value : DateTime.MinValue;
            gridTimesheet.SearchEndDate = ucEndDate.SelectedDate.HasValue ? ucEndDate.SelectedDate.Value : DateTime.MinValue;
            gridTimesheet.SearchProjectId = _projectId;
            gridTimesheet.ResetGrid();
        }
    }

    #endregion

    /// <summary>
    /// submit button click
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
            RebindGrid();
    }

    /// <summary>
    /// clear button on click
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnClear_Click(object sender, EventArgs e)
    {
        Response.Redirect(Request.RawUrl);
    }

    /// <summary>
    /// export button click
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnExport_Click(object sender, System.EventArgs e)
    {
        ConfigureExport();
        rgReport.MasterTableView.ExportToCSV();
    }

    /// <summary>
    /// close timesheet modal
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnCloseTimesheetModal_Click(object sender, EventArgs e)
    {
        gridTimesheet.Visible = false;
        ModalIsOpen = false;
        ScriptManager.RegisterStartupScript(this, this.GetType(), "CloseTimesheetModal", "CloseTimesheetModal();", true);
        RebindGrid();
    }

    #region custom validators

    /// <summary>
    /// validate dates
    /// </summary>
    /// <param name="source"></param>
    /// <param name="args"></param>
    protected void cvDates_ServerValidate(object source, ServerValidateEventArgs args)
    {
        args.IsValid = true;
        if (!ucStartDate.SelectedDate.HasValue || !ucEndDate.SelectedDate.HasValue)
            return;
        args.IsValid = ucEndDate.SelectedDate.Value.Date >= ucStartDate.SelectedDate.Value.Date;
    }

    #endregion

    #endregion

    #region private properties

    /// <summary>
    /// get/set sort column name
    /// </summary>
    private string SortColumnName
    {
        get
        {
            object obj = this.ViewState["scn_ph"];
            return (obj == null) ? "ClientCompany" : (string)obj;
        }
        set { this.ViewState["scn_ph"] = value; }
    }

    /// <summary>
    /// get/set sort operator
    /// </summary>
    private SD.LLBLGen.Pro.ORMSupportClasses.SortOperator SortOperator
    {
        get
        {
            object obj = this.ViewState["so_ph"];
            return (obj == null) ? SD.LLBLGen.Pro.ORMSupportClasses.SortOperator.Ascending : (SD.LLBLGen.Pro.ORMSupportClasses.SortOperator)obj;
        }
        set { this.ViewState["so_ph"] = value; }
    }

    /// <summary>
    /// get/set reset search
    /// </summary>
    private bool ResetSearch
    {
        get
        {
            object obj = this.ViewState["rs_ph"];
            return (obj == null) ? false : (bool)obj;
        }
        set { this.ViewState["rs_ph"] = value; }

    }

    /// <summary>
    /// get/set modal is open
    /// </summary>
    private bool ModalIsOpen
    {
        get
        {
            object obj = this.ViewState["mio_ph"];
            return (obj == null) ? false : (bool)obj;
        }
        set { this.ViewState["mio_ph"] = value; }

    }

    #endregion





    
}