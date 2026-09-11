using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Reports_EmployeeTimesheetMonthlyReport : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (ucCreatedStart.SelectedDate.HasValue && ucCreatedEnd.SelectedDate.HasValue)
        {
            if (ucCreatedEnd.SelectedDate.Value.Date <= ucCreatedStart.SelectedDate.Value.Date)
            {
                cvEmployeeTimeSheetMonthly.IsValid = false;
                phSearchResults.Visible = false;
                return;
            }
        }
        if (Page.IsValid)
        {
            phSearchResults.Visible = true;
            BindGrid();
            rpgTimesheetMonthly.DataBind();
        }
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        Response.Redirect("EmployeeTimesheetMonthlyReport.aspx");
    }
    private System.Data.DataTable GetDataMonthlyEmployee()
    {
        System.Data.DataTable dt = new System.Data.DataTable();
        string connectionString = System.Configuration.ConfigurationManager.AppSettings["ConnectionString.SQL Server (SqlClient)"];
        System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(connectionString);
        try
        {

            using (conn)
            {
                conn.Open();
                System.Data.SqlClient.SqlDataAdapter adp = new System.Data.SqlClient.SqlDataAdapter("proc_GetTimesheet", conn);
                adp.SelectCommand.CommandType = CommandType.StoredProcedure;
                adp.SelectCommand.Parameters.AddWithValue("@type", "byMonthly");
                adp.SelectCommand.Parameters.AddWithValue("@startdate", ucCreatedStart.SelectedDate);
                adp.SelectCommand.Parameters.AddWithValue("@enddate", ucCreatedEnd.SelectedDate);

                dt.Reset();
                adp.Fill(dt);
            }
        }
        catch
        { return null; }
        finally { conn.Close(); }

        return dt;
    }

    private void BindGrid()
    {
        rpgTimesheetMonthly.DataSource = GetDataMonthlyEmployee();
    }
    protected void rpgTimesheetMonthly_NeedDataSource(object sender, Telerik.Web.UI.PivotGridNeedDataSourceEventArgs e)
    {
        BindGrid();
    }
}