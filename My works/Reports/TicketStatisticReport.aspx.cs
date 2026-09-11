using DesktopShared;
using DesktopShared.DaoClasses;
using DesktopShared.FactoryClasses;
using DesktopShared.HelperClasses;
using iTextSharp.text.pdf;
using SD.LLBLGen.Pro.ORMSupportClasses;
using System;
using System.Configuration;
using System.Data;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using System.Text;
using System.Net;
using System.IO;
using iTextSharp.text;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;

public partial class Reports_TicketStatisticReport : System.Web.UI.Page
{
    private static List<int> _refno;
    protected void Page_Load(object sender, EventArgs e)
    {
       
        if (!this.IsPostBack)
        {
            ucDateEnd.SelectedDate = DateTime.Now;
            ucDateStart.SelectedDate = DateTime.Now;
        }
    }

    protected void rgTicket_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        BindGrid();
    }

    protected void BindGrid()
    {   
        DateTime? _start, _end;
           
        if (ucDateStart.SelectedDate.HasValue)
            _start = ucDateStart.SelectedDate.Value;
        else
            _start = null;
        if (ucDateEnd.SelectedDate.HasValue)
            _end = ucDateEnd.SelectedDate.Value;
        else
            _end = null;
                   
        rgTicket.DataSource = GetTicketStatistic(_start, _end);
        
    }

    protected void cvTicketdate_ServerValidate(object source, ServerValidateEventArgs args)
    {
        args.IsValid = true;
        if (!ucDateStart.SelectedDate.HasValue || !ucDateEnd.SelectedDate.HasValue)
            return;
        args.IsValid = ucDateEnd.SelectedDate.Value.Date >= ucDateStart.SelectedDate.Value.Date;
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        BindGrid();
        rgTicket.DataBind();
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        ucDateEnd.SelectedDate = DateTime.Now;
        ucDateStart.SelectedDate = DateTime.Now;
      
    }


    #region private methods
    private DataTable GetTicketStatistic(DateTime? StartDate, DateTime? EndDate)
    {
        
        DateTime sd = Convert.ToDateTime(StartDate);
        DateTime ed = Convert.ToDateTime(EndDate).AddDays(1);
        string SDate = sd.ToString("yyyy-MM-dd");
        string EDate = ed.ToString("yyyy-MM-dd");

        string connectionString = BitByBit.Configuration.GetConfigString("ConnectionString.SQL Server (SqlClient)");

        SqlConnection connection = new SqlConnection(connectionString);
        DataTable dt = new DataTable();

        connection.Open();
        SqlDataAdapter da = new SqlDataAdapter("ticketAudit_get", connection);
        da.SelectCommand.CommandType = CommandType.StoredProcedure;
        da.SelectCommand.Parameters.Add("@StartDate", SqlDbType.VarChar, 100).Value = SDate;
        da.SelectCommand.Parameters.Add("@EndDate", SqlDbType.VarChar, 100).Value = EDate;
      
        da.SelectCommand.CommandTimeout = 600;

        da.Fill(dt);
        connection.Close();

        return dt;
    }
    #endregion
}