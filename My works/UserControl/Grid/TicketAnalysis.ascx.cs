using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using SD.LLBLGen.Pro.ORMSupportClasses;
using System.Data;

public partial class UserControl_Grid_TicketAnalysis : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    #region protected events

    #region telerk grid

    /// <summary>
    /// grid on need data source
    /// </summary>
    /// <param name="source"></param>
    /// <param name="e"></param>
    protected void rgTicketAnalysis_NeedDataSource(object source, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        if ((SearchStartDate != DateTime.MinValue) && (SearchEndDate != DateTime.MinValue))
        {
            rgTicketAnalysis.Visible = true;
            List<DesktopShared.Ticket.Report.AnalysisList> _analysisList = DesktopShared.Ticket.Report.GetTicketStatsForMonthView(SearchStartDate, SearchEndDate, SearchClientId, SearchDesignation, SearchTicketType, SearchMspOnly);
            rgTicketAnalysis.DataSource = _analysisList;
        }
        else
            rgTicketAnalysis.Visible = false;
    }

    /// <summary>
    /// grid on item data bound
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rgTicketAnalysis_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        if (e.Item.ItemType == GridItemType.Item || e.Item.ItemType == GridItemType.AlternatingItem)
        {
            

        }

    }

    /// <summary>
    /// grid on detail table data bind
    /// </summary>
    /// <param name="source"></param>
    /// <param name="e"></param>
    protected void rgTicketAnalysis_DetailTableDataBind(object source, Telerik.Web.UI.GridDetailTableDataBindEventArgs e)
    {
        GridDataItem dataItem = (GridDataItem)e.DetailTableView.ParentItem;
        switch (e.DetailTableView.Name)
        {
            #region week

            case "Week":
                {
                    #region dates

                    DateTime _date = Convert.ToDateTime(dataItem.GetDataKeyValue("Date").ToString());

                    #region start date

                    DateTime _startDate = DesktopShared.Utility.Date.GetFirstDayOfMonth(_date);

                    //if ((_date.Month == SearchStartDate.Month) && (_date.Year == SearchStartDate.Year))
                        //_startDate = SearchStartDate; //use exact start date for frist month

                    #endregion

                    #region end date

                    DateTime _endDate = DesktopShared.Utility.Date.GetLastDayOfMonth(_date);

                    //if ((_date.Month == SearchEndDate.Month) && (_date.Year == SearchEndDate.Year)) 
                        //_endDate = SearchEndDate; //use exact end date for last month

                    #endregion

                    #endregion

                    //bind data table view
                    List<DesktopShared.Ticket.Report.AnalysisList> weekList = DesktopShared.Ticket.Report.GetTicketStatsForWeekView(_startDate, _endDate, SearchClientId, SearchDesignation, SearchTicketType, SearchMspOnly);
                    e.DetailTableView.DataSource = weekList;

                    break;
                }

            #endregion

            #region day

            case "Day":
                {
                    #region dates

                    //parse start & end dates from display value (i.e. 	"01/01/2012 - 01/07/2012")
                    string _dates = dataItem.GetDataKeyValue("DisplayValue").ToString().Replace(" ", "");
                    int _pos = _dates.IndexOf("-");
                    DateTime _startDate = Convert.ToDateTime(_dates.Substring(0, _pos));
                    DateTime _endDate = Convert.ToDateTime(_dates.Substring(_pos+1));

                    #endregion

                    //bind data table view
                    List<DesktopShared.Ticket.Report.AnalysisList> dayList = DesktopShared.Ticket.Report.GetTicketStatsForDayView(_startDate, _endDate, SearchClientId, SearchDesignation, SearchTicketType, SearchMspOnly);
                    e.DetailTableView.DataSource = dayList;

                    break;
                }

            #endregion

        }
    }

    #endregion

    #endregion

    #region public methods

    /// <summary>
    /// reset/hide grid
    /// </summary>
    public void ResetGrid()
    {
        rgTicketAnalysis.Visible = true;

        rgTicketAnalysis.EditIndexes.Clear();
        rgTicketAnalysis.DataSource = null;
        rgTicketAnalysis.Rebind();
    }

    /// <summary>
    /// clear grid
    /// </summary>
    public void ClearGrid()
    {
        rgTicketAnalysis.CurrentPageIndex = 0;
        rgTicketAnalysis.EditIndexes.Clear();
        rgTicketAnalysis.Visible = false;
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
            object obj = this.ViewState["SearchClientIdForTicketAnalysis"];
            if (obj == null)
                return null;
            else
                return (int)obj;
        }
        set { this.ViewState["SearchClientIdForTicketAnalysis"] = value; }
    }

    /// <summary>
    /// get/set search designation status
    /// </summary>
    public int? SearchDesignation
    {
        get
        {
            object obj = this.ViewState["SearchDesignationForTicketAnalysis"];
            if (obj == null)
                return null;
            else
                return (int)obj;
        }
        set { this.ViewState["SearchDesignationForTicketAnalysis"] = value; }
    }

    /// <summary>
    /// get/set search start date (stored in viewstate)
    /// </summary>
    public DateTime SearchStartDate
    {
        get
        {
            object obj = this.ViewState["SearchStartDateForTicketAnalysis"];
            if (obj == null)
                return DateTime.MinValue;
            else
                return (DateTime)obj;
        }
        set { this.ViewState["SearchStartDateForTicketAnalysis"] = value; }
    }

    /// <summary>
    /// get/set search end date (stored in viewstate)
    /// </summary>
    public DateTime SearchEndDate
    {
        get
        {
            object obj = this.ViewState["SearchEndDateForTicketAnalysis"];
            if (obj == null)
                return DateTime.MinValue;
            else
                return (DateTime)obj;
        }
        set { this.ViewState["SearchEndDateForTicketAnalysis"] = value; }
    }

    /// <summary>
    /// get/set search msp only
    /// </summary>
    public bool SearchMspOnly
    {
        get
        {
            object obj = this.ViewState["SearchMspForTicketAnalysis"];
            if (obj == null)
                return false;
            else
                return (bool)obj;
        }
        set { this.ViewState["SearchMspForTicketAnalysis"] = value; }
    }

    /// <summary>
    /// get/set search Ticket type status
    /// </summary>
    public int? SearchTicketType
    {
        get
        {
            object obj = this.ViewState["SearchTicketTypeForTicketAnalysis"];
            if (obj == null)
                return null;
            else
                return (int)obj;
        }
        set { this.ViewState["SearchTicketTypeForTicketAnalysis"] = value; }
    }

    #endregion


}