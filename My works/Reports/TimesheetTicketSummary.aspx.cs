using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Reports_TimesheetTicketSummary : BasePage
{
    /// <summary>
    /// page load
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
            SetDefaultValues();
    }

    #region protected methods

    /// <summary>
    /// submit button on click
    /// </summary>
    /// <param name="source"></param>
    /// <param name="e"></param>
    protected void btnSubmit_Click(object source, EventArgs e)
    {
        if (Page.IsValid)
        {
            BindTimesheetTicketSummaryGrid();
        }
    }

    /// <summary>
    /// clear button on click
    /// </summary>
    /// <param name="source"></param>
    /// <param name="e"></param>
    protected void btnClear_Click(object source, EventArgs e)
    {
        //clear dates
        ucToFrom.ClearAll();

        //clear employee drop down list
        ucEmployeeDDL.EmployeeId = -1;

        //clear client cb
        ucClientComboBox.ClearItems();

        //hide grid
        pnlGrid.Visible = false;
    }

    #region custom validators

    protected void cvSearch_ServerValidate(object sender, ServerValidateEventArgs args)
    {
        //if client or employee selected -> return true.  date range is already enforced
        if ((ucEmployeeDDL.EmployeeId > 0) || (ucClientComboBox.SelectedClientId > 0))
        {
            args.IsValid = true;
            return;
        }

        //max of 31 days if not selecting client or employee
        TimeSpan _ts = EndDate - StartDate;
        int _days = _ts.Days;
        if (_days > 31)
        {
            args.IsValid = false;
            cvSearch.ErrorMessage = "Maximium date range is 31 days if not selecting client or employee.";
            return;
        }

        args.IsValid = true;
    }

    #endregion

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
    private int? SelectedEmployeeId
    {
        get { return ucEmployeeDDL.EmployeeId > 0 ? ucEmployeeDDL.EmployeeId : (int?)null; }
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

    #endregion

    #region private methods

    /// <summary>
    /// display ticket analysis grid
    /// </summary>
    private void BindTimesheetTicketSummaryGrid()
    {
        pnlGrid.Visible = true;

        //set search values
        ucTimesheetTicketSummaryGrid.SearchStartDate = StartDate;
        ucTimesheetTicketSummaryGrid.SearchEndDate = EndDate;
        ucTimesheetTicketSummaryGrid.SearchClientId = SelectedClientID;
        ucTimesheetTicketSummaryGrid.SearchEmployeeId = SelectedEmployeeId;

        //rebind grid
        ucTimesheetTicketSummaryGrid.ResetGrid();

    }

    /// <summary>
    /// set default values for search 
    /// </summary>
    private void SetDefaultValues()
    {
        //default start / end dates 
        ucToFrom.StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        ucToFrom.EndDate = DateTime.Now;
    }

    #endregion
}