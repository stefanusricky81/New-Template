using SD.LLBLGen.Pro.ORMSupportClasses;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class Reports_Backup : BasePage
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
        txtProtectedMachine.Focus();

        short _tabIndex = 0;
        ddlClient.TabIndex = ++_tabIndex;
        txtProtectedMachine.TabIndex = ++_tabIndex;
        txtBackupServer.TabIndex = ++_tabIndex;
        txtBackupSet.TabIndex = ++_tabIndex;
        ddlStatus.TabIndex = ++_tabIndex;
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
        }

       
        ddlClient.DisplayChosenScript = _displayChosenScript;
        ddlClient.CssClass = _cssClass;
        ddlStatus.CssClass = _cssClass;
        if (_displayChosenScript)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append("<script type='text/javascript'>");
            sb.Append("$(document).ready(function() {");
            sb.Append(String.Format("SetChosenUserStatus_{0}();", this.ClientID));
            sb.Append(String.Format("Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(SetChosenUserStatus_{0})", this.ClientID));
            sb.Append("});");
            sb.Append(String.Format("function SetChosenUserStatus_{0}(sender, args) {{", this.ClientID));
            sb.Append(String.Format("$('#{0}').chosen({{", ddlStatus.ClientID));
            sb.Append("allow_single_deselect: true,");
            sb.Append("placeholder_text_single: \"Select a Status ...\",");
            sb.Append("width: \"100%\"");
            sb.Append("});");
            sb.Append("}");
            sb.Append("</script>");
            litStatusJs.Text = sb.ToString();
        }
    }

    /// <summary>
    /// rebind document grid
    /// </summary>
    private void RebindGrid()
    {
        litMessage.Text = "";
        phSearchResults.Visible = true;
        rgBackup.Visible = true;
        rgBackup.EditIndexes.Clear();
        rgBackup.DataSource = null;
        rgBackup.Rebind();
    }

    #endregion

    #region protected events

    #region telerik grid

    /// <summary>
    /// grid on need data source
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rgBackup_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        bool? _active = null;
        if (!String.IsNullOrWhiteSpace(ddlStatus.SelectedValue))
            _active = ddlStatus.SelectedValue == "1";
        rgBackup.DataSource = DesktopShared.StoredProcedureCallerClasses.RetrievalProcedures.ProcSearchBackupSets(ddlClient.ClientId, txtProtectedMachine.Text.Trim(), txtBackupServer.Text.Trim(), txtBackupSet.Text.Trim(), _active);
    }

    /// <summary>
    /// grid on item data bound
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rgBackup_ItemDataBound(object sender, GridItemEventArgs e)
    {
        #region Item or AlternatingItem

        if (e.Item.ItemType == GridItemType.Item || e.Item.ItemType == GridItemType.AlternatingItem)
        {
            GridDataItem _gdi = e.Item as GridDataItem;
            DataRowView _drv = (DataRowView)e.Item.DataItem;

            DateTime _tempDate = DateTime.Now;

            string _lastBackup = "";
            if (DateTime.TryParse(_drv["LastBackupDate"].ToString(), out _tempDate))
                _lastBackup = _tempDate.ToString("MM/dd/yy");

            string _lastSuccess = "";
            if (DateTime.TryParse(_drv["LastSuccessfulBackupDate"].ToString(), out _tempDate))
                _lastSuccess = _tempDate.ToString("MM/dd/yy");

            string _active = "";
            bool _tempBool = false;
            if (bool.TryParse(_drv["Active"].ToString(), out _tempBool))
                _active = _tempBool ? "Y" : "N";

            TelerikHelper.AddLabelToCell(_drv["ClientName"].ToString(), _gdi["ClientName"]);
            TelerikHelper.AddLabelToCell(_drv["ProtectedMachine"].ToString(), _gdi["ProtectedMachine"]);
            TelerikHelper.AddLabelToCell(_drv["BackupServer"].ToString(), _gdi["BackupServer"]);
            TelerikHelper.AddLabelToCell(_drv["BackupSetName"].ToString(), _gdi["BackupSetName"]);
            TelerikHelper.AddLabelToCell(_lastBackup, _gdi["LastBackupDate"]);
            TelerikHelper.AddLabelToCell(_lastSuccess, _gdi["LastSuccessfulBackupDate"]);
            TelerikHelper.AddLabelToCell(_active, _gdi["Active"]);

            int _backupMontorStatusId = BitByBit.Utility.DataBase.ConvertHelper.ConvertToInteger(_drv["LastSuccessfulBackupStatusId"], -1);
            if (_backupMontorStatusId == DesktopShared.Backup.Monitor.Status.ID.Warning)
            {
                _gdi["LastSuccessfulBackupDate"].Attributes.CssStyle.Add("background-color", DesktopShared.Bootstrap.BackgroundColor(DesktopShared.Bootstrap.Alert.AlertType.Warning)); 
                _gdi["LastSuccessfulBackupDate"].Attributes.CssStyle.Add("color", DesktopShared.Bootstrap.Color(DesktopShared.Bootstrap.Alert.AlertType.Warning));
            }

        }

        #endregion
    }

    /// <summary>
    /// grid on pre render
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rgBackup_PreRender(object sender, EventArgs e)
    {
        if (DesktopShared.Utility.BrowserHelper.IsTabletOrSmartPhone(this.Request))
        {
            int _colIndex = -1;
            rgBackup.MasterTableView.Columns[++_colIndex].HeaderStyle.Width = Unit.Pixel(225);
            rgBackup.MasterTableView.Columns[++_colIndex].HeaderStyle.Width = Unit.Pixel(225);
            rgBackup.MasterTableView.Columns[++_colIndex].HeaderStyle.Width = Unit.Pixel(225);
            rgBackup.MasterTableView.Columns[++_colIndex].HeaderStyle.Width = Unit.Pixel(225);
            rgBackup.MasterTableView.Columns[++_colIndex].HeaderStyle.Width = Unit.Pixel(120);
            rgBackup.MasterTableView.Columns[++_colIndex].HeaderStyle.Width = Unit.Pixel(120);
            rgBackup.MasterTableView.Columns[++_colIndex].HeaderStyle.Width = Unit.Pixel(120);
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

    #endregion
}