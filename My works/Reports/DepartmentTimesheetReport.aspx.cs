using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class Reports_DepartmentTimesheetReport : System.Web.UI.Page
{
    decimal _clienthours;
    decimal _hours;
    decimal _bbbhours;
    int _clientper=0;

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    private System.Data.DataTable GetDataDeptName()
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
                adp.SelectCommand.Parameters.AddWithValue("@type", "byDepartment");
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
        rgTimesheetbydepartment.DataSource = GetDataDeptName();
    }

    protected void rgTimesheetbydepartment_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        BindGrid();
    }

    protected void rgTimesheetbydepartment_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item.ItemType == GridItemType.Item || e.Item.ItemType == GridItemType.AlternatingItem)
        {
            GridDataItem _gdi = e.Item as GridDataItem;
            Label lblClientHours = _gdi.FindControl("lblTotalClientHours") as Label;
            Label lblHours = _gdi.FindControl("lblTotalHours") as Label;
            Label lblClientper = _gdi.FindControl("lblTotalClientper") as Label;
            Label lblBBBHours = _gdi.FindControl("lblTotalBBBHours") as Label;

            _clienthours += Convert.ToDecimal(lblClientHours.Text);
            _hours += Convert.ToDecimal(lblHours.Text);
            _bbbhours += Convert.ToDecimal(lblBBBHours.Text);

            if (_hours == 0)
                _clientper = 0;
            else
                _clientper = Convert.ToInt32((Convert.ToDecimal(_clienthours) / Convert.ToDecimal(_hours)) * 100);
        }

        if (e.Item is GridFooterItem)
        {
            GridFooterItem footerItem = e.Item as GridFooterItem;
            footerItem["DeptName"].Text = "Total";
            footerItem["ClientHours"].Text = _clienthours.ToString();
            footerItem["BBBHours"].Text = _bbbhours.ToString();
            footerItem["HOURS"].Text = _hours.ToString();
            footerItem["Clientper"].Text = string.Format("{0} %", _clientper);
        }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (ucCreatedStart.SelectedDate.HasValue && ucCreatedEnd.SelectedDate.HasValue)
        {
            if (ucCreatedEnd.SelectedDate.Value.Date <= ucCreatedStart.SelectedDate.Value.Date)
            {
                cvEmployeeTimeSheet.IsValid = false;
                phSearchResults.Visible = false;
                return;
            }
        }
        if (Page.IsValid)
        {
            phSearchResults.Visible = true;
            BindGrid();
            rgTimesheetbydepartment.DataBind();
        }
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        Response.Redirect("DepartmentTimesheetReport.aspx");
    }

    protected void lbexport_Click(object sender, EventArgs e)
    {
        rgTimesheetbydepartment.ExportSettings.IgnorePaging = true;
        rgTimesheetbydepartment.ExportSettings.ExportOnlyData = true;
        rgTimesheetbydepartment.ExportSettings.HideStructureColumns = true;

        rgTimesheetbydepartment.GridLines = GridLines.Both;
        rgTimesheetbydepartment.BorderStyle = BorderStyle.Solid;

        rgTimesheetbydepartment.ExportSettings.FileName = "Department Timesheet_" + ucCreatedStart.SelectedDate.Value.ToString("yyyyMMdd") + "-" + ucCreatedEnd.SelectedDate.Value.ToString("yyyyMMdd") + "_" + DateTime.Now.ToString("yyyyMMdd-hhmmss");
        rgTimesheetbydepartment.MasterTableView.ExportToExcel();
    }
}