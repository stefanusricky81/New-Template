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
using DesktopShared.EntityClasses;
using DesktopShared.CollectionClasses;
using Newtonsoft.Json;
using System.Collections.Generic;
using DesktopShared.HelperClasses;

public partial class Client_Default : BasePage
{

    private static RestSharp.RestClient _client = new RestSharp.RestClient("https://api.itglue.com/");
    
    public int ClientId
    {
        get
        {
            if (ViewState["ClientId"] == null)
                return 0;

            return Convert.ToInt32(ViewState["ClientId"]);
        }
        set
        {
            ViewState["ClientId"] = value;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        DropDownList _ddl = ddlCountry.GetDropDownList();
        _ddl.AutoPostBack = true;
        _ddl.SelectedIndexChanged += ddlCountry_SelectedIndexChanged;

        gridClientDomain.ItemCommand += GridClientDomain_ItemCommand;
        ConfigureForDevice();
        if (!this.IsPostBack)
        {
            SetTabIndex();
            BindStates();
            BindPriority();
            BindAccountStatus();
            BindProjectManaget_Lead();
            BindSalesPerson();
            BindClientType();
            BindTAM();

            //BindClientContact();// This will be called from needdatasource event of client contact
            int _id;
            if (Int32.TryParse(Request.QueryString["ClientId"], out _id))
            {
                ClientId = _id;
                LoadClientInformation();
            }
           
        }
        

    }

    private void ddlCountry_SelectedIndexChanged(object sender, EventArgs e)
    {
        try 
        {
            CountryCheck();
        }
        catch (Exception ex)
        { }
    }

    #region private methods



    /// <summary>
    /// grid client domain on item command
    /// </summary>
    /// <param name="success"></param>
    /// <param name="message"></param>
    private void GridClientDomain_ItemCommand(bool success, string message)
    {
        if (!String.IsNullOrWhiteSpace(message))
            DisplayMessage(message, success ? Bootstrap.Alert.AlertType.Success : Bootstrap.Alert.AlertType.Warning);
    }

    /// <summary>
    /// configure for device
    /// </summary>
    private void ConfigureForDevice()
    {
        
    }

    private void SetTabIndex()
    {
        short _tabIndex = 0;

        lblCompanyName.Focus();
        lblCompanyName.TabIndex = ++_tabIndex;
        lblClientCode.TabIndex = ++_tabIndex;
        lblAddress.TabIndex = ++_tabIndex;
        lblAddress2.TabIndex = ++_tabIndex;
        lblFax.TabIndex = ++_tabIndex;
        lblCity.TabIndex = ++_tabIndex;
        ddlState.TabIndex = ++_tabIndex;
        lblZip.TabIndex = ++_tabIndex;
        lblPhone.TabIndex = ++_tabIndex;
        txtURL.TabIndex = ++_tabIndex;

        chkActive.TabIndex = ++_tabIndex;
        ddlPriority.TabIndex = ++_tabIndex;
        ddlAccStatus.TabIndex = ++_tabIndex;
        txtAccountingComments.TabIndex = ++_tabIndex;
        txtHourlyRate.TabIndex = ++_tabIndex;
        txtRecID.TabIndex = ++_tabIndex;
        txtDocumentationURL.TabIndex = ++_tabIndex;
        chkDarkWeb.TabIndex = ++_tabIndex;
        chkHelpDesk.TabIndex = ++_tabIndex;
        ddlTicketType.TabIndex = ++_tabIndex;
        txtAutoCC.TabIndex = ++_tabIndex;
        chkBackupMonitor.TabIndex = ++_tabIndex;
        ddlEndpointPlatform.TabIndex = ++_tabIndex;
        txtSentinelOneSiteName.TabIndex = ++_tabIndex;
        txtSplashtopName.TabIndex = ++_tabIndex;
        txtArcticWolfName.TabIndex = ++_tabIndex;

        ddlProjectManager.TabIndex = ++_tabIndex;
        ddlTechnicalLead.TabIndex = ++_tabIndex;
        ddlSalesPerson.TabIndex = ++_tabIndex;

        txtNotes.TabIndex = ++_tabIndex;
        txtWhatwedo.TabIndex = ++_tabIndex;
        txtDirections.TabIndex = ++_tabIndex;
        txtWhatwecando.TabIndex = ++_tabIndex;

        btnSave.TabIndex = ++_tabIndex;
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

    private void LoadClientInformation()
    {
        if (ClientId == 0)
        {
            chkActive.Checked = true;
            chkBackupMonitor.Checked = true;

            btnSetQWSync.Visible = false;
            btnITGlueSync.Visible = false;
            rbCRMHistory.Visible = false;

            hlBacktoList.Visible = false;
            hlGoToOldDetail.Visible = false;
            phDomains.Visible = false;
            phLastUpdated.Visible = false;
            ddlCountry.CountryId = 1;
            return;
        }

        DesktopShared.EntityClasses.ClientEntity objClient = new DesktopShared.EntityClasses.ClientEntity(ClientId);

        int CRMLeadId = CRM.Client.GetLeadId(objClient.Pclient);
        if (CRMLeadId > 0)
        {
            rbCRMHistory.Visible = true;
            rbCRMHistory.CommandArgument = CRMLeadId.ToString();
        }
        else
            rbCRMHistory.Visible = false;

        hlGoToOldDetail.NavigateUrl = "~/Client/Detail.aspx?ClientID=" + objClient.Pclient.ToString();

        lblClientName.Text = objClient.Company.Trim();
        lblAddress.Text = objClient.Addr1.Trim();
        lblAddress2.Text = objClient.Addr2.Trim();
        lblCity.Text = objClient.City.Trim();
        lblClientCode.Text = objClient.Code.Trim();
        lblCompanyName.Text = objClient.Company.Trim();
        txtClientJobNumber.Text = objClient.ClientJobNumber.Trim();

        ddlCountry.CountryId = (int?)objClient.Country == null ? 1 : (int?)objClient.Country;
        CountryCheck();
        if (ddlCountry.CountryId > 1)
        {
            txtState.Text = objClient.StateProvenceRegion.Trim();
            txtPhone.Text = objClient.Busphone.Trim();
            txtFax.Text = objClient.Faxphone.Trim();
        }
        else
        {
            //if (ddlState.SelectedIndex > 0)
            ddlState.SelectedValue = objClient.State.Trim();
            
            lblPhone.Text = objClient.Busphone.Trim();
            lblFax.Text = objClient.Faxphone.Trim();
        }
        lblZip.Text = objClient.Zip.Trim();
        txtURL.Text = objClient.Url.Trim();
        ddlTAM.SelectedValue = objClient.TamEmployeeId == null ? string.Empty : objClient.TamEmployeeId.Value.ToString().Trim();

        chkActive.Checked = objClient.Active.ToUpper() == "Y" ? true : false;
        ddlPriority.SelectedValue = Convert.ToString(objClient.FkRanking);
        ddlAccStatus.SelectedValue = Convert.ToString(objClient.FkAcctstatus);
        txtAccountingComments.Text = objClient.AcctComments;

        txtRecID.Text = objClient.Id.Trim();
        txtDocumentationURL.Text = objClient.DocumentationUrl;
        chkDarkWeb.Checked = objClient.DarkWeb;
        //txtAutoCC.Text = objClient.AutoCcupdate;
        chkHelpDesk.Enabled = true;
        chkHelpDesk.Checked = objClient.UseHelpDesk;
        if (objClient.IsMasterClient)
        {
            chkHelpDesk.Checked = true;
            chkHelpDesk.Enabled = false;
        }
        ddlTicketType.TicketTypeId = objClient.TicketTypeId;
        chkBackupMonitor.Checked = objClient.BackupMonitorActive;
        ddlEndpointPlatform.EndpointPlatformId = objClient.EndpointPlatformId;
        txtSentinelOneSiteName.Text = objClient.SentinelOneSiteName.Trim();
        txtSplashtopName.Text = objClient.SplashtopName.Trim();
        txtArcticWolfName.Text = objClient.ArcticWolfName.Trim();

        ddlProjectManager.SelectedValue = Convert.ToString(objClient.FkProjmanager);
        ddlTechnicalLead.SelectedValue = Convert.ToString(objClient.FkLeadtech);
        txtNotes.Text = objClient.Notes.Trim();
        cbExcludeFromReports.Checked = objClient.ExcludefromReport == null ? false : (bool)objClient.ExcludefromReport;
        cbPatchingReport.Checked = objClient.PatchingReport == null ? false : (bool)objClient.PatchingReport;
        chkPortalManagedFaxReport.Checked = objClient.PortalManagedFaxReport;

        txtNotes.Text = objClient.Notes.Trim();
        txtDirections.Text = objClient.Directions.Trim();
        txtWhatwedo.Text = objClient.WhatWeDoForYou.Trim();
        txtWhatwecando.Text = objClient.WhatWeCanDoForYou.Trim();
        txtHourlyRate.Text = Convert.ToString(objClient.Bilrate);
        ddlBillingType.SelectedValue = objClient.BillingType.ToString();
        //last updated -> ddMMyy hhmmss
        string _lastUpdated = objClient.LastUpdated.Trim();
        _lastUpdated = _lastUpdated.Replace(" ", "");
        string _tempLastUpdated = new String(_lastUpdated.Where(Char.IsDigit).ToArray());
        if (_tempLastUpdated.Length == 12)
            _lastUpdated = String.Format("{0}/{1}/{2} {3}:{4}", _tempLastUpdated.Substring(2, 2), _tempLastUpdated.Substring(0, 2), _tempLastUpdated.Substring(4, 2), _tempLastUpdated.Substring(6, 2), _tempLastUpdated.Substring(8, 2));
        else
            _lastUpdated = "N/A";
        string _lastUpdatedBy = objClient.LastUpdatedby.Trim();
        if (String.IsNullOrWhiteSpace(_lastUpdatedBy))
            _lastUpdatedBy = "N/A";
        phLastUpdated.Visible = true;
        litLastUpdated.Text = String.Format("{0} by {1}", _lastUpdated, _lastUpdatedBy);

        phDomains.Visible = true;
        gridClientDomain.ClientIdForDomain = objClient.Pclient;
        gridClientDomain.RebindGrid();

        //Sales Person
        PredicateExpression salesPersonFilter = new PredicateExpression();
        salesPersonFilter.Add(DesktopShared.HelperClasses.ClientEmployeesalesFields.FkClient == ClientId);

        ISortExpression prioritySort = new SD.LLBLGen.Pro.ORMSupportClasses.SortExpression();

        var res = DesktopShared.CollectionClasses.ClientEmployeesalesCollection.GetMultiAsDataTable(salesPersonFilter, 0, prioritySort);

        foreach (DataRow dr in res.Rows)
        {
            int emp = Convert.ToInt16(dr["fkemployee"]);
            foreach (ListItem listItem in ddlSalesPerson.Items)
            {
                if (listItem.Value.Equals(Convert.ToString(emp)))
                {
                    listItem.Selected = true;
                    break;
                }
            }
        }
        //End of sales person

        //Client type
        PredicateExpression ClientTypeFilter = new PredicateExpression();
        ClientTypeFilter.Add(ClientTypeClientIdFields.Fkclient == ClientId);
        ISortExpression ClientTypeSort = new SD.LLBLGen.Pro.ORMSupportClasses.SortExpression();
        var resClientType = ClientTypeClientIdCollection.GetMultiAsDataTable(ClientTypeFilter, 0, ClientTypeSort);
        foreach (DataRow drClientType in resClientType.Rows)
        {
            int emp = Convert.ToInt16(drClientType["fkclienttypeid"]);
            foreach (ListItem listItem in lbClientType.Items)
            {
                if (listItem.Value.Equals(Convert.ToString(emp)))
                {
                    listItem.Selected = true;
                    break;
                }
            }
        }
        //End of Client type

        #region Client Services
        //List<string> _selectedValues = new List<string>();

        //PredicateExpression Clientservices = new PredicateExpression();
        //Clientservices.Add(ClientClientServiceFields.FkClient == ClientId);
        //ISortExpression ClientServicesSort = new SD.LLBLGen.Pro.ORMSupportClasses.SortExpression();
        //var resClientServices = ClientClientServiceCollection.GetMultiAsDataTable(Clientservices, 0, ClientServicesSort);
        //foreach (DataRow drClientType in resClientServices.Rows)
        //{
        //    int clientservices = Convert.ToInt16(drClientType["fkClienServices"]);
        //    _selectedValues.Add(clientservices.ToString().Trim());
        //}
        //lbClientService.Populate(_selectedValues);
        #endregion
    }

    #endregion

    #region Bind DropDown

    private void BindStates()
    {
        //clear items
        ddlState.ClearSelection();
        ddlState.Items.Clear();

        #region bind drop down list

        PredicateExpression stateFilter = new PredicateExpression();

        var res = DesktopShared.CollectionClasses.StateCollection.GetMultiAsDataTable(stateFilter, 0, null);


        ddlState.DataSource = res;

        ddlState.DataTextField = "name";
        ddlState.DataValueField = "pstate";
        ddlState.DataBind();
        ddlState.DataSource = null;

        #endregion


        ListItem liDefault = new ListItem("Select a State", "");
        ddlState.Items.Insert(0, liDefault);
    }

    private void BindPriority()
    {
        //clear items
        ddlPriority.ClearSelection();
        ddlPriority.Items.Clear();

        #region bind drop down list

        PredicateExpression priorityFilter = new PredicateExpression();
        priorityFilter.Add(DesktopShared.HelperClasses.SupportingTableFields.TableType == "ClientRanking");

        ISortExpression prioritySort = new SD.LLBLGen.Pro.ORMSupportClasses.SortExpression();
        prioritySort.Add(DesktopShared.HelperClasses.SupportingTableFields.Description | SortOperator.Ascending);

        var res = DesktopShared.CollectionClasses.SupportingTableCollection.GetMultiAsDataTable(priorityFilter, 0, prioritySort);


        ddlPriority.DataSource = res;

        ddlPriority.DataTextField = "description";
        ddlPriority.DataValueField = "psupportingtable";
        ddlPriority.DataBind();
        ddlPriority.DataSource = null;

        #endregion


        ListItem liDefault = new ListItem("Select Priority", "");
        ddlPriority.Items.Insert(0, liDefault);
    }

    private void BindAccountStatus()
    {
        //clear items
        ddlAccStatus.ClearSelection();
        ddlAccStatus.Items.Clear();

        #region bind drop down list

        PredicateExpression priorityFilter = new PredicateExpression();
        priorityFilter.Add(DesktopShared.HelperClasses.SupportingTableFields.TableType == "ACCTSTATUS");

        ISortExpression prioritySort = new SD.LLBLGen.Pro.ORMSupportClasses.SortExpression();
        prioritySort.Add(DesktopShared.HelperClasses.SupportingTableFields.Description | SortOperator.Ascending);

        var res = DesktopShared.CollectionClasses.SupportingTableCollection.GetMultiAsDataTable(priorityFilter, 0, prioritySort);


        ddlAccStatus.DataSource = res;

        ddlAccStatus.DataTextField = "description";
        ddlAccStatus.DataValueField = "psupportingtable";
        ddlAccStatus.DataBind();
        ddlAccStatus.DataSource = null;

        #endregion


        //ListItem liDefault = new ListItem("Select Acc Astatus", "");
        //ddlAccStatus.Items.Insert(0, liDefault);
    }

    private void BindProjectManaget_Lead()
    {
        var employees = DesktopShared.Employee.GetActiveEmployees();
        ddlProjectManager.DataSource = employees;
        ddlProjectManager.DataTextField = "FullName";
        ddlProjectManager.DataValueField = "EmployeeID";
        ddlProjectManager.DataBind();
        ddlProjectManager.Items.Insert(0, new ListItem("Select Project Manager", "0"));

        //lstEmployees.DataSource = employees;
        //lstEmployees.DataTextField = "FullName";
        //lstEmployees.DataValueField = "EmployeeID";
        //lstEmployees.DataBind();

        ddlTechnicalLead.DataSource = employees;
        ddlTechnicalLead.DataTextField = "FullName";
        ddlTechnicalLead.DataValueField = "EmployeeID";
        ddlTechnicalLead.DataBind();
        ddlTechnicalLead.Items.Insert(0, new ListItem("Select Technical Lead", "0"));
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
        var employees = DesktopShared.ClientTypeLists.GetAllActive();
        lbClientType.DataSource = employees;
        lbClientType.DataTextField = "ClientType";
        lbClientType.DataValueField = "Id";
        lbClientType.DataBind();
        lbClientType.Items.Insert(0, new ListItem("Select Client Type", "0"));
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

    //private void BindExpirationMonthAndYear()
    //{
    //    for (var i = DateTime.Now.Year; i < DateTime.Now.Year + 20; i++)
    //    {
    //        ddlYear.Items.Add(new ListItem() { Text = i.ToString(), Value = i.ToString() });
    //    }

    //    for (var i = 1; i < 12; i++)
    //    {
    //        ddlExpMonth.Items.Add(new ListItem() { Text = i.ToString(), Value = i.ToString() });
    //    }

    //    ddlYear.Items.Insert(0, new ListItem() { Text = "Select Year", Value = "-1", Selected = true });
    //    ddlExpMonth.Items.Insert(0, new ListItem() { Text = "Select Month", Value = "-1", Selected = true });
    //}
    #endregion


    private DesktopShared.EntityClasses.ClientEntity SaveClientInformation()
    {
        DesktopShared.EntityClasses.ClientEntity objClient = ClientId > 0 ? new DesktopShared.EntityClasses.ClientEntity(ClientId) : new DesktopShared.EntityClasses.ClientEntity();

        try
        {
            objClient.Code = lblClientCode.Text.Trim();
            objClient.Company = lblCompanyName.Text.Trim();
            objClient.Addr1 = lblAddress.Text.Trim();
            objClient.Addr2 = lblAddress2.Text.Trim();
            objClient.City = lblCity.Text.Trim();
            objClient.Country = ddlCountry.CountryId;
            objClient.ClientJobNumber = txtClientJobNumber.Text.Trim();
            objClient.TicketCategoryRequired = true;

            if (ddlCountry.CountryId > 1)
            {
                objClient.StateProvenceRegion = txtState.Text;
                objClient.Busphone = txtPhone.Text.Trim();
                objClient.Faxphone = txtFax.Text.Trim();
            }
            else
            {
                objClient.State = ddlState.SelectedValue;
                objClient.Busphone = lblPhone.Text.Trim();
                objClient.Faxphone = lblFax.Text.Trim();
            }
            objClient.Zip = lblZip.Text.Trim();            
            objClient.Url = txtURL.Text.Trim();

            bool NeedToDeactivateContacts = false;

            if (ClientId > 0 && objClient.Active.Trim().ToUpper() == "Y" && !chkActive.Checked)
                NeedToDeactivateContacts = true;

            objClient.Active = chkActive.Checked?"Y":"N";
            objClient.Id = txtRecID.Text.Trim();
            objClient.FkRanking = ddlPriority.SelectedValue == string.Empty ? (int?)null : Convert.ToInt16(ddlPriority.SelectedValue);
            objClient.FkAcctstatus = Convert.ToInt16(ddlAccStatus.SelectedValue);
            objClient.AcctComments = txtAccountingComments.Text.Trim();
            objClient.DocumentationUrl = txtDocumentationURL.Text.Trim();
            if (string.IsNullOrEmpty(txtHourlyRate.Text.Trim()) )          
                objClient.SetNewFieldValue((int)DesktopShared.ClientFieldIndex.Bilrate, null);                
            else
                objClient.Bilrate = Convert.ToDouble(txtHourlyRate.Text);

             // txtDocumentationURL.Text.Trim();
            objClient.DarkWeb = chkDarkWeb.Checked;
            //objClient.AutoCcupdate = txtAutoCC.Text.Trim();
            objClient.UseHelpDesk = chkHelpDesk.Checked;
            objClient.TicketTypeId = ddlTicketType.TicketTypeId;
            objClient.BackupMonitorActive = chkBackupMonitor.Checked;
            objClient.EndpointPlatformId = ddlEndpointPlatform.EndpointPlatformId;
            objClient.SentinelOneSiteName = txtSentinelOneSiteName.Text.Trim();
            objClient.SplashtopName = txtSplashtopName.Text.Trim();
            objClient.ArcticWolfName = txtArcticWolfName.Text.Trim();
            objClient.BillingType = Convert.ToInt32(ddlBillingType.SelectedValue);

            if (!string.IsNullOrEmpty(ddlTAM.SelectedValue.Trim()))
                objClient.TamEmployeeId = Convert.ToInt16(ddlTAM.SelectedValue);
            else
                objClient.TamEmployeeId = null;

            objClient.ExcludefromReport = cbExcludeFromReports.Checked == true ? true : false;
            objClient.FkProjmanager = Convert.ToInt16(ddlProjectManager.SelectedValue);
            objClient.FkLeadtech = Convert.ToInt16(ddlTechnicalLead.SelectedValue);
            objClient.PatchingReport = cbPatchingReport.Checked == true ? true : false;
            objClient.PortalManagedFaxReport = chkPortalManagedFaxReport.Checked;

            objClient.Notes = txtNotes.Text.Trim();
            objClient.Directions = txtDirections.Text.Trim();
            objClient.WhatWeDoForYou = txtWhatwedo.Text.Trim();
            objClient.WhatWeCanDoForYou = txtWhatwecando.Text.Trim();

            if (ClientId == 0)
            {
                objClient.Created = DateTime.Now.ToString("ddMMyy hhmmss");
                objClient.Rectype = "C";
            }
            else
            {
                if (ddlBillingType.SelectedItem.Text != "Credit Card")
                    objClient.BillingCreditCard = Convert.ToBoolean(false);
                else
                    objClient.BillingCreditCard = Convert.ToBoolean(true);
            }

            objClient.LastUpdated = DateTime.Now.ToString("ddMMyy hhmmss");
            objClient.LastUpdatedby = DesktopShared.User.UserName;           

            objClient.Save();
            objClient.Refetch();
            //Sales Person
            PredicateExpression salesPersonFilter = new PredicateExpression();
            salesPersonFilter.Add(DesktopShared.HelperClasses.ClientEmployeesalesFields.FkClient == objClient.Pclient);
            ISortExpression prioritySort = new SD.LLBLGen.Pro.ORMSupportClasses.SortExpression();
            var res = DesktopShared.CollectionClasses.ClientEmployeesalesCollection.GetMultiAsDataTable(salesPersonFilter, 0, prioritySort);

            foreach (DataRow dr in res.Rows)
            {
                int pkey = Convert.ToInt16(dr["PemployeeSalesperson"]);
                ClientEmployeesalesEntity objSalesPerson = new ClientEmployeesalesEntity(pkey);
                objSalesPerson.Delete();
                objSalesPerson.Save();
                
            }

            foreach (ListItem li in ddlSalesPerson.Items)
            {
                if (li.Selected)
                {
                    ClientEmployeesalesEntity objSalesPerson = new ClientEmployeesalesEntity();
                    objSalesPerson.FkClient = objClient.Pclient;// ClientId;
                    objSalesPerson.FkEmployee = li.Value;
                    objSalesPerson.Created = DateTime.Now;
                    objSalesPerson.LastUpdated = DateTime.Now.ToString("ddMMyy hhmmss");
                    objSalesPerson.LastUpdatedby = DesktopShared.User.UserName;
                    objSalesPerson.Save();

                }
            }
            //End of Sales Person

            #region client Type
            PredicateExpression ClientTypeFilter = new PredicateExpression();
            ClientTypeFilter.Add(ClientTypeClientIdFields.Fkclient == objClient.Pclient);
            ISortExpression clientTypeSort = new SD.LLBLGen.Pro.ORMSupportClasses.SortExpression();
            var resclienttype = ClientTypeClientIdCollection.GetMultiAsDataTable(ClientTypeFilter, 0, clientTypeSort);

            foreach (DataRow dr in resclienttype.Rows)
            {
                int pkey = Convert.ToInt16(dr["Id"]);
                ClientTypeClientIdEntity objClientTypeClient = new ClientTypeClientIdEntity(pkey);
                objClientTypeClient.Delete();
                objClientTypeClient.Save();

            }

            foreach (ListItem _clienttype in lbClientType.Items)
            {
                if (_clienttype.Selected)
                {
                    ClientTypeClientIdEntity _assignclienttype = new ClientTypeClientIdEntity();
                    _assignclienttype.Fkclient= objClient.Pclient;
                    _assignclienttype.Fkclienttypeid = Convert.ToInt32(_clienttype.Value.Trim());
                    _assignclienttype.CreatedDate = DateTime.Now;
                    _assignclienttype.CreatedBy= DesktopShared.User.UserID;
                    _assignclienttype.LastUpdateDate = DateTime.Now;
                    _assignclienttype.LastUpdateBy = DesktopShared.User.UserID;
                    _assignclienttype.Save();
                }
            }
            #endregion

            #region Client Services
            PredicateExpression ClientServicesFilter = new PredicateExpression();
            ClientServicesFilter.Add(ClientClientServiceFields.FkClient == objClient.Pclient);
            ISortExpression clientservicesaSort = new SD.LLBLGen.Pro.ORMSupportClasses.SortExpression();
            var resclientservices = ClientClientServiceCollection.GetMultiAsDataTable(ClientServicesFilter, 0, clientservicesaSort);
            foreach (DataRow dr in resclientservices.Rows)
            {
                int pkey = Convert.ToInt16(dr["Id"]);
                ClientClientServiceEntity objClientclientservice = new ClientClientServiceEntity(pkey);
                objClientclientservice.Delete();
                objClientclientservice.Save();
            }

            //List<string> _clientclientservices = lbClientService.SelectedValues;
            //for (int i = 0; i <= _clientclientservices.Count - 1; i++)
            //{
            //    ClientClientServiceEntity _cclientservice = new ClientClientServiceEntity();
            //    _cclientservice.FkClient = objClient.Pclient;
            //    _cclientservice.FkClienServices = Convert.ToInt32(_clientclientservices[i].ToString().Trim());
            //    _cclientservice.CreatedDate = DateTime.Now;
            //    _cclientservice.Createdby = DesktopShared.User.UserID;
            //    _cclientservice.Save();
            //}
            #endregion

            if (NeedToDeactivateContacts)
                DeactivateContacts(objClient.Pclient);
           
            return objClient;

        }
        catch (Exception ex)
        {
            DisplayMessage(String.Format("Error occured while {0} the record - {1}",
                      ClientId == 0 ? "adding" : "updating",
                      ex.ToString(),
                      DateTime.Now,
                      ClientId == 0 ? "" : ""
                      ),
                      Bootstrap.Alert.AlertType.Danger);
            return null;
        }

        
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        string errorbilling = string.Empty;
        string errorphone = string.Empty;

        if (chkActive.Checked == false)
        {
            vsClient.Enabled = false;
        }
        else
        {
            if (lblClientCode.Text == string.Empty)
                cvRequiredClientCode.IsValid = false;
            if (ddlPriority.SelectedValue == string.Empty)
                cvRequiredPriority.IsValid = false;
            litMessage.Visible = false;
            vsClient.Enabled = true;
        }
        if (Page.IsValid || chkActive.Checked == false)
        {
            if (ddlCountry.CountryId == 1)
            {
                if (CheckPhone(lblPhone, "Phone", ref errorphone) == false)
                {
                    DisplayMessage(errorphone, Bootstrap.Alert.AlertType.Danger);
                    return;
                }
                if (CheckPhone(lblPhone, "Fax", ref errorphone) == false)
                {
                    DisplayMessage(errorphone, Bootstrap.Alert.AlertType.Danger);
                    return;
                }
            }

            DesktopShared.EntityClasses.ClientEntity objClient = SaveClientInformation();
            if (objClient != null && objClient.Pclient > 0)
            {

                if (ClientId == 0)
                    ClientTabs.EnableTabs(objClient.Pclient);

                DisplayMessage(String.Format("Client Information has been {0} - {1}.{2}",
                    ClientId == 0 ? "added" : "updated",
                    DateTime.Now,
                    ClientId == 0 ? "" : ""
                    ),
                    Bootstrap.Alert.AlertType.Success);
                ClientId = objClient.Pclient;

                int _id;
                if (Int32.TryParse(Request.QueryString["ClientId"], out _id))
                {
                    if (_id == 0)
                        Response.Redirect(String.Format("/Client/ClientContacts.aspx?ClientId={0}&TabIndex=1", ClientId));
                    else
                        //Response.Redirect(String.Format("/Client/ClientDetail.aspx?ClientId={0}", ClientId));
                       LoadClientInformation();
                }
            }
        }
    }

    /// <summary>
    /// validate auto cc
    /// </summary>
    /// <param name="source"></param>
    /// <param name="args"></param>
    protected void cvAutoCc_ServerValidate(object source, ServerValidateEventArgs args)
    {
        #region remark because new function for client auto cc
        //args.IsValid = true;

        //string _emails = txtAutoCC.Text.Trim();
        //if (String.IsNullOrWhiteSpace(_emails))
        //    return;

        //if (_emails.EndsWith(";") || _emails.EndsWith(","))
        //{
        //    args.IsValid = false;
        //    //cvAutoCc.ErrorMessage = "Invalid ending character for Auto CC";
        //    return;
        //}

        //_emails = _emails.Replace(",", ";");
        //System.Text.StringBuilder _sb = new System.Text.StringBuilder();
        //foreach (string _email in _emails.Split(';'))
        //{
        //    if (String.IsNullOrWhiteSpace(_email))
        //        _sb.Append("(blank)<br>");
        //    else if (!DesktopShared.Utility.IsValidEmailAddress(_email))
        //        _sb.Append(String.Format("{0}<br>", _email));
        //}
        //if (_sb.Length > 0)
        //{
        //    //cvAutoCc.ErrorMessage = String.Format("Invalid Auto Cc Email:<br />{0}", _sb.ToString());
        //    args.IsValid = false;
        //    return;
        //}
        #endregion
    }

    /// <summary>
    /// validate minimum of 1 domain assigned to client
    /// </summary>
    /// <param name="source"></param>
    /// <param name="args"></param>
    protected void cvDomains_ServerValidate(object source, ServerValidateEventArgs args)
    {
        args.IsValid = !chkActive.Checked || DesktopShared.Client.Domain.GetCount(ClientId, true) > 0;
    }

    /// <summary>
    /// validate client code is unique
    /// </summary>
    /// <param name="source"></param>
    /// <param name="args"></param>
    protected void cvClientCode_ServerValidate(object source, ServerValidateEventArgs args)
    {
        args.IsValid = DesktopShared.Client.IsUniqueCode(lblClientCode.Text.Trim(), ClientId > 0 ? ClientId : (int?)null);
    }

    /// <summary>
    /// validate sentinelOne site name
    /// </summary>
    /// <param name="source"></param>
    /// <param name="args"></param>
    protected void cvSentinelOneSiteName_ServerValidate(object source, ServerValidateEventArgs args)
    {
        //<asp:CustomValidator ID="cvSentinelOneSiteName" runat="server" ValidateEmptyText="false" ErrorMessage="SentinelOne Site Name already in use" ValidationGroup="vgClient" 
            //OnServerValidate="cvSentinelOneSiteName_ServerValidate" Display="None" />
        args.IsValid = true;
        var siteName = txtSentinelOneSiteName.Text.Trim();
        if (String.IsNullOrWhiteSpace(siteName))
            return;

        args.IsValid = DesktopShared.Client.SentinelOne.IsUniqueSiteName(siteName, ClientId > 0 ? ClientId : (int?)null);
    }

    #region Deactivate client contacts when client is changed from Active to Inactive
    private void DeactivateContacts(int ClientId)
    {
        DesktopShared.CollectionClasses.ClientContactCollection clientcontact_c = new ClientContactCollection();
        SD.LLBLGen.Pro.ORMSupportClasses.IPredicateExpression filter = new SD.LLBLGen.Pro.ORMSupportClasses.PredicateExpression(DesktopShared.HelperClasses.ClientContactFields.FkClient == ClientId );
        clientcontact_c.GetMulti(filter);
        int cnt = clientcontact_c.Count;
        if (cnt > 0)
        {            
            string updatetime = DateTime.Now.ToString();
            for (int i = 0; i < cnt; i++)
            {
                clientcontact_c[i].Active = "N";                
                clientcontact_c[i].LastUpdated = updatetime;
                clientcontact_c[i].LastUpdatedby = DesktopShared.User.UserName;
                clientcontact_c[i].Save();
            }            
        }
        filter = null; clientcontact_c.Dispose(); clientcontact_c = null;
        
    }
    #endregion

    #region QuoteWerks
    /// <summary>
    /// Add contact to the next "Bit By Bit to QuoteWerks" sync process.
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSetQWSync_Click(object sender, EventArgs e)
    {
        try
        {
            DesktopShared.CollectionClasses.ClientContactCollection objClientsContact = new ClientContactCollection();
            SD.LLBLGen.Pro.ORMSupportClasses.IPredicateExpression filter = new SD.LLBLGen.Pro.ORMSupportClasses.PredicateExpression(DesktopShared.HelperClasses.ClientContactFields.FkClient == ClientId);
            filter.AddWithAnd(DesktopShared.HelperClasses.ClientContactFields.Active == 'Y');
            objClientsContact.GetMulti(filter);
            int cnt = objClientsContact.Count;

            for (int i = 0; i < cnt; i++)
            {
                objClientsContact[i].LastUpdatedby = DesktopShared.User.UserName;
                objClientsContact[i].Save();
            }
            objClientsContact.Dispose(); objClientsContact = null;
            filter = null;
            DisplayMessage(String.Format("{0} been added to the next QuoteWerks Sync.", cnt > 1 ? cnt.ToString() + " contacts have " : "1 contact has " ), Bootstrap.Alert.AlertType.Success);
        }
        catch
        {

            DisplayMessage(String.Format("Unable to add contact to the next QuoteWerks Sync."), Bootstrap.Alert.AlertType.Danger, false);
        }

    }
    #endregion

    #region IT Glue
    protected void btnITGlueSync_Click(object sender, EventArgs e)
    {

        try
        {
            DesktopShared.EntityClasses.ClientEntity entity = new ClientEntity(ClientId);
            string org = entity.Company.Trim();
            entity = null;

            int OrganizationId = 0;

            var _request = new RestSharp.RestRequest("/organizations?filter[name]=" + org, RestSharp.Method.GET);
            _request.RequestFormat = RestSharp.DataFormat.Json;
            _request.AddHeader("x-api-key", ConfigurationManager.AppSettings["ITGlueAPIKey"]);
            _request.AddHeader("Content-Type", "application/vnd.api+json");


            var _response = _client.Execute(_request);
            dynamic organizations = Newtonsoft.Json.JsonConvert.DeserializeObject(_response.Content);

            if (organizations.data.Count > 0)
            {
                OrganizationId = organizations.data[0].id;
            }
            else
            {
                DisplayMessage(String.Format("Organization is not found in IT Glue.  Please add with exact match of name and then retry."), Bootstrap.Alert.AlertType.Danger, false);
                return;
            }
            
            
            DesktopShared.CollectionClasses.ClientContactCollection objClientsContact = new ClientContactCollection();
            SD.LLBLGen.Pro.ORMSupportClasses.IPredicateExpression filter = new SD.LLBLGen.Pro.ORMSupportClasses.PredicateExpression(DesktopShared.HelperClasses.ClientContactFields.FkClient == ClientId);
            filter.AddWithAnd(DesktopShared.HelperClasses.ClientContactFields.Active == 'Y');
            objClientsContact.GetMulti(filter);
            int cnt = objClientsContact.Count;

            int successCount = 0;
            int failureCount = 0;

            for (int i = 0; i < cnt; i++)
            {
                bool blnSuccess = ITGlueSync(objClientsContact[i], OrganizationId);
                if (blnSuccess)
                    successCount = successCount + 1;
                else
                    failureCount = failureCount + 1;
            }
            objClientsContact.Dispose(); objClientsContact = null;
            filter = null;
            DisplayMessage(String.Format("{0} Contact(s). {1} added/updated to ITGlue. {2} failure(s).", cnt.ToString(), successCount.ToString(), failureCount.ToString()), Bootstrap.Alert.AlertType.Success);
        }
        catch (Exception ex)
        {

            DisplayMessage(String.Format("Unable to add contacts to IT Glue" + ex.ToString()), Bootstrap.Alert.AlertType.Danger, false);
        } 

    }

    private bool ITGlueSync(DesktopShared.EntityClasses.ClientContactEntity objClientsContact, int OrganizationId)
    {
      

       var _request = new RestSharp.RestRequest("/organizations/" + OrganizationId.ToString() + "/relationships/contacts/?filter[first-name]=" + objClientsContact.First.Trim() + "&filter[last-name]" + objClientsContact.Last.Trim(), RestSharp.Method.GET);
       _request.RequestFormat = RestSharp.DataFormat.Json;
       _request.AddHeader("x-api-key", ConfigurationManager.AppSettings["ITGlueAPIKey"]);
       _request.AddHeader("Content-Type", "application/vnd.api+json");
        
        
        var  _response = _client.Execute(_request);


        dynamic contacts = Newtonsoft.Json.JsonConvert.DeserializeObject(_response.Content);

        int myCount = contacts.data.Count;

        if (contacts.data.Count > 0)
        {
            return PatchContact(contacts.data[0], objClientsContact);
        }
        else
        {
            return PutContact(OrganizationId.ToString(), objClientsContact);
        }
    }
    public bool PatchContact(dynamic ITGlueContact, DesktopShared.EntityClasses.ClientContactEntity entity)
    {
        var contactId = ITGlueContact.id;
        var organizationId = ITGlueContact.attributes["organization-id"].Value;

        var contactFirstName = ITGlueContact.attributes["first-name"].Value;
        var contactLastName = ITGlueContact.attributes["last-name"].Value;
        var contactName = ITGlueContact.attributes["name"].Value;


        var _request = new RestSharp.RestRequest("/organizations/" + organizationId + "/relationships/contacts/" + contactId, RestSharp.Method.PATCH);

        _request.AddHeader("x-api-key", ConfigurationManager.AppSettings["ITGlueAPIKey"]);

        var myroot = new Contact.RootObject();

        var myobj = new Contact.Data();
        myobj.type = "contacts";

        var myattributes = new Contact.Attributes();
        myattributes.firstname = contactFirstName;
        myattributes.lastname = contactLastName;
        //myattributes.title = null;
        myattributes.name = contactName;

        ///Logging.WriteMessage("Updating " + contactName);
        var emaillist = new List<Contact.ContactEmail>();

        if (!string.IsNullOrWhiteSpace(entity.Email.Trim()))
        {
            var myemail = new Contact.ContactEmail();
            myemail.value = entity.Email.Trim();
            myemail.primary = true;
            myemail.labelname = "Work";

            emaillist.Add(myemail);
            //Logging.WriteMessage("Updating  Work Email:" + entity.Email.Trim());
        }

        if (!string.IsNullOrWhiteSpace(entity.Email2.Trim()))
        {
            var myemail2 = new Contact.ContactEmail();
            myemail2.value = entity.Email2.Trim();
            myemail2.primary = false;
            myemail2.labelname = "Home";
            emaillist.Add(myemail2);

            //Logging.WriteMessage("Updating Home Email:" + entity.Email2.Trim());

        }

        if (!string.IsNullOrWhiteSpace(entity.Email3.Trim()))
        {
            var myemail = new Contact.ContactEmail();
            myemail.value = entity.Email3.Trim();
            myemail.primary = false;
            myemail.labelname = "Other";

            emaillist.Add(myemail);
            //Logging.WriteMessage("Updating Other Email:" + entity.Email3.Trim());
        }

        var phonelist = new List<Contact.ContactPhone>();

        if (!string.IsNullOrWhiteSpace(entity.Busphone.Trim()))
        {
            var myphone = new Contact.ContactPhone();
            myphone.value = entity.Busphone.Trim();
            myphone.primary = true;
            myphone.labelname = "Work";
            myphone.extension = entity.Busext;

            phonelist.Add(myphone);
            //Logging.WriteMessage("Updating Work Phone:" + entity.Busphone.Trim());
        }
        if (!string.IsNullOrWhiteSpace(entity.Cellphone.Trim()))
        {
            var myphone2 = new Contact.ContactPhone();
            myphone2.value = entity.Cellphone.Trim();
            myphone2.primary = false;
            myphone2.labelname = "Mobile";
            phonelist.Add(myphone2);
            //Logging.WriteMessage("Updating Mobile Phone:" + entity.Cellphone.Trim());
        }

        if (!string.IsNullOrWhiteSpace(entity.Faxphone.Trim()))
        {
            var myphone2 = new Contact.ContactPhone();
            myphone2.value = entity.Faxphone.Trim();
            myphone2.primary = false;
            myphone2.labelname = "Fax";
            phonelist.Add(myphone2);
            // Logging.WriteMessage("Updating Fax Phone:" + entity.Faxphone.Trim());
        }

        if (emaillist.Count > 0)
            myattributes.ContactEmails = emaillist;

        if (phonelist.Count > 0)
            myattributes.ContactPhones = phonelist;
        myobj.attributes = myattributes;

        myroot.data = myobj;


        string myJson = Newtonsoft.Json.JsonConvert.SerializeObject(myroot);


        _request.AddParameter("application/vnd.api+json", myJson, RestSharp.ParameterType.RequestBody);



        var _response = _client.Execute(_request);
        bool blnSuccess = false;

        if (_response.StatusCode != System.Net.HttpStatusCode.OK)
        {
            //Logging.WriteMessage("Error Updating Contact " + _response.StatusCode.ToString());
            blnSuccess = false;
        }
        else
        {
            //Logging.WriteMessage("Contact Updated");
            blnSuccess = true;
        }
        var mycontent = _response.Content;


        return blnSuccess;
    }

    public static bool PutContact(string organizationId, DesktopShared.EntityClasses.ClientContactEntity entity)
    {



        var _request = new RestSharp.RestRequest("/organizations/" + organizationId + "/relationships/contacts", RestSharp.Method.POST);
        _request.AddHeader("x-api-key", ConfigurationManager.AppSettings["ITGlueAPIKey"]);


        var myroot = new Contact.RootObject();

        var myobj = new Contact.Data();
        myobj.type = "contacts";

        var myattributes = new Contact.Attributes();
        myattributes.firstname = entity.First.Trim();
        myattributes.lastname = entity.Last.Trim();
        myattributes.name = entity.First.Trim() + " " + entity.Last.Trim();
        // Logging.WriteMessage("Creating Contact:" + entity.First.Trim() + " " + entity.Last.Trim());


        var emaillist = new List<Contact.ContactEmail>();



        if (!string.IsNullOrWhiteSpace(entity.Email.Trim()) && entity.Email.Trim().ToLower() != "n/a")
        {
            var myemail = new Contact.ContactEmail();
            myemail.value = entity.Email.Trim();
            myemail.primary = true;
            myemail.labelname = "Work";
            emaillist.Add(myemail);
            //Logging.WriteMessage("Adding  Work Email:" + entity.Email.Trim());
        }

        if (!string.IsNullOrWhiteSpace(entity.Email2.Trim()))
        {

            var myemail2 = new Contact.ContactEmail();
            myemail2.value = entity.Email2.Trim();
            myemail2.primary = false;
            myemail2.labelname = "Home";


            emaillist.Add(myemail2);
            //Logging.WriteMessage("Adding Home Email:" + entity.Email2.Trim());
        }

        if (!string.IsNullOrWhiteSpace(entity.Email3.Trim()))
        {

            var myemail2 = new Contact.ContactEmail();
            myemail2.value = entity.Email3.Trim();
            myemail2.primary = false;
            myemail2.labelname = "Other";

            emaillist.Add(myemail2);
            //Logging.WriteMessage("Adding Other Email:" + entity.Email3.Trim());
        }

        var phonelist = new List<Contact.ContactPhone>();
        if (!string.IsNullOrWhiteSpace(entity.Busphone.Trim()))
        {
            var myphone = new Contact.ContactPhone();
            myphone.value = entity.Busphone.Trim();
            myphone.primary = true;
            myphone.labelname = "Work";
            myphone.extension = entity.Busext.Trim();
            phonelist.Add(myphone);

            //Logging.WriteMessage("Adding Work phone:" + entity.Busphone.Trim());
        }


        if (!string.IsNullOrWhiteSpace(entity.Cellphone.Trim()))
        {
            var myphone2 = new Contact.ContactPhone();
            myphone2.value = entity.Cellphone.Trim();
            myphone2.primary = false;
            myphone2.labelname = "Mobile";
            phonelist.Add(myphone2);
            //Logging.WriteMessage("Adding Mobile phone:" + entity.Cellphone.Trim());
        }

        if (!string.IsNullOrWhiteSpace(entity.Faxphone.Trim()))
        {
            var myphone2 = new Contact.ContactPhone();
            myphone2.value = entity.Faxphone.Trim();
            myphone2.primary = false;
            myphone2.labelname = "Fax";
            phonelist.Add(myphone2);
            //Logging.WriteMessage("Adding Fax phone:" + entity.Faxphone.Trim());
        }
        if (emaillist.Count > 0)
            myattributes.ContactEmails = emaillist;

        if (phonelist.Count > 0)
            myattributes.ContactPhones = phonelist;

        myobj.attributes = myattributes;

        myroot.data = myobj;


        string myJson = Newtonsoft.Json.JsonConvert.SerializeObject(myroot);


        _request.AddParameter("application/vnd.api+json", myJson, RestSharp.ParameterType.RequestBody);


        bool blnSuccess = false;
        var _response = _client.Execute(_request);

        if (_response.StatusCode != System.Net.HttpStatusCode.Created)
        {
            //Logging.WriteMessage("Error Creating Contact " + _response.StatusCode.ToString());
            blnSuccess = false;
        }
        else
        {
            //Logging.WriteMessage("Contact Created");
            blnSuccess = true;
        }
        var mycontent = _response.Content;


        return blnSuccess;
    }
    #region contact class
    public class Contact
    {

        public class ContactEmail
        {
            public string value { get; set; }
            public bool primary { get; set; }
            [JsonProperty("label-name")]
            public string labelname { get; set; }
        }

        public class ContactPhone
        {
            public string value { get; set; }
            public string extension { get; set; }
            public bool primary { get; set; }
            [JsonProperty("label-name")]
            public string labelname { get; set; }
        }

        public class Attributes
        {
            [JsonProperty("first-name")]

            public string firstname { get; set; }
            [JsonProperty("last-name")]
            public string lastname { get; set; }
            // public string title { get; set; }
            public string name { get; set; }


            // [JsonProperty("location-id")]
            //public int locationid { get; set; }
            [JsonProperty("contact-emails")]
            public List<ContactEmail> ContactEmails { get; set; }
            [JsonProperty("contact-phones")]
            public List<ContactPhone> ContactPhones { get; set; }
        }

        public class Data
        {
            public string type { get; set; }
            public Attributes attributes { get; set; }
        }

        public class RootObject
        {
            public Data data { get; set; }
        }


    }

    #endregion
    #endregion

    #region new additional

    private void BindGrid()
    {
        rgAutoClientCC.DataSource = Search(ClientId);
    }

    protected void lbAutoCC_Click(object sender, EventArgs e)
    {
        if (txtAutoCC.Text.Split(',').Length > 1)
        {
            DisplayMessage(String.Format("Only for single email address {0}", DateTime.Now), Bootstrap.Alert.AlertType.Danger);
            return;
        }
        else if (txtAutoCC.Text.Split(';').Length > 1)
        {
            DisplayMessage(String.Format("Only for single email address {0}", DateTime.Now), Bootstrap.Alert.AlertType.Danger);
            return;
        }

        if (DesktopShared.Utility.IsValidEmailAddress(txtAutoCC.Text) == false)
        {
            DisplayMessage(String.Format("Invalid Email address for auto client cc {0} - {1}",txtAutoCC.Text, DateTime.Now), Bootstrap.Alert.AlertType.Danger);
            return;
        }
        if (checkEmail(ClientId, txtAutoCC.Text) > 0)
        {
            DisplayMessage(String.Format("Email address {0} already existed - {1}", txtAutoCC.Text, DateTime.Now), Bootstrap.Alert.AlertType.Danger);
            return;
        }
        
        if (AddAutoClientCC(ClientId, txtAutoCC.Text) == false)
            DisplayMessage(String.Format("Error occured while added client cc {0}", DateTime.Now), Bootstrap.Alert.AlertType.Danger);
        else
        {
            DisplayMessage(String.Format("Client email auto cc has been added {0}", DateTime.Now), Bootstrap.Alert.AlertType.Success);
            BindGrid();
            rgAutoClientCC.DataBind();
            txtAutoCC.Text = string.Empty;
        }
    }

    protected void rgAutoClientCC_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindGrid();
    }

    protected void rgAutoClientCC_ItemCommand(object sender, GridCommandEventArgs e)
    {
        int _auditUserId = DesktopShared.User.UserID;
        string command = e.CommandArgument.ToString().Trim();

        switch (command)
        {
            case "DeleteRecord":
                if (true)
                {
                    GridDataItem item = (GridDataItem)e.Item;

                    lblModalDeleteTitle.Text = "Delete client auto cc";
                    lblModalDeleteWording.Text = "Are you sure want to delete " + item["Email"].Text.Trim() + " ? ";
                    hfDelete.Value = item["Id"].Text;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "myModalDelete", "$('#myModalDelete').modal('show');", true);
                }
                break;
        }
    }

    protected void Decision_Command(object sender, CommandEventArgs e)
    {
        int _auditUserId = DesktopShared.User.UserID;
        string str = e.CommandArgument.ToString().Trim();

        switch (str)
        {
            case "Delete":
                if (DeleteClientCC(Convert.ToInt32(hfDelete.Value)))
                {
                    DisplayMessage(String.Format("Client auto cc email has been {0} - {1}", "Deleted", DateTime.Now), Bootstrap.Alert.AlertType.Success);
                    BindGrid();
                    rgAutoClientCC.DataBind();
                }
                else
                    DisplayMessage("Something went wrong", Bootstrap.Alert.AlertType.Danger);
                break;
        }
    }
    private void CountryCheck()
    {
        if (ddlCountry.CountryId > 1)
        {
            ddlState.Visible = false;
            txtState.Visible = true;
            lblPhone.Visible = false;
            txtPhone.Visible = true;
            txtFax.Visible = true;
            lblFax.Visible = false;
            lbZip.Text = "Zip/Postal Code";
            lbState.Text = "State/Provence/Region";
        }
        else
        {
            ddlState.Visible = true;
            BindStates();
            txtState.Visible = false;
            lblPhone.Visible = true;
            txtPhone.Visible = false;
            txtFax.Visible = false;
            lblFax.Visible = true;
            lbZip.Text = "Zip";
            lbState.Text = "State";
        }
    }
    private bool CheckPhone(TextBox txt, string desc, ref string error)
    {
        if (txt.Text.Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "").Length > 10)
        {
            error = String.Format("{0} For Country US should be 10", desc);
            return false;
        }
        else
            return true;
    }

    #region DB Stuff

    private static ClientAutoCcCollection Search(int id)
    {
        ClientAutoCcCollection _autoclientcc = new ClientAutoCcCollection();

        IPredicateExpression _orFilter = new PredicateExpression();

        if (id != null)
            _orFilter.AddWithAnd(ClientAutoCcFields.ClientId == id);

        //sort expression
        ISortExpression _autoclientccSort = new SD.LLBLGen.Pro.ORMSupportClasses.SortExpression();
        _autoclientccSort.Add(ClientAutoCcFields.Created | SortOperator.Descending);

        //fetch
        _autoclientcc.GetMulti(_orFilter, 0, _autoclientccSort);

        //return
        return _autoclientcc;
    }

    private bool AddAutoClientCC(int clientid, string email)
    {
        try
        {
            ClientAutoCcEntity _clientautocc = new ClientAutoCcEntity();

            _clientautocc.ClientId = clientid;
            _clientautocc.Email = email;
            _clientautocc.Created = DateTime.Now;
            _clientautocc.CreatedByUserId = DesktopShared.User.UserID;
            _clientautocc.Save();

            _clientautocc.Refetch();

            return true;
        }
        catch
        {
            return false;
        }
    }

    private bool DeleteClientCC(int clientid)
    {
        try
        {
            ClientAutoCcEntity _clientautocc = new ClientAutoCcEntity(clientid);
            _clientautocc.Delete();
            _clientautocc.Save();
            return true;
        }
        catch
        {
            return false;
        }
    }

    private int checkEmail(int clientid, string email)
    {
        ClientAutoCcCollection _clientauto = new ClientAutoCcCollection();

        IPredicateExpression _orFilter = new PredicateExpression();
        _orFilter.AddWithAnd(ClientAutoCcFields.ClientId == clientid);
        _orFilter.AddWithAnd(ClientAutoCcFields.Email == email);

        //fetch
        _clientauto.GetMulti(_orFilter, 0, null);

        //return
        return _clientauto.Count;
    }
    #endregion

    #endregion

    public string FName
    {
        get
        {
            if (ViewState["fname"] == null)
                return string.Empty;

            return ViewState["fname"].ToString();
        }
        set
        {
            ViewState["fname"] = value;
        }
    }

    public string LName
    {
        get
        {
            if (ViewState["lname"] == null)
                return string.Empty;

            return ViewState["lname"].ToString();
        }
        set
        {
            ViewState["lname"] = value;
        }
    }
    public string Email
    {
        get
        {
            if (ViewState["email"] == null)
                return string.Empty;

            return ViewState["email"].ToString();
        }
        set
        {
            ViewState["email"] = value;
        }
    }
}
