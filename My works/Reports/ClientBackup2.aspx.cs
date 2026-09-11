using SD.LLBLGen.Pro.ORMSupportClasses;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class Reports_ClientBackup2 : BasePage
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
    }

    #region private methods

    /// <summary>
    /// set up for initial page load
    /// </summary>
    private void SetUpPage()
    {
        txtServer.Focus();

        short _tabIndex = 0;
        txtServer.TabIndex = ++_tabIndex;
        ddlClient.TabIndex = ++_tabIndex;
        ddlType.TabIndex = ++_tabIndex;
        ddlStatus.TabIndex = ++_tabIndex;
        ucStartDate.TabIndex = ++_tabIndex;
        ucEndDate.TabIndex = ++_tabIndex;
        btnSubmit.TabIndex = ++_tabIndex;
        btnClear.TabIndex = ++_tabIndex;

        ucEndDate.SelectedDate = DateTime.Now.AddDays(-1);
        ucStartDate.SelectedDate = ucEndDate.SelectedDate.Value;

        ddlStatus.Populate();
        DropDownList _ddl = ddlStatus.GetDropDownList();
        ListItem _liTbd = _ddl.Items.FindByValue(DesktopShared.Backup.Monitor.Status.ID.Tbd.ToString());
        if (_liTbd != null)
            _ddl.Items.Remove(_liTbd);

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
            phTabletPagerCss.Visible = true;
        }

        ddlClient.DisplayChosenScript = _displayChosenScript;
        ddlClient.CssClass = _cssClass;
        ddlType.CssClass = _cssClass;
        ddlStatus.DisplayChosenScript = _displayChosenScript;
        ddlStatus.CssClass = _cssClass;

        if (_displayChosenScript)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append("<script type='text/javascript'>");
            sb.Append("$(document).ready(function() {");
            sb.Append(String.Format("SetChosenType_{0}();", ddlType.ClientID));
            sb.Append(String.Format("Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(SetChosenType_{0})", ddlType.ClientID));
            sb.Append("});");
            sb.Append(String.Format("function SetChosenType_{0}(sender, args) {{", ddlType.ClientID));
            sb.Append(String.Format("$('#{0}').chosen({{", ddlType.ClientID));
            sb.Append("allow_single_deselect: true,");
            sb.Append("placeholder_text_single: \"Select a Type ...\",");
            sb.Append("width: \"100%\"");
            sb.Append("});");
            sb.Append("}");
            sb.Append("</script>");
            litTypeJs.Text = sb.ToString();
        }
    }

    /// <summary>
    /// rebind document grid
    /// </summary>
    private void RebindGrid()
    {
        litMessage.Text = "";
        phSearchResults.Visible = true;
        rgClientBackupReport.Visible = true;
        rgClientBackupReport.EditIndexes.Clear();
        rgClientBackupReport.DataSource = null;
        rgClientBackupReport.Rebind();
    }

    #endregion

    #region protected events

    #region telerik grid

    /// <summary>
    /// grid on need data source
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rgClientBackupReport_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        int _currentPage = ResetSearch ? 1 : rgClientBackupReport.CurrentPageIndex + 1;
        int _resultCount = 0;
        int _resultsPerPage = rgClientBackupReport.PageSize;

        int? _clientId = ddlClient.ClientId;
        string _server = txtServer.Text.Trim();
        string _type = ddlType.SelectedValue.Trim();
        int? _statusId = ddlStatus.StatusId > 0 ? ddlStatus.StatusId : (int?)null;
        DateTime? _startDate = ucStartDate.SelectedDate;
        DateTime? _endDate = ucEndDate.SelectedDate;

        var _dtCount = DesktopShared.StoredProcedureCallerClasses.RetrievalProcedures.ProcSearchBackupMonitorCount(_clientId, _server, _type, _startDate, _endDate, _statusId);
        if (_dtCount.Rows.Count > 0)
            _resultCount = Convert.ToInt32(_dtCount.Rows[0][0].ToString());

        var _dtReport = DesktopShared.StoredProcedureCallerClasses.RetrievalProcedures.ProcSearchBackupMonitor(_clientId, _server, _type, _startDate, _endDate, _statusId,
        _resultsPerPage, _currentPage, SortColumnName, SortOperator == SD.LLBLGen.Pro.ORMSupportClasses.SortOperator.Ascending ? "asc" : "desc");

        rgClientBackupReport.VirtualItemCount = _resultCount;
        rgClientBackupReport.DataSource = _dtReport;
        ResetSearch = false;
    }

    /// <summary>
    /// grid on item data bound
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rgClientBackupReport_ItemDataBound(object sender, GridItemEventArgs e)
    {
        #region Item or AlternatingItem

        if (e.Item.ItemType == GridItemType.Item || e.Item.ItemType == GridItemType.AlternatingItem)
        {
            GridDataItem _gdi = e.Item as GridDataItem;
            DataRowView _drv = (DataRowView)e.Item.DataItem;

            TelerikHelper.AddLabelToCell(_drv["Server"].ToString(), _gdi["Server"]);
            TelerikHelper.AddLabelToCell(_drv["Company"].ToString(), _gdi["Company"]);
            TelerikHelper.AddLabelToCell(_drv["JobTypeName"].ToString(), _gdi["JobTypeName"]);
            TelerikHelper.AddLabelToCell(_drv["Status"].ToString(), _gdi["Status"]);

            int _statusId = 0;
            if (int.TryParse(_drv["StatusId"].ToString(), out _statusId))
            {
                string _bgColor = "";
                string _color = "";

                if (_statusId == DesktopShared.Backup.Monitor.Status.ID.Successful)
                {
                    _bgColor = DesktopShared.Bootstrap.BackgroundColor(DesktopShared.Bootstrap.Alert.AlertType.Success);
                    _color = DesktopShared.Bootstrap.Color(DesktopShared.Bootstrap.Alert.AlertType.Success);
                }
                else if (_statusId == DesktopShared.Backup.Monitor.Status.ID.UnSuccessful)
                {
                    _bgColor = DesktopShared.Bootstrap.BackgroundColor(DesktopShared.Bootstrap.Alert.AlertType.Danger);
                    _color = DesktopShared.Bootstrap.Color(DesktopShared.Bootstrap.Alert.AlertType.Danger);
                }
                else if (_statusId == DesktopShared.Backup.Monitor.Status.ID.Warning)
                {
                    _bgColor = DesktopShared.Bootstrap.BackgroundColor(DesktopShared.Bootstrap.Alert.AlertType.Warning);
                    _color = DesktopShared.Bootstrap.Color(DesktopShared.Bootstrap.Alert.AlertType.Warning);
                }

                if (!String.IsNullOrWhiteSpace(_bgColor))
                    _gdi["Status"].Attributes.CssStyle.Add("background-color", _bgColor);
                if (!String.IsNullOrWhiteSpace(_color))
                    _gdi["Status"].Attributes.CssStyle.Add("color", _color);
            }

            string _displayValue = "";
            DateTime _backupDate = DateTime.MinValue;
            if (DateTime.TryParse(_drv["BackupDate"].ToString(), out _backupDate))
                _displayValue = _backupDate.ToString("MM/dd/yy HH:mm");
            TelerikHelper.AddLabelToCell(_displayValue, _gdi["BackupDate"]);

            _displayValue = "";
            _backupDate = DateTime.MinValue;
            if (DateTime.TryParse(_drv["LastSuccessBackupDate_Local"].ToString(), out _backupDate))
                _displayValue = _backupDate.ToString("MM/dd/yy HH:mm");
            TelerikHelper.AddLabelToCell(_displayValue, _gdi["LastSuccessBackupDate_Local"]);

            _displayValue = "";
            _backupDate = DateTime.MinValue;
            if (DateTime.TryParse(_drv["LastSuccessBackupDate_Offsite"].ToString(), out _backupDate))
                _displayValue = _backupDate.ToString("MM/dd/yy HH:mm");
            TelerikHelper.AddLabelToCell(_displayValue, _gdi["LastSuccessBackupDate_Offsite"]);
            _displayValue = "";
            _backupDate = DateTime.MinValue;
            if (DateTime.TryParse(_drv["LastSuccessBackupDate_Replication"].ToString(), out _backupDate))
                _displayValue = _backupDate.ToString("MM/dd/yy HH:mm");
            TelerikHelper.AddLabelToCell(_displayValue, _gdi["LastSuccessBackupDate_Replication"]);
        }

        #endregion
    }

    /// <summary>
    /// grid on sort command
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rgClientBackupReport_SortCommand(object sender, GridSortCommandEventArgs e)
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
        rgClientBackupReport.CurrentPageIndex = 0;
        rgClientBackupReport.Rebind();

    }

    /// <summary>
    /// grid on pre render
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rgClientBackupReport_PreRender(object sender, EventArgs e)
    {
        if (DesktopShared.Utility.BrowserHelper.IsTabletOrSmartPhone(this.Request))
        {
            int _colIndex = -1;

            rgClientBackupReport.MasterTableView.Columns[++_colIndex].HeaderStyle.Width = Unit.Pixel(225);
            rgClientBackupReport.MasterTableView.Columns[++_colIndex].HeaderStyle.Width = Unit.Pixel(225);
            rgClientBackupReport.MasterTableView.Columns[++_colIndex].HeaderStyle.Width = Unit.Pixel(125);
            rgClientBackupReport.MasterTableView.Columns[++_colIndex].HeaderStyle.Width = Unit.Pixel(125);
            rgClientBackupReport.MasterTableView.Columns[++_colIndex].HeaderStyle.Width = Unit.Pixel(125);
            rgClientBackupReport.MasterTableView.Columns[++_colIndex].HeaderStyle.Width = Unit.Pixel(125);
            rgClientBackupReport.MasterTableView.Columns[++_colIndex].HeaderStyle.Width = Unit.Pixel(125);
            rgClientBackupReport.MasterTableView.Columns[++_colIndex].HeaderStyle.Width = Unit.Pixel(125);
        }
    }

    #endregion

    /// <summary>
    /// submit button on click
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            phSearchResults.Visible = true;
            ResetSearch = true;
            RebindGrid();
        }
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
            object obj = this.ViewState["scn_cb"];
            return (obj == null) ? "BackupDate" : (string)obj;
        }
        set { this.ViewState["scn_cb"] = value; }
    }

    /// <summary>
    /// get/set sort operator
    /// </summary>
    private SD.LLBLGen.Pro.ORMSupportClasses.SortOperator SortOperator
    {
        get
        {
            object obj = this.ViewState["so_cb"];
            return (obj == null) ? SD.LLBLGen.Pro.ORMSupportClasses.SortOperator.Descending : (SD.LLBLGen.Pro.ORMSupportClasses.SortOperator)obj;
        }
        set { this.ViewState["so_cb"] = value; }
    }

    /// <summary>
    /// get/set reset search
    /// </summary>
    public bool ResetSearch
    {
        get
        {
            object obj = this.ViewState["rs_cb"];
            return (obj == null) ? false : (bool)obj;
        }
        set { this.ViewState["rs_cb"] = value; }
    }

    #endregion
}

