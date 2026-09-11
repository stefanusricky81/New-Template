using SD.LLBLGen.Pro.ORMSupportClasses;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class Reports_Csat : BasePage
{
    /// <summary>
    /// page load
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        ConfigureForDevice();
        if (!IsPostBack)
        {
            SetUpPage();
        }
    }

    #region private methods

    /// <summary>
    /// set up for initial page load
    /// </summary>
    private void SetUpPage()
    {
        ucStartDate.Focus();

        short _tabIndex = 0;
        ucStartDate.TabIndex = ++_tabIndex;
        ucEndDate.TabIndex = ++_tabIndex;
        lbRating.TabIndex = ++_tabIndex;
        ddlCsatCompany.TabIndex = _tabIndex;
        ddlCsatTeamMember.TabIndex = _tabIndex;
        btnSubmit.TabIndex = ++_tabIndex;
        btnClear.TabIndex = ++_tabIndex;

        int clientid;
        if (int.TryParse(Request.QueryString["ClientId"], out clientid))
        {
            ddlCsatCompany.Company = DesktopShared.Client.GetName(clientid);
        }

        int _id;
        if (int.TryParse(Request.QueryString["Id"], out _id))
        {
            if (_id > 0 && _id < 4)
            {
                ListItem li;
                if (_id == 1)
                {
                    li = lbRating.Items.FindByValue("4");
                    if (li != null)
                        li.Selected = true;
                    li = lbRating.Items.FindByValue("5");
                    if (li != null)
                        li.Selected = true;
                }
                else if (_id == 3)
                {
                    li = lbRating.Items.FindByValue("3");
                    if (li != null)
                        li.Selected = true;
                }
                else if (_id == 2)
                {
                    li = lbRating.Items.FindByValue("2");
                    if (li != null)
                        li.Selected = true;
                }

                DateTime endDate = DateTime.Now;
                DateTime startDate = endDate.AddMonths(-2);
                ucStartDate.SelectedDate = startDate;
                ucEndDate.SelectedDate = endDate;

                RebindGrid();
            }
        }
        
    }

    /// <summary>
    /// configure for device
    /// </summary>
    private void ConfigureForDevice()
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        sb.Append("<script type='text/javascript'>");
        sb.Append("$(document).ready(function() {");
        sb.Append(String.Format("SetChosenRating_{0}();", this.ClientID));
        sb.Append(String.Format("Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(SetChosenRating_{0})", this.ClientID));
        sb.Append("});");
        sb.Append(String.Format("function SetChosenRating_{0}(sender, args) {{", this.ClientID));
        sb.Append(String.Format("$('#{0}').chosen({{", lbRating.ClientID));
        sb.Append("allow_single_deselect: true,");
        sb.Append("placeholder_text_multiple: \"Select Rating ...\",");
        sb.Append("width: \"100%\"");
        sb.Append("});");
        sb.Append("}");
        sb.Append("</script>");
        litRatingJs.Text = sb.ToString();

        bool _displayChosenScript = true;
        string _cssClass = "form-control select-chosen";
        if (DesktopShared.Utility.BrowserHelper.IsTabletOrSmartPhone(this.Request))
        {
            _displayChosenScript = false;
            _cssClass = "form-control";
            phTabletPagerCss.Visible = true;
        }
        else
        {
            litRatingJs.Visible = true;
        }

        ddlCsatCompany.DisplayChosenScript = _displayChosenScript;
        ddlCsatCompany.CssClass = _cssClass;
        ddlCsatTeamMember.DisplayChosenScript = _displayChosenScript;
        ddlCsatTeamMember.CssClass = _cssClass;
        lbRating.CssClass = _cssClass;
    }

    /// <summary>
    /// rebind document grid
    /// </summary>
    private void RebindGrid()
    {
        litMessage.Text = "";
        phSearchResults.Visible = true;
        rgReport.Visible = true;
        rgReport.EditIndexes.Clear();
        rgReport.DataSource = null;
        rgReport.Rebind();
    }

    /// <summary>
    /// configure grid for export
    /// </summary>
    private void ConfigureExport()
    {
        rgReport.ExportSettings.IgnorePaging = true;
        rgReport.ExportSettings.OpenInNewWindow = true;
        rgReport.ExportSettings.ExportOnlyData = true;
        rgReport.ExportSettings.HideStructureColumns = true;

        rgReport.ExportSettings.FileName = "Csat_" + DateTime.Now.ToString("yyyyMMdd-hhmmss");

        rgReport.MasterTableView.Columns.FindByUniqueName("ChoiceNoFormatting").Visible = true;
        rgReport.MasterTableView.Columns.FindByUniqueName("Choice").Visible = false;

    }

    #endregion

    #region protected events

    #region telerik grid

    /// <summary>
    /// grid on need data source
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rgReport_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        int _currentPage = ResetSearch ? 1 : rgReport.CurrentPageIndex + 1;
        int _resultCount = 0;
        int _takeRows = rgReport.PageSize;
        int _skipRows = _takeRows * (_currentPage - 1);

        var choices = "";
        foreach (ListItem item in lbRating.Items)
        {
            if (item.Selected)
            {
                if (choices.Trim().Length > 0)
                    choices += ",";
                choices += item.Value.Trim();
            }
        }

        var _dtReport = DesktopShared.StoredProcedureCallerClasses.RetrievalProcedures.CsatSearch(ucStartDate.SelectedDate, ucEndDate.SelectedDate, choices, ddlCsatTeamMember.TeamMemberId, ddlCsatCompany.Company,
            _skipRows, _takeRows, SortColumnName, SortOperator == SortOperator.Ascending ? "asc" : "desc");

        if (_dtReport.Rows.Count > 0)
            _resultCount = Convert.ToInt32(_dtReport.Rows[0]["TotalRows"].ToString());
        
        rgReport.VirtualItemCount = _resultCount;
        rgReport.DataSource = _dtReport;
        ResetSearch = false;
    }

    /// <summary>
    /// grid on item data bound
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rgReport_ItemDataBound(object sender, GridItemEventArgs e)
    {
        #region Item or AlternatingItem

        if (e.Item.ItemType == GridItemType.Item || e.Item.ItemType == GridItemType.AlternatingItem)
        {
            GridDataItem _gdi = e.Item as GridDataItem;
            DataRowView _drv = (DataRowView)e.Item.DataItem;

            //rating
            string choice = _drv["Choice"].ToString();
            string choiceColor = "#d0021b";
            if (choice == "3")
                choiceColor = "#f5a623";
            else if (choice == "4")
                choiceColor = "#4ec677";
            else if (choice == "5")
                choiceColor = "#19853e";
            TelerikHelper.AddLabelToCell(String.Format("<span style=\"color:{0};\">{1}<span>", choiceColor, choice), choice, _gdi["Choice"]);

            //date
            string _displayValue = "";
            DateTime _date = DateTime.MinValue;
            if (DateTime.TryParse(_drv["SimplesatCreated"].ToString(), out _date))
                _displayValue = _date.ToString("g");
            TelerikHelper.AddLabelToCell(_displayValue, _gdi["SimplesatCreated"]);

            //ticket link
            var ticketId = _drv["TicketId"].ToString();
            TelerikHelper.AddHyperLinkToCell(ticketId, String.Format("/Ticket/Detail2.aspx?Id={0}", ticketId), _gdi["TicketId"]);

            TelerikHelper.AddLabelToCell(_drv["CustomerName"].ToString(), _gdi["CustomerName"]);
            TelerikHelper.AddLabelToCell(_drv["Company"].ToString(), _gdi["Company"]);
            TelerikHelper.AddLabelToCell(_drv["TeamMembers"].ToString(), _gdi["TeamMembers"]);
            TelerikHelper.AddLabelToCell(_drv["FollowUpAnswer"].ToString(), _gdi["FollowUpAnswer"]);    
        }

        #endregion
    }

    /// <summary>
    /// grid on sort command
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rgReport_SortCommand(object sender, GridSortCommandEventArgs e)
    {
        GridSortExpression sortExpression = new GridSortExpression();

        //these columns will sort descending on first click
        var _nonAscList = new List<string> { "Id" };
        bool _sortAscending = !_nonAscList.Contains(e.SortExpression.Trim());

        SortColumnName = e.SortExpression.Trim();
        switch (e.OldSortOrder)
        {
            case GridSortOrder.None:
                sortExpression.FieldName = e.SortExpression;
                sortExpression.SortOrder = _sortAscending ? GridSortOrder.Ascending : GridSortOrder.Descending;
                e.Item.OwnerTableView.SortExpressions.AddSortExpression(sortExpression);
                break;
            case GridSortOrder.Ascending:

                sortExpression.FieldName = e.SortExpression;
                sortExpression.SortOrder = _sortAscending ? GridSortOrder.Descending : GridSortOrder.None;
                e.Item.OwnerTableView.SortExpressions.AddSortExpression(sortExpression);
                break;
            case GridSortOrder.Descending:

                sortExpression.FieldName = e.SortExpression;
                sortExpression.SortOrder = _sortAscending ? GridSortOrder.None : GridSortOrder.Ascending;
                e.Item.OwnerTableView.SortExpressions.AddSortExpression(sortExpression);
                break;
        }
        SortOperator = (sortExpression.SortOrder == GridSortOrder.Ascending) ? SD.LLBLGen.Pro.ORMSupportClasses.SortOperator.Ascending : SD.LLBLGen.Pro.ORMSupportClasses.SortOperator.Descending;

        e.Canceled = true;
        rgReport.CurrentPageIndex = 0;
        rgReport.Rebind();

    }

    /// <summary>
    /// grid on pre render
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rgReport_PreRender(object sender, EventArgs e)
    {
        if (DesktopShared.Utility.BrowserHelper.IsTabletOrSmartPhone(this.Request))
        {
            int _colIndex = -1;
            
            rgReport.MasterTableView.Columns[++_colIndex].HeaderStyle.Width = Unit.Pixel(100);
            rgReport.MasterTableView.Columns[++_colIndex].HeaderStyle.Width = Unit.Pixel(150);
            rgReport.MasterTableView.Columns[++_colIndex].HeaderStyle.Width = Unit.Pixel(150);
            rgReport.MasterTableView.Columns[++_colIndex].HeaderStyle.Width = Unit.Pixel(150);
            rgReport.MasterTableView.Columns[++_colIndex].HeaderStyle.Width = Unit.Pixel(100);
            rgReport.MasterTableView.Columns[++_colIndex].HeaderStyle.Width = Unit.Pixel(100);
            rgReport.MasterTableView.Columns[++_colIndex].HeaderStyle.Width = Unit.Pixel(350);
        }
    }

    #endregion

    /// <summary>
    /// export button click
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnExport_Click(object sender, System.EventArgs e)
    {
        ConfigureExport();
        rgReport.MasterTableView.ExportToCSV(); 
    }

    /// <summary>
    /// submit button on click
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            phSearchResults.Visible = true;
            ResetSearch = true;
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
        Response.Redirect("Csat.aspx");
    }

    #region custom validators

    /// <summary>
    /// validate last backup dates
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

    #region private properties

    /// <summary>
    /// get/set sort column name
    /// </summary>
    private string SortColumnName
    {
        get
        {
            object obj = this.ViewState["scn_cb"];
            return (obj == null) ? "SimplesatCreated" : (string)obj;
        }
        set { this.ViewState["scn_cb"] = value; }
    }

    /// <summary>
    /// get/set sort operator
    /// </summary>
    private SortOperator SortOperator
    {
        get
        {
            object obj = this.ViewState["so_cb"];
            return (obj == null) ? SortOperator.Descending : (SortOperator)obj;
        }
        set { this.ViewState["so_cb"] = value; }
    }

    /// <summary>
    /// get/set reset search
    /// </summary>
    public bool ResetSearch
    {
        get
        {
            object obj = this.ViewState["rs_cb"];
            return (obj == null) ? false : (bool)obj;
        }
        set { this.ViewState["rs_cb"] = value; }
    }

    #endregion
}

