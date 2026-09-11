using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Reports_TicketAnalysis : BasePage
{
    private int _maxDaysDifference = 365;
    /// <summary>
    /// page load
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        DisplayDatepickerPluginJs();
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
            BindTicketAnalysisGrid();
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
        //ucToFrom.ClearAll();

        //clear designation
        ddlTicketDesignationDDL.TicketDesignationId = null;
        ddlTicketType.TicketTypeId = null;
        //clear client cb
        ucClientComboBox.ClientId = null;

        //clear msp only
        chkMsp.Checked = false;

        //hide grid
        phSearchResults.Visible = false;
    }

    #endregion

    #region private properties

    /// <summary>
    /// get client id
    /// </summary>
    private int? SelectedClientID
    {
        get { return ucClientComboBox.ClientId > 0 ? ucClientComboBox.ClientId : (int?)null; }
    }

    private int? TicketTypeID
    {
        get { return ddlTicketType.TicketTypeId > 0 ? ddlTicketType.TicketTypeId : (int?)null; }
    }

    /// <summary>
    /// get designation
    /// </summary>
    private int? Designation
    {
        get { return ddlTicketDesignationDDL.TicketDesignationId; }
    }

    /// <summary>
    /// get start date
    /// </summary>
    private DateTime StartDate
    {
        get { return Convert.ToDateTime(txtStartDate.Text); }
    }

    /// <summary>
    /// get end date
    /// </summary>
    private DateTime EndDate
    {
        get { return Convert.ToDateTime(txtEndDate.Text); }
    }

    /// <summary>
    /// get msp only
    /// </summary>
    private bool MspOnly
    {
        get { return chkMsp.Checked; }
    }

    #endregion

    #region private methods

    /// <summary>
    /// display ticket analysis grid
    /// </summary>
    private void BindTicketAnalysisGrid()
    {
        phSearchResults.Visible = true;

        //set search values
        ucTicketAnalysisGrid.SearchStartDate = StartDate;
        ucTicketAnalysisGrid.SearchEndDate = EndDate;
        ucTicketAnalysisGrid.SearchClientId = SelectedClientID;
        ucTicketAnalysisGrid.SearchDesignation = Designation;
        ucTicketAnalysisGrid.SearchMspOnly = MspOnly;
        ucTicketAnalysisGrid.SearchTicketType = TicketTypeID;
        //rebind grid
        ucTicketAnalysisGrid.ResetGrid();

    }

    /// <summary>
    /// set default values for search 
    /// </summary>
    private void SetDefaultValues()
    {
        //default start / end dates 
        txtStartDate.Text = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).ToString("MM/dd/yyyy");
        txtEndDate.Text = DateTime.Now.ToString("MM/dd/yyyy");

        //bind ticket analysis grid
        BindTicketAnalysisGrid();
    }

    #endregion

    private void DisplayDatepickerPluginJs()
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        sb.Append("<script type='text/javascript'>");
        sb.Append("$(document).ready(function() {");
        sb.Append(String.Format("Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(PageLoadedDatePicker_{0})", this.ClientID));
        sb.Append("});");
        sb.Append(String.Format("function PageLoadedDatePicker_{0}(sender, args) {{", this.ClientID));
        sb.Append("$('.input-group.date').datepicker({");
        sb.Append("format: 'mm/dd/yyyy',");
        sb.Append("todayHighlight: true,");
        sb.Append("clearBtn: true,");
        sb.Append("showOnFocus: false,");
        sb.Append("assumeNearbyYear: true,");
        sb.Append("autoclose: true,");
        sb.Append("});");
        sb.Append("}");
        sb.Append("</script>");
        litJs.Text = sb.ToString();
    }

    protected void customvDates_ServerValidate(object source, ServerValidateEventArgs args)
    {
        if (_maxDaysDifference > 0)
        {
            TimeSpan _ts = Convert.ToDateTime(txtEndDate.Text) - Convert.ToDateTime(txtStartDate.Text);
            int _days = _ts.Days;

            if (_days > _maxDaysDifference)
            {
                args.IsValid = false;
                return;
            }
        }

        args.IsValid = true;
    }
}