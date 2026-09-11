using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SD.LLBLGen.Pro.ORMSupportClasses;
using Telerik.Web.UI;

public partial class UserControl_Grid_EmployeeActivity : System.Web.UI.UserControl
{
    bool _isExport = false;
    Unit _gridUnit = Unit.Percentage(100);

    /// <summary>
    /// page load
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        rgEmployeeActivity.MasterTableView.Width = _gridUnit;
    }

    #region protected methods / events

    #region telerik grid events

    /// <summary>
    /// grid on need data source
    /// </summary>
    /// <param name="source"></param>
    /// <param name="e"></param>
    protected void rgEmployeeActivity_NeedDataSource(object source, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        //get typed list
        //DesktopShared.TypedListClasses.BackupMonitorTypedList monitors = DesktopShared.Backup.Monitor.GetTypedList(SearchClientId, SearchStartDate, SearchEndDate, SearchBackupMonitorStatusId, SearchCore, SearchServer, SearchNumberOfDaysUnsuccessful, 
            //SearchHasTicket, SearchHasRollupData, SearchHasReplicationData, SearchIsUnsuccessful);

        int[] numbers = { 5, 4, 1, 3, 9, 8, 6, 7, 2, 0 };
        string[] strings = { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" };

        var digitOddEvens =
            from n in numbers
            select new { Digit = strings[n], Even = (n % 2 == 0) }; 

        //bind
        rgEmployeeActivity.DataSource = digitOddEvens;
    }

    /// <summary>
    /// grid on item created
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rgEmployeeActivity_ItemCreated(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        #region Item or AlternatingItem

        if (e.Item.ItemType == GridItemType.Item || e.Item.ItemType == GridItemType.AlternatingItem)
        {
            GridDataItem item = e.Item as GridDataItem;
        }

        #endregion
    }

    /// <summary>
    /// grid on item databound
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rgEmployeeActivity_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {

        #region Item or AlternatingItem

        if (e.Item.ItemType == GridItemType.Item || e.Item.ItemType == GridItemType.AlternatingItem)
        {
            GridEditableItem item = e.Item as GridEditableItem;
            /*
            DesktopShared.TypedListClasses.BackupMonitorRow _backup =
                (DesktopShared.TypedListClasses.BackupMonitorRow)
                ((DataRowView)e.Item.DataItem).Row;

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

            //monitor date
            TelerikHelper.AddLabelToCell(_backup.Date.ToString("MM/dd/yy").Trim(), item["Date"]);

            //client name
            TelerikHelper.AddLabelToCell(_backup.ClientCompany.Trim(), item["ClientCompany"]);

            //server
            TelerikHelper.AddLabelToCell(_backup.Server.Trim(), item["Server"]);

            //core
            TelerikHelper.AddLabelToCell(_backup.BackupServer.Trim(), item["BackupServer"]);

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
            
            //backed up date
            TelerikHelper.AddLabelToCell(_backup.AppassureReportStartDateTime.ToString("MM/dd/yy HH:mm").Trim(), _backup.AppassureReportStartDateTime.ToString(), item["AppassureReportStartDateTime"]);

            //updated by
            TelerikHelper.AddLabelToCell(String.Format("{0} {1}", _backup.First.Trim(),_backup.Last.Trim()), item["FullName"]);
            
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
        }

        #endregion

    }

    /// <summary>
    /// grid on pre render
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rgEmployeeActivity_PreRender(object sender, EventArgs e)
    {
        #region default - show header, footer, pager.  set no records text

        rgEmployeeActivity.ShowHeader = true;
        rgEmployeeActivity.ShowFooter = true;

        rgEmployeeActivity.PagerStyle.AlwaysVisible = true;
        rgEmployeeActivity.PagerStyle.Visible = true;

        rgEmployeeActivity.MasterTableView.NoMasterRecordsText = "No records found.";

        #endregion

        #region command item template -> populate export to type drop down list

        if (rgEmployeeActivity.MasterTableView.GetItems(GridItemType.CommandItem).Length > 0)
        {
            UserControl_DropDownList_GridExportType ucGridExportType =
                rgEmployeeActivity.MasterTableView.GetItems(GridItemType.CommandItem)[0].FindControl("ucGridExportType")
                as UserControl_DropDownList_GridExportType;

            ucGridExportType.Populate();
        }

        #endregion

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

        UserControl_DropDownList_GridExportType ucGridExportType =
           rgEmployeeActivity.MasterTableView.GetItems(GridItemType.CommandItem)[0].FindControl("ucGridExportType")
           as UserControl_DropDownList_GridExportType;

        if (ucGridExportType.Mode == UserControl_DropDownList_GridExportType.ExportMode.Csv)
            rgEmployeeActivity.MasterTableView.ExportToCSV();
        else if (ucGridExportType.Mode == UserControl_DropDownList_GridExportType.ExportMode.Excel)
            rgEmployeeActivity.MasterTableView.ExportToExcel();
        else if (ucGridExportType.Mode == UserControl_DropDownList_GridExportType.ExportMode.Word)
            rgEmployeeActivity.MasterTableView.ExportToWord();
        else if (ucGridExportType.Mode == UserControl_DropDownList_GridExportType.ExportMode.Pdf)
            rgEmployeeActivity.MasterTableView.ExportToPdf();
    }

    #endregion

    #region private methods

    /// <summary>
    /// configure grid for export
    /// </summary>
    private void ConfigureExport()
    {
        rgEmployeeActivity.ExportSettings.IgnorePaging = true;
        rgEmployeeActivity.ExportSettings.OpenInNewWindow = true;
        rgEmployeeActivity.ExportSettings.ExportOnlyData = true;
        rgEmployeeActivity.ExportSettings.HideStructureColumns = true;

        rgEmployeeActivity.ExportSettings.FileName = "Backup_" + DateTime.Now.ToString("yyyyMMdd-hhmmss");

        //hide command item template
        foreach (GridItem commandItem in this.rgEmployeeActivity.MasterTableView.GetItems(GridItemType.CommandItem))
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
    /// get/set show hide done (stored in viewstate) todo: needed? - use ajax CollapsiblePanelExtender instead?
    /// </summary>
    public bool SetShowHideDone
    {
        get
        {
            object obj = this.ViewState["SetShowHideDoneForClientBackupSet"];
            if (obj == null)
                return false;
            else
                return (bool)obj;
        }

        set
        {
            this.ViewState["SetShowHideDoneForClientBackupSet"] = value;
        }
    }

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

    #endregion

    /// <summary>
    /// set width of grid.  examples:
    /// </summary>
    public Unit Width
    {
        set { _gridUnit = value; }
    }

    #endregion

    #region public methods

    /// <summary>
    /// reset grid
    /// </summary>
    public void ResetGrid()
    {
        rgEmployeeActivity.Visible = true;
        //pnlHeader.Visible = true;

        rgEmployeeActivity.EditIndexes.Clear();
        rgEmployeeActivity.DataSource = null;
        rgEmployeeActivity.Rebind();
    }

    /// <summary>
    /// clear grid
    /// </summary>
    public void ClearGrid()
    {
        rgEmployeeActivity.CurrentPageIndex = 0;
        rgEmployeeActivity.EditIndexes.Clear();
        rgEmployeeActivity.Visible = false;
        //pnlHeader.Visible = false;
    }

    #endregion

}