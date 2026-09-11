using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class Reports_ClientTicketReport : System.Web.UI.Page
{
    int _critical;
    int _high;
    int _normal;
    int _low;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ucCreatedStart.SelectedDate = DateTime.Now.AddMonths(-1);
            ucCreatedEnd.SelectedDate = DateTime.Now;
        }
    }

    #region protected
    protected void cvCreatedDates_ServerValidate(object source, ServerValidateEventArgs args)
    {
        args.IsValid = true;
        if (!ucCreatedStart.SelectedDate.HasValue || !ucCreatedEnd.SelectedDate.HasValue)
            return;
        args.IsValid = ucCreatedEnd.SelectedDate.Value.Date >= ucCreatedStart.SelectedDate.Value.Date;
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            BindGrid();
            rgClientTicketReport.DataBind();
            rgReceiveVia.DataBind();
            rgTicketCategory.DataBind();
            rgTicketStatus.DataBind();
            rgResponseTime.DataBind();
            rgVIP.DataBind();
        }
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        Response.Redirect("ClientTicketReport.aspx");
    }
    protected void rgClientTicketReport_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        BindGrid();
    }

    protected void lbexport_Click(object sender, EventArgs e)
    {
        rgClientTicketReport.ExportSettings.IgnorePaging = true;
        rgClientTicketReport.ExportSettings.ExportOnlyData = true;
        rgClientTicketReport.ExportSettings.HideStructureColumns = true;

        rgClientTicketReport.GridLines = GridLines.Both;
        rgClientTicketReport.BorderStyle = BorderStyle.Solid;

        rgClientTicketReport.ExportSettings.FileName = "Client Ticket Report :" + ucCreatedStart.SelectedDate.Value.ToString("MM/dd/yyyy") + "-" + ucCreatedEnd.SelectedDate.Value.ToString("MM/dd/yyyy") +"_"+ DateTime.Now.ToString("yyyyMMdd-hhmmss");

        rgClientTicketReport.MasterTableView.ExportToExcel();
    }

    protected void rgReceiveVia_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            LinkButton lbCritical = (LinkButton)e.Item.FindControl("lbCritical");
            LinkButton lbHigh = (LinkButton)e.Item.FindControl("lbHigh");
            LinkButton lbNormaal = (LinkButton)e.Item.FindControl("lbNormal");
            LinkButton lbLow = (LinkButton)e.Item.FindControl("lbLow");
            _critical += Convert.ToInt32(lbCritical.Text);
            _high += Convert.ToInt32(lbHigh.Text);
            _normal += Convert.ToInt32(lbNormaal.Text);
            _low += Convert.ToInt32(lbLow.Text);
        }
        

        if (e.Item is GridFooterItem)
        {
            GridFooterItem footeritem = e.Item as GridFooterItem;
            footeritem["RECEIVEDMETHOD"].Text = "Total help desk request";
            footeritem["Critical"].Text = _critical.ToString();
            footeritem["High"].Text = _high.ToString();
            footeritem["Normal"].Text = _normal.ToString();
            footeritem["Low"].Text = _low.ToString();
        }
    }

    protected void rgReceiveVia_ItemCommand(object sender, GridCommandEventArgs e)
    {
        string command = e.CommandArgument.ToString().Trim();
        string DataKey = string.Empty, _type=string.Empty;
        string _open = "76, 58";
        string closed = "77, 59";
        switch (command)
        {
            #region received method
            case "CriticalReceivedVia":
                GridDataItem itemcrv = (GridDataItem)e.Item;
                DataKey = itemcrv["Detail"].Text;
                Response.Redirect(string.Format("~/Reports/ClientTicketReportDetail.aspx?type={0}&clientid={1}&priority={2}&start={3}&end={4}&detail={5}", "RV", ddlClient.ClientId, "C", ucCreatedStart.SelectedDate.Value, ucCreatedEnd.SelectedDate.Value, DataKey));
                break;
            case "HighReceivedVia":
                GridDataItem itemhrv = (GridDataItem)e.Item;
                DataKey = itemhrv["Detail"].Text;
                Response.Redirect(string.Format("~/Reports/ClientTicketReportDetail.aspx?type={0}&clientid={1}&priority={2}&start={3}&end={4}&detail={5}", "RV", ddlClient.ClientId, "H", ucCreatedStart.SelectedDate.Value, ucCreatedEnd.SelectedDate.Value, DataKey));
                break;
            case "NormalReceivedVia":
                GridDataItem itemnrv = (GridDataItem)e.Item;
                DataKey = itemnrv["Detail"].Text;
                Response.Redirect(string.Format("~/Reports/ClientTicketReportDetail.aspx?type={0}&clientid={1}&priority={2}&start={3}&end={4}&detail={5}", "RV", ddlClient.ClientId, "N", ucCreatedStart.SelectedDate.Value, ucCreatedEnd.SelectedDate.Value, DataKey));
                break;
            case "LowReceivedVia":
                GridDataItem itemlrv = (GridDataItem)e.Item;
                DataKey = itemlrv["Detail"].Text;
                Response.Redirect(string.Format("~/Reports/ClientTicketReportDetail.aspx?type={0}&clientid={1}&priority={2}&start={3}&end={4}&detail={5}", "RV", ddlClient.ClientId, "L", ucCreatedStart.SelectedDate.Value, ucCreatedEnd.SelectedDate.Value, DataKey));
                break;
            #endregion

            #region Ticket Category
            case "CriticalTicketCategory":
                GridDataItem itemctg = (GridDataItem)e.Item;
                DataKey = itemctg["Detail"].Text;
                Response.Redirect(string.Format("~/Reports/ClientTicketReportDetail.aspx?type={0}&clientid={1}&priority={2}&start={3}&end={4}&detail={5}", "TG", ddlClient.ClientId, "C", ucCreatedStart.SelectedDate.Value, ucCreatedEnd.SelectedDate.Value, DataKey));
                break;
            case "HighTicketCategory":
                GridDataItem itemhtg = (GridDataItem)e.Item;
                DataKey = itemhtg["Detail"].Text;
                Response.Redirect(string.Format("~/Reports/ClientTicketReportDetail.aspx?type={0}&clientid={1}&priority={2}&start={3}&end={4}&detail={5}", "TG", ddlClient.ClientId, "H", ucCreatedStart.SelectedDate.Value, ucCreatedEnd.SelectedDate.Value, DataKey));
                break;
            case "NormalTicketCategory":
                GridDataItem itemntg = (GridDataItem)e.Item;
                DataKey = itemntg["Detail"].Text;
                Response.Redirect(string.Format("~/Reports/ClientTicketReportDetail.aspx?type={0}&clientid={1}&priority={2}&start={3}&end={4}&detail={5}", "TG", ddlClient.ClientId, "N", ucCreatedStart.SelectedDate.Value, ucCreatedEnd.SelectedDate.Value, DataKey));
                break;
            case "LowTicketCategory":
                GridDataItem itemltg = (GridDataItem)e.Item;
                DataKey = itemltg["Detail"].Text;
                Response.Redirect(string.Format("~/Reports/ClientTicketReportDetail.aspx?type={0}&clientid={1}&priority={2}&start={3}&end={4}&detail={5}", "TG", ddlClient.ClientId, "L", ucCreatedStart.SelectedDate.Value, ucCreatedEnd.SelectedDate.Value, DataKey));
                break;
            #endregion

            #region Ticket Status
            case "CriticalTicketStatus":
                GridDataItem itemcts = (GridDataItem)e.Item;
                DataKey = itemcts["TICKETSTATUS"].Text == "Open" ? _open : closed;
                Response.Redirect(string.Format("~/Reports/ClientTicketReportDetail.aspx?type={0}&clientid={1}&priority={2}&start={3}&end={4}&detail={5}", "TS", ddlClient.ClientId, "C", ucCreatedStart.SelectedDate.Value, ucCreatedEnd.SelectedDate.Value, DataKey));
                break;
            case "HighTicketStatus":
                GridDataItem itemhts = (GridDataItem)e.Item;
                DataKey = itemhts["TICKETSTATUS"].Text == "Open" ? _open : closed;
                Response.Redirect(string.Format("~/Reports/ClientTicketReportDetail.aspx?type={0}&clientid={1}&priority={2}&start={3}&end={4}&detail={5}", "TS", ddlClient.ClientId, "H", ucCreatedStart.SelectedDate.Value, ucCreatedEnd.SelectedDate.Value, DataKey));
                break;
            case "NormalTicketStatus":
                GridDataItem itemnts = (GridDataItem)e.Item;
                DataKey = itemnts["TICKETSTATUS"].Text == "Open" ? _open : closed;
                Response.Redirect(string.Format("~/Reports/ClientTicketReportDetail.aspx?type={0}&clientid={1}&priority={2}&start={3}&end={4}&detail={5}", "TS", ddlClient.ClientId, "N", ucCreatedStart.SelectedDate.Value, ucCreatedEnd.SelectedDate.Value, DataKey));
                break;
            case "LowTicketStatus":
                GridDataItem itemlts = (GridDataItem)e.Item;
                DataKey = itemlts["TICKETSTATUS"].Text == "Open" ? _open : closed;
                Response.Redirect(string.Format("~/Reports/ClientTicketReportDetail.aspx?type={0}&clientid={1}&priority={2}&start={3}&end={4}&detail={5}", "TS", ddlClient.ClientId, "L", ucCreatedStart.SelectedDate.Value, ucCreatedEnd.SelectedDate.Value, DataKey));
                break;
            #endregion

            #region Response Time
            case "CriticalResponseTime":
                GridDataItem itemcrt = (GridDataItem)e.Item;
                LinkButton lbCritical = (LinkButton)e.Item.FindControl("lbCritical");
                _type = itemcrt["AVGresponsetime"].Text;
                DataKey = lbCritical.Text;
                Response.Redirect(string.Format("~/Reports/ClientTicketReportDetail.aspx?type={0}&clientid={1}&priority={2}&start={3}&end={4}&detail={5}", _type, ddlClient.ClientId, "C", ucCreatedStart.SelectedDate.Value, ucCreatedEnd.SelectedDate.Value, DataKey));
                break;
            case "HighResponseTime":
                GridDataItem itemhrt = (GridDataItem)e.Item;
                LinkButton lbHigh = (LinkButton)e.Item.FindControl("lbHigh"); 
                _type = itemhrt["AVGresponsetime"].Text;
                DataKey = lbHigh.Text;
                Response.Redirect(string.Format("~/Reports/ClientTicketReportDetail.aspx?type={0}&clientid={1}&priority={2}&start={3}&end={4}&detail={5}", _type, ddlClient.ClientId, "H", ucCreatedStart.SelectedDate.Value, ucCreatedEnd.SelectedDate.Value, DataKey));
                break;
            case "NormalResponseTime":
                GridDataItem itemnrt = (GridDataItem)e.Item;
                LinkButton lbNormaal = (LinkButton)e.Item.FindControl("lbNormal"); 
                _type = itemnrt["AVGresponsetime"].Text;
                DataKey = lbNormaal.Text;
                Response.Redirect(string.Format("~/Reports/ClientTicketReportDetail.aspx?type={0}&clientid={1}&priority={2}&start={3}&end={4}&detail={5}", _type, ddlClient.ClientId, "N", ucCreatedStart.SelectedDate.Value, ucCreatedEnd.SelectedDate.Value, DataKey));
                break;
            case "LowResponseTime":
                GridDataItem itemlrt = (GridDataItem)e.Item;
                LinkButton lbLow = (LinkButton)e.Item.FindControl("lbLow");
                _type = itemlrt["AVGresponsetime"].Text;
                DataKey = lbLow.Text;
                Response.Redirect(string.Format("~/Reports/ClientTicketReportDetail.aspx?type={0}&clientid={1}&priority={2}&start={3}&end={4}&detail={5}", _type, ddlClient.ClientId, "L", ucCreatedStart.SelectedDate.Value, ucCreatedEnd.SelectedDate.Value, DataKey));
                break;
            #endregion

            #region VIP
            case "CriticalVIP":
                Response.Redirect(string.Format("~/Reports/ClientTicketReportDetail.aspx?type={0}&clientid={1}&priority={2}&start={3}&end={4}&detail={5}", "VIP", ddlClient.ClientId, "C", ucCreatedStart.SelectedDate.Value, ucCreatedEnd.SelectedDate.Value, "VIP"));
                break;
            case "HighVIP":
                Response.Redirect(string.Format("~/Reports/ClientTicketReportDetail.aspx?type={0}&clientid={1}&priority={2}&start={3}&end={4}&detail={5}", "VIP", ddlClient.ClientId, "H", ucCreatedStart.SelectedDate.Value, ucCreatedEnd.SelectedDate.Value, "VIP"));
                break;
            case "NormalVIP":
                Response.Redirect(string.Format("~/Reports/ClientTicketReportDetail.aspx?type={0}&clientid={1}&priority={2}&start={3}&end={4}&detail={5}", "VIP", ddlClient.ClientId, "N", ucCreatedStart.SelectedDate.Value, ucCreatedEnd.SelectedDate.Value, "VIP"));
                break;
            case "LowVIP":
                Response.Redirect(string.Format("~/Reports/ClientTicketReportDetail.aspx?type={0}&clientid={1}&priority={2}&start={3}&end={4}&detail={5}", "VIP", ddlClient.ClientId, "L", ucCreatedStart.SelectedDate.Value, ucCreatedEnd.SelectedDate.Value, "VIP"));
                break;
                #endregion
        }
    }

    protected void rgClientTicketReport_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            string _notes = string.Empty;
            string _newnotes = string.Empty;
            Telerik.Web.UI.GridDataItem item = e.Item as Telerik.Web.UI.GridDataItem;
            TableCell cell = item["AllUpdateNotes"];

            int _count = System.Text.RegularExpressions.Regex.Matches(item["AllUpdateNotes"].Text.Replace("||", "#note#"), "#note#").Count;
            _newnotes = item["AllUpdateNotes"].Text;

            for (int i = 0; i <= _count - 1; i++)
            {
                if (_newnotes.IndexOf("||") == 0)
                {
                    _newnotes = _newnotes.Substring(2, _newnotes.Length - 2);
                    if (_newnotes.IndexOf("||") > -1)
                        _notes = _newnotes.Substring(0, _newnotes.IndexOf("||"));
                    else
                        _notes = _newnotes;
                    if (i == 0)
                        cell.Text = "<b> Note " + (i + 1) + "</b> " + _notes;
                    else
                        cell.Text += "<br/><b>Note " + (i + 1) + "</b> " + _notes;

                    _newnotes = _newnotes.Substring(_notes.Length, _newnotes.Length - _notes.Length);
                }
            }
        }
    }
    #endregion

    #region Private method
    private void DisplayMessage(string message, Bootstrap.Alert.AlertType alertType, bool dismissable = true, string overrideHeader = "")
    {
        ScriptManager.RegisterClientScriptBlock(this, Page.GetType(), "ToTheTop", "ToTopOfPage();", true);
        litMessage.Visible = true;
        litMessage.Text = Bootstrap.Alert.GetDivText(alertType, message, dismissable, true, overrideHeader);
    }

    private void BindGrid()
    {
        phSearchResults.Visible = true;
        rgClientTicketReport.DataSource = GetData("AllData");
        rgReceiveVia.DataSource = GetData("ReceiveMethod");
        rgTicketCategory.DataSource = GetData("TicketCategory");
        rgTicketStatus.DataSource = GetData("TicketStatus");
        rgResponseTime.DataSource = GetData("ResponseTime");
        rgVIP.DataSource = GetData("VIP");
    }

    private DataTable GetData(string typedata)
    {
        string connectionString = System.Configuration.ConfigurationManager.AppSettings["ConnectionString.SQL Server (SqlClient)"];
        System.Data.SqlClient.SqlConnection connection = new System.Data.SqlClient.SqlConnection(connectionString);
        DataTable dt = new DataTable();
        string _tickettype = string.Empty;

        for (int i = 0; i <= ddlTicketType.SelectedValues.Count - 1; i++)
        {
            if (_tickettype == string.Empty)
                _tickettype = ddlTicketType.SelectedValues[i];
            else
                _tickettype += "," + ddlTicketType.SelectedValues[i];
        }
        try
        {
            using (connection)
            {
                connection.Open();
                System.Data.SqlClient.SqlDataAdapter adp = new System.Data.SqlClient.SqlDataAdapter("Proc_GETCLIENTTICKETREPORT_desktop", connection);
                adp.SelectCommand.CommandType = CommandType.StoredProcedure;
                adp.SelectCommand.Parameters.AddWithValue("@CLIENTID", ddlClient.ClientId);
                adp.SelectCommand.Parameters.AddWithValue("@type", typedata);
                adp.SelectCommand.Parameters.AddWithValue("@Tickettype", _tickettype);
                adp.SelectCommand.Parameters.AddWithValue("@startdate", ucCreatedStart.SelectedDate);
                adp.SelectCommand.Parameters.AddWithValue("@enddate", ucCreatedEnd.SelectedDate);

                dt.Reset();
                adp.Fill(dt);
            }
        }
        catch (Exception ex)
        {
            return null;
        }
        finally { connection.Close(); }

        return dt;
    }

    #endregion
}