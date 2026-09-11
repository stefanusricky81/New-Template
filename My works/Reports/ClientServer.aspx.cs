using SD.LLBLGen.Pro.ORMSupportClasses;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class Reports_ClientServer : BasePage
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
        {
            int _id;
            if (Int32.TryParse(Request.QueryString["ClientId"], out _id))
                ddlClient.ClientId = _id;
            SetUpPage();
        }
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
        btnSubmit.TabIndex = ++_tabIndex;
        btnClear.TabIndex = ++_tabIndex;
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
        var _dtCount = DesktopShared.StoredProcedureCallerClasses.RetrievalProcedures.ProcSearchBackupServerCount(_clientId, _server);
        if (_dtCount.Rows.Count > 0)
            _resultCount = Convert.ToInt32(_dtCount.Rows[0][0].ToString());

        var _dtReport = DesktopShared.StoredProcedureCallerClasses.RetrievalProcedures.ProcSearchBackupServer(_clientId, _server, 
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

            //local attempt
            string _displayValue = "";
            DateTime _backupDate = DateTime.MinValue;
            if (DateTime.TryParse(_drv["LastBackupDate_Local"].ToString(), out _backupDate))
                _displayValue = _backupDate.ToString("MM/dd/yy HH:mm");
            TelerikHelper.AddLabelToCell(_displayValue, _gdi["LastBackupDate_Local"]);

            //local success
            _displayValue = "";
            _backupDate = DateTime.MinValue;
            if (DateTime.TryParse(_drv["LastSuccessBackupDate_Local"].ToString(), out _backupDate))
                _displayValue = _backupDate.ToString("MM/dd/yy HH:mm");
            TelerikHelper.AddLabelToCell(_displayValue, _gdi["LastSuccessBackupDate_Local"]);

            //offsite attempt
            _displayValue = "";
            _backupDate = DateTime.MinValue;
            if (DateTime.TryParse(_drv["LastBackupDate_Offsite"].ToString(), out _backupDate))
                _displayValue = _backupDate.ToString("MM/dd/yy HH:mm");
            TelerikHelper.AddLabelToCell(_displayValue, _gdi["LastBackupDate_Offsite"]);

            //offsite success
            _displayValue = "";
            _backupDate = DateTime.MinValue;
            if (DateTime.TryParse(_drv["LastSuccessBackupDate_Offsite"].ToString(), out _backupDate))
                _displayValue = _backupDate.ToString("MM/dd/yy HH:mm");
            TelerikHelper.AddLabelToCell(_displayValue, _gdi["LastSuccessBackupDate_Offsite"]);

            //replication attempt
            _displayValue = "";
            _backupDate = DateTime.MinValue;
            if (DateTime.TryParse(_drv["LastBackupDate_Replication"].ToString(), out _backupDate))
                _displayValue = _backupDate.ToString("MM/dd/yy HH:mm");
            TelerikHelper.AddLabelToCell(_displayValue, _gdi["LastBackupDate_Replication"]);

            //replication success
            _displayValue = "";
            _backupDate = DateTime.MinValue;
            if (DateTime.TryParse(_drv["LastSuccessBackupDate_Replication"].ToString(), out _backupDate))
                _displayValue = _backupDate.ToString("MM/dd/yy HH:mm");
            TelerikHelper.AddLabelToCell(_displayValue, _gdi["LastSuccessBackupDate_Replication"]);

            //highlight warnings
            int _backupMonitorStatusIdLocal = BitByBit.Utility.DataBase.ConvertHelper.ConvertToInteger(_drv["LastSuccessBackupMonitorStatusId_Local"], -1);
            if (_backupMonitorStatusIdLocal == DesktopShared.Backup.Monitor.Status.ID.Warning)
            {
                _gdi["LastSuccessBackupDate_Local"].Attributes.CssStyle.Add("background-color", DesktopShared.Bootstrap.BackgroundColor(DesktopShared.Bootstrap.Alert.AlertType.Warning));
                _gdi["LastSuccessBackupDate_Local"].Attributes.CssStyle.Add("color", DesktopShared.Bootstrap.Color(DesktopShared.Bootstrap.Alert.AlertType.Warning));
            }
            int _backupMonitorStatusIdOffsite = BitByBit.Utility.DataBase.ConvertHelper.ConvertToInteger(_drv["LastSuccessBackupMonitorStatusId_Offsite"], -1);
            if (_backupMonitorStatusIdOffsite == DesktopShared.Backup.Monitor.Status.ID.Warning)
            {
                _gdi["LastSuccessBackupDate_Offsite"].Attributes.CssStyle.Add("background-color", DesktopShared.Bootstrap.BackgroundColor(DesktopShared.Bootstrap.Alert.AlertType.Warning));
                _gdi["LastSuccessBackupDate_Offsite"].Attributes.CssStyle.Add("color", DesktopShared.Bootstrap.Color(DesktopShared.Bootstrap.Alert.AlertType.Warning));
            }
            int _backupMonitorStatusIdReplication = BitByBit.Utility.DataBase.ConvertHelper.ConvertToInteger(_drv["LastSuccessBackupMonitorStatusId_Replication"], -1);
            if (_backupMonitorStatusIdReplication == DesktopShared.Backup.Monitor.Status.ID.Warning)
            {
                _gdi["LastSuccessBackupDate_Replication"].Attributes.CssStyle.Add("background-color", DesktopShared.Bootstrap.BackgroundColor(DesktopShared.Bootstrap.Alert.AlertType.Warning));
                _gdi["LastSuccessBackupDate_Replication"].Attributes.CssStyle.Add("color", DesktopShared.Bootstrap.Color(DesktopShared.Bootstrap.Alert.AlertType.Warning));
            }
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
            rgClientBackupReport.MasterTableView.Columns[++_colIndex].HeaderStyle.Width = Unit.Pixel(150);
            rgClientBackupReport.MasterTableView.Columns[++_colIndex].HeaderStyle.Width = Unit.Pixel(150);
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
            return (obj == null) ? "Server" : (string)obj;
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
            return (obj == null) ? SD.LLBLGen.Pro.ORMSupportClasses.SortOperator.Ascending : (SD.LLBLGen.Pro.ORMSupportClasses.SortOperator)obj;
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

