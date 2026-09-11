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
using SD.LLBLGen.Pro.ORMSupportClasses;

public partial class UserControl_Grid_TicketHistoryResponsive : System.Web.UI.UserControl
{
    /// <summary>
    /// page load
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        ConfigureForDevice();
    }

    #region private methods

    /// <summary>
    /// configure for device
    /// </summary>
    private void ConfigureForDevice()
    {
        if (DesktopShared.Utility.BrowserHelper.IsTabletOrSmartPhone(this.Request))
        {
            rgTicketHistory.RenderMode = RenderMode.Lightweight;
            phTabletPagerCss.Visible = true;
        }
    }

    #endregion

    #region protected events

    #region telerik grid

    /// <summary>
    /// grid on need data source
    /// </summary>
    /// <param name="source"></param>
    /// <param name="e"></param>
    protected void rgTicketHistory_NeedDataSource(object source, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        if (TicketId.HasValue)
        {
            rgTicketHistory.Visible = true;

            #region get TicketHistory typed list

            //typed list
            DesktopShared.TypedListClasses.TicketHistoryTypedList tlTicketHistory = new DesktopShared.TypedListClasses.TicketHistoryTypedList();

            //predicate expression
            IPredicateExpression ticketHistoryFilter = new PredicateExpression();
            ticketHistoryFilter.Add(DesktopShared.HelperClasses.CscHistoryFields.CscdefectsId == TicketId.Value);
            if (ExcludeAlerts)
            {
                ticketHistoryFilter.Add(DesktopShared.HelperClasses.CscHistoryFields.UpdatedBy != DesktopShared.User.TicketAlertIds);
                ticketHistoryFilter.Add(DesktopShared.HelperClasses.UsersFields.ServiceAccount == false);
            }
            if (ExcludeViews)
                ticketHistoryFilter.Add(DesktopShared.HelperClasses.TicketHistoryTypeFields.IsNonHuman == false);
            //sort expression
            ISortExpression ticketHistorySort = new SD.LLBLGen.Pro.ORMSupportClasses.SortExpression();
            ticketHistorySort.Add(DesktopShared.HelperClasses.CscHistoryFields.Created | SortOperator.Descending);
            ticketHistorySort.Add(DesktopShared.HelperClasses.CscHistoryFields.Id | SortOperator.Descending);

            //fetch
            tlTicketHistory.Fill(0, ticketHistorySort, false, ticketHistoryFilter);

            #endregion

            //bind grid
            rgTicketHistory.VirtualItemCount = tlTicketHistory.Rows.Count;
            rgTicketHistory.DataSource = tlTicketHistory;
          
        }
        else
            rgTicketHistory.Visible = false;
    }

    /// <summary>
    /// grid on item data bound
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rgTicketHistory_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        if (e.Item.ItemType == GridItemType.Item || e.Item.ItemType == GridItemType.AlternatingItem)
        {
            GridEditableItem _gdi= e.Item as GridEditableItem;
            DataRowView _row = (DataRowView)e.Item.DataItem;

            DateTime _tempDate = DateTime.MinValue;
            string _created = "";
            if (DateTime.TryParse(_row["Created"].ToString(), out _tempDate))
                _created = _tempDate.ToString("MM/dd/yy HH:mm");

            string _notes = _row["Notes"].ToString().Trim();
            if (BitByBit.Utility.DataBase.ConvertHelper.ConvertToInteger(_row["TicketHistoryTypeId"], -1) == DesktopShared.Ticket.History.Type.Id.EmailResent)
                _notes += "\n[Email Resubmitted]";

            string _internalRecpients = _row["InternalRecipients"].ToString().Trim().Replace(",", "\n").Replace(";", "\n");
            string _externalRecpients = _row["ExternalRecipients"].ToString().Trim().Replace(",", "\n").Replace(";", "\n");
            string _viewableBy = _row["InternalUsage"].ToString().Trim().ToUpper() == "Y" ? "Internal Only" : "Client";
            string _emailFailure = BitByBit.Utility.DataBase.ConvertHelper.ConvertToBoolean(_row["EmailFailure"], false) ? "Y" : "";
            string _attachments = _row["Attachments"].ToString().Trim().Replace("|", Environment.NewLine);

            DesktopShared.Utility.Grid.TelerikHelper.AddLabelToCell(_created, _gdi["Created"]);
            DesktopShared.Utility.Grid.TelerikHelper.AddLabelToCell(_notes.Replace("\n", "<br />"), _notes, _gdi["Notes"]);
            DesktopShared.Utility.Grid.TelerikHelper.AddLabelToCell(String.Format("{0} {1}", _row["UserFirst"].ToString().Trim(), _row["UserLast"].ToString().Trim()), _gdi["UpdatedBy"]);
            DesktopShared.Utility.Grid.TelerikHelper.AddLabelToCell(_row["StatusName"].ToString().Trim(), _gdi["StatusName"]);
            DesktopShared.Utility.Grid.TelerikHelper.AddLabelToCell(_internalRecpients.Replace("\n", "<br />"), _internalRecpients, _gdi["EmployeeEmailed"]);
            DesktopShared.Utility.Grid.TelerikHelper.AddLabelToCell(_externalRecpients.Replace("\n", "<br />"), _externalRecpients, _gdi["ClientEmailed"]);
            DesktopShared.Utility.Grid.TelerikHelper.AddLabelToCell(_viewableBy, _gdi["ViewableBy"]);
            DesktopShared.Utility.Grid.TelerikHelper.AddLabelToCell(_emailFailure, _gdi["EmailFailure"]);
            DesktopShared.Utility.Grid.TelerikHelper.AddLabelToCell(_attachments.Replace("\n", "<br />"), _attachments, _gdi["Attachments"]);
        }
    }

    /// <summary>
    /// grid on pre render
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rgTicketHistory_PreRender(object sender, EventArgs e)
    {
        if (DesktopShared.Utility.BrowserHelper.IsTabletOrSmartPhone(this.Request))
        {
            int _colIndex = -1;
            rgTicketHistory.MasterTableView.Columns[++_colIndex].HeaderStyle.Width = Unit.Pixel(125);
            rgTicketHistory.MasterTableView.Columns[++_colIndex].HeaderStyle.Width = Unit.Pixel(350);
            rgTicketHistory.MasterTableView.Columns[++_colIndex].HeaderStyle.Width = Unit.Pixel(150);
            rgTicketHistory.MasterTableView.Columns[++_colIndex].HeaderStyle.Width = Unit.Pixel(150);
            rgTicketHistory.MasterTableView.Columns[++_colIndex].HeaderStyle.Width = Unit.Pixel(250);
            rgTicketHistory.MasterTableView.Columns[++_colIndex].HeaderStyle.Width = Unit.Pixel(250);
            rgTicketHistory.MasterTableView.Columns[++_colIndex].HeaderStyle.Width = Unit.Pixel(150);
            rgTicketHistory.MasterTableView.Columns[++_colIndex].HeaderStyle.Width = Unit.Pixel(150);
            rgTicketHistory.MasterTableView.Columns[++_colIndex].HeaderStyle.Width = Unit.Pixel(150);
        }
    }

    /// <summary>
    /// grid on item command
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rgTicketHistory_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (evItemCommand != null)
            evItemCommand(this, e);
    }

    #endregion

    #endregion

    #region public methods

    /// <summary>
    /// rebind grid
    /// </summary>
    public void RebindGrid()
    {
        rgTicketHistory.Visible = true;
        rgTicketHistory.EditIndexes.Clear();
        rgTicketHistory.DataSource = null;
        rgTicketHistory.Rebind();
    }

    #endregion

    #region public properties

    /// <summary>
    /// get/set ticket id
    /// </summary>
    public int? TicketId
    {
        get
        {
            object obj = this.ViewState["tid_thg"];
            return (obj == null) ? (int?)null : (int)obj;
        }
        set { this.ViewState["tid_thg"] = value; }
    }

    /// <summary>
    /// get/set exclude alerts from history
    /// </summary>
    public bool ExcludeAlerts
    {
        get
        {
            object obj = this.ViewState["ea_thg"];
            return (obj == null) ? false : (bool)obj;
        }
        set { this.ViewState["ea_thg"] = value; }
    }

    /// <summary>
    /// get/set exclude views from history
    /// </summary>
    public bool ExcludeViews
    {
        get
        {
            object obj = this.ViewState["ev_thg"];
            return (obj == null) ? false : (bool)obj;
        }
        set { this.ViewState["ev_thg"] = value; }
    }

    #endregion

    #region public events

    /// <summary>
    /// add event handler for item command
    /// </summary>
    public event EventHandler evItemCommand;

    #endregion
}
