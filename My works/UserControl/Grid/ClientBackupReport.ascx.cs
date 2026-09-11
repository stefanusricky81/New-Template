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
using SD.LLBLGen.Pro.ORMSupportClasses;
using Telerik.Web.UI;

public partial class UserControl_Grid_ClientBackupReport : System.Web.UI.UserControl
{
    bool _isExport = false;

    protected void Page_Load(object sender, EventArgs e)
    {
        //records per page event handler
        DropDownList ddlRecordsPerPage = ucRecordsPerPage.GetDropDownList();
        ddlRecordsPerPage.SelectedIndexChanged += new EventHandler(ddlRecordsPerPage_SelectedChange);

        if (!this.IsPostBack)
            SetUpRecordsPerPage(); //set value for drop down list control for number of records to display
    }

    #region protected methods / events

    #region telerik grid events

    /// <summary>
    /// grid on need data source
    /// </summary>
    /// <param name="source"></param>
    /// <param name="e"></param>
    protected void rgClientBackupReport_NeedDataSource(object source, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        //get typed list
        DesktopShared.TypedListClasses.BackupMonitor_TypedList monitors = DesktopShared.Backup.Monitor.GetTypedList(SearchClientId, SearchStartDate, SearchEndDate, SearchBackupMonitorStatusId, SearchCore, SearchServer, SearchNumberOfDaysUnsuccessful, 
            SearchHasTicket, SearchHasRollupData, SearchHasReplicationData, SearchIsUnsuccessful, SearchBackupTypeId, SearchServerName, SearchVeeamBackupTypeId);
        //todo: datto backup date = "Date", not appasure
        //bind
        rgClientBackupReport.DataSource = monitors;
        monitors.DefaultView.Sort = "ClientCompany, Date";
        
        #region paging

        if (TicketsPerPage > 0)
            rgClientBackupReport.PageSize = TicketsPerPage;
        else
        {
            if (monitors.Rows.Count > 0)
                rgClientBackupReport.PageSize = monitors.Rows.Count;
        }

        #endregion
    }

    /// <summary>
    /// grid on item created
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rgClientBackupReport_ItemCreated(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        #region Item or AlternatingItem

        if (e.Item.ItemType == GridItemType.Item || e.Item.ItemType == GridItemType.AlternatingItem)
        {
            GridDataItem item = e.Item as GridDataItem;

            #region status

            string _statusName = (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["BackupMonitorStatusName"].ToString().ToLower();
            string _gridStatusName = (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["StatusGridName"].ToString();
            bool _isFailure = BitByBit.Utility.DataBase.ConvertHelper.ConvertToBoolean((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["IsFailure"], false);

            TableCell tcStatusName = item["BackupMonitorStatusName"];
            Image imgStatus = new Image();
            imgStatus.ID = "imgStatus";
            imgStatus.ImageAlign = ImageAlign.AbsBottom;

            if (_statusName == "successful")
            {
                tcStatusName.ForeColor = System.Drawing.ColorTranslator.FromHtml("#24BF49"); // green
                imgStatus.ImageUrl = DesktopShared.SiteHelper.Icon.Success;
                imgStatus.AlternateText = "Successful";
                imgStatus.Attributes.Add("title", "Successful");

                tcStatusName.Controls.Clear();
                tcStatusName.Controls.Add(imgStatus);
            }
            else if (_isFailure)
            {
                tcStatusName.ForeColor = System.Drawing.ColorTranslator.FromHtml("#DB2929"); // red
                imgStatus.ImageUrl = DesktopShared.SiteHelper.Icon.Error;
                imgStatus.AlternateText = "Unsuccessful";
                imgStatus.Attributes.Add("title", "Unsuccessful");

                tcStatusName.Controls.Add(imgStatus);
                TelerikHelper.AddLabelToCell("&nbsp;" + _gridStatusName, _statusName, tcStatusName, false);
            }
            else if (_statusName == "warning")
            {
                tcStatusName.ForeColor = System.Drawing.ColorTranslator.FromHtml("#FFFF00"); // yellow
                imgStatus.ImageUrl = DesktopShared.SiteHelper.Icon.Warning;
                imgStatus.AlternateText = "Warning";
                imgStatus.Attributes.Add("title", "Warning");

                tcStatusName.Controls.Clear();
                tcStatusName.Controls.Add(imgStatus);
            }

            #endregion

        }

        #endregion
    }

    /// <summary>
    /// grid on item databound
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rgClientBackupReport_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {

        #region Item or AlternatingItem

        if (e.Item.ItemType == GridItemType.Item || e.Item.ItemType == GridItemType.AlternatingItem)
        {
            GridEditableItem item = e.Item as GridEditableItem;
            DesktopShared.TypedListClasses.BackupMonitor_Row _backup =
                (DesktopShared.TypedListClasses.BackupMonitor_Row)
                ((DataRowView)e.Item.DataItem).Row;

            //monitor date
            TelerikHelper.AddLabelToCell(_backup.Date.ToString("MM/dd/yy").Trim(), item["Date"]);

            //client name
            TelerikHelper.AddLabelToCell(_backup.ClientCompany.Trim(), item["ClientCompany"]);

            //server
            TelerikHelper.AddLabelToCell(_backup.Server.Trim(), item["Server"]);

            //core
            TelerikHelper.AddLabelToCell(_backup.BackupSetBackupServer.Trim(), item["BackupServer"]);

            //bu set name
            TelerikHelper.AddLabelToCell(_backup.BackupSetName.Trim(), item["BackupSetName"]);

            //veeam job type
            TelerikHelper.AddLabelToCell(_backup.VeeamBackupTypeName.Trim(), item["VeeamBackupTypeName"]);

            //status
            TableCell tcStatusName = item["BackupMonitorStatusName"];
            Image imgStatus = new Image();
            imgStatus.ID = "imgStatus";
            imgStatus.ImageAlign = ImageAlign.AbsBottom;
            if (tcStatusName.Text.ToLower() == "successful")
            {
                tcStatusName.ForeColor = System.Drawing.ColorTranslator.FromHtml("#24BF49"); // green
                imgStatus.ImageUrl = DesktopShared.SiteHelper.Icon.Success;
                imgStatus.AlternateText = "Successful";
                imgStatus.Attributes.Add("title", "Successful");

                tcStatusName.Controls.Clear();
                tcStatusName.Controls.Add(imgStatus);
            }
            else if (_backup.IsFailure)
            {
                tcStatusName.ForeColor = System.Drawing.ColorTranslator.FromHtml("#DB2929"); // red
                imgStatus.ImageUrl = DesktopShared.SiteHelper.Icon.Error;
                imgStatus.AlternateText = "Unsuccessful";
                imgStatus.Attributes.Add("title", "Unsuccessful");

                tcStatusName.Controls.Add(imgStatus);
                TelerikHelper.AddLabelToCell("&nbsp;" + _backup.StatusGridName.Trim(), _backup.BackupMonitorStatusName.Trim(), tcStatusName, false);
            }
            else if (tcStatusName.Text.ToLower() == "warning")
            {
                tcStatusName.ForeColor = System.Drawing.ColorTranslator.FromHtml("#FFA500"); // orange
                imgStatus.ImageUrl = DesktopShared.SiteHelper.Icon.Warning;
                imgStatus.AlternateText = "Warning";
                imgStatus.Attributes.Add("title", "Warning");

                tcStatusName.Controls.Clear();
                tcStatusName.Controls.Add(imgStatus);
            }

            //backed up date
            TelerikHelper.AddLabelToCell(_backup.IsAppassureReportStartDateTimeNull() ? "" :_backup.AppassureReportStartDateTime.ToString("MM/dd HH:mm").Trim(), _backup.AppassureReportStartDateTime.ToString(), item["AppassureReportStartDateTime"]);

            //updated by
            TelerikHelper.AddLabelToCell(String.Format("{0} {1}", _backup.First.Trim(), _backup.Last.Trim()), item["FullName"]);

            #region removed columns

            /*
            
            #region ID -> ticket id - status - assigned to

            TableCell tcId = item["Id"];
            tcId.Text = "";
            tcId.Controls.Clear();

            if ((!_backup.IsTicketIdNull()) && (_backup.TicketId > 0))
            {
                HyperLink hlTicketView = new HyperLink();

                string _ticketStatus = _backup.TicketStatus.Trim();
                hlTicketView.Text = String.Format("{0} - {1} - {2}", _backup.TicketId.ToString(), _ticketStatus, _backup.AssignedToFirst.Trim());
                hlTicketView.ToolTip = String.Format("Click here to view Ticket # {0} for Backup Monitor Id {1}.  Ticket Status = {2}.  Assigned to {3} {4}.", _backup.TicketId, _backup.Id, _ticketStatus, _backup.AssignedToFirst.Trim(), _backup.AssignedToLast.Trim());
                hlTicketView.NavigateUrl = String.Format("/Ticket/Detail2.aspx?Id={0}", _backup.TicketId);

                if (_backup.CscDefectsFkStatus == DesktopShared.SiteHelper.Ticket.Status.Id.Closed)
                    hlTicketView.Attributes.CssStyle.Add("color", "#999980");

                tcId.Controls.Add(hlTicketView);
            }

            #endregion

            //space used
            TelerikHelper.AddLabelToCell(_backup.SpaceUsed.ToString().Trim(), String.Format("{0} GB - Space Used", _backup.SpaceUsed), item["SpaceUsed"]);

            //space free
            TelerikHelper.AddLabelToCell(_backup.SpaceFree.ToString().Trim(), String.Format("{0} GB - Space Free", _backup.SpaceFree), item["SpaceFree"]);

            //amount data changed
            TelerikHelper.AddLabelToCell(_backup.AmtDataChanged.ToString().Trim(), String.Format("{0} GB - Amount Data Changed", _backup.AmtDataChanged), item["AmtDataChanged"]);
            if (_backup.IsAmtDataChangedNull() || (_backup.AmtDataChanged == 0))
                item["AmtDataChanged"].ForeColor = System.Drawing.ColorTranslator.FromHtml("#DB2929"); // red
            
            //back up size
            TelerikHelper.AddLabelToCell(_backup.BackupSize.ToString().Trim(), String.Format("{0} GB - Backup Size", _backup.BackupSize), item["BackupSize"]);

            //snapshots passed
            TelerikHelper.AddLabelToCell(_backup.Sspass.ToString().Trim(), String.Format("{0} Snapshots Passed", _backup.Sspass), item["SSPass"]);

            //snap shots failed
            TelerikHelper.AddLabelToCell(_backup.Ssfail.ToString().Trim(), String.Format("{0} Snapshots Failed", _backup.Ssfail), item["SSFail"]);

            //days successfull 
            TelerikHelper.AddLabelToCell(_backup.DaysSuccessful.ToString().Trim(), String.Format("{0} Days Successful", _backup.DaysSuccessful), item["DaysSuccessful"]);

            //days unsuccessfull 
            TelerikHelper.AddLabelToCell(_backup.DaysUnsuccessful.ToString().Trim(), String.Format("{0} Days Unsuccessful", _backup.DaysUnsuccessful), item["DaysUnsuccessful"]);

            #region rollup data

            if (_backup.RollupFileProcessed)
            {
                //rollups
                TelerikHelper.AddLabelToCell(_backup.Rollup.ToString().Trim(), String.Format("{0} Rollups", _backup.Rollup), item["Rollup"]);
                if (_backup.IsRollupNull() || (_backup.Rollup == 0))
                    item["Rollup"].ForeColor = System.Drawing.ColorTranslator.FromHtml("#DB2929"); // red

                //duration
                TelerikHelper.AddLabelToCell(_backup.Duration.ToString().Trim(), String.Format("{0} - Duration", _backup.Duration), item["Duration"]);

                //throughput
                TelerikHelper.AddLabelToCell(_backup.Throughput.ToString().Trim(), String.Format("{0} Throughput", _backup.Throughput), item["Throughput"]);

                //number of recovery points consolidated
                TelerikHelper.AddLabelToCell(_backup.RecoveryPointsConsolidated.ToString().Trim(), String.Format("{0} Recovery Points Consolidated", _backup.RecoveryPointsConsolidated), item["RecoveryPointsConsolidated"]);
            }
            else
            {
                item["Rollup"].Text = "";
                item["Duration"].Text = "";
                item["Throughput"].Text = "";
                item["RecoveryPointsConsolidated"].Text = "";
            }

            #endregion

            #region replication data

            if (_backup.ReplicationFileProcessed)
            {
                bool _displayError = ((_backup.InQueue > DesktopShared.Backup.Appassure.Replication.InQueueMaximum) && (_backup.RecoveryPointsReplicated < DesktopShared.Backup.Appassure.Replication.RecoveryPointsReplicatedMinimum));

                //in queue
                TelerikHelper.AddLabelToCell(_backup.InQueue.ToString().Trim(), String.Format("{0} In Queue", _backup.InQueue), item["InQueue"]);
                if (_displayError)
                    item["InQueue"].ForeColor = System.Drawing.ColorTranslator.FromHtml("#DB2929"); // red

                //queued over 24 hours
                TelerikHelper.AddLabelToCell(_backup.QueuedOver24Hours.ToString().Trim(), String.Format("{0} Queued Over 24 Hours", _backup.QueuedOver24Hours), item["QueuedOver24Hours"]);

                //waiting to replicate
                TelerikHelper.AddLabelToCell(_backup.WaitingToReplicate.ToString().Trim(), String.Format("{0} GB - Waiting To Replicate", _backup.WaitingToReplicate), item["WaitingToReplicate"]);

                //recovery points replicated
                TelerikHelper.AddLabelToCell(_backup.RecoveryPointsReplicated.ToString().Trim(), String.Format("{0} Recovery Points Replicated", _backup.RecoveryPointsReplicated), item["RecoveryPointsReplicated"]);
                if (_displayError)
                    item["RecoveryPointsReplicated"].ForeColor = System.Drawing.ColorTranslator.FromHtml("#DB2929"); // red

                //amount replicated
                TelerikHelper.AddLabelToCell(_backup.AmountReplicated.ToString().Trim(), String.Format("{0} GB - Amount Replicated", _backup.AmountReplicated), item["AmountReplicated"]);

                //average replication speed
                TelerikHelper.AddLabelToCell(_backup.AverageReplicationSpeed.ToString().Trim(), String.Format("{0} MBS - Average Replication Speed", _backup.AverageReplicationSpeed), item["AverageReplicationSpeed"]);
            }
            else
            {
                item["InQueue"].Text = "";
                item["QueuedOver24Hours"].Text = "";
                item["WaitingToReplicate"].Text = "";
                item["RecoveryPointsReplicated"].Text = "";
                item["AmountReplicated"].Text = "";
                item["AverageReplicationSpeed"].Text = "";
            }

            #endregion
            */

            #endregion
        }

        #endregion

    }

    /// <summary>
    /// grid on pre render
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rgClientBackupReport_PreRender(object sender, EventArgs e)
    {
        #region default - show header, footer, pager.  set no records text

        rgClientBackupReport.ShowHeader = true;
        rgClientBackupReport.ShowFooter = true;

        rgClientBackupReport.PagerStyle.AlwaysVisible = true;
        rgClientBackupReport.PagerStyle.Visible = true;

        rgClientBackupReport.MasterTableView.NoMasterRecordsText = "No records found.";

        #endregion

        GridCommandItem commandItem = null; //add link button 

        if (rgClientBackupReport.MasterTableView.Items.Count > 0)
        {
            commandItem = (GridCommandItem)rgClientBackupReport.MasterTableView.GetItems(GridItemType.CommandItem)[0];
            commandItem.Visible = true;
        }

        #region command item template -> populate export to type drop down list

        if (rgClientBackupReport.MasterTableView.GetItems(GridItemType.CommandItem).Length > 0)
        {
            UserControl_DropDownList_GridExportType ucGridExportType =
                rgClientBackupReport.MasterTableView.GetItems(GridItemType.CommandItem)[0].FindControl("ucGridExportType")
                as UserControl_DropDownList_GridExportType;

            ucGridExportType.Populate();
        }

        #endregion

    }

    /// <summary>
    /// grid on item command
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rgClientBackupReport_ItemCommand(object sender, GridCommandEventArgs e)
    {
        bool _rebind = false;
        string _confirmationMessage = "";

        if (e.CommandName == "Delete")
        {
            int _backupSetId = Convert.ToInt32((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["BackupSetId"]);
            DesktopShared.EntityClasses.BackupSetEntity backupSet = new DesktopShared.EntityClasses.BackupSetEntity(_backupSetId);
            backupSet.BackupMonitor.DeleteMulti();
            backupSet.Delete();
            _rebind = true;
            _confirmationMessage = "Backup task has been deleted";
        }

        if (_rebind)
            ResetGrid();

        if (_confirmationMessage.Trim().Length > 0)
        {
            rnConfirmation.Text = _confirmationMessage;
            rnConfirmation.Show();
        }
    }

    #endregion

    /// <summary>
    /// records per page drop list on selected index changed -> set number of records to display per grid page and rebind grid
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlRecordsPerPage_SelectedChange(object sender, EventArgs e)
    {
        TicketsPerPage = ucRecordsPerPage.SelectedValue;
        rgClientBackupReport.Rebind();
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
           rgClientBackupReport.MasterTableView.GetItems(GridItemType.CommandItem)[0].FindControl("ucGridExportType")
           as UserControl_DropDownList_GridExportType;

        if (ucGridExportType.Mode == UserControl_DropDownList_GridExportType.ExportMode.Csv)
            rgClientBackupReport.MasterTableView.ExportToCSV();
        else if (ucGridExportType.Mode == UserControl_DropDownList_GridExportType.ExportMode.Excel)
            rgClientBackupReport.MasterTableView.ExportToExcel();
        else if (ucGridExportType.Mode == UserControl_DropDownList_GridExportType.ExportMode.Word)
            rgClientBackupReport.MasterTableView.ExportToWord();
        else if (ucGridExportType.Mode == UserControl_DropDownList_GridExportType.ExportMode.Pdf)
            rgClientBackupReport.MasterTableView.ExportToPdf();
    }

    #endregion

    #region private methods

    /// <summary>
    /// set up records per page -> set selected value in drop down list 
    /// </summary>
    private void SetUpRecordsPerPage()
    {
        ucRecordsPerPage.SelectedValue = TicketsPerPage;
    }

    /// <summary>
    /// configure grid for export
    /// </summary>
    private void ConfigureExport()
    {
        rgClientBackupReport.ExportSettings.IgnorePaging = true;
        rgClientBackupReport.ExportSettings.OpenInNewWindow = true;
        rgClientBackupReport.ExportSettings.ExportOnlyData = true;
        rgClientBackupReport.ExportSettings.HideStructureColumns = true;

        rgClientBackupReport.ExportSettings.FileName = "Backup_" + DateTime.Now.ToString("yyyyMMdd-hhmmss");

        //hide command item template
        foreach (GridItem commandItem in this.rgClientBackupReport.MasterTableView.GetItems(GridItemType.CommandItem))
        {
            commandItem.Visible = false;
            commandItem.Display = false;
        }

        _isExport = true;
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
                return 0;
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
    /// get/set hide grid (stored in viewstate) todo: needed? - use ajax CollapsiblePanelExtender instead?
    /// </summary>
    public bool HideGrid
    {
        get
        {
            object obj = this.ViewState["HideGridForClientBackupSet"];
            if (obj == null)
                return false;
            else
                return (bool)obj;
        }

        set
        {
            this.ViewState["HideGridForClientBackupSet"] = value;
        }
    }

    #region search criteria

    /// <summary>
    /// get/set search client id (stored in viewstate)
    /// </summary>
    public int? SearchClientId
    {
        get
        {
            object obj = this.ViewState["SearchClientIdForBackupReport"];
            if (obj == null)
                return null;
            else
                return (int)obj;
        }
        set { this.ViewState["SearchClientIdForBackupReport"] = value; }
    }
    
    /// <summary>
    /// get/set search backup monitor status id (stored in viewstate)
    /// </summary>
    public int? SearchBackupMonitorStatusId
    {
        get
        {
            object obj = this.ViewState["SearchMonitorStatusForBackupReport"];
            if (obj == null)
                return null;
            else
                return (int)obj;
        }
        set { this.ViewState["SearchMonitorStatusForBackupReport"] = value; }
    }

    /// <summary>
    /// get/set search start date (stored in viewstate)
    /// </summary>
    public DateTime SearchStartDate
    {
        get
        {
            object obj = this.ViewState["SearchStartDateForBackupReport"];
            if (obj == null)
                return DateTime.MinValue;
            else
                return (DateTime)obj;
        }
        set { this.ViewState["SearchStartDateForBackupReport"] = value; }
    }

    /// <summary>
    /// get/set search end date (stored in viewstate)
    /// </summary>
    public DateTime SearchEndDate
    {
        get
        {
            object obj = this.ViewState["SearchEndDateForBackupReport"];
            if (obj == null)
                return DateTime.MinValue;
            else
                return (DateTime)obj;
        }
        set { this.ViewState["SearchEndDateForBackupReport"] = value; }
    }

    /// <summary>
    /// get/set search server
    /// </summary>
    public string SearchServer
    {
        get
        {
            object obj = this.ViewState["SearchServerForBackupReport"];
            return obj == null ? "" : (string)obj;
        }
        set { this.ViewState["SearchServerForBackupReport"] = value; }
    }

    /// <summary>
    /// get/set search server name
    /// </summary>
    public string SearchServerName
    {
        get
        {
            object obj = this.ViewState["ssn_br"];
            return obj == null ? "" : (string)obj;
        }
        set { this.ViewState["ssn_br"] = value; }
    }

    /// <summary>
    /// get/set search core
    /// </summary>
    public string SearchCore
    {
        get
        {
            object obj = this.ViewState["SearchCoreForBackupReport"];
            return obj == null ? "" : (string)obj;
        }
        set { this.ViewState["SearchCoreForBackupReport"] = value; }
    }

    /// <summary>
    /// get/set search number of days unsuccessful
    /// </summary>
    public int? SearchNumberOfDaysUnsuccessful
    {
        get
        {
            object obj = this.ViewState["SearchNumberOfDaysUnsuccessfulForBackupReport"];
            return obj == null ? (int?)null : (int)obj;
        }
        set { this.ViewState["SearchNumberOfDaysUnsuccessfulForBackupReport"] = value; }
    }

    /// <summary>
    /// get/set search has ticket
    /// </summary>
    public bool? SearchHasTicket
    {
        get
        {
            object obj = this.ViewState["SearchHasTicketForBackupReport"];
            return obj == null ? (bool?)null : (bool)obj;
        }
        set { this.ViewState["SearchHasTicketForBackupReport"] = value; }
    }

    /// <summary>
    /// get/set search has rollup data
    /// </summary>
    public bool? SearchHasRollupData
    {
        get
        {
            object obj = this.ViewState["SearchHasRollupDataForBackupReport"];
            return obj == null ? (bool?)null : (bool)obj;
        }
        set { this.ViewState["SearchHasRollupDataForBackupReport"] = value; }
    }

    /// <summary>
    /// get/set search has replication data
    /// </summary>
    public bool? SearchHasReplicationData
    {
        get
        {
            object obj = this.ViewState["SearchHasReplicationDataForBackupReport"];
            return obj == null ? (bool?)null : (bool)obj;
        }
        set { this.ViewState["SearchHasReplicationDataForBackupReport"] = value; }
    }

    /// <summary>
    /// get/set search is unsuccessful
    /// </summary>
    public bool? SearchIsUnsuccessful
    {
        get
        {
            object obj = this.ViewState["SearchIsUnsuccessfulForBackupReport"];
            return obj == null ? (bool?)null : (bool)obj;
        }
        set { this.ViewState["SearchIsUnsuccessfulForBackupReport"] = value; }
    }

    /// <summary>
    /// get/set search backup tyep id
    /// </summary>
    public int? SearchBackupTypeId
    {
        get
        {
            object obj = this.ViewState["SearchSearchBackupTypeIdForBackupReport"];
            return obj == null ? (int?)null : (int)obj;
        }
        set { this.ViewState["SearchSearchBackupTypeIdForBackupReport"] = value; }
    }

    /// <summary>
    /// get/set search backup tyep id
    /// </summary>
    public int? SearchVeeamBackupTypeId
    {
        get
        {
            object obj = this.ViewState["svbtid_br"];
            return obj == null ? (int?)null : (int)obj;
        }
        set { this.ViewState["svbtid_br"] = value; }
    }

    #endregion

    #endregion

    #region public methods

    /// <summary>
    /// reset grid
    /// </summary>
    public void ResetGrid()
    {
        rgClientBackupReport.Visible = true;
        pnlHeader.Visible = true;

        rgClientBackupReport.EditIndexes.Clear();
        rgClientBackupReport.DataSource = null;
        rgClientBackupReport.Rebind();
    }

    /// <summary>
    /// clear grid
    /// </summary>
    public void ClearGrid()
    {
        rgClientBackupReport.CurrentPageIndex = 0;
        rgClientBackupReport.EditIndexes.Clear();
        rgClientBackupReport.Visible = false;
        pnlHeader.Visible = false;
    }

    #endregion

    #region removed columns

    /*
    <telerik:GridBoundColumn UniqueName="Id" DataField="Id" SortExpression="Id" HeaderText="Tick." HeaderTooltip="Ticket" HeaderStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Top" />
    <telerik:GridBoundColumn UniqueName = "SpaceUsed" DataField="SpaceUsed" SortExpression="SpaceUsed" HeaderText="Used" HeaderTooltip="Space Used (GB)" HeaderStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Top" HeaderStyle-Width="60px"/>
    <telerik:GridBoundColumn UniqueName = "SpaceFree" DataField="SpaceFree" SortExpression="SpaceFree" HeaderText="Free" HeaderTooltip="Space Free (GB)" HeaderStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Top" HeaderStyle-Width="60px"/>
    <telerik:GridBoundColumn UniqueName = "AmtDataChanged" DataField="AmtDataChanged" SortExpression="AmtDataChanged" HeaderText="Changed" HeaderTooltip="Amount Data Changed (GB)" HeaderStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Top" />
    <telerik:GridBoundColumn UniqueName = "BackupSize" DataField="BackupSize" SortExpression="BackupSize" HeaderText="Size" HeaderTooltip="Backup Size (GB)" HeaderStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Top" />
    <telerik:GridBoundColumn UniqueName = "SSPass" DataField="SSPass" SortExpression="SSPass" HeaderText="S-Pass" HeaderTooltip="Snapshots Passed" HeaderStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Top" />
    <telerik:GridBoundColumn UniqueName = "SSFail" DataField="SSFail" SortExpression="SSFail" HeaderText="S-Fail" HeaderTooltip="Snapshots Failed" HeaderStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Top" />
    <telerik:GridBoundColumn UniqueName = "DaysSuccessful" DataField="DaysSuccessful" SortExpression="DaysSuccessful" HeaderText="Days Succ."  HeaderTooltip="Days Successful" HeaderStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Top" />
    <telerik:GridBoundColumn UniqueName = "DaysUnsuccessful" DataField="DaysUnsuccessful" SortExpression="DaysUnsuccessful" HeaderText="Days Unsucc."  HeaderTooltip="Days Unsuccessful" HeaderStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Top" />
    <telerik:GridBoundColumn UniqueName = "Rollup" DataField="Rollup" SortExpression="Rollup" HeaderText="Rollups"  HeaderTooltip="Rollup" HeaderStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Top" />
    <telerik:GridBoundColumn UniqueName = "Duration" DataField="Duration" SortExpression="Duration" HeaderText="Dur."  HeaderTooltip="Duration" HeaderStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Top" />
    <telerik:GridBoundColumn UniqueName = "Throughput" DataField="Throughput" SortExpression="Throughput" HeaderText="TP."  HeaderTooltip="Throughput" HeaderStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Top" />
    <telerik:GridBoundColumn UniqueName = "RecoveryPointsConsolidated" DataField="RecoveryPointsConsolidated" SortExpression="RecoveryPointsConsolidated" HeaderText="Rec. Pts. Cons."  HeaderTooltip="Recovery Points Consolidated" HeaderStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Top" />
    <telerik:GridBoundColumn UniqueName = "InQueue" DataField="InQueue" SortExpression="InQueue" HeaderText="In Q."  HeaderTooltip="In Queue" HeaderStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Top" />
    <telerik:GridBoundColumn UniqueName = "QueuedOver24Hours" DataField="QueuedOver24Hours" SortExpression="QueuedOver24Hours" HeaderText="Q.>24"  HeaderTooltip="Queued Over 24 Hours" HeaderStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Top" />
    <telerik:GridBoundColumn UniqueName = "WaitingToReplicate" DataField="WaitingToReplicate" SortExpression="WaitingToReplicate" HeaderText="Rep. Wait"  HeaderTooltip="Waiting To Replicate (GB)" HeaderStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Top" />
    <telerik:GridBoundColumn UniqueName = "RecoveryPointsReplicated" DataField="RecoveryPointsReplicated" SortExpression="RecoveryPointsReplicated" HeaderText="Rec. Pts. Repl."  HeaderTooltip="Recovery Points Replicated" HeaderStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Top" />
    <telerik:GridBoundColumn UniqueName = "AmountReplicated" DataField="AmountReplicated" SortExpression="AmountReplicated" HeaderText="Amt. Repl."  HeaderTooltip="Amount Replicated (GB)" HeaderStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Top" />
    <telerik:GridBoundColumn UniqueName = "AverageReplicationSpeed" DataField="AverageReplicationSpeed" SortExpression="AverageReplicationSpeed" HeaderText="Rep. Spd."  HeaderTooltip="Average Replication Speed (MBS)" HeaderStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Top" />
    */
    #endregion
}