using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using SD.LLBLGen.Pro.ORMSupportClasses;

public partial class UserControl_Grid_TicketContactResponsive : System.Web.UI.UserControl
{
    /// <summary>
    /// page load
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        ConfigureForDevice();
    }

    #region private methods

    /// <summary>
    /// configure for device
    /// </summary>
    private void ConfigureForDevice()
    {
        if (DesktopShared.Utility.BrowserHelper.IsTabletOrSmartPhone(this.Request))
        {
            rgTicketContact.RenderMode = RenderMode.Lightweight;
        }
    }

    #endregion

    #region public methods

    /// <summary>
    /// rebind grid
    /// </summary>
    public void RebindGrid()
    {
        rgTicketContact.Visible = true;
        rgTicketContact.EditIndexes.Clear();
        rgTicketContact.DataSource = null;
        rgTicketContact.Rebind();
    }

    #endregion

    #region protected methods / events

    /// <summary>
    /// grid on need data source
    /// </summary>
    /// <param name="source"></param>
    /// <param name="e"></param>
    protected void rgTicketContact_NeedDataSource(object source, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        if (TicketId.HasValue || !String.IsNullOrWhiteSpace(TemporaryGuid))
        {
            rgTicketContact.Visible = true;
            List<DesktopShared.Ticket.Notification.Contact> _contacts = new List<DesktopShared.Ticket.Notification.Contact>();

            //var objClient = new DesktopShared.EntityClasses.ClientEntity(ClientId.HasValue ? ClientId.Value : MasterClientId.HasValue ? MasterClientId.Value : -1);
            //if (objClient.Fields.State == EntityState.Fetched)
            //DisplayLocation = objClient.UseTicketLocation;

            if (ClientId.HasValue)
                _contacts = DesktopShared.Ticket.Notification.GetClientContacts(TicketId, TemporaryGuid, false);//, DisplayLocation, ClientId.Value);
            else if (MasterClientId.HasValue)
                _contacts = DesktopShared.Ticket.Notification.GetClientContacts(TicketId, TemporaryGuid, true);//, DisplayLocation, MasterClientId.Value);
            else
                _contacts = DesktopShared.Ticket.Notification.GetEmployees(TicketId, TemporaryGuid);

            rgTicketContact.DataSource = _contacts;
        }
        else
        {
            rgTicketContact.Visible = false;
        }
    }

    /// <summary>
    /// grid on pre render
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rgTicketContact_PreRender(object sender, EventArgs e)
    {
        if (DesktopShared.Utility.BrowserHelper.IsTabletOrSmartPhone(this.Request))
        {
            int _colIndex = -1;
            rgTicketContact.MasterTableView.Columns[++_colIndex].HeaderStyle.Width = Unit.Pixel(100);
            rgTicketContact.MasterTableView.Columns[++_colIndex].HeaderStyle.Width = Unit.Pixel(550);
        }
    }

    /// <summary>
    /// grid on item data bound
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rgTicketContact_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        #region GridItemType.Item or GridItemType.AlternatingItem

        if (e.Item.ItemType == GridItemType.Item || e.Item.ItemType == GridItemType.AlternatingItem)
        {
            GridEditableItem _gdi = e.Item as GridEditableItem;
            DesktopShared.Ticket.Notification.Contact objContact = (DesktopShared.Ticket.Notification.Contact)e.Item.DataItem;

            string _display = objContact.FullName.Trim();
            if (!String.IsNullOrWhiteSpace(_display))
                _display += String.Format(" ({0})", objContact.Email.Trim());
            else
                _display = objContact.Email.Trim();
            //if (DisplayLocation && !String.IsNullOrWhiteSpace(objContact.PrimaryLocationName))
                //_display += String.Format(" - {0}", objContact.PrimaryLocationName.Trim());
            DesktopShared.Utility.Grid.TelerikHelper.AddLabelToCell(_display, _gdi["FullName"]);

            CheckBox chkEmail = _gdi.FindControl("chkEmail") as CheckBox;
            chkEmail.Checked = objContact.Checked;

            LinkButton btnRemove = _gdi.FindControl("btnRemove") as LinkButton;
            btnRemove.Visible = !objContact.AutoCc && !objContact.PrimaryContact;
        }

        #endregion
    }

    /// <summary>
    /// grid on item command
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rgTicketContact_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName =="Remove")
        {
            int _id = Convert.ToInt32((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["Id"].ToString());
            DesktopShared.EntityClasses.TicketNotificationEntity objTicketNotification = new DesktopShared.EntityClasses.TicketNotificationEntity(_id);
            objTicketNotification.Delete();
            //RebindGrid();
        }

        if (evItemCommand != null)
            evItemCommand(this, e);
    }

    /// <summary>
    /// checked on change
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void chkEmail_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox _cb = (CheckBox)sender;
        GridEditableItem _item = (GridEditableItem)_cb.NamingContainer;
        int _id = Convert.ToInt32(_item.GetDataKeyValue("Id").ToString());
        var objTicketContact = new DesktopShared.EntityClasses.TicketNotificationEntity(_id);
        if (objTicketContact.Fields.State == EntityState.Fetched)
        {
            objTicketContact.Checked = _cb.Checked;
            objTicketContact.Save();
        }   

        if (evItemCommand != null)
            evItemCommand(this, e);

    }

    /// <summary>
    /// uncheck all checked changed
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void chkUncheckAll_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox _cb = (CheckBox)sender;
        if (!_cb.Checked)
        {
            if (evItemCommand != null)
                evItemCommand(this, e);
            return;
        }

        List<int> _ids = new List<int>();
        foreach (GridDataItem gridDataItem in rgTicketContact.MasterTableView.Items)
        {
            CheckBox chkBox = gridDataItem.FindControl("chkEmail") as CheckBox;
            chkBox.Checked = false;

            int _id = Convert.ToInt32(gridDataItem.GetDataKeyValue("Id").ToString());
            _ids.Add(_id);
        }

        #region update all

        var objTicketContact = new DesktopShared.EntityClasses.TicketNotificationEntity();
        objTicketContact.Checked = false;

        var _ticketContacts = new DesktopShared.CollectionClasses.TicketNotificationCollection();
        IPredicateExpression _ticketContactsFilter = new PredicateExpression();
        _ticketContactsFilter.Add(DesktopShared.HelperClasses.TicketNotificationFields.Id == _ids);
        _ticketContacts.UpdateMulti(objTicketContact, _ticketContactsFilter);

        #endregion

        if (evItemCommand != null)
            evItemCommand(this, e);
    }

    #endregion

    #region public methods

    /// <summary>
    /// check all check boxes
    /// </summary>
    public void CheckAll()
    {
        List<int> _ids = new List<int>();
        foreach (GridDataItem gridDataItem in rgTicketContact.MasterTableView.Items)
        {
            CheckBox chkBox = gridDataItem.FindControl("chkEmail") as CheckBox;
            chkBox.Checked = true;

            int _id = Convert.ToInt32(gridDataItem.GetDataKeyValue("Id").ToString());
            _ids.Add(_id);
        }

        #region update all

        var objTicketContact = new DesktopShared.EntityClasses.TicketNotificationEntity();
        objTicketContact.Checked = true;

        var _ticketContacts = new DesktopShared.CollectionClasses.TicketNotificationCollection();
        IPredicateExpression _ticketContactsFilter = new PredicateExpression();
        _ticketContactsFilter.Add(DesktopShared.HelperClasses.TicketNotificationFields.Id == _ids);
        _ticketContacts.UpdateMulti(objTicketContact, _ticketContactsFilter);

        #endregion
    }

    #endregion

    #region public properties

    /// <summary>
    /// get/set ticket id
    /// </summary>
    public int? TicketId
    {
        get
        {
            object obj = this.ViewState["tid_tfg"];
            return (obj == null) ? (int?)null : (int)obj;
        }
        set { this.ViewState["tid_tfg"] = value; }
    }

    /// <summary>
    /// get/set temp guid used for recipients
    /// </summary>
    public string TemporaryGuid
    {
        get
        {
            object obj = this.ViewState["tguid_tfg"];
            return (obj == null) ? "" : (string)obj;
        }
        set { this.ViewState["tguid_tfg"] = value; }
    }


    /// <summary>
    /// get/set client id
    /// </summary>
    public int? ClientId
    {
        get
        {
            object obj = this.ViewState["cid_tfg"];
            return (obj == null) ? (int?)null : (int)obj;
        }
        set { this.ViewState["cid_tfg"] = value; }
    }

    /// <summary>
    /// get/set master client id
    /// </summary>
    public int? MasterClientId
    {
        get
        {
            object obj = this.ViewState["mcid_tfg"];
            return (obj == null) ? (int?)null : (int)obj;
        }
        set { this.ViewState["mcid_tfg"] = value; }
    }

    /// <summary>
    /// get list selected emails
    /// </summary>
    public List<string> SelectedEmails
    {
        get
        {
            List<string> _list = new List<string>();

            foreach (GridDataItem gridDataItem in rgTicketContact.MasterTableView.Items)
            {
                CheckBox chkBox = gridDataItem.FindControl("chkEmail") as CheckBox;
                if (chkBox.Checked)
                    _list.Add(gridDataItem.GetDataKeyValue("Email").ToString());
            }
            return _list;
        }
    }

    /*
    /// <summary>
    /// get/set display location for recipients
    /// </summary>
    public bool DisplayLocation
    {
        get
        {
            object obj = this.ViewState["dl_tfg"];
            return (obj == null) ? false : (bool)obj;
        }
        set { this.ViewState["dl_tfg"] = value; }
    }
    */

    #endregion

    #region public events

    /// <summary>
    /// add event handler for item command
    /// </summary>
    public event EventHandler evItemCommand;

    #endregion  
}
