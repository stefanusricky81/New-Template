using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using SD.LLBLGen.Pro.ORMSupportClasses;
using System.Data;

public partial class UserControl_Grid_TimesheetTicketSummary : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //records per page event handler
        DropDownList ddlRecordsPerPage = ucRecordsPerPage.GetDropDownList();
        ddlRecordsPerPage.SelectedIndexChanged += new EventHandler(ddlRecordsPerPage_SelectedChange);

        if (!this.IsPostBack)
            SetUpRecordsPerPage(); //set value for drop down list control for number of records to display
    }

    #region protected events

    #region telerk grid

    /// <summary>
    /// grid on need data source
    /// </summary>
    /// <param name="source"></param>
    /// <param name="e"></param>
    protected void rgTimesheetTicketSummary_NeedDataSource(object source, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        if ((SearchStartDate.HasValue)  || (SearchEndDate.HasValue) || (SearchClientId.HasValue))
        {
            rgTimesheetTicketSummary.Visible = true;
            DataTable dtResult = DesktopShared.Timesheet.Report.GetTicketData(SearchStartDate, SearchEndDate, SearchClientId, SearchEmployeeId);
            rgTimesheetTicketSummary.DataSource = dtResult;

            #region paging

            if (RecordsPerPage > 0)
                rgTimesheetTicketSummary.PageSize = RecordsPerPage;
            else
            {
                if (dtResult.Rows.Count > 0)
                    rgTimesheetTicketSummary.PageSize = dtResult.Rows.Count;
            }

            #endregion
        }
        else
            rgTimesheetTicketSummary.Visible = false;
    }

    /// <summary>
    /// grid on item data bound
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rgTimesheetTicketSummary_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        if (e.Item.ItemType == GridItemType.Item || e.Item.ItemType == GridItemType.AlternatingItem)
        {

            #region employee table view

            if (e.Item.OwnerTableView.Name == "Employee")
            {
                GridEditableItem item = e.Item as GridEditableItem;
                DesktopShared.EntityClasses.TimesheetEntity objTimesheet = (DesktopShared.EntityClasses.TimesheetEntity)e.Item.DataItem;

                #region notes / client details

                TableCell tcMemo = item["Memo"];
                tcMemo.Controls.Clear();

                //label 
                Label lblMemo = new Label();
                lblMemo.ID = "lblMemo";
                lblMemo.Text = objTimesheet.Memo.Trim();

                //tooltip 
                if (objTimesheet.Memo.Trim().Length > 0)
                {
                    Telerik.Web.UI.RadToolTip _toolTipMemo = new RadToolTip();
                    _toolTipMemo.Text = objTimesheet.Memo.Trim().Replace("\n", "<br />");
                    _toolTipMemo.TargetControlID = lblMemo.ID.ToString();
                    _toolTipMemo.Width = Unit.Pixel(400);
                    _toolTipMemo.HideEvent = ToolTipHideEvent.ManualClose;
                    _toolTipMemo.ManualCloseButtonText = "Close";
                    _toolTipMemo.Skin = "BitByBit";
                    _toolTipMemo.EnableEmbeddedSkins = false;
                    _toolTipMemo.EnableEmbeddedBaseStylesheet = false;
                    tcMemo.Controls.Add(_toolTipMemo);
                }

                tcMemo.Controls.Add(lblMemo);

                #endregion

                #region internal notes

                TableCell tcInternalComments = item["InternalComments"];
                tcInternalComments.Controls.Clear();

                //label
                Label lblInternalComments = new Label();
                lblInternalComments.ID = "lblInternalComments";
                lblInternalComments.Text = objTimesheet.InternalComments.Trim();

                //tooltip 
                if (objTimesheet.InternalComments.Trim().Length > 0)
                {
                    Telerik.Web.UI.RadToolTip _toolTipInternalComments = new RadToolTip();
                    _toolTipInternalComments.Text = objTimesheet.InternalComments.Trim().Replace("\n", "<br />");
                    _toolTipInternalComments.TargetControlID = lblInternalComments.ID.ToString();
                    _toolTipInternalComments.Width = Unit.Pixel(400);
                    _toolTipInternalComments.HideEvent = ToolTipHideEvent.ManualClose;
                    _toolTipInternalComments.ManualCloseButtonText = "Close";
                    _toolTipInternalComments.Skin = "BitByBit";
                    _toolTipInternalComments.EnableEmbeddedSkins = false;
                    _toolTipInternalComments.EnableEmbeddedBaseStylesheet = false;
                    tcMemo.Controls.Add(_toolTipInternalComments);
                }

                tcInternalComments.Controls.Add(lblInternalComments);

                #endregion
            }

            #endregion
        }

    }

    /// <summary>
    /// grid on detail table data bind
    /// </summary>
    /// <param name="source"></param>
    /// <param name="e"></param>
    protected void rgTimesheetTicketSummary_DetailTableDataBind(object source, Telerik.Web.UI.GridDetailTableDataBindEventArgs e)
    {
        GridDataItem dataItem = (GridDataItem)e.DetailTableView.ParentItem;
        switch (e.DetailTableView.Name)
        {
            #region ticket

            case "Ticket":
                {
                    //ticket id
                    int ticketId = Convert.ToInt32(dataItem.GetDataKeyValue("TicketId").ToString());

                    //get data table
                    DataTable dtEmployee = DesktopShared.Timesheet.Report.GetEmployeeAndHoursForTicket(SearchStartDate, SearchEndDate, SearchEmployeeId, ticketId);

                    //add ticket id as column (needed for employee below)
                    dtEmployee.Columns.Add("TicketID");
                    foreach (DataRow _row in dtEmployee.Rows)
                        _row["TicketId"] = ticketId;

                    //bind data table view
                    e.DetailTableView.DataSource = dtEmployee;

                    break;
                }

            #endregion

            #region employee

            case "Employee":
                {
                    //ticket id
                    int ticketId = Convert.ToInt32(dataItem.GetDataKeyValue("TicketId").ToString());

                    //employee id
                    int employeeId = Convert.ToInt32(dataItem.GetDataKeyValue("EmployeeId").ToString());

                    //bind data table view
                    DesktopShared.CollectionClasses.TimesheetCollection timesheets = DesktopShared.Timesheet.Report.GetTimeSheetEntriesForEmployee(employeeId, ticketId, SearchStartDate, SearchEndDate);
                    e.DetailTableView.DataSource = timesheets;

                    break;
                }

            #endregion

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
        RecordsPerPage = ucRecordsPerPage.SelectedValue;
        ResetGrid();
    }

    #endregion

    #region public methods

    /// <summary>
    /// reset/hide grid
    /// </summary>
    public void ResetGrid()
    {
        rgTimesheetTicketSummary.Visible = true;

        rgTimesheetTicketSummary.EditIndexes.Clear();
        rgTimesheetTicketSummary.DataSource = null;
        rgTimesheetTicketSummary.Rebind();
    }

    /// <summary>
    /// clear grid
    /// </summary>
    public void ClearGrid()
    {
        rgTimesheetTicketSummary.CurrentPageIndex = 0;
        rgTimesheetTicketSummary.EditIndexes.Clear();
        rgTimesheetTicketSummary.Visible = false;
    }

    #endregion

    #region private methods

    /// <summary>
    /// set up records per page -> set selected value in drop down list 
    /// </summary>
    private void SetUpRecordsPerPage()
    {
        ucRecordsPerPage.SelectedValue = RecordsPerPage;
    }

    #endregion

    #region private properties

    /// <summary>
    /// get/set records per page (stored in session)
    /// </summary>
    private int RecordsPerPage
    {
        get
        {
            object obj = this.Session["RecordsPerPageTimeSheetSummary"];
            if (obj == null)
                return 25;
            else
                return (int)obj;
        }

        set { this.Session["RecordsPerPageTimeSheetSummary"] = value; }
    }

    #endregion

    #region public properties

    /// <summary>
    /// get/set search client id (stored in viewstate)
    /// </summary>
    public int? SearchClientId
    {
        get
        {
            object obj = this.ViewState["SearchClientIdForTimesheetTicketSummary"];
            if (obj == null)
                return null;
            else
                return (int)obj;
        }
        set { this.ViewState["SearchClientIdForTimesheetTicketSummary"] = value; }
    }

    /// <summary>
    /// get/set search employee id (stored in viewstate)
    /// </summary>
    public int? SearchEmployeeId
    {
        get
        {
            object obj = this.ViewState["SearchEmployeeIdForTimesheetTicketSummary"];
            if (obj == null)
                return null;
            else
                return (int)obj;
        }
        set { this.ViewState["SearchEmployeeIdForTimesheetTicketSummary"] = value; }
    }

    /// <summary>
    /// get/set search start date (stored in viewstate)
    /// </summary>
    public DateTime? SearchStartDate
    {
        get
        {
            object obj = this.ViewState["SearchStartDateForTimesheetTicketSummary"];
            if (obj == null)
                return null;
            else
                return (DateTime)obj;
        }
        set { this.ViewState["SearchStartDateForTimesheetTicketSummary"] = value; }
    }

    /// <summary>
    /// get/set search end date (stored in viewstate)
    /// </summary>
    public DateTime? SearchEndDate
    {
        get
        {
            object obj = this.ViewState["SearchEndDateForTimesheetTicketSummary"];
            if (obj == null)
                return null;
            else
                return (DateTime)obj;
        }
        set { this.ViewState["SearchEndDateForTimesheetTicketSummary"] = value; }
    }

    #endregion
}