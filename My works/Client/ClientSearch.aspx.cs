using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class Client_ClientSearch : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindProjectManaget_Lead();
            BindSalesPerson();
            BindClientType();
            BindMSPType();
            BindTAM();
            phSearchResults.Visible = false;
        }
    }

    /// <summary>
    /// rebind grid
    /// </summary>
    private void RebindGrid()
    {
        phSearchResults.Visible = true;
        rgClient.EditIndexes.Clear();
        rgClient.DataSource = null;
        rgClient.Rebind();
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        RebindGrid();
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        Response.Redirect(Request.RawUrl);
    }
    

    protected void rgClient_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        #region Item or AlternatingItem

        if (e.Item.ItemType == GridItemType.Item || e.Item.ItemType == GridItemType.AlternatingItem)
        {
            GridEditableItem item = e.Item as GridEditableItem;
            DataRowView drv = (DataRowView)e.Item.DataItem;

            if ((bool)drv["ChargeFutureInvoices"])
                item["ChargeFutureInvoices"].Text = "Y";
            else
                item["ChargeFutureInvoices"].Text = "N";

            if ((bool)drv["BackupMonitorActive"])
                item["BackupMonitorActive"].Text = "Y";
            else
                item["BackupMonitorActive"].Text = "N";
            //DesktopShared.EntityClasses.ClientEntity objClient = (DesktopShared.EntityClasses.ClientEntity)e.Item.DataItem;

            //TelerikHelper.AddLabelToCell(objClient.ChargeFutureInvoices  ? "Y" : "N", _gdi["ChargeFutureInvoices"]);
            //TelerikHelper.AddLabelToCell(objClient.BackupMonitorActive ? "Y" : "N", _gdi["BackupMonitorActive"]);

        }

        #endregion
    }

    protected void rgClient_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        //rgClient.DataSource = DesktopShared.Client.SearchClients(txtCompany.Text, txtCode.Text, ddlAutoBill.SelectedIndex == 0 ? (bool?)null : ddlAutoBill.SelectedIndex == 1 ? true : false);
        bool? _autoBill = ddlAutoBill.SelectedIndex == 0 ? (bool?)null : ddlAutoBill.SelectedIndex == 1 ? true : false;
        bool? _backupMonitorActive = ddlBackupMonitor.SelectedIndex == 0 ? (bool?)null : ddlBackupMonitor.SelectedIndex == 1 ? true : false; 
        //rgClient.DataSource = SearchClients(txtCompany.Text, txtCode.Text, ddlStatus.SelectedValue,ddlTicketType.TicketTypeId, _autoBill, _backupMonitorActive);
        rgClient.DataSource = NewSearchClients(txtCompany.Text, txtCode.Text, ddlStatus.SelectedValue, ddlTicketType.TicketTypeId, _autoBill, _backupMonitorActive);
    }

    private DesktopShared.CollectionClasses.ClientCollection SearchClients(string companyName, string companyCode, string status, int? tickettypeid, bool? autoBilling = null, bool? backupMonitorActive = null)
    {
        DesktopShared.CollectionClasses.ClientCollection collection = new DesktopShared.CollectionClasses.ClientCollection();
        SD.LLBLGen.Pro.ORMSupportClasses.IPredicateExpression clientsFilter = new SD.LLBLGen.Pro.ORMSupportClasses.PredicateExpression();

        if (!String.IsNullOrEmpty(companyName))
        {
            string companyLike = "%" + companyName + "%";
            clientsFilter.Add(DesktopShared.HelperClasses.ClientFields.Company % companyLike);
        }

        if (!string.IsNullOrEmpty(companyCode))
        {
            clientsFilter.Add(DesktopShared.HelperClasses.ClientFields.Code == companyCode);
        }
        clientsFilter.Add(DesktopShared.HelperClasses.ClientFields.Company != "");
        if (autoBilling.HasValue)
            clientsFilter.Add(DesktopShared.HelperClasses.ClientFields.ChargeFutureInvoices == autoBilling.Value);
        if (!string.IsNullOrEmpty(status))
            clientsFilter.Add(DesktopShared.HelperClasses.ClientFields.Active == status);
        if (backupMonitorActive.HasValue)
            clientsFilter.Add(DesktopShared.HelperClasses.ClientFields.BackupMonitorActive == backupMonitorActive.Value);
        if(tickettypeid!=null)
            clientsFilter.Add(DesktopShared.HelperClasses.ClientFields.TicketTypeId == tickettypeid);

        #region relations
        SD.LLBLGen.Pro.ORMSupportClasses.IRelationCollection _relations = new SD.LLBLGen.Pro.ORMSupportClasses.RelationCollection();

        SD.LLBLGen.Pro.ORMSupportClasses.IEntityRelation _relation = new SD.LLBLGen.Pro.ORMSupportClasses.EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToOne);
        _relation.AddEntityFieldPair(DesktopShared.FactoryClasses.EntityFieldFactory.Create(DesktopShared.ClientFieldIndex.FkProjmanager),
                                     DesktopShared.FactoryClasses.EntityFieldFactory.Create(DesktopShared.EmployeeFieldIndex.Pemployee));
        _relations.Add(_relation, SD.LLBLGen.Pro.ORMSupportClasses.JoinHint.Full);
        #endregion

        SD.LLBLGen.Pro.ORMSupportClasses.ISortExpression clientsSort = new SD.LLBLGen.Pro.ORMSupportClasses.SortExpression();
        clientsSort.Add(DesktopShared.HelperClasses.ClientFields.Company | SD.LLBLGen.Pro.ORMSupportClasses.SortOperator.Ascending);

        collection.GetMulti(clientsFilter, 0, clientsSort);
        return collection;


    }

    private DataTable NewSearchClients(string companyName, string companyCode, string status, int? tickettypeid, bool? autoBilling = null, bool? backupMonitorActive = null)
    {
        DataTable dt = null;
        int? projectmanager = null, techlead = null, msptype=null, _tam=null, _billingtype=null ;
        string salesperson = string.Empty;
        string clienttype = "";
        string clientservices = "";

        if (lbClientService.SelectedValues.Count > 0)
        {
            List<string> _clientclientservices = lbClientService.SelectedValues;
            for (int i = 0; i <= _clientclientservices.Count - 1; i++)
            {
                if (clientservices == string.Empty)
                    clientservices = _clientclientservices[i].ToString().Trim();
                else
                    clientservices += "," + _clientclientservices[i].ToString().Trim();
            }
        }

        if (ddlProjectManager.SelectedValue != string.Empty)
            projectmanager = Convert.ToInt32(ddlProjectManager.SelectedValue);
        if (ddlTechnicalLead.SelectedValue != string.Empty)
            techlead = Convert.ToInt32(ddlTechnicalLead.SelectedValue);
        if (ddlSalesPerson.SelectedValue != string.Empty)
            salesperson = ddlSalesPerson.SelectedItem.Text.Trim();

        if (ddlClientType.SelectedValue != string.Empty)
            clienttype = ddlClientType.SelectedItem.Text.Trim();
        if (ddlMSPType.SelectedValue != string.Empty)
            msptype = Convert.ToInt32(ddlMSPType.SelectedValue);
        if (ddlTAM.SelectedValue != string.Empty)
            _tam = Convert.ToInt32(ddlTAM.SelectedValue);

        if (ddlBillingType.SelectedValue != string.Empty)
            _billingtype = Convert.ToInt32(ddlBillingType.SelectedValue);

        dt =DesktopShared.StoredProcedureCallerClasses.RetrievalProcedures.ProcSearchClients(companyName, companyCode, status,tickettypeid, 
            autoBilling, backupMonitorActive, projectmanager, techlead, salesperson, clienttype, msptype, _tam, clientservices, _billingtype);
        return dt;
        /*
        //dynamic list 
        DataTable _dynamicList = new DataTable();

        //dao
        DesktopShared.DaoClasses.TypedListDAO _dao = new DesktopShared.DaoClasses.TypedListDAO();

        #region result set fields

        DesktopShared.HelperClasses.ResultsetFields _fields = new DesktopShared.HelperClasses.ResultsetFields(15);
        int _fieldCount = -1;

        _fields.DefineField(DesktopShared.HelperClasses.ClientFields.Pclient, ++_fieldCount, "Id");
        _fields.DefineField(DesktopShared.HelperClasses.ClientFields.Pclient, ++_fieldCount, "Pclient");
        _fields.DefineField(DesktopShared.HelperClasses.ClientFields.Code, ++_fieldCount, "Code");
        _fields.DefineField(DesktopShared.HelperClasses.ClientFields.Company, ++_fieldCount, "Company");
        _fields.DefineField(DesktopShared.HelperClasses.ClientFields.Addr1, ++_fieldCount, "Addr1");
        _fields.DefineField(DesktopShared.HelperClasses.ClientFields.Addr2, ++_fieldCount, "Addr2");
        _fields.DefineField(DesktopShared.HelperClasses.ClientFields.City, ++_fieldCount, "City");
        _fields.DefineField(DesktopShared.HelperClasses.ClientFields.State, ++_fieldCount, "State");
        _fields.DefineField(DesktopShared.HelperClasses.ClientFields.Zip, ++_fieldCount, "Zip");
        _fields.DefineField(DesktopShared.HelperClasses.ClientFields.Busphone, ++_fieldCount, "Busphone");
        _fields.DefineField(DesktopShared.HelperClasses.ClientFields.ChargeFutureInvoices, ++_fieldCount, "ChargeFutureInvoices");
        _fields.DefineField(DesktopShared.HelperClasses.ClientFields.BackupMonitorActive, ++_fieldCount, "BackupMonitorActive");
        _fields.DefineField(DesktopShared.HelperClasses.ClientFields.Active, ++_fieldCount, "Active");
        _fields.DefineField(DesktopShared.HelperClasses.EmployeeFields.First, ++_fieldCount, "First", "ProjectManager");
        _fields.DefineField(DesktopShared.HelperClasses.EmployeeFields.Last, ++_fieldCount, "Last", "ProjectManager");

        #endregion

        #region relations
        SD.LLBLGen.Pro.ORMSupportClasses.IRelationCollection _relations = new SD.LLBLGen.Pro.ORMSupportClasses.RelationCollection();

        SD.LLBLGen.Pro.ORMSupportClasses.IEntityRelation _relation = new SD.LLBLGen.Pro.ORMSupportClasses.EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToOne);
        _relation.AddEntityFieldPair(DesktopShared.FactoryClasses.EntityFieldFactory.Create(DesktopShared.ClientFieldIndex.FkProjmanager),
                                     DesktopShared.FactoryClasses.EntityFieldFactory.Create(DesktopShared.EmployeeFieldIndex.Pemployee));
        _relations.Add(_relation, SD.LLBLGen.Pro.ORMSupportClasses.JoinHint.Full);
        #endregion

        #region filter

        SD.LLBLGen.Pro.ORMSupportClasses.IPredicateExpression _filter = new SD.LLBLGen.Pro.ORMSupportClasses.PredicateExpression();

        if (!String.IsNullOrEmpty(companyName))
        {
            string companyLike = "%" + companyName + "%";
            _filter.Add(DesktopShared.HelperClasses.ClientFields.Company % companyLike);
        }

        if (!string.IsNullOrEmpty(companyCode))
        {
            _filter.Add(DesktopShared.HelperClasses.ClientFields.Code == companyCode);
        }
        _filter.Add(DesktopShared.HelperClasses.ClientFields.Company != "");
        if (autoBilling.HasValue)
            _filter.Add(DesktopShared.HelperClasses.ClientFields.ChargeFutureInvoices == autoBilling.Value);
        if (!string.IsNullOrEmpty(status))
            _filter.Add(DesktopShared.HelperClasses.ClientFields.Active == status);
        if (backupMonitorActive.HasValue)
            _filter.Add(DesktopShared.HelperClasses.ClientFields.BackupMonitorActive == backupMonitorActive.Value);
        if (tickettypeid != null)
            _filter.Add(DesktopShared.HelperClasses.ClientFields.TicketTypeId == tickettypeid);

        #endregion

        //sort expression
        SD.LLBLGen.Pro.ORMSupportClasses.ISortExpression _sorter = new SD.LLBLGen.Pro.ORMSupportClasses.SortExpression();
        _sorter.Add(DesktopShared.HelperClasses.ClientFields.Company | SD.LLBLGen.Pro.ORMSupportClasses.SortOperator.Ascending);

        //fetch
        _dao.GetMultiAsDataTable(_fields, _dynamicList, 0, _sorter, _filter, _relations, false, null, null, 0, 0);

        return _dynamicList;*/
    }

    #region ddl
    private void BindProjectManaget_Lead()
    {
        var employees = DesktopShared.Employee.GetActiveEmployees();
        ddlProjectManager.DataSource = employees;
        ddlProjectManager.DataTextField = "FullName";
        ddlProjectManager.DataValueField = "EmployeeID";
        ddlProjectManager.DataBind();
        ddlProjectManager.Items.Insert(0, new ListItem("Select Project Manager", ""));

        ddlTechnicalLead.DataSource = employees;
        ddlTechnicalLead.DataTextField = "FullName";
        ddlTechnicalLead.DataValueField = "EmployeeID";
        ddlTechnicalLead.DataBind();
        ddlTechnicalLead.Items.Insert(0, new ListItem("Select Technical Lead", ""));
    }

    private void BindSalesPerson()
    {
        //ddlSalesPerson.DataSource = DesktopShared.ClientContract.GetSalesEmployeeOfSupportcontracts(-999,0,0,0);
        ddlSalesPerson.DataSource = DesktopShared.StoredProcedureCallerClasses.RetrievalProcedures.ProcGetSalesperson(-999, 0, 0, 0);
        ddlSalesPerson.DataTextField = "Name";
        ddlSalesPerson.DataValueField = "ID";
        ddlSalesPerson.DataBind();
        ListItem liDefault = new ListItem("Select a Sales Person", "");
        ddlSalesPerson.Items.Insert(0, liDefault);
    }

    private void BindClientType()
    {
        ddlClientType.DataSource = DesktopShared.ClientTypeLists.GetAllActive();
        ddlClientType.DataTextField = "ClientType";
        ddlClientType.DataValueField = "ID";
        ddlClientType.DataBind();
        ListItem liDefault = new ListItem("Select a Client Type", "");
        ddlClientType.Items.Insert(0, liDefault);
    }

    private void BindMSPType()
    {
        ddlMSPType.DataSource = DesktopShared.MSPTypeLists.GetAllActive();
        ddlMSPType.DataTextField = "MSPType";
        ddlMSPType.DataValueField = "ID";
        ddlMSPType.DataBind();
        ListItem liDefault = new ListItem("Select a MSP Type", "");
        ddlMSPType.Items.Insert(0, liDefault);
    }

    private void BindTAM()
    {
        var employees = DesktopShared.Employee.GetActiveEmployees();
        ddlTAM.DataSource = employees;
        ddlTAM.DataTextField = "FullName";
        ddlTAM.DataValueField = "EmployeeID";
        ddlTAM.DataBind();
        ddlTAM.Items.Insert(0, new ListItem("Select Tam", ""));
    }
    #endregion

}