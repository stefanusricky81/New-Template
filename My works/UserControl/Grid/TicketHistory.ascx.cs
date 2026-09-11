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

public partial class UserControl_Grid_TicketHistory : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    #region protected events

    #region telerik grid

    /// <summary>
    /// grid on need data source
    /// </summary>
    /// <param name="source"></param>
    /// <param name="e"></param>
    protected void rgTicketHistory_NeedDataSource(object source, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        if (TicketId > 0)
        {
            rgTicketHistory.Visible = true;

            #region get TicketHistory typed list

            //typed list
            DesktopShared.TypedListClasses.TicketHistoryTypedList tlTicketHistory = new DesktopShared.TypedListClasses.TicketHistoryTypedList();

            //predicate expression
            IPredicateExpression ticketHistoryFilter = new PredicateExpression();
            ticketHistoryFilter.Add(DesktopShared.HelperClasses.CscHistoryFields.CscdefectsId == TicketId);
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
            GridEditableItem item = e.Item as GridEditableItem;
            DataRowView drv = (DataRowView)e.Item.DataItem;

            
            #region notes

            TableCell tcNotes = item["Notes"];
            tcNotes.Text = drv["Notes"].ToString().Trim().Replace("\n", "<br />");

            if (BitByBit.Utility.DataBase.ConvertHelper.ConvertToInteger(drv["TicketHistoryTypeId"], -1) == DesktopShared.Ticket.History.Type.Id.EmailResent)
                tcNotes.Text += "<br />[Email Resubmitted]";
            
            #endregion

            #region updated by

            TableCell tcUpdatedBy = item["UpdatedBy"];
            tcUpdatedBy.Text = drv["UserFirst"].ToString().Trim() + " " + drv["UserLast"].ToString().Trim();

            #endregion

            #region EmployeeEmailed

            TableCell tcEmployeeEmailed = item["EmployeeEmailed"];

            string employeeEmailedValue = drv["InternalRecipients"].ToString().Trim();
            employeeEmailedValue = employeeEmailedValue.Replace(",", "<br />").Replace(";", "<br />");

            tcEmployeeEmailed.Text = employeeEmailedValue;

            #endregion

            #region ClientEmailed

            TableCell tcClientEmailed = item["ClientEmailed"];

            string clientEmailedValue = drv["ExternalRecipients"].ToString().Trim();
            clientEmailedValue = clientEmailedValue.Replace(",", "<br />").Replace(";", "<br />");

            tcClientEmailed.Text = clientEmailedValue;

            #endregion

            #region Viewable By

            TableCell tcViewableBy = item["ViewableBy"];

            string viewableByValue = "Client";
            if (drv["InternalUsage"].ToString().Trim().ToUpper() == "Y")
                viewableByValue = "Internal Only";

            tcViewableBy.Text = viewableByValue;

            #endregion

            #region email failure

            TableCell tcEmailFailure = item["EmailFailure"];

            string _emailFailure = "";
            if (BitByBit.Utility.DataBase.ConvertHelper.ConvertToBoolean(drv["EmailFailure"], false))
                _emailFailure = "Y";

            tcEmailFailure.Text = _emailFailure;

            #endregion


        }
    }

    #endregion

    #endregion

    #region public methods

    /// <summary>
    /// rebind grid
    /// </summary>
    public void RebindGrid()
    {
        rgTicketHistory.DataSource = null;
        rgTicketHistory.Rebind();
    }

    #endregion

    #region public properties

    /// <summary>
    /// get/set ticket id
    /// on set -> bind grid
    /// </summary>
    public int TicketId
    {
        get
        {
            object obj = this.ViewState["TicketId"];
            if (obj == null)
                return -1;
            else
                return (int)obj;
        }

        set
        {
            this.ViewState["TicketId"] = value;
            RebindGrid();
        }
    }

    /// <summary>
    /// get/set exclude alerts from history
    /// </summary>
    public bool ExcludeAlerts
    {
        get
        {
            object obj = this.ViewState["ExcludeAlertsForTicketHistory"];
            if (obj == null)
                return false;
            else
                return (bool)obj;
        }
        set { this.ViewState["ExcludeAlertsForTicketHistory"] = value; }
    }

    /// <summary>
    /// get/set exclude views from history
    /// </summary>
    public bool ExcludeViews
    {
        get
        {
            object obj = this.ViewState["ExcludeViewsForTicketHistory"];
            if (obj == null)
                return false;
            else
                return (bool)obj;
        }
        set { this.ViewState["ExcludeViewsForTicketHistory"] = value; }
    }

    #endregion
}
