using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class UserControl_Grid_ClientLocation : System.Web.UI.UserControl
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
        rgClientLocation.Visible = true;
        rgClientLocation.EditIndexes.Clear();
        rgClientLocation.DataSource = null;
        rgClientLocation.Rebind();
    }

    #endregion

    #region private methods

    /// <summary>
    /// configure for device
    /// </summary>
    private void ConfigureForDevice()
    {
        if (DesktopShared.Utility.BrowserHelper.IsTabletOrSmartPhone(this.Request))
            rgClientLocation.RenderMode = RenderMode.Lightweight;

    }

    #endregion

    #region protected events

    /// <summary>
    /// grid on need data source
    /// </summary>
    /// <param name="source"></param>
    /// <param name="e"></param>
    protected void rgClientLocation_NeedDataSource(object source, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        if (!ClientIdForLocation.HasValue)
        {
            rgClientLocation.Visible = false;
            return;
        }
        rgClientLocation.DataSource = DesktopShared.Client.Location.Get(ClientIdForLocation.Value);
    }

    /// <summary>
    /// groid on item data bound
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rgClientLocation_ItemDataBound(object sender, GridItemEventArgs e)
    {
        #region Item or AlternatingItem

        if (e.Item.ItemType == GridItemType.Item || e.Item.ItemType == GridItemType.AlternatingItem)
        {
            GridDataItem _gdi = (GridDataItem)e.Item;
            DesktopShared.EntityClasses.ClientLocationEntity objClientLocation = (DesktopShared.EntityClasses.ClientLocationEntity)e.Item.DataItem;

            DesktopShared.Utility.Grid.TelerikHelper.AddLabelToCell(objClientLocation.Name.Trim(), _gdi["Name"]);
            DesktopShared.Utility.Grid.TelerikHelper.AddLabelToCell(objClientLocation.Description.Trim(), _gdi["Description"]);
            DesktopShared.Utility.Grid.TelerikHelper.AddLabelToCell(objClientLocation.Active ? "Y" : "N", _gdi["Active"]);

        }

        #endregion

        #region GridEditFormItem and IsInEditMode

        if (e.Item is GridEditFormItem && e.Item.IsInEditMode)
        {

            UserControl_Grid_EditForm_ClientLocationEdit ucEditForm = e.Item.FindControl(GridEditFormItem.EditFormUserControlID) as UserControl_Grid_EditForm_ClientLocationEdit;
            ucEditForm.ClientIdForLocation = ClientIdForLocation.Value;
            if (!e.Item.OwnerTableView.IsItemInserted) //edit/view
            {
                DesktopShared.EntityClasses.ClientLocationEntity objClientLocation = (DesktopShared.EntityClasses.ClientLocationEntity)e.Item.DataItem;
                ucEditForm.LoadValues(objClientLocation);
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
    protected void rgClientLocation_UpdateCommand(object sender, GridCommandEventArgs e)
    {
        GridEditableItem editedItem = (GridEditableItem)e.Item;
        int id = int.Parse(editedItem.OwnerTableView.DataKeyValues[editedItem.ItemIndex]["Id"].ToString());
        UserControl_Grid_EditForm_ClientLocationEdit ucEditForm = e.Item.FindControl(GridEditFormItem.EditFormUserControlID) as UserControl_Grid_EditForm_ClientLocationEdit;
        ucEditForm.SaveValues(id);
        RebindGrid();
        string _message = String.Format("Location has been updated - {0}.", DateTime.Now);
        if (ItemCommand != null)
            ItemCommand(true, _message);
    }


    /// <summary>
    /// grid on insert command
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rgClientLocation_InsertCommand(object sender, GridCommandEventArgs e)
    {
        UserControl_Grid_EditForm_ClientLocationEdit ucEditForm = e.Item.FindControl(GridEditFormItem.EditFormUserControlID) as UserControl_Grid_EditForm_ClientLocationEdit;
        int _locationid = ucEditForm.SaveValues(null);
        RebindGrid();
        string _message = String.Format("Location has been added - {0}.", DateTime.Now);
        if (ItemCommand != null)
            ItemCommand(true, _message);
    }

    /// <summary>
    /// grid on delete command
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rgClientLocation_DeleteCommand(object sender, GridCommandEventArgs e)
    {
        string _message = "";
        try
        {
            GridEditableItem editedItem = (GridEditableItem)e.Item;
            int id = int.Parse(editedItem.OwnerTableView.DataKeyValues[editedItem.ItemIndex]["Id"].ToString());

            DesktopShared.EntityClasses.ClientLocationEntity objClientLocation = new DesktopShared.EntityClasses.ClientLocationEntity(id);
            if (objClientLocation.CscDefects.Count > 0)
            {
                _message = String.Format("Location is assigned to Tickets and cannot be deleted - {0}.", DateTime.Now);
                if (ItemCommand != null)
                    ItemCommand(false, _message);
                return;
            }

            if (DesktopShared.Client.Location.IsAssignedToUser(id))
            {
                _message = String.Format("Location is assigned to Users and cannot be deleted - {0}.", DateTime.Now);
                if (ItemCommand != null)
                    ItemCommand(false, _message);
                return;
            }

            objClientLocation.Delete();
            RebindGrid();
            
            _message = String.Format("Location has been deleted - {0}.", DateTime.Now);
            if (ItemCommand != null)
                ItemCommand(true, _message);

        }
        catch
        {
            _message = String.Format("Location is in use and cannot be deleted - {0}.", DateTime.Now);
            if (ItemCommand != null)
                ItemCommand(false, _message);
        }
    }

    /// <summary>
    /// grid on pre render
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rgClientLocation_PreRender(object sender, EventArgs e)
    {
        if (DesktopShared.Utility.BrowserHelper.IsTabletOrSmartPhone(this.Request))
        {
            rgClientLocation.MasterTableView.Columns[0].HeaderStyle.Width = Unit.Pixel(200);
            rgClientLocation.MasterTableView.Columns[1].HeaderStyle.Width = Unit.Pixel(400);
            rgClientLocation.MasterTableView.Columns[2].HeaderStyle.Width = Unit.Pixel(125);
        }

        rgClientLocation.ShowHeader = true;
        rgClientLocation.MasterTableView.PagerStyle.AlwaysVisible = true;
        rgClientLocation.MasterTableView.PagerStyle.Visible = true;

        GridCommandItem commandItem = null;
        if (rgClientLocation.MasterTableView.Items.Count > 0)
        {
            commandItem = (GridCommandItem)rgClientLocation.MasterTableView.GetItems(GridItemType.CommandItem)[0];
            commandItem.Visible = true;
        }

        #region add/edit 

        if (rgClientLocation.EditItems.Count > 0 || rgClientLocation.MasterTableView.IsItemInserted)
        {
            foreach (GridDataItem item in rgClientLocation.MasterTableView.Items)
                item.Visible = false;

            rgClientLocation.ShowHeader = false;
            
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
    protected void rgClientLocation_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (ItemCommand != null)
            ItemCommand(false, "");
    }

    #endregion

    #region public properties

    /// <summary>
    /// get/set client id
    /// </summary>
    public int? ClientIdForLocation
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