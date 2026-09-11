using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class Reports_ProjectMonthly : BasePage
{
    /// <summary>
    /// page load
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        ConfigureForDevice();
        PopulateDropDownLists();
        if (!IsPostBack)
            SetUpPage();

        if (ModalIsOpen)
            ShowTimesheetModal();
    }

    #region private methods

    /// <summary>
    /// populate drop down lists
    /// </summary>
    private void PopulateDropDownLists()
    {
        if (ddlYear.Items.Count ==0)
        {
            for (int i = 1996; i < DateTime.Now.Year + 1; i++)
                ddlYear.Items.Insert(i - 1996, new ListItem(i.ToString(), i.ToString()));
        }
        if (ddlMonth.Items.Count == 0)
        {
            for (int i = 1; i < 13; i++)
                ddlMonth.Items.Insert(i - 1, new ListItem(i.ToString().PadLeft(2, '0'), i.ToString()));
        }
    }

    /// <summary>
    /// set up for initial page load
    /// </summary>
    private void SetUpPage()
    {
        ddlClient.Focus();

        short _tabIndex = 0;
        ddlClient.TabIndex = ++_tabIndex;
        ddlEmployee.TabIndex = ++_tabIndex;
        ddlProjectStatus.TabIndex = ++_tabIndex;
        ddlYear.TabIndex = ++_tabIndex;
        ddlMonth.TabIndex = ++_tabIndex;
        ddlClientStatus.TabIndex = ++_tabIndex;
        btnSubmit.TabIndex = ++_tabIndex;
        btnClear.TabIndex = ++_tabIndex;

        DateTime _now = DateTime.Now;
        ListItem _li = ddlYear.Items.FindByValue(_now.Year.ToString());
        if (_li != null)
            _li.Selected = true;
        _li = ddlMonth.Items.FindByValue(_now.Month.ToString());
        if (_li != null)
            _li.Selected = true;
    }

    /// <summary>
    /// show timesheet model
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
        ddlProjectStatus.CssClass = _cssClass;
        ddlYear.CssClass = _cssClass;
        ddlMonth.CssClass = _cssClass;
        ddlClientStatus.CssClass = _cssClass;
    }

    /// <summary>
    /// rebind document grid
    /// </summary>
    private void RebindGrid()
    {
        phSearchResults.Visible = true;
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

        rgReport.ExportSettings.FileName = String.Format("MonthlProjects_{0}{1}_{2}", ddlYear.SelectedValue.Trim(), ddlMonth.SelectedValue.Trim(), DateTime.Now.ToString("yyyyMMdd-hhmmss"));

        //hide command item template
        foreach (GridItem commandItem in this.rgReport.MasterTableView.GetItems(GridItemType.CommandItem))
        {
            commandItem.Visible = false;
            commandItem.Display = false;
        }

        rgReport.MasterTableView.Columns.FindByUniqueName("HoursLoggedForExport").Visible = true;
        rgReport.MasterTableView.Columns.FindByUniqueName("HoursLogged").Visible = false;
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
        
        int? _clientId = ddlClient.ClientId;
        int? _employeeId = ddlEmployee.EmployeeId > 0 ? ddlEmployee.EmployeeId : (int?)null;

        DataTable _dtReport = DesktopShared.StoredProcedureCallerClasses.RetrievalProcedures.ProcSearchMonthlyProjects(_clientId, _employeeId, Convert.ToInt32(ddlMonth.SelectedValue), Convert.ToInt32(ddlYear.SelectedValue), ddlProjectStatus.SelectedValue);
        _dtReport.Columns.Add("ExtraTime", typeof(decimal));
        _dtReport.Columns.Add("AdditionalCharge", typeof(decimal));
        _dtReport.Columns.Add("Sales", typeof(string));

        for (int i = _dtReport.Rows.Count - 1; i >= 0; i--)
        {
            DataRow _row = _dtReport.Rows[i];
            int _rowClientId = Convert.ToInt32(_row["fk_client"].ToString());

            _row["Sales"] = DesktopShared.Client.GetSalesForDisplay(_rowClientId);
            _row["ExtraTime"] = 0;
            _row["AdditionalCharge"] = 0;

            decimal _monthlyHours = 0;
            decimal.TryParse(_row["mainthoursavail"].ToString(), out _monthlyHours);
            decimal _hours = 0;
            decimal.TryParse(_row["projhrs"].ToString(), out _hours);
            if ((_hours > _monthlyHours) && (_monthlyHours > 0))
            {
                decimal _extraTime = _hours - _monthlyHours;
                _row["ExtraTime"] = _extraTime;

                decimal _billRate = 0;
                if (decimal.TryParse(_row["bilrate"].ToString(), out _billRate))
                    _row["AdditionalCharge"] = (_billRate * _extraTime).ToString();
            }
            else if (ddlClientStatus.SelectedValue == "A")
                _row.Delete();
        }
        _dtReport.AcceptChanges();

        rgReport.DataSource = _dtReport;
        litGrandTotal.Text = Convert.ToDecimal(_dtReport.Compute("Sum(AdditionalCharge)", String.Empty)).ToString("C");


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
            int.TryParse(drv["pproject"].ToString(), out _projectId);
            TelerikHelper.AddHyperLinkToCell(drv["ProjectDescr"].ToString().Trim(), String.Format("/Client/ClientProjectAdd.aspx?ProjectID={0}&ClientId={1}", _projectId, _clientId), item["ProjectDescr"]);

            TelerikHelper.AddLabelToCell(drv["Sales"].ToString().Trim(), drv["Sales"].ToString().Trim().ToString().Replace("<br />", Environment.NewLine), item["Sales"]);

            decimal _tempDec = 0;
            string _monthlyCharge = drv["laborprice"].ToString().Trim();
            if (decimal.TryParse(_monthlyCharge, out _tempDec))
            {
                if (_tempDec == 0)
                    _monthlyCharge = "Variable";
                else
                    _monthlyCharge = _tempDec.ToString("C").Replace("$", "");
            }
            TelerikHelper.AddLabelToCell(_monthlyCharge, item["MonthlyCharge"]);

            TelerikHelper.AddLabelToCell(drv["mainthoursavail"].ToString().Trim(), item["MonthlyHours"]);

            decimal _hours = 0;
            decimal.TryParse(drv["projhrs"].ToString(), out _hours);
            LinkButton btnViewHours = (LinkButton)e.Item.FindControl("btnViewHours");
            btnViewHours.ToolTip = _hours.ToString();
            btnViewHours.Text = _hours.ToString();
            btnViewHours.CommandArgument = _projectId.ToString();

            TelerikHelper.AddLabelToCell(drv["ExtraTime"].ToString().Trim(), item["ExtraTime"]);

            TelerikHelper.AddLabelToCell(drv["bilrate"].ToString().Trim(), item["Rate"]);

            string _additionalChargeDisplay = "NC";
            decimal _additionalCharge = 0;
            if (decimal.TryParse(drv["AdditionalCharge"].ToString().Trim(), out _additionalCharge))
            {
                if (_additionalCharge > 0)
                    _additionalChargeDisplay = _additionalCharge.ToString();
            }
            TelerikHelper.AddLabelToCell(_additionalChargeDisplay, item["AdditionalCharge"]);
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
            rgReport.MasterTableView.Columns[++_colIndex].HeaderStyle.Width = Unit.Pixel(150);
            rgReport.MasterTableView.Columns[++_colIndex].HeaderStyle.Width = Unit.Pixel(125);
            rgReport.MasterTableView.Columns[++_colIndex].HeaderStyle.Width = Unit.Pixel(125);
            rgReport.MasterTableView.Columns[++_colIndex].HeaderStyle.Width = Unit.Pixel(125);
            rgReport.MasterTableView.Columns[++_colIndex].HeaderStyle.Width = Unit.Pixel(100);
            rgReport.MasterTableView.Columns[++_colIndex].HeaderStyle.Width = Unit.Pixel(100);
            rgReport.MasterTableView.Columns[++_colIndex].HeaderStyle.Width = Unit.Pixel(100);
            rgReport.MasterTableView.Columns[++_colIndex].HeaderStyle.Width = Unit.Pixel(100);
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

            int _projectId = int.Parse((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["pproject"].ToString());
            DateTime _startDate = new DateTime(Convert.ToInt32(ddlYear.SelectedValue), Convert.ToInt32(ddlMonth.SelectedValue), 1);
            gridTimesheet.Visible = true;
            gridTimesheet.SearchStartDate = _startDate;
            gridTimesheet.SearchEndDate = DesktopShared.Utility.Date.GetLastDayOfMonth(_startDate);
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

   
    #endregion

    #region private properties

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