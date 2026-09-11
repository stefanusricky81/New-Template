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
using System.Web.UI.WebControls.WebParts;
using Telerik.Web.UI;

public partial class Client_ClientAdd : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Redirect("/Default.aspx");
    }

    public int SaveValues(int ClientID)
    {
        DesktopShared.EntityClasses.ClientEntity Client = null;
        int _employeeId = DesktopShared.User.EmployeeID;

        Client = new DesktopShared.EntityClasses.ClientEntity();

        Client.Clientcode = ClientCode;
        Client.Company = Company;
        Client.Busphone = Phone; 
        Client.Faxphone = Fax;
        Client.Addr1 = Address1;
        Client.Addr2 = Address2;
        Client.City = City;
        Client.State = State;
        Client.Zip = Zip;
        Client.Password = Password;
        Client.Rectype = Rectype;
        Client.Id = RecId;
        Client.FkProjmanager = FkProjmanager;
        Client.FkLeadtech = FkLeadTech;
        Client.Url = Url;
        Client.AutoCcupdate = AutoCcupdate;
        Client.Id = RecId;
        Client.Bilrate = HourlyRate;
        Client.FkRanking = Ranking;
        Client.AcctComments = AccountComments;
        Client.ProspectorVendor = LeadSystemVendor;
        if (chkPremiumSupport.Checked)
            Client.PremiumSupport = PremiumSupport;
        if (cbBackupEnabled.Checked)
            Client.Backupenabled = BackupEnabled;
        if (cbPriorityClient.Checked)
            Client.Priority = Priority;
        if (chLSV.Checked)
            Client.Fordelivery = VendorForDelivery;
        if (chBBBCSC.Checked)
            Client.Bbbcsc = BBBCSC;
        if (ch3B.Checked)
            Client.Threebcsc = threebcsc;
        Client.Active = Active;
        Client.Notes = Notes;
        Client.Directions = Directions;
        Client.WhatWeCanDoForThem = WhatWeCanDoForThem;
        Client.WhatWeDoForThem = WhatWeDoForThem;
        Client.Save();

        DesktopShared.EntityClasses.ClientEmployeesalesEntity EmployeeSales = null;
        EmployeeSales = new DesktopShared.EntityClasses.ClientEmployeesalesEntity();

        //EmployeeSales.FkEmployee = SalesPerson;

        return Client.Pclient;
    }

    public string ClientCode
    {
        get { return tbClientCode.Text; }
        set { tbClientCode.Text = value; }
    }
    public string Company
    {
        get { return tbCompany.Text; }
        set { tbCompany.Text = value; }
    }
    public string AcctStatus
    {
        get { return ddlStatus.SelectedValue; }
        set { ddlStatus.SelectedValue = value; }
    }
    
    public string Phone
    {
        get { return tbPhone.Text; }
        set { tbPhone.Text = value; }
    }
    public string Fax
    {
        get { return tbFax.Text; }
        set { tbFax.Text = value; }
    }     

    public string Address1
    {
        get { return tbAddress1.Text; }
        set { tbAddress1.Text = value; }
    }
    public string Address2
    {
        get { return tbAddress2.Text; }
        set { tbAddress2.Text = value; }
    }
    public string City
    {
        get { return tbCity.Text; }
        set { tbCity.Text = value; }
    }
    public string State
    {
        get { return ddlState.SelectedValue; }
        set { ddlState.SelectedValue = value; }
    }
    public string Zip
    {
        get { return tbZip.Text; }
        set { tbZip.Text = value; }
    }
    public string Password
    {
        get { return tbPasswordCode.Text; }
        set { tbPasswordCode.Text = value; }
    }

    public string RecId
    {
        get { return tbRecId.Text; }
        set { tbRecId.Text = value; }
    }

    public string AutoCcupdate
    {
        get { return tbTicketUpdate.Text; }
        set { tbTicketUpdate.Text = value; }
    }

    public string Url
    {
        get { return tbUrl.Text; }
        set { tbUrl.Text = value; }
    }

    public string AccountComments
    {
        get { return tbAcctComments.Text; }
        set { tbAcctComments.Text = value; }
    }

    public string Notes
    {
        get { return tbNotes.Text; }
        set { tbNotes.Text = value; }
    }

    public string Directions
    {
        get { return tbDirections.Text; }
        set { tbDirections.Text = value; }
    }
    
    public string Type
    {
        get { return ddlType.SelectedValue; }
        set { ddlType.SelectedValue = value; }
    }

    public string Status
    {
        get { return ddlStatus.SelectedValue; }
        set { ddlStatus.SelectedValue = value; }
    }
    public string Active
    {
        get { return ddlActive.SelectedValue; }
        set { ddlActive.SelectedValue = value; }
    }

    public string Rectype
    {
        get { return ddlType.SelectedValue; }
        set { ddlType.SelectedValue = value; }
    }
    
    public string WhatWeCanDoForThem
    {
        get { return tbWhatWeCanDoForThem.Text; }
        set { tbWhatWeCanDoForThem.Text = value; }
    }

    public string WhatWeDoForThem
    {
        get { return tbWhatWeDoForThem.Text; }
        set { tbWhatWeDoForThem.Text = value; }
    }


    public int FkProjmanager
    {
        get { return ucEmployeeComboBoxProjectManager.EmployeeId; }
        set { ucEmployeeComboBoxProjectManager.EmployeeId = value; }
    }

    public int FkLeadTech
    {
        get { return ucEmployeeComboBoxTechnicalLead.EmployeeId; }
        set { ucEmployeeComboBoxTechnicalLead.EmployeeId = value; }
    }
    
    public double? HourlyRate
    {
        get { return Convert.ToDouble(tbHourlyRate.Text); }
        set { tbHourlyRate.Text = Convert.ToString(value); }
    }
    public int Ranking
    {
        get { return Convert.ToInt32(ddlRanking.SelectedValue); }
        set { ddlRanking.SelectedValue = Convert.ToString(value); }
    }

    public string PremiumSupport
    {
        get { return "Y"; }
        set { chkPremiumSupport.Checked = Convert.ToBoolean(value); }
    }

    public string BackupEnabled
    {
        get { return "Y"; }
        set { cbBackupEnabled.Checked = Convert.ToBoolean(value); }
    }

    public bool Priority
    {
        get { return cbPriorityClient.Checked; }
        set { cbPriorityClient.Checked = value; }
    }
    public bool LeadSystemVendor
    {
        get { return cbPriorityClient.Checked; }
        set { cbPriorityClient.Checked = value; }
    }

     public string VendorForDelivery
    {
        get { return "Y"; }
        set { cbBackupEnabled.Checked = Convert.ToBoolean(value); }
    }

     
     public string BBBCSC
     {
         get { return "Y"; }
         set { chBBBCSC.Checked = Convert.ToBoolean(value); }
     }

     public string threebcsc
     {
         get { return "Y"; }
         set { ch3B.Checked = Convert.ToBoolean(value); }
     }

    protected void btnInsert_Click(object sender, EventArgs e)
    {
        SaveValues(-1);
        rnClientAdd.Text = "Client has been added";
        rnClientAdd.Show();
    }

    /*
    public string SalesPerson
    {
        get { return ucClientSalesPerson.SelectedSalesPerson; } 
        set { ucClientSalesPerson.SelectedSalesPerson = value; }
    }
    */

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

    public DataTable SearchActiveClients(string Id, string Phone, string Email, string Rectype, bool? Active, bool? Priority, bool? TypeClient,  bool? TypeVendor)
    {
        DataTable dynamicList = new DataTable();
        DesktopShared.DaoClasses.TypedListDAO dao = new DesktopShared.DaoClasses.TypedListDAO();

        #region result set fields

        DesktopShared.HelperClasses.ResultsetFields fields = new DesktopShared.HelperClasses.ResultsetFields(35);

        fields.DefineField(DesktopShared.HelperClasses.ClientFields.Clientcode, 0, "ClientCode");
        fields.DefineField(DesktopShared.HelperClasses.ClientFields.Rectype, 1, "Type");
        fields.DefineField(DesktopShared.HelperClasses.ClientFields.Faxphone, 2, "Fax");
        fields.DefineField(DesktopShared.HelperClasses.ClientFields.FkRanking, 3, "Ranking");
        fields.DefineField(DesktopShared.HelperClasses.ClientFields.FkProjmanager, 4, "ProjectManager");
        fields.DefineField(DesktopShared.HelperClasses.ClientFields.Url, 5, "URL");
        fields.DefineField(DesktopShared.HelperClasses.ClientFields.Id, 6, "RecId");
        fields.DefineField(DesktopShared.HelperClasses.ClientFields.Bilrate, 7, "HourlyRate");
        fields.DefineField(DesktopShared.HelperClasses.ClientFields.Active, 8, "Active");
        fields.DefineField(DesktopShared.HelperClasses.EmployeeFields.Descr1, 9, "SalesPerson"); //where descr1 = 'Sales'
        fields.DefineField(DesktopShared.HelperClasses.ClientFields.Directions, 10, "Directions");
        fields.DefineField(DesktopShared.HelperClasses.ClientFields.Company, 11, "Company");
        fields.DefineField(DesktopShared.HelperClasses.ClientFields.Notes, 12, "Notes");
        fields.DefineField(DesktopShared.HelperClasses.ClientFields.WhatWeCanDoForThem, 13, "WhatWeCanDoForThem");
        fields.DefineField(DesktopShared.HelperClasses.ClientFields.Addr1, 14, "Addr1");
        fields.DefineField(DesktopShared.HelperClasses.ClientFields.Addr2, 15, "Addr2");
        fields.DefineField(DesktopShared.HelperClasses.ClientFields.Busphone, 16, "Busphone");
        fields.DefineField(DesktopShared.HelperClasses.ClientFields.Faxphone, 17, "Faxphone");
        fields.DefineField(DesktopShared.HelperClasses.ClientFields.PremiumSupport, 18, "PremiumSupport");
        fields.DefineField(DesktopShared.HelperClasses.ClientFields.City, 19, "City");
        fields.DefineField(DesktopShared.HelperClasses.ClientFields.Zip, 20, "Zip");
        fields.DefineField(DesktopShared.HelperClasses.ClientFields.Password, 21, "Password");
        fields.DefineField(DesktopShared.HelperClasses.ClientFields.State, 22, "State");
        fields.DefineField(DesktopShared.HelperClasses.ClientFields.Busext, 23, "Busext");
        fields.DefineField(DesktopShared.HelperClasses.ClientFields.Id, 24, "Id");
        fields.DefineField(DesktopShared.HelperClasses.ClientFields.Priority, 26, "Priority");
        fields.DefineField(DesktopShared.HelperClasses.ClientFields.Rectype, 27, "Rectype");
        fields.DefineField(DesktopShared.HelperClasses.ClientFields.FkAcctstatus, 28, "AccountStatus");
        fields.DefineField(DesktopShared.HelperClasses.ClientFields.AcctComments, 29, "AccountComments");
        fields.DefineField(DesktopShared.HelperClasses.ClientFields.Backupenabled, 30, "BackupEnabled");
        fields.DefineField(DesktopShared.HelperClasses.ClientFields.AutoCcupdate, 31, "AutoCConTicketUpdate");
        fields.DefineField(DesktopShared.HelperClasses.ClientFields.FkAcctstatus, 32, "VendorClassification"); //couldn't find
        fields.DefineField(DesktopShared.HelperClasses.ClientFields.Bbbcsc, 33, "CSCSupportOptions"); //all items null or empty
        fields.DefineField(DesktopShared.HelperClasses.ClientFields.FkAcctstatus, 34, "WhatTheyCanDoForUs"); //couldn't find
        #endregion

        #region relations

        IRelationCollection relations = new RelationCollection();

        /*
        IEntityRelation relation = new EntityRelation(RelationType.OneToOne);
        relation.AddEntityFieldPair(DesktopShared.FactoryClasses.EntityFieldFactory.Create(
           DesktopShared.UsersFieldIndex.FkClient),
           DesktopShared.FactoryClasses.EntityFieldFactory.Create(
           DesktopShared.ClientFieldIndex.Pclient));
        relations.Add(relation, JoinHint.Left);

        relation = new EntityRelation(RelationType.OneToOne);
        relation.AddEntityFieldPair(DesktopShared.FactoryClasses.EntityFieldFactory.Create(
           DesktopShared.ClientFieldIndex.FkClient),
           DesktopShared.FactoryClasses.EntityFieldFactory.Create(
           DesktopShared.ClientFieldIndex.Pclient));
        relations.Add(relation, JoinHint.Left);
        */
        #endregion

        #region filter

        IPredicateExpression userFilter = new PredicateExpression();
        
        if (!String.IsNullOrEmpty(Id))
            userFilter.Add(DesktopShared.HelperClasses.ClientFields.Id == Id);

        if (Priority.HasValue && Priority.Value)
            userFilter.Add(DesktopShared.HelperClasses.ClientFields.Priority == Priority.Value);

        List<string> _recordTypes = new List<string>();


        if (TypeClient.HasValue && TypeClient.Value)
            _recordTypes.Add("C");

        //if (TypeGeneral.HasValue && TypeGeneral.Value)
        //    _recordTypes.Add("G");

        if (TypeVendor.HasValue && TypeVendor.Value)
            _recordTypes.Add("V");

        userFilter.Add(DesktopShared.HelperClasses.ClientFields.Rectype == _recordTypes);

        if (SearchActiveStatus.HasValue)
        {
            string _active = SearchActiveStatus.Value ? "Y" : "N";
            userFilter.Add(DesktopShared.HelperClasses.ClientFields.Active == _active);
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

    public enum EditMode
    {
        Grid,
        Page
    }

    private EditMode _editMode = EditMode.Grid;

    public void LoadValues(System.Data.DataRowView ClientRow)
    {
        btnInsert.Visible = (ClientRow == null);

        #region existing record

        if (ClientRow != null)
        {
            tbClientCode.Text = ClientRow["Clientcode"].ToString().Trim();
            tbCompany.Text = ClientRow["Company"].ToString().Trim();
            tbUrl.Text = ClientRow["Url"].ToString().Trim();
            ddlType.SelectedValue = ClientRow["RecType"].ToString().Trim();
            tbHourlyRate.Text = ClientRow["Bilrate"].ToString().Trim();
            tbPhone.Text = ClientRow["Busphone"].ToString().Trim();
            //chkPremiumSupport.Checked = ClientRow["PremiumSupport"].ToBoolean().Trim();
            tbFax.Text = ClientRow["Faxphone"].ToString().Trim();
            if (tbFax.Text.Length == 10)
            {
                tbFax.Text = String.Format("{0:(###) ###-####}", Convert.ToInt64(ClientRow["Faxphone"].ToString().Trim()));
            }

            tbAddress1.Text = ClientRow["Addr1"].ToString().Trim();
            tbAddress2.Text = ClientRow["Addr2"].ToString().Trim();
            tbCity.Text = ClientRow["City"].ToString().Trim();
            ddlState.SelectedValue = ClientRow["State"].ToString().Trim();
            tbZip.Text = ClientRow["Zip"].ToString().Trim();
            tbPasswordCode.Text = ClientRow["Password"].ToString().Trim();
            ddlStatus.SelectedValue = ClientRow["Active"].ToString().Trim();

            //if (_editMode == EditMode.Page)
            //    btnUpdatePage.Text = "UPDATE";

        }

        #endregion
    }
}

