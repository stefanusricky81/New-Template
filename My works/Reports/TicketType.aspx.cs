using DesktopShared.EntityClasses;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class Reports_TicketType : BasePage
{
    //note -> need to manually keep running totals as but with applying aggregate function for auto-generated columns during column created event

    /// <summary>
    /// page load
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
            SetUpPage();
    }

    #region private methods

    /// <summary>
    /// get data table for grid
    /// </summary>
    /// <param name="startDate"></param>
    /// <param name="endDate"></param>
    /// <param name="gridType"></param>
    /// <param name="ticketTypes"></param>
    /// <returns></returns>
    private DataTable GetDataTable(DateTime startDate, DateTime endDate, GridType gridType, DesktopShared.CollectionClasses.TicketTypeCollection ticketTypes = null)
    {
        //create data table
        DataTable _dt = new DataTable();

        //get ticket types
        if (ticketTypes == null)
            ticketTypes = DesktopShared.Ticket.TypeHelper.Get(true);

        //add columns
        _dt.Columns.Add("Name", typeof(System.String));
        foreach (var _type in ticketTypes)
            _dt.Columns.Add(_type.Name.Trim(), typeof(System.Int32));

        #region monthly

        if (gridType == GridType.Monthly) 
        {
            //get number of months
            int _months = endDate.Month - startDate.Month + (endDate.Year - startDate.Year) * 12;
            for (int i = 0; i <= _months; i++)
            {
                #region dates

                //date
                DateTime date = startDate.AddMonths(i);

                //start date
                DateTime _start = date;
                if ((startDate.Month == _start.Month) && ((startDate.Year == _start.Year)))//use exact start day
                    _start = startDate;
                else
                    _start = new DateTime(_start.Year, _start.Month, 1); //use 1st day of month

                //end date
                DateTime _end = date;
                if ((_end.Month == endDate.Month) && ((_end.Year == endDate.Year)))
                    _end = endDate; //use exact end day
                else
                    _end = _end.AddMonths(1).AddDays(-1); //use last day of month

                #endregion

                //create row
                DataRow _row = _dt.NewRow();

                //date column
                _row[0] = String.Format("{0} - {1}", _start.ToString("MM/dd/yy"), _end.ToString("MM/dd/yy"));

                //ticket types columns
                foreach (var _type in ticketTypes)
                    _row[_type.Name.Trim()] = DesktopShared.Ticket.TypeHelper.GetCount(_type.Id, _start, _end, ddlClient.ClientId);

                //add row
                _dt.Rows.Add(_row);

            }   
        }

        #endregion

        #region weekly

        else if (gridType == GridType.Weekly)
        {
            //get number of weeks
            int _weeks = (int)Math.Ceiling((decimal)(endDate - startDate).Days / 7);

            for (int i = 0; i < _weeks; i++)
            {
                #region dates

                int _day = 1;
                if (i > 0)
                    _day = (i * 7) + 1;

                //date
                DateTime date = new DateTime(startDate.Year, startDate.Month, _day);

                //start date
                DateTime _start = new DateTime(startDate.Year, startDate.Month, _day);

                //if start of week is greater than end date range -> break out of loop
                if (_start > endDate)
                    break;

                //end date
                DateTime _end = _start.AddDays(6);
                if (_end > endDate)
                    _end = endDate; //use exact end day

                if (_end.Month != _start.Month)
                    _end = new DateTime(_start.Year, _start.Month, 1).AddMonths(1).AddDays(-1);

                #endregion

                //create row
                DataRow _row = _dt.NewRow();

                //date column
                _row[0] = String.Format("{0} - {1}", _start.ToString("MM/dd/yy"), _end.ToString("MM/dd/yy"));

                //ticket types columns
                foreach (var _type in ticketTypes)
                    _row[_type.Name.Trim()] = DesktopShared.Ticket.TypeHelper.GetCount(_type.Id, _start, _end, ddlClient.ClientId);

                //add row
                _dt.Rows.Add(_row);

            }

        }

        #endregion

        #region weekly

        else if (gridType == GridType.Daily)
        {
            //get number of days
            int _days = (endDate - startDate).Days;

            for (int i = 0; i <= _days; i++)
            {
                //date
                DateTime _date = startDate.AddDays(i);

                //create row
                DataRow _row = _dt.NewRow();

                //date column
                _row[0] = String.Format("{0}", _date.ToString("MM/dd/yy"));

                //ticket types columns
                foreach (var _type in ticketTypes)
                    _row[_type.Name.Trim()] = DesktopShared.Ticket.TypeHelper.GetCount(_type.Id, _date, _date, ddlClient.ClientId);

                //add row
                _dt.Rows.Add(_row);

            }
        }

        #endregion

        //reurn
        return _dt;
    }

    /// <summary>
    /// configure for page load
    /// </summary>
    private void SetUpPage()
    {
        BaseMaster _baseMaster = this.Master as BaseMaster;
        _baseMaster.PageName = "Ticket Type Analysis";

        dtEndDate.SelectedDate = DateTime.Now;
        dtStartDate.SelectedDate = DesktopShared.Utility.Date.GetFirstDayOfMonth(dtEndDate.SelectedDate.Value);

        short _tabIndex = 0;

        dtStartDate.TabIndex = ++_tabIndex;
        dtEndDate.TabIndex = ++_tabIndex;
        ddlClient.TabIndex = ++_tabIndex;
        btnSearch.TabIndex = ++_tabIndex;
        btnClear.TabIndex = ++_tabIndex;
    }

    /// <summary>
    /// rebind grid
    /// </summary>
    private void RebindGrid()
    {
        phGrid.Visible = true;
        rgTicketType.EditIndexes.Clear();
        rgTicketType.DataSource = null;
        rgTicketType.Rebind();
    }

    #endregion

    #region protected events 

    /// <summary>
    /// /export button on click
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnExport_Click(object sender, EventArgs e)
    {
        rgTicketType.MasterTableView.ExportToCSV();
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

    /// <summary>
    /// search buton on click
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        if (this.IsValid)
        {
            litDebug.Text = "";
            RebindGrid();
        }
    }

    #region telerik grid

    /// <summary>
    /// /grid on need data source
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rgTicketType_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        //get ticket types
        var _types = DesktopShared.Ticket.TypeHelper.Get(true);
        if (TicketTypeCount == 0)
            TicketTypeCount = _types.Count;

        //counter for rrunning totals
        TicketTypeCounterList = new List<TicketTypeCounter>();
        int _count = 0;
        foreach (var _type in _types)
        {
            _count++;
            TicketTypeCounterList.Add(new TicketTypeCounter { Index = _count, Count = 0 });

        }

        //bind data source
        rgTicketType.DataSource = GetDataTable(dtStartDate.SelectedDate.Value, dtEndDate.SelectedDate.Value, GridType.Monthly, _types);
    }

    /// <summary>
    /// grid on item data bound
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rgTicketType_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        #region item / alternating item

        if (e.Item.ItemType == GridItemType.Item || e.Item.ItemType == GridItemType.AlternatingItem)
        {
            GridDataItem _gdi = e.Item as GridDataItem;
            DataRowView _row = (DataRowView)e.Item.DataItem;

            TelerikHelper.AddLabelToCell(_row[0].ToString(), _gdi["Name"]);

            //update running totals
            for (int i = 0; i <= TicketTypeCount; i++)
            {
                var _ttc = TicketTypeCounterList.FirstOrDefault(x => x.Index == i);
                if (_ttc != null)
                    _ttc.Count += Convert.ToInt32(_row[i].ToString());
            }
        }

        #endregion

        #region footer

        else if (e.Item is GridFooterItem)
        {
            GridFooterItem _gfi = e.Item as GridFooterItem;
            _gfi["Name"].Text = "Totals";

            for (int i = 0; i <= TicketTypeCount; i++)
            {
                var _ttc = TicketTypeCounterList.FirstOrDefault(x => x.Index == i);
                if (_ttc != null)
                    _gfi.Cells[i + 2].Text = _ttc.Count.ToString(); //skip expand/colllapse column and skip name column
            }
        }

        #endregion
    }

    /// <summary>
    /// grid on column created
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rgTicketType_ColumnCreated(object sender, GridColumnCreatedEventArgs e)
    {
        /*if (e.Column is GridBoundColumn)
        {
            GridBoundColumn _gbc = e.Column as GridBoundColumn;
            if (_gbc.DataField != "Name")    
            {
                _gbc.Aggregate = GridAggregateFunction.Sum;//known bug with second expanding row
            }
        }*/
        if (e.Column is GridExpandColumn)
        {
            (e.Column as GridExpandColumn).ButtonType = GridExpandColumnType.LinkButton;
        }
    }

    /// <summary>
    /// grid on exporting
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rgTicketType_GridExporting(object sender, GridExportingArgs e)
    {
        if (e.ExportType == ExportType.Csv)
        {
            string footer = "";
            GridFooterItem footerItem = rgTicketType.MasterTableView.GetItems(GridItemType.Footer)[0] as GridFooterItem;
            foreach (GridColumn column in rgTicketType.MasterTableView.RenderColumns)
            {
                if (column.UniqueName == "Name")
                    footer += "Totals,";
                else if (column.Visible)
                    footer += String.Format("\"{0}\",", footerItem[column.UniqueName].Text.Replace(" ", ""));
            }
            e.ExportOutput += footer.TrimEnd(',');
        }
    }

    /// <summary>
    /// grid on detail table bind
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rgTicketType_DetailTableDataBind(object sender, GridDetailTableDataBindEventArgs e)
    {
        GridDataItem dataItem = (GridDataItem)e.DetailTableView.ParentItem;

        //parse start & end dates from display value (i.e. 	"01/01/2012 - 01/07/2012")
        string _dates = dataItem.GetDataKeyValue("Name").ToString().Replace(" ", "");
        int _pos = _dates.IndexOf("-");
        DateTime _startDate = Convert.ToDateTime(_dates.Substring(0, _pos));
        DateTime _endDate = Convert.ToDateTime(_dates.Substring(_pos + 1));

        switch (e.DetailTableView.Name)
        {
            case "Week":
                {
                    e.DetailTableView.DataSource = GetDataTable(_startDate, _endDate, GridType.Weekly);
                    break;
                }

            case "Day":
                {
                    e.DetailTableView.DataSource = GetDataTable(_startDate, _endDate, GridType.Daily);
                    break;
                }
        }
    }

    #endregion

    #region custom validators

    /// <summary>
    /// dates validations
    /// </summary>
    /// <param name="source"></param>
    /// <param name="args"></param>
    protected void cvDates_ServerValidate(object source, ServerValidateEventArgs args)
    {
        if (dtEndDate.SelectedDate.Value.Date < dtStartDate.SelectedDate.Value.Date)
        {
            args.IsValid = false;
            cvDates.ErrorMessage = "End Date must be greater than Start Date";
            return;
        }
        TimeSpan _ts = dtEndDate.SelectedDate.Value.Subtract(dtStartDate.SelectedDate.Value);
        if (_ts.Days > 365)
        {
            args.IsValid = false;
            cvDates.ErrorMessage = "Maximum Date Range is 1 year";
            return;
        }
        args.IsValid = true;
    }

    #endregion

    #endregion

    #region private properties

    /// <summary>
    /// get/set ticket type count
    /// </summary>
    private int TicketTypeCount
    {
        get
        {
            object obj = this.ViewState["tt_ttc"];
            return (obj == null) ? 0 : (int)obj;
        }
        set { this.ViewState["tt_ttc"] = value; }
    }

    /// <summary>
    /// get/set ticket type counter list
    /// </summary>
    private List<TicketTypeCounter> TicketTypeCounterList
    {
        get
        {
            object obj = this.ViewState["tt_ttcl"];
            return (obj == null) ? new List<TicketTypeCounter>() : (List<TicketTypeCounter>)obj;
        }
        set { this.ViewState["tt_ttcl"] = value; }
    }

    #endregion

    #region enum / classes

    /// <summary>
    /// enum for grid type
    /// </summary>
    private enum GridType
    {
        Monthly,
        Weekly,
        Daily
    }

    /// <summary>
    /// ticket type counter custom class
    /// </summary>
    [Serializable]
    class TicketTypeCounter
    {
        public int Index { get; set; }
        public int Count { get; set; }
    }

    #endregion


    
}