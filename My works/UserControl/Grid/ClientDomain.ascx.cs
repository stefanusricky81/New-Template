using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class UserControl_Grid_ClientDomain : System.Web.UI.UserControl
{
    /// <summary>
    /// page load
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    #region public methods

    /// <summary>
    /// rebind grid
    /// </summary>
    public void RebindGrid()
    {
        rgClientDomain.Visible = true;
        rgClientDomain.EditIndexes.Clear();
        rgClientDomain.DataSource = null;
        rgClientDomain.Rebind();
    }

    #endregion

    #region private methods

    /// <summary>
    /// configure for device
    /// </summary>
    private void ConfigureForDevice()
    {
        if (DesktopShared.Utility.BrowserHelper.IsTabletOrSmartPhone(this.Request))
            rgClientDomain.RenderMode = RenderMode.Lightweight;

    }

    #endregion

    #region protected events

    /// <summary>
    /// grid on need data source
    /// </summary>
    /// <param name="source"></param>
    /// <param name="e"></param>
    protected void rgClientDomain_NeedDataSource(object source, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        if (!ClientIdForDomain.HasValue)
        {
            rgClientDomain.Visible = false;
            return;
        }
        rgClientDomain.DataSource = DesktopShared.Client.Domain.Get(ClientIdForDomain.Value);
    }

    /// <summary>
    /// groid on item data bound
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rgClientDomain_ItemDataBound(object sender, GridItemEventArgs e)
    {
        #region Item or AlternatingItem

        if (e.Item.ItemType == GridItemType.Item || e.Item.ItemType == GridItemType.AlternatingItem)
        {
            GridDataItem _gdi = (GridDataItem)e.Item;
            DesktopShared.EntityClasses.ClientDomainEntity objClientDomain = (DesktopShared.EntityClasses.ClientDomainEntity)e.Item.DataItem;

            DesktopShared.Utility.Grid.TelerikHelper.AddLabelToCell(objClientDomain.Name.Trim(), _gdi["Name"]);
            DesktopShared.Utility.Grid.TelerikHelper.AddLabelToCell(objClientDomain.Description.Trim(), _gdi["Description"]);
            DesktopShared.Utility.Grid.TelerikHelper.AddLabelToCell(objClientDomain.Active ? "Y" : "N", _gdi["Active"]);

        }

        #endregion

        #region GridEditFormItem and IsInEditMode

        if (e.Item is GridEditFormItem && e.Item.IsInEditMode)
        {

            UserControl_Grid_EditForm_ClientDomainEdit ucEditForm = e.Item.FindControl(GridEditFormItem.EditFormUserControlID) as UserControl_Grid_EditForm_ClientDomainEdit;
            ucEditForm.ClientIdForDomain = ClientIdForDomain.Value;
            if (!e.Item.OwnerTableView.IsItemInserted) //edit/view
            {
                DesktopShared.EntityClasses.ClientDomainEntity objClientDomain = (DesktopShared.EntityClasses.ClientDomainEntity)e.Item.DataItem;
                ucEditForm.LoadValues(objClientDomain);
            }
            else
                ucEditForm.LoadValues(null);
        }

        #endregion
    }

    /// <summary>
    /// grid on update command
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rgClientDomain_UpdateCommand(object sender, GridCommandEventArgs e)
    {
        GridEditableItem editedItem = (GridEditableItem)e.Item;
        int id = int.Parse(editedItem.OwnerTableView.DataKeyValues[editedItem.ItemIndex]["Id"].ToString());
        UserControl_Grid_EditForm_ClientDomainEdit ucEditForm = e.Item.FindControl(GridEditFormItem.EditFormUserControlID) as UserControl_Grid_EditForm_ClientDomainEdit;
        ucEditForm.SaveValues(id);
        RebindGrid();
        string _message = String.Format("Domain has been updated - {0}.", DateTime.Now);
        if (ItemCommand != null)
            ItemCommand(true, _message);
    }


    /// <summary>
    /// grid on insert command
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rgClientDomain_InsertCommand(object sender, GridCommandEventArgs e)
    {
        UserControl_Grid_EditForm_ClientDomainEdit ucEditForm = e.Item.FindControl(GridEditFormItem.EditFormUserControlID) as UserControl_Grid_EditForm_ClientDomainEdit;
        int _domainid = ucEditForm.SaveValues(null);
        RebindGrid();
        string _message = String.Format("Domain has been added - {0}.", DateTime.Now);
        if (ItemCommand != null)
            ItemCommand(true, _message);
    }

    /// <summary>
    /// grid on delete command
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rgClientDomain_DeleteCommand(object sender, GridCommandEventArgs e)
    {
        string _message = "";
        try
        {
            GridEditableItem editedItem = (GridEditableItem)e.Item;
            int id = int.Parse(editedItem.OwnerTableView.DataKeyValues[editedItem.ItemIndex]["Id"].ToString());

            DesktopShared.EntityClasses.ClientDomainEntity objClientDomain = new DesktopShared.EntityClasses.ClientDomainEntity(id);
            objClientDomain.Delete();
            RebindGrid();
            
            _message = String.Format("Domain has been deleted - {0}.", DateTime.Now);
            if (ItemCommand != null)
                ItemCommand(true, _message);

        }
        catch
        {
            _message = String.Format("Domain is in use and cannot be deleted - {0}.", DateTime.Now);
            if (ItemCommand != null)
                ItemCommand(false, _message);
        }
    }

    /// <summary>
    /// grid on pre render
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rgClientDomain_PreRender(object sender, EventArgs e)
    {
        if (DesktopShared.Utility.BrowserHelper.IsTabletOrSmartPhone(this.Request))
        {
            rgClientDomain.MasterTableView.Columns[0].HeaderStyle.Width = Unit.Pixel(200);
            rgClientDomain.MasterTableView.Columns[1].HeaderStyle.Width = Unit.Pixel(400);
            rgClientDomain.MasterTableView.Columns[2].HeaderStyle.Width = Unit.Pixel(125);
        }

        rgClientDomain.ShowHeader = true;
        rgClientDomain.MasterTableView.PagerStyle.AlwaysVisible = true;
        rgClientDomain.MasterTableView.PagerStyle.Visible = true;

        GridCommandItem commandItem = null;
        if (rgClientDomain.MasterTableView.Items.Count > 0)
        {
            commandItem = (GridCommandItem)rgClientDomain.MasterTableView.GetItems(GridItemType.CommandItem)[0];
            commandItem.Visible = true;
        }

        #region add/edit 

        if (rgClientDomain.EditItems.Count > 0 || rgClientDomain.MasterTableView.IsItemInserted)
        {
            foreach (GridDataItem item in rgClientDomain.MasterTableView.Items)
                item.Visible = false;

            rgClientDomain.ShowHeader = false;
            
            if (commandItem != null)
                commandItem.Visible = false; 
        }

        #endregion

    }

    /// <summary>
    /// grid on item command
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rgClientDomain_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (ItemCommand != null)
            ItemCommand(false, "");
    }

    #endregion

    #region public properties

    /// <summary>
    /// get/set client id
    /// </summary>
    public int? ClientIdForDomain
    {
        get
        {
            object obj = this.ViewState["cid_clg"];
            return (obj == null) ? (int?)null : (int)obj;
        }
        set { this.ViewState["cid_clg"] = value; }
    }

    #endregion

    #region public delegates/events

    public delegate void ActionCompleted(bool success, string message);
    public event ActionCompleted ItemCommand;

    #endregion   
}