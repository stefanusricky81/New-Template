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
using System.Collections.Generic;

public partial class UserControl_Grid_ClientBackupMonitor : System.Web.UI.UserControl
{

    protected void Page_Load(object sender, EventArgs e)
    {
        //records per page event handler
        DropDownList ddlRecordsPerPage = ucRecordsPerPage.GetDropDownList();
        ddlRecordsPerPage.SelectedIndexChanged += new EventHandler(ddlRecordsPerPage_SelectedChange);

        if (!this.IsPostBack)
        {
            SetShowHide(); //hide/show grid click todo: needed? 
            SetUpRecordsPerPage(); //set value for drop down list control for number of records to display
        }
    }

    #region protected events

    #region telerik grid events

    /// <summary>
    /// grid on need data source
    /// </summary>
    /// <param name="source"></param>
    /// <param name="e"></param>
    protected void rgClientBackupMonitor_NeedDataSource(object source, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        phCssClip.Visible = !ClipCellContent;
        CreateColumns();
        DesktopShared.TypedListClasses.BackupMonitorTypedList monitors = DesktopShared.Backup.Monitor.GetTypedList(SearchClientId, SearchDate, SearchStatustId, SearchrblUpdatedFilter, SearchTeamId, SearchServerName, SearchMultipleStatustIds);
        monitors.DefaultView.Sort = "ClientCompany Asc";
        rgClientBackupMonitor.DataSource = monitors;

        #region paging

        if (TicketsPerPage > 0)
            rgClientBackupMonitor.PageSize = TicketsPerPage;
        else
        {
            if (monitors.Rows.Count > 0)
                rgClientBackupMonitor.PageSize = monitors.Rows.Count;
        }

        #endregion
    }

    /// <summary>
    /// grid on item databound
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rgClientBackupMonitor_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        #region GridEditFormItem and IsInEditMode - edit 1 or batch edit

        if (e.Item is GridEditFormItem && e.Item.IsInEditMode)
        {
            rgClientBackupMonitor.MasterTableView.NoMasterRecordsText = "";

            #region edit existing

            if (!e.Item.OwnerTableView.IsItemInserted)
            {
                #region default edit one item

                if (!IsBatchEditMode)
                {

                    AbstractBackupMonitorEdit ucBackupMonitorEdit = e.Item.FindControl
                        (GridEditFormItem.EditFormUserControlID) as AbstractBackupMonitorEdit;

                    DesktopShared.TypedListClasses.BackupMonitorRow _backupRow =
                        (DesktopShared.TypedListClasses.BackupMonitorRow)
                        ((DataRowView)e.Item.DataItem).Row;
                    ucBackupMonitorEdit.LoadValues(_backupRow);
                }

                #endregion

                #region batch edit all

                else
                {
                    AbstractBackupMonitorBatchEdit ucBackupMonitorEdit = e.Item.FindControl
                        (GridEditFormItem.EditFormUserControlID) as AbstractBackupMonitorBatchEdit;

                    DataRowView drv = (DataRowView)e.Item.DataItem;
                    ucBackupMonitorEdit.LoadValues(Convert.ToInt32(drv["Id"].ToString().Trim()));
                }

                #endregion

            }

            #endregion

        }

        #endregion

        #region Item or AlternatingItem

        if (e.Item.ItemType == GridItemType.Item || e.Item.ItemType == GridItemType.AlternatingItem)
        {
            GridEditableItem item = e.Item as GridEditableItem;
            DesktopShared.TypedListClasses.BackupMonitorRow _backup = (DesktopShared.TypedListClasses.BackupMonitorRow)((DataRowView)e.Item.DataItem).Row;

            #region begin new

            TelerikHelper.AddLabelToCell(_backup.IsLastUpdatedNull() ? "" : _backup.LastUpdated.ToString("MM/dd HH:mm"), item["LastUpdated"]);
            TelerikHelper.AddLabelToCell(_backup.ClientCompany.Trim(), item["ClientCompany"]);
            TelerikHelper.AddLabelToCell(_backup.Server.Trim(), item["Server"]);
            TelerikHelper.AddLabelToCell(_backup.BackupSetName.Trim(), item["BackupSetName"]);
            TelerikHelper.AddLabelToCell(_backup.BackupSetDescription.Trim(), item["BackupSetDescription"]);
            TelerikHelper.AddLabelToCell(_backup.BackupSetBackupServer.Trim(), item["BackupSetBackupServer"]);
            TelerikHelper.AddLabelToCell(_backup.SupportingTableDescription.Trim(), item["SupportingTableDescription"]);
            TelerikHelper.AddLabelToCell(_backup.BackupMonitorStatusName.Trim(), item["BackupMonitorStatusName"]);
            TelerikHelper.AddLabelToCell(_backup.TeamName.Trim(), item["TeamName"]);
            TelerikHelper.AddLabelToCell(_backup.Comment.Trim(), item["Comment"]);
            TelerikHelper.AddLabelToCell(_backup.IsAppassureReportStartDateTimeNull() ? "" : _backup.AppassureReportStartDateTime.ToString("MM/dd HH:mm"), item["AppassureReportStartDateTime"]);

            #endregion

            #region status

            TableCell tcStatusName = item["BackupMonitorStatusName"];

            if (tcStatusName.Text.ToLower() == "successful")
                tcStatusName.ForeColor = System.Drawing.ColorTranslator.FromHtml("#24BF49"); // green
            else if (tcStatusName.Text.ToLower() == "unsuccessful")
                tcStatusName.ForeColor = System.Drawing.ColorTranslator.FromHtml("#DB2929"); // red
            else if (tcStatusName.Text.ToLower() == "warning")
                tcStatusName.ForeColor = System.Drawing.ColorTranslator.FromHtml("#FFA500"); // orange
            

            #endregion

        }

        #endregion


    }

    /// <summary>
    /// grid on update command -> edit entity (use EditForm/BackupMonitorEdit.ascx)
    /// </summary>
    /// <param name="source"></param>
    /// <param name="e"></param>
    protected void rgClientBackupMonitor_UpdateCommand(object source, GridCommandEventArgs e)
    {
        if (Page.IsValid)
        {
            GridEditableItem editedItem = e.Item as GridEditableItem;
            int Id = Convert.ToInt32(editedItem.OwnerTableView.DataKeyValues
                [editedItem.ItemIndex]["Id"]);

            AbstractBackupMonitorEdit ucBackupMonitorEdit = e.Item.FindControl
                (GridEditFormItem.EditFormUserControlID) as AbstractBackupMonitorEdit;

            ucBackupMonitorEdit.SaveValues(Id);
        }
    }

    /// <summary>
    /// grid on item command
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rgClientBackupMonitor_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        bool doRebind = false;
        RadGrid grid = (sender as RadGrid);

        #region edit click

        if (e.CommandName == RadGrid.EditCommandName)
        {
            IsBatchEditMode = false;

            e.Item.OwnerTableView.IsItemInserted = false;
            e.Item.OwnerTableView.EditFormSettings.UserControlName = "/UserControl/Grid/EditForm/BackupMonitorEdit.ascx";
        }

        #endregion

        #region refresh grid click

        else if (e.CommandName == "RefreshGrid")
        {
            doRebind = true;
        }

        #endregion

        #region edit all

        else if (e.CommandName == "EditAll")
        {
            IsBatchEditMode = true;

            e.Item.OwnerTableView.IsItemInserted = false;
            e.Item.OwnerTableView.EditFormSettings.UserControlName = "/UserControl/Grid/EditForm/BackupMonitorBatchEdit.ascx";

            foreach (GridItem _gridItem in rgClientBackupMonitor.MasterTableView.Items)
            {
                if (_gridItem is GridEditableItem)
                {
                    GridEditableItem _gridEditableItem = _gridItem as GridDataItem;
                    _gridEditableItem.Edit = true;
                }
            }

            doRebind = true;
        }

        #endregion

        #region update all

        else if (e.CommandName == "UpdateAll")
        {
            if (Page.IsValid)
            {
                if (rgClientBackupMonitor.EditIndexes.Count == 0)
                    return;

                //iterate through grid items in edit mode
                foreach (GridDataItem dataItem in rgClientBackupMonitor.EditItems)
                {
                    int backupMonitorID = Convert.ToInt32(dataItem.OwnerTableView.DataKeyValues
                        [dataItem.ItemIndex]["Id"]);

                    GridEditableItem editedItem = (GridEditableItem)dataItem.EditFormItem;

                    AbstractBackupMonitorBatchEdit ucBackupMonitorEdit = editedItem.FindControl
                        (GridEditFormItem.EditFormUserControlID) as AbstractBackupMonitorBatchEdit;

                    DesktopShared.EntityClasses.BackupMonitorEntity backupMonitor = new DesktopShared.EntityClasses.BackupMonitorEntity(backupMonitorID);

                    if (ucBackupMonitorEdit != null)
                        ucBackupMonitorEdit.SaveValues(backupMonitor);
                }

                IsBatchEditMode = false;
                rgClientBackupMonitor.EditIndexes.Clear();
                doRebind = true;
            }
        }

        #endregion

        #region cancel update all

        else if (e.CommandName == "CancelUpdateAll")
        {
            if (rgClientBackupMonitor.EditIndexes.Count == 0)
                return;

            IsBatchEditMode = false;
            rgClientBackupMonitor.EditIndexes.Clear();
            doRebind = true;

        }

        #endregion

        #region do rebind

        if (doRebind)
        {
            //rebind sender grid
            RebindGrid(rgClientBackupMonitor);

        }

        #endregion
    }

    /// <summary>
    /// grid on pre render
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rgClientBackupMonitor_PreRender(object sender, EventArgs e)
    {

        #region default - show header, footer, pager.  set no records text

        rgClientBackupMonitor.ShowHeader = true;
        rgClientBackupMonitor.ShowFooter = true;

        rgClientBackupMonitor.PagerStyle.AlwaysVisible = true;
        rgClientBackupMonitor.PagerStyle.Visible = true;

        rgClientBackupMonitor.MasterTableView.NoMasterRecordsText = "No records found.";

        #endregion

        GridCommandItem commandItem = null; //add ssl certificate link button 

        if (rgClientBackupMonitor.MasterTableView.Items.Count > 0)
        {
            commandItem = (GridCommandItem)rgClientBackupMonitor.MasterTableView.GetItems(GridItemType.CommandItem)[0];
            commandItem.Visible = true;
            commandItem.Edit = true;
        }

        #region editing backup monitors

        if (rgClientBackupMonitor.EditItems.Count > 0 || rgClientBackupMonitor.MasterTableView.IsItemInserted)
        {
            //hide all other line items while in edit mode
            foreach (GridDataItem item in rgClientBackupMonitor.MasterTableView.Items)
                item.Visible = false;

            //hide header if in edit mode
            rgClientBackupMonitor.ShowHeader = false;

            //hide footer/pager
            //rgClientBackupMonitor.MasterTableView.PagerStyle.AlwaysVisible = false;
            //rgClientBackupMonitor.MasterTableView.PagerStyle.Visible = false;

            if (commandItem != null)
            {
                if (!IsBatchEditMode)
                    commandItem.Visible = false; //hide command template (add ticket, refresh etc) if editing one item
                else
                {
                    //batch edit -> set active view in command item template that has update all / cancel buttons
                    MultiView mv = (MultiView)commandItem.FindControl("mvCommandTemplate");
                    mv.ActiveViewIndex = 1;
                }
            }
        }

        #endregion

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
        rgClientBackupMonitor.Rebind();
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
    /// set show/hide of grid via click -> todo: remove?
    /// </summary>
    private void SetShowHide()
    {
        //<a href="#" class="hide" onclick="showHideInfo(this, '<%=this.ClientID %>'); return false;"></a>

        if (!SetShowHideDone)
        {
            string _linkClass = "hide";
            string _divDisplay = "";

            if (HideGrid)
            {
                _linkClass = "show";
                _divDisplay = "none";
            }

            hlShowHide.Attributes.Add("href", "#");
            hlShowHide.Attributes.Add("class", _linkClass);

            string _onClickFunction = "showHideInfo(this, '" + divClientBackupMonitor.ClientID + "');";
            _onClickFunction += "return false;";
            hlShowHide.Attributes.Add("onclick", _onClickFunction);

            divClientBackupMonitor.Attributes.CssStyle.Add("display", _divDisplay);

            SetShowHideDone = true;
        }
    }

    /// <summary>
    /// create grid columns
    /// </summary>
    private void CreateColumns()
    {
        rgClientBackupMonitor.Columns.Clear();

        GridBoundColumn boundColumn;
        GridEditCommandColumn _gridEditCmdColumn;


        //edit command
        _gridEditCmdColumn = new GridEditCommandColumn();
        rgClientBackupMonitor.MasterTableView.Columns.Add(_gridEditCmdColumn);
        _gridEditCmdColumn.UniqueName = "EditCommandColumn";
        _gridEditCmdColumn.HeaderStyle.Width = Unit.Percentage(2); 
        //_gridEditCmdColumn.HeaderStyle.Width = Unit.Pixel(10);
        _gridEditCmdColumn.EditText = "E";

        //last updated date
        boundColumn = new GridBoundColumn();
        rgClientBackupMonitor.MasterTableView.Columns.Add(boundColumn);
        boundColumn.DataField = "LastUpdated";
        boundColumn.UniqueName = "LastUpdated";
        boundColumn.HeaderText = "Updated";
        boundColumn.HeaderTooltip = "Last Updated";
        boundColumn.SortExpression = "LastUpdated";
        boundColumn.DataFormatString = "{0:MM/dd/yy HH:mm}";
        boundColumn.HeaderStyle.Width = Unit.Percentage(5);
        //boundColumn.HeaderStyle.Width = Unit.Pixel(100);

        //Client Name
        boundColumn = new GridBoundColumn();
        rgClientBackupMonitor.MasterTableView.Columns.Add(boundColumn);
        boundColumn.DataField = "ClientCompany";
        boundColumn.UniqueName = "ClientCompany";
        boundColumn.HeaderText = "Client";
        boundColumn.SortExpression = "ClientCompany";
        boundColumn.HeaderStyle.Width = Unit.Percentage(7);
        //boundColumn.HeaderStyle.Width = Unit.Pixel(100);

        //Server Name
        boundColumn = new GridBoundColumn();
        rgClientBackupMonitor.MasterTableView.Columns.Add(boundColumn);
        boundColumn.DataField = "Server";
        boundColumn.UniqueName = "Server";
        boundColumn.HeaderText = "Server Name";
        boundColumn.SortExpression = "Server";
        boundColumn.HeaderStyle.Width = Unit.Percentage(7);
        //boundColumn.HeaderStyle.Width = Unit.Pixel(100);

        //backup set name
        boundColumn = new GridBoundColumn();
        rgClientBackupMonitor.MasterTableView.Columns.Add(boundColumn);
        boundColumn.DataField = "BackupSetName";
        boundColumn.UniqueName = "BackupSetName";
        boundColumn.HeaderText = "BU Set Name";
        boundColumn.SortExpression = "BackupSetName";
        boundColumn.HeaderStyle.Width = Unit.Percentage(7);
        //boundColumn.HeaderStyle.Width = Unit.Pixel(100);

        //backup server name
        boundColumn = new GridBoundColumn();
        rgClientBackupMonitor.MasterTableView.Columns.Add(boundColumn);
        boundColumn.DataField = "BackupSetBackupServer";
        boundColumn.UniqueName = "BackupSetBackupServer";
        boundColumn.HeaderText = "BU Server";
        boundColumn.SortExpression = "BackupSetBackupServer";
        boundColumn.HeaderStyle.Width = Unit.Percentage(7);

        //backup set description
        boundColumn = new GridBoundColumn();
        rgClientBackupMonitor.MasterTableView.Columns.Add(boundColumn);
        boundColumn.DataField = "BackupSetDescription";
        boundColumn.UniqueName = "BackupSetDescription";
        boundColumn.HeaderText = "BU Set Description";
        boundColumn.SortExpression = "BackupSetDescription";
        boundColumn.HeaderStyle.Width = Unit.Percentage(8);
        //boundColumn.HeaderStyle.Width = Unit.Pixel(200);

        //Backup Type
        boundColumn = new GridBoundColumn();
        rgClientBackupMonitor.MasterTableView.Columns.Add(boundColumn);
        boundColumn.DataField = "SupportingTableDescription";
        boundColumn.UniqueName = "SupportingTableDescription";
        boundColumn.HeaderText = "Backup Type";
        boundColumn.SortExpression = "SupportingTableDescription";
        boundColumn.HeaderStyle.Width = Unit.Percentage(8);
        //boundColumn.HeaderStyle.Width = Unit.Pixel(100);

        //Status
        boundColumn = new GridBoundColumn();
        rgClientBackupMonitor.MasterTableView.Columns.Add(boundColumn);
        boundColumn.DataField = "BackupMonitorStatusName";
        boundColumn.UniqueName = "BackupMonitorStatusName";
        boundColumn.HeaderText = "Status";
        boundColumn.SortExpression = "BackupMonitorStatusName";
        boundColumn.HeaderStyle.Width = Unit.Percentage(5);
        //boundColumn.HeaderStyle.Width = Unit.Pixel(50);

        //team
        boundColumn = new GridBoundColumn();
        rgClientBackupMonitor.MasterTableView.Columns.Add(boundColumn);
        boundColumn.DataField = "TeamName";
        boundColumn.UniqueName = "TeamName";
        boundColumn.HeaderText = "Team";
        boundColumn.SortExpression = "TeamName";
        boundColumn.HeaderStyle.Width = Unit.Percentage(5);

        //Comment
        boundColumn = new GridBoundColumn();
        rgClientBackupMonitor.MasterTableView.Columns.Add(boundColumn);
        boundColumn.DataField = "Comment";
        boundColumn.UniqueName = "Comment";
        boundColumn.HeaderText = "Comment";
        boundColumn.SortExpression = "Comment";
        boundColumn.HeaderStyle.Width = Unit.Percentage(8);

        //boundColumn.HeaderStyle.Width = Unit.Pixel(250);

        //Appassure Backed up Date
        boundColumn = new GridBoundColumn();
        rgClientBackupMonitor.MasterTableView.Columns.Add(boundColumn);
        boundColumn.DataField = "AppassureReportStartDateTime";
        boundColumn.UniqueName = "AppassureReportStartDateTime";
        boundColumn.HeaderText = "BU Date";
        boundColumn.HeaderTooltip = "Backup Date";
        boundColumn.SortExpression = "AppassureReportStartDateTime";
        boundColumn.HeaderStyle.Width = Unit.Percentage(5);
        //boundColumn.HeaderStyle.Width = Unit.Pixel(100);
    }

    /// <summary>
    /// rebind grid
    /// </summary>
    /// <param name="grid"></param>
    private void RebindGrid(Telerik.Web.UI.RadGrid grid)
    {
        if (grid != null)
        {
            grid.DataSource = null;
            grid.Rebind();
        }
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
            return (obj == null) ? 0 : (int)obj;
        }
        set { this.Session["TicketsPerPage"] = value; }
    }

    /// <summary>
    /// get/set is edit all mode
    /// </summary>
    public bool IsBatchEditMode
    {
        get
        {
            object obj = this.ViewState["IsBatchEditMode"];
            return (obj == null) ? false : (bool)obj;
        }
        set { this.ViewState["IsBatchEditMode"] = value; }
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
            return (obj == null) ? false : (bool)obj;
        }
        set { this.ViewState["SetShowHideDoneForClientBackupSet"] = value; }
    }

    /// <summary>
    /// get/set clip cell content (display on one line)
    /// </summary>
    public bool ClipCellContent
    {
        get
        {
            object obj = this.ViewState["ClipCellContentClientBackupSet"];
            return (obj == null) ? true : (bool)obj;
        }
        set { this.ViewState["ClipCellContentClientBackupSet"] = value; }
    }

    /// <summary>
    /// get/set hide grid (stored in viewstate) todo: needed? - use ajax CollapsiblePanelExtender instead?
    /// </summary>
    public bool HideGrid
    {
        get
        {
            object obj = this.ViewState["HideGridForClientBackupSet"];
            return (obj == null) ? false : (bool)obj;
        }
        set { this.ViewState["HideGridForClientBackupSet"] = value; }
    }

    #region search

    /// <summary>
    /// get/set search client id (stored in viewstate)
    /// </summary>
    public int? SearchClientId
    {
        get
        {
            object obj = this.ViewState["SearchClientIdForBackupSet"];
            return (obj == null) ? (int?)null : (int)obj;
        }
        set { this.ViewState["SearchClientIdForBackupSet"] = value; }
    }

    /// <summary>
    /// get/set search team id (stored in viewstate)
    /// </summary>
    public int? SearchTeamId
    {
        get
        {
            object obj = this.ViewState["SearchTeamIdForBackupSet"];
            return (obj == null) ? (int?)null : (int)obj;
        }
        set { this.ViewState["SearchTeamIdForBackupSet"] = value; }
    }

    /// <summary>
    /// get/set search date (stored in viewstate)
    /// </summary>
    public DateTime SearchDate
    {
        get
        {
            object obj = this.ViewState["SearchDateForBackupMonitor"];
            return (obj == null) ? DateTime.MinValue : (DateTime)obj;
        }
        set { this.ViewState["SearchDateForBackupMonitor"] = value; }
    }

    /// <summary>
    /// get/set search status (stored in viewstate)
    /// </summary>
    public int? SearchStatustId
    {
        get
        {
            object obj = this.ViewState["SearchStatusIdForBackupSet"];
            return (obj == null) ? (int?)null : (int)obj;
        }
        set { this.ViewState["SearchStatusIdForBackupSet"] = value; }
    }

    /// <summary>
    /// get/set search multiple status ids (stored in viewstate)
    /// </summary>
    public List<int> SearchMultipleStatustIds
    {
        get
        {
            object obj = this.ViewState["SearchMultipleStatusIdsForBackupSet"];
            return (obj == null) ? null : (List<int>)obj;
        }
        set { this.ViewState["SearchMultipleStatusIdsForBackupSet"] = value; }
    }

    /// <summary>
    /// get/set Is Updated Filter status
    /// </summary>
    public string SearchrblUpdatedFilter
    {
        get
        {
            object obj = this.ViewState["SearchrblUpdatedFilterBackupSet"];
            return (obj == null) ? "" : obj.ToString();
        }
        set { this.ViewState["SearchrblUpdatedFilterBackupSet"] = value; }
    }

    /// <summary>
    /// get/set server status
    /// </summary>
    public string SearchServerName
    {
        get
        {
            object obj = this.ViewState["SearchServerNameForBackupSet"];
            return (obj == null) ? "All" : obj.ToString();
        }
        set { this.ViewState["SearchServerNameForBackupSet"] = value; }
    }

    #endregion

    #endregion

    #region public methods

    /// <summary>
    /// reset grid
    /// </summary>
    public void ResetGrid()
    {
        rgClientBackupMonitor.Visible = true;
        pnlHeader.Visible = true;
        rgClientBackupMonitor.EditIndexes.Clear();
        rgClientBackupMonitor.DataSource = null;
        rgClientBackupMonitor.Rebind();
    }

    /// <summary>
    /// clear grid
    /// </summary>
    public void ClearGrid()
    {
        rgClientBackupMonitor.CurrentPageIndex = 0;
        rgClientBackupMonitor.EditIndexes.Clear();
        rgClientBackupMonitor.Visible = false;
        pnlHeader.Visible = false;
    }

    #endregion
}