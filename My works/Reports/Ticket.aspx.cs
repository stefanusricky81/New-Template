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

public partial class Reports_Ticket : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
            SetDefaultValuesAndChart();
    }

    #region protected methods

    /// <summary>
    /// submit button click
    /// </summary>
    /// <param name="source"></param>
    /// <param name="e"></param>
    protected void btnSubmit_Click(object source, EventArgs e)
    {
        if (Page.IsValid)
        {
            string _reportType = ddlReportType.SelectedValue.Trim();
            switch (_reportType)
            {
                #region TicketAnalysis

                case "TicketAnalysis":
                    BindTicketAnalysisChart();
                    break;

                #endregion

                #region TicketEmployee

                case "TicketEmployee":
                    BindTicketEmployeeChart();
                    break;

                #endregion
            }
        }
    }

    /// <summary>
    /// clear button click
    /// </summary>
    /// <param name="source"></param>
    /// <param name="e"></param>
    protected void btnClear_Click(object source, EventArgs e)
    {
        //clear client cb
        ucClientComboBox.ClearItems();
        //clear assigned to
        ucEmployeeAssignedTo.EmployeeId = -1;
        //clear dates
        ucToFrom.ClearAll();
        //3d as default
        Use3dChart = true;
        
        //include all options as default
        IncludeOpen = true;
        IncludeNew = true;
        IncludeClosed = true;
        IncludeUpdated = true;

        //remove report
        phReport.Controls.Clear();

        //report type
        ddlReportType.ClearSelection();
        ListItem liDefault = ddlReportType.Items.FindByValue("");
        if (liDefault != null)
            liDefault.Selected = true;
    }

    #endregion

    #region private properties

    /// <summary>
    /// get client id
    /// </summary>
    private int? SelectedClientID
    {
        get { return ucClientComboBox.SelectedClientId > 0 ? ucClientComboBox.SelectedClientId : (int?)null; }
    }

    /// <summary>
    /// get employee id
    /// </summary>
    private int? EmployeeID
    {
        get { return ucEmployeeAssignedTo.EmployeeId > 0 ? ucEmployeeAssignedTo.EmployeeId : (int?)null; }
    }

    /// <summary>
    /// get start date
    /// </summary>
    private DateTime StartDate
    {
        get { return ucToFrom.StartDate; }
    }

    /// <summary>
    /// get end date
    /// </summary>
    private DateTime EndDate
    {
        get { return ucToFrom.EndDate; }
    }

    /// <summary>
    /// get use 3d chart
    /// </summary>
    private bool Use3dChart
    {
        get { return chk3d.Checked; }
        set { chk3d.Checked = value; }
    }

    /// <summary>
    /// get/set include open tickets in report
    /// </summary>
    private bool IncludeOpen
    {
        get { return chkOpen.Checked; }
        set { chkOpen.Checked = value; }
    }

    /// <summary>
    /// get/set include closed tickets in report
    /// </summary>
    public bool IncludeClosed
    {
        get { return chkClosed.Checked; }
        set { chkClosed.Checked = value; }
    }

    /// <summary>
    /// get/set include new tickets in report
    /// </summary>
    public bool IncludeNew
    {
        get { return chkNew.Checked; }
        set { chkNew.Checked = value; }
    }

    /// <summary>
    /// get/set include updated tickets in report
    /// </summary>
    public bool IncludeUpdated
    {
        get { return chkUpdated.Checked; }
        set { chkUpdated.Checked = value; }
    }

    #endregion

    #region private methods

    /// <summary>
    /// display ticket analysis chart
    /// </summary>
    private void BindTicketAnalysisChart()
    {
        UserControl_Report_Ticket_TicketAnalysis ucTicketAnalysis =
            (UserControl_Report_Ticket_TicketAnalysis)
            LoadControl("/UserControl/Report/Ticket/TicketAnalysis.ascx");

        ucTicketAnalysis.Use3dChart = Use3dChart;
        ucTicketAnalysis.StartDate = StartDate;
        ucTicketAnalysis.EndDate = EndDate;
        ucTicketAnalysis.SelectedClientID = SelectedClientID;
        ucTicketAnalysis.EmployeeID = EmployeeID;
        ucTicketAnalysis.IncludeOpen = IncludeOpen;
        ucTicketAnalysis.IncludeNew = IncludeNew;
        ucTicketAnalysis.IncludeClosed = IncludeClosed;
        ucTicketAnalysis.IncludeUpdated = IncludeUpdated;
        
        ucTicketAnalysis.BindChart();

        phReport.Controls.Add(ucTicketAnalysis);

    }

    /// <summary>
    /// display ticket employee chart
    /// </summary>
    private void BindTicketEmployeeChart()
    {
        UserControl_Report_Ticket_TicketEmployee ucTicketEmployee = 
            (UserControl_Report_Ticket_TicketEmployee)
            LoadControl("/UserControl/Report/Ticket/TicketEmployee.ascx");

        ucTicketEmployee.Use3dChart = Use3dChart;
        ucTicketEmployee.BindChart();

        phReport.Controls.Add(ucTicketEmployee);
    }

    /// <summary>
    /// set default values for search / chart display
    /// </summary>
    private void SetDefaultValuesAndChart()
    {
        //default start / end dates past 30 days
        ucToFrom.EndDate = DateTime.Now;
        ucToFrom.StartDate = DateTime.Now.AddDays(-30);
        
        //default use 3d chart
        Use3dChart = true;

        //default to all types
        IncludeOpen = true;
        IncludeNew = true;
        IncludeClosed = true;
        IncludeUpdated = true;

        //default to ticket analyis for ddl
        ListItem li = ddlReportType.Items[1];
        if (li != null)
            li.Selected = true;

        //bind ticket analysis chart
        BindTicketAnalysisChart();
    }

    #endregion

}
