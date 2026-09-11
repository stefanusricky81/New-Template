using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class Reports_VendorInvoiceSearch : BasePage
{
    /// <summary>
    /// page load
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        ConfigureForDevice();
        if (!this.IsPostBack)
        {
            SetUpPage();
            ucStartDate.SelectedDate = DateTime.Now.AddMonths(-1);
            ucEndDate.SelectedDate = DateTime.Now;
        }
    }

    #region private methods

    /// <summary>
    /// get boolean selection from drop down list
    /// </summary>
    /// <param name="ddl"></param>
    /// <returns></returns>
    private bool? GetBoolSelection(DropDownList ddl)
    {
        if (ddl.SelectedIndex == 0)
            return null;
        return ddl.SelectedIndex == 1;
    }

    /// <summary>
    /// configure for device
    /// </summary>
    private void ConfigureForDevice()
    {
        string _cssClass = "form-control select-chosen";
        //bool _displayChosenScript = true;
        if (DesktopShared.Utility.BrowserHelper.IsTabletOrSmartPhone(this.Request))
        {
            _cssClass = "form-control";
            //phTabletPagerCss.Visible = true;
            //_displayChosenScript = false;
        }

        //ddlPaid.CssClass = _cssClass;
    }

    /// <summary>
    /// set up page
    /// </summary>
    private void SetUpPage()
    {
        txtVendor.Focus();

        short _tabIndex = 0;
        txtVendor.TabIndex = ++_tabIndex;        
        ucStartDate.TabIndex = ++_tabIndex;
        ucEndDate.TabIndex = ++_tabIndex;
        btnSubmit.TabIndex = ++_tabIndex;
        btnClear.TabIndex = ++_tabIndex;
    }

    /// <summary>
    /// rebind document grid
    /// </summary>
    private void RebindGrid()
    {
        phSearchResults.Visible = true;
        rgInvoice.Visible = true;
        rgInvoice.EditIndexes.Clear();
        rgInvoice.DataSource = null;
        rgInvoice.Rebind();
        litMessage.Text = "";
    }

    private DataTable GetInvoices(string Vendor, DateTime? StartDate, DateTime? EndDate)
    {

        string connectionString = BitByBit.Configuration.GetConfigString("ConnectionString.SQL Server (SqlClient)");

        SqlConnection connection = new SqlConnection(connectionString);
        DataTable dt = new DataTable();

        connection.Open();
        SqlDataAdapter da = new SqlDataAdapter("QB_VendorInvoicesSearch", connection);
        da.SelectCommand.CommandType = CommandType.StoredProcedure;        
        if (!string.IsNullOrEmpty(Vendor))
            da.SelectCommand.Parameters.Add("@Vendor", SqlDbType.VarChar, 100).Value = Vendor;

        if (StartDate.HasValue)
        {           
            string sDate = string.Format("{0:MM/dd/yyyy}", StartDate);
            da.SelectCommand.Parameters.Add("@sBillStartDate", SqlDbType.VarChar,10).Value = sDate;
        }

        if (EndDate.HasValue)
        {            
            string eDate = string.Format("{0:MM/dd/yyyy}", EndDate);
            da.SelectCommand.Parameters.Add("@sBillEndDate", SqlDbType.VarChar, 10).Value = eDate;
        }
        
        da.SelectCommand.CommandTimeout = 600;

        da.Fill(dt);
        connection.Close();

        return dt;
    }

    #endregion

    #region protected events

    /// <summary>
    /// search button click
    /// </summary>
    /// <param name="source"></param>
    /// <param name="e"></param>
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {    
            RebindGrid();
        }
    }

    /// <summary>
    /// clear button on click
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnClear_Click(object sender, EventArgs e)
    {        
        txtVendor.Text = "";
        ucStartDate.SelectedDate = DateTime.Now;
        ucEndDate.SelectedDate = DateTime.Now.AddMonths(-1);
    }

  
    /// <summary>
    /// export button click
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnExport_Click(object sender, System.EventArgs e)
    {
        rgInvoice.ExportSettings.IgnorePaging = true;
        rgInvoice.ExportSettings.OpenInNewWindow = true;
        rgInvoice.ExportSettings.ExportOnlyData = false;
        rgInvoice.ExportSettings.HideStructureColumns = true;

        rgInvoice.ExportSettings.FileName = "BitByBit-Auto-Pay-History-" + DateTime.Now.ToString("yyyyMMdd-hhmmss");

        //hide command item template
        foreach (GridItem commandItem in this.rgInvoice.MasterTableView.GetItems(GridItemType.CommandItem))
        {
            commandItem.Visible = false;
            commandItem.Display = false;
        }

        rgInvoice.Columns.FindByUniqueName("Action").Visible = false;

        rgInvoice.MasterTableView.ExportToCSV();
    }

    #region telerik grid

    /// <summary>
    /// document grid on need data source
    /// </summary>
    /// <param name="source"></param>
    /// <param name="e"></param>
    protected void rgInvoice_NeedDataSource(object source, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        DateTime? _startDate = ucStartDate.SelectedDate;
        DateTime? _endDate = ucEndDate.SelectedDate;

        rgInvoice.DataSource = GetInvoices(txtVendor.Text, _startDate, _endDate);        
    }      

    /// <summary>
    /// grid on pre render
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rgInvoice_PreRender(object sender, EventArgs e)
    {
        if (DesktopShared.Utility.BrowserHelper.IsTabletOrSmartPhone(this.Request))
        {
            int _colIndex = -1;

            rgInvoice.MasterTableView.Columns[++_colIndex].HeaderStyle.Width = Unit.Pixel(175);
            rgInvoice.MasterTableView.Columns[++_colIndex].HeaderStyle.Width = Unit.Pixel(125);
            rgInvoice.MasterTableView.Columns[++_colIndex].HeaderStyle.Width = Unit.Pixel(125);
            rgInvoice.MasterTableView.Columns[++_colIndex].HeaderStyle.Width = Unit.Pixel(125);
            rgInvoice.MasterTableView.Columns[++_colIndex].HeaderStyle.Width = Unit.Pixel(175);
            rgInvoice.MasterTableView.Columns[++_colIndex].HeaderStyle.Width = Unit.Pixel(250);
            rgInvoice.MasterTableView.Columns[++_colIndex].HeaderStyle.Width = Unit.Pixel(100);
        }
    }
   
    #endregion

    #region custom validators

    /// <summary>
    /// validate dates
    /// </summary>
    /// <param name="source"></param>
    /// <param name="args"></param>
    protected void cvDates_ServerValidate(object source, ServerValidateEventArgs args)
    {
        args.IsValid = true;
        if (!ucStartDate.SelectedDate.HasValue || !ucEndDate.SelectedDate.HasValue)
            return;
        args.IsValid = ucEndDate.SelectedDate.Value.Date >= ucStartDate.SelectedDate.Value.Date;
    }

    #endregion

    #endregion

    
    protected void rgInvoice_ItemCommand(object sender, GridCommandEventArgs e)
    {
        #region send email

        if (e.CommandName == "SendEmail")
        {
            int _id = int.Parse((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["Id"].ToString());
            DesktopShared.Email.Client.Invoice.Send(new DesktopShared.EntityClasses.ClientInvoiceEntity(_id), DesktopShared.User.UserID);

            RebindGrid();
            DesktopShared.Bootstrap.Alert.DisplayMessage(litMessage, String.Format("Email has been sent - {0}.", DateTime.Now), DesktopShared.Bootstrap.Alert.AlertType.Success);
        }

        #endregion

        #region view pdf

        else if (e.CommandName == "ViewPdf")
        {
            int _id = int.Parse((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["Id"].ToString());
            var objClientInvoice = new DesktopShared.EntityClasses.ClientInvoiceEntity(_id);
            string _errorMessage = "";

            string _containerName = objClientInvoice.AzureContainerName.Trim();
            DesktopShared.AzureHelper.ContainerName _containerNameEnum = DesktopShared.AzureHelper.ContainerName.client;
            if (!Enum.TryParse(_containerName, out _containerNameEnum))
            {
                _errorMessage = "Unable to parse Container Name";
                DesktopShared.Bootstrap.Alert.DisplayMessage(litMessage, String.Format("Unable to get File - {0}", _errorMessage), DesktopShared.Bootstrap.Alert.AlertType.Danger);
                return;
            }

            System.IO.MemoryStream _ms = DesktopShared.AzureHelper.GetBlobAsStream(_containerNameEnum, objClientInvoice.AzureFileId, ref _errorMessage);

            if (_ms == null)
            {
                DesktopShared.Bootstrap.Alert.DisplayMessage(litMessage, String.Format("Unable to get File - {0}", _errorMessage), DesktopShared.Bootstrap.Alert.AlertType.Danger);
                return;
            }
            else
            {
                string _fullFileName = String.Format("{0}{1}", objClientInvoice.FileName.Trim(), objClientInvoice.FileExtension.Trim());
                string _headerValue = String.Format("attachment;filename={0}", _fullFileName);
                Response.ContentType = MimeMapping.GetMimeMapping(_fullFileName);
                Response.AddHeader("content-disposition", _headerValue);
                Response.Buffer = true;
                _ms.WriteTo(Response.OutputStream);
                Response.End();
            }
        }

        #endregion
    }
}
