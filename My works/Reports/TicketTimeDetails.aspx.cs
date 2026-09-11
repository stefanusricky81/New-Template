using System;
using System.Data;
using System.Web.UI.WebControls;

public partial class Reports_TicketTimeDetails : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        int _id;
        DateTime _startdate, _enddate;

        if (!String.IsNullOrEmpty(Request.QueryString["type"]))
            Type = Request.QueryString["type"].ToString().Trim();
        if (!String.IsNullOrEmpty(Request.QueryString["priority"]))
            Priority = Request.QueryString["priority"].ToString().Trim();
        if (!String.IsNullOrEmpty(Request.QueryString["detail"]))
            Detail = Request.QueryString["detail"].ToString().Trim();
        if (Int32.TryParse(Request.QueryString["clientid"], out _id))
            ClientId = _id;
        if (DateTime.TryParse(Request.QueryString["start"], out _startdate))
            StartDate = _startdate;
        if (DateTime.TryParse(Request.QueryString["end"], out _enddate))
            EndDate = _enddate;
    }

    protected void rgClientTicketReportDetail_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        BindGrid();
    }

    protected void rgClientTicketReportDetail_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        if (e.Item is Telerik.Web.UI.GridDataItem)
        {
            string _notes = string.Empty;
            string _newnotes = string.Empty;
            Telerik.Web.UI.GridDataItem item = e.Item as Telerik.Web.UI.GridDataItem;
            TableCell cell = item["AllUpdateNotes"];

            int _count = System.Text.RegularExpressions.Regex.Matches(item["AllUpdateNotes"].Text.Replace("||", "#note#"), "#note#").Count;
            _newnotes = Server.HtmlDecode(item["AllUpdateNotes"].Text).Replace("\r", "").Replace("\n", "<br/>");

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

    protected void lbexport_Click(object sender, EventArgs e)
    {
        rgClientTicketReportDetail.ExportSettings.IgnorePaging = true;
        rgClientTicketReportDetail.ExportSettings.ExportOnlyData = true;
        rgClientTicketReportDetail.ExportSettings.HideStructureColumns = true;

        rgClientTicketReportDetail.GridLines = GridLines.Both;
        rgClientTicketReportDetail.BorderStyle = BorderStyle.Solid;

        rgClientTicketReportDetail.ExportSettings.FileName = "Ticket Time Details :" + StartDate.Value.ToString("MM/dd/yyyy") + "-" + EndDate.Value.ToString("MM/dd/yyyy") + "_" + DateTime.Now.ToString("yyyyMMdd-hhmmss");

        rgClientTicketReportDetail.MasterTableView.ExportToExcel();
    }

    private void BindGrid()
    {
        rgClientTicketReportDetail.DataSource = GetData();
    }

    private DataTable GetData()
    {
        string connectionString = System.Configuration.ConfigurationManager.AppSettings["ConnectionString.SQL Server (SqlClient)"];
        System.Data.SqlClient.SqlConnection connection = new System.Data.SqlClient.SqlConnection(connectionString);
        DataTable dt = new DataTable();

        try
        {
            using (connection)
            {
                connection.Open();
                System.Data.SqlClient.SqlDataAdapter adp = new System.Data.SqlClient.SqlDataAdapter("Proc_GETCLIENTTICKETREPORTDetail", connection);
                adp.SelectCommand.CommandType = CommandType.StoredProcedure;
                adp.SelectCommand.Parameters.AddWithValue("@TYPE", Type);
                adp.SelectCommand.Parameters.AddWithValue("@CLIENTID", ClientId);
                adp.SelectCommand.Parameters.AddWithValue("@priority", Priority);
                adp.SelectCommand.Parameters.AddWithValue("@STARTDATE", StartDate);
                adp.SelectCommand.Parameters.AddWithValue("@ENDDATE", EndDate);
                adp.SelectCommand.Parameters.AddWithValue("@detail", Detail);

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

    #region public variabel
    public int ClientId
    {
        get
        {
            if (ViewState["ClientId"] == null)
                return 0;

            return Convert.ToInt32(ViewState["ClientId"]);
        }
        set
        {
            ViewState["ClientId"] = value;
        }
    }

    public DateTime? StartDate
    {
        get
        {
            object obj = this.ViewState["start_date"];
            return (obj == null) ? (DateTime?)null : (DateTime)obj;

        }
        set { this.ViewState["start_date"] = value; }
    }

    public DateTime? EndDate
    {
        get
        {
            object obj = this.ViewState["end_date"];
            return (obj == null) ? (DateTime?)null : (DateTime)obj;

        }
        set { this.ViewState["end_date"] = value; }
    }

    public string Type
    {
        get
        {
            object obj = this.ViewState["type"];
            return (obj == null) ? "" : (string)obj;

        }
        set { this.ViewState["type"] = value; }
    }

    public string Priority
    {
        get
        {
            object obj = this.ViewState["_priority"];
            return (obj == null) ? "" : (string)obj;

        }
        set { this.ViewState["_priority"] = value; }
    }

    public string Detail
    {
        get
        {
            object obj = this.ViewState["_detail"];
            return (obj == null) ? "" : (string)obj;

        }
        set { this.ViewState["_detail"] = value; }
    }
    #endregion

    protected void lbBack_Click(object sender, EventArgs e)
    {

    }
}