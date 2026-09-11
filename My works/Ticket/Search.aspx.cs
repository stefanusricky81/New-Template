using SD.LLBLGen.Pro.ORMSupportClasses;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class Ticket_Search : BasePage
{
    //prod not updated

    /// <summary>
    /// page load
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        #region event handlers

        DropDownList _ddl = ddlTicketViewUser.GetDropDownList();
        _ddl.AutoPostBack = true;
        _ddl.SelectedIndexChanged += ddlTicketViewUser_SelectedIndexChanged;

        _ddl = ddlTicketViewPublic.GetDropDownList();
        _ddl.AutoPostBack = true;
        _ddl.SelectedIndexChanged += ddlTicketViewPublic_SelectedIndexChanged;

        //DropDownList _ddlClient = ddlClient.GetDropDownList();
        //_ddlClient.AutoPostBack = true;
        //_ddlClient.SelectedIndexChanged += ddlClient_SelectedIndexChanged;

        Telerik.Web.UI.RadComboBox rcbClient = (Telerik.Web.UI.RadComboBox)ddlSmartClient.FindControl("rcbClient");
        if (rcbClient != null)
        {
            rcbClient.AutoPostBack = true;
            rcbClient.SelectedIndexChanged += new RadComboBoxSelectedIndexChangedEventHandler(ddlClient_SelectedIndexChanged);
        }

        gridTicket.evAwaitingResponseCleared += gridTicket_evAwaitingResponseCleared;
        gridTicket.evTicketsMerged += gridTicket_evTicketsMerged;
        gridTicket.evClosedTicket += gridTicket_evClosedTicket;
        #endregion

        ConfigureForDevice();
        if (!IsPostBack)
        {
            int _id, _alertid, _updateby;
           
            bool _view;

            
            if (Int32.TryParse(Request.QueryString["ClientId"], out _id))
                ddlSmartClient.SelectedClientId = _id;
            if (Int32.TryParse(Request.QueryString["type"], out _alertid))
                ddlTicketType.TicketTypeId = _alertid;
            
            //if (DateTime.TryParse(Request.QueryString["uuStartDate"], out _lastupdatestart))
            //    ucLastUpdatedStart.SelectedDate = _lastupdatestart;
            
            if (Boolean.TryParse(Request.QueryString["views"], out _view))
            {
                cbExcludeViews.Checked = _view;
                ddlTicketStatus.Status = DesktopShared.Ticket.Status.All;
            }
            SetUpPage();
        }
    }

    #region private methods

    private void ddlClient_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlClientContact.ClientId = ddlSmartClient.SelectedClientId;// ddlClient.ClientId;
        ddlClientContact.PopulateDropDownList();

        ddlReportedBy.ClientId = ddlSmartClient.SelectedClientId;
        ddlReportedBy.PopulateDropDownList();

        ddlClientLocation.DisplayDefaultValue = true;
        ddlClientLocation.ClientIdForLocation = ddlSmartClient.SelectedClientId;
        ddlClientLocation.PopulateDropDownList();
    }

    /// <summary>
    /// user ticket view on selected index changed
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ddlTicketViewUser_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlTicketViewUser.TicketViewId.HasValue)
            SetDataFromView(ddlTicketViewUser.TicketViewId.Value, true);
        else
            ClearForm();
    }

    /// <summary>
    /// public ticket view on selected index changed
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ddlTicketViewPublic_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlTicketViewPublic.TicketViewId.HasValue)
            SetDataFromView(ddlTicketViewPublic.TicketViewId.Value, false);
        else
            ClearForm();
    }

    /// <summary>
    /// clear all search criteria and hide grid
    /// </summary>
    private void ClearForm()
    {
        DesktopShared.User.Ticket.Search.MyViewId = null;
        DesktopShared.User.Ticket.Search.PublicViewId = null;
        DesktopShared.User.Ticket.Search.TicketNumber = null;
        DesktopShared.User.Ticket.Search.TicketDescription = "";
        DesktopShared.User.Ticket.Search.ClientId = null;
        DesktopShared.User.Ticket.Search.AssignedTo = null;
        DesktopShared.User.Ticket.Search.CreatedByUserId = null;
        DesktopShared.User.Ticket.Search.UpdatedByUserUserId = null;
        DesktopShared.User.Ticket.Search.TicketStatus = DesktopShared.Ticket.Status.Open;
        DesktopShared.User.Ticket.Search.DispositionId = null;
        DesktopShared.User.Ticket.Search.PriorityId = null;
        DesktopShared.User.Ticket.Search.TagName = "";
        DesktopShared.User.Ticket.Search.TeamId = null;
        DesktopShared.User.Ticket.Search.TicketTypeId = null;
        DesktopShared.User.Ticket.Search.CreatedStartDate = DateTime.MinValue;
        DesktopShared.User.Ticket.Search.CreatedEndDate = DateTime.MinValue;
        DesktopShared.User.Ticket.Search.UpdatedStartDate = DateTime.MinValue;
        DesktopShared.User.Ticket.Search.UpdatedEndDate = DateTime.MinValue;
        DesktopShared.User.Ticket.Search.CreatedLastDays = null;
        DesktopShared.User.Ticket.Search.UpdatedLastDays = null;
        DesktopShared.User.Ticket.Search.SalesEmployeeId = null;
        DesktopShared.User.Ticket.Search.AwaitingResponse = false;
        

        Response.Redirect("Search.aspx");
    }


    /// <summary>
    /// fill out fields from values stored in TicketView table
    /// </summary>
    /// <param name="ticketViewID"></param>
    /// <param name="isUserView"></param>
    private void SetDataFromView(int ticketViewID, bool isUserView)
    {
        bool _view;
        DesktopShared.EntityClasses.TicketViewEntity _ticketView = new DesktopShared.EntityClasses.TicketViewEntity(ticketViewID);

        if (_ticketView.Fields.State == EntityState.Fetched)
        {
            
            DesktopShared.Ticket.Status _status = DesktopShared.Ticket.Status.Open;
            if (_ticketView.TicketStatus.Trim().Length > 0)
            {
                try { _status = (DesktopShared.Ticket.Status)Enum.Parse(typeof(DesktopShared.Ticket.Status), _ticketView.TicketStatus.Trim(), true); }
                catch { _status = DesktopShared.Ticket.Status.Open; }
            }

            #region user view

            if (isUserView)
            {
                //default to save  
                chkSaveView.Checked = true;

                //public view
                chkIsPublicView.Checked = _ticketView.Public;

                //selected view id
                ddlTicketViewUser.TicketViewId = ticketViewID;

                //clear selected view id of public view DDL
                ddlTicketViewPublic.TicketViewId = null;

                //save id to cookie
                DesktopShared.User.Ticket.Search.MyViewId = _ticketView.Id;

                //remove public view id
                DesktopShared.User.Ticket.Search.PublicViewId = null;

                //display delete button
                phDeleteView.Visible = true;
            }

            #endregion

            #region public view

            else
            {
                //cannot save
                chkSaveView.Checked = false;

                //cannot save
                chkIsPublicView.Checked = false;

                //selected view id
                ddlTicketViewPublic.TicketViewId = ticketViewID;

                //clear selected view id of user view DDL
                ddlTicketViewUser.TicketViewId = null;

                //save id to cookie
                DesktopShared.User.Ticket.Search.PublicViewId = _ticketView.Id;

                //remove user view id
                DesktopShared.User.Ticket.Search.MyViewId = null;

                //hide user view delete button
                phDeleteView.Visible = false;

            }

            #endregion

            txtTicketNumber.Text = _ticketView.TicketId.HasValue ? _ticketView.TicketId.Value.ToString().Trim() : "";
            txtDescription.Text = _ticketView.TicketDescription.Trim();
            //ddlClient.ClientId = _ticketView.ClientId;
            ddlSmartClient.SelectedClientId = _ticketView.ClientId.HasValue ? (int)_ticketView.ClientId : -1;
            ddlEmployeeAssignedTo.EmployeeId = _ticketView.AssignedTo.HasValue ? _ticketView.AssignedTo.Value : -1;
            ddlEmployeeCreatedBy.EmployeeId = _ticketView.CreatedByUserId.HasValue ? _ticketView.CreatedByUserId.Value : -1;
            ddlEmployeeUpdatedBy.EmployeeId = _ticketView.UpdatedByUserUserId.HasValue ? _ticketView.UpdatedByUserUserId.Value : -1;
            ddlTicketStatus.Status = _status;
            ddlTicketDisposition.TicketDispositionId= _ticketView.DispositionId;
            ddlTicketPriority.PriorityId = _ticketView.PriorityId.HasValue ? _ticketView.PriorityId.Value : -1;
            if (!String.IsNullOrWhiteSpace(_ticketView.TagName.Trim()))
            {
                List<string> _selectedValues = new List<string>();
                List<string> result = _ticketView.TagName.Trim().Split(new char[] { ',' }).ToList();
                foreach (string _selected in result)
                {
                    int _pos1 = _selected.IndexOf("|");
                    int _pos2 = _selected.LastIndexOf("|");

                    string _publicClient = "";

                    if (_selected.Substring(_pos1 + 1, 1) == "1" && _selected.Substring(_pos2 + 1, 1) == "1")
                        _publicClient = " (PC)";
                    else if (_selected.Substring(_pos1 + 1, 1) == "1" && _selected.Substring(_pos2 + 1, 1) == "0")
                        _publicClient = " (P)";
                    else if (_selected.Substring(_pos1 + 1, 1) == "0" && _selected.Substring(_pos2 + 1, 1) == "1")
                        _publicClient = " (C)";

                    if ((_pos1 > 0) && (_pos2 > 0))
                        _selectedValues.Add(String.Format("{0}{1}", _selected.Substring(0, _pos1).Trim(), _publicClient));
                }
                ddlTicketTag.Populate(_selectedValues);
                //for (int i = 0; i < result.Count - 1; i++)
                //{
                //    //ddlTicketTag.SelectedValues = result[i].Trim();
                //}
                //ddlTicketTag.Populate(result);
                //foreach (var objTicketTag in _ticketView.TagName.Trim())
                //{ }
                //    ddlTicketTag.SelectedValues = _ticketView.TagName.Trim();
                //    ddlTicketTag.PopulateDropDownList();
            }
            ddlTeam.TeamId= _ticketView.TeamId;
            ddlTicketType.TicketTypeId= _ticketView.TicketTypeId;
            ucCreatedStart.SelectedDate= _ticketView.CreatedStartDate;
            ucCreatedEnd.SelectedDate = _ticketView.CreatedEndDate;
            ucLastUpdatedStart.SelectedDate = _ticketView.UpdatedStartDate ;
            ucLastUpdatedEnd.SelectedDate= _ticketView.UpdatedEndDate;
            txtCreatedLastDays.Text = _ticketView.CreatedLastDays.HasValue ? _ticketView.CreatedLastDays.Value.ToString() : "";
            txtUpdatedLastDays.Text = _ticketView.UpdatedLastDays.HasValue ? _ticketView.UpdatedLastDays.Value.ToString() : "";
            ddlEmployeeSales.EmployeeId = _ticketView.SalesEmployeeeId.HasValue ? _ticketView.SalesEmployeeeId.Value : -1;
            chkAwaitingResponse.Checked = _ticketView.AwaitingResponse;

            string TimeSpent = DesktopShared.Utility.Time.TimeDoubleToString((double)_ticketView.TimeSpent);
            if (_ticketView.TimeSpent.HasValue)
            {
                ddlTimeSpent.Hour = Convert.ToInt32(TimeSpent.Substring(0, 2));
                ddlTimeSpent.Minute = Convert.ToInt32(TimeSpent.Substring(2, 2));
            }
            RebindGrid();           
        }
        else
            ClearForm();
    }

    /// <summary>
    /// grid awating response click event handler
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void gridTicket_evAwaitingResponseCleared(object sender, EventArgs e)
    {
        DesktopShared.Bootstrap.Alert.DisplayMessage(litMessage, "Awating Response flag as been cleared.", DesktopShared.Bootstrap.Alert.AlertType.Success);
    }

    /// <summary>
    /// grid ticket merged click event handler
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void gridTicket_evTicketsMerged(object sender, EventArgs e)
    {
        DesktopShared.Bootstrap.Alert.DisplayMessage(litMessage, "Ticket Merge is complete.", DesktopShared.Bootstrap.Alert.AlertType.Success);
    }
    
    private void gridTicket_evClosedTicket(object sender, EventArgs e)
    {
        DesktopShared.Bootstrap.Alert.DisplayMessage(litMessage, "Ticket Closed is complete.", DesktopShared.Bootstrap.Alert.AlertType.Success);
    }

    /// <summary>
    /// set up for initial page load
    /// </summary>
    private void SetUpPage()
    {
        DateTime _lastupdatestart, _lastupdateend;
        int _updateby;
        txtTicketNumber.Focus();

        #region tab index

        short _tabIndex = 0;

        ddlTicketViewUser.TabIndex = ++_tabIndex;
        ddlTicketViewPublic.TabIndex = ++_tabIndex;
        chkSaveView.TabIndex = ++_tabIndex;
        chkIsPublicView.TabIndex = ++_tabIndex;
        btnDeleteView.TabIndex = ++_tabIndex;

        txtTicketNumber.TabIndex = ++_tabIndex;
        txtDescription.TabIndex = ++_tabIndex;
        //ddlClient.TabIndex = ++_tabIndex;
        ddlSmartClient.TabIndex = ++_tabIndex;
        ddlEmployeeAssignedTo.TabIndex = ++_tabIndex;
        ddlEmployeeCreatedBy.TabIndex = ++_tabIndex;
        ddlTicketStatus.TabIndex = ++_tabIndex;
        ddlTicketDisposition.TabIndex = ++_tabIndex;
        ddlTicketPriority.TabIndex = ++_tabIndex;
        ddlTicketTag.TabIndex = ++_tabIndex;
        ddlTeam.TabIndex = ++_tabIndex;
        ddlTicketType.TabIndex = ++_tabIndex;
        ucCreatedStart.TabIndex = ++_tabIndex;
        ucCreatedEnd.TabIndex = ++_tabIndex;
        ucLastUpdatedStart.TabIndex = ++_tabIndex;
        ucLastUpdatedEnd.TabIndex = ++_tabIndex;
        ddlEmployeeUpdatedBy.TabIndex = ++_tabIndex;
        txtCreatedLastDays.TabIndex = ++_tabIndex;
        txtUpdatedLastDays.TabIndex = ++_tabIndex;
        ddlEmployeeSales.TabIndex = ++_tabIndex;
        chkAwaitingResponse.TabIndex = ++_tabIndex;

        btnSubmit.TabIndex = ++_tabIndex;
        btnClear.TabIndex = ++_tabIndex;
        cbExcludeViews.TabIndex = ++_tabIndex;
        #endregion

        #region ticket id in query string

        string _ticketIdRequest = BitByBit.Web.Request.GetString("TicketId");
        if (!String.IsNullOrWhiteSpace(_ticketIdRequest))
        {
            int _tempTicketId = 0;
            if (int.TryParse(_ticketIdRequest, out _tempTicketId))
            {
                DesktopShared.User.Ticket.Search.TicketNumber = _tempTicketId;
                txtTicketNumber.Text = _tempTicketId.ToString();
                ddlTicketStatus.Status = DesktopShared.Ticket.Status.All;
                RebindGrid();
                return;
            }
        }

        #endregion

        #region client id in query string

        string _clientIdRequest = BitByBit.Web.Request.GetString("ClientId");
        if (!String.IsNullOrWhiteSpace(_clientIdRequest))
        {
            int _tempClientId = 0;
            if (int.TryParse(_clientIdRequest, out _tempClientId))
            {
                DesktopShared.User.Ticket.Search.ClientId= _tempClientId;
                //ddlClient.ClientId = _tempClientId;
                ddlSmartClient.SelectedClientId = _tempClientId;
                ddlTicketStatus.Status = DesktopShared.Ticket.Status.Open;
                RebindGrid();
                return;
            }
        }

        #endregion

        #region my view or public view id is stored in cookie, use those values and skip rest of function

        if (DesktopShared.User.Ticket.Search.MyViewId.HasValue && DesktopShared.User.Ticket.Search.MyViewId.Value > 0)
        {
            SetDataFromView(DesktopShared.User.Ticket.Search.MyViewId.Value, true);
            return;
        }

        if (DesktopShared.User.Ticket.Search.PublicViewId.HasValue && DesktopShared.User.Ticket.Search.PublicViewId.Value > 0)
        {
            SetDataFromView(DesktopShared.User.Ticket.Search.PublicViewId.Value, false);
            return;

        }

        #endregion

        #region saved values in cookie 

        bool hasSearchCriteria = false;

        //ticket #
        if (DesktopShared.User.Ticket.Search.TicketNumber.HasValue)
        {
            txtTicketNumber.Text = DesktopShared.User.Ticket.Search.TicketNumber.Value.ToString();
            hasSearchCriteria = true;
        }

        //description
        string _description = DesktopShared.User.Ticket.Search.TicketDescription.Trim();
        if (_description.Length > 0)
        {
            txtDescription.Text = _description;
            hasSearchCriteria = true;
        }

        //client id
        if (DesktopShared.User.Ticket.Search.ClientId.HasValue)
        {
            //ddlClient.ClientId = DesktopShared.User.Ticket.Search.ClientId.Value;
            ddlSmartClient.SelectedClientId = DesktopShared.User.Ticket.Search.ClientId.Value;
            hasSearchCriteria = true;
        }

        //assigned to
        if (DesktopShared.User.Ticket.Search.AssignedTo.HasValue)
        {
            ddlEmployeeAssignedTo.EmployeeId = DesktopShared.User.Ticket.Search.AssignedTo.Value;
            hasSearchCriteria = true;
        }

        //created by
        if (DesktopShared.User.Ticket.Search.CreatedByUserId.HasValue)
        {
            ddlEmployeeCreatedBy.EmployeeId = DesktopShared.User.Ticket.Search.CreatedByUserId.Value;
            hasSearchCriteria = true;
        }
        //updated by
        if (Int32.TryParse(Request.QueryString["uuUserId"], out _updateby))
        {
            ddlEmployeeUpdatedBy.EmployeeId = _updateby;
            hasSearchCriteria = true;
        }
        else
        {
            if (DesktopShared.User.Ticket.Search.UpdatedByUserUserId.HasValue)
            {
                ddlEmployeeUpdatedBy.EmployeeId = DesktopShared.User.Ticket.Search.UpdatedByUserUserId.Value;
                hasSearchCriteria = true;
            }
        }
        //status

        DesktopShared.Ticket.Status _ticketStatus = DesktopShared.User.Ticket.Search.TicketStatus;
        if (Int32.TryParse(Request.QueryString["uuUserId"], out _updateby))
            ddlTicketStatus.Status = DesktopShared.Ticket.Status.All;
        else
            ddlTicketStatus.Status = _ticketStatus;

        //disposition
        if (DesktopShared.User.Ticket.Search.DispositionId.HasValue)
        {
            ddlTicketDisposition.TicketDispositionId = DesktopShared.User.Ticket.Search.DispositionId.Value;
            hasSearchCriteria = true;
        }

        //priority
        if (DesktopShared.User.Ticket.Search.PriorityId.HasValue)
        {
            ddlTicketPriority.PriorityId = DesktopShared.User.Ticket.Search.PriorityId.Value;
            hasSearchCriteria = true;
        }

        //tag
        if (!String.IsNullOrWhiteSpace(DesktopShared.User.Ticket.Search.TagName))
        {
        //    ddlTicketTag.TagName = DesktopShared.User.Ticket.Search.TagName.Trim();
        //    ddlTicketTag.PopulateDropDownList();
            hasSearchCriteria = true;
        }

        //team
        if (DesktopShared.User.Ticket.Search.TeamId.HasValue)
        {
            ddlTeam.TeamId = DesktopShared.User.Ticket.Search.TeamId;
            hasSearchCriteria = true;
        }

        //type
        if (DesktopShared.User.Ticket.Search.TicketTypeId.HasValue)
        {
            ddlTicketType.TicketTypeId = DesktopShared.User.Ticket.Search.TicketTypeId;
            hasSearchCriteria = true;
        }

        //created start
        if (DesktopShared.User.Ticket.Search.CreatedStartDate != DateTime.MinValue)
        {
            ucCreatedStart.SelectedDate = DesktopShared.User.Ticket.Search.CreatedStartDate;
            hasSearchCriteria = true;
        }

        //created end
        if (DesktopShared.User.Ticket.Search.CreatedEndDate != DateTime.MinValue)
        {
            ucCreatedEnd.SelectedDate = DesktopShared.User.Ticket.Search.CreatedEndDate;
            hasSearchCriteria = true;
        }

        //updated start
        if (DateTime.TryParse(Request.QueryString["uuStartDate"], out _lastupdatestart))
        {
            ucLastUpdatedStart.SelectedDate = _lastupdatestart;
            hasSearchCriteria = true;
        }
        else
        {
            if (DesktopShared.User.Ticket.Search.UpdatedStartDate != DateTime.MinValue)
            {
                ucLastUpdatedStart.SelectedDate = DesktopShared.User.Ticket.Search.UpdatedStartDate;
                hasSearchCriteria = true;
            }
        }
        //updated end
        if (DateTime.TryParse(Request.QueryString["uuEndDate"], out _lastupdateend))
        {
            ucLastUpdatedEnd.SelectedDate = _lastupdateend;
            hasSearchCriteria = true;
        }
        else
        {
            if (DesktopShared.User.Ticket.Search.UpdatedEndDate != DateTime.MinValue)
            {
                ucLastUpdatedEnd.SelectedDate = DesktopShared.User.Ticket.Search.UpdatedEndDate;
                hasSearchCriteria = true;
            }
        }

        //created last days
        if (DesktopShared.User.Ticket.Search.CreatedLastDays.HasValue)
        {
            txtCreatedLastDays.Text = DesktopShared.User.Ticket.Search.CreatedLastDays.Value.ToString();
            hasSearchCriteria = true;
        }

        //updated last days
        if (DesktopShared.User.Ticket.Search.UpdatedLastDays.HasValue)
        {
            txtUpdatedLastDays.Text = DesktopShared.User.Ticket.Search.UpdatedLastDays.Value.ToString();
            hasSearchCriteria = true;
        }

        //sales
        if (DesktopShared.User.Ticket.Search.SalesEmployeeId.HasValue)
        {
            ddlEmployeeSales.EmployeeId = DesktopShared.User.Ticket.Search.SalesEmployeeId.Value;
            hasSearchCriteria = true;
        }

        //awaiting response
        if (DesktopShared.User.Ticket.Search.AwaitingResponse)
        {
            chkAwaitingResponse.Checked = true;
            hasSearchCriteria = true;
        }

        if (hasSearchCriteria)
            RebindGrid();

        #endregion
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
            phScrollNotes.Visible = true;
            _displayChosenScript = false;
            _cssClass = "form-control";
        }

        ddlTicketViewUser.DisplayChosenScript = _displayChosenScript;
        ddlTicketViewUser.CssClass = _cssClass;
        ddlTicketViewPublic.DisplayChosenScript = _displayChosenScript;
        ddlTicketViewPublic.CssClass = _cssClass;

        //ddlClient.DisplayChosenScript = _displayChosenScript;
        //ddlClient.CssClass = _cssClass;
        ddlEmployeeAssignedTo.DisplayChosenScript = _displayChosenScript;
        ddlEmployeeAssignedTo.CssClass = _cssClass;
        ddlEmployeeCreatedBy.DisplayChosenScript = _displayChosenScript;
        ddlEmployeeCreatedBy.CssClass = _cssClass;
        ddlEmployeeUpdatedBy.DisplayChosenScript = _displayChosenScript;
        ddlEmployeeUpdatedBy.CssClass = _cssClass;
        ddlTicketStatus.DisplayChosenScript = _displayChosenScript;
        ddlTicketStatus.CssClass = _cssClass;
        ddlTicketDisposition.DisplayChosenScript = _displayChosenScript;
        ddlTicketDisposition.CssClass = _cssClass;
        ddlTicketPriority.DisplayChosenScript = _displayChosenScript;
        ddlTicketPriority.CssClass = _cssClass;
        ddlTicketTag.DisplayChosenScript = _displayChosenScript;
        ddlTicketTag.CssClass = _cssClass;
        ddlTeam.DisplayChosenScript = _displayChosenScript;
        ddlTeam.CssClass = _cssClass;
        ddlTicketType.DisplayChosenScript = _displayChosenScript;
        ddlTicketType.CssClass = _cssClass;
        ddlEmployeeSales.DisplayChosenScript = _displayChosenScript;
        ddlEmployeeSales.CssClass = _cssClass;
    }

    /// <summary>
    /// rebind document grid
    /// </summary>
    private void RebindGrid()
    {
        litMessage.Text = "";
        phSearchResults.Visible = true;

        gridTicket.ResetSearch = true;
        gridTicket.SearchTicketNumber = TicketId;

        if (txtDescription.Text.Trim().Contains("[") == false)
            gridTicket.SearchDescription = txtDescription.Text.Trim();
        else
            gridTicket.SearchDescription = txtDescription.Text.Trim().Replace("[", "\\[");
        //gridTicket.SearchClientId = ddlClient.ClientId;
        if (ddlSmartClient.SelectedClientId != -1)
            gridTicket.SearchClientId = ddlSmartClient.SelectedClientId;
        else
            gridTicket.SearchClientId = null;

        gridTicket.SearchAssignedToUserId = AssignedToUserId;
        gridTicket.SearchCreatedByUserId = CreatedByUserId;
        gridTicket.SearchStatus = ddlTicketStatus.Status;
        gridTicket.SearchDispositionId = ddlTicketDisposition.TicketDispositionId;
        gridTicket.SearchCreatedStartDate = ucCreatedStart.SelectedDate;
        gridTicket.SearchCreatedEndDate = ucCreatedEnd.SelectedDate;
        gridTicket.SearchLastUpdatedStartDate = ucLastUpdatedStart.SelectedDate;
        gridTicket.SearchLastUpdatedEndDate = ucLastUpdatedEnd.SelectedDate;
        gridTicket.SearchPriorityId = ddlTicketPriority.PriorityId > 0 ? ddlTicketPriority.PriorityId : (int?)null;

        List<DesktopShared.Ticket.Tagged.TicketTag> _selectedTags = new List<DesktopShared.Ticket.Tagged.TicketTag>();
        if (ddlTicketTag.SelectedValues.Count > 0)
        {
            foreach (string _selected in ddlTicketTag.SelectedValues)
            {
                int _pos1 = _selected.IndexOf("|");
                int _pos2 = _selected.LastIndexOf("|");

                if ((_pos1 > 0) && (_pos2 > 0))
                    _selectedTags.Add(new DesktopShared.Ticket.Tagged.TicketTag { Name = _selected.Substring(0, _pos1).Trim(), Public = _selected.Substring(_pos1 + 1, 1) == "1", ClientPublic = _selected.Substring(_pos2 + 1, 1) == "1" });
            }
        }

        string _tags = string.Empty;

        if (_selectedTags.Count > 0)
        {
            for (int i = 0; i <= _selectedTags.Count - 1; i++)
            {
                if (_tags == string.Empty)
                    _tags = _selectedTags[i].Name;
                else
                    _tags += "," + _selectedTags[i].Name;
            }
        }
        
        string _timespent = ddlTimeSpent.Hour.ToString().PadLeft(2, '0') + ddlTimeSpent.Minute.ToString().PadLeft(2, '0');
        int _intHourSpent = Convert.ToInt32(_timespent.Substring(0, 2));
        int _intMinuteSpent = Convert.ToInt32(_timespent.Substring(2));
        string _strMinuteSpent = "";

        switch (_intMinuteSpent)
        {
            case 30:
                _strMinuteSpent = ".5";
                break;
        }

        double _hours = Convert.ToDouble(_intHourSpent.ToString() + _strMinuteSpent);


        gridTicket.SearchTag = _tags.Trim();
        //gridTicket.SearchTagUserId = ddlTicketTag.IsPublic ? (int?)null : DesktopShared.User.UserID;
        gridTicket.SearchTeamId = ddlTeam.TeamId;
        gridTicket.SearchTicketTypeId = ddlTicketType.TicketTypeId;
        gridTicket.SearchCreatedLastDays = String.IsNullOrWhiteSpace(txtCreatedLastDays.Text) ? (int?)null : Convert.ToInt32(txtCreatedLastDays.Text);
        gridTicket.SearchUpdatedLastDays = String.IsNullOrWhiteSpace(txtUpdatedLastDays.Text) ? (int?)null : Convert.ToInt32(txtUpdatedLastDays.Text);
        gridTicket.SearchSalesEmployeeId = SalesEmployeeId;
        gridTicket.SearchAwaitingResponse = chkAwaitingResponse.Checked;
        gridTicket.SearchUpdatedByUserId = ddlEmployeeUpdatedBy.EmployeeId > 0 ? ddlEmployeeUpdatedBy.EmployeeId : (int?)null;
        gridTicket.SearchCategoryId = ddlTicketCategory.TicketCategoryId.HasValue ? ddlTicketCategory.TicketCategoryId : (int?)null;
        gridTicket.SearchExcludeHelpdesk = cbExcludeHelpdesk.Checked;
        gridTicket.SearchClientLocationId = ddlClientLocation.ClientLocationId;
        gridTicket.SearchClientContactId = ddlClientContact.ClientContactId;
        gridTicket.SearchReportedby = ddlReportedBy.ClientContactId == null ? null : ddlReportedBy.ClientContactName;
        gridTicket.SearchExcludeViews = cbExcludeViews.Checked;
        gridTicket.SearchVIP = cbVIP.Checked;
        if (_hours > 0)
            gridTicket.SearchTimeSpent = _hours;
        else
            gridTicket.SearchTimeSpent = null;
        gridTicket.RebindGrid();
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
        
        if (Page.IsValid)
        {
            DesktopShared.User.Ticket.Search.TicketNumber = TicketId;
            DesktopShared.User.Ticket.Search.TicketDescription = txtDescription.Text.Trim();
            //DesktopShared.User.Ticket.Search.ClientId = ddlClient.ClientId;
            if (ddlSmartClient.SelectedClientId != -1)
                DesktopShared.User.Ticket.Search.ClientId = ddlSmartClient.SelectedClientId;
            else
                DesktopShared.User.Ticket.Search.ClientId = null;

            DesktopShared.User.Ticket.Search.AssignedTo = AssignedToUserId;
            DesktopShared.User.Ticket.Search.CreatedByUserId = CreatedByUserId;
            DesktopShared.User.Ticket.Search.TicketStatus = ddlTicketStatus.Status;
            DesktopShared.User.Ticket.Search.DispositionId = ddlTicketDisposition.TicketDispositionId;
            DesktopShared.User.Ticket.Search.PriorityId = ddlTicketPriority.PriorityId > 0 ? ddlTicketPriority.PriorityId : (int?)null;
            
            List<DesktopShared.Ticket.Tagged.TicketTag> _selectedTags = new List<DesktopShared.Ticket.Tagged.TicketTag>();
            if (ddlTicketTag.SelectedValues.Count > 0)
            {
                foreach (string _selected in ddlTicketTag.SelectedValues)
                {
                    int _pos1 = _selected.IndexOf("|");
                    int _pos2 = _selected.LastIndexOf("|");

                    if ((_pos1 > 0) && (_pos2 > 0))
                        _selectedTags.Add(new DesktopShared.Ticket.Tagged.TicketTag { Name = _selected.Substring(0, _pos1).Trim(), Public = _selected.Substring(_pos1 + 1, 1) == "1", ClientPublic = _selected.Substring(_pos2 + 1, 1) == "1" });
                }
            }

            string _tags = string.Empty;
            string _viewtags = string.Empty;
            int _public = 0;
            int _clientpublic = 0;

            if (_selectedTags.Count > 0)
            {
                for (int i = 0; i <= _selectedTags.Count - 1; i++)
                {
                    _public = _selectedTags[i].Public == true ? 1 : 0;
                    _clientpublic = _selectedTags[i].ClientPublic == true ? 1 : 0;

                    if (_tags == string.Empty)
                    {
                        _tags = _selectedTags[i].Name;
                        _viewtags = _selectedTags[i].Name + "|" + _public + "|" + _clientpublic;
                    }
                    else
                    {
                        _tags += "," + _selectedTags[i].Name;
                        _viewtags += "," + _selectedTags[i].Name + "|" + _public + "|" + _clientpublic;
                    }
                        
                }
            }

            DesktopShared.User.Ticket.Search.TagName = _tags.Trim();
            DesktopShared.User.Ticket.Search.TeamId = ddlTeam.TeamId;
            DesktopShared.User.Ticket.Search.TicketTypeId = ddlTicketType.TicketTypeId;
            DesktopShared.User.Ticket.Search.CreatedStartDate = ucCreatedStart.SelectedDate.HasValue ? ucCreatedStart.SelectedDate.Value : DateTime.MinValue;
            DesktopShared.User.Ticket.Search.CreatedEndDate = ucCreatedEnd.SelectedDate.HasValue ? ucCreatedEnd.SelectedDate.Value : DateTime.MinValue;
            DesktopShared.User.Ticket.Search.UpdatedStartDate = ucLastUpdatedStart.SelectedDate.HasValue ? ucLastUpdatedStart.SelectedDate.Value : DateTime.MinValue;
            DesktopShared.User.Ticket.Search.UpdatedEndDate = ucLastUpdatedEnd.SelectedDate.HasValue ? ucLastUpdatedEnd.SelectedDate.Value : DateTime.MinValue;
            DesktopShared.User.Ticket.Search.CreatedLastDays = String.IsNullOrWhiteSpace(txtCreatedLastDays.Text) ? (int?)null : Convert.ToInt32(txtCreatedLastDays.Text);
            DesktopShared.User.Ticket.Search.UpdatedLastDays = String.IsNullOrWhiteSpace(txtUpdatedLastDays.Text) ? (int?)null : Convert.ToInt32(txtUpdatedLastDays.Text);
            DesktopShared.User.Ticket.Search.SalesEmployeeId = SalesEmployeeId;
            DesktopShared.User.Ticket.Search.AwaitingResponse = chkAwaitingResponse.Checked;
            //DesktopShared.User.Ticket.Search.UpdatedByUserUserId = ddlEmployeeUpdatedBy.EmployeeId;

            #region save view 

            if (chkSaveView.Checked)
            {
                DesktopShared.EntityClasses.TicketViewEntity _ticketView = null;

                if ((ddlTicketViewUser.TicketViewId.HasValue) && (txtViewName.Text.Trim().Length == 0)) //save existing entity
                {
                    _ticketView = new DesktopShared.EntityClasses.TicketViewEntity(ddlTicketViewUser.TicketViewId.Value);
                }
                else //create new entity
                {
                    _ticketView = new DesktopShared.EntityClasses.TicketViewEntity();
                    _ticketView.ResponsivePage = true;
                    _ticketView.Name = txtViewName.Text.Trim();
                    _ticketView.UsersId = DesktopShared.User.UserID;
                }

                _ticketView.Public = chkIsPublicView.Checked;
                _ticketView.TicketId = String.IsNullOrWhiteSpace(txtTicketNumber.Text) ? (int?)null : Convert.ToInt32(txtTicketNumber.Text);
                _ticketView.TicketDescription = txtDescription.Text.Trim();
                //_ticketView.ClientId = ddlClient.ClientId;
                if (ddlSmartClient.SelectedClientId != -1)
                    _ticketView.ClientId = ddlSmartClient.SelectedClientId;
                else
                    _ticketView.ClientId = null;

                _ticketView.AssignedTo = ddlEmployeeAssignedTo.EmployeeId > 0 ? ddlEmployeeAssignedTo.EmployeeId : (int?)null;
                _ticketView.CreatedByUserId = ddlEmployeeCreatedBy.EmployeeId > 0 ? ddlEmployeeCreatedBy.EmployeeId : (int?)null;
                _ticketView.UpdatedByUserUserId = ddlEmployeeUpdatedBy.EmployeeId > 0 ? ddlEmployeeUpdatedBy.EmployeeId : (int?)null;
                _ticketView.TicketStatus = ddlTicketStatus.Status.ToString();
                _ticketView.DispositionId = ddlTicketDisposition.TicketDispositionId;
                _ticketView.PriorityId = ddlTicketPriority.PriorityId > 0 ? ddlTicketPriority.PriorityId : (int?)null;
                //if (!String.IsNullOrWhiteSpace(ddlTicketTag.TagValue))
                //{
                //    _ticketView.TagName = ddlTicketTag.TagValue.Trim();
                //    _ticketView.TagIsPublic = ddlTicketTag.IsPublic;
                //}
                //else
                //{
                //    _ticketView.TagName = null;
                //}
                //new tag
                if(_tags.Trim() != string.Empty)
                    _ticketView.TagName = _viewtags.Trim();
                else
                    _ticketView.TagName = null;

                _ticketView.TeamId = ddlTeam.TeamId;
                _ticketView.TicketTypeId = ddlTicketType.TicketTypeId;
                _ticketView.CreatedStartDate = ucCreatedStart.SelectedDate;
                _ticketView.CreatedEndDate = ucCreatedEnd.SelectedDate;
                _ticketView.UpdatedStartDate = ucLastUpdatedStart.SelectedDate;
                _ticketView.UpdatedEndDate = ucLastUpdatedEnd.SelectedDate;
                _ticketView.CreatedLastDays = String.IsNullOrWhiteSpace(txtCreatedLastDays.Text) ? (int?)null : Convert.ToInt32(txtCreatedLastDays.Text);
                _ticketView.UpdatedLastDays = String.IsNullOrWhiteSpace(txtUpdatedLastDays.Text) ? (int?)null : Convert.ToInt32(txtUpdatedLastDays.Text);
                _ticketView.SalesEmployeeeId = SalesEmployeeId;
                _ticketView.AwaitingResponse = chkAwaitingResponse.Checked;
                _ticketView.ExcludeHelpdesk = cbExcludeHelpdesk.Checked;
                _ticketView.LastUpdated = DateTime.Now;

                string _timespent = ddlTimeSpent.Hour.ToString().PadLeft(2, '0') + ddlTimeSpent.Minute.ToString().PadLeft(2, '0');
                int _intHourSpent = Convert.ToInt32(_timespent.Substring(0, 2));
                int _intMinuteSpent = Convert.ToInt32(_timespent.Substring(2));
                string _strMinuteSpent = "";

                switch (_intMinuteSpent)
                {
                    case 30:
                        _strMinuteSpent = ".5";
                        break;
                }

                double _hours = Convert.ToDouble(_intHourSpent.ToString() + _strMinuteSpent);
                _ticketView.TimeSpent = _hours;

                _ticketView.Save();

                //re-populate user view drop down list
                ddlTicketViewUser.TicketViewId = _ticketView.Id;

                //re-populate public view drop down list
                ddlTicketViewPublic.Populate();

                //clear view name
                txtViewName.Text = "";

                //display delete button
                phDeleteView.Visible = true;

                _ticketView.Save();

            }

            #endregion

            RebindGrid();
        }
        else
            phSearchResults.Visible = false;
    }

    /// <summary>
    /// clear button on click
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnClear_Click(object sender, EventArgs e)
    {
        ClearForm();
    }

    /// <summary>
    /// delete view on button click
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnDeleteView_Click(object sender, EventArgs e)
    {
        if (ddlTicketViewUser.TicketViewId.HasValue && ddlTicketViewUser.TicketViewId.Value > 0)
        {
            DesktopShared.EntityClasses.TicketViewEntity _view = new DesktopShared.EntityClasses.TicketViewEntity(ddlTicketViewUser.TicketViewId.Value);
            _view.Delete();
            ClearForm();
        }
    }

    #region custom validators

    /// <summary>
    /// validate minimum search criteria
    /// </summary>
    /// <param name="source"></param>
    /// <param name="args"></param>
    protected void cvCriteriaEntered_ServerValidate(object source, ServerValidateEventArgs args)
    {
        args.IsValid = true;
        
        if (txtDescription.Text.Trim() != string.Empty)
        {
            if (!ucCreatedStart.SelectedDate.HasValue)
            {
                ucCreatedStart.IsRequired = true;
                ucCreatedStart.ErrorMessage = "Created start date is required";
            }
            else
                return;
        }


        if (!String.IsNullOrWhiteSpace(txtTicketNumber.Text))
            return;
        if (!String.IsNullOrWhiteSpace(txtDescription.Text))
            return;
        //if (ddlClient.ClientId.HasValue)
        //    return;
        if (ddlSmartClient.SelectedClientId != -1)
            return;

        if (AssignedToUserId.HasValue)
            return;
        if (CreatedByUserId.HasValue)
            return;

        if (ddlTicketDisposition.TicketDispositionId.HasValue)
            return;
        if (ddlTicketPriority.PriorityId > 0)
            return;
        //if (!String.IsNullOrWhiteSpace(ddlTicketTag.TagValue))
        //    return;

        if (ddlTicketTag.SelectedValues.Count > 0)
            return;

        if (ddlTeam.TeamId.HasValue)
            return;
        if (ddlTicketType.TicketTypeId.HasValue)
            return;
        if (ucCreatedStart.SelectedDate.HasValue)
            return;

        if (ucCreatedEnd.SelectedDate.HasValue)
            return;
        if (ucLastUpdatedStart.SelectedDate.HasValue)
            return;
        if (ucLastUpdatedEnd.SelectedDate.HasValue)
            return;

        if (!String.IsNullOrWhiteSpace(txtCreatedLastDays.Text))
            return;
        if (!String.IsNullOrWhiteSpace(txtUpdatedLastDays.Text))
            return;
        if (SalesEmployeeId.HasValue)
            return;

        if (chkAwaitingResponse.Checked)
            return;
        if (UpdateByUserId.HasValue)
            return;
        args.IsValid = false;
    }

    /// <summary>
    /// validate created dates
    /// </summary>
    /// <param name="source"></param>
    /// <param name="args"></param>
    protected void cvCreatedDates_ServerValidate(object source, ServerValidateEventArgs args)
    {
        args.IsValid = true;
        if (!ucCreatedStart.SelectedDate.HasValue || !ucCreatedEnd.SelectedDate.HasValue)
            return;
        args.IsValid = ucCreatedEnd.SelectedDate.Value.Date >= ucCreatedStart.SelectedDate.Value.Date;
    }

    /// <summary>
    /// validate last updated dates
    /// </summary>
    /// <param name="source"></param>
    /// <param name="args"></param>
    protected void cvLastUpdatedDates_ServerValidate(object source, ServerValidateEventArgs args)
    {
        args.IsValid = true;
        if (!ucLastUpdatedStart.SelectedDate.HasValue || !ucLastUpdatedEnd.SelectedDate.HasValue)
            return;
        args.IsValid = ucLastUpdatedEnd.SelectedDate.Value.Date >= ucLastUpdatedStart.SelectedDate.Value.Date;
    }

    /// <summary>
    /// save view validations
    /// </summary>
    /// <param name="source"></param>
    /// <param name="args"></param>
    protected void cvView_ServerValidate(object source, ServerValidateEventArgs args)
    {
        args.IsValid = true;
        if (!chkSaveView.Checked)
            return;

        if (ddlTicketViewUser.TicketViewId.HasValue || txtViewName.Text.Trim().Length > 0) 
        {
            //validate unique name if text entered in view name text box
            string _viewName = txtViewName.Text.Trim();
            if (_viewName.Length > 0)
            {
                if (!DesktopShared.Ticket.View.IsUniqueName(DesktopShared.User.UserID, _viewName))
                {
                    cvView.ErrorMessage = "Ticket View Name is already in use";
                    args.IsValid = false;
                }
            }
        }
        else
        {
            cvView.ErrorMessage = "You must select a Ticket View from the My Views Drop Down List or enter a name when saving a view.";
            args.IsValid = false;
        }
    }

    #endregion

    #endregion

    #region private properties

    /// <summary>
    /// get ticked id if entered
    /// </summary>
    private int? TicketId
    {
        get { return String.IsNullOrWhiteSpace(txtTicketNumber.Text) ? (int?)null : Convert.ToInt32(txtTicketNumber.Text); }
    }

    /// <summary>
    /// get assigned to user id if entered
    /// </summary>
    private int? AssignedToUserId
    {
        get { return ddlEmployeeAssignedTo.EmployeeId > 0 ? ddlEmployeeAssignedTo.EmployeeId : (int?)null; }
    }

    /// <summary>
    /// get created by user id if entered
    /// </summary>
    private int? CreatedByUserId
    {
        get { return ddlEmployeeCreatedBy.EmployeeId > 0 ? ddlEmployeeCreatedBy.EmployeeId : (int?)null; }
    }

    /// <summary>
    /// get created by user id if entered
    /// </summary>
    private int? UpdateByUserId
    {
        get { return ddlEmployeeUpdatedBy.EmployeeId > 0 ? ddlEmployeeUpdatedBy.EmployeeId : (int?)null; }
    }

    /// <summary>
    /// get sales employee id if entered
    /// </summary>
    private int? SalesEmployeeId
    {
        get { return ddlEmployeeSales.EmployeeId > 0 ? ddlEmployeeSales.EmployeeId : (int?)null; }
    }

    #endregion

}