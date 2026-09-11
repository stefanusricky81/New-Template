using SD.LLBLGen.Pro.ORMSupportClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class Ticket_Add2 : BasePage
{
    private string _defaultTextAddTag = "";

    /// <summary>
    /// page load
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        #region grid event handlers

        //DropDownList _ddl = ddlClient.GetDropDownList();
        //_ddl.AutoPostBack = true;
        ////_ddl.SelectedIndexChanged += ddlClient_SelectedIndexChanged;

        Telerik.Web.UI.RadComboBox rcbClient = (Telerik.Web.UI.RadComboBox)ddlSmartClient.FindControl("rcbClient");
        if (rcbClient != null)
        {
            rcbClient.AutoPostBack = true;
            rcbClient.SelectedIndexChanged += new RadComboBoxSelectedIndexChangedEventHandler(rcbClient_SelectedIndexChanged);
        }

        DropDownList _ddl = ddlClientContact.GetDropDownList();
        _ddl.AutoPostBack = true;
        _ddl.SelectedIndexChanged += ddlClientContact_SelectedIndexChanged;

        _ddl = ddlTicketCheckListMaster.GetDropDownList();
        _ddl.AutoPostBack = true;
        _ddl.SelectedIndexChanged += ddlTicketCheckListMaster_SelectedIndexChanged;

        ddlEmployeeAssignedTo.EmployeeTypeToDisplay = UserControl_DropDownList_Employee.EmployeeType.AssignTo;
        _ddl = ddlEmployeeAssignedTo.GetDropDownList();
        _ddl.AutoPostBack = true;
        _ddl.SelectedIndexChanged += ddlEmployeeAssignedTo_SelectedIndexChanged;

        _ddl = ddlDefaultProject.GetDropDownList();
        _ddl.AutoPostBack = true;
        _ddl.SelectedIndexChanged += ddlDefaultProject_SelectedIndexChanged;

        gridTicketContactClient.evItemCommand += Grid_evItemCommand;
        gridTicketContactEmployee.evItemCommand += Grid_evItemCommand;
        gridTicketContactMasterClient.evItemCommand += Grid_evItemCommand;


        #endregion

        ConfigurePhoneValidations();
        ConfigureForDevice();
        //rauMain.AllowedFileExtensions = DesktopShared.Utility.ValidFileUploadExtensions().ToArray();
        if (!this.IsPostBack)
        {
            ddlTicketCategory.PopulateDropDownList(ClientId);
            SetUpPage();
        }
    }

    private void ddlDefaultProject_SelectedIndexChanged(object sender, EventArgs e)
    {
        try 
        {
            ddlTimesheetProject.SelectedClientId = ddlSmartClient.SelectedClientId;
            ddlTimesheetProject.Populate();

            chkAddTimesheet.Checked = true;
            ddlTimesheetProject.SelectedProjtaskId = ddlDefaultProject.SelectedProjtaskId;
        }
        catch (Exception ex)
        {
            DesktopShared.Bootstrap.Alert.DisplayMessage(litMessage, ex.ToString().Trim(), DesktopShared.Bootstrap.Alert.AlertType.Danger, false);
        }
    }

    protected void rcbClient_SelectedIndexChanged(object o, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
    {
        litMessage.Text = "";
        ClearContactEditForm();
        DesktopShared.Ticket.Notification.DeleteByTempGuid(TemporaryTicketGuid); //delete client contacts
        DesktopShared.Ticket.Notification.DeleteByTempGuid(TemporaryTicketGuid, false, false, true); //delete master client contacts
        phAddContact.Visible = false;
        phEditContact.Visible = false;
        litSales.Text = "";
        ClientId = ddlSmartClient.SelectedClientId;

        if (ddlSmartClient.SelectedClientId == -1)
        {
            ddlClientContact.Visible = false;
            phMasterClientsNotification.Visible = false;
            phAddContactButton.Visible = false;
            MasterClientId = null;
            litTicketCategoryRequired.Visible = false;
            ddlTicketCategory.IsRequired = false;
            return;
        }
        
        //ddlTicketCategory.Client = ddlClient.ClientId;
        ddlTicketCategory.PopulateDropDownList(ClientId);

        litSales.Text = DesktopShared.Client.GetSalesForDisplay(ClientId.Value, "<br />", true);
        lblLeadTech.Text = DesktopShared.Client.GetLeadTech(ClientId.Value);
        lblTam.Text = DesktopShared.Client.GetTAMEmployee(ClientId.Value);
        lblCSM.Text = DesktopShared.Client.GetCSMEmployee(ClientId.Value);

        ddlClientContact.Visible = true;
        ddlClientContact.ClientId = ddlSmartClient.SelectedClientId;
        ddlClientContact.PopulateDropDownList();

        phAddContact.Visible = true;

        //ddlClientContactRecipient.ClientId = ClientId;
        //ddlClientContactRecipient.IsSilent = "on";
        //ddlClientContactRecipient.PopulateDropDownList();

        lbClientContact.ClientId = ClientId;
        lbClientContact.IsSilent = "on,not";
        lbClientContact.Populate();

        lbEmployeeRecipient.ClientId = ClientId;
        lbEmployeeRecipient.IsSilent = "on,not";
        lbEmployeeRecipient.Populate();

        phAddContactButton.Visible = true;

        ddlEmployeeAssignedTo.HelpDeskClientId = ClientId.Value;
        ddlEmployeeAssignedTo.EmployeeId = ddlEmployeeAssignedTo.EmployeeId;
        ddlEmployeeAssignedTo.Populate();

        ddlDefaultProject.SelectedClientId= ClientId.Value;
        ddlDefaultProject.Populate();

        ddlTimesheetProject.SelectedClientId = ClientId.Value;
        ddlDefaultProject.Populate();

        var objClient = new DesktopShared.EntityClasses.ClientEntity(ClientId.Value);

        #region dropdown ticket type
        if (objClient.TicketTypeId != null)
            ddlTicketType.TicketTypeId = objClient.TicketTypeId;
        else
            ddlTicketType.TicketTypeId = 3;
        #endregion

        //category required
        bool _categoryRequired = false;
        if (objClient.Fields.State == EntityState.Fetched)
            _categoryRequired = objClient.TicketCategoryRequired;
        ddlTicketCategory.IsRequired = _categoryRequired;
        litTicketCategoryRequired.Visible = _categoryRequired;

        phLocation.Visible = (objClient.Fields.State == EntityState.Fetched) && objClient.UseTicketLocation;

        MasterClientId = objClient.MasterClientId;
        if (!MasterClientId.HasValue && objClient.IsMasterClient)
            MasterClientId = objClient.Pclient;

        phMasterClientsNotification.Visible = MasterClientId.HasValue;
        if (phMasterClientsNotification.Visible)
        {
            ddlMasterClientContactRecipient.ClientId = MasterClientId;
            ddlMasterClientContactRecipient.PopulateDropDownList();
        }

        if (phLocation.Visible)
        {
            if (!DesktopShared.Client.Location.HasOneActive(ClientId.Value))
            {
                System.Text.StringBuilder _sb = new StringBuilder();
                _sb.Append(String.Format("Selected client ({0}) does not have any active locations. ", objClient.Company.Trim()));
                _sb.Append(String.Format("<a href=\"/Client/ClientDetail2.aspx?ClientId={0}&TabIndex=8\" class=\"alert-link\">Click here</a> to manage locations. ", ClientId.Value));
                _sb.Append("<a href=\"Add2.aspx\" class=\"alert-link\">Click here</a> to enter a ticket for an alternate client.");
                pnlContainer.Visible = false;
                DesktopShared.Bootstrap.Alert.DisplayMessage(litMessage, _sb.ToString(), DesktopShared.Bootstrap.Alert.AlertType.Danger, false);
                return;
            }
            ddlClientLocation.DisplayDefaultValue = true;
            ddlClientLocation.ClientIdForLocation = ClientId.Value;
            ddlClientLocation.PopulateDropDownList();
        }

        CheckClientContactList();
        RebindGrids();

        //on hold
        if (DesktopShared.Client.Status.IsOnHold(objClient))
            DesktopShared.Bootstrap.Alert.DisplayMessage(litMessage, "Client Status = On Hold. Please Refer Client To Accounting.", DesktopShared.Bootstrap.Alert.AlertType.Danger, false, "ALERT");
    }
    #region private methods

    /// <summary>
    /// set up for initial page load
    /// </summary>
    private void SetUpPage()
    {
        //ddlClient.Focus();
        ddlSmartClient.Focus();// scrClient.Focus();

        #region tab index

        short _tabIndex = 0;
        //ddlClient.TabIndex = ++_tabIndex;
        chkInternal.TabIndex = ++_tabIndex;
        ddlClientContact.TabIndex = ++_tabIndex;
        btnAddContact.TabIndex = ++_tabIndex;
        ddlTicketDisposition.TabIndex = ++_tabIndex;
        ddlEmployeeAssignedTo.TabIndex = ++_tabIndex;
        ddlClientLocation.TabIndex = ++_tabIndex;
        ddlTicketCheckListMaster.TabIndex = ++_tabIndex;

        //ddlClientContactRecipient.TabIndex = ++_tabIndex;
        lbClientContact.TabIndex= ++_tabIndex;
        btnAddRecipientClient.TabIndex = ++_tabIndex;
        //ddlEmployeeRecipient.TabIndex = ++_tabIndex;
        lbEmployeeRecipient.TabIndex = ++_tabIndex;

        btnAddRecipientBbb.TabIndex = ++_tabIndex;
        ddlMasterClientContactRecipient.TabIndex = ++_tabIndex;

        btnAddRecipientMasterClient.TabIndex = ++_tabIndex;
        txtSummary.TabIndex = ++_tabIndex;
        txtDescription.TabIndex = ++_tabIndex;
        txtInternalNotes.TabIndex = ++_tabIndex;
        ddlTicketCategory.TabIndex = ++_tabIndex;
        ddlTicketPriority.TabIndex = ++_tabIndex;
        chkAddTimesheet.TabIndex = ++_tabIndex;
        ddlTimesheetProject.TabIndex = ++_tabIndex;
        ddlTimesheetStartTime.HourTabIndex = ++_tabIndex;
        ddlTimesheetStartTime.MinuteTabIndex = ++_tabIndex;
        ddlTimesheetActivity.TabIndex = ++_tabIndex;
        ucTimesheetDate.TabIndex = ++_tabIndex;
        ddlTimesheetTime.HourTabIndex = ++_tabIndex;
        ddlTimesheetTime.MinuteTabIndex = ++_tabIndex;
        btnSubmit.TabIndex = ++_tabIndex;

        ucScheduledDate.TabIndex = ++_tabIndex;
        ddlScheduledTime.HourTabIndex = ++_tabIndex;
        ddlScheduledTime.MinuteTabIndex = ++_tabIndex;
        ddlDurationTime.HourTabIndex = ++_tabIndex;
        ddlDurationTime.MinuteTabIndex = ++_tabIndex;

        lbTicketTag.TabIndex = ++_tabIndex;

        //rauMain.TabIndex = ++_tabIndex;
        fuOne.TabIndex = ++_tabIndex;
        litAllowedFileExtensions.Text = string.Join(",", DesktopShared.Utility.ValidFileUploadExtensions().ToArray());

        btnSubmit2.TabIndex = ++_tabIndex;

        #endregion

        //guid for recipients / tags
        TemporaryTicketGuid = Guid.NewGuid().ToString();

        //default assigned to logged in user
        
        ddlEmployeeAssignedTo.EmployeeId = DesktopShared.User.UserID;
        AddAssignedToRecipients();

        //default disposition
        ddlTicketDisposition.TicketDispositionId = BitByBit.Configuration.GetConfigInt("TicketDisposition_BBBNewId", 99);

        //timesheet
        ddlTimesheetActivity.TaskActionId = DesktopShared.User.DefaultActivityId;
        ucTimesheetDate.SelectedDate = DateTime.Now;
        string _startTime = "0800";
        string _timeSpent = "0000";
        DesktopShared.Timesheet.GetStartTimeAndTimeSpent(ref _startTime, ref _timeSpent);
        ddlTimesheetTime.Hour = Convert.ToInt32(_timeSpent.Substring(0, 2));
        ddlTimesheetTime.Minute = Convert.ToInt32(_timeSpent.Substring(2, 2));
        TimesheetStartTime = _startTime;
        var objEmployee = DesktopShared.Employee.GetEmployeeEntity(DesktopShared.User.EmployeeID);
        if ((objEmployee != null) && (objEmployee.Fields.State == EntityState.Fetched) && objEmployee.TicketTimesheetUseTime)
        {
            TimesheetUseStartTime = true;
            divTimesheetStartTime.Attributes.CssStyle.Add("display", "");
            ddlTimesheetStartTime.Hour = Convert.ToInt32(_startTime.Substring(0, 2));
            ddlTimesheetStartTime.Minute = Convert.ToInt32(_startTime.Substring(2, 2));

            ddlTicketType.TicketTypeId = Convert.ToInt32(objEmployee.TicketTypeId);
        }

        //ticket tags
        lbTicketTag.Populate(null);
    }

    /// <summary>
    /// confirue phone validation
    /// </summary>
    private void ConfigurePhoneValidations()
    {
        revAddContactPhone.ValidationExpression = DesktopShared.Utility.ValidPhoneNumberRegEx;
        revAddContactPhone.ErrorMessage = DesktopShared.Utility.InvalidPhoneNumberMessage("Phone");
        revAddContactCellPhone.ValidationExpression = DesktopShared.Utility.ValidPhoneNumberRegEx;
        revAddContactCellPhone.ErrorMessage = DesktopShared.Utility.InvalidPhoneNumberMessage("Cell Phone");
        revAddContactHomePhone.ValidationExpression = DesktopShared.Utility.ValidPhoneNumberRegEx;
        revAddContactHomePhone.ErrorMessage = DesktopShared.Utility.InvalidPhoneNumberMessage("Home Phone");

        revEditContactPhone.ValidationExpression = DesktopShared.Utility.ValidPhoneNumberRegEx;
        revEditContactPhone.ErrorMessage = DesktopShared.Utility.InvalidPhoneNumberMessage("Phone");
        revEditContactCellPhone.ValidationExpression = DesktopShared.Utility.ValidPhoneNumberRegEx;
        revEditContactCellPhone.ErrorMessage = DesktopShared.Utility.InvalidPhoneNumberMessage("Cell Phone");
        revEditContactHomePhone.ValidationExpression = DesktopShared.Utility.ValidPhoneNumberRegEx;
        revEditContactHomePhone.ErrorMessage = DesktopShared.Utility.InvalidPhoneNumberMessage("Home Phone");
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

        //ddlClient.DisplayChosenScript = _displayChosenScript;
        //ddlClient.CssClass = _cssClass;

        ddlClientContact.DisplayChosenScript = _displayChosenScript;
        ddlClientContact.CssClass = _cssClass;
        ddlEmployeeAssignedTo.DisplayChosenScript = _displayChosenScript;
        ddlEmployeeAssignedTo.CssClass = _cssClass;
        ddlEmployeeAssignedTo.SetSize = false;
        ddlTicketDisposition.DisplayChosenScript = _displayChosenScript;
        ddlTicketDisposition.CssClass = _cssClass;
        ddlTicketCheckListMaster.DisplayChosenScript = _displayChosenScript;
        ddlTicketCheckListMaster.CssClass = _cssClass;
        ddlClientLocation.DisplayChosenScript = _displayChosenScript;
        ddlClientLocation.CssClass = _cssClass;
        ddlTicketCategory.DisplayChosenScript = _displayChosenScript;
        ddlTicketCategory.CssClass = _cssClass;
        ddlTicketPriority.DisplayChosenScript = _displayChosenScript;
        ddlTicketPriority.CssClass = _cssClass;
        ddlTimesheetStartTime.DisplayChosenScript = _displayChosenScript;
        ddlTimesheetStartTime.CssClass = _cssClass;
        ddlTimesheetProject.DisplayChosenScript = _displayChosenScript;
        ddlTimesheetProject.CssClass = _cssClass;
        ddlTimesheetActivity.DisplayChosenScript = _displayChosenScript;
        ddlTimesheetActivity.CssClass = _cssClass;
        ddlTimesheetTime.DisplayChosenScript = _displayChosenScript;
        ddlTimesheetTime.CssClass = _cssClass;
        ddlScheduledTime.DisplayChosenScript = _displayChosenScript;
        ddlScheduledTime.CssClass = _cssClass;
        ddlDurationTime.DisplayChosenScript = _displayChosenScript;
        ddlDurationTime.CssClass = _cssClass;
        //ddlClientContactRecipient.DisplayChosenScript = _displayChosenScript;
        //ddlClientContactRecipient.CssClass = _cssClass;
        lbClientContact.DisplayChosenScript = _displayChosenScript;
        lbClientContact.CssClass = _cssClass;

        //ddlEmployeeRecipient.DisplayChosenScript = _displayChosenScript;
        //ddlEmployeeRecipient.CssClass = _cssClass;

        lbEmployeeRecipient.DisplayChosenScript= _displayChosenScript;
        lbEmployeeRecipient.CssClass = _cssClass;

        ddlMasterClientContactRecipient.DisplayChosenScript = _displayChosenScript;
        ddlMasterClientContactRecipient.CssClass = _cssClass;
    }

    /// <summary>
    /// check for auto cc / entered by assigned to notifications
    /// </summary>
    private void CheckClientContactList()
    {
        int _ticketNotificationId = -1;
        DesktopShared.Ticket.Notification.CheckClientContactList(ClientId, TemporaryTicketGuid, ddlClientContact.ClientContactId, ref _ticketNotificationId);
        TicketNotificationId = _ticketNotificationId > 0 ? _ticketNotificationId : (int?)null;
    }

    /// <summary>
    /// update contact drop down lists -> if already assiged or missing , remove from ddl
    /// </summary>
    private void UpdateContacts()
    {
        DropDownList _ddl = null;
        ListBox _lb = null;
        ListItem _li = null;

        //contacts
        List<DesktopShared.Ticket.Notification.Contact> _contacts = DesktopShared.Ticket.Notification.GetClientContacts(null, TemporaryTicketGuid);
        if (_contacts.Count > 0)
        {
            _lb = lbClientContact.GetListBox();
            if (_lb.Items.Count == 0)
            {
                lbClientContact.UseEmailAsDataValue = true;
                lbClientContact.Populate();
            }
            for (int i = 0; i < _lb.Items.Count; i++)
            {
                _li = _lb.Items[i];
                if ((_contacts.FirstOrDefault(x => x.Email.ToLower() == _li.Value.ToLower()) != null) || (_li.Value.Trim() == ""))
                {
                    if (i != 0)
                    {
                        _lb.Items.Remove(_li);
                        i--;
                    }
                }
            }
            //_ddl = ddlClientContactRecipient.GetDropDownList(); here 1
            //if (_ddl.Items.Count == 0)
            //{
            //    ddlClientContactRecipient.UseEmailAsDataValue = true;
            //    ddlClientContactRecipient.PopulateDropDownList();
            //}

                //for (int i = 0; i < _ddl.Items.Count; i++)
                //{
                //    _li = _ddl.Items[i];
                //    if ((_contacts.FirstOrDefault(x => x.Email.ToLower() == _li.Value.ToLower()) != null) || (_li.Value.Trim() == ""))
                //    {
                //        if (i != 0)
                //        {
                //            _ddl.Items.Remove(_li);
                //            i--;
                //        }
                //    }
                //}
        }

        //employees
        _contacts = DesktopShared.Ticket.Notification.GetEmployees(null, TemporaryTicketGuid);
        if (_contacts.Count > 0)
        {
            _lb = lbEmployeeRecipient.GetListBox();
            if (_lb.Items.Count == 0)
            {
                lbEmployeeRecipient.UseEmailAsDataValue = true;
                lbEmployeeRecipient.Populate();
            }
            for (int i = 0; i < _lb.Items.Count; i++)
            {
                _li = _lb.Items[i];
                if ((_contacts.FirstOrDefault(x => x.Email.ToLower() == _li.Value.ToLower()) != null) || (_li.Value.Trim() == ""))
                {
                    if (i != 0)
                    {
                        _lb.Items.Remove(_li);
                        i--;
                    }
                }
            }
            //_ddl = ddlEmployeeRecipient.GetDropDownList();
            //if (_ddl.Items.Count == 0)
            //{
            //    ddlEmployeeRecipient.UseEmailAsDataValue = true;
            //    ddlEmployeeRecipient.Populate();
            //}

            //for (int i = 0; i < _ddl.Items.Count; i++)
            //{
            //    _li = _ddl.Items[i];
            //    if ((_contacts.FirstOrDefault(x => x.Email.ToLower() == _li.Value.ToLower()) != null) || (_li.Value.Trim() == ""))
            //    {
            //        if (i != 0)
            //        {
            //            _ddl.Items.Remove(_li);
            //            i--;
            //        }
            //    }
            //}
        }

        //master client contacts
        if (phMasterClientsNotification.Visible)
        {
            _contacts = DesktopShared.Ticket.Notification.GetClientContacts(null, TemporaryTicketGuid, true);
            if (_contacts.Count > 0)
            {
                _ddl = ddlMasterClientContactRecipient.GetDropDownList();
                if (_ddl.Items.Count == 0)
                {
                    ddlMasterClientContactRecipient.UseEmailAsDataValue = true;
                    ddlMasterClientContactRecipient.PopulateDropDownList();
                }

                for (int i = 0; i < _ddl.Items.Count; i++)
                {
                    _li = _ddl.Items[i];
                    if ((_contacts.FirstOrDefault(x => x.Email.ToLower() == _li.Value.ToLower()) != null) || (_li.Value.Trim() == ""))
                    {
                        if (i != 0)
                        {
                            _ddl.Items.Remove(_li);
                            i--;
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// /// <summary>
    /// 
    /// </summary>
    /// <param name="history"></param>
    /// <param name="file"></param>
    /// </summary>
    /// <param name="history"></param>
    /// <param name="file"></param>
    /// <param name="tag"></param>
    private void RebindGrids()
    {
        //client contacts
        if (ClientId.HasValue)
        {
            gridTicketContactClient.TicketId = null;
            gridTicketContactClient.TemporaryGuid = TemporaryTicketGuid.Trim();
            gridTicketContactClient.ClientId = ClientId.Value;
            gridTicketContactClient.RebindGrid();
        }

        //employee contacts
        gridTicketContactEmployee.TicketId = null;
        gridTicketContactEmployee.TemporaryGuid = TemporaryTicketGuid.Trim();
        gridTicketContactEmployee.ClientId = null;
        gridTicketContactEmployee.RebindGrid();

        //master client contacts
        if (phMasterClientsNotification.Visible && MasterClientId.HasValue)
        {
            gridTicketContactMasterClient.TicketId = null;
            gridTicketContactMasterClient.TemporaryGuid = TemporaryTicketGuid.Trim();
            gridTicketContactMasterClient.MasterClientId = MasterClientId.Value;
            gridTicketContactMasterClient.RebindGrid();

        }
        //remove existing contacts from ddl 
        UpdateContacts();
    }

    /// <summary>
    /// event handler for item command for recipient / history / file / tag grids
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Grid_evItemCommand(object sender, EventArgs e)
    {
        string _senderId = ((Control)sender).ID.Trim().Trim();
        bool _clientContacts = _senderId == "gridTicketContactClient";
        bool _masterClientContacts = _senderId == "gridTicketContactMasterClient";
        bool _employeeContacts = _senderId == "gridTicketContactEmployee";

        if (_clientContacts)
            //ddlClientContactRecipient.PopulateDropDownList();
            lbClientContact.Populate();
        if (_employeeContacts)
            //ddlEmployeeRecipient.Populate();
            lbEmployeeRecipient.Populate();
        if (_masterClientContacts)
            ddlMasterClientContactRecipient.PopulateDropDownList();

        RebindGrids();
    }

    /// <summary>
    /// client contact on selected index changed
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ddlClient_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            litMessage.Text = "";
            ClearContactEditForm();
            DesktopShared.Ticket.Notification.DeleteByTempGuid(TemporaryTicketGuid); //delete client contacts
            DesktopShared.Ticket.Notification.DeleteByTempGuid(TemporaryTicketGuid, false, false, true); //delete master client contacts
            phAddContact.Visible = false;
            phEditContact.Visible = false;
            litSales.Text = "";
            ClientId = ddlSmartClient.SelectedClientId;

            if (ddlSmartClient.SelectedClientId == -1)
            {
                ddlClientContact.Visible = false;
                phMasterClientsNotification.Visible = false;
                phAddContactButton.Visible = false;
                MasterClientId = null;
                litTicketCategoryRequired.Visible = false;
                ddlTicketCategory.IsRequired = false;
                return;
            }
            //if (!scrClient.SelectedClientId.HasValue)
            //{
            //    ddlClientContact.Visible = false;
            //    phMasterClientsNotification.Visible = false;
            //    phAddContactButton.Visible = false;
            //    MasterClientId = null;
            //    litTicketCategoryRequired.Visible = false;
            //    ddlTicketCategory.IsRequired = false;
            //    return;
            //}
            //ddlTicketCategory.Client = ddlClient.ClientId;
            ddlTicketCategory.PopulateDropDownList(ClientId);

            litSales.Text = DesktopShared.Client.GetSalesForDisplay(ClientId.Value, "<br />", true);

            ddlClientContact.Visible = true;
            ddlClientContact.ClientId = ddlSmartClient.SelectedClientId;
            ddlClientContact.PopulateDropDownList();

            phAddContact.Visible = true;

            //ddlClientContactRecipient.ClientId = ClientId;
            //ddlClientContactRecipient.IsSilent = "on";
            //ddlClientContactRecipient.PopulateDropDownList();

            lbClientContact.ClientId = ClientId;
            lbClientContact.IsSilent = "on";
            lbClientContact.Populate();

            phAddContactButton.Visible = true;

            ddlTimesheetProject.SelectedClientId = ddlSmartClient.SelectedClientId;
            ddlTimesheetProject.Populate();

            ddlEmployeeAssignedTo.HelpDeskClientId = ClientId.Value;
            ddlEmployeeAssignedTo.EmployeeId = ddlEmployeeAssignedTo.EmployeeId;
            ddlEmployeeAssignedTo.Populate();

            var objClient = new DesktopShared.EntityClasses.ClientEntity(ClientId.Value);

            #region dropdown ticket type
            if (objClient.TicketTypeId != null)
                ddlTicketType.TicketTypeId = objClient.TicketTypeId;
            else
                ddlTicketType.TicketTypeId = 3;
            #endregion

            //category required
            bool _categoryRequired = false;
            if (objClient.Fields.State == EntityState.Fetched)
                _categoryRequired = objClient.TicketCategoryRequired;
            ddlTicketCategory.IsRequired = _categoryRequired;
            litTicketCategoryRequired.Visible = _categoryRequired;

            phLocation.Visible = (objClient.Fields.State == EntityState.Fetched) && objClient.UseTicketLocation;

            MasterClientId = objClient.MasterClientId;
            if (!MasterClientId.HasValue && objClient.IsMasterClient)
                MasterClientId = objClient.Pclient;

            phMasterClientsNotification.Visible = MasterClientId.HasValue;
            if (phMasterClientsNotification.Visible)
            {
                ddlMasterClientContactRecipient.ClientId = MasterClientId;
                ddlMasterClientContactRecipient.PopulateDropDownList();
            }

            if (phLocation.Visible)
            {
                if (!DesktopShared.Client.Location.HasOneActive(ClientId.Value))
                {
                    System.Text.StringBuilder _sb = new StringBuilder();
                    _sb.Append(String.Format("Selected client ({0}) does not have any active locations. ", objClient.Company.Trim()));
                    _sb.Append(String.Format("<a href=\"/Client/ClientDetail2.aspx?ClientId={0}&TabIndex=8\" class=\"alert-link\">Click here</a> to manage locations. ", ClientId.Value));
                    _sb.Append("<a href=\"Add2.aspx\" class=\"alert-link\">Click here</a> to enter a ticket for an alternate client.");
                    pnlContainer.Visible = false;
                    DesktopShared.Bootstrap.Alert.DisplayMessage(litMessage, _sb.ToString(), DesktopShared.Bootstrap.Alert.AlertType.Danger, false);
                    return;
                }
                ddlClientLocation.DisplayDefaultValue = true;
                ddlClientLocation.ClientIdForLocation = ClientId.Value;
                ddlClientLocation.PopulateDropDownList();
            }

            CheckClientContactList();
            RebindGrids();

            //on hold
            if (DesktopShared.Client.Status.IsOnHold(objClient))
                DesktopShared.Bootstrap.Alert.DisplayMessage(litMessage, "Client Status = On Hold. Please Refer Client To Accounting.", DesktopShared.Bootstrap.Alert.AlertType.Danger, false, "ALERT");
        }
        catch (Exception ex)
        { }
    }

    /// <summary>
    /// client contact on selected index changed
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ddlClientContact_SelectedIndexChanged(object sender, EventArgs e)
    {
        DesktopShared.Ticket.Notification.DeleteByTempGuid(TemporaryTicketGuid);
        if (!ddlClientContact.ClientContactId.HasValue)
        {
            phAddContact.Visible = true;
            phEditContact.Visible = false;
            ClearContactEditForm();
            litEditContactHeader.Text = "Add New Contact";
            return;
        }

        phAddContact.Visible = false;
        phEditContact.Visible = true;
        litEditContactHeader.Text = "Update Contact";

        var objClientContact = new DesktopShared.EntityClasses.ClientContactEntity(ddlClientContact.ClientContactId.Value);
        if (objClientContact.Fields.State == EntityState.Fetched)
        {
            if(objClientContact.Vip)
                ltVIP.Text = String.Format("{0}", DesktopShared.User.VipIcon);

            txtEditContactFirst.Text = objClientContact.First.Trim();
            txtEditContactLast.Text = objClientContact.Last.Trim();
            txtEditContactEmail.Text = objClientContact.Email.Trim();
            txtEditContactPhone.Text = objClientContact.Busphone.Trim();
            txtEditContactPhoneExtension.Text = objClientContact.Busext.Trim();
            txtEditContactCellPhone.Text = objClientContact.Cellphone.Trim();
            txtEditContactHomePhone.Text = objClientContact.Homephone.Trim();
            ClientContactId = objClientContact.PclientContact;
            ClientContactUserId = DesktopShared.User.GetIdForClientContact(objClientContact.PclientContact);

            if (phLocation.Visible && ClientContactUserId.HasValue)
            {
                int? _primaryClientLocationId = DesktopShared.User.Client.Location.GetPrimaryClientLocationId(ClientContactUserId.Value);
                if (_primaryClientLocationId.HasValue)
                {
                    DropDownList _ddl = ddlClientLocation.GetDropDownList();
                    ListItem _li = _ddl.Items.FindByValue(_primaryClientLocationId.Value.ToString());
                    if (_li != null)
                    {
                        _ddl.ClearSelection();
                        _li.Selected = true;
                    }
                }
            }
        }
        if (objClientContact.NotificationPreferences == "on")
            CheckClientContactList();
        RebindGrids();
    }

    /// <summary>
    /// ticket check list master on selected index changed
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ddlTicketCheckListMaster_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (!ddlTicketCheckListMaster.TicketCheckListMasterId.HasValue)
            return;

        DesktopShared.EntityClasses.TicketCheckListMasterEntity objTicketCheckListMaster = new DesktopShared.EntityClasses.TicketCheckListMasterEntity(ddlTicketCheckListMaster.TicketCheckListMasterId.Value);
        if (objTicketCheckListMaster.Fields.State == EntityState.Fetched)
            txtDescription.Text += objTicketCheckListMaster.CheckListCode.Trim();
        ddlTicketCheckListMaster.TicketCheckListMasterId = null;
    }

    /// <summary>
    /// assigned to on selected index changed
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ddlEmployeeAssignedTo_SelectedIndexChanged(object sender, EventArgs e)
    {
        AddAssignedToRecipients();
        if (ddlEmployeeAssignedTo.EmployeeId == DesktopShared.SiteHelper.User.Id.OpportunitiesQueue)
        {
            chkInternal.Checked = true;
            ConfigureForInternal();
            DesktopShared.Ticket.Tagged.AddTagValue(-1, DesktopShared.Ticket.Tagged.Opportunities, null, true, true, TemporaryTicketGuid);
            lbTicketTag.Populate(DesktopShared.Ticket.Tagged.GetSelectedTagTextForTicket(-1, DesktopShared.User.UserID, TemporaryTicketGuid));
        }
    }

    /// <summary>
    /// add employee assigned to list of recipiients
    /// </summary>
    private void AddAssignedToRecipients()
    {
        if (ddlEmployeeAssignedTo.EmployeeId < 1)
        {
            RebindGrids();
            return;
        }
        if (ddlEmployeeAssignedTo.EmployeeName.ToLower().Contains("queue"))
        {
            RebindGrids();
            return;
        }

        DesktopShared.Ticket.Notification.AddUser(null, TemporaryTicketGuid, ddlEmployeeAssignedTo.EmployeeId, ddlEmployeeAssignedTo.HelpDeskClientId == Desktop.SiteHelper.Client.Id.BBB);
        RebindGrids();
    }

    /// <summary>
    /// clear client contact edit form
    /// </summary>
    private void ClearContactEditForm()
    {
        TicketNotificationId = null;
        ClientContactId = null;
        ClientContactUserId = null;
        txtEditContactFirst.Text = "";
        txtEditContactLast.Text = "";
        txtEditContactEmail.Text = "";
        txtEditContactPhone.Text = "";
        txtEditContactPhoneExtension.Text = "";
        txtEditContactCellPhone.Text = "";
        txtEditContactHomePhone.Text = "";
    }

    /// <summary>
    /// close and then show client contact modal
    /// calling both methods to resolve issue with postbaclks
    /// </summary>
    private void CloseAndShowClientContact()
    {
        ScriptManager.RegisterStartupScript(this, this.GetType(), "Close", "CloseEditContact();", true);
        ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowEditContact", "$('#modal-edit-contact').modal('show');", true);
    }

    /// <summary>
    /// close and then show add client contact modal
    /// calling both methods to resolve issue with postbaclks
    /// </summary>
    private void CloseAndShowAddClientContact()
    {
        ScriptManager.RegisterStartupScript(this, this.GetType(), "Close", "CloseAddContact();", true);
        ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowAddContact", "$('#modal-add-contact').modal('show');", true);
    }

    /// <summary>
    /// set tag values
    /// </summary>
    /// <param name="ticketId"></param>
    private void SetTagValues(int? ticketId = null)
    {
        List<DesktopShared.Ticket.Tagged.TicketTag> _selectedTags = new List<DesktopShared.Ticket.Tagged.TicketTag>();
        foreach (string _selected in lbTicketTag.SelectedValues)
        {
            int _pos1 = _selected.IndexOf("|");
            int _pos2 = _selected.LastIndexOf("|");

            if ((_pos1 > 0) && (_pos2 > 0))
                _selectedTags.Add(new DesktopShared.Ticket.Tagged.TicketTag { Name = _selected.Substring(0, _pos1).Trim(), Public = _selected.Substring(_pos1 + 1, 1) == "1", ClientPublic = _selected.Substring(_pos2 + 1, 1) == "1" });
        }
        DesktopShared.Ticket.Tagged.Set(_selectedTags, DesktopShared.User.UserID, ticketId.HasValue ? ticketId.Value : -1, ticketId.HasValue ? "" : TemporaryTicketGuid);
    }

    /// <summary>
    /// configure page for internal only status
    /// </summary>
    private void ConfigureForInternal()
    {
        bool _internal = chkInternal.Checked;
        litClientNotificationHeader.Text = _internal ? " - Internal Ticket. Client Notifications Disabled" : "";
        btnAddRecipientClient.Visible = !_internal;
        //ddlClientContactRecipient.Visible = !_internal;
        lbClientContact.Visible = !_internal;
        gridTicketContactClient.Visible = !_internal;
        phAddContactButton.Visible = !_internal;

        if (phMasterClientsNotification.Visible)
        {
            litMasterClientNotificationHeader.Text = _internal ? " - Internal Ticket. Client Notifications Disabled" : "";
            btnAddRecipientMasterClient.Visible = !_internal;
            ddlMasterClientContactRecipient.Visible = !_internal;
            gridTicketContactMasterClient.Visible = !_internal;
        }
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
        int i = ddlSmartClient.SelectedClientId;

        if (Page.IsValid)
        {
            DesktopShared.EntityClasses.CscDefectsEntity ticket = new DesktopShared.EntityClasses.CscDefectsEntity();

            int _ticketHistoryTypeId = DesktopShared.Ticket.History.Type.Id.UpdateByEmployee;
            int _assignedTo = ddlEmployeeAssignedTo.EmployeeId;
            string _errorMessage = "";

            #region entered ticket criteria

            ticket.InternalOnly = chkInternal.Checked;
            ticket.Summary = txtSummary.Text.Trim();
            ticket.Description = txtDescription.Text.Trim();
            ticket.Assignedto = _assignedTo;
            int _dispositionId = ddlTicketDisposition.TicketDispositionId.Value;
            if ((_dispositionId == DesktopShared.SiteHelper.Ticket.Dispostion.Id.Active) && (_assignedTo != DesktopShared.User.UserID)) //do not allow user to add active ticket for other user
                ticket.FkDisposition = DesktopShared.SiteHelper.Ticket.Dispostion.Id.New;
            else
                ticket.FkDisposition = _dispositionId;
            ticket.FkStatus = _dispositionId == 93 ? 77 : 76;
            if (phLocation.Visible)
                ticket.ClientLocationId = ddlClientLocation.ClientLocationId;

            string _notes = "";
            List<DesktopShared.Ticket.CheckList.Node> _checkListTasks = DesktopShared.Ticket.CheckList.ParseItems(txtDescription.Text.Trim(), ref _notes);
            bool _checkListAdded = _checkListTasks.Count > 0;
            if (!_checkListAdded)
                _notes = txtDescription.Text.Trim();
            ticket.Description = Server.HtmlEncode(_notes);
            ticket.FkClient = ClientId;// ddlClient.ClientId;
            ticket.TicketTypeId = ddlTicketType.TicketTypeId;
            ticket.TicketCategoryId = ddlTicketCategory.TicketCategoryId;
            ticket.FkPriority = ddlTicketPriority.PriorityId > 0 ? ddlTicketPriority.PriorityId : (int?)null;

            #endregion

            #region client contact

            var objClientContact = new DesktopShared.EntityClasses.ClientContactEntity(ddlClientContact.ClientContactId.Value);
            if (objClientContact.Fields.State == EntityState.Fetched)
            {
                string _reportedByFullName = objClientContact.First.Trim() + " " + objClientContact.Last.Trim();
                if (_reportedByFullName.Length > 50)
                    _reportedByFullName = _reportedByFullName.Substring(0, 50);

                ticket.ReportedByFirst = objClientContact.First.Trim();
                ticket.ReportedByLast = objClientContact.Last.Trim();
                ticket.Email = objClientContact.Email.Trim();
                ticket.Phone = objClientContact.Busphone.Trim();
                ticket.Reportedby = _reportedByFullName;
                ticket.FkUser = DesktopShared.User.GetIdForClientContact(objClientContact.PclientContact);
            }

            #endregion

            #region scheduled

            DateTime _scheduledDate = DateTime.Now;
            if (ucScheduledDate.SelectedDate.HasValue)
            {
                _scheduledDate = new DateTime(
                    ucScheduledDate.SelectedDate.Value.Year, ucScheduledDate.SelectedDate.Value.Month, ucScheduledDate.SelectedDate.Value.Day,
                    ddlScheduledTime.Hour, ddlScheduledTime.Minute, 0);

                ticket.TicketScheduled = true;
            }
            ticket.ScheduledDate = ucScheduledDate.SelectedDate.HasValue ? _scheduledDate : (DateTime?)null;
            ticket.ScheduledDuration = DesktopShared.Utility.Time.TimeStringToDouble(String.Format("{0}{1}", ddlDurationTime.Hour.ToString().PadLeft(2, '0'), ddlDurationTime.Minute.ToString().PadLeft(2, '0')));

            #endregion

            #region defaults

            ticket.AllowAutoClose = "N";
            ticket.TimerInterval = null;
            ticket.Designation = "N";
            ticket.ReceiveMethod = "P";
            ticket.FkCscprojects = 117;
            ticket.ResponsivePage = true;

            #endregion

            ticket.ReceiveDate = DateTime.Now;
            ticket.EnteredBy = DesktopShared.User.UserID;
            ticket.Lastupdated = DateTime.Now;
            ticket.DefaultProjectId = ddlDefaultProject.SelectedProjtaskId;

            if (_assignedTo != DesktopShared.User.UserID)
                ticket.AwaitingResponseUserId = _assignedTo;

            ticket.Save();
            int _ticketId = ticket.Pcscdefects;

            #region  timesheet

            bool _addTimesheet = AddTimesheetEntry;
            if (_addTimesheet)
            {
                //time spent
                string _timesheetTimeSpent = ddlTimesheetTime.Hour.ToString().PadLeft(2, '0') + ddlTimesheetTime.Minute.ToString().PadLeft(2, '0');
                int _intHourSpent = Convert.ToInt32(_timesheetTimeSpent.Substring(0, 2));
                int _intMinuteSpent = Convert.ToInt32(_timesheetTimeSpent.Substring(2));
                string _strMinuteSpent = "";
                switch (_intMinuteSpent)
                {
                    case 15:
                        _strMinuteSpent = ".25";
                        break;
                    case 30:
                        _strMinuteSpent = ".5";
                        break;
                    case 45:
                        _strMinuteSpent = ".75";
                        break;
                }

                double _hours = Convert.ToDouble(_intHourSpent.ToString() + _strMinuteSpent);
                int _projectTaskId = ddlTimesheetProject.SelectedProjtaskId;

                DesktopShared.EntityClasses.TimesheetEntity _timeSheet = new DesktopShared.EntityClasses.TimesheetEntity();
                _timeSheet.Date = ucTimesheetDate.SelectedDate.Value;
                _timeSheet.Time = TimesheetUseStartTime ? ddlTimesheetStartTime.Hour.ToString().PadLeft(2, '0') + ddlTimesheetStartTime.Minute.ToString().PadLeft(2, '0') : TimesheetStartTime;
                _timeSheet.Hrs = _hours;
                _timeSheet.Projtaskid = _projectTaskId.ToString();
                _timeSheet.FkProjtask = _projectTaskId;
                _timeSheet.FkClient = ddlTimesheetProject.SelectedClientId;
                _timeSheet.Descr1 = ddlTimesheetActivity.TaskActionName;
                _timeSheet.FkTaskaction = ddlTimesheetActivity.TaskActionId;
                _timeSheet.Memo = Server.HtmlEncode(_notes);
                _timeSheet.InternalComments = Server.HtmlEncode(txtInternalNotes.Text.Trim());
                _timeSheet.Workid = DesktopShared.User.GetUserNameOnly();
                _timeSheet.FkEmployee = DesktopShared.User.EmployeeID;
                _timeSheet.FkCscdefects = _ticketId;
                _timeSheet.FkProject = ddlTimesheetProject.SelectedProjectId;

                #region null value not allowed

                _timeSheet.Clientcode = "";
                _timeSheet.Jobno = "";
                _timeSheet.Taskno = "";
                _timeSheet.Fncode = "";
                _timeSheet.Bilhrs = 0;
                _timeSheet.Location = "";
                _timeSheet.Miles = 0;
                _timeSheet.Tolls = 0;
                _timeSheet.Parking = 0;
                _timeSheet.Masstran = 0;
                _timeSheet.Misc = 0;
                _timeSheet.Bilamt = 0;
                _timeSheet.Commamt = 0;
                _timeSheet.Ssman = "";
                _timeSheet.Workdate = "";
                _timeSheet.Timeid = "";

                #endregion

                string dateTimeString = DesktopShared.Utility.Date.DateToString(DateTime.Now);
                _timeSheet.Created = dateTimeString;
                _timeSheet.LastUpdated = dateTimeString;
                _timeSheet.LastUpdatedby = DesktopShared.User.GetUserNameOnly();
                _timeSheet.Save();

                //check project hours, send email alert if past threshold
                DesktopShared.Timesheet.CheckProjectHours(_projectTaskId, _timeSheet.Ptimesheet, ddlTimesheetProject.SelectedClientId);
            }
            #endregion

            #region history entries and email

            string _employeeEmails = string.Join(",", gridTicketContactEmployee.SelectedEmails);
            string _clientEmails = "";
            if (!chkInternal.Checked)
            {
                List<string> _clientEmailsList = gridTicketContactClient.SelectedEmails;
                if (phMasterClientsNotification.Visible)
                {
                    foreach (string _masterClientContactEmail in gridTicketContactMasterClient.SelectedEmails)
                    {
                        if (!_clientEmailsList.Contains(_masterClientContactEmail))
                            _clientEmailsList.Add(_masterClientContactEmail);
                    }
                }
                _clientEmails = string.Join(",", _clientEmailsList);
            }

            bool _externalMailSuccess = true;
            int? _externalMessageTrackingId = null;
            bool _internalMailSuccess = true;
            int? _internalMessageTrackingId = null;
            int _ticketHistoryId = 0;
            DesktopShared.Ticket.History.Add(
                _ticketId, //, //ticket id
                DesktopShared.User.UserID, //user id, 
                "Ticket Added: Desktop", //notes
                txtInternalNotes.Text.Trim(), //internal notes 
                true, // is new ticket
                _clientEmails, // client email addresses
                _employeeEmails, // employee email addresses
                true,  // send email out
                _ticketHistoryTypeId,  // history type id
                DateTime.Now,  //date/time stamp to user for history note created
                null, //ids for file to attach to email
                "", //update by override
                null, //internal ticket history type id (override history type id)
                ref _ticketHistoryId, //entity id
                ref _externalMailSuccess, //email successfully sent to client
                ref _externalMessageTrackingId, //message tracking id for email to client
                ref _internalMailSuccess, //email successfully sent to employees
                ref _internalMessageTrackingId, //message tracking id for email to employee
                true //is responsive page
                );

            #endregion

            #region file upload

            #region telerik

            /*
            _errorMessage = "";
            foreach (UploadedFile _file in rauMain.UploadedFiles)
            {
                try
                {
                    #region upload to azure

                    string _fileName = "";
                    System.IO.MemoryStream _ms = new System.IO.MemoryStream();
                    _file.InputStream.CopyTo(_ms);
                    _ms.Seek(0, System.IO.SeekOrigin.Begin);

                    bool _uploaded = DesktopShared.AzureHelper.UploadBlob(DesktopShared.AzureHelper.ContainerName.ticket, _ticketId.ToString(), _file.GetExtension(), _ms, ref _fileName, ref _errorMessage);

                    if (_ms != null)
                        _ms.Dispose();

                    #endregion

                    if (!_uploaded)
                    {
                        DesktopShared.Bootstrap.Alert.DisplayMessage(litMessage, String.Format("An error has occured while uploading to Azure - {0}.", _errorMessage), DesktopShared.Bootstrap.Alert.AlertType.Danger);
                        return;
                    }

                    //create ticket file entity
                    DesktopShared.EntityClasses.CscdefectfileEntity objFile = new DesktopShared.EntityClasses.CscdefectfileEntity();
                    objFile.Name = _file.GetName();
                    objFile.FkCscdefects = ticket.Pcscdefects;
                    objFile.UploadedBy = DesktopShared.User.UserID.ToString();
                    objFile.FkProject = ticket.CscProjects.FkProject.Value;
                    objFile.FkCscprojects = ticket.FkCscprojects.Value;
                    objFile.FkClient = ticket.FkClient.Value;
                    objFile.AzureFileId = _fileName.Trim();
                    objFile.AzureContainerName = DesktopShared.AzureHelper.ContainerName.ticket.ToString();
                    objFile.AzureStorage = true;
                    objFile.FileExtension = _file.GetExtension();
                    objFile.Save();

                    //create history note
                    DesktopShared.EntityClasses.CscHistoryEntity objHistory = new DesktopShared.EntityClasses.CscHistoryEntity();
                    objHistory.CscdefectsId = ticket.Pcscdefects;
                    objHistory.Notes = String.Format("{0}{1}", DesktopShared.SiteHelper.Message.FileUploaded, _file.GetName());
                    objHistory.UpdatedBy = DesktopShared.User.UserID;
                    objHistory.InternalUsage = "N";
                    objHistory.FkStatus = ticket.FkStatus;
                    objHistory.InternalRecipients = "";
                    objHistory.ExternalRecipients = "";
                    objHistory.TicketHistoryTypeId = DesktopShared.Ticket.History.Type.Id.FileUpload;
                    objHistory.Save();
                }
                catch (Exception ex)
                {
                    _errorMessage = ex.ToString();
                }
            }
            */
            #endregion

            #region asp.net file upload

            _errorMessage = "";
            foreach (HttpPostedFile postedFile in fuOne.PostedFiles)
            {
                if (postedFile != null && postedFile.ContentLength > 0)
                {
                    try
                    {
                        #region upload to azure

                        string _extension = System.IO.Path.GetExtension(postedFile.FileName);
                        string _fileName = "";
                        System.IO.MemoryStream _ms = new System.IO.MemoryStream();
                        postedFile.InputStream.CopyTo(_ms);
                        _ms.Seek(0, System.IO.SeekOrigin.Begin);

                        bool _uploaded = DesktopShared.AzureHelper.UploadMultipleBlob(System.IO.Path.GetFileNameWithoutExtension(postedFile.FileName),DesktopShared.AzureHelper.ContainerName.ticket, _ticketId.ToString(), _extension, _ms, ref _fileName, ref _errorMessage);

                        if (_ms != null)
                            _ms.Dispose();

                        #endregion

                        if (!_uploaded)
                        {
                            DesktopShared.Bootstrap.Alert.DisplayMessage(litMessage, String.Format("An error has occured while uploading to Azure - {0}.", _errorMessage), DesktopShared.Bootstrap.Alert.AlertType.Danger);
                            return;
                        }

                        //create ticket file entity
                        DesktopShared.EntityClasses.CscdefectfileEntity objFile = new DesktopShared.EntityClasses.CscdefectfileEntity();
                        objFile.Name = postedFile.FileName.Trim();
                        objFile.FkCscdefects = ticket.Pcscdefects;
                        objFile.UploadedBy = DesktopShared.User.UserID.ToString();
                        objFile.FkProject = ticket.CscProjects.FkProject.Value;
                        objFile.FkCscprojects = ticket.FkCscprojects.Value;
                        objFile.FkClient = ticket.FkClient.Value;
                        objFile.AzureFileId = _fileName.Trim();
                        objFile.AzureContainerName = DesktopShared.AzureHelper.ContainerName.ticket.ToString();
                        objFile.AzureStorage = true;
                        objFile.FileExtension = _extension;
                        objFile.Save();

                        //create history note
                        DesktopShared.EntityClasses.CscHistoryEntity objHistory = new DesktopShared.EntityClasses.CscHistoryEntity();
                        objHistory.CscdefectsId = ticket.Pcscdefects;
                        objHistory.Notes = String.Format("{0}{1}", DesktopShared.SiteHelper.Message.FileUploaded, postedFile.FileName.Trim());
                        objHistory.UpdatedBy = DesktopShared.User.UserID;
                        objHistory.InternalUsage = "N";
                        objHistory.FkStatus = ticket.FkStatus;
                        objHistory.InternalRecipients = "";
                        objHistory.ExternalRecipients = "";
                        objHistory.TicketHistoryTypeId = DesktopShared.Ticket.History.Type.Id.FileUpload;
                        objHistory.Save();
                    }
                    catch (Exception ex)
                    {
                        _errorMessage = ex.ToString();
                    }
                }
            }

            #endregion

            #endregion

            //tags
            DesktopShared.Ticket.Tagged.SetTicketIdForTempGuid(TemporaryTicketGuid, _ticketId);
            SetTagValues(_ticketId);

            //ticket recipients
            if(objClientContact.NotificationPreferences =="on")
                DesktopShared.Ticket.Notification.SetTicketIdForTempGuid(TemporaryTicketGuid, _ticketId);

            //ticket is set to active, all other active tickets need to be set to idle
            if ((_dispositionId == DesktopShared.SiteHelper.Ticket.Dispostion.Id.Active) && (_assignedTo == DesktopShared.User.UserID))
                DesktopShared.Ticket.MakeActive(ticket.Pcscdefects, DesktopShared.User.UserID, _assignedTo, false, "");

            //check list tasks
            if (_checkListAdded)
                DesktopShared.Ticket.CheckList.AddByList(ticket.Pcscdefects, _checkListTasks, DesktopShared.User.UserID);

            //calender entry
            bool _schedulingError = false;
            if (ucScheduledDate.SelectedDate.HasValue)
                _schedulingError = !DesktopShared.Exchange.Calendar.CreateAppointment(ticket.Pcscdefects, _assignedTo, false, DesktopShared.User.UserID);

            #region confirmation

            string _confirmationMessage = "Ticket # " + _ticketId + " has been added. ";
            if (!String.IsNullOrWhiteSpace(_errorMessage))
                _confirmationMessage = String.Format("Ticket has been updated but there was an error with the file upload. Error: {0}. ", _errorMessage);
            else if (_schedulingError)
                _confirmationMessage = String.Format("Ticket has been added. Unable to add to exchange calendar so no scheduling occured. ", "");
            _confirmationMessage += String.Format("<a href=\"Detail2.aspx?Id={0}\" class=\"alert-link\">View Ticket</a>. ", _ticketId);
            _confirmationMessage += "<a href=\"Add2.aspx\" class=\"alert-link\">Add new Ticket</a> - ";
            _confirmationMessage += String.Format("{0}.", DateTime.Now);

            pnlContainer.Visible = false;
            if (String.IsNullOrWhiteSpace(_errorMessage) && !_schedulingError)
                DesktopShared.Bootstrap.Alert.DisplayMessage(litMessage, _confirmationMessage, DesktopShared.Bootstrap.Alert.AlertType.Success, false);
            else
                DesktopShared.Bootstrap.Alert.DisplayMessage(litMessage, _confirmationMessage, DesktopShared.Bootstrap.Alert.AlertType.Danger, false);

            #region for Summary
            pnlSummaryAdd.Visible = true;
            string _summaryassignedTo = "";
            if (ddlEmployeeAssignedTo.EmployeeName.Trim().Length > 0)
                _summaryassignedTo = String.Format("- Assigned to {0}", ddlEmployeeAssignedTo.EmployeeName.Trim());

            Uri uri = Request.Url;
            string baseUrl = uri.Scheme + "://" + uri.Host;
            if (!uri.IsDefaultPort)
                baseUrl += ":" + uri.Port;

            string path = uri.AbsolutePath;
            int lastSlash = path.LastIndexOf('/');
            if (lastSlash >= 0)
                baseUrl += path.Substring(0, lastSlash + 1);
            string detail = String.Format("Detail2.aspx?Id={0}", ticket.Pcscdefects);
            string fullurl = String.Format("{0}{1}", baseUrl, detail);

            string _textToCopy = String.Format("{0} - {1}{2} - {3}", ddlSmartClient.SelectedClientName.Trim(), Server.HtmlEncode(ticket.Summary.Trim()), _summaryassignedTo, fullurl);
            litCopySummary.Text = String.Format("<a class=\"copy-btn\" data-clipboard-text=\"{0}\" href=\"javascript:void(0);\" style=\"padding-left:5px;\" tooltip=\"Copy To Clipboard\"><i class=\"fa fa-clipboard\" title=\"Copy To Clipboard\"></i></a>", _textToCopy);
            litSummary.Text = DesktopShared.Utility.String.ReplaceLineBreaks(txtSummary.Text);
            #endregion

            #endregion

            #region autoclosed

            DateTime? _changeautocloseddate = null;
            DateTime? _firstnotifdate = null;
            DateTime? _secondnotifdate = null;
            DateTime? _autocloseddate = null;
            bool? _autoclosedstatus = null;
            string err = "";

            int autocloseid = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["AutoCloseId"]);

            if (ddlTicketDisposition.TicketDispositionId == autocloseid)
                DesktopShared.AutoCloses.AddAutoClosedLog(_ticketId);
            #endregion
        }
    }

    /// <summary>
    /// add contact button on click
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnAddContact_Click(object sender, EventArgs e)
    {
        if (this.IsValid)
        {
            string _first = txtAddContactFirst.Text.Trim();
            string _last = txtAddContactLast.Text.Trim();
            string _email = txtAddContactEmail.Text.Trim();
            string _phone = txtAddContactPhone.Text.Trim();
            string _extension = txtAddContactPhoneExtension.Text.Trim();
            string _cell = txtAddContactCellPhone.Text.Trim();
            string _home = txtAddContactHomePhone.Text.Trim();

            //add client contact entity
            DesktopShared.EntityClasses.ClientContactEntity _clientContact = new DesktopShared.EntityClasses.ClientContactEntity();
            _clientContact.Created = DateTime.Now.ToString();
            _clientContact.FkClient = ClientId;
            _clientContact.First = DesktopShared.Utility.String.Truncate(_first, 12);
            _clientContact.Last = DesktopShared.Utility.String.Truncate(_last, 18);
            _clientContact.Email = _email;
            _clientContact.Busphone = DesktopShared.Utility.String.Truncate(_phone, 17);
            _clientContact.Busext = DesktopShared.Utility.String.Truncate(_extension, 6);
            _clientContact.Cellphone = DesktopShared.Utility.String.Truncate(_cell, 17);
            _clientContact.Homephone = DesktopShared.Utility.String.Truncate(_home, 17);
            _clientContact.LastUpdated = DesktopShared.Utility.Date.DateToString(DateTime.Now);
            _clientContact.LastUpdatedby = DesktopShared.User.UserName;
            _clientContact.Active = "Y";
            _clientContact.Save();

            //user user entity
            var objUser = DesktopShared.User.GetForClientContact(_clientContact.PclientContact);
            if ((objUser != null) && (objUser.Fields.State == EntityState.Fetched))
            {
                objUser.ClientPortalActive = true;
                objUser.Save();
            }

            //add to notification
            DesktopShared.EntityClasses.TicketNotificationEntity objTicketNotification = new DesktopShared.EntityClasses.TicketNotificationEntity();
            objTicketNotification.TicketId = null;
            objTicketNotification.TemporaryGuid = TemporaryTicketGuid;
            objTicketNotification.ClientContactEmail = _email;
            objTicketNotification.ClientContactName = String.Format("{0} {1}", _first, _last);
            objTicketNotification.Save();

            //load 
            ClientContactId = _clientContact.PclientContact;
            ddlClientContact.ClientContactId = _clientContact.PclientContact;
            ddlClientContact.PopulateDropDownList();

            txtAddContactFirst.Text = "";
            txtAddContactLast.Text = "";
            txtAddContactEmail.Text = "";
            txtAddContactPhone.Text = "";
            txtAddContactPhoneExtension.Text = "";
            txtAddContactCellPhone.Text = "";
            txtAddContactHomePhone.Text = "";

            CheckClientContactList();
            RebindGrids();

            DesktopShared.Bootstrap.Alert.DisplayMessage(litMessage, String.Format("Contact has been added - {0}.", DateTime.Now), DesktopShared.Bootstrap.Alert.AlertType.Success);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "btnAddContactClose", "CloseAddContact();", true);
        }
    }

    /// <summary>
    /// add bbb recipient button on click
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnAddRecipientBbb_Click(object sender, EventArgs e)
    {
        for (int i = 0; i <= lbEmployeeRecipient.SelectedValues.Count - 1; i++)
        {
            DesktopShared.EntityClasses.TicketNotificationEntity objTicketNotification = new DesktopShared.EntityClasses.TicketNotificationEntity();
            objTicketNotification.TemporaryGuid = TemporaryTicketGuid.Trim();
            objTicketNotification.EmployeeEmail = lbEmployeeRecipient.SelectedValues[i].Trim();
            objTicketNotification.EmployeeName = lbEmployeeRecipient.SelectedText[i].Trim();
            objTicketNotification.Save();
        }
        RebindGrids();
        ddlEmployeeAssignedTo.Focus();
    }

    /// <summary>
    /// add client contact recipient button on click
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnAddRecipientClient_Click(object sender, EventArgs e)
    {
        for (int i = 0; i <= lbClientContact.SelectedValues.Count - 1; i++)
        {
            DesktopShared.EntityClasses.TicketNotificationEntity objTicketNotification = new DesktopShared.EntityClasses.TicketNotificationEntity();
            objTicketNotification.TemporaryGuid = TemporaryTicketGuid.Trim();

            objTicketNotification.ClientContactEmail = lbClientContact.SelectedValues[i].Trim();
            objTicketNotification.ClientContactName = lbClientContact.SelectedText[i].Trim();
            objTicketNotification.Save();
        }
        RebindGrids();
        ddlEmployeeAssignedTo.Focus();
    }

    /// <summary>
    /// add master client contact recpient (employee) button on click
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnAddRecipientMasterClient_Click(object sender, EventArgs e)
    {
        DesktopShared.EntityClasses.TicketNotificationEntity objTicketNotification = new DesktopShared.EntityClasses.TicketNotificationEntity();
        objTicketNotification.TemporaryGuid = TemporaryTicketGuid.Trim();
        objTicketNotification.MasterClientContactEmail = ddlMasterClientContactRecipient.SelectedValue.Trim();
        objTicketNotification.MasterClientContactName = ddlMasterClientContactRecipient.ClientContactName.Trim();
        objTicketNotification.Save();

        RebindGrids();
        ddlEmployeeAssignedTo.Focus();

    }


    /// <summary>
    /// save contact button on click
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSaveContact_Click(object sender, EventArgs e)
    {
        if (this.IsValid)
        {
            string _first = txtEditContactFirst.Text.Trim();
            string _last = txtEditContactLast.Text.Trim();
            string _email = txtEditContactEmail.Text.Trim();
            string _phone = txtEditContactPhone.Text.Trim();
            string _extension = txtEditContactPhoneExtension.Text.Trim();
            string _cell = txtEditContactCellPhone.Text.Trim();
            string _home = txtEditContactHomePhone.Text.Trim();

            DesktopShared.EntityClasses.ClientContactEntity _clientContact = null;
            if (!ClientContactId.HasValue)//add
            {
                _clientContact = new DesktopShared.EntityClasses.ClientContactEntity();
                _clientContact.Created = DateTime.Now.ToString();
                _clientContact.FkClient = ClientId;

            }
            else //update
                _clientContact = new DesktopShared.EntityClasses.ClientContactEntity(ClientContactId.Value);

            _clientContact.First = DesktopShared.Utility.String.Truncate(_first, 12);
            _clientContact.Last = DesktopShared.Utility.String.Truncate(_last, 18);
            _clientContact.Email = _email;
            _clientContact.Busphone = DesktopShared.Utility.String.Truncate(_phone, 17);
            _clientContact.Busext = DesktopShared.Utility.String.Truncate(_extension, 6);
            _clientContact.Cellphone = DesktopShared.Utility.String.Truncate(_cell, 17);
            _clientContact.Homephone = DesktopShared.Utility.String.Truncate(_home, 17);
            _clientContact.LastUpdated = DesktopShared.Utility.Date.DateToString(DateTime.Now);
            _clientContact.LastUpdatedby = DesktopShared.User.UserName;
            _clientContact.Save();

            #region update ticket notification 
            
            if (TicketNotificationId.HasValue)
            {
                var objTicketNotification = new DesktopShared.EntityClasses.TicketNotificationEntity(TicketNotificationId.Value);
                if (objTicketNotification.Fields.State == EntityState.Fetched)
                {
                    objTicketNotification.PrimaryContact = true;
                    objTicketNotification.ClientContactEmail = _email;
                    objTicketNotification.ClientContactName = String.Format("{0} {1}", _first, _last);
                    objTicketNotification.Save();
                }
            }

            #endregion

            ddlClientContact.ClientContactId = _clientContact.PclientContact;
            ddlClientContact.PopulateDropDownList();

            CheckClientContactList();
            RebindGrids();

            DesktopShared.Bootstrap.Alert.DisplayMessage(litMessage, "Contact has been updated.", DesktopShared.Bootstrap.Alert.AlertType.Success);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "btnSaveContactClose", "CloseEditContact();", true);
        }
        else
            CloseAndShowClientContact();
    }

    /// <summary>
    /// add tag on click
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnAddTag_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            //save existing selections
            SetTagValues();

            //user id (public tags are not assigned to user)
            int? userId = chkTagPublic.Checked ? (int?)null : DesktopShared.User.UserID;

            string _tagValue = txtNewTag.Text.Trim();

            //add tag
            DesktopShared.Ticket.Tagged.AddTagValue(-1, _tagValue, userId, chkTagPublic.Checked, chkTagClientPublic.Checked, TemporaryTicketGuid);

            //clear selection
            txtNewTag.Text = "";
            chkTagPublic.Checked = false;
            chkTagClientPublic.Checked = false;

            RebindGrids();
            lbTicketTag.Populate(DesktopShared.Ticket.Tagged.GetSelectedTagTextForTicket(-1, DesktopShared.User.UserID, TemporaryTicketGuid));
            lblEnd.Focus();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "btnAddTagClose", "CloseAddTag();", true);
        }
        else
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowAddTag", "$('#modal-add-tag').modal('show');", true);
    }

    /// <summary>
    /// internal on checked changed
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void chkInternal_CheckedChanged(object sender, EventArgs e)
    {
        ConfigureForInternal();
        RebindGrids();
    }

    #region custom validators

    /// <summary>
    /// validate at least one 1 file has been upload
    /// </summary>
    /// <param name="server"></param>
    /// <param name="e"></param>
    protected void cvFile_ServerValidate(object server, ServerValidateEventArgs args)
    {
        #region asp.net file upload

        if (fuOne.PostedFile != null && fuOne.PostedFile.ContentLength > 0)
        {
            cvFile.ErrorMessage = String.Format("Invalid File Upload. Allowed extensions: {0}", string.Join(",", DesktopShared.Utility.ValidFileUploadExtensions().ToArray()));
            args.IsValid = DesktopShared.Utility.ValidFileUploadExtensions().Contains(System.IO.Path.GetExtension(fuOne.PostedFile.FileName).Replace(".", "").ToLower());
        }

        #endregion
    }

    /// <summary>
    /// validate email address for editing client contact
    /// </summary>
    /// <param name="source"></param>
    /// <param name="args"></param>
    protected void cvEditContactEmail_ServerValidate(object source, ServerValidateEventArgs args)
    {
        string _email = txtEditContactEmail.Text.Trim();
        if (!DesktopShared.Utility.IsValidEmailAddress(_email))
        {
            cvEditContactEmail.ErrorMessage = "Invalid Email Address";
            args.IsValid = false;
            CloseAndShowClientContact();
            return;
        }

        args.IsValid = true;
    }

    /// <summary>
    /// validate email address for adding client contact
    /// </summary>
    /// <param name="source"></param>
    /// <param name="args"></param>
    protected void cvAddContactEmail_ServerValidate(object source, ServerValidateEventArgs args)
    {
        string _email = txtAddContactEmail.Text.Trim();
        if (!DesktopShared.Utility.IsValidEmailAddress(_email))
        {
            cvAddContactEmail.ErrorMessage = "Invalid Email Address";
            args.IsValid = false;
            CloseAndShowAddClientContact();
            return;
        }
        args.IsValid = true;
    }

    /// <summary>
    /// timesheet validation
    /// </summary>
    /// <param name="source"></param>
    /// <param name="args"></param>
    protected void cvTimesheet_ServerValidate(object source, ServerValidateEventArgs args)
    {
        args.IsValid = true;
        if (!AddTimesheetEntry)
            return;

        StringBuilder _sbTimesheetError = new StringBuilder();

        if (ddlTimesheetProject.SelectedProjectId < 1)
            _sbTimesheetError.Append("Project");

        if (ddlTimesheetActivity.TaskActionId < 1)
        {
            if (_sbTimesheetError.Length > 0)
                _sbTimesheetError.Append(", ");
            _sbTimesheetError.Append("Activity");
        }

        if (!ucTimesheetDate.SelectedDate.HasValue)
        {
            if (_sbTimesheetError.Length > 0)
                _sbTimesheetError.Append(", ");
            _sbTimesheetError.Append("Date");
        }

        if ((ddlTimesheetTime.Hour <= 0) && (ddlTimesheetTime.Minute <= 0))
        {
            if (_sbTimesheetError.Length > 0)
                _sbTimesheetError.Append(", ");
            _sbTimesheetError.Append("Time");
        }

        if (TimesheetUseStartTime)
        {
            if ((ddlTimesheetStartTime.Hour <= 0) && (ddlTimesheetStartTime.Minute <= 0))
            {
                if (_sbTimesheetError.Length > 0)
                    _sbTimesheetError.Append(", ");
                _sbTimesheetError.Append("Start Time");
            }
        }

        if (_sbTimesheetError.Length == 0)
            return;

        args.IsValid = false;
        cvTimesheet.ErrorMessage = String.Format("The following Timesheet fields are required: {0}", _sbTimesheetError.ToString());
        ScriptManager.RegisterClientScriptBlock(this, Page.GetType(), "ToTheTop", "ToTopOfPage();", true);
    }

    /// <summary>
    /// scheduling validation
    /// </summary>
    /// <param name="source"></param>
    /// <param name="args"></param>
    protected void cvScheduled_ServerValidate(object source, ServerValidateEventArgs args)
    {
        if (!ucScheduledDate.SelectedDate.HasValue && (ddlScheduledTime.Hour == 0) && (ddlScheduledTime.Minute == 0))
        {
            args.IsValid = true;
            return;
        }

        if (!ucScheduledDate.SelectedDate.HasValue)
        {
            cvScheduled.ErrorMessage = "Scheduled Date is required";
            args.IsValid = false;
            return;
        }

        if ((ddlScheduledTime.Hour == 0) && (ddlScheduledTime.Minute == 0))
        {
            cvScheduled.ErrorMessage = "Scheduled Time is required";
            args.IsValid = false;
            return;
        }

        if ((ddlDurationTime.Hour == 0) && (ddlDurationTime.Minute == 0))
        {
            cvScheduled.ErrorMessage = "Scheduled Duration is required";
            args.IsValid = false;
            return;
        }
    }


    /// <summary>
    /// server validation for add new tag
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cvAddTag_ServerValidate(object sender, ServerValidateEventArgs args)
    {
        args.IsValid = true;
        //text for ddl selection
        if (String.IsNullOrWhiteSpace(txtNewTag.Text))
        {
            cvAddTag.ErrorMessage = "Tag value is required.";
            args.IsValid = false;
            ScriptManager.RegisterClientScriptBlock(this, Page.GetType(), "ToTheTop", "ToTopOfPage();", true);
            return;
        }

        //client public must also be public
        if (chkTagClientPublic.Checked && !chkTagPublic.Checked)
        {
            cvAddTag.ErrorMessage = "Client Public tags must also be flagged as Public";
            args.IsValid = false;
            ScriptManager.RegisterClientScriptBlock(this, Page.GetType(), "ToTheTop", "ToTopOfPage();", true);
            return;
        }

        //characters "|", "(P)", "(PC)" not allowed
        string _tagText = txtNewTag.Text.Trim();
        if (_tagText.Length > 0)
        {
            if (_tagText.Contains("|") || _tagText.Contains("(P)") || _tagText.Contains("(PC)"))
            {
                cvAddTag.ErrorMessage = "Tags may not contain \"|\", \"(P)\", or \"(PC)\"";
                args.IsValid = false;
                ScriptManager.RegisterClientScriptBlock(this, Page.GetType(), "ToTheTop", "ToTopOfPage();", true);
                return;
            }
        }

        //check tag is unique
        //int? userId = chkTagPublic.Checked ? (int?)null : DesktopShared.User.UserID;
        //args.IsValid = !DesktopShared.Ticket.Tagged.TagExistsForTicket(TicketId.Value, txtNewTag.Text.Trim(), userId);
    }

    #endregion

    #endregion

    #region private properties

    /// <summary>
    /// get/set client id
    /// </summary>
    private int? ClientId
    {
        get
        {
            object obj = this.ViewState["cid_at"];
            return (obj == null) ? (int?)null : (int)obj;
        }
        set { this.ViewState["cid_at"] = value; }
    }

    /// <summary>
    /// get/set master client id
    /// </summary>
    private int? MasterClientId
    {
        get
        {
            object obj = this.ViewState["mcid_at"];
            return (obj == null) ? (int?)null : (int)obj;
        }
        set { this.ViewState["mcid_at"] = value; }
    }

    /// <summary>
    /// get/set client contact id
    /// </summary>
    private int? ClientContactId
    {
        get
        {
            object obj = this.ViewState["ccid_at"];
            return (obj == null) ? (int?)null : (int)obj;
        }
        set { this.ViewState["ccid_at"] = value; }
    }

    /// <summary>
    /// get/set client contact user id
    /// </summary>
    private int? ClientContactUserId
    {
        get
        {
            object obj = this.ViewState["ccuid_at"];
            return (obj == null) ? (int?)null : (int)obj;
        }
        set { this.ViewState["ccuid_at"] = value; }
    }

    /// <summary>
    /// get/set ticket notificatino id
    /// </summary>
    private int? TicketNotificationId
    {
        get
        {
            object obj = this.ViewState["tnid_at"];
            return (obj == null) ? (int?)null : (int)obj;
        }
        set { this.ViewState["tnid_at"] = value; }
    }

    
    /// <summary>
    /// get/set timesheet start time
    /// </summary>
    private string TimesheetStartTime
    {
        get
        {
            object obj = this.ViewState["tst_at"];
            return (obj == null) ? "" : (string)obj;
        }
        set { this.ViewState["tst_at"] = value; }
    }

    /// <summary>
    /// get/set temp guid used for recipients / tags
    /// </summary>
    private string TemporaryTicketGuid
    {
        get
        {
            object obj = this.ViewState["trg_at"];
            return (obj == null) ? "" : (string)obj;
        }
        set { this.ViewState["trg_at"] = value; }
    }

    /// <summary>
    /// get user is adding timesheet entry
    /// </summary>
    private bool AddTimesheetEntry
    {
        get { return chkAddTimesheet.Checked; }
    }

    /// <summary>
    /// get/set use start time for timesheet
    /// </summary>
    private bool TimesheetUseStartTime
    {
        get
        {
            object obj = this.ViewState["tust_at"];
            return (obj == null) ? false : (bool)obj;
        }
        set { this.ViewState["tust_at"] = value; }
    }

    #endregion
}
