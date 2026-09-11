using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class Reports_TimeofDayTicketReport : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    #region protected
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        phSearchResults.Visible = true;
        BindGrid();
        rgTimeofDay.Rebind();
    }

    protected void cvCreatedDates_ServerValidate(object source, ServerValidateEventArgs args)
    {
        args.IsValid = true;
        if (!ucCreatedStart.SelectedDate.HasValue || !ucCreatedEnd.SelectedDate.HasValue)
            return;
        args.IsValid = ucCreatedEnd.SelectedDate.Value.Date >= ucCreatedStart.SelectedDate.Value.Date;
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        Response.Redirect("TimeofDayTicketReport.aspx");
    }

    protected void lbexport_Click(object sender, EventArgs e)
    {
        string _tickettypename = string.Empty;

        rgTimeofDay.ExportSettings.IgnorePaging = true;
        rgTimeofDay.ExportSettings.ExportOnlyData = true;
        rgTimeofDay.ExportSettings.OpenInNewWindow = true;
        rgTimeofDay.ExportSettings.HideStructureColumns = true;

        rgTimeofDay.GridLines = GridLines.Both;
        rgTimeofDay.BorderStyle = BorderStyle.Solid;

        _tickettypename = ddlTicketType.TicketTypeId == null ? "Helpdesk" : ddlTicketType.TicketTypeName;

        rgTimeofDay.ExportSettings.FileName = "Time of Day Ticket Report for Client" + DesktopShared.User.Portal.ClientName + "-" + _tickettypename;

        rgTimeofDay.MasterTableView.ExportToExcel();
    }

    protected void rgTimeofDay_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        //BindGrid();
    }
    #endregion

    #region private
    private void BindGrid()
    {
       rgTimeofDay.DataSource = GetTimeofDayTicket(ddlClient.ClientId,ddlTicketType.TicketTypeId, ucCreatedStart.SelectedDate,ucCreatedEnd.SelectedDate);
    }

    private DataTable GetTimeofDayTicket(int? _client, int? tickettypeid, DateTime? _startedate, DateTime? enddate)
    {
        DataTable dt = null;
        dt = DesktopShared.StoredProcedureCallerClasses.RetrievalProcedures.ProcGetTimeofDayTicketReport(_client, tickettypeid, _startedate, enddate);
        return dt;
    }
    #endregion
    double sum12AM = 0;
    double sum9AM = 0;
    double sum5PM = 0;
    protected void rgTimeofDay_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            GridDataItem dataItem = (GridDataItem)e.Item;
            sum12AM += double.Parse((dataItem["12AM"].FindControl("lbl12AM") as Label).Text);
            sum9AM += double.Parse((dataItem["9AM"].FindControl("lbl9AM") as Label).Text);
            sum5PM += double.Parse((dataItem["5PM"].FindControl("lbl5PM") as Label).Text);
        }
        else if (e.Item is GridFooterItem)
        {
            GridFooterItem footer = (GridFooterItem)e.Item;
            (footer["12AM"].FindControl("lblSum12AM") as Label).Text = sum12AM.ToString();
            (footer["9AM"].FindControl("lblSum9AM") as Label).Text = sum9AM.ToString();
            (footer["5PM"].FindControl("lblSum5PM") as Label).Text = sum5PM.ToString();
        }
    }
}