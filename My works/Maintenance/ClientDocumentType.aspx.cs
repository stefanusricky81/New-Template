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
using DesktopShared.CollectionClasses;

public partial class Maintenance_ClientDocumentType : BasePage
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
            rgClientDocumentType.RenderMode = RenderMode.Lightweight;
        }
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
    /// rebind grid
    /// </summary>
    private void RebindGrid()
    {
        rgClientDocumentType.Visible = true;
        rgClientDocumentType.EditIndexes.Clear();
        rgClientDocumentType.DataSource = null;
        rgClientDocumentType.Rebind();
    }

    #endregion

    #region protected events

    #region telerik grid

    /// <summary>
    /// grid on need data source
    /// </summary>
    /// <param name="source"></param>
    /// <param name="e"></param>
    protected void rgClientDocumentType_NeedDataSource(object source, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        rgClientDocumentType.DataSource = DesktopShared.Client.Document.Type.Get();
    }

    /// <summary>
    /// grid on update command
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rgClientDocumentType_UpdateCommand(object sender, GridCommandEventArgs e)
    {
        GridEditableItem editedItem = (GridEditableItem)e.Item;
        int id = int.Parse(editedItem.OwnerTableView.DataKeyValues[editedItem.ItemIndex]["Id"].ToString());
        UserControl_Grid_EditForm_ClientDocumentTypeEdit ucEditForm = e.Item.FindControl(GridEditFormItem.EditFormUserControlID) as UserControl_Grid_EditForm_ClientDocumentTypeEdit;
        ucEditForm.SaveValues(id);
        RebindGrid();
        DisplayMessage("Client Document Type has been updated.", Bootstrap.Alert.AlertType.Success);
    }


    /// <summary>
    /// grid on insert command
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rgClientDocumentType_InsertCommand(object sender, GridCommandEventArgs e)
    {
        UserControl_Grid_EditForm_ClientDocumentTypeEdit ucEditForm = e.Item.FindControl(GridEditFormItem.EditFormUserControlID) as UserControl_Grid_EditForm_ClientDocumentTypeEdit;
        ucEditForm.SaveValues(null);
        RebindGrid();
        DisplayMessage("Client Document Type has been added.", Bootstrap.Alert.AlertType.Success);
    }

    /// <summary>
    /// grid on delete command
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rgClientDocumentType_DeleteCommand(object sender, GridCommandEventArgs e)
    {
        GridEditableItem editedItem = (GridEditableItem)e.Item;
        int id = int.Parse(editedItem.OwnerTableView.DataKeyValues[editedItem.ItemIndex]["Id"].ToString());

        if (!DesktopShared.Client.Document.Type.CanBeDeleted(id))
        {
            e.Item.Edit = false;
            RebindGrid();
            DisplayMessage("Client Document Type is in used and cannot be deleted.", Bootstrap.Alert.AlertType.Danger);
        }
        else
        {
            DesktopShared.EntityClasses.ClientDocumentTypeEntity objClientDocumentType = new DesktopShared.EntityClasses.ClientDocumentTypeEntity(id);
            objClientDocumentType.Delete();

            e.Item.Edit = false;
            RebindGrid();
            DisplayMessage("Client Document Type has been deleted.", Bootstrap.Alert.AlertType.Success);
        }
    }

    /// <summary>
    /// grid on item data bound
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rgClientDocumentType_ItemDataBound(object sender, GridItemEventArgs e)
    {
        #region Item or AlternatingItem

        if (e.Item.ItemType == GridItemType.Item || e.Item.ItemType == GridItemType.AlternatingItem)
        {
            GridDataItem _gdi = (GridDataItem)e.Item;
            DesktopShared.EntityClasses.ClientDocumentTypeEntity objClientDocumentType = (DesktopShared.EntityClasses.ClientDocumentTypeEntity)e.Item.DataItem;

            TelerikHelper.AddLabelToCell(objClientDocumentType.Name.Trim(), _gdi["Name"]);
            TelerikHelper.AddLabelToCell(objClientDocumentType.Description.Trim(), _gdi["Description"]);
            TelerikHelper.AddLabelToCell(objClientDocumentType.Active ? "Yes" : "No", _gdi["Active"]);

            if (objClientDocumentType.DisableDelete)
            {
                LinkButton btnDeleteClientDocumentType = e.Item.FindControl("btnDeleteClientDocumentType") as LinkButton;
                btnDeleteClientDocumentType.Visible = false;
            }
        }

        #endregion

        #region GridEditFormItem and IsInEditMode

        if (e.Item is GridEditFormItem && e.Item.IsInEditMode)
        {
            UserControl_Grid_EditForm_ClientDocumentTypeEdit ucEditForm = e.Item.FindControl(GridEditFormItem.EditFormUserControlID) as UserControl_Grid_EditForm_ClientDocumentTypeEdit;
            if (!e.Item.OwnerTableView.IsItemInserted) //edit/view
            {
                DesktopShared.EntityClasses.ClientDocumentTypeEntity objClientDocumentType = (DesktopShared.EntityClasses.ClientDocumentTypeEntity)e.Item.DataItem;
                ucEditForm.LoadValues(objClientDocumentType);
            }
            else
                ucEditForm.LoadValues(null);
        }

        #endregion
    }

    /// <summary>
    /// grid on pre render
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rgClientDocumentType_PreRender(object sender, EventArgs e)
    {
        if (DesktopShared.Utility.BrowserHelper.IsSmartPhone(this.Request))
        {
            rgClientDocumentType.MasterTableView.Columns[0].HeaderStyle.Width = Unit.Pixel(75);
            rgClientDocumentType.MasterTableView.Columns[1].HeaderStyle.Width = Unit.Pixel(150);
            rgClientDocumentType.MasterTableView.Columns[2].HeaderStyle.Width = Unit.Pixel(250);
            rgClientDocumentType.MasterTableView.Columns[3].HeaderStyle.Width = Unit.Pixel(75);
        }

        rgClientDocumentType.ShowHeader = true;
        rgClientDocumentType.MasterTableView.PagerStyle.AlwaysVisible = true;
        rgClientDocumentType.MasterTableView.PagerStyle.Visible = true;

        GridCommandItem commandItem = null;
        if (rgClientDocumentType.MasterTableView.Items.Count > 0)
        {
            commandItem = (GridCommandItem)rgClientDocumentType.MasterTableView.GetItems(GridItemType.CommandItem)[0];
            commandItem.Visible = true;

        }

        #region add/edit

        if (rgClientDocumentType.EditItems.Count > 0 || rgClientDocumentType.MasterTableView.IsItemInserted)
        {
            //hide all other line items while in edit mode
            foreach (GridDataItem item in rgClientDocumentType.MasterTableView.Items)
                item.Visible = false;

            rgClientDocumentType.ShowHeader = false;
            //rgClientDocumentType.MasterTableView.PagerStyle.AlwaysVisible = false;
            //rgClientDocumentType.MasterTableView.PagerStyle.Visible = false;

            if (commandItem != null)
                commandItem.Visible = false; //hide command template
        }

        #endregion

    }

    #endregion

    #endregion
}