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

public partial class UserControl_Grid_ClientEmailBroadcast : System.Web.UI.UserControl
{
    bool _isExport = false;

    protected void Page_Load(object sender, EventArgs e)
    {
        //records per page event handler
        DropDownList ddlRecordsPerPage = ucRecordsPerPage.GetDropDownList();
        ddlRecordsPerPage.SelectedIndexChanged += new EventHandler(ddlRecordsPerPage_SelectedChange);

        if (!this.IsPostBack)
        {
            //ajax manager, set conditional postback script to cancel ajax on grid export
            Telerik.Web.UI.RadAjaxManager.GetCurrent(this.Page).ClientEvents.OnRequestStart = "conditionalPostback";
            SetUpRecordsPerPage(); //set value for drop down list control for number of records to display
        }
    }

    #region telerik grid events

    protected void rgEmailBroadcast_NeedDataSource(object source, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        #region ContactClientTypedList method (disabled)

        //DesktopShared.TypedListClasses.ContactClientTypedList contacts =
        //    new DesktopShared.TypedListClasses.ContactClientTypedList();

        //IPredicateExpression contactsFilter = new PredicateExpression();

        ////filter by a list of clients (array list)
        //if (SearchClients != null)
        //{
        //    contactsFilter.Add(new FieldCompareRangePredicate(DesktopShared.HelperClasses.ClientContactFields.FkClient, false, SearchClients));
        //    //contactsFilter.Add(new FieldCompareRangePredicate(DesktopShared.HelperClasses.ClientProductsFields.ClientId, false, SearchClients));
        //}

        ////filter by a list of products (array list)
        ////if (SearchProducts != null)
        ////{
        ////    contactsFilter.Add(new FieldCompareRangePredicate(DesktopShared.HelperClasses.ClientProductsFields.ProductsId, true, SearchProducts));
        ////    //contactsFilter.Add(DesktopShared.HelperClasses.ClientProductsFields.ProductsId == SearchProducts[0]);
        ////}
        //if (SearchProductId > 0)
        //{
        //    contactsFilter.Add(DesktopShared.HelperClasses.ClientProductsFields.ProductsId == SearchProductId);
        //}

        //contactsFilter.Add(DesktopShared.HelperClasses.ClientContactFields.Email % "%@%");

        //if (SearchClients != null || SearchProductId > 0)
        //{            
        //    ISortExpression contactsSort = new SortExpression();
        //    contactsSort.Add(DesktopShared.HelperClasses.ClientFields.Company | SortOperator.Ascending);
        //    contactsSort.Add(DesktopShared.HelperClasses.ClientContactFields.Email | SortOperator.Ascending);

        //    contacts.Fill(0, contactsSort, false, contactsFilter);
            
        //    rgEmailBroadcast.DataSource = contacts;           
        //}

        #endregion

        DataTable contacts = new DataTable();

        // get a list of search client values from an arrylist and convert it into a string to pass in to stored procedure
        string SearchClientsSQL = "";
        if (SearchClients != null)
        {
            for(int i=0; i<SearchClients.Count; i++)
            {
                SearchClientsSQL += "," + SearchClients[i].ToString();
            }
            SearchClientsSQL = SearchClientsSQL.Substring(1);
        }

        if (SearchClients != null || SearchProductId > 0)
        {
            contacts = DesktopShared.StoredProcedureCallerClasses.RetrievalProcedures.ProcQueryEmailBroadcast(SearchClientsSQL, Convert.ToInt32(SearchProductId));            
        }

        rgEmailBroadcast.DataSource = contacts;

        #region paging

        if (TicketsPerPage > 0)
            rgEmailBroadcast.PageSize = TicketsPerPage;
        else
        {
            if (contacts.Rows.Count > 0)
                rgEmailBroadcast.PageSize = contacts.Rows.Count;
        }

        #endregion
        
    }

    /// <summary>
    /// grid on item command
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rgEmailBroadcast_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        bool doRebind = false;
        RadGrid grid = (sender as RadGrid);

        #region refresh grid click

        if (e.CommandName == "RefreshGrid")
        {
            doRebind = true;
        }

        #endregion

        #region do rebind

        if (doRebind)
        {
            //rebind sender grid
            RebindGrid(rgEmailBroadcast);

        }

        #endregion
    }

    /// <summary>
    /// grid on pre render
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rgEmailBroadcast_PreRender(object sender, EventArgs e)
    {
        #region default - show header, footer, pager.  set no records text

        rgEmailBroadcast.ShowHeader = true;
        rgEmailBroadcast.ShowFooter = true;

        rgEmailBroadcast.PagerStyle.AlwaysVisible = true;
        rgEmailBroadcast.PagerStyle.Visible = true;

        rgEmailBroadcast.MasterTableView.NoMasterRecordsText = "No records found.";

        #endregion

        #region command item template -> populate export to type drop down list

        if (rgEmailBroadcast.MasterTableView.GetItems(GridItemType.CommandItem).Length > 0)
        {
            UserControl_DropDownList_GridExportType ucGridExportType =
                rgEmailBroadcast.MasterTableView.GetItems(GridItemType.CommandItem)[0].FindControl("ucGridExportType")
                as UserControl_DropDownList_GridExportType;

            ucGridExportType.Populate();
        }

        #endregion

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
           rgEmailBroadcast.MasterTableView.GetItems(GridItemType.CommandItem)[0].FindControl("ucGridExportType")
           as UserControl_DropDownList_GridExportType;

        if (ucGridExportType.Mode == UserControl_DropDownList_GridExportType.ExportMode.Csv)
            rgEmailBroadcast.MasterTableView.ExportToCSV();
        else if (ucGridExportType.Mode == UserControl_DropDownList_GridExportType.ExportMode.Excel)
            rgEmailBroadcast.MasterTableView.ExportToExcel();
        else if (ucGridExportType.Mode == UserControl_DropDownList_GridExportType.ExportMode.Word)
            rgEmailBroadcast.MasterTableView.ExportToWord();
        else if (ucGridExportType.Mode == UserControl_DropDownList_GridExportType.ExportMode.Pdf)
            rgEmailBroadcast.MasterTableView.ExportToPdf();
    }

    /// <summary>
    /// records per page drop list on selected index changed -> set number of records to display per grid page and rebind grid
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlRecordsPerPage_SelectedChange(object sender, EventArgs e)
    {
        TicketsPerPage = ucRecordsPerPage.SelectedValue;
        rgEmailBroadcast.Rebind();
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
        rgEmailBroadcast.ExportSettings.IgnorePaging = true;
        rgEmailBroadcast.ExportSettings.OpenInNewWindow = true;
        rgEmailBroadcast.ExportSettings.ExportOnlyData = true;
        rgEmailBroadcast.ExportSettings.HideStructureColumns = true;

        rgEmailBroadcast.ExportSettings.FileName = "EmailBroadcast_" + DateTime.Now.ToString("yyyyMMdd-hhmmss");

        //hide command item template
        foreach (GridItem commandItem in this.rgEmailBroadcast.MasterTableView.GetItems(GridItemType.CommandItem))
        {
            commandItem.Visible = false;
            commandItem.Display = false;
        }

        //hide link columns
        //rgClientBackupReport.MasterTableView.Columns.FindByUniqueName("EditCommandColumn").Visible = false;
        //rgClientBackupReport.MasterTableView.Columns.FindByUniqueName("Tag").Visible = false;              

        _isExport = true;
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
    
    #region private variables held in Session state

    public int? SearchClientId
    {
        get
        {
            object obj = this.ViewState["SearchClientIdForEmailBroadcast"];
            if (obj == null)
                return -1;
            else
                return (int)obj;
        }

        set
        {
            this.ViewState["SearchClientIdForEmailBroadcast"] = value;

            rgEmailBroadcast.DataSource = null;
            rgEmailBroadcast.Rebind();
        }
    }

    /// <summary>
    /// get/set search product id (stored in viewstate)
    /// </summary>
    public int? SearchProductId
    {
        get
        {
            object obj = this.ViewState["SearchProductIdForBackupReport"];
            if (obj == null)
                return -1;
            else
                return (int)obj;
        }
        set { this.ViewState["SearchProductIdForBackupReport"] = value; }
    }

    /// <summary>
    /// get/set tickets per page (stored in session)
    /// </summary>
    private int TicketsPerPage
    {
        get
        {
            object obj = this.Session["BroadcastPerPage"];
            if (obj == null)
                return 25;
            else
                return (int)obj;
        }

        set
        {
            this.Session["BroadcastPerPage"] = value;
        }
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
            object obj = this.ViewState["SearchClientsForEmailBroadcast"];
            if (obj == null)
                return null;
            else
                return (ArrayList)obj;
        }

        set
        {
            this.ViewState["SearchClientsForEmailBroadcast"] = value;
        }
    }

    /// <summary>
    /// get/set search products array list (stored in viewstate)
    /// </summary>
    public ArrayList SearchProducts
    {
        get
        {
            object obj = this.ViewState["SearchProductsForEmailBroadcast"];
            if (obj == null)
                return null;
            else
                return (ArrayList)obj;
        }

        set
        {
            this.ViewState["SearchProductsForEmailBroadcast"] = value;
        }
    }

    #region public properties

    /// <summary>
    /// grid result
    /// </summary>
    public RadGrid _rgEmailBroadcast
    {
        get
        {
            return rgEmailBroadcast;
        }
    }

    #endregion

    #endregion

    #region public methods

    /// <summary>
    /// reset grid
    /// </summary>
    public void ResetGrid()
    {
        rgEmailBroadcast.Visible = true;
        //pnlHeader.Visible = true;

        rgEmailBroadcast.EditIndexes.Clear();
        rgEmailBroadcast.DataSource = null;
        rgEmailBroadcast.Rebind();
    }

    /// <summary>
    /// clear grid
    /// </summary>
    public void ClearGrid()
    {
        rgEmailBroadcast.CurrentPageIndex = 0;
        rgEmailBroadcast.EditIndexes.Clear();
        rgEmailBroadcast.Visible = false;
        //pnlHeader.Visible = false;
    }

    #endregion
    


}