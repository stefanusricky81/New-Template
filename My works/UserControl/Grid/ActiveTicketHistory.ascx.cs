using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class UserControl_Grid_ActiveTicketHistory : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void rgaActiveTicketHistory_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        var _dtactiveticket = DesktopShared.StoredProcedureCallerClasses.RetrievalProcedures.ProcGetActiveTiketHistory(GetTicketNumber);
        rgaActiveTicketHistory.DataSource = _dtactiveticket;

    }
    public void RebindGrid()
    {
        rgaActiveTicketHistory.Visible = true;
        rgaActiveTicketHistory.EditIndexes.Clear();
        rgaActiveTicketHistory.DataSource = null;
        rgaActiveTicketHistory.Rebind();

    }

    public int? GetTicketNumber
    {
        get
        {
            object obj = this.ViewState["ath_ticketno"];
            return (obj == null) ? (int?)null : (int)obj;
        }
        set { this.ViewState["ath_ticketno"] = value; }
    }
}