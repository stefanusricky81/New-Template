using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Telerik.Web.UI;
using BitByBit;
using SD.LLBLGen.Pro.ORMSupportClasses;
using DesktopShared;

public partial class Reports_NewClientBackup : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!this.IsPostBack)
        {
            GetClientId(); //get client from query string if available
            SetUpPage(); //set client combo box & client name if client id passed in.  set width for user controls
        }
    }
    #region private methods

    /// <summary>
    /// set up page -> if client id available, use.  set width of user controls
    /// </summary>
    private void SetUpPage()
    {
        #region client id

        if (ClientId > 0)
        {
            if (ClientId.HasValue)
            {
                ddlSmartClient.SelectedClientId = ClientId.Value;
                litClientName.Text = " - " + DesktopShared.Client.GetName(ClientId.Value).Trim();
                ucClientBackupReportGrid.SearchClientId = ClientId.Value;
            }
        }

        #endregion

        #region get search values from cookie

        // current date by default
        DateTime _startDate = DateTime.Now;

        if (!String.IsNullOrEmpty(BitByBit.Web.Request.GetString("Date")))
        {
            try { _startDate = Convert.ToDateTime(BitByBit.Web.Request.GetString("Date").Trim()); }
            catch { _startDate = DateTime.Now; }
        }

        DateTime _endDate = _startDate.AddDays(1); ;

        //client id
        if (!ClientId.HasValue && DesktopShared.User.BackupMonitor.Search.ClientId.HasValue)
            ddlSmartClient.SelectedClientId = DesktopShared.User.BackupMonitor.Search.ClientId.Value;

        //start date
        if (DesktopShared.User.BackupMonitor.Search.StartDate.HasValue)
            _startDate = DesktopShared.User.BackupMonitor.Search.StartDate.Value;

        //end date
        if (DesktopShared.User.BackupMonitor.Search.EndDate.HasValue)
            _endDate = DesktopShared.User.BackupMonitor.Search.EndDate.Value;

        //status id
        if (DesktopShared.User.BackupMonitor.Search.StatusId.HasValue)
            ucBackupMonitorStatusDDL.StatusId = DesktopShared.User.BackupMonitor.Search.StatusId.Value;

        //core
        if (DesktopShared.User.BackupMonitor.Search.Core.Trim().Length > 0)
            txtBackupServer.Text = DesktopShared.User.BackupMonitor.Search.Core.Trim();

        //server
        if (DesktopShared.User.BackupMonitor.Search.Server.Trim().Length > 0)
            txtServer.Text = DesktopShared.User.BackupMonitor.Search.Server.Trim();

        //backup type
        if (DesktopShared.User.BackupMonitor.Search.BackupTypeId.HasValue)
            ucBackupTypeDDL.StatusId = DesktopShared.User.BackupMonitor.Search.BackupTypeId.Value;

        //is unsuccessful
        if (DesktopShared.User.BackupMonitor.Search.IsUnsuccessful.HasValue)
            chkIsUnsuccessful.Checked = true;

        //veeam backup type 
        if (DesktopShared.User.BackupMonitor.Search.VeeamBackupTypeId.HasValue)
            ddlVeeamBackupType.VeeamBackupTypeId = DesktopShared.User.BackupMonitor.Search.VeeamBackupTypeId;

        #endregion

        //dates
        rdpStartDate.SelectedDate = _startDate;
        rdpEndDate.SelectedDate = _endDate;

        if (BitByBit.Web.Request.GetString("cl") == "y")
        {
            ucClientBackupReportGrid.ClearGrid();
            return;
        }
        BindGrid(ClientId.HasValue ? ClientId.Value : DesktopShared.User.BackupMonitor.Search.ClientId);
    }

    /// <summary>
    /// bind results grid
    /// </summary>
    private void BindGrid(int? clientId)
    {
        ucClientBackupReportGrid.SearchClientId = clientId.HasValue ? clientId.Value : (int?)null;
        ucClientBackupReportGrid.SearchStartDate = (DateTime)rdpStartDate.SelectedDate;
        ucClientBackupReportGrid.SearchEndDate = (DateTime)rdpEndDate.SelectedDate;
        ucClientBackupReportGrid.SearchBackupMonitorStatusId = ucBackupMonitorStatusDDL.StatusId;
        ucClientBackupReportGrid.SearchCore = txtBackupServer.Text.Trim();
        //ucClientBackupReportGrid.SearchServer = txtServer.Text.Trim();
        ucClientBackupReportGrid.SearchNumberOfDaysUnsuccessful = null;
        ucClientBackupReportGrid.SearchHasTicket = null;
        ucClientBackupReportGrid.SearchHasRollupData = null;
        ucClientBackupReportGrid.SearchHasReplicationData = null;
        ucClientBackupReportGrid.SearchIsUnsuccessful = chkIsUnsuccessful.Checked ? true : (bool?)null;
        ucClientBackupReportGrid.SearchBackupTypeId = ucBackupTypeDDL.StatusId > 0 ? ucBackupTypeDDL.StatusId : (int?)null;
        ucClientBackupReportGrid.SearchServerName = txtServer.Text.Trim();
        ucClientBackupReportGrid.SearchVeeamBackupTypeId = ddlVeeamBackupType.VeeamBackupTypeId;

        //reset grid for new search parameters
        ucClientBackupReportGrid.ResetGrid();
    }

    /// <summary>
    /// get client id from query string
    /// </summary>
    private void GetClientId()
    {
        if (!String.IsNullOrEmpty(BitByBit.Web.Request.GetString("ClientId")))
        {
            try { ClientId = Convert.ToInt32(BitByBit.Web.Request.GetString("ClientId")); }
            catch { ClientId = 0; }

            ucClientBackupReportGrid.SearchClientId = ClientId;
        }
    }

    #endregion

    #region protected methods
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            //set values stored in cookie
            DesktopShared.User.BackupMonitor.Search.ClientId = ddlSmartClient.SelectedClientId > 0 ? ddlSmartClient.SelectedClientId : (int?)null;
            DesktopShared.User.BackupMonitor.Search.StatusId = ucBackupMonitorStatusDDL.StatusId > 0 ? ucBackupMonitorStatusDDL.StatusId : (int?)null;
            DesktopShared.User.BackupMonitor.Search.StartDate = (DateTime)rdpStartDate.SelectedDate;
            DesktopShared.User.BackupMonitor.Search.EndDate = (DateTime)rdpEndDate.SelectedDate;
            DesktopShared.User.BackupMonitor.Search.Core = txtBackupServer.Text.Trim();
            DesktopShared.User.BackupMonitor.Search.Server = txtServer.Text.Trim();
            DesktopShared.User.BackupMonitor.Search.IsUnsuccessful = chkIsUnsuccessful.Checked ? true : (bool?)null;
            DesktopShared.User.BackupMonitor.Search.BackupTypeId = ucBackupTypeDDL.StatusId > 0 ? ucBackupTypeDDL.StatusId : (int?)null;
            DesktopShared.User.BackupMonitor.Search.VeeamBackupTypeId = ddlVeeamBackupType.VeeamBackupTypeId;

            BindGrid(ddlSmartClient.SelectedClientId > 0 ? ddlSmartClient.SelectedClientId : (int?)null);

            //client selected -> add name to header
            if (ddlSmartClient.SelectedClientId > 0)
                litClientName.Text = " - " + ddlSmartClient.SelectedClientName;
        }
    }

    protected void btnViewToday_Click(object sender, EventArgs e)
    {
        //dates
        rdpStartDate.SelectedDate = DateTime.Now;
        rdpEndDate.SelectedDate = DateTime.Now.AddDays(1);

        //bind grid
        BindGrid(ddlSmartClient.SelectedClientId > 0 ? ddlSmartClient.SelectedClientId : (int?)null);
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        //client
        ddlSmartClient.ClearItems();
        ucClientBackupReportGrid.SearchClientId = -1;
        litClientName.Text = "";

        //search controls
        ucBackupMonitorStatusDDL.StatusId = -1;
        rdpStartDate.Clear();
        rdpEndDate.Clear();
        txtBackupServer.Text = "";
        txtServer.Text = "";
        ucBackupTypeDDL.StatusId = -1;
        chkIsUnsuccessful.Checked = false;
        ddlVeeamBackupType.VeeamBackupTypeId = null;

        //clear values stored in cookie
        DesktopShared.User.BackupMonitor.Search.ClientId = null;
        DesktopShared.User.BackupMonitor.Search.StatusId = null;
        DesktopShared.User.BackupMonitor.Search.StartDate = null;
        DesktopShared.User.BackupMonitor.Search.EndDate = null;
        DesktopShared.User.BackupMonitor.Search.Core = "";
        DesktopShared.User.BackupMonitor.Search.DaysUnsuccessful = null;
        DesktopShared.User.BackupMonitor.Search.Server = "";
        DesktopShared.User.BackupMonitor.Search.HasTicket = null;
        DesktopShared.User.BackupMonitor.Search.HasRollupData = null;
        DesktopShared.User.BackupMonitor.Search.HasReplicationData = null;
        DesktopShared.User.BackupMonitor.Search.IsUnsuccessful = null;
        DesktopShared.User.BackupMonitor.Search.BackupTypeId = null;
        DesktopShared.User.BackupMonitor.Search.VeeamBackupTypeId = null;

        //ucClientBackupReportGrid.ClearGrid();
        Response.Redirect("ClientBackup.aspx?&cl=y");
    }
    #endregion

    #region public properties

    /// <summary>
    /// get/set client id (stored in viewstate)
    /// </summary>
    public int? ClientId
    {
        get
        {
            object obj = this.ViewState["ClientIdForBackupReport"];
            if (obj == null)
                return null;
            else
                return (int)obj;
        }
        set { this.ViewState["ClientIdForBackupReport"] = value; }
    }

    #endregion
}