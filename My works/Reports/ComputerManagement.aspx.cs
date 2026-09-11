using iTextSharp.text.pdf.parser;
using SD.LLBLGen.Pro.ORMSupportClasses;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows.Forms.VisualStyles;
using Telerik.Web.UI;

public partial class Reports_ComputerManagement : BasePage
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
        txtComputerName.Focus();

        short _tabIndex = 0;
        ddlClient.TabIndex = ++_tabIndex;
        txtComputerName.TabIndex = ++_tabIndex;
        lbManagedBy.TabIndex = ++_tabIndex;
        ddlActive.TabIndex = ++_tabIndex;
        ddlServerWorkstation.TabIndex = ++_tabIndex;
        ucLastBackupStartDate.TabIndex = ++_tabIndex;
        ucLastBackupEndDate.TabIndex = ++_tabIndex;
        ucLastPatchStartDate.TabIndex = ++_tabIndex;
        ucLastPatchEndDate.TabIndex = ++_tabIndex;
        ucLastActiveStartDate.TabIndex = ++_tabIndex;
        ucLastActiveEndDate.TabIndex = ++_tabIndex;
        btnSubmit.TabIndex = ++_tabIndex;
        btnClear.TabIndex = ++_tabIndex;

        bool _managedby;
        if (! Boolean.TryParse(Request.QueryString["ManagedBy"], out _managedby))
        {
            foreach (ListItem item in lbManagedBy.Items)
                item.Selected = true;
        }

        ListItem liActive = ddlActive.Items.FindByValue("1");
        if (liActive != null)
            liActive.Selected = true;
    }

    /// <summary>
    /// configure for device
    /// </summary>
    private void ConfigureForDevice()
    {
        #region chosen js

        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        sb.Append("<script type='text/javascript'>");
        sb.Append("$(document).ready(function() {");
        sb.Append(String.Format("SetChosenManagedBy_{0}();", this.ClientID));
        sb.Append(String.Format("Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(SetChosenManagedBy_{0})", this.ClientID));
        sb.Append("});");
        sb.Append(String.Format("function SetChosenManagedBy_{0}(sender, args) {{", this.ClientID));
        sb.Append(String.Format("$('#{0}').chosen({{", lbManagedBy.ClientID));
        sb.Append("allow_single_deselect: true,");
        sb.Append("placeholder_text_multiple: \"Select Managed By ...\",");
        sb.Append("width: \"100%\"");
        sb.Append("});");
        sb.Append("}");
        sb.Append("</script>");
        litManagedByJs.Text = sb.ToString();

        sb = new System.Text.StringBuilder();
        sb.Append("<script type='text/javascript'>");
        sb.Append("$(document).ready(function() {");
        sb.Append(String.Format("SetChosenServerWorkstation_{0}();", this.ClientID));
        sb.Append(String.Format("Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(SetChosenServerWorkstation_{0})", this.ClientID));
        sb.Append("});");
        sb.Append(String.Format("function SetChosenServerWorkstation_{0}(sender, args) {{", this.ClientID));
        sb.Append(String.Format("$('#{0}').chosen({{", ddlServerWorkstation.ClientID));
        sb.Append("allow_single_deselect: true,");
        sb.Append("placeholder_text_single: \"Select a Type ...\",");
        sb.Append("width: \"100%\"");
        sb.Append("});");
        sb.Append("}");
        sb.Append("</script>");
        litServerWorkstationJs.Text = sb.ToString();

        sb = new System.Text.StringBuilder();
        sb.Append("<script type='text/javascript'>");
        sb.Append("$(document).ready(function() {");
        sb.Append(String.Format("SetChosenActive_{0}();", this.ClientID));
        sb.Append(String.Format("Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(SetChosenActive_{0})", this.ClientID));
        sb.Append("});");
        sb.Append(String.Format("function SetChosenActive_{0}(sender, args) {{", this.ClientID));
        sb.Append(String.Format("$('#{0}').chosen({{", ddlActive.ClientID));
        sb.Append("allow_single_deselect: true,");
        sb.Append("placeholder_text_single: \"Select Active ...\",");
        sb.Append("width: \"100%\"");
        sb.Append("});");
        sb.Append("}");
        sb.Append("</script>");
        litActiveJs.Text = sb.ToString();

        #endregion

        bool _displayChosenScript = true;
        string _cssClass = "form-control select-chosen";
        if (DesktopShared.Utility.BrowserHelper.IsTabletOrSmartPhone(this.Request))
        {
            _displayChosenScript = false;
            _cssClass = "form-control";
            phTabletPagerCss.Visible = true;
        }
        else
        {
            litManagedByJs.Visible = true;
            litServerWorkstationJs.Visible = true;
            litActiveJs.Visible = true;
        }

        ddlActive.CssClass = _cssClass;
        ddlServerWorkstation.CssClass = _cssClass;
        lbManagedBy.CssClass = _cssClass;
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

        rgReport.ExportSettings.FileName = "ComputerManagement_" + DateTime.Now.ToString("yyyyMMdd-hhmmss");

        //hide link columns
        rgReport.MasterTableView.Columns.FindByUniqueName("Action").Visible = false;

    }

    /// <summary>
    /// configure grid for client report export (expose additional/hidden columns)
    /// </summary>
    private void ConfigureClientReportExport()
    {
        rgReport.ExportSettings.IgnorePaging = true;
        rgReport.ExportSettings.OpenInNewWindow = true;
        rgReport.ExportSettings.ExportOnlyData = true;
        rgReport.ExportSettings.HideStructureColumns = true;

        rgReport.ExportSettings.FileName = "ComputerManagement_" + DateTime.Now.ToString("yyyyMMdd-hhmmss");

        //hide link columns
        rgReport.MasterTableView.Columns.FindByUniqueName("Action").Visible = false;

        //display hidden columns
        rgReport.MasterTableView.Columns.FindByUniqueName("MachineId").Visible = true;
        rgReport.MasterTableView.Columns.FindByUniqueName("CurrentUser").Visible = true;
        rgReport.MasterTableView.Columns.FindByUniqueName("DomainWorkgroup").Visible = true;
        rgReport.MasterTableView.Columns.FindByUniqueName("IpAddress").Visible = true;
        rgReport.MasterTableView.Columns.FindByUniqueName("DefaultGateway").Visible = true;
        rgReport.MasterTableView.Columns.FindByUniqueName("ConnectionGateway").Visible = true;
        rgReport.MasterTableView.Columns.FindByUniqueName("Manufacturer").Visible = true;
        rgReport.MasterTableView.Columns.FindByUniqueName("SystemSerialNumber").Visible = true;
        rgReport.MasterTableView.Columns.FindByUniqueName("RamSize").Visible = true;
        rgReport.MasterTableView.Columns.FindByUniqueName("CpuType").Visible = true;
        rgReport.MasterTableView.Columns.FindByUniqueName("SystemPurchaseDate").Visible = true;
        rgReport.MasterTableView.Columns.FindByUniqueName("SystemWarrantyExpireDate").Visible = true;
        rgReport.MasterTableView.Columns.FindByUniqueName("MacAddress").Visible = true;
        rgReport.MasterTableView.Columns.FindByUniqueName("EndpointStatus").Visible = true;
        rgReport.MasterTableView.Columns.FindByUniqueName("BitlockerRecoveryKey").Visible = true;
        rgReport.MasterTableView.Columns.FindByUniqueName("TpmVersion").Visible = true;
        rgReport.MasterTableView.Columns.FindByUniqueName("TpmEnabled").Visible = true;
        rgReport.MasterTableView.Columns.FindByUniqueName("TpmOwnership").Visible = true;
    }

    #endregion

    #region protected events

    #region telerik grid

    /// <summary>
    /// grid on need data source
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rgReport_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        int _currentPage = ResetSearch ? 1 : rgReport.CurrentPageIndex + 1;
        int _resultCount = 0;
        int _takeRows = rgReport.PageSize;
        int _skipRows = _takeRows * (_currentPage - 1);

        bool? _managedByJamf = lbManagedBy.Items.FindByValue("1").Selected ? true : (bool?)null;
        bool? _managedByKaseya = lbManagedBy.Items.FindByValue("2").Selected ? true : (bool?)null;
        bool? _managedBySentinelOne = lbManagedBy.Items.FindByValue("3").Selected ? true : (bool?)null;
        bool? _managedBySplashtop = lbManagedBy.Items.FindByValue("4").Selected ? true : (bool?)null;
        bool? _isServer = ddlServerWorkstation.SelectedIndex == 0 ? (bool?)null : ddlServerWorkstation.SelectedIndex == 1;
        bool? _active = ddlActive.SelectedIndex == 0 ? (bool?)null : ddlActive.SelectedIndex == 1;

        var _dtReport = DesktopShared.StoredProcedureCallerClasses.RetrievalProcedures.ClientServerSearch(ddlClient.ClientId, _isServer, _managedByKaseya, _managedBySentinelOne, _managedBySplashtop, _managedByJamf,null, txtComputerName.Text.Trim(), 
            ucLastBackupStartDate.SelectedDate, ucLastBackupEndDate.SelectedDate, ucLastPatchStartDate.SelectedDate, ucLastPatchEndDate.SelectedDate, ucLastActiveStartDate.SelectedDate, ucLastActiveEndDate.SelectedDate, _active,
            _skipRows, _takeRows, SortColumnName, SortOperator == SortOperator.Ascending ? "asc" : "desc");

        if (_dtReport.Rows.Count > 0)
            _resultCount = Convert.ToInt32(_dtReport.Rows[0]["TotalRows"].ToString());
        
        rgReport.VirtualItemCount = _resultCount;
        rgReport.DataSource = _dtReport;
        ResetSearch = false;
    }

    /// <summary>
    /// grid on item data bound
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rgReport_ItemDataBound(object sender, GridItemEventArgs e)
    {
        #region Item or AlternatingItem

        if (e.Item.ItemType == GridItemType.Item || e.Item.ItemType == GridItemType.AlternatingItem)
        {
            GridDataItem _gdi = e.Item as GridDataItem;
            DataRowView _drv = (DataRowView)e.Item.DataItem;

            LinkButton btnActiveInactive = e.Item.FindControl("btnActiveInactive") as LinkButton;
            if (_drv["Active"].ToString().ToUpper().Trim() == "Y")
            {
                btnActiveInactive.CommandArgument = "0";
                btnActiveInactive.Text = "<i class=\"gi gi-pause\" title=\"Make Inactive\"></i>";
                btnActiveInactive.Attributes.Add("onclick", "return confirm('Make server inactive?');");
            }
            else
            {
                btnActiveInactive.CommandArgument = "1";
                btnActiveInactive.Text = "<i class=\"gi gi-play\" title=\"Make Active\"></i>";
                btnActiveInactive.Attributes.Add("onclick", "return confirm('Make server active?');");
            }

            TelerikHelper.AddLabelToCell(_drv["Name"].ToString(), _gdi["Name"]);
            TelerikHelper.AddLabelToCell(_drv["Company"].ToString(), _gdi["Company"]);
            TelerikHelper.AddLabelToCell(_drv["Os"].ToString(), _gdi["Os"]);
            TelerikHelper.AddLabelToCell(_drv["Endpoint"].ToString(), _gdi["Endpoint"]);
            
            //last backup date
            string _displayValue = "";
            DateTime _date = DateTime.MinValue;
            if (DateTime.TryParse(_drv["LastBackupDate"].ToString(), out _date))
                _displayValue = _date.ToString("MM/dd/yy");
            TelerikHelper.AddLabelToCell(_displayValue, _gdi["LastBackupDate"]);

            //last patch date
            _displayValue = "";
            if (DateTime.TryParse(_drv["LastPatchDate"].ToString(), out _date))
                _displayValue = _date.ToString("MM/dd/yy");
            TelerikHelper.AddLabelToCell(_displayValue, _gdi["LastPatchDate"]);

            //last Reboot date
            _displayValue = "";
            if (DateTime.TryParse(_drv["KaseyaLastReboot"].ToString(), out _date))
                _displayValue = _date.ToString("MM/dd/yy");
            TelerikHelper.AddLabelToCell(_displayValue, _gdi["KaseyaLastReboot"]);

            //last active date
            _displayValue = "";
            if (DateTime.TryParse(_drv["LastActiveDate"].ToString(), out _date))
                _displayValue = _date.ToString("MM/dd/yy");
            TelerikHelper.AddLabelToCell(_displayValue, _gdi["LastActiveDate"]);

            TelerikHelper.AddLabelToCell(_drv["LastLoggedInUser"].ToString(), _gdi["LastLoggedInUser"]);
            TelerikHelper.AddLabelToCell(_drv["ManagedBy"].ToString(), _gdi["ManagedBy"]);
            TelerikHelper.AddLabelToCell(_drv["Type"].ToString(), _gdi["Type"]);
            TelerikHelper.AddLabelToCell(_drv["Active"].ToString(), _gdi["Active"]);

            #region additional hidden fields only used for client report export

            //need to strip out bad data found in kaseay
            //tpmtool : The term 'tpmtool' is not recognized as the name of a cmdlet, function, script file, or operable program. 

            //"Method invocation failed because [System.Char] does not contain a method named 'Substring'.
            //At line:1 char:1
            //+ (tpmtool getdeviceinformation - split '`n')[2].Substring(14)
            //+ ~~~~~~~~~~~~~~~~~~~~~~~~~~~"

            //"At line:1 char:142
            //+... sion from win32_tpm | foreach {$_.SpecVersion.ToString() - like * 2.0 *}
            //+~
            //You must provide a value expression"

            //"You cannot call a method on a null-valued expression.
            //At line:1 char:81
            //+ ... ectors C: -get | findstr / R / C:""......-......-""; $RecoveryKey.Trim();
            //+"

            var exclusionsWords = new[] { "tpmtool", "win32_tpm", "get-tpm", "null-valued" };
            var tpmVersion = _drv["TpmVersion"].ToString().Trim();
            if (exclusionsWords.Any(x => tpmVersion.Contains(x)))
                tpmVersion = "";
            TelerikHelper.AddLabelToCell(tpmVersion, _gdi["TpmVersion"]);

            var bitlockerRecoveryKey = _drv["BitlockerRecoveryKey"].ToString().Trim();
            if (exclusionsWords.Any(x => bitlockerRecoveryKey.Contains(x)))
                bitlockerRecoveryKey = "";
            TelerikHelper.AddLabelToCell(bitlockerRecoveryKey, _gdi["BitlockerRecoveryKey"]);
            #endregion
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

            rgReport.MasterTableView.Columns[++_colIndex].HeaderStyle.Width = Unit.Pixel(50);
            rgReport.MasterTableView.Columns[++_colIndex].HeaderStyle.Width = Unit.Pixel(225);
            rgReport.MasterTableView.Columns[++_colIndex].HeaderStyle.Width = Unit.Pixel(225);
            rgReport.MasterTableView.Columns[++_colIndex].HeaderStyle.Width = Unit.Pixel(175);
            rgReport.MasterTableView.Columns[++_colIndex].HeaderStyle.Width = Unit.Pixel(125);
            rgReport.MasterTableView.Columns[++_colIndex].HeaderStyle.Width = Unit.Pixel(125);
            rgReport.MasterTableView.Columns[++_colIndex].HeaderStyle.Width = Unit.Pixel(125);
            rgReport.MasterTableView.Columns[++_colIndex].HeaderStyle.Width = Unit.Pixel(125);
            rgReport.MasterTableView.Columns[++_colIndex].HeaderStyle.Width = Unit.Pixel(150);
            rgReport.MasterTableView.Columns[++_colIndex].HeaderStyle.Width = Unit.Pixel(250);
            rgReport.MasterTableView.Columns[++_colIndex].HeaderStyle.Width = Unit.Pixel(200);
            rgReport.MasterTableView.Columns[++_colIndex].HeaderStyle.Width = Unit.Pixel(125);
        }
    }

    /// <summary>
    /// grid on ite command
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rgReport_ItemCommand(object sender, GridCommandEventArgs e)
    {
        
        if (e.CommandName == "MakeActiveInactive")
        {
            int _id = int.Parse((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["Id"].ToString());
            var _active = e.CommandArgument.ToString() == "1";
            var objClientServer = new DesktopShared.EntityClasses.ClientServerEntity(_id);
            objClientServer.Active = _active;
            objClientServer.Save();
            RebindGrid();
            DesktopShared.Bootstrap.Alert.DisplayMessage(litMessage, String.Format("Server has been made {0} - {1}.", _active ? "active" : "inactive", DateTime.Now), DesktopShared.Bootstrap.Alert.AlertType.Success);
        }
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
        rgReport.MasterTableView.ExportToCSV(); 
    }

    /// <summary>
    /// export button click
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnExport2_Click(object sender, System.EventArgs e)
    {
        ConfigureClientReportExport();
        rgReport.MasterTableView.ExportToCSV();
    }

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
    /// validate last backup dates
    /// </summary>
    /// <param name="source"></param>
    /// <param name="args"></param>
    protected void cvLastBackupDates_ServerValidate(object source, ServerValidateEventArgs args)
    {
        args.IsValid = true;
        if (!ucLastBackupStartDate.SelectedDate.HasValue || !ucLastBackupEndDate.SelectedDate.HasValue)
            return;
        args.IsValid = ucLastBackupEndDate.SelectedDate.Value.Date >= ucLastBackupStartDate.SelectedDate.Value.Date;
    }

    /// <summary>
    /// validate last patch dates
    /// </summary>
    /// <param name="source"></param>
    /// <param name="args"></param>
    protected void cvLastPatchDates_ServerValidate(object source, ServerValidateEventArgs args)
    {
        args.IsValid = true;
        if (!ucLastPatchStartDate.SelectedDate.HasValue || !ucLastPatchEndDate.SelectedDate.HasValue)
            return;
        args.IsValid = ucLastPatchEndDate.SelectedDate.Value.Date >= ucLastPatchStartDate.SelectedDate.Value.Date;
    }

    /// <summary>
    /// validate last active dates
    /// </summary>
    /// <param name="source"></param>
    /// <param name="args"></param>
    protected void cvLastActiveDates_ServerValidate(object source, ServerValidateEventArgs args)
    {
        args.IsValid = true;
        if (!ucLastActiveStartDate.SelectedDate.HasValue || !ucLastActiveEndDate.SelectedDate.HasValue)
            return;
        args.IsValid = ucLastActiveEndDate.SelectedDate.Value.Date >= ucLastActiveStartDate.SelectedDate.Value.Date;
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
            return (obj == null) ? "Name" : (string)obj;
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

