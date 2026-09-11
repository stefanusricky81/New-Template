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
using Telerik.Web.UI;
using BitByBit;
using SD.LLBLGen.Pro.ORMSupportClasses;
using DesktopShared;
using System.Collections.Generic;

public partial class Maintenance_NewClientContactSearch : System.Web.UI.Page
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
            DesktopShared.EntityClasses.ClientEmailCategoryEntity objClientEmail = null;
            if (!GetEmailCategorytId(ref objClientEmail))
                return;
            else
                litMessage.Visible = false;

            SetUpPage();
        }
    }

    #region private methods

    private bool GetEmailCategorytId(ref DesktopShared.EntityClasses.ClientEmailCategoryEntity objClientEmail)
    {
        int _ecat = -1;
        if (int.TryParse(BitByBit.Web.Request.GetString("ecat").Trim(), out _ecat))
        {
            EmailCategoryId = _ecat;
            return true;
        }

        DesktopShared.Bootstrap.Alert.DisplayMessage(litMessage, String.Format("Unable to Email Category.  ID = {0}", _ecat > 0 ? _ecat.ToString() : "N/A"), DesktopShared.Bootstrap.Alert.AlertType.Danger, false);
        pnlContainer.Visible = false;
        return false;
    }

    /// <summary>
    /// set up for initial page load
    /// </summary>
    private void SetUpPage()
    {
        txtCompany.Focus();

        short _tabIndex = 0;
        txtCompany.TabIndex = ++_tabIndex;
        txtFirst.TabIndex = ++_tabIndex;
        txtLast.TabIndex = ++_tabIndex;
        txtEmail.TabIndex = ++_tabIndex;
        txtCode.TabIndex = ++_tabIndex;
        ddlPriority.TabIndex = ++_tabIndex;
        ddlStatus.TabIndex = ++_tabIndex;
        btnSubmit.TabIndex = ++_tabIndex;
        btnClear.TabIndex = ++_tabIndex;

        BindPriorityDropDown();

        ddlClientEmailCategory.ClientEmailCategoryId = EmailCategoryId;
        ddlClientEmailCategory.IsEnabled = false;

        lbltitle.Text = ddlClientEmailCategory.ClientEmailCategoryName;
        lblAssign.Text = ddlClientEmailCategory.ClientEmailCategoryName;
    }

    /// <summary>
    /// configure for device
    /// </summary>
    private void ConfigureForDevice()
    {
        if (DesktopShared.Utility.BrowserHelper.IsTabletOrSmartPhone(this.Request))
        {
            rgClientContact.RenderMode = RenderMode.Lightweight;
            ddlPriority.CssClass = "form-control";
            ddlStatus.CssClass = "form-control";
        }
    }

    /// <summary>
    /// bind priority drop down list
    /// </summary>
    private void BindPriorityDropDown()
    {
        //clear items
        ddlPriority.ClearSelection();
        ddlPriority.Items.Clear();

        //get collection
        PredicateExpression priorityFilter = new PredicateExpression();
        priorityFilter.Add(DesktopShared.HelperClasses.SupportingTableFields.TableType == "ClientRanking");

        ISortExpression prioritySort = new SD.LLBLGen.Pro.ORMSupportClasses.SortExpression();
        prioritySort.Add(DesktopShared.HelperClasses.SupportingTableFields.Description | SortOperator.Ascending);

        var res = DesktopShared.CollectionClasses.SupportingTableCollection.GetMultiAsDataTable(priorityFilter, 0, prioritySort);

        //bind
        ddlPriority.DataSource = res;
        ddlPriority.DataTextField = "description";
        ddlPriority.DataValueField = "psupportingtable";
        ddlPriority.DataBind();
        ddlPriority.DataSource = null;

        //insert default item
        ListItem liDefault = new ListItem("Select Priority", "");
        ddlPriority.Items.Insert(0, liDefault);
    }

    /// <summary>
    /// display message
    /// </summary>
    /// <param name="message"></param>
    /// <param name="alertType"></param>
    /// <param name="overrideHeader"></param>
    private void DisplayMessage(string message, Bootstrap.Alert.AlertType alertType, bool dismissable = true, string overrideHeader = "")
    {
        ScriptManager.RegisterClientScriptBlock(this, Page.GetType(), "ToTheTop", "ToTopOfPage();", true);
        litMessage.Visible = true;
        litMessage.Text = Bootstrap.Alert.GetDivText(alertType, message, dismissable, true, overrideHeader);
    }

    /// <summary>
    /// rebind document grid
    /// </summary>
    private void RebindGrid()
    {
        phSearchResults.Visible = true;
        rgClientContact.Visible = true;
        rgClientContact.EditIndexes.Clear();
        rgClientContact.DataSource = null;
        rgClientContact.Rebind();
    }

    #endregion

    #region protected events

    #region grid

    /// <summary>
    /// grid on need data source
    /// </summary>
    /// <param name="source"></param>
    /// <param name="e"></param>
    protected void rgClientContact_NeedDataSource(object source, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        int _currentPage = ResetSearch ? 1 : rgClientContact.CurrentPageIndex + 1;
        int _resultCount = 0;
        int _resultsPerPage = rgClientContact.PageSize;

        bool _success = false;
        string _errorMessage = "";

        string _active = string.Empty;
        if (!String.IsNullOrWhiteSpace(ddlStatus.SelectedValue.Trim()))
            _active = ddlStatus.SelectedValue.Trim() == "1" ? "Y" : "N";

        int? _userRankingId = null;
        if (!String.IsNullOrWhiteSpace(ddlPriority.SelectedValue))
            _userRankingId = Convert.ToInt32(ddlPriority.SelectedValue.Trim());

        DataTable _contacts = DesktopShared.StoredProcedureCallerClasses.RetrievalProcedures.ProcGetNewClientContact(
            txtCompany.Text.Trim(), txtFirst.Text.Trim(), txtLast.Text.Trim(), txtEmail.Text.Trim(), txtCode.Text.Trim(), 
            _userRankingId,
            _active //client is active
            );
        if (_contacts != null)
            _success = true;
        //var _contacts = DesktopShared.Client.Contact.Search(
        //    ref _success, //success / no errors
        //    ref _resultCount, //results count
        //    ref _errorMessage, //errror message if unsuccessful
        //    txtCompany.Text.Trim(), //company name
        //    txtCode.Text.Trim(), //company code
        //    txtEmail.Text.Trim(), //contact email
        //    txtLast.Text.Trim(), //contact last name
        //    txtFirst.Text.Trim(), //contact first name
        //    _active, //client is active
        //    _userRankingId, //user ranking id
        //    _currentPage, //page number
        //    _resultsPerPage, //page size
        //    SortColumnName, //sort column name
        //    SortOperator //sort operator
        //    );

        if (_success)
        {
            rgClientContact.VirtualItemCount = _contacts.Rows.Count;
            rgClientContact.DataSource = _contacts;
            ResetSearch = false;
        }
        else
        {
            phSearchResults.Visible = false;
            DisplayMessage(String.Format("An error has occured - {0}", _errorMessage), Bootstrap.Alert.AlertType.Danger);
        }

    }

    /// <summary>
    /// grid on pre render
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rgClientContact_PreRender(object sender, EventArgs e)
    {
        if (DesktopShared.Utility.BrowserHelper.IsTabletOrSmartPhone(this.Request))
        {
            int _colIndex = -1;
            rgClientContact.MasterTableView.Columns[++_colIndex].HeaderStyle.Width = Unit.Pixel(75);
            rgClientContact.MasterTableView.Columns[++_colIndex].HeaderStyle.Width = Unit.Pixel(75);
            rgClientContact.MasterTableView.Columns[++_colIndex].HeaderStyle.Width = Unit.Pixel(275);
            rgClientContact.MasterTableView.Columns[++_colIndex].HeaderStyle.Width = Unit.Pixel(125);
            rgClientContact.MasterTableView.Columns[++_colIndex].HeaderStyle.Width = Unit.Pixel(125);
            rgClientContact.MasterTableView.Columns[++_colIndex].HeaderStyle.Width = Unit.Pixel(200);
            rgClientContact.MasterTableView.Columns[++_colIndex].HeaderStyle.Width = Unit.Pixel(125);
            rgClientContact.MasterTableView.Columns[++_colIndex].HeaderStyle.Width = Unit.Pixel(125);
            rgClientContact.MasterTableView.Columns[++_colIndex].HeaderStyle.Width = Unit.Pixel(75);
        }
    }

    /// <summary>
    /// grid on item data bound
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rgClientContact_ItemDataBound(object sender, GridItemEventArgs e)
    {
        #region Item or AlternatingItem

        if (e.Item.ItemType == GridItemType.Item || e.Item.ItemType == GridItemType.AlternatingItem)
        {
            DataRowView _row = (DataRowView)e.Item.DataItem;
            GridDataItem _gdi = e.Item as GridDataItem;

            string _email = _row["Email"].ToString().Trim();
            string _emailLink = _email;
            if (DesktopShared.Utility.IsValidEmailAddress(_email))
                _emailLink = String.Format("<a href=\"mailto:{0}\">{0}</a>", _email);

            DesktopShared.Utility.Grid.TelerikHelper.AddLabelToCell(_row["UserRanking"].ToString().Trim(), _gdi["UserRanking"]);
            DesktopShared.Utility.Grid.TelerikHelper.AddLabelToCell(_row["Company"].ToString().Trim(), _gdi["Company"]);
            DesktopShared.Utility.Grid.TelerikHelper.AddLabelToCell(_row["First"].ToString().Trim(), _gdi["First"]);
            DesktopShared.Utility.Grid.TelerikHelper.AddLabelToCell(_row["Last"].ToString().Trim(), _gdi["Last"]);
            DesktopShared.Utility.Grid.TelerikHelper.AddLabelToCell(_emailLink, _email, _gdi["Email"]);
            DesktopShared.Utility.Grid.TelerikHelper.AddLabelToCell(_row["BusPhone"].ToString().Trim(), _gdi["BusPhone"]);
            DesktopShared.Utility.Grid.TelerikHelper.AddLabelToCell(_row["Cellphone"].ToString().Trim(), _gdi["Cellphone"]);
            DesktopShared.Utility.Grid.TelerikHelper.AddLabelToCell(_row["Active"].ToString().Trim(), _gdi["Active"]);

        }

        #endregion
        /*
         * <telerik:GridBoundColumn DataField="Code" HeaderStyle-Width="8%" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderText="Code" SortExpression="Code" UniqueName="Code" />
         * <telerik:GridBoundColumn DataField="Title" HeaderStyle-Width="10%" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderText="Title" SortExpression="Title" UniqueName="Title" />
         * <telerik:GridBoundColumn DataField="ContactHomePhone" HeaderStyle-Width="10%" DataType="System.String" ItemStyle-HorizontalAlign="Left" HeaderText="Contact Phone" SortExpression="ContactHomePhone" UniqueName="ContactHomePhone" />
        */
    }

    /// <summary>
    /// grid on sort command
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rgClientContact_SortCommand(object sender, GridSortCommandEventArgs e)
    {
        GridSortExpression sortExpression = new GridSortExpression();

        //these columns will sort descending on first click
        var _nonAscList = new List<string> { "none" };
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
        rgClientContact.CurrentPageIndex = 0;
        rgClientContact.Rebind();

        if (!e.Item.OwnerTableView.SortExpressions.ContainsExpression(e.SortExpression))
        {
            sortExpression = new GridSortExpression();
            sortExpression.FieldName = e.SortExpression;
            sortExpression.SortOrder = GridSortOrder.Ascending;
            e.Item.OwnerTableView.SortExpressions.AddSortExpression(sortExpression);
        }
    }

    #endregion

    /// <summary>
    /// submit button on click
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        ResetSearch = true;
        RebindGrid();
    }

    /// <summary>
    /// clear button on click
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnClear_Click(object sender, EventArgs e)
    {
        Response.Redirect(Request.RawUrl);
    }

    #endregion

    #region private properties

    /// <summary>
    /// get/set sort column name
    /// </summary>
    private string SortColumnName
    {
        get
        {
            object obj = this.ViewState["scn_ccs"];
            return (obj == null) ? "" : (string)obj;
        }
        set { this.ViewState["scn_ccs"] = value; }
    }

    /// <summary>
    /// get/set sort operator
    /// </summary>
    private SD.LLBLGen.Pro.ORMSupportClasses.SortOperator SortOperator
    {
        get
        {
            object obj = this.ViewState["so_ccs"];
            return (obj == null) ? SD.LLBLGen.Pro.ORMSupportClasses.SortOperator.Descending : (SD.LLBLGen.Pro.ORMSupportClasses.SortOperator)obj;
        }
        set { this.ViewState["so_ccs"] = value; }
    }

    /// <summary>
    /// get/set reset search
    /// </summary>
    private bool ResetSearch
    {
        get
        {
            object obj = this.ViewState["rs_ccs"];
            return (obj == null) ? false : (bool)obj;
        }
        set { this.ViewState["rs_ccs"] = value; }

    }

    private int? EmailCategoryId
    {
        get
        {
            object obj = this.ViewState["emcat"];
            return (obj == null) ? (int?)null : (int)obj;
        }
        set { this.ViewState["emcat"] = value; }
    }
    #endregion

    protected void lbAssign_Click(object sender, EventArgs e)
    {
        int _count=0;
        try
        {
            foreach (GridDataItem _assign in rgClientContact.MasterTableView.Items)
            {
                CheckBox chk = (CheckBox)_assign.FindControl("cboxSelect");
                if (chk.Checked == true)
                {
                    int Id = (int)_assign.GetDataKeyValue("Id");
                    int ClientId = Convert.ToInt32(_assign["ClientId"].Text.ToString().Trim());

                    DesktopShared.EntityClasses.ClientCategoriesEmailEntity emailcate = new DesktopShared.EntityClasses.ClientCategoriesEmailEntity();

                    emailcate.ClientId = ClientId;
                    emailcate.ClientContactId = Id;
                    emailcate.EmailCategoryId = (int)EmailCategoryId;
                    emailcate.Created = DateTime.Now;
                    emailcate.Active = "Y";
                    emailcate.CreatedByUserId = DesktopShared.User.UserID;

                    emailcate.Save();

                    //DesktopShared.EntityClasses.ClientContactEntity cce = new DesktopShared.EntityClasses.ClientContactEntity(Id);

                    //cce.FkClientemailcategory= (int)EmailCategoryId;
                    //cce.LastUpdated = DateTime.Now.ToString();
                    //cce.LastUpdatedby= DesktopShared.User.UserName.ToString().Trim();
                    //cce.Save();


                    _count++;
                }
            }
            DisplayMessage(String.Format("Succesfully Assign {0} to the {1}", _count, CategoryEmailName()), Bootstrap.Alert.AlertType.Success);

            rgClientContact.Rebind();
        }
        catch (Exception ex)
        {
            DisplayMessage(ex.Message, Bootstrap.Alert.AlertType.Danger);
        }
    }

    private string CategoryEmailName()
    {
        try {
            DesktopShared.CollectionClasses.ClientEmailCategoryCollection emailCat = new DesktopShared.CollectionClasses.ClientEmailCategoryCollection();
            IPredicateExpression expression = new PredicateExpression();
            expression.Add(DesktopShared.HelperClasses.ClientEmailCategoryFields.Id == EmailCategoryId);
            emailCat.GetMulti(expression);
            return emailCat[0].CategoryName;
        }
        catch
        {
            return string.Empty;
        }
    }
}