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
using System.Text;
using DesktopShared.EntityClasses;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public partial class Client_ClientContactAdd : BasePage
{
    private static RestSharp.RestClient _client = new RestSharp.RestClient("https://api.itglue.com/");

    /// <summary>
    /// page load
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        ConfigurePhoneValidations();
        ConfigureForDevice();

        DropDownList _ddl = ddlClientLocation.GetDropDownList();
        _ddl.AutoPostBack = true;
        _ddl.SelectedIndexChanged += ddlClientLocation_SelectedIndexChanged;

        _ddl = ddlCountry.GetDropDownList();
        _ddl.AutoPostBack = true;
        _ddl.SelectedIndexChanged += ddlCountry_SelectedIndexChanged;

        int _clientcontactid = 0;
        int.TryParse(BitByBit.Web.Request.GetString("ClientContactID").Trim(), out _clientcontactid);
        if (_clientcontactid == 0)
            phEmailCategory.Visible = false;

        int _clientid = 0;
        int.TryParse(BitByBit.Web.Request.GetString("ClientId").Trim(), out _clientid);
        if (_clientid == 651)
            btnSubmit.Visible = IsItBBBSuperAdmin();

        if (!this.IsPostBack)
        {
            SetUpPage();
            SetClientID();
            BindStates();
            BindPriority();
            BindDesignation();

            DesktopShared.EntityClasses.ClientContactEntity objClientContact = null;
            if (GetClientsContactId(ref objClientContact))
                LoadValues(objClientContact);

            populatedropdownlist();
        }

    }

    #region private methods

    private void ddlCountry_SelectedIndexChanged(object sender, EventArgs e)
    {
        CountryCheck();
    }

    /// <summary>
    /// confirue phone validation
    /// </summary>
    private void ConfigurePhoneValidations()
    {
        revPhone.ValidationExpression = DesktopShared.Utility.ValidPhoneNumberRegEx;
        revPhone.ErrorMessage = DesktopShared.Utility.InvalidPhoneNumberMessage("Phone");
        revCellPhone.ValidationExpression = DesktopShared.Utility.ValidPhoneNumberRegEx;
        revCellPhone.ErrorMessage = DesktopShared.Utility.InvalidPhoneNumberMessage("Cell Phone");
        revHomePhone.ValidationExpression = DesktopShared.Utility.ValidPhoneNumberRegEx;
        revHomePhone.ErrorMessage = DesktopShared.Utility.InvalidPhoneNumberMessage("Home Phone");
    }

    /// <summary>
    /// configure for device
    /// </summary>
    private void ConfigureForDevice()
    {
        bool _displayChosenScript = true;
        string _cssClass = "form-control select-chosen";
        if (DesktopShared.Utility.BrowserHelper.IsTabletOrSmartPhone(this.Request))
        {
            _displayChosenScript = false;
            _cssClass = "form-control";
        }

        ddlClientLocation.DisplayChosenScript = _displayChosenScript;
        ddlClientLocation.CssClass = _cssClass;
        lbClientLocation.DisplayChosenScript = _displayChosenScript;
        lbClientLocation.CssClass = _cssClass;
    }

    /// <summary>
    /// scroll to top of page
    /// </summary>
    private void ScrollToTop()
    {
        ScriptManager.RegisterClientScriptBlock(this, Page.GetType(), "ToTheTop", "ToTopOfPage();", true);
    }

    #region bind drop downs

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

    private void BindDesignation()
    {
        //clear items
        ddldesignation.ClearSelection();
        ddldesignation.Items.Clear();

        #region bind drop down list

        PredicateExpression descFilter = new PredicateExpression();

        ISortExpression descSort = new SD.LLBLGen.Pro.ORMSupportClasses.SortExpression();
        descSort.Add(DesktopShared.HelperClasses.DesignationFields.Name | SortOperator.Ascending);

        var res = DesktopShared.CollectionClasses.DesignationCollection.GetMultiAsDataTable(descFilter, 0, descSort);


        ddldesignation.DataSource = res;

        ddldesignation.DataTextField = "name";
        ddldesignation.DataValueField = "id";
        ddldesignation.DataBind();
        ddldesignation.DataSource = null;

        #endregion


        ListItem liDefault = new ListItem("Select Title", "");
        ddldesignation.Items.Insert(0, liDefault);
    }

    #endregion

    /// <summary>
    /// get user id from query string
    /// </summary>
    /// <param name="objClientsProductr"></param>
    /// <returns></returns>
    private bool GetClientsContactId(ref DesktopShared.EntityClasses.ClientContactEntity objClientContact)
    {
        int _id = -1;
        if (int.TryParse(BitByBit.Web.Request.GetString("ClientContactID").Trim(), out _id))
        {
            ClientsContactId = _id;
            if (_id == 0) //add new
                return true;

            //edit existing
            objClientContact = new DesktopShared.EntityClasses.ClientContactEntity(ClientsContactId.Value);
            if (objClientContact.Fields.State == EntityState.Fetched)
                return true;
        }
        pnlContainer.Visible = false;
        DisplayMessage(String.Format("Unable to fetch ClientsProduct Information.  ID = {0}", _id > 0 ? _id.ToString() : "N/A"), Bootstrap.Alert.AlertType.Danger, false);
        return false;
    }

    private void SetClientID()
    {
        try
        {
            ClientId = Convert.ToInt16(Request.QueryString["ClientId"]);
            hlBacktoList.NavigateUrl = "ClientContacts.aspx?ClientId=" + Convert.ToString(ClientId) + "&TabIndex=1";
            hlAddNewRecord.NavigateUrl = "ClientContactAdd.aspx?ClientId=" + Convert.ToString(ClientId) + "&ClientContactID=0";
        }
        catch
        {
            DisplayMessage(String.Format("Unable to Set Client ID "), Bootstrap.Alert.AlertType.Danger, false);
        }
    }

    /// <summary>
    /// load user details
    /// </summary>
    /// <param name="objClientsProduct"></param>
    private void LoadValues(DesktopShared.EntityClasses.ClientContactEntity objClientsContact)
    {
        try
        {
            bool _isNewRecord = ((objClientsContact == null) || (objClientsContact.Fields.State != SD.LLBLGen.Pro.ORMSupportClasses.EntityState.Fetched));

            if (_isNewRecord)
            {
                //btnSubmit.Text = "<i class=\"hi hi-ok\"></i> Add & Send Email";
                btnSetQWSync.Visible = false;
                chkPortalActive.Checked = true;
                rptClientPortalUserRole.BindRepeater();

                if (ClientId.HasValue)
                {
                    var objClient = new DesktopShared.EntityClasses.ClientEntity(ClientId.Value);
                    phLocations.Visible = objClient.UseTicketLocation;
                    if (phLocations.Visible)
                    {
                        //primary
                        ddlClientLocation.Active = true;
                        ddlClientLocation.ClientIdForLocation = ClientId.Value;
                        ddlClientLocation.PopulateDropDownList();

                        //secondary
                        lbClientLocation.ClientIdForLocation = ClientId.Value;
                        ConfigureLocations();
                    }
                }
                ddlCountry.CountryId = 1;
                return;
            }

            //btnSubmit.Text = "<i class=\"hi hi-ok\"></i> Submit";
            var objUser = DesktopShared.User.GetForClientContact(objClientsContact.PclientContact);
            if ((objUser == null) || (objUser.Fields.State != EntityState.Fetched))
            {
                DisplayMessage("Unable to fetch User entity.", Bootstrap.Alert.AlertType.Danger, false);
                phButtons.Visible = false;
                return;
            }

            UserId = objUser.Pusers;
            txtAddress1.Text = objClientsContact.Addr1.Trim();
            txtAddress2.Text = objClientsContact.Addr2.Trim();


            txtExt.Text = objClientsContact.Busext.Trim();
            txtCellPhone.Text = PhoneFormat.CellPhone.cellformat(objClientsContact.Cellphone.Trim());
            txtPhone.Text = PhoneFormat.CellPhone.cellformat(objClientsContact.Busphone.Trim());
            txtHomePhone.Text = objClientsContact.Homephone.Trim();
            txtCity.Text = objClientsContact.City.Trim();
            txtEmail.Text = objClientsContact.Email.Trim();           
            Email = objClientsContact.Email.Trim();
            txtFax.Text = objClientsContact.Faxphone.Trim();
            txtFirstName.Text = objClientsContact.First.Trim();
            txtLastName.Text = objClientsContact.Last.Trim();
            ddlCountry.CountryId = (int?)objClientsContact.Country == null ? 1 : (int?)objClientsContact.Country;
            rblNotificationPreferences.SelectedValue = objClientsContact.NotificationPreferences.ToString().Trim();
            CountryCheck();
            if (ddlCountry.CountryId > 1)
                txtState.Text= objClientsContact.StateProvenceRegion.Trim();
            else
                ddlState.SelectedValue = objClientsContact.State.Trim();

            txtZip.Text = objClientsContact.Zip.Trim();
            ddldesignation.SelectedValue = Convert.ToString(objClientsContact.DesignationId);
            ddlPriority.SelectedValue = Convert.ToString(objUser.RankingId);
            chkPortalActive.Checked = objUser.ClientPortalActive || objClientsContact.Active == "Y";
            phHelpDesk.Visible = objClientsContact.Client.UseHelpDesk;
            chkHelpDesk.Checked = objUser.UseHelpDesk;
            //ddlClientEmailCategory.SelectedValue = objClientsContact.FkClientemailcategory.;
            btnResetPassword.Visible = objUser.ClientPortalActive;
            btnPortalInvite.Visible = btnResetPassword.Visible;
            btnSetQWSync.Visible = true;
            rptClientPortalUserRole.SelectedIds = DesktopShared.User.Portal.Role.GetIds(objUser.Pusers);
            rptClientPortalUserRole.BindRepeater();
            btnPortalBilling.Visible = chkPortalActive.Checked && DesktopShared.User.Portal.Role.IsInRole(objUser.Pusers, DesktopShared.Role.Portal.Id.BillingAdministrator);
            btnShowPortalBillingLink.Visible = btnPortalBilling.Visible;
            phPortalBillingLink.Visible = false;
            cbkVIP.Checked = objClientsContact.Vip;
            txtEmail2.Text = objClientsContact.Email4;
            txtEmail3.Text = objClientsContact.Email5;
            txtEmail4.Text = objClientsContact.Email6;
            ddlMethodofContact.SelectedValue = objClientsContact.PreferredMethodOfContact;
            
            #region locations

            phLocations.Visible = objClientsContact.Client.UseTicketLocation;
            if (phLocations.Visible)
            {
                var _userLocations = DesktopShared.User.Client.Location.Get(objUser.Pusers);
                int? _primaryLocationId = null;
                foreach (var objLocation in _userLocations)
                {
                    if (objLocation.Primary && objLocation.ClientLocationId.HasValue)
                    {
                        _primaryLocationId = objLocation.ClientLocationId.Value;
                        break;
                    }
                }

                //primary
                if (_primaryLocationId.HasValue)
                    ddlClientLocation.ClientLocationId = _primaryLocationId;
                ddlClientLocation.Active = true;
                ddlClientLocation.ClientIdForLocation = objClientsContact.Client.Pclient;
                ddlClientLocation.PopulateDropDownList();

                //secondary
                lbClientLocation.ClientIdForLocation = objClientsContact.Client.Pclient;
                ConfigureLocations();

            }

            #endregion
        }
        catch(Exception ex)
        {
            DisplayMessage(String.Format("Unable to load Client Contact. " + ex.Message), Bootstrap.Alert.AlertType.Danger, false);
        }
    }

    /// <summary>
    /// configure for primary / additional locations
    /// </summary>
    private void ConfigureLocations()
    {
        ListBox _li = lbClientLocation.GetListBox();
        _li.Enabled = false;
        int? _primaryCLientLocationId = ddlClientLocation.ClientLocationId;
        if (!_primaryCLientLocationId.HasValue)
            return;

        _li.Enabled = true;
        lbClientLocation.Populate(DesktopShared.User.Client.Location.GetClientLocationIds(UserId.HasValue ? UserId.Value : -1));
        //litDebug.Text = String.Format("ids-{0}", String.Join(",", DesktopShared.User.Client.Location.GetIds(UserId.HasValue ? UserId.Value : -1).ToArray()));
        lbClientLocation.RemoveItem(_primaryCLientLocationId.Value);
    }

    /// <summary>
    /// primary location on on selected index changed
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ddlClientLocation_SelectedIndexChanged(object sender, EventArgs e)
    {
        ConfigureLocations();
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
    /// set up for initial page load
    /// </summary>
    private void SetUpPage()
    {
        txtFirstName.Focus();

        #region tab index

        short _tabIndex = 0;
        txtFirstName.TabIndex = ++_tabIndex;
        txtLastName.TabIndex = ++_tabIndex;
        txtEmail.TabIndex = ++_tabIndex;
        txtPhone.TabIndex = ++_tabIndex;
        txtExt.TabIndex = ++_tabIndex;
        txtCellPhone.TabIndex = ++_tabIndex;
        txtHomePhone.TabIndex = ++_tabIndex;
        txtFax.TabIndex = ++_tabIndex;
        txtAddress1.TabIndex = ++_tabIndex;
        txtAddress2.TabIndex = ++_tabIndex;
        txtCity.TabIndex = ++_tabIndex;
        ddlState.TabIndex = ++_tabIndex;
        txtZip.TabIndex = ++_tabIndex;
        ddlPriority.TabIndex = ++_tabIndex;
        ddldesignation.TabIndex = ++_tabIndex;
        chkPortalActive.TabIndex = ++_tabIndex;
        chkHelpDesk.TabIndex = ++_tabIndex;
        rptClientPortalUserRole.TabIndex = ++_tabIndex;
        ddlClientLocation.TabIndex = ++_tabIndex;
        lbClientLocation.TabIndex = ++_tabIndex;
        ddlClientEmailCategory.TabIndex = ++_tabIndex;
        btnSubmit.TabIndex = ++_tabIndex;
        btnResetPassword.TabIndex = ++_tabIndex;
        btnSetQWSync.TabIndex = ++_tabIndex;

        #endregion
    }

    #region it glue sync methods

    private void ITGlueSync(DesktopShared.EntityClasses.ClientContactEntity objClientsContact)
    {
        string org = objClientsContact.Client.Company.Trim();

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
            return;

        _request.Resource = "/organizations/" + OrganizationId.ToString() + "/relationships/contacts/?filter[first-name]=" + objClientsContact.First.Trim() + "&filter[last-name]" + objClientsContact.Last.Trim();
        _response = _client.Execute(_request);

        dynamic contacts = Newtonsoft.Json.JsonConvert.DeserializeObject(_response.Content);

        int myCount = contacts.data.Count;

        if (contacts.data.Count > 0)
        {
            PatchContact(contacts.data[0], objClientsContact);
        }
        else
        {
            PutContact(OrganizationId.ToString(), objClientsContact);
        }
    }

    private void PatchContact(dynamic ITGlueContact, DesktopShared.EntityClasses.ClientContactEntity entity)
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

        if (_response.StatusCode != System.Net.HttpStatusCode.OK)
        {
            //Logging.WriteMessage("Error Updating Contact " + _response.StatusCode.ToString());
        }
        else
        {
            //Logging.WriteMessage("Contact Updated");
        }
        var mycontent = _response.Content;


        return;
    }

    private static void PutContact(string organizationId, DesktopShared.EntityClasses.ClientContactEntity entity)
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



        var _response = _client.Execute(_request);

        if (_response.StatusCode != System.Net.HttpStatusCode.Created)
        {
            //Logging.WriteMessage("Error Creating Contact " + _response.StatusCode.ToString());
        }
        else
        {
            //Logging.WriteMessage("Contact Created");
        }
        var mycontent = _response.Content;


        return;
    }

    #endregion

    /// <summary>
    /// Save Client Information
    /// </summary>
    /// <param name="copy">True if creating copy of product</param>
    /// <returns></returns>
    private DesktopShared.EntityClasses.ClientContactEntity SaveClientContactInformation()
    {
        DesktopShared.EntityClasses.ClientContactEntity objClientsContact = ClientsContactId.Value > 0 ? new DesktopShared.EntityClasses.ClientContactEntity(ClientsContactId.Value) : new DesktopShared.EntityClasses.ClientContactEntity();

        try
        {
            DateTime _now = DateTime.Now;
            bool _isNew = false;
            int _auditUserId = DesktopShared.User.UserID;

            if (!string.IsNullOrWhiteSpace(txtEmail.Text.Trim()))
            {
                bool blnIsEmailUnique = IsUniqueEmail(ClientsContactId.Value, txtEmail.Text.Trim(), ClientId.Value);
                if (!blnIsEmailUnique)
                {
                    throw new Exception("Email is not unique for this client");
                }
            }
            
            if (ClientsContactId.Value == 0)
            {
                objClientsContact.Created = _now.ToString();
                _isNew = true;
            }

            objClientsContact.Addr1 = txtAddress1.Text;
            objClientsContact.Addr2 = txtAddress2.Text;
            objClientsContact.Busext = txtExt.Text;
            objClientsContact.Cellphone = txtCellPhone.Text;
            objClientsContact.Homephone = txtHomePhone.Text.Trim();
            objClientsContact.Busphone = txtPhone.Text;
            objClientsContact.City = txtCity.Text;
            objClientsContact.Email = txtEmail.Text;
            objClientsContact.Faxphone = txtFax.Text;
            objClientsContact.First = txtFirstName.Text;
            objClientsContact.FkClient = ClientId;
            objClientsContact.Last = txtLastName.Text;
            objClientsContact.LastUpdated = _now.ToString();
            objClientsContact.LastUpdatedby = DesktopShared.User.UserName;

            if (ddlCountry.CountryId > 1)
                objClientsContact.StateProvenceRegion = txtState.Text;
            else
                objClientsContact.State = ddlState.SelectedValue;

            //objClientsContact.FkClientemailcategory = ddlClientEmailCategory.ClientEmailCategoryId;
            objClientsContact.Zip = txtZip.Text;
            if (ddldesignation.SelectedIndex > 0)
                objClientsContact.DesignationId = Convert.ToInt16(ddldesignation.SelectedValue);
            else
                objClientsContact.DesignationId = null;
            objClientsContact.Active = chkPortalActive.Checked ? "Y" : "N";
            objClientsContact.Vip = cbkVIP.Checked ? true : false;
            objClientsContact.Email4 = txtEmail2.Text;
            objClientsContact.Email5 = txtEmail3.Text;
            objClientsContact.Email6 = txtEmail4.Text;
            objClientsContact.PreferredMethodOfContact = ddlMethodofContact.SelectedValue;
            objClientsContact.Country = ddlCountry.CountryId;
            objClientsContact.NotificationPreferences = rblNotificationPreferences.SelectedValue;
            objClientsContact.Save();

            //user entity
            var objUser = DesktopShared.User.GetForClientContact(objClientsContact.PclientContact);
            if ((objUser != null) && (objUser.Fields.State == EntityState.Fetched))
            {
                if (ddlPriority.SelectedIndex > 0)
                    objUser.RankingId = Convert.ToInt16(ddlPriority.SelectedValue);
                else
                    objUser.RankingId = null;

                objUser.ClientPortalActive = chkPortalActive.Checked;
                if (phHelpDesk.Visible)
                    objUser.UseHelpDesk = chkHelpDesk.Checked;
                objUser.Save();
                UserId = objUser.Pusers;

                DesktopShared.User.Portal.Role.Set(objUser.Pusers, rptClientPortalUserRole.GetSelectedIds(), DesktopShared.User.UserID);

                foreach (int id in rptClientPortalUserRole.GetSelectedIds())
                {
                    int? _emailcategory = EmailCategoryBillingAdministratorID();
                    if (_emailcategory != null)
                    {
                        if (CheckBillingAdministrator(id))
                        {
                            DesktopShared.ClientEmailCategory.AddEmailCategoriesClientContact(objClientsContact.PclientContact, (int)_emailcategory, _auditUserId);
                            rgEmailCategory.Rebind();// BindGridEmailCategory();
                        }
                    }
                }

                if (_isNew)
                    DesktopShared.RoleWidget.Portal.AddAllForUser(objUser.Pusers);
            }

            if (phLocations.Visible)
            {
                List<int> _selectedIds = lbClientLocation.SelectedValues;
                int _primaryId = -1;
                if (ddlClientLocation.ClientLocationId.HasValue)
                {
                    _primaryId = ddlClientLocation.ClientLocationId.Value;
                    _selectedIds.Add(_primaryId);
                }
                DesktopShared.User.Client.Location.Set(UserId.Value, _selectedIds, _primaryId, DesktopShared.User.UserID);
                //litDebug.Text = String.Join(",", _selectedIds.ToArray());
            };
            try
            {
               if (objClientsContact.Active.ToUpper() == "Y")
                    ITGlueSync(objClientsContact);
            }
            catch
            {
            }
        }
        catch (Exception ex)
        {
            DisplayMessage(String.Format("Error occured while {0} the record - {1}", ClientsContactId.Value == 0 ? "adding" : "updating", ex.ToString()),Bootstrap.Alert.AlertType.Warning);
            return null;
        }

        return objClientsContact;
    }
        
    private bool IsUniqueEmail(int contactId, string emailaddress, int clientId)
    {
        

        DesktopShared.CollectionClasses.ClientContactCollection _collection = new DesktopShared.CollectionClasses.ClientContactCollection();

        SD.LLBLGen.Pro.ORMSupportClasses.IPredicateExpression _filter = new SD.LLBLGen.Pro.ORMSupportClasses.PredicateExpression();
        _filter.Add(DesktopShared.HelperClasses.ClientContactFields.Email == emailaddress);
        _filter.Add(DesktopShared.HelperClasses.ClientContactFields.FkClient == clientId);
        _filter.Add(DesktopShared.HelperClasses.ClientContactFields.Active == "Y");


        if (contactId > 0 )
            _filter.Add(DesktopShared.HelperClasses.ClientContactFields.PclientContact != contactId);

        //return
        return _collection.GetDbCount(_filter) == 0;

       


        
    }
    private void BindGridEmailCategory()
    {
        rgEmailCategory.DataSource = DesktopShared.ClientEmailCategory.GetClientCategoriesEmailClientContact(ClientsContactId, "CC");
    }

    private bool CheckBillingAdministrator(int id)
    {
        try {
            DesktopShared.CollectionClasses.ClientPortalRoleCollection  cce = new DesktopShared.CollectionClasses.ClientPortalRoleCollection();
            IPredicateExpression cceFilter = new PredicateExpression();
            cceFilter.Add(DesktopShared.HelperClasses.ClientPortalRoleFields.Id == id);
            cceFilter.Add(DesktopShared.HelperClasses.ClientPortalRoleFields.Active == true);

            //sort
            ISortExpression _cceSort = new SD.LLBLGen.Pro.ORMSupportClasses.SortExpression();
            _cceSort.Add(DesktopShared.HelperClasses.ClientPortalRoleFields.Name | SortOperator.Ascending);

            cce.GetMulti(cceFilter, 0, _cceSort);

            if (cce.Count > 0)
            {
                if (cce[0].Name.Trim() == System.Web.Configuration.WebConfigurationManager.AppSettings["ClientEmailCategoryBilling"].Trim())
                    return true;
                else
                    return false;
            }
            else
                return false;
        }
        catch(Exception ex)
        { 
            return false; 
        }
    }

    private int? EmailCategoryBillingAdministratorID()
    {
        try
        {
            DesktopShared.CollectionClasses.ClientEmailCategoryCollection cce = new DesktopShared.CollectionClasses.ClientEmailCategoryCollection();
            IPredicateExpression cceFilter = new PredicateExpression();
            cceFilter.Add(DesktopShared.HelperClasses.ClientEmailCategoryFields.CategoryName == System.Web.Configuration.WebConfigurationManager.AppSettings["ClientEmailCategoryBilling"]);
            cceFilter.Add(DesktopShared.HelperClasses.ClientEmailCategoryFields.Active == true);
            cceFilter.Add(DesktopShared.HelperClasses.ClientEmailCategoryFields.AvailableFor == "CC");

            //sort
            ISortExpression _cceSort = new SD.LLBLGen.Pro.ORMSupportClasses.SortExpression();
            _cceSort.Add(DesktopShared.HelperClasses.ClientEmailCategoryFields.CategoryName | SortOperator.Ascending);

            cce.GetMulti(cceFilter, 0, _cceSort);

            if (cce.Count > 0)
                return cce[0].Id;
            else
                return null;
        }
        catch
        {
            return null;
        }
    }

    private int? GetEmailCategoryBillingAdministratorID(int clientcontact, int emailcategory)
    {
        try
        {
            DesktopShared.CollectionClasses.ClientCategoriesEmailCollection cce = new DesktopShared.CollectionClasses.ClientCategoriesEmailCollection();
            IPredicateExpression cceFilter = new PredicateExpression();
            cceFilter.Add(DesktopShared.HelperClasses.ClientCategoriesEmailFields.ClientContactId == clientcontact);
            cceFilter.Add(DesktopShared.HelperClasses.ClientCategoriesEmailFields.Active == "Y");
            cceFilter.Add(DesktopShared.HelperClasses.ClientCategoriesEmailFields.EmailCategoryId == emailcategory);

            //sort
            ISortExpression _cceSort = new SD.LLBLGen.Pro.ORMSupportClasses.SortExpression();
            _cceSort.Add(DesktopShared.HelperClasses.ClientCategoriesEmailFields.Created | SortOperator.Descending);

            cce.GetMulti(cceFilter, 0, _cceSort);

            if (cce.Count > 0)
                return cce[0].Id;
            else
                return null;
        }
        catch
        {
            return null;
        }
    }

    private int? GetClientPortalRole()
    {
        try
        {
            string _role = System.Web.Configuration.WebConfigurationManager.AppSettings["ClientEmailCategoryBilling"];
            DesktopShared.CollectionClasses.ClientPortalRoleCollection _clientportalrole = new DesktopShared.CollectionClasses.ClientPortalRoleCollection();

            SD.LLBLGen.Pro.ORMSupportClasses.IPredicateExpression _clientportalroleFilter = new SD.LLBLGen.Pro.ORMSupportClasses.PredicateExpression();
            _clientportalroleFilter.Add(DesktopShared.HelperClasses.ClientPortalRoleFields.Active == true);
            _clientportalroleFilter.Add(DesktopShared.HelperClasses.ClientPortalRoleFields.Name == _role);

            _clientportalrole.GetMulti(_clientportalroleFilter, 0);

            return _clientportalrole[0].Id;
        }
        catch
        {
            return null;
        }
    }

    private int? GetIDClientPortalRole()
    {
        try
        {
            int portalroleid = (int)GetClientPortalRole();
            DesktopShared.CollectionClasses.ClientPortalUserRoleCollection _clientportaluserrole = new DesktopShared.CollectionClasses.ClientPortalUserRoleCollection();

            SD.LLBLGen.Pro.ORMSupportClasses.IPredicateExpression _clientportaluserroleFilter = new SD.LLBLGen.Pro.ORMSupportClasses.PredicateExpression();
            _clientportaluserroleFilter.Add(DesktopShared.HelperClasses.ClientPortalUserRoleFields.UserId == UserId);
            _clientportaluserroleFilter.Add(DesktopShared.HelperClasses.ClientPortalUserRoleFields.ClientPortalRoleId == portalroleid);

            _clientportaluserrole.GetMulti(_clientportaluserroleFilter, 0);

            return _clientportaluserrole[0].Id;
        }
        catch
        {
            return null;
        }
    }

    private bool DeleteClientPortalRole(int id)
    {
        try
        {
            ClientPortalUserRoleEntity _clientportalrole = new ClientPortalUserRoleEntity(id);
            _clientportalrole.Delete();
            _clientportalrole.Save();

            return true;
        }
        catch
        {
            return false;
        }
    }

    private bool IsItBBBSuperAdmin()
    {
        try {
            int roleid = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["BBBSuperAdmin"].Trim());
            DesktopShared.CollectionClasses.UsersRoleCollection _usersrole = new DesktopShared.CollectionClasses.UsersRoleCollection();

            IPredicateExpression _filters = new PredicateExpression();

            _filters.Add(DesktopShared.HelperClasses.UsersRoleFields.UserId == DesktopShared.User.UserID);
            _filters.Add(DesktopShared.HelperClasses.UsersRoleFields.RoleId == roleid);

            _usersrole.GetMulti(_filters, 1, null);

            if (_usersrole.Count > 0)
                return true;
            else
                return false;
        }
        catch (Exception ex)
        { return false; }
    }

    #endregion

    #region protected events


    /// <summary>
    /// submit button on click
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (chkPortalActive.Checked == false)
        {
            revPhone.Enabled = false;
            revCellPhone.Enabled = false;
            revHomePhone.Enabled = false;
            vsProfile.Enabled = false;
        }
        else
        {
            rfvFirstName.Enabled = true;
            rfvLastName.Enabled = true;
            rfvEmail.Enabled = true;
        }
        if (Page.IsValid || chkPortalActive.Checked == false)
        {
            if (ddlCountry.CountryId == 1)
            {
                CheckPhone(txtPhone, "Phone");
                CheckPhone(txtCellPhone, "Cell Phone");
                CheckPhone(txtHomePhone, "Home Phone");
                CheckPhone(txtFax, "Fax");
            }

            DesktopShared.EntityClasses.ClientContactEntity objClientsContact = SaveClientContactInformation();
            if ((objClientsContact != null) && (objClientsContact.PclientContact > 0))
            {
                //if (ClientsContactId.Value == 0)
                //DesktopShared.User.Portal.Password.Reset(UserId.Value, DesktopShared.User.UserID, true);
                //DisplayMessage(String.Format("Client Contact Information has been {0} - {1}.", ClientsContactId.Value == 0 ? "added and invitation email sent" : "updated", DateTime.Now), Bootstrap.Alert.AlertType.Success);
                DisplayMessage(String.Format("Client Contact Information has been {0} - {1}.", ClientsContactId.Value == 0 ? "added" : "updated", DateTime.Now), Bootstrap.Alert.AlertType.Success);
                ClientsContactId = objClientsContact.PclientContact;
                LoadValues(objClientsContact);
            }
        }
    }

    /// <summary>
    /// reset password button on click
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnResetPassword_Click(object sender, EventArgs e)
    {
        string err = "";
        if (Page.IsValid)
        {
            string mailcc = System.Web.Configuration.WebConfigurationManager.AppSettings["ClientSetupCCBBBSalesPerson"];
            mailcc += "," + DesktopShared.Client.ClientSetup.SalesPersonEmail((int)ClientId);

            DesktopShared.EntityClasses.ClientContactEntity objClientsContact = SaveClientContactInformation();
            if (objClientsContact.PclientContact > 0)
            {
                DesktopShared.User.Portal.Password.ResetByEmail(Email, UserId, DesktopShared.User.UserID, mailcc);
                DisplayMessage(String.Format("Client Contact Information has been updated and reset email password has been sent - {0}.", DateTime.Now), Bootstrap.Alert.AlertType.Success);
                ClientsContactId = objClientsContact.PclientContact;
                LoadValues(objClientsContact);
            }
            DesktopShared.Client.Contact.AddButtonHistory("Reset Password", Email, (int)ClientsContactId, DesktopShared.User.UserID, DesktopShared.User.UserFullName, null, ref err);
        }
        rgButtonSendHistory.Rebind();
    }

    /// <summary>
    /// invite to portal click
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnPortalInvite_Click(object sender, EventArgs e)
    {
        string err = "";

        if (Page.IsValid)
        {
            string mailcc = System.Web.Configuration.WebConfigurationManager.AppSettings["ClientSetupCCBBBSalesPerson"];
            mailcc += "," + DesktopShared.Client.ClientSetup.SalesPersonEmail((int)ClientId);

            DesktopShared.EntityClasses.ClientContactEntity objClientsContact = SaveClientContactInformation();
            if ((objClientsContact != null) && (objClientsContact.PclientContact > 0))
            {
                DesktopShared.User.Portal.Password.ResetByEmail(Email, UserId, DesktopShared.User.UserID, mailcc, true);
                DisplayMessage(String.Format("Client Contact Information has been updated and invitation email sent - {0}.", DateTime.Now), Bootstrap.Alert.AlertType.Success);
                ClientsContactId = objClientsContact.PclientContact;
                LoadValues(objClientsContact);
            }
            DesktopShared.Client.Contact.AddButtonHistory("Invite to Portal", Email, (int)ClientsContactId, DesktopShared.User.UserID, DesktopShared.User.UserFullName,null, ref err);
        }

        rgButtonSendHistory.Rebind();
    }

    /// <summary>
    /// show portal billing link on click
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnShowPortalBillingLink_Click(object sender, EventArgs e)
    {
        string err = "";
        if (Page.IsValid)
        {
            DesktopShared.EntityClasses.ClientContactEntity objClientsContact = SaveClientContactInformation();
            if ((objClientsContact != null) && (objClientsContact.PclientContact > 0))
            {
                bool _isNewPortalUser = false;
                string _token = "";
                var objUser = DesktopShared.User.GetForClientContact(objClientsContact.PclientContact);
                if ((objUser != null) && (objUser.Fields.State == EntityState.Fetched))
                {
                    if (String.IsNullOrWhiteSpace(objUser.SaltedPassword))
                    {
                        objUser.PortalBillingEmailSent = true;
                        objUser.Save();
                        DesktopShared.User.Portal.Password.ResetByEmail(Email, UserId, DesktopShared.User.UserID,"", false, false, false);
                        _isNewPortalUser = true;
                        objUser.Refetch();
                        _token = objUser.ResetPasswordToken.Trim();
                    }
                }
                
                ClientsContactId = objClientsContact.PclientContact;
                LoadValues(objClientsContact);

                string _fqdn = BitByBit.Configuration.GetConfigString("ClientPortalFqdn", "portal.bitxbit.com");
                string _resetLink = String.Format("{0}/ResetPassword.aspx?Token={1}", _fqdn, _token);
                string _billingLink = String.Format("{0}/Billing/Detail.aspx", _fqdn);

                phPortalBillingLink.Visible = true;
                litPortalBillingLink.Text = String.Format("http://{0}", _isNewPortalUser ? _resetLink : _billingLink);
                btnPortalBilling.Focus();
            }

            DesktopShared.Client.Contact.AddButtonHistory("Show portal billing link", Email, (int)ClientsContactId, DesktopShared.User.UserID, DesktopShared.User.UserFullName,null, ref err);
        }
        rgButtonSendHistory.Rebind();
    }

    /// <summary>
    /// send billing portal email cliek click
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnPortalBilling_Click(object sender, EventArgs e)
    {
        string err = "";

        if (Page.IsValid)
        {
            DesktopShared.EntityClasses.ClientContactEntity objClientsContact = SaveClientContactInformation();
            if ((objClientsContact != null) && (objClientsContact.PclientContact > 0))
            {
                bool _isNewPortalUser = false;
                var objUser = DesktopShared.User.GetForClientContact(objClientsContact.PclientContact);
                if ((objUser != null) && (objUser.Fields.State == EntityState.Fetched))
                {
                    if (String.IsNullOrWhiteSpace(objUser.SaltedPassword))
                    {
                        objUser.PortalBillingEmailSent = true;
                        objUser.Save();
                        DesktopShared.User.Portal.Password.ResetByEmail(Email, UserId, DesktopShared.User.UserID,"", false, false, false);
                        _isNewPortalUser = true;
                        objUser.Refetch();
                    }
                }
                DesktopShared.Email.Portal.User.SendPortalBilling(objUser, _isNewPortalUser);
                DisplayMessage(String.Format("Client Contact Information has been updated and portal billing email has been sent - {0}.", DateTime.Now), Bootstrap.Alert.AlertType.Success);
                ClientsContactId = objClientsContact.PclientContact;
                LoadValues(objClientsContact);
            }

            DesktopShared.Client.Contact.AddButtonHistory("Send portal billing email", Email, (int)ClientsContactId, DesktopShared.User.UserID, DesktopShared.User.UserFullName,null, ref err);
        }
        rgButtonSendHistory.Rebind();
    }

    /// <summary>
    /// Add contact to the next "Bit By Bit to QuoteWerks" sync process.
    /// 
    /// </summary>
    /// <param name="sender"></param>btnResetPassword
    /// <param name="e"></param>
    protected void btnSetQWSync_Click(object sender, EventArgs e)
    {
        DesktopShared.EntityClasses.ClientContactEntity objClientsContact = ClientsContactId.Value > 0 ? new DesktopShared.EntityClasses.ClientContactEntity(ClientsContactId.Value) : new DesktopShared.EntityClasses.ClientContactEntity();

        try
        {            
            if (ClientsContactId.Value > 0)
            {
                objClientsContact.LastUpdatedby = DesktopShared.User.UserName; ;
                objClientsContact.Save();  // This will fire the trigger which will set the datetime file "Lastupdated".

                DisplayMessage("Contact has been added to the next QuoteWerks Sync.", Bootstrap.Alert.AlertType.Success);
            }

            objClientsContact = null;

        }
        catch
        {

            DisplayMessage(String.Format("Unable to add contact to the next QuoteWerks Sync."), Bootstrap.Alert.AlertType.Danger, false);
        }
    }

    protected void rgEmailCategory_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindGridEmailCategory();
    }

    protected void rgEmailCategory_ItemCommand(object sender, GridCommandEventArgs e)
    {
        int _auditUserId = DesktopShared.User.UserID;
        string command = e.CommandArgument.ToString().Trim();

        switch (command)
        {
            case "DeleteRecordEmailCategory":
                if (true)
                {
                    GridDataItem item = (GridDataItem)e.Item;

                    lblModalDeleteTitle.Text = "Delete Email Category";
                    lblModalDeleteWording.Text = "Are you sure want to delete " + item["CategoryName"].Text.Trim() + " ? ";
                    hfCategoryname.Value = item["CategoryName"].Text.Trim();
                    hfDelete.Value = item["id"].Text;
                    hfDeleteFor.Value = "Email";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "myModalDelete", "$('#myModalDelete').modal('show');", true);

                }
                break;
        }
    }

    protected void btnAddClientEmailCategory_Click(object sender, EventArgs e)
    {
        int _clientcontactid = 0;
        int _emailcategory = 0;
        int _auditUserId = DesktopShared.User.UserID;
        int.TryParse(BitByBit.Web.Request.GetString("ClientContactID").Trim(), out _clientcontactid);
        if (ddlClientEmailCategory.SelectedValue != string.Empty)
            _emailcategory =Convert.ToInt32(ddlClientEmailCategory.SelectedValue);
        else
        {
            DisplayMessage("Please Choose Email category first", Bootstrap.Alert.AlertType.Danger);
            return;
        }

        if (int.TryParse(BitByBit.Web.Request.GetString("ClientContactID").Trim(), out _clientcontactid))
        {
            if (DesktopShared.ClientEmailCategory.checkDuplicateEmailCategoryForClientContact(_clientcontactid, _emailcategory) == 0)
            {
                if (DesktopShared.ClientEmailCategory.AddEmailCategoriesClientContact(_clientcontactid, _emailcategory, _auditUserId) == true)
                {
                    DisplayMessage(String.Format("Email Category for client {0} - {1}", "Added", DateTime.Now), Bootstrap.Alert.AlertType.Success);
                    BindGridEmailCategory();
                    rgEmailCategory.DataBind();
                    populatedropdownlist();
                }
                else
                    DisplayMessage("Something went wrong", Bootstrap.Alert.AlertType.Danger);
            }
            else
                DisplayMessage("Email category already existed", Bootstrap.Alert.AlertType.Warning);
        }
    }

    protected void Decision_Command(object sender, CommandEventArgs e)
    {
        int _auditUserId = DesktopShared.User.UserID;
        string str = e.CommandArgument.ToString().Trim();

        switch (str)
        {
            case "Delete":
                switch (hfDeleteFor.Value)
                {
                    case "Email":
                        if (DesktopShared.ClientEmailCategory.DeleteEmailCategory(Convert.ToInt32(hfDelete.Value)))
                        {
                            DisplayMessage(String.Format("Email Category has been {0} - {1}", "Deleted", DateTime.Now), Bootstrap.Alert.AlertType.Success);
                            BindGridEmailCategory();
                            rgEmailCategory.DataBind();
                            populatedropdownlist();

                            if (hfCategoryname.Value.Trim() == System.Web.Configuration.WebConfigurationManager.AppSettings["ClientEmailCategoryBilling"].Trim())
                            {
                                DeleteClientPortalRole((int)GetIDClientPortalRole());

                                DesktopShared.EntityClasses.ClientContactEntity objClientContact = null;
                                if (GetClientsContactId(ref objClientContact))
                                    LoadValues(objClientContact);
                            }

                        }
                        else
                            DisplayMessage("Something went wrong", Bootstrap.Alert.AlertType.Danger);
                        break;
                }
                break;
        }
    }
    #region custom validator

    /// <summary>
    /// email validator
    /// </summary>
    /// <param name="source"></param>
    /// <param name="args"></param>
    protected void cvEmail_ServerValidate(object source, ServerValidateEventArgs args)
    {
        args.IsValid = true;
        string _email = txtEmail.Text.Trim();

        if (!DesktopShared.Utility.Email.CheckCustomEmail(_email))
        {
            cvEmail.ErrorMessage = "Invalid Email Address";
            args.IsValid = false;
            ScrollToTop();
            return;
        }

        //if (!DesktopShared.Utility.IsValidEmailAddress(_email))
        //{
        //    cvEmail.ErrorMessage = "Invalid Email Address";
        //    args.IsValid = false;
        //    ScrollToTop();
        //    return;
        //}

        if (txtEmail2.Text != string.Empty)
        {
            if (!DesktopShared.Utility.Email.CheckCustomEmail(txtEmail2.Text.Trim()))
            {
                cvEmail.ErrorMessage = "Invalid Email 2 Address";
                args.IsValid = false;
                ScrollToTop();
                return;
            }
        }
        if (txtEmail3.Text != string.Empty)
        {
            if (!DesktopShared.Utility.Email.CheckCustomEmail(txtEmail3.Text.Trim()))
            {
                cvEmail.ErrorMessage = "Invalid Email 3 Address";
                args.IsValid = false;
                ScrollToTop();
                return;
            }
        }
        if (txtEmail4.Text != string.Empty)
        {
            if (!DesktopShared.Utility.Email.CheckCustomEmail(txtEmail4.Text.Trim()))
            {
                cvEmail.ErrorMessage = "Invalid Email 4 Address";
                args.IsValid = false;
                ScrollToTop();
                return;
            }
        }
    }
    protected void cvPhone_ServerValidate(object source, ServerValidateEventArgs args)
    {
        if (txtHomePhone.Text == string.Empty && txtCellPhone.Text == string.Empty && txtPhone.Text == string.Empty)
        {
            cvPhone.ErrorMessage = "Please fill in at least one phone number";
            args.IsValid = false;
            ScrollToTop();
            return;
        }
    }
    #endregion

    #endregion

    #region private properties

    /// <summary>
    /// get/set client contact id
    /// </summary>
    private int? ClientsContactId
    {
        get
        {
            object obj = this.ViewState["ClientsContactId"];
            return (obj == null) ? (int?)null : (int)obj;
        }
        set { this.ViewState["ClientsContactId"] = value; }
    }

    /// <summary>
    /// get/set client id
    /// </summary>
    private int? ClientId
    {
        get
        {
            object obj = this.ViewState["ClientID"];
            return (obj == null) ? (int?)null : (int)obj;
        }
        set { this.ViewState["ClientID"] = value; }
    }

    /// <summary>
    /// get/set user id
    /// </summary>
    private int? UserId
    {
        get
        {
            object obj = this.ViewState["uid_cc"];
            return (obj == null) ? (int?)null : (int)obj;
        }
        set { this.ViewState["uid_cc"] = value; }
    }

    /// <summary>
    /// get/set email
    /// </summary>
    private string Email
    {
        get
        {
            object obj = this.ViewState["em_cc"];
            return (obj == null) ? "" : (string)obj;
        }
        set { this.ViewState["em_cc"] = value; }
    }

    #endregion

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

    protected void lbSendInvite_Click(object sender, EventArgs e)
    {
        bool isNewUser = false;
        bool sendEmail = true;
        bool defaultExpiration = true;
        string _resetLink = string.Empty;
        string err = "";

        if (Page.IsValid)
        {
            DesktopShared.EntityClasses.ClientContactEntity objClientsContact = SaveClientContactInformation();
            if ((objClientsContact != null) && (objClientsContact.PclientContact > 0))
            {
                string mailfrom = System.Web.Configuration.WebConfigurationManager.AppSettings["ClientSetupFrom"];
                string mailto = txtEmail.Text.Trim();
                string mailcc= System.Web.Configuration.WebConfigurationManager.AppSettings["ClientSetupCCBBBSalesPerson"];
                string mailbcc = System.Web.Configuration.WebConfigurationManager.AppSettings["ClientSetupBcc"];
                string subject = "Bit By Bit Client Setup Process";
                string _fqdn = System.Web.Configuration.WebConfigurationManager.AppSettings["PortalFqdn"];
                string _clientsetup = "/Client/ClientSetup.aspx";
                mailcc += "," + DesktopShared.Client.ClientSetup.SalesPersonEmail((int)ClientId);

                var _users = DesktopShared.User.GetByEmail(txtEmail.Text.Trim());
                if (_users.Count == 0)
                    return;

                DateTime _expirationDate = isNewUser ? DateTime.Now.AddDays(2) : DateTime.Now.AddMinutes(20);
                if (!defaultExpiration)
                    _expirationDate = DateTime.Now.AddYears(1);
                string _guid = Guid.NewGuid().ToString().Trim();

                foreach (var objUser in _users)
                {
                    if (objUser.ClientPortalActive && (objUser.Del != "Y"))
                    {
                        objUser.ResetPassword = true;
                        objUser.ResetPasswordExpires = _expirationDate;
                        objUser.ResetPasswordToken = _guid;
                        objUser.ResetPasswordNewUser = isNewUser;
                        objUser.Save();
                        objUser.Refetch();

                        //clear any failed audits as counting for account lock
                       DesktopShared.User.Portal.Security.FailedLogin.ClearLockedCount(objUser.Pusers);

                        if(!String.IsNullOrEmpty(objUser.Salt) || !String.IsNullOrEmpty(objUser.SaltedPassword))
                            _resetLink = String.Format("{0}/{1}", _fqdn, _clientsetup);
                        else
                            _resetLink = String.Format("{0}/ResetPassword.aspx?Token={1}&ReturnUrl={2}", _fqdn, _guid, _clientsetup);
                    }
                }


                //int _clientcontactid = 0;
                //int.TryParse(BitByBit.Web.Request.GetString("ClientContactID").Trim(), out _clientcontactid);
                //if (_clientcontactid == 0)
                //    _resetLink = String.Format("{0}/ResetPassword.aspx?Token={1}&ReturnUrl={2}", _fqdn, _guid, _clientsetup);
                //else
                //    _resetLink = String.Format("{0}/{1}", _fqdn, _clientsetup);

                System.Text.StringBuilder _sb = new System.Text.StringBuilder();
                _sb.Append(DesktopShared.Email.GetHtmlBegin());
                _sb.Append("<p>");
                _sb.Append(String.Format("Dear {0} {1},<br />", txtFirstName.Text.Trim(), txtLastName.Text.Trim()));
                _sb.Append(GetEmailMessage());
                //_sb.Append(String.Format("<br />Please create a portal account and complete the client setup screen."));
                _sb.Append(String.Format("<br /><a href=\"http://{0}\">Link to Client Setup Screen </a>", _resetLink));
                _sb.Append(DesktopShared.Email.GetFooter());

                if (DesktopShared.Email.SendEmail2(mailfrom, subject, mailto, mailbcc, _sb.ToString(),mailcc) == false)
                {
                    DisplayMessage("Error Send Email", Bootstrap.Alert.AlertType.Danger, false);
                    return;
                }
                else
                    DisplayMessage("Email Sending", Bootstrap.Alert.AlertType.Info, false);

                ClientsContactId = objClientsContact.PclientContact;
            }

            DesktopShared.Client.Contact.AddButtonHistory("Send client setup screen", txtEmail.Text.Trim(), (int)ClientsContactId, DesktopShared.User.UserID, DesktopShared.User.UserFullName, null, ref err);
        }
        rgButtonSendHistory.Rebind();
    }

    private string GetEmailMessage()
    {
        DesktopShared.CollectionClasses.ClientSetupInvitationEmailCollection _clientsetupemail = new DesktopShared.CollectionClasses.ClientSetupInvitationEmailCollection();

        SD.LLBLGen.Pro.ORMSupportClasses.IPredicateExpression _ClientFilter = new SD.LLBLGen.Pro.ORMSupportClasses.PredicateExpression();

        _clientsetupemail.GetMulti(_ClientFilter, 0);

        return _clientsetupemail[0].EmailMessage.Replace("\r\n", "<br/>").Trim();
    }

    private void CountryCheck()
    {
        if (ddlCountry.CountryId > 1)
        {
            ddlState.Visible = false;
            txtState.Visible = true;
            lblZip.Text = "Zip/Postal Code";
            lblState.Text = "State/Provence/Region";
        }
        else
        {
            ddlState.Visible = true;
            txtState.Visible = false;
            lblZip.Text = "Zip";
            lblState.Text = "State";
        }
    }
    private void CheckPhone(TextBox txt, string desc)
    {
        if (txt.Text.Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "").Length > 10)
        {
            DisplayMessage(String.Format("{0} For Country US should be 10", desc), Bootstrap.Alert.AlertType.Danger);
            return;
        }
    }

    #region populate dropdowlist
    private void populatedropdownlist()
    {
        ddlClientEmailCategory.DataSource = DesktopShared.StoredProcedureCallerClasses.RetrievalProcedures.ProcGetddlEmailCategory("CC", ClientsContactId);
        ddlClientEmailCategory.DataTextField = "CategoryName";
        ddlClientEmailCategory.DataValueField = "ID";
        ddlClientEmailCategory.DataBind();
        ddlClientEmailCategory.Items.Insert(0, string.Empty);
    }
    #endregion


    protected void rgButtonSendHistory_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        bindgridsendbuttonhist();
    }

    private void bindgridsendbuttonhist()
    {
        rgButtonSendHistory.DataSource = DesktopShared.Client.Contact.Get((int)ClientsContactId);
    }
}
