using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class Reports_EmployeeTimesheetReport : System.Web.UI.Page
{
    decimal _clienthours;
    decimal _hours;
    int _clientper;
    decimal _bbbhours;

    protected void Page_Load(object sender, EventArgs e)
    {
        DisplayChosenPluginJs();
        if (!IsPostBack)
        {
            DdlClient();
            litJs.Visible = true;
        }
    }

    #region protected
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
            rgTimesheetbyemployee.DataBind();
        }
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        Response.Redirect("EmployeeTimesheetReport.aspx");
    }

    protected void rgTimesheetbyemployee_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        BindGrid();
    }

    protected void rgTimesheetbyemployee_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        
        if (e.Item.ItemType == GridItemType.Item || e.Item.ItemType == GridItemType.AlternatingItem)
        {
            GridDataItem _gdi = e.Item as GridDataItem;
            Label lblClientHours = _gdi.FindControl("lblTotalClientHours") as Label;
            Label lblHours = _gdi.FindControl("lblTotalHours") as Label;
            Label lblClientper = _gdi.FindControl("lblTotalClientper") as Label;
            Label lblBBBHours = _gdi.FindControl("lblBBBHours") as Label;

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
            footerItem["EmpName"].Text = "Total";
            footerItem["ClientHours"].Text = _clienthours.ToString();
            footerItem["HOURS"].Text = _hours.ToString();
            footerItem["BBHours"].Text = _bbbhours.ToString();
            footerItem["Clientper"].Text = string.Format("{0} %", _clientper);
            //footerItem["BILLEDAMT"].Text = string.Format("$ {0}", _billamount);
            //footerItem["BILLABLEperHRS"].Text = string.Format("$ {0:#.00}", _billperhours);
            //footerItem["BILLABLEperClientHRS"].Text = string.Format("$ {0:#.00}", _billperclienthours);
        }
    }

    protected void lbexport_Click(object sender, EventArgs e)
    {
        rgTimesheetbyemployee.ExportSettings.IgnorePaging = true;
        rgTimesheetbyemployee.ExportSettings.ExportOnlyData = true;
        rgTimesheetbyemployee.ExportSettings.HideStructureColumns = true;

        rgTimesheetbyemployee.GridLines = GridLines.Both;
        rgTimesheetbyemployee.BorderStyle = BorderStyle.Solid;

        rgTimesheetbyemployee.ExportSettings.FileName = "Employee Timesheet_" + ucCreatedStart.SelectedDate.Value.ToString("yyyyMMdd") + "-" + ucCreatedEnd.SelectedDate.Value.ToString("yyyyMMdd") + "_" + DateTime.Now.ToString("yyyyMMdd-hhmmss");
        rgTimesheetbyemployee.MasterTableView.ExportToExcel();
    }

    #endregion

    #region private
    private void BindGrid()
    {
        rgTimesheetbyemployee.DataSource = GetDataEmployeeName();
    }

    private System.Data.DataTable GetDataEmployeeName()
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
                adp.SelectCommand.CommandTimeout = 3600;
                adp.SelectCommand.Parameters.AddWithValue("@type", "byEmployee");
                adp.SelectCommand.Parameters.AddWithValue("@startdate", ucCreatedStart.SelectedDate);
                adp.SelectCommand.Parameters.AddWithValue("@enddate", ucCreatedEnd.SelectedDate);
                adp.SelectCommand.Parameters.AddWithValue("@clientid", ddlClient.SelectedValue) ;
                adp.SelectCommand.Parameters.AddWithValue("@projectid", ddlProject.SelectedValue);

                dt.Reset();
                adp.Fill(dt);
            }
        }
        catch(Exception ex)
        { return null; }
        finally { conn.Close(); }

        return dt;
    }

    #endregion

    #region new addition
    private string _cssClass = "form-control select-chosen";

    private void DisplayChosenPluginJs()
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        sb.Append("<script type='text/javascript'>");
        sb.Append("$(document).ready(function() {");
        sb.Append(String.Format("SetChosenClient_{0}();", this.ClientID));
        sb.Append(String.Format("Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(SetChosenClient_{0})", this.ClientID));
        sb.Append("});");
        sb.Append(String.Format("function SetChosenClient_{0}(sender, args) {{", this.ClientID));
        sb.Append(String.Format("$('#{0}').chosen({{", ddlClient.ClientID));
        sb.Append("allow_single_deselect: true,");
        sb.Append("placeholder_text_single: \"Select a Client ...\",");
        sb.Append("width: \"100%\"");
        sb.Append("});");
        sb.Append("}");
        sb.Append("</script>");
        litJs.Text = sb.ToString();
    }

    public void DdlClient()
    {
        //clear items
        ddlClient.ClearSelection();
        ddlClient.Items.Clear();

        ddlClient.DataSource = DesktopShared.Client.GetAllActive("", false);
        ddlClient.DataValueField = "pclient";
        ddlClient.DataTextField = "company";
        ddlClient.DataBind();
        ddlClient.Items.Insert(0, "");
    }
    #endregion

    protected void ddlClient_SelectedIndexChanged(object sender, EventArgs e)
    {
        int clientid = 0;

        ddlProject.ClearSelection();
        ddlProject.Items.Clear();

        if (ddlClient.SelectedValue != string.Empty)
            clientid = Convert.ToInt32(ddlClient.SelectedValue);

        ddlProject.DataSource = GetActiveProject(clientid);
        ddlProject.DataValueField = "pprojtask";
        ddlProject.DataTextField = "descr1";
        ddlProject.DataBind();
        ddlProject.Items.Insert(0, "");
    }

       public static DesktopShared.CollectionClasses.ProjtaskCollection GetActiveProject(int clientid)
        {
            DesktopShared.CollectionClasses.ProjtaskCollection collection = new DesktopShared.CollectionClasses.ProjtaskCollection();
            SD.LLBLGen.Pro.ORMSupportClasses.IPredicateExpression _filters = new SD.LLBLGen.Pro.ORMSupportClasses.PredicateExpression();

            _filters.Add(DesktopShared.HelperClasses.ProjtaskFields.FkClient == clientid);
            _filters.Add(DesktopShared.HelperClasses.ProjtaskFields.Status == 'O');

            collection.GetMulti(_filters);
            return collection;
        }
}