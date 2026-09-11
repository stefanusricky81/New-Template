using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web.Security;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using SD.LLBLGen.Pro.ORMSupportClasses;
using Telerik.Web.UI;


public partial class UserControl_Grid_ClientEmailBroadcastLog : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //records per page event handler
        DropDownList ddlRecordsPerPage = ucRecordsPerPage.GetDropDownList();
        ddlRecordsPerPage.SelectedIndexChanged += new EventHandler(ddlRecordsPerPage_SelectedChange);

        if (!this.IsPostBack)
        {            
            SetUpRecordsPerPage(); //set value for drop down list control for number of records to display
        }
    }

    #region protected methods / events

    #region telerik grid events

    /// <summary>
    /// grid on need data source
    /// </summary>
    /// <param name="source"></param>
    /// <param name="e"></param>
    protected void rgClientEmailBroadcastReport_NeedDataSource(object source, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {

            pnlHeader.Visible = true;
            rgClientEmailBroadcastReport.Visible = true;

            DataTable broadcasts = new DataTable();

            // get a list of search client values from an arrylist and convert it into a string to pass in to stored procedure
            string SearchClientsSQL = "";
            if (SearchClients != null)
            {
                for (int i = 0; i < SearchClients.Count; i++)
                {
                    SearchClientsSQL += "," + SearchClients[i].ToString();
                }
                SearchClientsSQL = SearchClientsSQL.Substring(1);
            }

            //if (SearchClients != null || SearchProductId > 0)
            //{
                broadcasts = DesktopShared.StoredProcedureCallerClasses.RetrievalProcedures.ProcQueryEmailBroadcastReport(SearchClientsSQL, Convert.ToInt32(SearchProductId), SearchStartDate.ToShortDateString(), SearchEndDate.ToShortDateString());
                
            //}

            rgClientEmailBroadcastReport.DataSource = broadcasts;
        

            #region get Collection
                
            //DesktopShared.TypedListClasses.EmailBroadcastsTypedList broadcasts = new DesktopShared.TypedListClasses.EmailBroadcastsTypedList();

            //IPredicateExpression broadcastsFilter = new PredicateExpression();

            ////filter by a list of clients (array list)
            ////if (SearchClients != null)
            ////{
            ////    broadcastsFilter.Add(new FieldCompareRangePredicate(DesktopShared.HelperClasses.ClientProductsFields.ClientId, false, SearchClients));
            ////}
                   
            ////product id
            //if (SearchProductId > 0)
            //{
            //    broadcastsFilter.Add(DesktopShared.HelperClasses.EmailBroadcastsFields.ProductId == SearchProductId);
            //}
 
            ////begin date
            //if (SearchStartDate != DateTime.MinValue)
            //{
            //    broadcastsFilter.Add(DesktopShared.HelperClasses.EmailBroadcastsFields.Created >=
            //        BitByBit.Utility.Date.StartOfDayDate(SearchStartDate));
            //}
        
            ////end date
            //if (SearchEndDate != DateTime.MinValue)
            //{
            //    broadcastsFilter.Add(DesktopShared.HelperClasses.EmailBroadcastsFields.Created <
            //        BitByBit.Utility.Date.EndOfDayDate(SearchEndDate));
            //}

            //ISortExpression broadcastsSort = new SortExpression();
            //broadcastsSort.Add(DesktopShared.HelperClasses.EmailBroadcastsFields.Created | SortOperator.Descending);

            //broadcasts.Fill(0, broadcastsSort, false, broadcastsFilter);

            //rgClientEmailBroadcastReport.DataSource = broadcasts;

            #endregion

            #region paging

            if (TicketsPerPage > 0)
                rgClientEmailBroadcastReport.PageSize = TicketsPerPage;
            else
            {
                if (broadcasts.Rows.Count > 0)
                    rgClientEmailBroadcastReport.PageSize = broadcasts.Rows.Count;
            }

            #endregion
    }

    protected void rgClientEmailBroadcastReport_DetailTableDataBind(object source, Telerik.Web.UI.GridDetailTableDataBindEventArgs e)
    {
        GridDataItem dataItem = (GridDataItem)e.DetailTableView.ParentItem;
        //switch (e.DetailTableView.Name)
        //{
        //    case "Recipients":
        //        {
                    string Id = dataItem.GetDataKeyValue("Id").ToString();

                    #region get Collection

                    DesktopShared.TypedListClasses.EmailBroadcastRecipientsTypedList recipients = new DesktopShared.TypedListClasses.EmailBroadcastRecipientsTypedList();

                    IPredicateExpression recipientsFilter = new PredicateExpression();

                    //filter by a list of clients (array list)
                    if (SearchClients != null)
                    {
                        recipientsFilter.Add(new FieldCompareRangePredicate(DesktopShared.HelperClasses.EmailBroadcastRecipientFields.ClientId, false, SearchClients));
                    }

                    recipientsFilter.Add(DesktopShared.HelperClasses.EmailBroadcastRecipientFields.EmailBroadcastId == Id);

                    ISortExpression recipientsSort = new SD.LLBLGen.Pro.ORMSupportClasses.SortExpression();
                    recipientsSort.Add(DesktopShared.HelperClasses.EmailBroadcastRecipientFields.ClientId | SortOperator.Ascending);
                    recipientsSort.Add(DesktopShared.HelperClasses.EmailBroadcastRecipientFields.Recipient | SortOperator.Ascending);

                    recipients.Fill(0, recipientsSort, false, recipientsFilter);                   

                    e.DetailTableView.DataSource = recipients;

                    #endregion

                    
        //            break;
        //        }
        //}
    }

    /// <summary>
    /// grid on pre render
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rgClientEmailBroadcastReport_PreRender(object sender, EventArgs e)
    {
        #region default - show header, footer, pager.  set no records text

        rgClientEmailBroadcastReport.ShowHeader = true;
        rgClientEmailBroadcastReport.ShowFooter = true;

        rgClientEmailBroadcastReport.PagerStyle.AlwaysVisible = true;
        rgClientEmailBroadcastReport.PagerStyle.Visible = true;

        rgClientEmailBroadcastReport.MasterTableView.NoMasterRecordsText = "No records found.";

        #endregion

        GridCommandItem commandItem = null; //add link button 

        if (rgClientEmailBroadcastReport.MasterTableView.Items.Count > 0)
        {
            commandItem = (GridCommandItem)rgClientEmailBroadcastReport.MasterTableView.GetItems(GridItemType.CommandItem)[0];
            commandItem.Visible = true;
        }

        #region command item template -> populate export to type drop down list

        if (rgClientEmailBroadcastReport.MasterTableView.GetItems(GridItemType.CommandItem).Length > 0)
        {
            UserControl_DropDownList_GridExportType ucGridExportType =
                rgClientEmailBroadcastReport.MasterTableView.GetItems(GridItemType.CommandItem)[0].FindControl("ucGridExportType")
                as UserControl_DropDownList_GridExportType;

            ucGridExportType.Populate();
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
        rgClientEmailBroadcastReport.Rebind();
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
           rgClientEmailBroadcastReport.MasterTableView.GetItems(GridItemType.CommandItem)[0].FindControl("ucGridExportType")
           as UserControl_DropDownList_GridExportType;

        if (ucGridExportType.Mode == UserControl_DropDownList_GridExportType.ExportMode.Csv)
            rgClientEmailBroadcastReport.MasterTableView.ExportToCSV();
        else if (ucGridExportType.Mode == UserControl_DropDownList_GridExportType.ExportMode.Excel)
            rgClientEmailBroadcastReport.MasterTableView.ExportToExcel();
        else if (ucGridExportType.Mode == UserControl_DropDownList_GridExportType.ExportMode.Word)
            rgClientEmailBroadcastReport.MasterTableView.ExportToWord();
        else if (ucGridExportType.Mode == UserControl_DropDownList_GridExportType.ExportMode.Pdf)
            rgClientEmailBroadcastReport.MasterTableView.ExportToPdf();
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
        rgClientEmailBroadcastReport.ExportSettings.IgnorePaging = true;
        rgClientEmailBroadcastReport.ExportSettings.OpenInNewWindow = true;
        rgClientEmailBroadcastReport.ExportSettings.ExportOnlyData = true;
        rgClientEmailBroadcastReport.ExportSettings.HideStructureColumns = true;

        rgClientEmailBroadcastReport.ExportSettings.FileName = "Backup_" + DateTime.Now.ToString("yyyyMMdd-hhmmss");

        //hide command item template
        foreach (GridItem commandItem in this.rgClientEmailBroadcastReport.MasterTableView.GetItems(GridItemType.CommandItem))
        {
            commandItem.Visible = false;
            commandItem.Display = false;
        }

        //hide link columns
        //rgClientBackupReport.MasterTableView.Columns.FindByUniqueName("EditCommandColumn").Visible = false;
        //rgClientBackupReport.MasterTableView.Columns.FindByUniqueName("Tag").Visible = false;              

        //_isExport = true;
    }

    #endregion

    #region public properties

    /// <summary>
    /// get/set search clients array list (stored in viewstate)
    /// </summary>
    public ArrayList SearchClients
    {
        get
        {
            object obj = this.ViewState["SearchClientsForEmailBroadcastReport"];
            if (obj == null)
                return null;
            else
                return (ArrayList)obj;
        }

        set
        {
            this.ViewState["SearchClientsForEmailBroadcastReport"] = value;
        }
    }

    /// <summary>
    /// get/set search product id (stored in viewstate)
    /// </summary>
    public int? SearchProductId
    {
        get
        {
            object obj = this.ViewState["SearchProductIdForEmailBroadcastReport"];
            if (obj == null)
                return null;
            else
                return (int)obj;
        }
        set { this.ViewState["SearchProductIdForEmailBroadcastReport"] = value; }
    }

    /// <summary>
    /// get/set search start date (stored in viewstate)
    /// </summary>
    public DateTime SearchStartDate
    {
        get
        {
            object obj = this.ViewState["SearchStartDateForEmailBroadcastReport"];
            if (obj == null)
                return DateTime.MinValue;
            else
                return (DateTime)obj;
        }
        set { this.ViewState["SearchStartDateForEmailBroadcastReport"] = value; }
    }

    /// <summary>
    /// get/set search end date (stored in viewstate)
    /// </summary>
    public DateTime SearchEndDate
    {
        get
        {
            object obj = this.ViewState["SearchEndDateForEmailBroadcastReport"];
            if (obj == null)
                return DateTime.MinValue;
            else
                return (DateTime)obj;
        }
        set { this.ViewState["SearchEndDateForEmailBroadcastReport"] = value; }
    }

    #endregion

    #region public methods

    /// <summary>
    /// reset grid
    /// </summary>
    public void ResetGrid()
    {
        rgClientEmailBroadcastReport.Visible = true;
        pnlHeader.Visible = true;

        rgClientEmailBroadcastReport.EditIndexes.Clear();
        rgClientEmailBroadcastReport.DataSource = null;
        rgClientEmailBroadcastReport.Rebind();
    }

    /// <summary>
    /// clear grid
    /// </summary>
    public void ClearGrid()
    {
        rgClientEmailBroadcastReport.CurrentPageIndex = 0;
        rgClientEmailBroadcastReport.EditIndexes.Clear();
        rgClientEmailBroadcastReport.Visible = false;
        pnlHeader.Visible = false;
    }

    #endregion
}