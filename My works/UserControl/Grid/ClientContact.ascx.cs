using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using SD.LLBLGen.Pro.ORMSupportClasses;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Telerik.Web.UI;

public partial class UserControl_Grid_ClientContact : System.Web.UI.UserControl
{
    bool _isExport = false;

    protected void Page_Load(object sender, EventArgs e)
    {
        DropDownList ddlRecordsPerPage = ucRecordsPerPage.GetDropDownList();
        ddlRecordsPerPage.SelectedIndexChanged += new EventHandler(ddlRecordsPerPage_SelectedChange);

        if (!this.IsPostBack)
        {
            SetShowHide();
            SetUpRecordsPerPage(); //set value for drop down list control for number of records to display
        }
    }

    /// <summary>
    /// reset grid
    /// </summary>
    public void ResetGrid()
    {
        rgClientContact.Visible = true;
        
        rgClientContact.EditIndexes.Clear();
        rgClientContact.DataSource = null;
        rgClientContact.Rebind();
    }

    public string SearchId
    {
        get
        {
            object obj = this.ViewState["SearchIdForClientContact"];
            if (obj == null)
                return "";
            else
                return (string)obj;
        }
        set { this.ViewState["SearchIdForClientContact"] = value; }
    }
   
    public string SearchFirst
    {
        get
        {
            object obj = this.ViewState["SearchFirstForClientContact"];
            if (obj == null)
                return "";
            else
                return (string)obj;
        }
        set { this.ViewState["SearchFirstForClientContact"] = value; }
    }

    public string SearchLast
    {
        get
        {
            object obj = this.ViewState["SearchLastForClientContact"];
            if (obj == null)
                return "";
            else
                return (string)obj;
        }
        set { this.ViewState["SearchLastForClientContact"] = value; }
    }

    public bool? SearchTypeClient
    {
        get
        {
            object obj = this.ViewState["SearchTypeClient"];
            if (obj == null)
                return false;
            else
                return (bool)obj;
        }
        set { this.ViewState["SearchTypeClient"] = value; }
    }
    public bool? SearchTypeGeneral
            {
        get
        {
            object obj = this.ViewState["SearchTypeGeneral"];
            if (obj == null)
                return false;
            else
                return (bool)obj;
        }
                set { this.ViewState["SearchTypeGeneral"] = value; }
    }

     public bool? SearchTypeVendor
    {
        get
        {
            object obj = this.ViewState["SearchTypeVendor"];
            if (obj == null)
                return false;
            else
                return (bool)obj;
        }
        set { this.ViewState["SearchTypeVendor"] = value; }
    }

    public string SearchEmail
    {
        get
        {
            object obj = this.ViewState["SearchEmailForClientContact"];
            if (obj == null)
                return "";
            else
                return (string)obj;
        }
        set { this.ViewState["SearchEmailForClientContact"] = value; }

    }
    public string SearchPhone
    {
        get
        {
            object obj = this.ViewState["SearchPhoneForClientContact"];
            if (obj == null)
                return "";
            else
                return (string)obj;
        }
        set { this.ViewState["SearchPhoneForClientContact"] = value; }

    }
    public bool? SearchPriority
    {
        get
        {
            object obj = this.ViewState["SearchPriority"];
            if (obj == null)
                return false;
            else
                return (bool)obj;
        }
        set { this.ViewState["SearchPriority"] = value; }
    }
    public string SearchRectype
    {
        get
        {
            object obj = this.ViewState["SearchRectypeForClientContact"];
            if (obj == null)
                return "";
            else
                return (string)obj;
        }
        set { this.ViewState["SearchRectypeForClientContact"] = value; }

    }
    protected void Page_PreRender(object o, EventArgs e)
    {
       GridCommandItem commandItem = null;
       if (rgClientContact.MasterTableView.Items.Count > 0)
       {
           commandItem = (GridCommandItem)rgClientContact.MasterTableView.GetItems(GridItemType.CommandItem)[0];
           commandItem.Visible = true;
       }
    }

    protected void rgClientContact_InsertCommand(object source, GridCommandEventArgs e)
    {
        if (Page.IsValid)
        {
           AbstractClientContactEdit ucClientContactEdit = e.Item.FindControl
                    (GridEditFormItem.EditFormUserControlID) as AbstractClientContactEdit;

            ucClientContactEdit.SaveValues(0);
            rnContactUpdate.Text = "Contact has been added " + DateTime.Now.ToString();
            rnContactUpdate.Show();
            
        }
    }
    protected void rgClientContact_UpdateCommand(object source, GridCommandEventArgs e)
    {
        if (Page.IsValid)
        {
            AbstractClientContactEdit ucClientContactEdit = e.Item.FindControl
                (GridEditFormItem.EditFormUserControlID) as AbstractClientContactEdit;

            GridEditableItem editedItem = e.Item as GridEditableItem;
            int Id = Convert.ToInt32(editedItem.OwnerTableView.DataKeyValues
                   [editedItem.ItemIndex]["PclientContact"]);
            ucClientContactEdit.SaveValues(Id);
            rnContactUpdate.Text = "Contact has been updated " + DateTime.Now.ToString();
            rnContactUpdate.Show();

        }
    }
    protected void rgClientContact_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        // e.Row.Cells[2] is assumed to be hyperlink column cell
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            HyperLink hlControl = new HyperLink();
            hlControl.Text = e.Row.Cells[4].Text;
            hlControl.NavigateUrl = "/UserControl/Grid/EditForm/ClientContactEdit.ascx";

            e.Row.Cells[4].Controls.Add(hlControl);
        }
    }

    protected void rgClientContact_PreRender(object sender, EventArgs e)
    {
        #region default - show header, footer, pager.  set no records text

        rgClientContact.ShowHeader = true;
        rgClientContact.ShowFooter = true;

        rgClientContact.PagerStyle.AlwaysVisible = true;
        rgClientContact.PagerStyle.Visible = true;

        rgClientContact.MasterTableView.NoMasterRecordsText = "No records found.";

        #endregion

        GridCommandItem commandItem = null; 

        if (rgClientContact.MasterTableView.Items.Count > 0)
        {
            commandItem = (GridCommandItem)rgClientContact.MasterTableView.GetItems(GridItemType.CommandItem)[0];
            commandItem.Visible = true;
        }

        #region edit or add mode


        if (rgClientContact.EditItems.Count > 0 || rgClientContact.MasterTableView.IsItemInserted)
        {
            foreach (GridDataItem item in rgClientContact.MasterTableView.Items)
            {
                item.Visible = false; //hide all rows not being edited
            }

            rgClientContact.ShowHeader = false; //hide grid header 
            rgClientContact.ShowFooter = false; //hide grid footer

            //hide grid pager
            rgClientContact.PagerStyle.AlwaysVisible = false;
            rgClientContact.PagerStyle.Visible = false;

            if (commandItem != null)
                commandItem.Visible = false;
        }

        #endregion

    }
    protected void rgClientContact_ItemCreated(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        #region command item -> hide for export

        if (e.Item is GridCommandItem && _isExport)
            e.Item.Display = false;

        #endregion

    }

    /// <summary>
    /// get client id from query string
    /// </summary>
    private void GetClientId()
    {
        if (!String.IsNullOrEmpty(BitByBit.Web.Request.GetString("ClientId")))
        {
            try { ClientId = Convert.ToInt32(BitByBit.Web.Request.GetString("ClientId")); }
            catch { ClientId = 0; }
        }
    }

     /// <summary>
    /// get/set client id (stored in viewstate)
    /// </summary>
    public int? ClientId
    {
        get
        {
            object obj = this.ViewState["ClientIdForBackupSet"];
            return (obj == null) ? (int?)null : (int)obj;
        }
        set { this.ViewState["ClientIdForBackupSet"] = value; }
    }

    protected void rgClientContact_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        #region GridEditFormItem and IsInEditMode

        if (e.Item is GridEditFormItem && e.Item.IsInEditMode)
        {
            rgClientContact.MasterTableView.NoMasterRecordsText = "";

            AbstractClientContactEdit ucClientContactEdit = e.Item.FindControl
                (GridEditFormItem.EditFormUserControlID) as AbstractClientContactEdit;

            #region edit existing

            if (!e.Item.OwnerTableView.IsItemInserted)
            {
                System.Data.DataRowView _ClientCont = (System.Data.DataRowView)e.Item.DataItem;
                ucClientContactEdit.LoadValues(_ClientCont);
            }

            #endregion

            #region add new item

            else
            {
                if (SearchClientId.HasValue) //default client id
                    ucClientContactEdit.FkClient= SearchClientId.Value;

                ucClientContactEdit.LoadValues(0);
            }
            #endregion
        }
    }

    protected void rgClientContact_DeleteCommand(object source, GridCommandEventArgs e)
    {
        int Id = Convert.ToInt32((e.Item as GridDataItem).OwnerTableView.DataKeyValues
            [e.Item.ItemIndex]["PclientContact"]);

        DesktopShared.EntityClasses.ClientContactEntity clientcont = new DesktopShared.EntityClasses.ClientContactEntity(Id);
        
        clientcont.Delete();
    }

    protected void ddlRecordsPerPage_SelectedChange(object sender, EventArgs e)
    {
        TicketsPerPage = ucRecordsPerPage.SelectedValue;
        rgClientContact.Rebind();
    }
    public int? SearchClientId
    {
        get
        {
            object obj = this.ViewState["SearchClientIdForClientContact"];
            if (obj == null)
                return null;
            else
                return (int)obj;
        }
        set { this.ViewState["SearchClientIdForClientContact"] = value; }
    }
    public bool? SearchActiveStatus
    {
        get
        {
            object obj = this.ViewState["SearchActiveStatus"];
            if (obj == null)
                return null;
            else
                return (bool)obj;
        }
        set { this.ViewState["SearchActiveStatus"] = value; }
    }

    protected void rgClientContact_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        DataTable dtContact = SearchActiveContacts(SearchId,  SearchFirst, SearchLast, SearchPhone, SearchEmail,
             SearchRectype, SearchActiveStatus, SearchPriority, SearchTypeClient, SearchTypeGeneral, SearchTypeVendor, SearchClientId); 
        rgClientContact.DataSource = dtContact;

        #region paging

        if (TicketsPerPage > 0)
            rgClientContact.PageSize = TicketsPerPage;
        else
        {
            if (dtContact.Rows.Count > 0)
                rgClientContact.PageSize = dtContact.Rows.Count;
        }

        #endregion
    }

    public DataTable  SearchActiveContacts(string Id,  string First, string Last, string Phone,
         string Email, string Rectype, bool? Active, bool? Priority, bool? TypeClient, bool? TypeGeneral, bool? TypeVendor, int? clientContactId = null)
    {
        DataTable dynamicList = new DataTable();
        DesktopShared.DaoClasses.TypedListDAO dao = new DesktopShared.DaoClasses.TypedListDAO();

        #region result set fields

        DesktopShared.HelperClasses.ResultsetFields fields = new DesktopShared.HelperClasses.ResultsetFields(32);

        fields.DefineField(DesktopShared.HelperClasses.UsersFields.Pusers, 0, "Pusers");
        fields.DefineField(DesktopShared.HelperClasses.UsersFields.Last, 1, "LastName");
        fields.DefineField(DesktopShared.HelperClasses.UsersFields.First, 2, "FirstName");
        fields.DefineField(DesktopShared.HelperClasses.UsersFields.Email, 3, "Email");
        fields.DefineField(DesktopShared.HelperClasses.UsersFields.Last, 4, "FullName");
        fields.DefineField(DesktopShared.HelperClasses.UsersFields.Last, 5, "FullNameWithComma");
        fields.DefineField(DesktopShared.HelperClasses.UsersFields.Last, 6, "FullNameWithCommaTrimmed");
        fields.DefineField(DesktopShared.HelperClasses.UsersFields.UseHelpDesk, 7, "UseHelpDesk");
        fields.DefineField(DesktopShared.HelperClasses.ClientContactFields.PclientContact, 8, "PclientContact");
        fields.DefineField(DesktopShared.HelperClasses.ClientContactFields.FkClient, 9, "FkClient");
        fields.DefineField(DesktopShared.HelperClasses.ClientContactFields.FkEmployee, 10, "EmployeeId");
        fields.DefineField(DesktopShared.HelperClasses.ClientFields.Company, 11, "Company");
        fields.DefineField(DesktopShared.HelperClasses.ClientContactFields.Email2, 12, "Email2");
        fields.DefineField(DesktopShared.HelperClasses.ClientContactFields.Email3, 13, "Email3");
        fields.DefineField(DesktopShared.HelperClasses.ClientContactFields.Addr1, 14, "Addr1");
        fields.DefineField(DesktopShared.HelperClasses.ClientContactFields.Addr2, 15, "Addr2");
        fields.DefineField(DesktopShared.HelperClasses.ClientContactFields.Busphone, 16, "Busphone");
        fields.DefineField(DesktopShared.HelperClasses.ClientContactFields.Faxphone, 17, "Faxphone");
        fields.DefineField(DesktopShared.HelperClasses.ClientContactFields.Cellphone, 18, "Cellphone");
        fields.DefineField(DesktopShared.HelperClasses.ClientContactFields.City, 19, "City");
        fields.DefineField(DesktopShared.HelperClasses.ClientContactFields.Zip, 20, "Zip");
        fields.DefineField(DesktopShared.HelperClasses.ClientContactFields.Password, 21, "Password");
        fields.DefineField(DesktopShared.HelperClasses.ClientContactFields.State, 22, "State");
        fields.DefineField(DesktopShared.HelperClasses.ClientContactFields.Busext, 23, "Busext");
        fields.DefineField(DesktopShared.HelperClasses.ClientContactFields.Id, 24, "Id");
        fields.DefineField(DesktopShared.HelperClasses.ClientFields.Code, 25, "Code");
        fields.DefineField(DesktopShared.HelperClasses.ClientFields.Priority, 26, "Priority");
        fields.DefineField(DesktopShared.HelperClasses.ClientFields.Rectype, 27, "Rectype");
        fields.DefineField(DesktopShared.HelperClasses.ClientFields.Active, 28, "Active");
        fields.DefineField(DesktopShared.HelperClasses.ClientContactFields.Created, 29, "Created");
        fields.DefineField(DesktopShared.HelperClasses.ClientContactFields.LastUpdated, 30, "LastUpdated");
        fields.DefineField(DesktopShared.HelperClasses.ClientContactFields.LastUpdatedby, 31, "LastUpdatedBy");
        #endregion

        #region relations

        IRelationCollection relations = new RelationCollection();

        //users -> client contact
        IEntityRelation relation = new EntityRelation(RelationType.OneToOne);
        relation.AddEntityFieldPair(DesktopShared.FactoryClasses.EntityFieldFactory.Create(
           DesktopShared.UsersFieldIndex.FkClientContact),
           DesktopShared.FactoryClasses.EntityFieldFactory.Create(
           DesktopShared.ClientContactFieldIndex.PclientContact));
        relations.Add(relation, JoinHint.Left);

        //client contact -> client
        relation = new EntityRelation(RelationType.OneToOne);
        relation.AddEntityFieldPair(DesktopShared.FactoryClasses.EntityFieldFactory.Create(
           DesktopShared.ClientContactFieldIndex.FkClient),
           DesktopShared.FactoryClasses.EntityFieldFactory.Create(
           DesktopShared.ClientFieldIndex.Pclient));
        relations.Add(relation, JoinHint.Left);

        #endregion

        #region filter
        
        IPredicateExpression userFilter = new PredicateExpression();
        if (clientContactId.HasValue)
            userFilter.Add(DesktopShared.HelperClasses.ClientContactFields.FkClient == clientContactId.Value);
        

            if (!String.IsNullOrEmpty(Id))
                userFilter.Add(DesktopShared.HelperClasses.ClientContactFields.PclientContact == Id);

            if (Priority.HasValue && Priority.Value)
                userFilter.Add(DesktopShared.HelperClasses.ClientFields.Priority == Priority.Value);

            List<string> _recordTypes = new List<string>();


            if (TypeClient.HasValue && TypeClient.Value)
                _recordTypes.Add("C");

            if (TypeGeneral.HasValue && TypeGeneral.Value)
                _recordTypes.Add("G");

            if (TypeVendor.HasValue && TypeVendor.Value)
                _recordTypes.Add("V");

             userFilter.Add(DesktopShared.HelperClasses.ClientFields.Rectype == _recordTypes);

           if (SearchActiveStatus.HasValue)
              {
                    string _active = SearchActiveStatus.Value ? "Y" : "N";
                    userFilter.Add(DesktopShared.HelperClasses.ClientContactFields.Active == _active);
              }

           if (Phone.Trim().Length > 0)
           {
               string phoneLike = "%" + Phone.Trim() + "%";
               userFilter.Add(DesktopShared.HelperClasses.ClientContactFields.Busphone % phoneLike);
           }

           if (First.Trim().Length > 0)
           {
               string firstLike = "%" + First.Trim() + "%";
               userFilter.Add(DesktopShared.HelperClasses.UsersFields.First % firstLike);
           }

           if (Last.Trim().Length > 0)
           {
               string lastLike = "%" + Last.Trim() + "%";
               userFilter.Add(DesktopShared.HelperClasses.UsersFields.Last % lastLike);
           }
           if (Email.Trim().Length > 0)
           {
               string emailLike = "%" + Email.Trim() + "%";
               userFilter.Add(DesktopShared.HelperClasses.UsersFields.Email % emailLike);
           }

        #endregion

        #region sort
        ISortExpression userSort = new SD.LLBLGen.Pro.ORMSupportClasses.SortExpression();
        userSort.Add(DesktopShared.HelperClasses.UsersFields.Last | SortOperator.Ascending);
        #endregion
        dao.GetMultiAsDataTable(fields, dynamicList, 0, userSort, userFilter, relations,
            false, null, null, 0, 0);

        return dynamicList;
    }
    private int TicketsPerPage
    {
        get
        {
            object obj = this.ViewState["ContactsPerPage"];
            if (obj == null)
                return 25;
            else
                return (int)obj;
        }

        set
        {
            this.ViewState["ContactsPerPage"] = value;
        }
    }

    private void SetUpRecordsPerPage()
    {
        ucRecordsPerPage.SelectedValue = TicketsPerPage;
       
    }
    public bool SetShowHideDone
    {
        get
        {
            object obj = this.ViewState["SetShowHideDoneForTimesheet"];
            if (obj == null)
                return false;
            else
                return (bool)obj;
        }

        set
        {
            this.ViewState["SetShowHideDoneForTimesheet"] = value;
        }
    }
    private void SetShowHide()
    {
        if (!SetShowHideDone)
        {
            string _linkClass = "hide";
            string _divDisplay = "";

            if (HideGrid)
            {
                _linkClass = "show";
                _divDisplay = "none";
            }

            hlShowHide.Attributes.Add("href", "#");
            hlShowHide.Attributes.Add("class", _linkClass);

            string _onClickFunction = "showHideInfo(this, '" + rgClientContact.ClientID + "');";
            _onClickFunction += "return false;";
            hlShowHide.Attributes.Add("onclick", _onClickFunction);

            rgClientContact.Attributes.CssStyle.Add("display", _divDisplay);

            SetShowHideDone = true;
        }
    }
    public bool HideGrid
    {
        get
        {
            object obj = this.ViewState["HideGridForTimesheet"];
            if (obj == null)
                return false;
            else
                return (bool)obj;
        }

        set
        {
            this.ViewState["HideGridForTimesheet"] = value;
        }
    }

    #endregion

    public GridHeaderButtonType HeaderButtonType {
        get;
        set; 
    }
}
