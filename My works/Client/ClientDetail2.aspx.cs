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
using System.Text.RegularExpressions;
using System.Collections.Generic;
using DesktopShared.HelperClasses;

public partial class Client_ClientDetail2 : BasePage
{
    static Regex validate_emailaddress = email_validation();

    /// <summary>
    /// page load
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        gridClientLocation.ItemCommand += GridClientLocation_ItemCommand;
        ConfigureForDevice();

        if (!this.IsPostBack)
        {
            SetUpPage();
            DesktopShared.EntityClasses.ClientEntity objClient = null;
            if (!GetClientId(ref objClient))
                return;

            populatedropdownlist();

            loadportalClientwidget();
            LoadValues(objClient);

            
        }
    }

    #region private methods

    /// <summary>
    /// grid client location on item command
    /// </summary>
    /// <param name="success"></param>
    /// <param name="message"></param>
    private void GridClientLocation_ItemCommand(bool success, string message)
    {
        if (!String.IsNullOrWhiteSpace(message))
            DesktopShared.Bootstrap.Alert.DisplayMessage(litMessage, message, success ? DesktopShared.Bootstrap.Alert.AlertType.Success : DesktopShared.Bootstrap.Alert.AlertType.Warning);
    }

    /// <summary>
    /// set up for initial page load
    /// </summary>
    private void SetUpPage()
    {
        chkLocationActive.Focus();

        #region tab index

        short _tabIndex = 0;

        chkLocationActive.TabIndex = ++_tabIndex; 

        #endregion
    }

    /// <summary>
    /// configure for device
    /// </summary>
    private void ConfigureForDevice()
    {
        /*bool _displayChosenScript = true;
        string _cssClass = "form-control select-chosen";
        if (DesktopShared.Utility.BrowserHelper.IsTabletOrSmartPhone(this.Request))
        {
            _displayChosenScript = false;
            _cssClass = "form-control";
        }*/
    }

    /// <summary>
    /// load values for client
    /// </summary>
    /// <param name="objClient"></param>
    private void LoadValues(DesktopShared.EntityClasses.ClientEntity objClient)
    {
        chkLocationActive.Checked = objClient.UseTicketLocation;
        gridClientLocation.Visible = objClient.UseTicketLocation;
        if (objClient.UseTicketLocation)
        {
            gridClientLocation.ClientIdForLocation = objClient.Pclient;
            gridClientLocation.RebindGrid();
        }
        //ddlClientEmailCategory.SelectedValue = objClient.FkClientemailcategory.ToString();
        cbRequireTicketCategory.Checked = objClient.TicketCategoryRequired;
        txtPagerDutyEmailForwarding.Text = objClient.PagerDutyEmailForwarding;

        bool _displayorder = false;
        if (objClient.DisplayTicketOrder != null)
            _displayorder = (bool)objClient.DisplayTicketOrder;

        cbDisplayTicketOrder.Checked = _displayorder;

        bool _emailhistory = false;
        if (objClient.CloseEmailHistory != null)
            _emailhistory = (bool)objClient.CloseEmailHistory;

        cbClosedTicketHistoryEmail.Checked = _emailhistory;

        txtJamfPortalURL.Text = objClient.JamfPortalUrl.ToString().Trim();
        txtClientID.Text = objClient.JamfClientId.ToString().Trim();
        txtClientSecret.Text = objClient.JamfClientSecret.ToString().Trim();
        txtUserName.Text = objClient.JamfuserName.ToString().Trim();
        txtPassword.Text = objClient.Jamfpassword.ToString().Trim();
    }
    private bool updateClientTicketCategory()
    {
        try {
            ClientEntity _client = new ClientEntity((int)SelectedClientId);
            _client.TicketCategoryRequired = cbRequireTicketCategory.Checked ? true : false;
            _client.Save();
            _client.Refetch();

            return true;
        }
        catch {
            return false;
        }
    }

    private bool UpdateDisplayOrder()
    {
        try
        {
            int _id = -1;
            if (int.TryParse(BitByBit.Web.Request.GetString("ClientID").Trim(), out _id))
            {
                SelectedClientId = _id;
            }

            ClientEntity _client = new ClientEntity((int)SelectedClientId);
            _client.DisplayTicketOrder = cbDisplayTicketOrder.Checked ? true : false;
            _client.Save();
            _client.Refetch();

            return true;

        }
        catch
        {
            return false;
        }
    }

    private bool UpdateEmailHistory()
    {
        try
        {
            int _id = -1;
            if (int.TryParse(BitByBit.Web.Request.GetString("ClientID").Trim(), out _id))
            {
                SelectedClientId = _id;
            }

            ClientEntity _client = new ClientEntity((int)SelectedClientId);
            _client.CloseEmailHistory = cbClosedTicketHistoryEmail.Checked ? true : false;
            _client.Save();
            _client.Refetch();

            return true;

        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// get client id from query string and fetch entity
    /// </summary>
    /// <param name="objClient"></param>
    /// <returns></returns>
    private bool GetClientId(ref DesktopShared.EntityClasses.ClientEntity objClient)
    {
        int _id = -1;
        if (int.TryParse(BitByBit.Web.Request.GetString("ClientID").Trim(), out _id))
        {
            SelectedClientId= _id;
            objClient = new ClientEntity(_id);
            if (objClient.Fields.State == SD.LLBLGen.Pro.ORMSupportClasses.EntityState.Fetched)
                return true;
        }

        DesktopShared.Bootstrap.Alert.DisplayMessage(litMessage, String.Format("Unable to fetch client.  ID = {0}", _id > 0 ? _id.ToString() : "N/A"), DesktopShared.Bootstrap.Alert.AlertType.Danger, false);
        pnlMain.Visible = false;
        return false;
    }

    private void BindGrid()
    {
        rgTicketCategories.DataSource = DesktopShared.ClientTicketCategory.Search(SelectedClientId);
    }
    private void BindGridEmailCategory()
    {
        rgEmailCategory.DataSource = DesktopShared.ClientEmailCategory.GetClientCategoriesEmailClient(SelectedClientId, "C");
    }

    private void DisplayMessage(string message, Bootstrap.Alert.AlertType alertType, bool dismissable = true, string overrideHeader = "")
    {
        ScriptManager.RegisterClientScriptBlock(this, Page.GetType(), "ToTheTop", "ToTopOfPage();", true);
        litMessage.Visible = true;
        litMessage.Text = Bootstrap.Alert.GetDivText(alertType, message, dismissable, true, overrideHeader);
    }
    #endregion

    #region protected events

    protected void loadportalClientwidget()
    {
        //rptWidget.DataSource = DesktopShared.PortalWidget.GetPortalWidgetClient(SelectedClientId);
        rptWidget.DataSource = GetPortalWidget();
        rptWidget.DataBind();
    }

    protected void loadpopup()
    {
        System.Collections.Generic.List<int> widgetid = new System.Collections.Generic.List<int>();

        DataTable dtPortalClientWidget = DesktopShared.PortalWidget.GetPortalWidgetClient(SelectedClientId);
        if (dtPortalClientWidget != null)
        {
            for (int i = 0; i <= dtPortalClientWidget.Rows.Count - 1; i++)
            {
                widgetid.Add(Convert.ToInt32(dtPortalClientWidget.Rows[i]["ClientWigdetID"].ToString()));
            }
        }

        lstSrcWidget.DataSource = DesktopShared.PortalWidget.GetUnsetPortalWidget(widgetid);
        lstSrcWidget.DataTextField = "WidgetDisplayName";
        lstSrcWidget.DataValueField = "ID";
        lstSrcWidget.DataBind();

        lstDestWidget.DataSource = DesktopShared.PortalWidget.GetPortalWidgetClient(SelectedClientId);
        lstDestWidget.DataTextField = "WidgetDisplayName";
        lstDestWidget.DataValueField = "ClientWigdetID";
        lstDestWidget.DataBind();
    }
    /// <summary>
    /// auto billing on checked changed
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void chkLocationActive_CheckedChanged(object sender, EventArgs e)
    {
        int _id = -1;
        if (int.TryParse(BitByBit.Web.Request.GetString("ClientID").Trim(), out _id))
        {
            SelectedClientId = _id;
        }
        var objClient = new DesktopShared.EntityClasses.ClientEntity(SelectedClientId.Value);
        objClient.UseTicketLocation = chkLocationActive.Checked;
        objClient.Save();
        DesktopShared.Bootstrap.Alert.DisplayMessage(litMessage, String.Format("Location has been {0}.", chkLocationActive.Checked ? "enabled" : "disabled"), DesktopShared.Bootstrap.Alert.AlertType.Success);
        LoadValues(objClient);
    }

    protected void btnAddPagerDuty_Click(object sender, EventArgs e)
    {
        try 
        {
            if (DesktopShared.Utility.Email.CheckCustomEmail(txtPagerDutyEmailForwarding.Text.Trim()))
            {
                if (txtPagerDutyEmailForwarding.Text == string.Empty)
                {
                    DisplayMessage("Email Address is required", Bootstrap.Alert.AlertType.Danger);
                    return;
                }
                else
                {
                    ClientEntity _client = new ClientEntity((int)SelectedClientId);
                    _client.PagerDutyEmailForwarding = txtPagerDutyEmailForwarding.Text.Trim();
                    _client.Save();
                }

                DisplayMessage(String.Format("Pager Duty has been succesfully {0} - {1}", "Added", DateTime.Now), Bootstrap.Alert.AlertType.Success);
            }
            else
            {
                DisplayMessage("Invalid Email Address", Bootstrap.Alert.AlertType.Danger);
                return;
            }
        }
        catch (Exception ex)
        {
            DisplayMessage(ex.Message.ToString().Trim(), Bootstrap.Alert.AlertType.Danger);
            return;
        }
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        int _ticketcategoryid = 0;
        TicketCategoryEntity _objclientticketcategory = new TicketCategoryEntity(_ticketcategoryid);
        string _categoryname = _objclientticketcategory.CategoryName;
        string _default = _objclientticketcategory.CategoryDefault;
        string _active = _objclientticketcategory.Active;
        int _auditUserId = DesktopShared.User.UserID;

        if (ddlTicketCategory.SelectedValue != string.Empty)
            _ticketcategoryid = Convert.ToInt32(ddlTicketCategory.SelectedValue);
        else
        {
            DisplayMessage("Please Choose Email category first", Bootstrap.Alert.AlertType.Danger);
            return;
        }
        if (DesktopShared.ClientTicketCategory.AddTicketCategory(0, _ticketcategoryid, (int)SelectedClientId, _auditUserId, _default, _active) == true)
        {
            DisplayMessage(String.Format("Ticket Category for client {0} - {1}", "Added", DateTime.Now), Bootstrap.Alert.AlertType.Success);
            BindGrid();
            rgTicketCategories.DataBind();
            populatedropdownlist();
        }
        else
            DisplayMessage("Something went wrong", Bootstrap.Alert.AlertType.Danger);
    }

    protected void btnAddNew_Click(object sender, EventArgs e)
    {
        if (true)
        {
            lblModalTitleAddEdit.Text = "Add/Edit Ticket Category";
            btnAddClientTicketCategory.Visible = true;
            txtCategoryName.Text = string.Empty;
            cbActive.Checked = true;
            cbDefault.Checked = true;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "myModalAddEdit", "$('#myModalAddEdit').modal('show');", true);
        }
    }

    protected void btnAddClientEmailCategory_Click(object sender, EventArgs e)
    {
        int _id = 0;
        int _emailcategory = 0;
        int _auditUserId = DesktopShared.User.UserID;
        int.TryParse(BitByBit.Web.Request.GetString("ClientID").Trim(), out _id);
        if (ddlClientEmailCategory.SelectedValue != string.Empty)
            _emailcategory = Convert.ToInt32(ddlClientEmailCategory.SelectedValue);
        else
        {
            DisplayMessage("Please Choose Email category first", Bootstrap.Alert.AlertType.Danger);
            return;
        }

        if (int.TryParse(BitByBit.Web.Request.GetString("ClientID").Trim(), out _id))
        {
            SelectedClientId = _id;
            if (DesktopShared.ClientEmailCategory.checkDuplicateEmailCategoryClient(_id, _emailcategory) == 0)
            {
                if (DesktopShared.ClientEmailCategory.AddEmailCategoriesCLient(_id, _emailcategory, _auditUserId) == true)
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
        int count = 0;
        string widgetname = string.Empty;

        switch (str)
        {
            case "Add":
                if (DesktopShared.TicketCategories.checkDuplicateTicketCategory(txtCategoryName.Text) == 0)
                {
                    if (DesktopShared.ClientTicketCategory.AddTicketCategory(txtCategoryName.Text, _auditUserId, (int)SelectedClientId, cbDefault.Checked == true ? "Y" : "N", cbActive.Checked == true ? "Y" : "N") == true)
                    {
                        DisplayMessage(String.Format("Ticket Category {0} for this client - {1}", "Added", DateTime.Now), Bootstrap.Alert.AlertType.Success);
                        BindGrid();
                        rgTicketCategories.DataBind();
                        populatedropdownlist();
                    }
                    else
                        DisplayMessage("Something went wrong", Bootstrap.Alert.AlertType.Danger);
                }
                else
                    DisplayMessage("Category name already existed in master list", Bootstrap.Alert.AlertType.Danger);
                break;
            case "Delete":
                switch (hfDeleteFor.Value)
                {
                    case "Ticket":
                        if (DesktopShared.ClientTicketCategory.DeleteTicketCategory(Convert.ToInt32(hfDelete.Value)))
                        {
                            DisplayMessage(String.Format("Ticket Category has been {0} - {1}", "Deleted", DateTime.Now), Bootstrap.Alert.AlertType.Success);
                            BindGrid();
                            rgTicketCategories.DataBind();
                            populatedropdownlist();
                        }
                        else
                            DisplayMessage("Something went wrong", Bootstrap.Alert.AlertType.Danger);
                        break;
                    case "Email":
                        if (DesktopShared.ClientEmailCategory.DeleteEmailCategory(Convert.ToInt32(hfDelete.Value)))
                        {
                            DisplayMessage(String.Format("Email Category has been {0} - {1}", "Deleted", DateTime.Now), Bootstrap.Alert.AlertType.Success);
                            BindGridEmailCategory();
                            rgEmailCategory.DataBind();
                            populatedropdownlist();
                        }
                        else
                            DisplayMessage("Something went wrong", Bootstrap.Alert.AlertType.Danger);
                        break;
                }
                break;
            case "Widget":
                if (DesktopShared.PortalWidget.CheckWidgetClient((int)SelectedClientId) > 0)
                {
                    if (DesktopShared.PortalWidget.DeleteWidgetClient((int)SelectedClientId) == true)
                    {
                        for (int i = 0; i <= lstDestWidget.Items.Count - 1; i++)
                        {
                            if (DesktopShared.PortalWidget.SaveWidgetClient(0, (int)SelectedClientId, Convert.ToInt32(lstDestWidget.Items[i].Value)) == true)
                            {
                                count++;
                                if (String.IsNullOrEmpty(widgetname))
                                    widgetname = lstDestWidget.Items[i].Text + ",";
                                else
                                    widgetname += lstDestWidget.Items[i].Text;
                            }
                        }
                    }
                    //DisplayMessage(String.Format("{0} Portal Widget, {1} has been {2} - {3}", count, widgetname, "Added", DateTime.Now), Bootstrap.Alert.AlertType.Success);
                    DisplayMessage(String.Format("Portal Widget has been {0} - {1}", "Added", DateTime.Now), Bootstrap.Alert.AlertType.Success);
                    loadportalClientwidget();
                }
                else
                {
                    for (int i = 0; i <= lstDestWidget.Items.Count - 1; i++)
                    {
                        if (DesktopShared.PortalWidget.SaveWidgetClient(0, (int)SelectedClientId, Convert.ToInt32(lstDestWidget.Items[i].Value)) == true)
                        {
                            count++;
                            if (String.IsNullOrEmpty(widgetname))
                                widgetname = lstDestWidget.Items[i].Text + ",";
                            else
                                widgetname += lstDestWidget.Items[i].Text;
                        }
                    }
                    DisplayMessage(String.Format("Portal Widget has been {0} - {1}", "Added", DateTime.Now), Bootstrap.Alert.AlertType.Success);
                    loadportalClientwidget();
                }
                break;
        }
    }

    protected void rgTicketCategories_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindGrid();
    }

    protected void rgTicketCategories_ItemCommand(object sender, GridCommandEventArgs e)
    {
        int _auditUserId = DesktopShared.User.UserID;
        string command = e.CommandArgument.ToString().Trim();

        switch (command)
        {
            case "DeleteRecord":
                if (true)
                {

                    GridDataItem item = (GridDataItem)e.Item;

                    lblModalDeleteTitle.Text = "Delete Ticket Category";
                    lblModalDeleteWording.Text = "Are you sure want to delete " + item["CategoryName"].Text.Trim() + " ? ";
                    hfDelete.Value = item["id"].Text;
                    hfDeleteFor.Value = "Ticket";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "myModalDelete", "$('#myModalDelete').modal('show');", true);
                }
                break;
            case "DeleteRecordEmailCategory":
                if (true)
                {

                    GridDataItem item = (GridDataItem)e.Item;

                    lblModalDeleteTitle.Text = "Delete Email Category";
                    lblModalDeleteWording.Text = "Are you sure want to delete " + item["CategoryName"].Text.Trim() + " ? ";
                    hfDelete.Value = item["id"].Text;
                    hfDeleteFor.Value = "Email";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "myModalDelete", "$('#myModalDelete').modal('show');", true);
                }
                break;
        }
    }

    protected void rgEmailCategory_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindGridEmailCategory();
    }

    protected void btnSubmitWidget_Click(object sender, EventArgs e)
    {
        int count = 0;
        for (int i = 0; i <= rptWidget.Items.Count - 1; i++)
        {
            CheckBox chk = (CheckBox)rptWidget.Items[i].FindControl("chkWidget");
            HiddenField hf = (HiddenField)rptWidget.Items[i].FindControl("hfWidgetID");
            HiddenField hfID = (HiddenField)rptWidget.Items[i].FindControl("hfID");

            if (DesktopShared.PortalWidget.UpdateWidgetClient(Convert.ToInt32(hfID.Value), chk.Checked ? true : false) == true)
                count++;
        }
        DisplayMessage(String.Format("Widget Succesfully updated -{0}", DateTime.Now), Bootstrap.Alert.AlertType.Success);
    }

    protected void btnAddwidget_Click(object sender, EventArgs e)
    {
        if (true)
        {
            lblModalSetWidget.Text = "Add widget";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "myModalSetWidget", "$('#myModalSetWidget').modal('show');", true);
            loadpopup();
        }
    }

    protected void cbRequireTicketCategory_CheckedChanged(object sender, EventArgs e)
    {
        if (updateClientTicketCategory() == true)
        {
            DisplayMessage("Successfully updated Ticket category required", Bootstrap.Alert.AlertType.Success);
            return;
        }
        else
        {
            DisplayMessage("Failed updated Ticket category required", Bootstrap.Alert.AlertType.Danger);
            return;
        }
    }

    protected void cbDisplayTicketOrder_CheckedChanged(object sender, EventArgs e)
    {
        if (UpdateDisplayOrder() == true)
        {
            DisplayMessage("Successfully updated Display Ticket Project", Bootstrap.Alert.AlertType.Success);
            return;
        }
        else
        {
            DisplayMessage("Failed updated Display Ticket Project", Bootstrap.Alert.AlertType.Danger);
            return;
        }
    }

    protected void cbClosedTicketHistoryEmail_CheckedChanged(object sender, EventArgs e)
    {
        if (UpdateEmailHistory() == true)
        {
            DisplayMessage("Successfully updated Closed Ticket Emal History", Bootstrap.Alert.AlertType.Success);
            return;
        }
        else
        {
            DisplayMessage("Failed updated Closed Ticket Emal History", Bootstrap.Alert.AlertType.Danger);
            return;
        }
    }

    protected void btnAddJamf_Click(object sender, EventArgs e)
    {
        ClientEntity _client = new ClientEntity((int)SelectedClientId);
        _client.JamfPortalUrl = txtJamfPortalURL.Text.Trim();
        _client.JamfClientId = txtClientID.Text.Trim();
        _client.JamfClientSecret = txtClientSecret.Text.Trim();
        _client.JamfuserName = txtUserName.Text.Trim();
        _client.Jamfpassword = txtPassword.Text.Trim();
        _client.Save();

        DisplayMessage("Successfully Updated", Bootstrap.Alert.AlertType.Success);
    }

    private static Regex email_validation()
    {
        string pattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|"
            + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)"
            + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";

        return new Regex(pattern, RegexOptions.IgnoreCase);
    }
    #endregion

    #region private properties

    /// <summary>
    /// get/set client id
    /// </summary>
    private int? SelectedClientId
    {
        get
        {
            object obj = this.ViewState["scid"];
            return (obj == null) ? (int?)null : (int)obj;
        }
        set { this.ViewState["scid"] = value; }
    }

    #endregion

    #region populate dropdowlist
    private void populatedropdownlist()
    {
        ddlClientEmailCategory.DataSource = DesktopShared.StoredProcedureCallerClasses.RetrievalProcedures.ProcGetddlEmailCategory("C", SelectedClientId);
        ddlClientEmailCategory.DataTextField = "CategoryName";
        ddlClientEmailCategory.DataValueField = "ID";
        ddlClientEmailCategory.DataBind();
        ddlClientEmailCategory.Items.Insert(0, string.Empty);

        ddlTicketCategory.DataSource = DesktopShared.StoredProcedureCallerClasses.RetrievalProcedures.ProcGetddlTicketCategory(SelectedClientId);
        ddlTicketCategory.DataTextField = "CategoryName";
        ddlTicketCategory.DataValueField = "ID";
        ddlTicketCategory.DataBind();
        ddlTicketCategory.Items.Insert(0, string.Empty);
    }
    #endregion

    #region populate Widget
    protected static PortalDashboardWidgetCollection GetPortalWidget()
    {
        PortalDashboardWidgetCollection collection = new PortalDashboardWidgetCollection();

        IPredicateExpression _orFilter = new PredicateExpression();
        _orFilter.AddWithAnd(PortalDashboardWidgetFields.Active == 1);

        ISortExpression _folderSort = new SD.LLBLGen.Pro.ORMSupportClasses.SortExpression();
        _folderSort.Add(PortalDashboardWidgetFields.Id | SortOperator.Ascending);
        collection.GetMulti(_orFilter, 0, _folderSort);

        return collection;
    }
    #endregion


}
