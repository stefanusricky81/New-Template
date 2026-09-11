using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SD.LLBLGen.Pro.ORMSupportClasses;
using Telerik.Web.UI;
using System.Data;

public partial class UserControl_Grid_SearchActivity : System.Web.UI.UserControl
{
    bool _isExport = false;

    protected void Page_Load(object sender, EventArgs e)
    {
        //ajax manager, set conditional postback script to cancel ajax on grid export
        Telerik.Web.UI.RadAjaxManager.GetCurrent(this.Page).ClientEvents.OnRequestStart = "conditionalPostback";
    }

    #region protected methods / events

    #region telerik grid events

    /// <summary>
    /// grid on need data source
    /// </summary>
    /// <param name="source"></param>
    /// <param name="e"></param>
    protected void rgSearchActivity_NeedDataSource(object source, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        //bind
        rgSearchActivity.DataSource = DesktopShared.Activity.Search(SearchIncludeUserCheckIns, SearchTicketTag, SearchTicketTagIsPublic, SearchTeamId, SearchUserId, SearchClientId, SearchStartDate, SearchEndDate); 

    }

    /// <summary>
    /// grid on item created
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rgSearchActivity_ItemCreated(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        #region Item or AlternatingItem

        /*
        if (e.Item.ItemType == GridItemType.Item || e.Item.ItemType == GridItemType.AlternatingItem)
        {
            GridDataItem item = e.Item as GridDataItem;
        }
        */

        #endregion
    }

    /// <summary>
    /// grid on item databound
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rgSearchActivity_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {

        #region Item or AlternatingItem

        if (e.Item.ItemType == GridItemType.Item || e.Item.ItemType == GridItemType.AlternatingItem)
        {
            GridEditableItem item = e.Item as GridEditableItem;
            DataRowView drv = (DataRowView)e.Item.DataItem;
            
            //ticket id
            int _ticketId = 0;
            int.TryParse(drv["TicketId"].ToString(), out _ticketId);

            #region employee name -> activity notes tooltip

            string _employeeFullName = String.Format("{0} {1}", drv["UserFirst"].ToString().Trim(), drv["UserLast"].ToString().Trim());
          
            //table cell
            TableCell tcUserFirst = item["UserFirst"];
            tcUserFirst.Controls.Clear();

             //label for employee full name and tooltip
            Label lblUserFirst = new Label();
            lblUserFirst.Text = _employeeFullName;
            lblUserFirst.ID = "lblUserFirst_" + _ticketId;
            tcUserFirst.Controls.Add(lblUserFirst); 

            //activity notes tooltip
            string _userActivityNotes = drv["UserActivityNotes"].ToString().Trim();
            Telerik.Web.UI.RadToolTip _toolTipUserActivityNotes = new RadToolTip();
            _toolTipUserActivityNotes.Text = _userActivityNotes.Replace("\n", "<br />");
            _toolTipUserActivityNotes.TargetControlID = lblUserFirst.ID.ToString();
            _toolTipUserActivityNotes.AutoCloseDelay = 5000;
            _toolTipUserActivityNotes.Width = Unit.Pixel(400);
            _toolTipUserActivityNotes.Skin = "BitByBit";
            _toolTipUserActivityNotes.EnableEmbeddedSkins = false;
            _toolTipUserActivityNotes.EnableEmbeddedBaseStylesheet = false;
            _toolTipUserActivityNotes.Title = _employeeFullName + " Activity Notes";
            tcUserFirst.Controls.Add(_toolTipUserActivityNotes);

            #endregion

            //start time
            DateTime _ticketStart = DateTime.MinValue;
            DateTime.TryParse(drv["ActiveTicketStartTime"].ToString(), out _ticketStart);
            if (_ticketStart != DateTime.MinValue)
                TelerikHelper.AddLabelToCell(_ticketStart.ToString("MM/dd/yy HH:mm"), item["ActiveTicketStartTime"]);

            //end time
            DateTime _ticketEnd = DateTime.MinValue;
            DateTime.TryParse(drv["ActiveTicketEndTime"].ToString(), out _ticketEnd);
            if (_ticketEnd != DateTime.MinValue)
                TelerikHelper.AddLabelToCell(_ticketEnd.ToString("MM/dd/yy HH:mm"), item["ActiveTicketEndTime"]);
            else
            {
                item["ActiveTicketEndTime"].Controls.Clear();
                item["ActiveTicketEndTime"].Text = "";
            }

            //ticket summary 
            if (_ticketId > 0)
            {
                TelerikHelper.AddHyperLinkToCell(
                    String.Format("{0} - {1}", _ticketId, drv["TicketSummary"].ToString().Trim()),
                    String.Format("/Ticket/Detail2.aspx?Id={0}", _ticketId),
                    item["TicketSummary"]);
            }

            //ticket estimate
            TelerikHelper.AddLabelToCell(drv["TicketEstimate"].ToString(), item["TicketEstimate"]);

            //ticket client name
            TelerikHelper.AddLabelToCell(drv["ClientCompany"].ToString().Trim(), item["ClientCompany"]);

            #region ticket entered

            //date entered display in grid
            DateTime _entered = DateTime.MinValue;
            string _ticketEnteredDisplay = "";
            DateTime.TryParse(drv["TicketDateEntered"].ToString(), out _entered);
            if (_entered != DateTime.MinValue)
                _ticketEnteredDisplay = _entered.ToString("MM/dd/yy HH:mm");

            //table cell
            TableCell tcTicketDateEntered = item["TicketDateEntered"];
            tcTicketDateEntered.Controls.Clear();

            //label for date/time entered and tooltip
            Label lblEntered = new Label();
            lblEntered.Text = _ticketEnteredDisplay;
            lblEntered.ID = "lblDateEntered_" + _ticketId;
            tcTicketDateEntered.Controls.Add(lblEntered);

            string _description = drv["TicketDescription"].ToString().Trim();

            if (_description.Length > 0)
            {
                //entered by
                string _enteredBy = drv["TicketEnteredByFirstName"].ToString().Trim();
                if (_enteredBy.Length > 0)
                    _enteredBy += " ";
                _enteredBy += drv["TicketEnteredByLastName"].ToString().Trim();
                if (_enteredBy.Length > 0)
                    _enteredBy = " By " + _enteredBy;

                //entered date/time
                if (_entered != DateTime.MinValue)
                    _enteredBy += " (" + _entered.ToString("dddd MM/dd/yy HH:mm:ss") + ") ";

                //entered by / date-time tooltip
                Telerik.Web.UI.RadToolTip _toolTipDescription = new RadToolTip();
                _toolTipDescription.Text = _description.Replace("\n", "<br />");
                _toolTipDescription.TargetControlID = lblEntered.ID.ToString();
                _toolTipDescription.AutoCloseDelay = 5000;
                _toolTipDescription.Width = Unit.Pixel(400);
                _toolTipDescription.Skin = "BitByBit";
                _toolTipDescription.EnableEmbeddedSkins = false;
                _toolTipDescription.EnableEmbeddedBaseStylesheet = false;
                _toolTipDescription.Title = "Entered" + _enteredBy;
                tcTicketDateEntered.Controls.Add(_toolTipDescription);
            }

            #endregion

            #region ticket last updated external
            
            #region date 

            DateTime _updated = DateTime.MinValue;
            string _ticketUpdatedDisplay = "";
            DateTime.TryParse(drv["TicketHistoryCreated"].ToString(), out _updated);
            if (_updated != DateTime.MinValue)
                _ticketUpdatedDisplay = _updated.ToString("MM/dd/yy HH:mm");    
            TelerikHelper.AddLabelToCell(_ticketUpdatedDisplay, item["TicketHistoryCreated"]);
            
            #endregion

            #region notes

            TelerikHelper.AddLabelToCell(drv["TicketHistoryNotes"].ToString(), item["TicketHistoryNotes"]);
            if (!ClipDisplayNotes)
                RemoveClipSettingForNotes(item["TicketHistoryNotes"]);
           
            #endregion
            
            #endregion

            #region ticket last updated internal

            #region date
            
            DateTime _updatedInternal = DateTime.MinValue;
            string _ticketUpdatedInternalDisplay = "";
            DateTime.TryParse(drv["InternalTicketHistoryCreated"].ToString(), out _updatedInternal);
            if (_updatedInternal != DateTime.MinValue)
                _ticketUpdatedInternalDisplay = _updatedInternal.ToString("MM/dd/yy HH:mm");
            TelerikHelper.AddLabelToCell(_ticketUpdatedInternalDisplay, item["InternalTicketHistoryCreated"]);

            #endregion

            #region notes

            TelerikHelper.AddLabelToCell(drv["InternalTicketHistoryNotes"].ToString(), item["InternalTicketHistoryNotes"]);
            if (!ClipDisplayNotes)
                RemoveClipSettingForNotes(item["InternalTicketHistoryNotes"]);

            #endregion

            #endregion
        }

        #endregion

    }

    /// <summary>
    /// grid on pre render
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rgSearchActivity_PreRender(object sender, EventArgs e)
    {
        #region default - show header, footer, pager.  set no records text

        rgSearchActivity.ShowHeader = true;
        rgSearchActivity.ShowFooter = false;

        rgSearchActivity.PagerStyle.AlwaysVisible = true;
        rgSearchActivity.PagerStyle.Visible = true;

        rgSearchActivity.MasterTableView.NoMasterRecordsText = "No records found.";

        #endregion

        #region command item template -> populate export to type drop down list

        if (rgSearchActivity.MasterTableView.GetItems(GridItemType.CommandItem).Length > 0)
        {
            UserControl_DropDownList_GridExportType ucGridExportType =
                rgSearchActivity.MasterTableView.GetItems(GridItemType.CommandItem)[0].FindControl("ucGridExportType")
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
    protected void rgSearchActivity_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        bool doRebind = false;
        RadGrid grid = (sender as RadGrid);

        #region refresh grid click

        if (e.CommandName == "RefreshGrid")
            doRebind = true;

        #endregion

        #region do rebind

        if (doRebind)
            ResetGrid();

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
           rgSearchActivity.MasterTableView.GetItems(GridItemType.CommandItem)[0].FindControl("ucGridExportType")
           as UserControl_DropDownList_GridExportType;

        if (ucGridExportType.Mode == UserControl_DropDownList_GridExportType.ExportMode.Csv)
            rgSearchActivity.MasterTableView.ExportToCSV();
        else if (ucGridExportType.Mode == UserControl_DropDownList_GridExportType.ExportMode.Excel)
            rgSearchActivity.MasterTableView.ExportToExcel();
        else if (ucGridExportType.Mode == UserControl_DropDownList_GridExportType.ExportMode.Word)
            rgSearchActivity.MasterTableView.ExportToWord();
        else if (ucGridExportType.Mode == UserControl_DropDownList_GridExportType.ExportMode.Pdf)
            rgSearchActivity.MasterTableView.ExportToPdf();
    }

    #endregion

    #region private methods

    /// <summary>
    /// update css so that notes text is not clipped in table cell
    /// </summary>
    /// <param name="tableCell"></param>
    private void RemoveClipSettingForNotes(TableCell tableCell)
    {
        tableCell.Attributes.CssStyle.Remove("white-space");
        tableCell.Attributes.CssStyle.Remove("overflow");
        tableCell.Attributes.CssStyle.Add("white-space", "normal");
        tableCell.Attributes.CssStyle.Add("overflow", "visible");
    }

    /// <summary>
    /// configure grid for export
    /// </summary>
    private void ConfigureExport()
    {
        rgSearchActivity.ExportSettings.IgnorePaging = true;
        rgSearchActivity.ExportSettings.OpenInNewWindow = true;
        rgSearchActivity.ExportSettings.ExportOnlyData = true;
        rgSearchActivity.ExportSettings.HideStructureColumns = true;

        rgSearchActivity.ExportSettings.FileName = "SearchActivity_" + DateTime.Now.ToString("yyyyMMdd-hhmmss");

        //hide command item template
        foreach (GridItem commandItem in this.rgSearchActivity.MasterTableView.GetItems(GridItemType.CommandItem))
        {
            commandItem.Visible = false;
            commandItem.Display = false;
        }

        _isExport = true;
    }

    #endregion

    
    #region public properties

    #region search criteria

    /// <summary>
    /// get/set search team id
    /// </summary>
    public int? SearchTeamId
    {
        get
        {
            object obj = this.ViewState["SearchTeamIdForCA"];
            return (obj == null) ? (int?)null : (int)obj;
        }
        set { this.ViewState["SearchTeamIdForCA"] = value; }
    }

    /// <summary>
    /// get/set search user id
    /// </summary>
    public int? SearchUserId
    {
        get
        {
            object obj = this.ViewState["SearchUserIdForCA"];
            return (obj == null) ? (int?)null : (int)obj;
        }
        set { this.ViewState["SearchUserIdForCA"] = value; }
    }

    /// <summary>
    /// get/set search client id
    /// </summary>
    public int? SearchClientId
    {
        get
        {
            object obj = this.ViewState["SearchClientIdForCA"];
            return (obj == null) ? (int?)null : (int)obj;
        }
        set { this.ViewState["SearchClientIdForCA"] = value; }
    }

    /// <summary>
    /// get/set search start date
    /// </summary>
    public DateTime? SearchStartDate
    {
        get
        {
            object obj = this.ViewState["SearchStartDateForCA"];
            return (obj == null) ? (DateTime?)null : (DateTime)obj;
        }
        set { this.ViewState["SearchStartDateForCA"] = value; }
    }

    /// <summary>
    /// get/set search end date
    /// </summary>
    public DateTime? SearchEndDate
    {
        get
        {
            object obj = this.ViewState["SearchEndDateForCA"];
            return (obj == null) ? (DateTime?)null : (DateTime)obj;
        }
        set { this.ViewState["SearchEndDateForCA"] = value; }
    }

    /// <summary>
    /// get/set search ticket tag
    /// </summary>
    public string SearchTicketTag
    {
        get
        {
            object obj = this.ViewState["SeachTicketTagForCA"];
            return (obj == null) ? "" : (string)obj;
        }
        set { this.ViewState["SeachTicketTagForCA"] = value; }
    }

    /// <summary>
    /// get/set search ticket tag is public
    /// </summary>
    public bool SearchTicketTagIsPublic
    {
        get
        {
            object obj = this.ViewState["SearchTicketTagIsPublicForCA"];
            return (obj == null) ? false : (bool)obj;
        }
        set { this.ViewState["SearchTicketTagIsPublicForCA"] = value; }
    }

    /// <summary>
    /// get/set search include user check ins
    /// </summary>
    public bool SearchIncludeUserCheckIns
    {
        get
        {
            object obj = this.ViewState["SearchIncludeUserCheckInsForCA"];
            return (obj == null) ? true : (bool)obj;
        }
        set { this.ViewState["SearchIncludeUserCheckInsForCA"] = value; }
    }

    #endregion

    /// <summary>
    /// get/set clip notes for one line display
    /// </summary>
    public bool ClipDisplayNotes
    {
        get
        {
            object obj = this.ViewState["ClipDisplayNotesForCA"];
            return (obj == null) ? false : (bool)obj;
        }
        set { this.ViewState["ClipDisplayNotesForCA"] = value; }
    }

    #endregion

    #region public methods

    /// <summary>
    /// reset grid
    /// </summary>
    public void ResetGrid()
    {
        rgSearchActivity.Visible = true;
        pnlHeader.Visible = true;

        rgSearchActivity.EditIndexes.Clear();
        rgSearchActivity.DataSource = null;
        rgSearchActivity.Rebind();
    }

    /// <summary>
    /// clear grid
    /// </summary>
    public void ClearGrid()
    {
        rgSearchActivity.CurrentPageIndex = 0;
        rgSearchActivity.EditIndexes.Clear();
        rgSearchActivity.Visible = false;
        pnlHeader.Visible = false;
    }

    #endregion
}