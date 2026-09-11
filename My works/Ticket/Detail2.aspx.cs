using SD.LLBLGen.Pro.ORMSupportClasses;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class Ticket_Detail2 : BasePage
{
    private string _ticketCheckListHeader = "";

    /// <summary>
    /// page load
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        #region event handlers
        gridTicketContactClient.evItemCommand += Grid_evItemCommand;
        gridTicketContactEmployee.evItemCommand += Grid_evItemCommand;
        gridTicketContactMasterClient.evItemCommand += Grid_evItemCommand;
        gridTicketHistory.evItemCommand += Grid_evItemCommand;
        gridTicketFile.evItemCommand += new UserControl_Grid_TicketFileResponsive.ItemCommandHandler(GridTicketFile_evItemCommand);
        gridTicketFile.evDeleteCompleted += gridTicketFile_evDeleteCompleted;

        DropDownList _ddl = ddlEditClientContact.GetDropDownList();
        _ddl.AutoPostBack = true;
        _ddl.SelectedIndexChanged += ddlEditClientContact_SelectedIndexChanged;

        ddlEmployeeAssignedTo.EmployeeTypeToDisplay = UserControl_DropDownList_Employee.EmployeeType.AssignTo;
        _ddl = ddlEmployeeAssignedTo.GetDropDownList();
        _ddl.AutoPostBack = true;
        _ddl.SelectedIndexChanged += ddlEmployeeAssignedTo_SelectedIndexChanged;

        _ddl = ddlTicketCheckListMaster.GetDropDownList();
        _ddl.AutoPostBack = true;
        _ddl.SelectedIndexChanged += ddlTicketCheckListMaster_SelectedIndexChanged;

        _ddl = ddlDefaultProject.GetDropDownList();
        _ddl.AutoPostBack = true;
        _ddl.SelectedIndexChanged += ddlDefaultProject_SelectedIndexChanged;

        #endregion

        ConfigurePhoneValidations();
        ConfigureForDevice();
        //rauMain.AllowedFileExtensions = DesktopShared.Utility.ValidFileUploadExtensions().ToArray();

        if (!this.IsPostBack)
        {            
            SetUpPage();

            DesktopShared.TypedListClasses.TicketRow objTicket = null;
            if (GetTicketId(ref objTicket))
            {
                ddlTicketCategory.PopulateDropDownList(objTicket.FkClient);
                
                LoadValues(objTicket, true, true);
                
                ddlDefaultProject.SelectedClientId = ClientId.Value;
                ddlDefaultProject.Populate();
            }
        }
    }

    private void ddlDefaultProject_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ddlTimesheetProject.SelectedClientId = ClientId.Value;
            ddlTimesheetProject.Populate();

            chkAddTimesheet.Checked = true;
            ddlTimesheetProject.SelectedProjtaskId = ddlDefaultProject.SelectedProjtaskId;
        }
        catch (Exception ex)
        {
            DesktopShared.Bootstrap.Alert.DisplayMessage(litMessage, ex.ToString().Trim(), DesktopShared.Bootstrap.Alert.AlertType.Danger, false);
        }
    }

    #region private methods

    /// <summary>
    /// set up for initial page load
    /// </summary>
    private void SetUpPage()
    {
        txtExternalNotes.Focus();

        short _tabIndex = 0;
        chkInternal.TabIndex = ++_tabIndex;
        btnSubmit2.TabIndex = ++_tabIndex;
        ddlTicketDisposition.TabIndex = ++_tabIndex;
        ddlEmployeeAssignedTo.TabIndex = ++_tabIndex;
        txtExternalNotes.TabIndex = ++_tabIndex;
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

        //ddlClientContactRecipient.TabIndex = ++_tabIndex;
        lbClientContactRecipient.TabIndex = ++_tabIndex;
        btnAddRecipientClient.TabIndex = ++_tabIndex;
        //ddlEmployeeRecipient.TabIndex = ++_tabIndex;
        lbEmployeeRecipient.TabIndex = ++_tabIndex;
        btnAddRecipientBbb.TabIndex = ++_tabIndex;
        ddlMasterClientContactRecipient.TabIndex = ++_tabIndex;
        btnAddRecipientMasterClient.TabIndex = ++_tabIndex;

        ucScheduledDate.TabIndex = ++_tabIndex;
        ddlScheduledTime.HourTabIndex = ++_tabIndex;
        ddlScheduledTime.MinuteTabIndex = ++_tabIndex;
        ddlDurationTime.HourTabIndex = ++_tabIndex;
        ddlDurationTime.MinuteTabIndex = ++_tabIndex;

        lbTicketTag.TabIndex = ++_tabIndex;

        chkHistoryExcludeAlerts.TabIndex = ++_tabIndex;
        chkHistoryExcludeViews.TabIndex = ++_tabIndex;

        //rauMain.TabIndex = ++_tabIndex;
        fuOne.TabIndex = ++_tabIndex;
        litAllowedFileExtensions.Text = string.Join(",", DesktopShared.Utility.ValidFileUploadExtensions().ToArray());

        ddlTicketMergeTo.TabIndex = ++_tabIndex;
        btnMergeTo.TabIndex = ++_tabIndex;
        lbTicketMergeFrom.TabIndex = ++_tabIndex;
        btnMergeFrom.TabIndex = ++_tabIndex;

        btnSubmit3.TabIndex = ++_tabIndex;

        gridTicketHistory.ExcludeViews = true;
        chkHistoryExcludeViews.Checked = true;

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

        ddlEmployeeAssignedTo.DisplayChosenScript = _displayChosenScript;
        ddlEmployeeAssignedTo.CssClass = _cssClass;
        ddlEmployeeAssignedTo.SetSize = false;
        ddlTicketDisposition.DisplayChosenScript = _displayChosenScript;
        ddlTicketDisposition.CssClass = _cssClass;
        ddlTimesheetStartTime.DisplayChosenScript = _displayChosenScript;
        ddlTimesheetStartTime.CssClass = _cssClass;
        ddlTimesheetProject.DisplayChosenScript = _displayChosenScript;
        ddlTimesheetProject.CssClass = _cssClass;
        ddlTimesheetActivity.DisplayChosenScript = _displayChosenScript;
        ddlTimesheetActivity.CssClass = _cssClass;
        ddlTimesheetTime.DisplayChosenScript = _displayChosenScript;
        ddlTimesheetTime.CssClass = _cssClass;
        //ddlClientContactRecipient.DisplayChosenScript = _displayChosenScript;
        //ddlClientContactRecipient.CssClass = _cssClass;
        lbClientContactRecipient.DisplayChosenScript = _displayChosenScript;
        lbClientContactRecipient.CssClass = _cssClass;

        ddlTicketCategory.DisplayChosenScript = _displayChosenScript;
        ddlTicketCategory.CssClass = _cssClass;
        ddlTicketPriority.DisplayChosenScript = _displayChosenScript;
        ddlTicketPriority.CssClass = _cssClass;
        ddlScheduledTime.DisplayChosenScript = _displayChosenScript;
        ddlScheduledTime.CssClass = _cssClass;
        ddlDurationTime.DisplayChosenScript = _displayChosenScript;
        ddlDurationTime.CssClass = _cssClass;
        //ddlEmployeeRecipient.DisplayChosenScript = _displayChosenScript;
        //ddlEmployeeRecipient.CssClass = _cssClass;
        lbEmployeeRecipient.DisplayChosenScript = _displayChosenScript;
        lbEmployeeRecipient.CssClass = _cssClass;

        lbTicketTag.DisplayChosenScript = _displayChosenScript;
        lbTicketTag.CssClass = _cssClass;
        //ddlClient.DisplayChosenScript = _displayChosenScript;
        //ddlClient.CssClass = _cssClass;
        ddlEditClientContact.DisplayChosenScript = _displayChosenScript;
        ddlEditClientContact.CssClass = _cssClass;
        ddlEditClientContactAction.CssClass = _cssClass;
        ddlClientLocation.DisplayChosenScript = _displayChosenScript;
        ddlClientLocation.CssClass = _cssClass;
        ddlMasterClientContactRecipient.DisplayChosenScript = _displayChosenScript;
        ddlMasterClientContactRecipient.CssClass = _cssClass;
        ddlTicketMergeTo.DisplayChosenScript = _displayChosenScript;
        ddlTicketMergeTo.CssClass = _cssClass;
        lbTicketMergeFrom.DisplayChosenScript = _displayChosenScript;
        lbTicketMergeFrom.CssClass = _cssClass;

        if (_displayChosenScript)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append("<script type='text/javascript'>");
            sb.Append("$(document).ready(function() {");
            sb.Append(String.Format("SetChosenEditClientContactAction_{0}();", this.ClientID));
            sb.Append(String.Format("Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(SetChosenEditClientContactAction_{0})", this.ClientID));
            sb.Append("});");
            sb.Append(String.Format("function SetChosenEditClientContactAction_{0}(sender, args) {{", this.ClientID));
            sb.Append(String.Format("$('#{0}').chosen({{", ddlEditClientContactAction.ClientID));
            sb.Append("allow_single_deselect: true,");
            sb.Append("placeholder_text_single: \"Select an Action ...\",");
            sb.Append("width: \"100%\"");
            sb.Append("});");
            sb.Append("}");
            sb.Append("</script>");

            litEditClientContactActionJs.Visible = true;
            litEditClientContactActionJs.Text = sb.ToString();
        }
        else
            litEditClientContactActionJs.Visible = false;
    }

    /// <summary>
    /// get ticket id from query string
    /// </summary>
    /// <param name="objTicket></param>
    /// <returns></returns>
    private bool GetTicketId(ref DesktopShared.TypedListClasses.TicketRow objTicket)
    {
        int _id = -1;
        if (int.TryParse(BitByBit.Web.Request.GetString("Id").Trim(), out _id))
        {
            TicketId = _id;
            objTicket = DesktopShared.Ticket.GetTicketTypedListRow(_id);
            if (objTicket != null)
                return true;
        }

        pnlContainer.Visible = false;
        DesktopShared.Bootstrap.Alert.DisplayMessage(litMessage, String.Format("Unable to fetch Ticket. Id = {0}", _id > 0 ? _id.ToString() : "N/A"), DesktopShared.Bootstrap.Alert.AlertType.Danger, false);
        return false;
    }

    /// <summary>
    /// load ticket details
    /// </summary>
    /// <param name="objTicket"></param>
    /// <param name="insertViewAudit"></param>
    /// <param name="resetAllFields"></param>
    /// <param name="ticketMadeActive"></param>
    private void LoadValues(DesktopShared.TypedListClasses.TicketRow objTicket, bool insertViewAudit, bool resetAllFields, bool ticketMadeActive = false)
    {
        string _last = string.Empty;
        string notification = string.Empty;

        if (objTicket == null)
            return;
        ClientId = objTicket.FkClient;

        litMessage.Text = "";
        #region details

        litHeader.Text = String.Format("Ticket : {0} : {1}", objTicket.Pcscdefects, objTicket.DispositionName);
        litId.Text = objTicket.Pcscdefects.ToString();
        litStatus.Text = objTicket.StatusName.Trim();
        hlTimeSpent.NavigateUrl = String.Format("/Timesheet/Default.aspx?TicketId={0}", objTicket.Pcscdefects);
        hlTimeSpent.Text = DesktopShared.Timesheet.GetTimeSpentOnTicket(objTicket.Pcscdefects).ToString();
        hlLegacyDetail.NavigateUrl = String.Format("Detail.aspx?TicketId={0}", objTicket.Pcscdefects);
        litClient.Text = objTicket.ClientCompany.Trim();

        litContactPhone.Text = "";
        litContactCellPhone.Text = "";
        litContactHomePhone.Text = "";
        litContactEmail.Text = "";
        litContactFirst.Text = "";
        litContactLast.Text = "";

        string _reportedBy = objTicket.Reportedby.Trim();
        if (String.IsNullOrWhiteSpace(_reportedBy))
            _reportedBy = String.Format("{0} {1}", objTicket.EnteredByFirstName.Trim(), objTicket.EnteredByLastName.Trim());
        litReported.Text = String.Format("{0} {1}", _reportedBy, objTicket.DateEntered.ToString("MM/dd/yy HH:mm:ss"));

        litSales.Text = DesktopShared.Client.GetSalesForDisplay(ClientId.Value, "<br />", true);
        lblLeadTech.Text = DesktopShared.Client.GetLeadTech(ClientId.Value);
        lblTam.Text = DesktopShared.Client.GetTAMEmployee(ClientId.Value);
        lblCSM.Text = DesktopShared.Client.GetCSMEmployee(ClientId.Value);

        if (Convert.ToBoolean(objTicket.InternalOnly))
            phInternalOnly.Visible = true;

        chkInternal.Checked = objTicket.InternalOnly;
        ConfigureForInternal();
        litSummary.Text = DesktopShared.Utility.String.ReplaceLineBreaks(objTicket.Summary.Trim());
        txtSummary.Text = objTicket.Summary.Trim();
        txtDescriptionReadOnly.Text = Server.HtmlDecode(objTicket.Description.Trim());
        txtDescription.Text = Server.HtmlDecode(objTicket.Description.Trim());
        ddlTicketType.TicketTypeId = objTicket.TicketTypeId;
        
        ddlTicketCategory.TicketCategoryId = objTicket.TicketCategoryId;
        ddlTicketCategory.IsRequired = objTicket.TicketCategoryRequired;
        litTicketCategoryRequired.Visible = objTicket.TicketCategoryRequired;
        ddlTicketPriority.PriorityId = objTicket.FkPriority;
        ddlDefaultProject.SelectedProjtaskId = objTicket.DefaultProjectId;

        //location
        if (objTicket.ClientUseTicketLocation)
        {
            phLocation.Visible = true;
            litLocation.Text = String.IsNullOrWhiteSpace(objTicket.ClientLocationName) ? "[None]" : objTicket.ClientLocationName.Trim();
            ddlClientLocation.Active = true;
            ddlClientLocation.ClientIdForLocation = ClientId;
            ddlClientLocation.ClientLocationId = objTicket.ClientLocationId;
            ddlClientLocation.PopulateDropDownList();
        }
        else
            phLocation.Visible = false;

        #endregion

        #region response

        if (resetAllFields || ticketMadeActive)
        {
            if (!ticketMadeActive)
            {
                txtExternalNotes.Text = "";
                txtInternalNotes.Text = "";
            }
            int _dispositionId = objTicket.IsFkDispositionNull() ? -1 : objTicket.FkDisposition;
            AssignedToUserId = objTicket.IsAssignedtoNull() ? -1 : objTicket.Assignedto;
            ddlTicketDisposition.TicketDispositionId = _dispositionId > 0 ? _dispositionId : (int?)null;
            ddlEmployeeAssignedTo.EmployeeId = AssignedToUserId;
            btnMakeActive.Visible = (_dispositionId != (DesktopShared.SiteHelper.Ticket.Dispostion.Id.Active) || (AssignedToUserId != DesktopShared.User.UserID));
            OriginalAssignedToName = String.Format("{0} {1}", objTicket.AssignedToFirst.Trim(), objTicket.AssignedToLast.Trim());

            //timesheet
            chkAddTimesheet.Checked = true;
            ddlTimesheetProject.SelectedClientId = ClientId.Value;
            ddlTimesheetActivity.TaskActionId = DesktopShared.User.DefaultActivityId;
            ucTimesheetDate.SelectedDate = DateTime.Now;
            string _startTime = "0800";
            string _timeSpent = "0000";
            DesktopShared.Timesheet.GetStartTimeAndTimeSpent(ref _startTime, ref _timeSpent);
            ddlTimesheetTime.Hour = Convert.ToInt32(_timeSpent.Substring(0, 2));
            ddlTimesheetTime.Minute = Convert.ToInt32(_timeSpent.Substring(2, 2));
            TimesheetStartTime = _startTime;
            DesktopShared.EntityClasses.TimesheetEntity objTimeSheet = DesktopShared.Timesheet.GetLastForTicketEmployee(objTicket.Pcscdefects, DesktopShared.User.EmployeeID);

            var objEmployee = DesktopShared.Employee.GetEmployeeEntity(DesktopShared.User.EmployeeID);
            if ((objEmployee != null) && (objEmployee.Fields.State == EntityState.Fetched) && objEmployee.TicketTimesheetUseTime)
            {
                TimesheetUseStartTime = true;
                divTimesheetStartTime.Attributes.CssStyle.Add("display", "");
                ddlTimesheetStartTime.Hour = Convert.ToInt32(_startTime.Substring(0, 2));
                ddlTimesheetStartTime.Minute = Convert.ToInt32(_startTime.Substring(2, 2));
            }

            if ((objTimeSheet != null) && objTimeSheet.FkProjtask.HasValue)
                ddlTimesheetProject.SelectedProjtaskId = objTimeSheet.FkProjtask.Value;
        }

        #endregion

        #region scheduling

        if (resetAllFields)
        {
            if (objTicket.ScheduledDate != DateTime.MinValue)
            {
                ucScheduledDate.SelectedDate = objTicket.ScheduledDate;
                ddlScheduledTime.Hour = objTicket.ScheduledDate.Hour;
                ddlScheduledTime.Minute = objTicket.ScheduledDate.Minute;
                string _duration = DesktopShared.Utility.String.IntegerOnly(DesktopShared.Utility.Time.TimeDoubleToString(objTicket.ScheduledDuration)).Trim();
                if ((_duration.Length == 4) && _duration != "0000")
                {
                    ddlDurationTime.Hour = Convert.ToInt32(_duration.Substring(0, 2));
                    ddlDurationTime.Minute = Convert.ToInt32(_duration.Substring(2, 2));
                }
            }
            else
            {
                ddlDurationTime.Hour = 0;
                ddlDurationTime.Minute = 0;
            }
        }

        #endregion

        #region tags

        lbTicketTag.Populate(DesktopShared.Ticket.Tagged.GetSelectedTagTextForTicket(objTicket.Pcscdefects, DesktopShared.User.UserID));

        #endregion

        #region check lists tasks

        phCheckList.Visible = false;
        DesktopShared.CollectionClasses.TicketCheckListCollection _tclCollection = DesktopShared.Ticket.CheckList.Get(objTicket.Pcscdefects);
        if (_tclCollection.Count > 0)
        {
            phCheckList.Visible = true;
            rptCheckList.DataSource = _tclCollection;
            rptCheckList.DataBind();
        }

        #endregion

        #region client ids
        
        //ddlClientContactRecipient.ClientId = ClientId;
        //ddlClientContactRecipient.IsSilent = "on";
        //ddlClientContactRecipient.PopulateDropDownList();
        lbClientContactRecipient.ClientId = ClientId;
        lbClientContactRecipient.IsSilent = "on,not";
        lbClientContactRecipient.Populate();

        if (resetAllFields)
        {
            ddlEmployeeAssignedTo.HelpDeskClientId = ClientId.Value;
            ddlEmployeeAssignedTo.Populate();
        }
        ddlTicketMergeTo.ClientId = ClientId;
        ddlTicketMergeTo.ExcludeTicketId = objTicket.Pcscdefects;
        ddlTicketMergeTo.PopulateDropDownList();
        lbTicketMergeFrom.ClientId = ClientId;
        lbTicketMergeFrom.IncludePlaceHolderTickets = true;
        lbTicketMergeFrom.ExcludeTicketId = objTicket.Pcscdefects;
        lbTicketMergeFrom.Populate();

        #endregion

        #region client contact 

        ddlEditClientContact.ClientId = ClientId;
        ddlEditClientContact.ClientContactId = objTicket.FkUser;
        ddlEditClientContact.PopulateDropDownList();

        ddlEditClientLocation.ClientIdForLocation = ClientId;
        ddlEditClientLocation.PopulateDropDownList();
        DropDownList _ddl = ddlEditClientLocation.GetDropDownList();
        if (_ddl.Items.Count < 2)
            pnlLocation.Visible = false;
        else
            pnlLocation.Visible = true;


        litContactFirst.Text = objTicket.ReportedByFirst.Trim();
        litContactLast.Text = objTicket.ReportedByLast.Trim();
        litContactEmail.Text = objTicket.Email.Trim();
        litContactPhone.Text = objTicket.Phone.Trim();

        if (!objTicket.IsFkUserNull())
        {
            var objUser = new DesktopShared.EntityClasses.UsersEntity(objTicket.FkUser);
            if (objUser.Fields.State == EntityState.Fetched)
            {
                var objClientContact = new DesktopShared.EntityClasses.ClientContactEntity(objUser.FkClientContact.HasValue ? objUser.FkClientContact.Value : -1);
                if (objClientContact.Fields.State == EntityState.Fetched)
                {
                    string _first = objClientContact.First.Trim();
                    _last = objClientContact.Last.Trim();
                    string _email = objClientContact.Email.Trim();
                    string _phone = objClientContact.Busphone.Trim();
                    string _cell = objClientContact.Cellphone.Trim();
                    string _home = objClientContact.Homephone.Trim();
                    notification = objClientContact.NotificationPreferences.Trim();

                    if (!String.IsNullOrWhiteSpace(_first))
                        litContactFirst.Text = _first;
                    if (!String.IsNullOrWhiteSpace(_last))
                    {
                        litContactLast.Text = _last;
                        if (objClientContact.Vip)
                            litContactLast.Text += String.Format(" {0}", DesktopShared.User.VipIcon);
                    }
                    if (!String.IsNullOrWhiteSpace(_email))
                    {
                        litContactEmail.Text = String.Format("<br />e: {0}", _email);

                        string _emailToCopy = String.Format("{0}", _email);
                        litCopyEmail.Text = String.Format("<a class=\"copy-btn\" data-clipboard-text=\"{0}\" href=\"javascript:void(0);\" style=\"padding-left:5px;\" tooltip=\"Copy To Clipboard\"><i class=\"fa fa-clipboard\" title=\"Copy To Clipboard\"></i></a>", _emailToCopy);

                    }
                    if (!String.IsNullOrWhiteSpace(_phone))
                    {
                        litContactPhone.Text = String.Format("<br />w: {0}", _phone);

                        string _phoneToCopy = String.Format("{0}", _phone);
                        litCopyContactPhone.Text = String.Format("<a class=\"copy-btn\" data-clipboard-text=\"{0}\" href=\"javascript:void(0);\" style=\"padding-left:5px;\" tooltip=\"Copy To Clipboard\"><i class=\"fa fa-clipboard\" title=\"Copy To Clipboard\"></i></a>", _phoneToCopy);
                    }
                    if (!String.IsNullOrWhiteSpace(_cell))
                    {
                        litContactCellPhone.Text = String.Format("<br />c: {0}", _cell);
                        
                        string _CellPhoneToCopy = String.Format("{0}", _cell);
                        litCopyCellPhone.Text = String.Format("<a class=\"copy-btn\" data-clipboard-text=\"{0}\" href=\"javascript:void(0);\" style=\"padding-left:5px;\" tooltip=\"Copy To Clipboard\"><i class=\"fa fa-clipboard\" title=\"Copy To Clipboard\"></i></a>", _CellPhoneToCopy);
                    }
                    if (!String.IsNullOrWhiteSpace(_home))
                    {
                        litContactHomePhone.Text = String.Format("<br />h: {0}", _home);
                        
                        string _HomePhoneToCopy = String.Format("{0}", _home);
                        litCopyHomePhone.Text = String.Format("<a class=\"copy-btn\" data-clipboard-text=\"{0}\" href=\"javascript:void(0);\" style=\"padding-left:5px;\" tooltip=\"Copy To Clipboard\"><i class=\"fa fa-clipboard\" title=\"Copy To Clipboard\"></i></a>", _HomePhoneToCopy);
                    }
                    

                    

                    txtEditContactPhoneExtension.Text = objClientContact.Busext.Trim();
                    ltNotification.Text = objClientContact.NotificationPreferences == "off" ? string.Format("<b>({0})</b>", "Silenced") : string.Empty;
                }
            }
            ddlEditClientLocation.ClientLocationId = ClientLocID(objTicket.FkUser);
            ddlEditClientLocation.ClientIdForLocation = ClientId;
            ddlEditClientLocation.PopulateDropDownList();
        }

        if (objTicket.FkClient != null)
        {
            var objClient = new DesktopShared.EntityClasses.ClientEntity(objTicket.FkClient);
            if (objClient.Fields.State == EntityState.Fetched)
            {
                ltNotes.Text = objClient.ClientNotes;
            }
        }

        txtEditContactFirst.Text = litContactFirst.Text;
        txtEditContactLast.Text = _last;
        txtEditContactEmail.Text = litContactEmail.Text.Replace("<br />e: ", "");
        txtEditContactPhone.Text = litContactPhone.Text.Replace("<br />w: ", "");
        txtEditContactCellPhone.Text = litContactCellPhone.Text.Replace("<br />c: ", "");
        txtEditContactHomePhone.Text = litContactHomePhone.Text.Replace("<br />h: ", "");

        //add user extension to conact phone display if available
        if (!String.IsNullOrWhiteSpace(txtEditContactPhoneExtension.Text.Trim()) && !String.IsNullOrWhiteSpace(litContactPhone.Text.Trim()))
            litContactPhone.Text += String.Format(" x{0}", txtEditContactPhoneExtension.Text.Trim());

        #endregion

        #region notifications

        AddAssignedToRecipients(false);
        if(notification=="on")
            CheckClientContactList(DesktopShared.Client.Contact.GetTicketAutoCc(objTicket.FkClient), objTicket.Email.Trim(), String.Format("{0} {1}", litContactFirst.Text.Trim(), litContactLast.Text.Trim()));
        if (!objTicket.IsClientMasterClientIdNull() || objTicket.ClientIsMasterClient)
        {
            phMasterClientsNotification.Visible = true;
            MasterClientId = objTicket.IsClientMasterClientIdNull() ? ClientId : objTicket.ClientMasterClientId;
            ddlMasterClientContactRecipient.ClientId = MasterClientId;
            ddlMasterClientContactRecipient.PopulateDropDownList();
        }
        else
        {
            phMasterClientsNotification.Visible = false;
            MasterClientId = null;
        }
        #endregion

        #region ticket already merged      

        if (objTicket.Merged)
        {
            mvMergeTo.SetActiveView(viewMergeToComplete);
            hlMergeCompleteTo.Text = String.Format("Ticket merged to {0}", objTicket.MergedMasterTicketId);
            hlMergeCompleteTo.NavigateUrl = String.Format("Detail2.aspx?Id={0}", objTicket.MergedMasterTicketId);
        }

        #endregion

        if (GetDisplayOrder().ToString() == "True")
        {
            pnlDisplayOrder.Visible = true;
            txtPriorityOrder.Text = objTicket.PriorityOrder == 0 ? string.Empty : objTicket.PriorityOrder.ToString();
        }

        //copy to clipboard [Client - Subject - Assigned to John Doe - Link to Ticket]
        string _assignedTo = "";
        if (ddlEmployeeAssignedTo.EmployeeName.Trim().Length > 0)
            _assignedTo = String.Format("- Assigned to {0}", ddlEmployeeAssignedTo.EmployeeName.Trim());
        string _textToCopy = String.Format("{0} - {1}{2} - {3}", objTicket.ClientCompany.Trim(), Server.HtmlEncode(objTicket.Summary.Trim()), _assignedTo, HttpContext.Current.Request.Url.AbsoluteUri.Trim());
        litCopySummary.Text = String.Format("<a class=\"copy-btn\" data-clipboard-text=\"{0}\" href=\"javascript:void(0);\" style=\"padding-left:5px;\" tooltip=\"Copy To Clipboard\"><i class=\"fa fa-clipboard\" title=\"Copy To Clipboard\"></i></a>", _textToCopy);

        //rebind grids
        RebindGrids();

        //default notifications to checked
        if (resetAllFields)
        {
            gridTicketContactClient.CheckAll();
            gridTicketContactEmployee.CheckAll();
            gridTicketContactMasterClient.CheckAll();
        }

        //on hold
        if (DesktopShared.Client.Status.IsOnHold(objTicket.FkClient))
            DesktopShared.Bootstrap.Alert.DisplayMessage(litMessage, "Client Status = On Hold. Please Refer Client To Accounting.", DesktopShared.Bootstrap.Alert.AlertType.Danger, false, "ALERT");

        //audit
        if (insertViewAudit)
            DesktopShared.Ticket.History.AddView(objTicket.Pcscdefects, DesktopShared.User.UserID, DesktopShared.User.UserFullName.Trim(), true);
    }

    /// <summary>
    /// check if auto cc and client contact email are assigned to ticket notification.  if not, add
    /// </summary>
    /// <param name="autoCcEmails"></param>
    /// <param name="clientContactEmail"></param>
    /// <param name="clientContactFullName"></param>
    private void CheckClientContactList(List<string> autoCcEmails, string clientContactEmail, string clientContactFullName)
    {
        if (!ClientId.HasValue)
            return;
        DesktopShared.CollectionClasses.TicketNotificationCollection _notifications = null;
        IPredicateExpression _notificationsFilter = null;

        #region auto cc

        if (autoCcEmails.Count > 0)
        {
            foreach (string autoCcEmail in autoCcEmails)
            {
                //collection
                _notifications = new DesktopShared.CollectionClasses.TicketNotificationCollection();

                //predicate expression
                _notificationsFilter = new PredicateExpression();
                _notificationsFilter.Add(DesktopShared.HelperClasses.TicketNotificationFields.TicketId == TicketId.Value);
                _notificationsFilter.Add(DesktopShared.HelperClasses.TicketNotificationFields.ClientContactEmail == autoCcEmail.Trim());

                //fetch
                _notifications.GetMulti(_notificationsFilter, 1);
                if (_notifications.Count == 0)
                {
                    DesktopShared.EntityClasses.TicketNotificationEntity objTicketNotification = new DesktopShared.EntityClasses.TicketNotificationEntity();
                    objTicketNotification.TicketId = TicketId.Value;
                    objTicketNotification.AutoCc = true;
                    objTicketNotification.ClientContactEmail = autoCcEmail.Trim();
                    objTicketNotification.ClientContactName = "Client Auto CC";
                    objTicketNotification.Save();
                }
                else
                {
                    if (!_notifications[0].AutoCc)
                    {
                        _notifications[0].AutoCc = true;
                        _notifications[0].Save();
                    }
                }
            }
        }

        #endregion

        #region primary contact

        if (!String.IsNullOrWhiteSpace(clientContactEmail) && !String.IsNullOrWhiteSpace(clientContactFullName))
        {
            //collection
            _notifications = new DesktopShared.CollectionClasses.TicketNotificationCollection();

            //predicate expression
            _notificationsFilter = new PredicateExpression();
            _notificationsFilter.Add(DesktopShared.HelperClasses.TicketNotificationFields.TicketId == TicketId.Value);
            _notificationsFilter.Add(DesktopShared.HelperClasses.TicketNotificationFields.ClientContactEmail == clientContactEmail.Trim());

            //fetch
            _notifications.GetMulti(_notificationsFilter, 1);
            if (_notifications.Count == 0)
            {
                DesktopShared.EntityClasses.TicketNotificationEntity objTicketNotification = new DesktopShared.EntityClasses.TicketNotificationEntity();
                objTicketNotification.PrimaryContact = true;
                objTicketNotification.TicketId = TicketId.Value;
                objTicketNotification.ClientContactEmail = clientContactEmail.Trim();
                objTicketNotification.ClientContactName = clientContactFullName.Trim();
                objTicketNotification.Save();
            }
            else
            {
                if (!_notifications[0].PrimaryContact)
                {
                    _notifications[0].PrimaryContact = true;
                    _notifications[0].Save();
                }
            }
        }

        #endregion
    }

    /// <summary>
    /// rebind grids
    /// </summary>
    /// <param name="history"></param>
    /// <param name="file"></param>
    private void RebindGrids(bool history = true, bool file = true)
    {
        if (!TicketId.HasValue || !ClientId.HasValue)
            return;

        //client contacts
        gridTicketContactClient.TicketId = TicketId.Value;
        gridTicketContactClient.ClientId = ClientId.Value;
        gridTicketContactClient.RebindGrid();
        
        //employee contacts
        gridTicketContactEmployee.TicketId = TicketId.Value;
        gridTicketContactEmployee.ClientId = null;
        gridTicketContactEmployee.RebindGrid();

        //master client contacts
        if (phMasterClientsNotification.Visible && MasterClientId.HasValue)
        {
            gridTicketContactMasterClient.TicketId = TicketId.Value;
            gridTicketContactMasterClient.MasterClientId = MasterClientId.Value;
            gridTicketContactMasterClient.RebindGrid();
        }
        
        //remove existing contacts from ddl 
        UpdateContacts();

        //history
        if (history)
        {
            gridTicketHistory.TicketId = TicketId.Value;
            gridTicketHistory.RebindGrid();
        }

        //file
        if (file)
        {
            gridTicketFile.TicketId = TicketId.Value;
            gridTicketFile.RebindGrid();
        }

        gridActiveHistory.GetTicketNumber = TicketId.Value;
        gridActiveHistory.RebindGrid();


    }

    /// <summary>
    /// event handler for ticket file delete
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void gridTicketFile_evDeleteCompleted(object sender, EventArgs e)
    {
        DesktopShared.Bootstrap.Alert.DisplayMessage(litMessage, String.Format("File has been deleted - {0}.", DateTime.Now), DesktopShared.Bootstrap.Alert.AlertType.Success);
    }

    /// <summary>
    /// /// event handler for item command for file grid
    /// </summary>
    /// <param name="success"></param>
    /// <param name="message"></param>
    private void GridTicketFile_evItemCommand(bool success, string message)
    {
        if (String.IsNullOrWhiteSpace(message))
            return;

        DesktopShared.Bootstrap.Alert.DisplayMessage(litMessage, message, success ? DesktopShared.Bootstrap.Alert.AlertType.Success : DesktopShared.Bootstrap.Alert.AlertType.Danger);
    }

    /// <summary>
    /// event handler for item command for recipient / history /  tag grids
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Grid_evItemCommand(object sender, EventArgs e)
    {
        string _senderId = ((Control)sender).ID.Trim().Trim();
        bool _clientContacts = _senderId == "gridTicketContactClient";
        bool _masterClientContacts = _senderId == "gridTicketContactMasterClient"; 
        bool _employeeContacts = _senderId == "gridTicketContactEmployee";
        bool _history = _senderId == "gridTicketHistory";
        bool _file = _senderId == "gridTicketFile";

        if (_clientContacts)
            //ddlClientContactRecipient.PopulateDropDownList();
            lbClientContactRecipient.Populate();
        if (_employeeContacts)
            //ddlEmployeeRecipient.Populate();
            lbEmployeeRecipient.Populate();
        if (_masterClientContacts)
            ddlMasterClientContactRecipient.PopulateDropDownList();

        RebindGrids(!_history, !_file);
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
        List<DesktopShared.Ticket.Notification.Contact> _contacts = DesktopShared.Ticket.Notification.GetClientContacts(TicketId.Value, "");
        if (_contacts.Count > 0)
        {
            _lb = lbClientContactRecipient.GetListBox();
            if (_lb.Items.Count == 0)
            {
                lbClientContactRecipient.UseEmailAsDataValue = true;
                lbClientContactRecipient.Populate();
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
            //    _ddl = ddlClientContactRecipient.GetDropDownList();
            //    if (_ddl.Items.Count == 0)
            //    {
            //        ddlClientContactRecipient.UseEmailAsDataValue = true;
            //        ddlClientContactRecipient.PopulateDropDownList();
            //    }

            //    for (int i = 0; i < _ddl.Items.Count; i++)
            //    {
            //        _li = _ddl.Items[i];
            //        if ((_contacts.FirstOrDefault(x => x.Email.ToLower() == _li.Value.ToLower()) != null) || (_li.Value.Trim() == ""))
            //        {
            //            if (i != 0)
            //            {
            //                _ddl.Items.Remove(_li);
            //                i--;
            //            }
            //        }
            //    }
        }

            //employees
            _contacts = DesktopShared.Ticket.Notification.GetEmployees(TicketId.Value, "");
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
            _contacts = DesktopShared.Ticket.Notification.GetClientContacts(TicketId, "", true);
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
    /// edit contact on selected index changed
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ddlEditClientContact_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtEditContactFirst.Text = "";
        txtEditContactLast.Text = "";
        txtEditContactEmail.Text = "";
        txtEditContactPhone.Text = "";
        txtEditContactPhoneExtension.Text = "";
        txtEditContactCellPhone.Text = "";
        txtEditContactHomePhone.Text = "";

        if (ddlEditClientContact.ClientContactId.HasValue)
        {
            DesktopShared.EntityClasses.UsersEntity objUser = new DesktopShared.EntityClasses.UsersEntity(ddlEditClientContact.ClientContactId.Value);
            txtEditContactFirst.Text = objUser.First.Trim();
            txtEditContactLast.Text = objUser.Last.Trim();
            txtEditContactEmail.Text = objUser.Email.Trim();
            txtEditContactPhone.Text = objUser.Phone.Trim();
            var objClientContact = new DesktopShared.EntityClasses.ClientContactEntity(objUser.FkClientContact.HasValue ? objUser.FkClientContact.Value : -1);
            if (objClientContact.Fields.State == EntityState.Fetched)
            {
                txtEditContactCellPhone.Text = objClientContact.Cellphone.Trim();
                txtEditContactPhoneExtension.Text = objClientContact.Busext.Trim();
                txtEditContactHomePhone.Text = objClientContact.Homephone.Trim();
            }
        }

        CloseAndShowClientContact();
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
            if (!DesktopShared.Ticket.Tagged.TagExistsForTicket(TicketId.Value, DesktopShared.Ticket.Tagged.Opportunities, null))
            {
                DesktopShared.Ticket.Tagged.AddTagValue(TicketId.Value, DesktopShared.Ticket.Tagged.Opportunities, null, true, true);
                lbTicketTag.Populate(DesktopShared.Ticket.Tagged.GetSelectedTagTextForTicket(TicketId.Value, DesktopShared.User.UserID));
            }
        }
    }

    /// <summary>
    /// ticket check list master on selected index changed
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ddlTicketCheckListMaster_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (!ddlTicketCheckListMaster.TicketCheckListMasterId.HasValue)
        {
            litMessage.Text = "NONE";
            return;
        }

        DesktopShared.EntityClasses.TicketCheckListMasterEntity objTicketCheckListMaster = new DesktopShared.EntityClasses.TicketCheckListMasterEntity(ddlTicketCheckListMaster.TicketCheckListMasterId.Value);
        if (objTicketCheckListMaster.Fields.State == EntityState.Fetched)
            txtExternalNotes.Text += objTicketCheckListMaster.CheckListCode.Trim();
        ddlTicketCheckListMaster.TicketCheckListMasterId = null;
    }

    /// <summary>
    /// add employee assigned to list of recipiients
    /// </summary>
    /// <param name="rebindGrids"></param>
    private void AddAssignedToRecipients(bool rebindGrids = true)
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

        DesktopShared.Ticket.Notification.AddUser(TicketId.Value, "", ddlEmployeeAssignedTo.EmployeeId, ddlEmployeeAssignedTo.HelpDeskClientId == Desktop.SiteHelper.Client.Id.BBB);
        if (rebindGrids)
            RebindGrids();
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
    /// configure page for internal only status
    /// </summary>
    private void ConfigureForInternal()
    {
        bool _internal = chkInternal.Checked;

        litClientNotificationHeader.Text = _internal ? " - Internal Ticket. Client Notifications Disabled" : "";
        btnAddRecipientClient.Visible = !_internal;
        //ddlClientContactRecipient.Visible = !_internal;
        lbClientContactRecipient.Visible = !_internal;
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

    /// <summary>
    /// get formatted duration used for legacy database values
    /// </summary>
    /// <returns></returns>
    private string GetFormattedDuration()
    {
        return String.Format("{0}{1}", ddlDurationTime.Hour.ToString().PadLeft(2, '0'), ddlDurationTime.Minute.ToString().PadLeft(2, '0'));
    }

    private string GetDisplayOrder()
    {
        int _clientid = 0;
        try
        {
            if (ClientId != null)
                 _clientid = (int)ClientId;

            DesktopShared.EntityClasses.ClientEntity objClient = new DesktopShared.EntityClasses.ClientEntity(_clientid);
            if (objClient.Fields.State != EntityState.Fetched)
                return "";

            return String.Format("{0}", objClient.DisplayTicketOrder);
        }
        catch
        {
            return string.Empty;
        }
    }

    #endregion

    #region protected events

    protected void lbDocumentViewer_Click(object sender, EventArgs e)
    {
        Response.Redirect(string.Format("/Ticket/DocumentViewer.aspx?Id={0}", TicketId));
        //rapTicket.Redirect(string.Format("/Ticket/DocumentViewer.aspx?Id={0}", TicketId));
    }

    /// <summary>
    /// submit button on click
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        string _errorMessage = "";

        try
        {
            if (Page.IsValid)
            {
                DesktopShared.EntityClasses.CscDefectsEntity ticket = new DesktopShared.EntityClasses.CscDefectsEntity(TicketId.Value);
                int usersid = (int)ticket.Assignedto;
                int userclientcontact = (int)ticket.FkUser;
                if (userclientcontact == 0)
                    userclientcontact = 11363;
                DesktopShared.EntityClasses.UsersEntity pusers = new DesktopShared.EntityClasses.UsersEntity(usersid);
                DesktopShared.EntityClasses.UsersEntity pclientcontact = new DesktopShared.EntityClasses.UsersEntity(userclientcontact);
                int clientconttact = (int)pclientcontact.FkClientContact;
                DesktopShared.EntityClasses.ClientContactEntity cc = new DesktopShared.EntityClasses.ClientContactEntity(clientconttact);
                DesktopShared.TypedListClasses.TicketRow objTicket = DesktopShared.Ticket.GetTicketTypedListRow(TicketId.Value);

                //assigned to
                int _assignedTo = ddlEmployeeAssignedTo.EmployeeId;
                bool _assignedToChanged = (ticket.Assignedto != _assignedTo);
                ticket.Assignedto = _assignedTo;
                bool _addTimesheet = AddTimesheetEntry;
                #region status/disposition

                string _oldDisposition = "";
                int _originalDispositionId = ticket.FkDisposition.HasValue ? ticket.FkDisposition.Value : -1;
                DateTime? _dateForHistoryNote = null;
                int _dispositionId = ddlTicketDisposition.TicketDispositionId.Value;
                bool _dipositionChanged = false;
                bool _ticketWasClosed = false;

                if (_dispositionId > 0)
                {
                    //ticket was changed to active, all other active tickets need to be set to idle
                    if ((_dispositionId == DesktopShared.SiteHelper.Ticket.Dispostion.Id.Active) && (ticket.FkDisposition != DesktopShared.SiteHelper.Ticket.Dispostion.Id.Active))
                        DesktopShared.Ticket.MakeActive(ticket.Pcscdefects, DesktopShared.User.UserID, _assignedTo, false, "", ref _dateForHistoryNote);
                    else if ((_dispositionId != DesktopShared.SiteHelper.Ticket.Dispostion.Id.Active) && (ticket.FkDisposition == DesktopShared.SiteHelper.Ticket.Dispostion.Id.Active)) //was active, changed to alternate disposition
                        DesktopShared.Ticket.MakeInactive(ticket.Pcscdefects, DesktopShared.User.UserID, _assignedTo, _addTimesheet, ref _dateForHistoryNote);

                    //check if changed .. history note will be addded if changed
                    if ((_dispositionId != ticket.FkDisposition) && (ticket.FkDisposition.HasValue))
                    {
                        _dipositionChanged = true;
                        DesktopShared.EntityClasses.CscFieldsEntity objOldDispostion = new DesktopShared.EntityClasses.CscFieldsEntity(ticket.FkDisposition.Value);
                        if (objOldDispostion.Fields.State == EntityState.Fetched)
                            _oldDisposition = objOldDispostion.Name.Trim();

                        if (_dispositionId == DesktopShared.Ticket.Disposition.Id.Closed)
                            _ticketWasClosed = true;
                    }

                    //if status is active and assignment has changed and user making assignment change is assigning to other user .... set status to idle, run make inactive 
                    if ((_assignedToChanged) && (_dispositionId == DesktopShared.SiteHelper.Ticket.Dispostion.Id.Active) && (_assignedTo != DesktopShared.User.UserID))
                    {
                        DesktopShared.Ticket.MakeInactive(ticket.Pcscdefects, DesktopShared.User.UserID, DesktopShared.User.UserID, _addTimesheet, ref _dateForHistoryNote);
                        ticket.FkDisposition = DesktopShared.SiteHelper.Ticket.Dispostion.Id.Idle;
                    }
                    else
                        ticket.FkDisposition = _dispositionId;

                    if (_dipositionChanged)
                    {
                        int scheduledid = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["ScheduledId"]);
                        if (_dispositionId == scheduledid)
                        {
                            //if (!ticket.TicketScheduled.HasValue || ticket.TicketScheduled == false)
                            //{
                            //    DesktopShared.Bootstrap.Alert.DisplayMessage(litMessage, "Tickets must be scheduled", DesktopShared.Bootstrap.Alert.AlertType.Danger);
                            //    return;
                            //}
                            if (ucScheduledDate.SelectedDate.HasValue && (ddlScheduledTime.Hour != 0 || ddlScheduledTime.Minute != 0))
                                ticket.TicketScheduled = true;
                            else
                            {
                                DesktopShared.Bootstrap.Alert.DisplayMessage(litMessage, "Please fills the date and time in Schedulling section first", DesktopShared.Bootstrap.Alert.AlertType.Danger);
                                return;
                            }
                        }
                    }
                }
                else
                    ticket.FkDisposition = null;

                int ticketStatusID = _dispositionId == 93 ? 77 : 76;
                ticket.FkStatus = ticketStatusID;
                ticket.TicketTypeId = ddlTicketType.TicketTypeId;

                #endregion

                if (DesktopShared.ReportSettings.SendEmail(DesktopShared.ReportSettings.SurveyId()) == true)
                {
                    if (ticket.InternalOnly == false)
                    {
                        if (ddlTicketDisposition.TicketDispositionId.Value == 93)
                        {
                            string surveyid = string.Empty;

                            if (DesktopShared.ReportSettings.SurveyId(ref surveyid) == true)
                            {
                                string fqdn= BitByBit.Configuration.GetConfigString("DesktopFqdn");
                                string _emailFrom = BitByBit.Configuration.GetConfigString("TicketSurveyEmailFrom");
                                string _emailto = objTicket.Email.Trim();
                                string cname = objTicket.ReportedByFirst.ToString().Trim() + " " + objTicket.ReportedByLast.ToString().Trim();
                                string cfname = objTicket.ReportedByFirst.ToString().Trim();
                                string clname = objTicket.ReportedByLast.ToString().Trim();
                                string compname = objTicket.ClientCompany.ToString().Trim();
                                string url = "https://"+ fqdn + "/Ticket/Detail2.aspx?Id=" + TicketId.Value.ToString().Trim();
                                //"Ticket #" + TicketId.Value.ToString()
                                if(DesktopShared.Email.SendEmail2(_emailFrom, "Your Recent Bit By Bit Service Experience", _emailto, string.Empty, Desktop.Email.ClosedBodyEmail(objTicket.AssignedToEmail.Trim(), cname, TicketId.Value, surveyid, objTicket.Email.Trim(),ticket.Summary,cfname,clname,compname,url)))
                                    DesktopShared.Client.Contact.AddButtonHistory("Submit Closed Ticket", _emailto, clientconttact, DesktopShared.User.UserID, DesktopShared.User.UserFullName, null, ref _errorMessage);
                                else
                                    DesktopShared.Bootstrap.Alert.DisplayMessage(litMessage, "Error send survey email", DesktopShared.Bootstrap.Alert.AlertType.Danger);
                            }
                            else
                            {
                                DesktopShared.Bootstrap.Alert.DisplayMessage(litMessage, surveyid, DesktopShared.Bootstrap.Alert.AlertType.Danger);
                                return;
                            }
                        }
                    }
                }


                //duplicate entry check .... not sure how this is occuring as testing is not able to repro
                var _dtDuplicate = DesktopShared.StoredProcedureCallerClasses.RetrievalProcedures.TicketHistoryIsDuplicate(TicketId.Value, DesktopShared.User.UserID);
                if (BitByBit.Utility.DataBase.ConvertHelper.ConvertToBoolean(_dtDuplicate.Rows[0][0], false))
                {
                    string _dupeError = String.Format("Ticket Id:{0}<br />User Id:{1}<br />External Notes:{2}<br />Internal Notes:{3}", TicketId.Value, DesktopShared.User.UserID, txtExternalNotes.Text.Trim(), txtInternalNotes.Text.Trim());
                    DesktopShared.Email.SendEmailAlert(_dupeError, "Desktop Duplicate History Alert");
                    LoadValues(DesktopShared.Ticket.GetTicketTypedListRow(TicketId.Value), false, true);
                    return;
                }

                int _ticketHistoryTypeId = DesktopShared.Ticket.History.Type.Id.UpdateByEmployee;

               
                #region exchange calender update/delete appointment

                DateTime _scheduledDate = DateTime.Now;
                if (ucScheduledDate.SelectedDate.HasValue)
                {
                    _scheduledDate = new DateTime(
                        ucScheduledDate.SelectedDate.Value.Year, ucScheduledDate.SelectedDate.Value.Month, ucScheduledDate.SelectedDate.Value.Day,
                        ddlScheduledTime.Hour, ddlScheduledTime.Minute, 0);
                }
                bool _addCalendarAppointment = false;
                int _addCalendarAppointmentUserId = 0;

                bool _deleteCalendarAppointment = false;

                bool _updateCalendarAppointment = false;

                if (ticket.ScheduledDate.HasValue && !ucScheduledDate.SelectedDate.HasValue)
                {
                    //was scheduled, now removed -> delete all future appointments for ticket
                    _deleteCalendarAppointment = true;
                }
                else if (!ticket.ScheduledDate.HasValue && ucScheduledDate.SelectedDate.HasValue)
                {
                    //not scheduled originally, now has scheduled -> create appointment for assigned to user
                    _addCalendarAppointment = true;
                    _addCalendarAppointmentUserId = _assignedTo;
                }
                else if (ticket.ScheduledDate.HasValue)
                {
                    if ((ticket.Assignedto.HasValue) && (ticket.Assignedto.Value != _assignedTo))
                    {
                        //asignment changed -> 

                        //1. delete all future appointments for ticket
                        _deleteCalendarAppointment = true;

                        //2.  create new appointment for new assigned to employee
                        _addCalendarAppointment = true;
                        _addCalendarAppointmentUserId = _assignedTo;
                    }
                    else
                    {
                        if ((ticket.ScheduledDate != _scheduledDate) || (GetFormattedDuration()) != DesktopShared.Utility.String.IntegerOnly(DesktopShared.Utility.Time.TimeDoubleToString(ticket.ScheduledDuration)).Trim())
                        {
                            //scheduled date / time has been updated -> update all future appointments for ticket with new scheduled date
                            _updateCalendarAppointment = true;
                        }
                    }
                }

                #endregion

                //internal
                ticket.InternalOnly = chkInternal.Checked;
                ticket.DefaultProjectId = ddlDefaultProject.SelectedProjtaskId;
                #region  timesheet
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
                    _timeSheet.Memo = Server.HtmlEncode(txtExternalNotes.Text.Trim());
                    _timeSheet.InternalComments = Server.HtmlEncode(txtInternalNotes.Text.Trim());
                    _timeSheet.Workid = DesktopShared.User.GetUserNameOnly();
                    _timeSheet.FkEmployee = DesktopShared.User.EmployeeID;
                    _timeSheet.FkCscdefects = TicketId;
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
                #region scheduled

                //scheduled
                ticket.ScheduledDate = ucScheduledDate.SelectedDate.HasValue ? _scheduledDate : (DateTime?)null;

                //scheduled duration
                if (ucScheduledDate.SelectedDate.HasValue)
                    ticket.ScheduledDuration = DesktopShared.Utility.Time.TimeStringToDouble(GetFormattedDuration());
                else
                    ticket.ScheduledDuration = 0;

                #endregion

                

                #region awaiting response

                if ((ticket.Assignedto.Value != _assignedTo) && (_assignedTo != DesktopShared.User.UserID)) //assigning to different user
                    ticket.AwaitingResponseUserId = _assignedTo;
                else if (_assignedTo != DesktopShared.User.UserID) //user is making update to ticket assigned to other user
                    ticket.AwaitingResponseUserId = _assignedTo;
                else if (DesktopShared.User.UserID == _assignedTo)//user awaiting response is updating ticket
                {
                    ticket.AwaitingResponseUserId = null;
                    ticket.AwaitingResponseInternal = true;
                }

                #endregion

                #region external & internal notes

                int? _internalTicketHistoryTypeId = null;

                //external notes
                string _externalNotes = "";
                string _enteredExternal = txtExternalNotes.Text.Trim();
                if (_enteredExternal.Length > 0)
                {
                    _externalNotes = _enteredExternal;

                    if (_externalNotes.Length > 8000)
                        _externalNotes = _externalNotes.Substring(0, 8000);
                    string _parsedNotes = "";
                    if (DesktopShared.Ticket.CheckList.AddByText(ticket.Pcscdefects, _externalNotes, ref _parsedNotes, DesktopShared.User.UserID))
                        _externalNotes = _parsedNotes;
                }

                //internal notes
                string _internalNotes = "";
                string _enteredInternalNotes = txtInternalNotes.Text.Trim();
                if (_enteredInternalNotes.Length > 0)
                {
                    _internalNotes = _enteredInternalNotes.Trim();
                    if (_internalNotes.Length > 8000)
                        _internalNotes = _internalNotes.Substring(0, 8000);
                }

                #endregion
                #region get notification emails

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
                    if (cc.NotificationPreferences == "off")
                    {
                        _clientEmailsList.Remove(cc.Email.Trim());
                        if (_clientEmailsList.Count == 0)
                            _clientEmails = "";
                        else
                            _clientEmails = string.Join(",", _clientEmailsList);
                    }
                    else
                    {
                        if (cc.NotificationPreferences == "not" && _ticketWasClosed == true)
                        {
                            _clientEmailsList.Remove(cc.Email.Trim());
                            if (_clientEmailsList.Count == 0)
                                _clientEmails = "";
                            else
                                _clientEmails = string.Join(",", _clientEmailsList);
                        }
                        else
                            _clientEmails = string.Join(",", _clientEmailsList);
                    }
                }

                #endregion
                #region assigned to changed text 

                bool _assignedToOnlyUpdate = false;
                string _assignedToText = "";
                if (_assignedToChanged)
                {
                    if (!ucScheduledDate.SelectedDate.HasValue)
                        _assignedToText = String.Format("Assigned to {0} by {1}.", ddlEmployeeAssignedTo.EmployeeName.Trim(), DesktopShared.User.UserFullName.Trim());
                    else
                        _assignedToText = String.Format("Scheduled for {0} on {1} by {2}.", ddlEmployeeAssignedTo.EmployeeName.Trim(), _scheduledDate.ToString(), DesktopShared.User.UserFullName.Trim());

                    if ((_enteredExternal.Trim().Length == 0) && (_enteredInternalNotes.Trim().Length == 0))
                        _assignedToOnlyUpdate = true;

                    //if(_dipositionChanged == true)
                    //    _assignedToOnlyUpdate = true;
                }

                #endregion

                //save ticket
                string _notes = "";
                List<DesktopShared.Ticket.CheckList.Node> _checkListTasks = DesktopShared.Ticket.CheckList.ParseItems(txtDescription.Text.Trim(), ref _notes);
                bool _checkListAdded = _checkListTasks.Count > 0;
                if (!_checkListAdded)
                    _notes = txtDescription.Text.Trim();
                ticket.Description = Server.HtmlEncode(_notes);
                ticket.Lastupdated = DateTime.Now;
                ticket.TicketCategoryId = ddlTicketCategory.TicketCategoryId;
                ticket.FkPriority = ddlTicketPriority.PriorityId > 0 ? ddlTicketPriority.PriorityId : (int?)null;
                ticket.PriorityOrder = txtPriorityOrder.Text != string.Empty ? int.Parse(txtPriorityOrder.Text) : (int?)null;

                ticket.Save();

                StringBuilder sbtext = new StringBuilder();
                var prevDate = Convert.ToDateTime(ticket.Lastupdated);
                var today = Convert.ToDateTime(DateTime.Now.ToString());
                var diffOfDates = today - prevDate;
                #region history entries and email

                #region add history to ticket and send emails

                bool _externalMailSuccess = true;
                int? _externalMessageTrackingId = null;
                bool _internalMailSuccess = true;
                int? _internalMessageTrackingId = null;

                List<int> _filesToEmail = gridTicketFile.FileIdsToEmail;
                int _mainTicketHistoryId = -1;
                int _ticketHistoryId = -1;

                if (!_assignedToOnlyUpdate)
                {
                    DesktopShared.Ticket.History.Add(
                        TicketId.Value, //ticket id
                        DesktopShared.User.UserID, //user id, 
                        _externalNotes.Trim(), //notes
                        _internalNotes.Trim(), //internal notes
                        false, // is new ticket
                        _clientEmails, // client email addresses
                        _employeeEmails, // employee email addresses
                        true,  // send email out
                        _ticketHistoryTypeId,  // history type id
                        _dateForHistoryNote,  //date/time stamp to user for history note created
                        _filesToEmail, //ids for file to attach to email
                        "", //update by override
                        _internalTicketHistoryTypeId, //internal ticket history type id (override history type id)
                        ref _mainTicketHistoryId, //entity id
                        ref _externalMailSuccess, //email successfully sent to client
                        ref _externalMessageTrackingId, //message tracking id for email to client
                        ref _internalMailSuccess, //email successfully sent to employees
                        ref _internalMessageTrackingId, //message tracking id for email to employee
                        true //is responsive page
                        );
                }


                #endregion

                #region assignment changed updated

                if (_assignedToChanged)
                {
                    //only send email if assigned to is not user making update
                    string _assignedToEmails = "";
                    if (DesktopShared.User.UserID != _assignedTo)
                    {
                        DesktopShared.EntityClasses.UsersEntity _userEmployee = new DesktopShared.EntityClasses.UsersEntity(_assignedTo);
                        if (!String.IsNullOrEmpty(_userEmployee.Email))
                            _assignedToEmails = _userEmployee.Email.Trim();
                    }

                    DesktopShared.Ticket.History.Add(
                        TicketId.Value, //ticket id
                        DesktopShared.User.UserID, //user id, 
                        "", //notes
                        _assignedToText, //internal notes
                        false, // is new ticket
                        "", // client email addresses
                        _assignedToEmails, // employee email addresses
                        true,  // send email out
                        null,  // history type id
                        _dateForHistoryNote,  //date/time stamp to user for history note created
                        null, //ids for file to attach to email
                        "", //update by override
                        DesktopShared.Ticket.History.Type.Id.AssignmentChange, //internal ticket history type id (override history type id)
                        ref _mainTicketHistoryId, //entity id
                        true //is responsive page
                   );
                }

                #endregion

                #region add history if disposition has been changed
                if (_dipositionChanged)
                {
                    _externalNotes = String.Format("STATUS WAS: {0}{2}NEW STATUS: {1}", _oldDisposition.Trim(), ddlTicketDisposition.TicketDispositionName.Trim(), Environment.NewLine);
                    int _statusChangeHistoryId = -1;

                    DesktopShared.Ticket.History.Add(
                    ticket.Pcscdefects, //ticket id
                    DesktopShared.User.UserID, //user id, 
                    _externalNotes.Trim(), //notes
                    "", //internal notes
                    false, // is new ticket
                   "",//_clientEmails, //"", // client email addresses
                    "",//_employeeEmails,//"", // employee email addresses
                    false,//true,//false, // send email out
                    DesktopShared.Ticket.History.Type.Id.DispositionChange, //history type
                    _dateForHistoryNote, //date/time stamp to user for history note created
                    null, //ids for file to attach to email
                    "", //update by override
                    null, //internal ticket history type id (override history type id)
                    ref _statusChangeHistoryId, //entity id
                    true //is responsive page
                    );

                    if (_mainTicketHistoryId == -1)
                        _mainTicketHistoryId = _statusChangeHistoryId;
                }

                #endregion
                #region checklist tasks
                if (_checkListAdded)
                    DesktopShared.Ticket.CheckList.AddByList(TicketId.Value, _checkListTasks, DesktopShared.User.UserID);

                if (phCheckList.Visible)
                {
                    System.Text.StringBuilder sbCheckListItemsCompleted = new System.Text.StringBuilder();
                    List<int> _updatedIds = new List<int>();

                    foreach (RepeaterItem _repeaterItem in rptCheckList.Items)
                    {
                        CheckBox chkCheckListCompleted = (CheckBox)_repeaterItem.FindControl("chkCheckListCompleted");
                        if (chkCheckListCompleted.Enabled && chkCheckListCompleted.Checked)
                        {
                            Literal litCheckListId = (Literal)_repeaterItem.FindControl("litCheckListId");
                            int _ticketCheckListId = -1;
                            if (int.TryParse(litCheckListId.Text.Trim(), out _ticketCheckListId))
                            {
                                _updatedIds.Add(_ticketCheckListId);
                                if (sbCheckListItemsCompleted.Length > 0)
                                    sbCheckListItemsCompleted.Append(", ");
                                sbCheckListItemsCompleted.Append(chkCheckListCompleted.Text.Trim());
                            }
                        }
                    }

                    if (_updatedIds.Count > 0)
                    {
                        DesktopShared.Ticket.CheckList.SetAsCompleted(_updatedIds, DesktopShared.User.UserID);

                        DesktopShared.Ticket.History.Add(
                           ticket.Pcscdefects, //ticket id
                           DesktopShared.User.UserID, //user id, 
                           String.Format("The following Check List items were completed: {0}", sbCheckListItemsCompleted.ToString()), //notes
                           "",//internal notes
                           false, // is new ticket
                           "", // client email addresses
                           "", // employee email addresses
                           false, // send email out
                           DesktopShared.Ticket.History.Type.Id.UpdateByEmployee, //history type
                           _dateForHistoryNote, //date/time stamp to user for history note created
                           null, //ids for file to attach to email
                           "", //update by override
                           null, //internal ticket history type id (override history type id)
                           ref _ticketHistoryId, //entity id
                           true //is responsive page
                      );
                    }
                }
                #endregion

                #endregion

                #region tags

                List<DesktopShared.Ticket.Tagged.TicketTag> _selectedTags = new List<DesktopShared.Ticket.Tagged.TicketTag>();
                foreach (string _selected in lbTicketTag.SelectedValues)
                {
                    int _pos1 = _selected.IndexOf("|");
                    int _pos2 = _selected.LastIndexOf("|");

                    if ((_pos1 > 0) && (_pos2 > 0))
                        _selectedTags.Add(new DesktopShared.Ticket.Tagged.TicketTag { Name = _selected.Substring(0, _pos1).Trim(), Public = _selected.Substring(_pos1 + 1, 1) == "1", ClientPublic = _selected.Substring(_pos2 + 1, 1) == "1" });
                }
                DesktopShared.Ticket.Tagged.Set(_selectedTags, DesktopShared.User.UserID, TicketId.Value);

                #endregion

                #region calendar

                //delete future appointments
                if (_deleteCalendarAppointment)
                    DesktopShared.Exchange.Calendar.DeleteAllFuture(TicketId.Value, DesktopShared.User.UserID, false);

                //add calendar appointment for specified user id   
                bool _calendarError = false;
                if (_addCalendarAppointment)
                    _calendarError = !DesktopShared.Exchange.Calendar.CreateAppointment(TicketId.Value, _addCalendarAppointmentUserId, false, DesktopShared.User.UserID);

                //edit future appointments
                if (_updateCalendarAppointment)
                    DesktopShared.Exchange.Calendar.UpdateAllFuture(TicketId.Value, DesktopShared.User.UserID, false);

                #endregion
                //3b exam specific ticket -> update 3b exam via api
                if (ClientId.Value == DesktopShared.BbbExam.ClientId)
                {
                    if (!_assignedToOnlyUpdate)
                        DesktopShared.BbbExam.Api.Support.Detail.Create(DesktopShared.Ticket.History.GetTypedListRow(_mainTicketHistoryId), _originalDispositionId, ddlTicketDisposition.TicketDispositionId.Value, ref _errorMessage);
                }

                //delete ticket timer tag if assigned to user is updated ticket or timer was reset to null
                if (ticket.TimerInterval.HasValue && (DesktopShared.User.UserID == ddlEmployeeAssignedTo.EmployeeId))
                {
                    if (!String.IsNullOrWhiteSpace(_internalNotes) || !String.IsNullOrWhiteSpace(_externalNotes))
                        DesktopShared.Ticket.Tagged.DeleteTagValue(TicketId.Value, DesktopShared.Ticket.Tagged.TicketTimerTagText, null);
                }

                //if ticket closed, user has mananger, and checklist items open -> send alert to manager
                if (_ticketWasClosed)
                {
                    int? _managerId = DesktopShared.User.ManagerId;
                    if (_managerId.HasValue)
                    {
                        if (DesktopShared.Ticket.CheckList.HasOpenItems(ticket.Pcscdefects))
                            DesktopShared.Email.Ticket.SendClosedWithOpenChecklist(ticket.Pcscdefects, DesktopShared.User.UserID, _managerId.Value);
                    }
                }

                #region file upload

                _errorMessage = "";

                #region telerik

                /*
                foreach (UploadedFile _file in rauMain.UploadedFiles)
                {
                    try
                    {
                        #region upload to azure

                        string _fileName = "";
                        System.IO.MemoryStream _ms = new System.IO.MemoryStream();
                        _file.InputStream.CopyTo(_ms);
                        _ms.Seek(0, System.IO.SeekOrigin.Begin);

                        bool _uploaded = DesktopShared.AzureHelper.UploadBlob(DesktopShared.AzureHelper.ContainerName.ticket, TicketId.ToString(), _file.GetExtension(), _ms, ref _fileName, ref _errorMessage);

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
                        objHistory.ResponsivePage = true;
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
                            fuOne.PostedFile.InputStream.CopyTo(_ms);
                            _ms.Seek(0, System.IO.SeekOrigin.Begin);

                            bool _uploaded = DesktopShared.AzureHelper.UploadMultipleBlob(System.IO.Path.GetFileNameWithoutExtension(postedFile.FileName), DesktopShared.AzureHelper.ContainerName.ticket, TicketId.ToString(), _extension, _ms, ref _fileName, ref _errorMessage);

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
                            objHistory.ResponsivePage = true;
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


                sbtext.Append("</br>Ticket Tracking</br>");
                sbtext.Append(string.Format("Ticket #: {0}</br>", TicketId.Value));
                sbtext.Append(string.Format("Last Updated: {0}</br>", ticket.Lastupdated));
                sbtext.Append(string.Format("Time Difference: {0}</br>", diffOfDates.Seconds));
                sbtext.Append(string.Format("Comment: {0}</br>", txtExternalNotes.Text.Trim()));
                sbtext.Append(string.Format("Internal Notes: {0}</br>", txtInternalNotes.Text.Trim()));
                sbtext.Append(string.Format("Client Emailed List: {0}</br>", _clientEmails.Trim()));

                #region confirmation / reload
                phRetryCalendar.Visible = false;
                LoadValues(DesktopShared.Ticket.GetTicketTypedListRow(ticket.Pcscdefects), false, true);
                string checkTicket = string.Empty;



                if (_calendarError)
                {
                    DesktopShared.Bootstrap.Alert.DisplayMessage(litMessage, String.Format("Ticket has been updated. Unable to add to exchange calendar so no scheduling occured - {0} {1}", DateTime.Now, sbtext.ToString()), DesktopShared.Bootstrap.Alert.AlertType.Danger);
                    phRetryCalendar.Visible = true;
                }
                else if (String.IsNullOrWhiteSpace(_errorMessage))
                    DesktopShared.Bootstrap.Alert.DisplayMessage(litMessage, String.Format("Ticket Updated: Current Time - {0}.{1}", DateTime.Now, sbtext.ToString()), DesktopShared.Bootstrap.Alert.AlertType.Success);
                else
                    DesktopShared.Bootstrap.Alert.DisplayMessage(litMessage, String.Format("Ticket has been updated but there was an error with the file upload - {0}. Error: {1} {2}", DateTime.Now, _errorMessage, sbtext.ToString()), DesktopShared.Bootstrap.Alert.AlertType.Danger);

                //sendError(_clientEmails.ToString().Trim());
                #endregion

                #region autoclosed

                DateTime? _changeautocloseddate = null;
                DateTime? _firstnotifdate = null;
                DateTime? _secondnotifdate = null;
                DateTime? _autocloseddate = null;
                bool? _autoclosedstatus = null;
                string err = "";
                long? autologid = null;

                autologid = DesktopShared.AutoCloses.GetAutologid(TicketId.Value);
                int autocloseid = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["AutoCloseId"]);
                if (ddlTicketDisposition.TicketDispositionId == autocloseid)
                {
                    if (DesktopShared.AutoCloses.checkexistAutoclosed(TicketId.Value, ref _changeautocloseddate, ref _firstnotifdate, ref _secondnotifdate, ref _autocloseddate, ref _autoclosedstatus, ref err) > 0)
                    {
                        if(autologid != null)
                            DesktopShared.AutoCloses.UpdateAutoClosedLog(autologid);
                    }
                    else
                        DesktopShared.AutoCloses.AddAutoClosedLog(TicketId.Value);
                }
                else
                {
                    if (DesktopShared.AutoCloses.checkexistAutoclosed(TicketId.Value, ref _changeautocloseddate, ref _firstnotifdate, ref _secondnotifdate, ref _autocloseddate, ref _autoclosedstatus, ref err) > 0)
                    {
                        if (autologid != null)
                            DesktopShared.AutoCloses.UpdateAutoClosedLogStatus(autologid);
                    }
                }
                #endregion
            }
        }
        catch (Exception ex)
        {
            DesktopShared.Bootstrap.Alert.DisplayMessage(litMessage, ex.Message.Trim() , DesktopShared.Bootstrap.Alert.AlertType.Danger);
        }
    }
    protected void sendError(string clientemail)
    {
        try {
            DesktopShared.EntityClasses.CscDefectsEntity ticket = new DesktopShared.EntityClasses.CscDefectsEntity(TicketId.Value);

            DateTime _lastupdate = Convert.ToDateTime(ticket.Lastupdated);
            DateTime _now = DateTime.Now;
            //TimeSpan diff1 = _now.Subtract(_lastupdate); //var diff = DateTime.Now - _lastupdate;
            var prevDate = Convert.ToDateTime(ticket.Lastupdated);
            var today = DateTime.Now;
            var diffOfDates = today.Subtract(prevDate);
            
            StringBuilder sbtext = new StringBuilder();
            sbtext.Append("</br>Ticket Tracking</br>");
            sbtext.Append(string.Format("Ticket #: {0}</br>", TicketId.Value));
            sbtext.Append(string.Format("Current Time: {0}</br>", today));
            sbtext.Append(string.Format("Last Updated: {0}</br>", ticket.Lastupdated));
            sbtext.Append(string.Format("Time Difference: {0}</br>", diffOfDates.Seconds));
            sbtext.Append(string.Format("Comment: {0}</br>", txtExternalNotes.Text.Trim()));
            sbtext.Append(string.Format("Internal Notes: {0}</br>", txtInternalNotes.Text.Trim()));
            sbtext.Append(string.Format("Client Emailed List: {0}</br>", clientemail.Trim()));

            if (diffOfDates.Seconds > 10)
            {
                string mailTo = "sricky@bitxbit.com,rob@bitxbit.com,dxuereb@bitxbit.com";
                DesktopShared.Bootstrap.Alert.DisplayMessage(litMessage, string.Format("Ticket Notes may not have been updated. Please confirm and re-save {0}", sbtext.ToString()), DesktopShared.Bootstrap.Alert.AlertType.Danger);
                DesktopShared.Email.SendEmail2("noreply@bitxbit.com", "Ticket Update Error", mailTo, "", string.Format("Ticket Notes may not have been updated. Please confirm and re-save, Ticket # {0} {1}", TicketId.Value, sbtext.ToString()), "");
            }
        }
        catch(Exception ex)
        {
            DesktopShared.Bootstrap.Alert.DisplayMessage(litMessage, ex.Message.Trim(), DesktopShared.Bootstrap.Alert.AlertType.Danger);
        }
    }
    /// <summary>
    /// update calendar button on click
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnUpdateCalendar_Click(object sender, EventArgs e)
    {
        if (this.IsValid)
        {
            DesktopShared.EntityClasses.CscDefectsEntity ticket = new DesktopShared.EntityClasses.CscDefectsEntity(TicketId.Value);
            int _assignedTo = ddlEmployeeAssignedTo.EmployeeId;
            DateTime _scheduledDate = DateTime.Now;
            if (ucScheduledDate.SelectedDate.HasValue)
            {
                _scheduledDate = new DateTime(
                    ucScheduledDate.SelectedDate.Value.Year, ucScheduledDate.SelectedDate.Value.Month, ucScheduledDate.SelectedDate.Value.Day,
                    ddlScheduledTime.Hour, ddlScheduledTime.Minute, 0);
            }
          
            //update ticket
            ticket.ScheduledDate = ucScheduledDate.SelectedDate.HasValue ? _scheduledDate : (DateTime?)null;
            if (ucScheduledDate.SelectedDate.HasValue)
                ticket.ScheduledDuration = DesktopShared.Utility.Time.TimeStringToDouble(GetFormattedDuration());
            else
                ticket.ScheduledDuration = 0;

            ticket.TicketScheduled = true;
            ticket.Save();

            //delete future appointments
            DesktopShared.Exchange.Calendar.DeleteAllFuture(TicketId.Value, DesktopShared.User.UserID, false);

            //add calendar appointment
            bool _calendarError = false;
            if (ucScheduledDate.SelectedDate.HasValue)
                _calendarError = !DesktopShared.Exchange.Calendar.CreateAppointment(TicketId.Value, _assignedTo, false, DesktopShared.User.UserID);

            LoadValues(DesktopShared.Ticket.GetTicketTypedListRow(ticket.Pcscdefects), false, true);
            if (_calendarError)
            {
                DesktopShared.Bootstrap.Alert.DisplayMessage(litMessage, String.Format("Unable to add to exchange calendar so no scheduling occured - {0}", DateTime.Now), DesktopShared.Bootstrap.Alert.AlertType.Danger);
                phRetryCalendar.Visible = true;
            }
            else
            {
                phRetryCalendar.Visible = false;
                DesktopShared.Bootstrap.Alert.DisplayMessage(litMessage, String.Format("Exchange calendar has been updated - {0}.", DateTime.Now), DesktopShared.Bootstrap.Alert.AlertType.Success);

            }
        }
    }

    /// <summary>
    /// upload button on click
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnUpload_Click(object sender, EventArgs e)
    {
        if (this.IsValid)
        {
            string _errorMessage = "";
            var ticket = new DesktopShared.EntityClasses.CscDefectsEntity(TicketId.Value);
            int _fileCount = 0;

            #region asp.net
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

                        bool _uploaded = DesktopShared.AzureHelper.UploadMultipleBlob(System.IO.Path.GetFileNameWithoutExtension(postedFile.FileName),DesktopShared.AzureHelper.ContainerName.ticket, TicketId.ToString(), _extension, _ms, ref _fileName, ref _errorMessage);

                        if (_ms != null)
                            _ms.Dispose();

                        #endregion

                        if (!_uploaded)
                        {
                            DesktopShared.Bootstrap.Alert.DisplayMessage(litMessage, String.Format("An error has occured while uploading to Azure - {0}.", _errorMessage), DesktopShared.Bootstrap.Alert.AlertType.Danger);
                            return;
                        }

                        _fileCount++;

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
                        objHistory.ResponsivePage = true;
                        objHistory.Save();
                    }
                    catch (Exception ex)
                    {
                        _errorMessage = ex.ToString();
                    }
                }
            }
            #endregion

            #region telerik

            /*
            foreach (UploadedFile _file in rauMain.UploadedFiles)
            {
                try
                {
                    #region upload to azure

                    string _fileName = "";
                    System.IO.MemoryStream _ms = new System.IO.MemoryStream();
                    _file.InputStream.CopyTo(_ms);
                    _ms.Seek(0, System.IO.SeekOrigin.Begin);

                    bool _uploaded = DesktopShared.AzureHelper.UploadBlob(DesktopShared.AzureHelper.ContainerName.ticket, TicketId.ToString(), _file.GetExtension(), _ms, ref _fileName, ref _errorMessage);

                    if (_ms != null)
                        _ms.Dispose();

                    #endregion

                    if (!_uploaded)
                    {
                        DesktopShared.Bootstrap.Alert.DisplayMessage(litMessage, String.Format("An error has occured while uploading to Azure - {0}.", _errorMessage), DesktopShared.Bootstrap.Alert.AlertType.Danger);
                        return;
                    }

                    _fileCount++;

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
                    objHistory.ResponsivePage = true;
                    objHistory.Save();
                }
                catch (Exception ex)
                {
                    _errorMessage = ex.ToString();
                }
            }
            */

            #endregion

            LoadValues(DesktopShared.Ticket.GetTicketTypedListRow(ticket.Pcscdefects), false, false);
            DesktopShared.Bootstrap.Alert.DisplayMessage(litMessage, String.Format("{0} file{1} {2} been uploaded - {3}.", _fileCount, _fileCount > 1 ? "s" : "", _fileCount > 1 ? "have" : "has", DateTime.Now), DesktopShared.Bootstrap.Alert.AlertType.Success);
        }
    }

    /// <summary>
    /// save summary modal button click
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSaveSummary_Click(object sender, EventArgs e)
    {
        if (this.IsValid)
        {
            var objTicket = new DesktopShared.EntityClasses.CscDefectsEntity(TicketId.Value);
            if (objTicket.Fields.State == EntityState.Fetched)
            {
                string _summary = txtSummary.Text.Trim();
                string _oldSummary = objTicket.Summary.Trim();
                if (_oldSummary != _summary)
                {
                    objTicket.Summary = _summary;
                    objTicket.Save();

                    string _externalNotes = String.Format("SUMMARY WAS: {0}{2}NEW SUMMARY: {1}", _oldSummary.Trim(), txtSummary.Text.Trim(), Environment.NewLine);
                    int _ticketHistoryId = 0;

                    DesktopShared.Ticket.History.Add(
                    TicketId.Value, //ticket id
                    DesktopShared.User.UserID, //user id, 
                    _externalNotes.Trim(), //notes
                    "", //internal notes
                    false, // is new ticket
                    "", // client email addresses
                    "", // employee email addresses
                    false, // send email out
                    DesktopShared.Ticket.History.Type.Id.SummaryChange, //history type
                    null, //date/time stamp to user for history note created
                    null, //ids for file to attach to email
                    "", //update by override
                    null, //internal ticket history type id (override history type id)
                    ref _ticketHistoryId, //entity id
                    true //is responsive page
                    );


                }
                LoadValues(DesktopShared.Ticket.GetTicketTypedListRow(TicketId.Value), false, false);
            }

            DesktopShared.Bootstrap.Alert.DisplayMessage(litMessage, String.Format("Summary has been updated - {0}.", DateTime.Now),  DesktopShared.Bootstrap.Alert.AlertType.Success);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Close", "CloseEditSummary();", true);
        }
    }

    /// <description>
    /// save description modal button click
    /// </description>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSaveDescription_Click(object sender, EventArgs e)
    {
        if (this.IsValid)
        {
            var objTicket = new DesktopShared.EntityClasses.CscDefectsEntity(TicketId.Value);
            if (objTicket.Fields.State == EntityState.Fetched)
            {
                string _description = txtDescription.Text.Trim();
                string _oldDescription = objTicket.Description.Trim();
                if (_oldDescription != _description)
                {
                    objTicket.Description = _description;
                    objTicket.Save();

                    string _externalNotes = String.Format("DESCRIPTION WAS: {0}{2}NEW DESCRIPTION: {1}", _oldDescription.Trim(), txtDescription.Text.Trim(), Environment.NewLine);
                    int _ticketHistoryId = 0;

                    DesktopShared.Ticket.History.Add(
                    TicketId.Value, //ticket id
                    DesktopShared.User.UserID, //user id, 
                    _externalNotes.Trim(), //notes
                    "", //internal notes
                    false, // is new ticket
                    "", // client email addresses
                    "", // employee email addresses
                    false, // send email out
                    DesktopShared.Ticket.History.Type.Id.DescriptionChange, //history type
                    null, //date/time stamp to user for history note created
                    null, //ids for file to attach to email
                    "", //update by override
                    null, //internal ticket history type id (override history type id)
                    ref _ticketHistoryId, //entity id
                    true //is responsive page
                    );


                }
                LoadValues(DesktopShared.Ticket.GetTicketTypedListRow(TicketId.Value), false, false);
            }

            DesktopShared.Bootstrap.Alert.DisplayMessage(litMessage, String.Format("Description has been updated - {0}.", DateTime.Now), DesktopShared.Bootstrap.Alert.AlertType.Success);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Close", "CloseEditDescription();", true);
        }
    }

    /// <summary>
    /// merge to button on click
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnMergeTo_Click(object sender, EventArgs e)
    {
        if (this.IsValid)
        {
            DesktopShared.Ticket.Merge.CompleteMerge(TicketId.Value, ddlTicketMergeTo.TicketId.Value, DesktopShared.User.UserID);
            var objTicket = new DesktopShared.EntityClasses.CscDefectsEntity(TicketId.Value);
            LoadValues(DesktopShared.Ticket.GetTicketTypedListRow(TicketId.Value), false, false);
            DesktopShared.Bootstrap.Alert.DisplayMessage(litMessage, String.Format("Ticket has been merged - {0}.", DateTime.Now), DesktopShared.Bootstrap.Alert.AlertType.Success);
        }
    }

    /// <summary>
    /// merge from button on click
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnMergeFrom_Click(object sender, EventArgs e)
    {
        if (this.IsValid)
        {
            foreach (int _ticketIdToMerge in lbTicketMergeFrom.SelectedValues)
            {
                DesktopShared.Ticket.Merge.CompleteMerge(_ticketIdToMerge, TicketId.Value, DesktopShared.User.UserID);
            }

            var objTicket = new DesktopShared.EntityClasses.CscDefectsEntity(TicketId.Value);
            LoadValues(DesktopShared.Ticket.GetTicketTypedListRow(TicketId.Value), false, false);
            DesktopShared.Bootstrap.Alert.DisplayMessage(litMessage, String.Format("Selected Tickets have been merged - {0}.", DateTime.Now), DesktopShared.Bootstrap.Alert.AlertType.Success);
        }
    }

    /// <summary>
    /// save location
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSaveLocation_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            var objTicket = new DesktopShared.EntityClasses.CscDefectsEntity(TicketId.Value);
            if (objTicket.Fields.State == EntityState.Fetched)
            {
                string _location = ddlClientLocation.ClientLocationName.Trim();
                string _oldLocation = objTicket.ClientLocationId.HasValue ? objTicket.ClientLocation.Name.Trim() : "";
                if (_oldLocation != _location)
                {
                    objTicket.ClientLocationId = ddlClientLocation.ClientLocationId;
                    objTicket.Save();

                    if (String.IsNullOrWhiteSpace(_location))
                        _location = "[None]";
                    if (String.IsNullOrWhiteSpace(_oldLocation))
                        _oldLocation = "[None]";

                    string _externalNotes = String.Format("LOCATION WAS: {0}{2}NEW LOCATION: {1}", _oldLocation, _location, Environment.NewLine);
                    int _ticketHistoryId = 0;

                    DesktopShared.Ticket.History.Add(
                    TicketId.Value, //ticket id
                    DesktopShared.User.UserID, //user id, 
                    _externalNotes.Trim(), //notes
                    "", //internal notes
                    false, // is new ticket
                    "", // client email addresses
                    "", // employee email addresses
                    false, // send email out
                    DesktopShared.Ticket.History.Type.Id.LocationChange, //history type
                    null, //date/time stamp to user for history note created
                    null, //ids for file to attach to email
                    "", //update by override
                    null, //internal ticket history type id (override history type id)
                    ref _ticketHistoryId, //entity id
                    true //is responsive page
                    );


                }
                LoadValues(DesktopShared.Ticket.GetTicketTypedListRow(TicketId.Value), false, false);
            }

            DesktopShared.Bootstrap.Alert.DisplayMessage(litMessage, String.Format("Location has been updated - {0}.", DateTime.Now), DesktopShared.Bootstrap.Alert.AlertType.Success);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Close", "CloseEditLocation();", true);
        }
    }

    /// <summary>
    /// save client button on click
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSaveClient_Click(object sender, EventArgs e)
    {
        if (this.IsValid)
        {
            var objTicket = new DesktopShared.EntityClasses.CscDefectsEntity(TicketId.Value);
            if (objTicket.Fields.State == EntityState.Fetched)
            {
                DesktopShared.Ticket.Notification.Delete(TicketId.Value, true, false, false); //delete client contacts
                DesktopShared.Ticket.Notification.Delete(TicketId.Value, false, false, true); //delete master client contacts

                //objTicket.FkClient = ddlClient.ClientId.Value;
                objTicket.DefaultProjectId = null;
                objTicket.FkClient = ddlSmartClient.SelectedClientId;
                objTicket.Save();
                LoadValues(DesktopShared.Ticket.GetTicketTypedListRow(TicketId.Value), false, false);

                //ddlEmployeeAssignedTo.HelpDeskClientId = ddlClient.ClientId.Value;
                ddlEmployeeAssignedTo.HelpDeskClientId = ddlSmartClient.SelectedClientId;
                ddlEmployeeAssignedTo.Populate();
                ddlEmployeeAssignedTo.EmployeeId = AssignedToUserId;

                ddlTimesheetProject.SelectedClientId = ddlSmartClient.SelectedClientId;
                ddlTimesheetProject.Populate();

                ddlDefaultProject.SelectedClientId = ddlSmartClient.SelectedClientId;
                ddlDefaultProject.Populate();
            }

            if (String.IsNullOrWhiteSpace(litMessage.Text))
                DesktopShared.Bootstrap.Alert.DisplayMessage(litMessage, String.Format("Client has been updated - {0}.", DateTime.Now), DesktopShared.Bootstrap.Alert.AlertType.Success);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Close", "CloseEditClient();", true);
        }
    }

    /// <summary>
    /// save contact button on click
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSaveContact_Click(object sender, EventArgs e)
    {
        if (! checkClientLocationActive())
            ddlEditClientLocation.IsRequired = false;
        else
            ddlEditClientLocation.IsRequired = true;

        if (this.IsValid)
        {
            string _first = txtEditContactFirst.Text.Trim();
            string _last = txtEditContactLast.Text.Trim();
            string _email = txtEditContactEmail.Text.Trim();
            string _extension = txtEditContactPhoneExtension.Text.Trim();
            string _phone = txtEditContactPhone.Text.Trim();
            string _cell = txtEditContactCellPhone.Text.Trim();
            string _home = txtEditContactHomePhone.Text.Trim();
            int? _userId = null;
            string err = string.Empty;


            #region add or edit client contact entity

            if (!String.IsNullOrWhiteSpace(ddlEditClientContactAction.SelectedValue))
            {
                DesktopShared.EntityClasses.ClientContactEntity _clientContact = null;
                DesktopShared.CollectionClasses.UsersCollection _users = null;
                IPredicateExpression _usersFilter = null;
                bool _isNew = false;

                if (ddlEditClientContactAction.SelectedValue.Trim() == "A")//add
                {
                    if (!CheckEmail(_email, (int)ClientId, ref err))
                    {
                        DesktopShared.Bootstrap.Alert.DisplayMessage(litMessage, err.ToString().Trim(), DesktopShared.Bootstrap.Alert.AlertType.Warning);
                        return;
                    }

                    _clientContact = new DesktopShared.EntityClasses.ClientContactEntity();
                    _clientContact.Created = DateTime.Now.ToString();
                    _clientContact.FkClient = ClientId;
                    _isNew = true;

                }
                else //update
                {
                    if (!ddlEditClientContact.ClientContactId.HasValue)
                    {
                        DesktopShared.Bootstrap.Alert.DisplayMessage(litMessage, "Please choose the contact first", DesktopShared.Bootstrap.Alert.AlertType.Warning);
                        return;
                    }
                    _users = new DesktopShared.CollectionClasses.UsersCollection();
                    _usersFilter = new PredicateExpression();
                    _usersFilter.Add(DesktopShared.HelperClasses.UsersFields.Pusers == ddlEditClientContact.ClientContactId);
                    _users.GetMulti(_usersFilter);

                    _clientContact = new DesktopShared.EntityClasses.ClientContactEntity(_users[0].FkClientContact.Value);
                    _userId = _users[0].Pusers;
                }

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

                DesktopShared.CollectionClasses.UserClientLocationCollection _ucl = new DesktopShared.CollectionClasses.UserClientLocationCollection();
                IPredicateExpression _uclFilter = new PredicateExpression();
                _uclFilter.Add(DesktopShared.HelperClasses.UserClientLocationFields.UserId == _userId);
                _ucl.GetMulti(_uclFilter);

                if (ddlEditClientLocation.ClientLocationId.HasValue)
                {
                    if (_ucl.Count < 0)
                    {
                        DesktopShared.EntityClasses.UserClientLocationEntity _uclentity = new DesktopShared.EntityClasses.UserClientLocationEntity(_ucl[0].Id);
                        _uclentity.ClientLocationId = ddlEditClientLocation.ClientLocationId;
                        _uclentity.Save();
                    }
                    else
                    {
                        DesktopShared.EntityClasses.UserClientLocationEntity _uclentity = new DesktopShared.EntityClasses.UserClientLocationEntity();
                        _uclentity.UserId = _userId;
                        _uclentity.ClientLocationId = ddlEditClientLocation.ClientLocationId;
                        _uclentity.Save();
                    }
                }

                if (_isNew)
                {
                    _users = new DesktopShared.CollectionClasses.UsersCollection();
                    _usersFilter = new PredicateExpression();
                    _usersFilter.Add(DesktopShared.HelperClasses.UsersFields.FkClientContact == _clientContact.PclientContact);
                    _users.GetMulti(_usersFilter);

                    if (_users.Count > 0)
                    {
                        _userId = _users[0].Pusers;
                        var objUser = new DesktopShared.EntityClasses.UsersEntity(_userId.Value);
                        objUser.ClientPortalActive = true;
                        objUser.Save();
                    }
                }
            }

            #endregion

            var objTicket = new DesktopShared.EntityClasses.CscDefectsEntity(TicketId.Value);
            if (objTicket.Fields.State == EntityState.Fetched)
            {
                objTicket.Email = _email;
                objTicket.Phone = _phone;
                if (_userId.HasValue)
                    objTicket.FkUser = _userId.Value;
                objTicket.Save();

                //update primary contact notification
                DesktopShared.EntityClasses.TicketNotificationEntity objPrimaryTicketNotification = DesktopShared.Ticket.Notification.GetPrimary(TicketId.Value);
                string _originalClientContactEmail = "";
                string _originalClientContactName = "";
                if ((objPrimaryTicketNotification != null) && (objPrimaryTicketNotification.Fields.State == EntityState.Fetched))
                {
                    //delete new primary contact to notification list if already present
                    var objTicketNotif = GetTicketNotif(TicketId.Value, _email);
                    if (objTicketNotif.Count > 0)
                    {
                        for (int i = 0; i <= objTicketNotif.Count - 1; i++)
                        {
                            DeleteTicketNotif(objTicketNotif[i].Id);
                        }
                    }

                    //DesktopShared.Ticket.Notification.DeleteClientContact(TicketId.Value, _email); --> getting error During a save action an entity's update action failed. The entity which failed is enclosed

                    _originalClientContactEmail = objPrimaryTicketNotification.ClientContactEmail.Trim();
                    _originalClientContactName = objPrimaryTicketNotification.ClientContactName.Trim();

                    objPrimaryTicketNotification.ClientContactName = String.Format("{0} {1}", _first, _last);
                    objPrimaryTicketNotification.ClientContactEmail = _email;
                    objPrimaryTicketNotification.Save();
                }

                //add original primary contact to notification list if not already present
                DesktopShared.Ticket.Notification.AddClientContact(TicketId.Value, _originalClientContactEmail, _originalClientContactName);

                ddlEditClientContactAction.SelectedIndex = 0;
                LoadValues(DesktopShared.Ticket.GetTicketTypedListRow(TicketId.Value), false, false);
            }

            DesktopShared.Bootstrap.Alert.DisplayMessage(litMessage, String.Format("Contact has been updated - {0}.", DateTime.Now), DesktopShared.Bootstrap.Alert.AlertType.Success);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "btnSaveContactClose", "CloseEditContact();", true);
  
        }
    }

    #region additional
    private bool checkClientLocationActive()
    {
        try {
            DesktopShared.CollectionClasses.ClientCollection _client = new DesktopShared.CollectionClasses.ClientCollection();

            IPredicateExpression _orFilter = new PredicateExpression();

            _orFilter.AddWithAnd(DesktopShared.HelperClasses.ClientFields.Pclient == ClientId);

            //fetch
            _client.GetMulti(_orFilter, 0, null);

            if (_client.Count > 0)
                return _client[0].UseTicketLocation;
            else
                return false;
        }
        catch (Exception ex)
        {
            return false;
        }
    }

    public static bool CheckEmail(string email,int clientid, ref string err)
    {
        try
        {
            DesktopShared.CollectionClasses.ClientContactCollection cc = new DesktopShared.CollectionClasses.ClientContactCollection();

            IPredicateExpression _orFilter = new PredicateExpression();

            _orFilter.AddWithAnd(DesktopShared.HelperClasses.ClientContactFields.Email == email);
            _orFilter.AddWithAnd(DesktopShared.HelperClasses.ClientContactFields.FkClient == clientid);

            //fetch
            cc.GetMulti(_orFilter, 0, null);

            if (cc.Count == 0)
                return true;
            else
            {
                err = "Email " + email + " already exists for " + String.Format("{0} {1}", cc[0].First, cc[0].Last);
                return false;
            }
        }
        catch (Exception ex)
        {
            err = ex.Message.Trim();
            return false;
        }
    }
    public static DesktopShared.CollectionClasses.TicketNotificationCollection GetTicketNotif(int ticketid, string email)
    {
        DesktopShared.CollectionClasses.TicketNotificationCollection _ticketnotif = new DesktopShared.CollectionClasses.TicketNotificationCollection();

        IPredicateExpression _orFilter = new PredicateExpression();

        _orFilter.AddWithAnd(DesktopShared.HelperClasses.TicketNotificationFields.TicketId == ticketid);
        _orFilter.AddWithAnd(DesktopShared.HelperClasses.TicketNotificationFields.ClientContactEmail == email);

        //fetch
        _ticketnotif.GetMulti(_orFilter, 0, null);

        //return
        return _ticketnotif;
    }

    private int? ClientLocID(int userid)
    {
        DesktopShared.CollectionClasses.UserClientLocationCollection ClientLoc = new DesktopShared.CollectionClasses.UserClientLocationCollection();
        IPredicateExpression _orFilter = new PredicateExpression();

        _orFilter.AddWithAnd(DesktopShared.HelperClasses.UserClientLocationFields.UserId == userid);

        //fetch
        ClientLoc.GetMulti(_orFilter, 0, null);

        if (ClientLoc.Count > 0)
            return ClientLoc[0].ClientLocationId;
        else
            return null;
    }

    public static void DeleteTicketNotif(int pid)
    {
        try
        {
            DesktopShared.EntityClasses.TicketDesignationEntity ticketnotif = new DesktopShared.EntityClasses.TicketDesignationEntity(pid);
            ticketnotif.Delete();
            ticketnotif.Save();
        }
        catch
        {
            return;
        }
    }
    #endregion
    
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
            objTicketNotification.TicketId = TicketId.Value;
            objTicketNotification.ClientContactEmail = _email;
            objTicketNotification.ClientContactName = String.Format("{0} {1}", _first, _last);
            objTicketNotification.Save();

            //load / confirmation message
            LoadValues(DesktopShared.Ticket.GetTicketTypedListRow(TicketId.Value), false, false);

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
            objTicketNotification.TicketId = TicketId.Value;
            //objTicketNotification.EmployeeEmail = ddlEmployeeRecipient.SelectedValue.Trim();
            //objTicketNotification.EmployeeName = ddlEmployeeRecipient.EmployeeName.Trim();

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
        for (int i = 0; i <= lbClientContactRecipient.SelectedValues.Count - 1; i++)
        {
            DesktopShared.EntityClasses.TicketNotificationEntity objTicketNotification = new DesktopShared.EntityClasses.TicketNotificationEntity();
            objTicketNotification.TicketId = TicketId.Value;
            objTicketNotification.ClientContactEmail = lbClientContactRecipient.SelectedValues[i].Trim();
            objTicketNotification.ClientContactName = lbClientContactRecipient.SelectedText[i].Trim();
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
        objTicketNotification.TicketId = TicketId;
        objTicketNotification.MasterClientContactEmail = ddlMasterClientContactRecipient.SelectedValue.Trim();
        objTicketNotification.MasterClientContactName = ddlMasterClientContactRecipient.ClientContactName.Trim();
        objTicketNotification.Save();

        RebindGrids();
        ddlEmployeeAssignedTo.Focus();

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
            //user id (public tags are not assigned to user)
            int? userId = chkTagPublic.Checked ? (int?)null : DesktopShared.User.UserID;

            string _tagValue = txtNewTag.Text.Trim();

            //add tag
            DesktopShared.Ticket.Tagged.AddTagValue(TicketId.Value, _tagValue, userId, chkTagPublic.Checked, chkTagClientPublic.Checked);

            //clear selection
            txtNewTag.Text = "";
            chkTagPublic.Checked = false;
            chkTagClientPublic.Checked = false;

            RebindGrids();
            lbTicketTag.Populate(DesktopShared.Ticket.Tagged.GetSelectedTagTextForTicket(TicketId.Value, DesktopShared.User.UserID));
            ScriptManager.RegisterStartupScript(this, this.GetType(), "btnAddTagClose", "CloseAddTag();", true);
            lblEnd.Focus();
        }
        else
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowAddTag", "$('#modal-add-tag').modal('show');", true);
    }

    /// <summary>
    /// make ticket active button on click
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnMakeActive_Click(object sender, EventArgs e)
    {
        int _assignedTo = ddlEmployeeAssignedTo.EmployeeId;
        bool _assignmentChanged = (_assignedTo != DesktopShared.User.UserID);

        //make active for employee that clicked make active link
        DesktopShared.Ticket.MakeActive(TicketId.Value, DesktopShared.User.UserID, DesktopShared.User.UserID, true, "");

        //if ticket assigned to other employee, call inactive method for that employee
        if (_assignmentChanged)
            DesktopShared.Ticket.MakeInactive(TicketId.Value, DesktopShared.User.UserID, _assignedTo, false);

        LoadValues(DesktopShared.Ticket.GetTicketTypedListRow(TicketId.Value), false, false, true);
        DesktopShared.Bootstrap.Alert.DisplayMessage(litMessage, String.Format("Ticket has been activated - {0}.", DateTime.Now), DesktopShared.Bootstrap.Alert.AlertType.Success);
    }

    /// <summary>
    /// ticket check list repeater on item databound
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rptCheckList_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        DesktopShared.EntityClasses.TicketCheckListEntity objTicketCheckList = (DesktopShared.EntityClasses.TicketCheckListEntity)e.Item.DataItem;

        if (!String.Equals(_ticketCheckListHeader, objTicketCheckList.Header.Trim(), StringComparison.OrdinalIgnoreCase))
        {
            PlaceHolder phHeader = (PlaceHolder)e.Item.FindControl("phHeader");
            phHeader.Visible = true;

            Literal litCheckListHeader = (Literal)e.Item.FindControl("litCheckListHeader");
            litCheckListHeader.Text = objTicketCheckList.Header.Trim();
        }

        CheckBox chkCheckListCompleted = (CheckBox)e.Item.FindControl("chkCheckListCompleted");

        bool _completed = objTicketCheckList.Completed;
        chkCheckListCompleted.Checked = _completed;
        if (_completed)
            chkCheckListCompleted.Enabled = false;  

        string _name = objTicketCheckList.Task.Trim();
        if (_completed && objTicketCheckList.CompletedDate.HasValue)
            _name += String.Format(" <em>[Completed {0}]</em>", objTicketCheckList.CompletedDate.Value);
        chkCheckListCompleted.Text = _name;

        Literal litCheckListId = (Literal)e.Item.FindControl("litCheckListId");
        litCheckListId.Text = objTicketCheckList.Id.ToString();

        _ticketCheckListHeader = objTicketCheckList.Header.Trim();
    }

    /// <summary>
    /// history view checkbox on click
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void chkHistoryView_CheckedChanged(object sender, EventArgs e)
    {
        gridTicketHistory.ExcludeAlerts = chkHistoryExcludeAlerts.Checked;
        gridTicketHistory.ExcludeViews = chkHistoryExcludeViews.Checked;
        RebindGrids();
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
    /// upload files on server validate
    /// </summary>
    /// <param name="source"></param>
    /// <param name="args"></param>
    protected void cvFile_ServerValidate(object source, ServerValidateEventArgs args)
    {
        args.IsValid = true;

        #region telerik

        /*if (rauMain.UploadedFiles.Count == 0)
        {
            cvUpload.ErrorMessage = "Please select at least one file to upload.";
            args.IsValid = false;
            return;
        }

        foreach (UploadedFile _file in rauMain.UploadedFiles)
        {
            if (_file.ContentLength == 0)
            {
                cvUpload.ErrorMessage = "Invalid File.  Size must be greater than 0 bytes.";
                args.IsValid = false;
                return;
            }
        }
        */

        #endregion

        #region asp.net file upload

        if (fuOne.PostedFile != null && fuOne.PostedFile.ContentLength > 0)
        {
            cvFile.ErrorMessage = String.Format("Invalid File Upload. Allowed extensions: {0}", string.Join(",", DesktopShared.Utility.ValidFileUploadExtensions().ToArray()));
            args.IsValid = DesktopShared.Utility.ValidFileUploadExtensions().Contains(System.IO.Path.GetExtension(fuOne.PostedFile.FileName).Replace(".", "").ToLower());
        }
        else
        {
            cvFile.ErrorMessage = "File is required";
            args.IsValid = false;
        }

        #endregion
    }

    /// <summary>
    /// validate at least one 1 file has been upload
    /// </summary>
    /// <param name="server"></param>
    /// <param name="e"></param>
    protected void cvFile2_ServerValidate(object server, ServerValidateEventArgs args)
    {
        #region asp.net file upload

        if (fuOne.PostedFile != null && fuOne.PostedFile.ContentLength > 0)
        {
            cvFile2.ErrorMessage = String.Format("Invalid File Upload. Allowed extensions: {0}", string.Join(",", DesktopShared.Utility.ValidFileUploadExtensions().ToArray()));
            args.IsValid = DesktopShared.Utility.ValidFileUploadExtensions().Contains(System.IO.Path.GetExtension(fuOne.PostedFile.FileName).Replace(".", "").ToLower());
        }

        #endregion
    }

    /// <summary>
    /// tag validation
    /// </summary>
    /// <param name="source"></param>
    /// <param name="args"></param>
    protected void cvAddTag_ServerValidate(object source, ServerValidateEventArgs args)
    {
        if (String.IsNullOrWhiteSpace(txtNewTag.Text))
        {
            cvAddTag.ErrorMessage = "Tag is required";
            args.IsValid = false;
            return;
        }
        cvAddTag.ErrorMessage = "Tag already exists for ticket";

        //client public must also be public
        if (chkTagClientPublic.Checked && !chkTagPublic.Checked)
        {
            cvAddTag.ErrorMessage = "Client Public tags must also be flagged as Public";
            args.IsValid = false;
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
                return;
            }
        }

        
        //check tag is unique
        int? userId = chkTagPublic.Checked ? (int?)null : DesktopShared.User.UserID;
        args.IsValid = !DesktopShared.Ticket.Tagged.TagExistsForTicket(TicketId.Value, txtNewTag.Text.Trim(), userId);
    }

    /// <summary>
    /// validate email address for editing client contact
    /// </summary>
    /// <param name="source"></param>
    /// <param name="args"></param>
    protected void cvEditContactEmail_ServerValidate(object source, ServerValidateEventArgs args)
    {
        string _email = txtEditContactEmail.Text.Trim();
        if (!DesktopShared.Utility.Email.CheckCustomEmail(_email))
        {
            cvEditContactEmail.ErrorMessage = "Invalid Email Address";
            args.IsValid = false;
            CloseAndShowClientContact();
            return;
        }

        if ((ddlEditClientContactAction.SelectedValue.Trim() == "U") && !ddlEditClientContact.ClientContactId.HasValue)
        {
            cvEditContactEmail.ErrorMessage = "Contact is required for updating";
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
        if (!DesktopShared.Utility.Email.CheckCustomEmail(_email))
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
        if (!chkInternal.Checked)
        {
            if (String.IsNullOrWhiteSpace(txtExternalNotes.Text))
            {
                if (_sbTimesheetError.Length > 0)
                    _sbTimesheetError.Append(", ");
                _sbTimesheetError.Append("Client Viewable Notes");
            }
        }
        else
        {
            if (String.IsNullOrWhiteSpace(txtExternalNotes.Text) && String.IsNullOrWhiteSpace(txtInternalNotes.Text))
            {
                if (_sbTimesheetError.Length > 0)
                    _sbTimesheetError.Append(", ");
                _sbTimesheetError.Append("Client Viewable Notes or Internal Only Notes");
            }
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
        var validationControl = source as CustomValidator; 
        if (!ucScheduledDate.SelectedDate.HasValue)
        {
            validationControl.ErrorMessage = "Scheduled Date is required";
            args.IsValid = false;
            return;
        }

        if ((ddlScheduledTime.Hour == 0) && (ddlScheduledTime.Minute == 0))
        {
            validationControl.ErrorMessage = "Scheduled Time is required";
            args.IsValid = false;
            return;
        }

        if ((ddlDurationTime.Hour == 0) && (ddlDurationTime.Minute == 0))
        {
            validationControl.ErrorMessage = "Scheduled Duration is required";
            args.IsValid = false;
            return;
        }
    }

    protected void cvProrityOrder_ServerValidate(object source, ServerValidateEventArgs args)
    {
        int parsedValue;
        //if (pnlDisplayOrder.Visible == true)
        //{
        //    if (txtPriorityOrder.Text == string.Empty)
        //    {
        //        cvProrityOrder.ErrorMessage = "Priority Order is required";
        //        args.IsValid = false;
        //        return;
        //    }
        //    else
        //        cvProrityOrder.ErrorMessage = string.Empty;
        //}
        if (txtPriorityOrder.Text != string.Empty)
        {
            if (!int.TryParse(txtPriorityOrder.Text, out parsedValue))
            {
                cvProrityOrder.ErrorMessage = "Priority Order Only Numbers";
                args.IsValid = false;
                return;
            }
            else
                cvProrityOrder.ErrorMessage = string.Empty;
        }
        args.IsValid = true;
    }

    #endregion

    #endregion

    #region private properties

    /// <summary>
    /// get/set ticket id
    /// </summary>
    private int? TicketId
    {
        get
        {
            object obj = this.ViewState["tid_td"];
            return (obj == null) ? (int?)null : (int)obj;
        }
        set { this.ViewState["tid_td"] = value; }
    }

    /// <summary>
    /// get/set client id
    /// </summary>
    private int? ClientId
    {
        get
        {
            object obj = this.ViewState["cid_td"];
            return (obj == null) ? (int?)null : (int)obj;
        }
        set { this.ViewState["cid_td"] = value; }
    }

    /// <summary>
    /// get/set master client id
    /// </summary>
    private int? MasterClientId
    {
        get
        {
            object obj = this.ViewState["mcid_td"];
            return (obj == null) ? (int?)null : (int)obj;
        }
        set { this.ViewState["mcid_td"] = value; }
    }

    /// <summary>
    /// get/set original assigned to anme
    /// </summary>
    private string OriginalAssignedToName
    {
        get
        {
            object obj = this.ViewState["oatn_td"];
            return (obj == null) ? "" : (string)obj;
        }
        set { this.ViewState["oatn_td"] = value; }
    }

    /// <summary>
    /// get/set assigned to user id
    /// </summary>
    private int AssignedToUserId
    {
        get
        {
            object obj = this.ViewState["atuid_td"];
            return (obj == null) ? -1 : (int)obj;
        }
        set { this.ViewState["atuid_td"] = value; }
    }

    /// <summary>
    /// get/set timesheet start time
    /// </summary>
    private string TimesheetStartTime
    {
        get
        {
            object obj = this.ViewState["tst_td"];
            return (obj == null) ? "" : (string)obj;
        }
        set { this.ViewState["tst_td"] = value; }
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
            object obj = this.ViewState["tust_td"];
            return (obj == null) ? false : (bool)obj;
        }
        set { this.ViewState["tust_td"] = value; }
    }

    #endregion

}
