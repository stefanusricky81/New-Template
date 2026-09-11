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

public partial class Maintenance_SystemEmail : BasePage
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
            rgSystemEmail.RenderMode = RenderMode.Lightweight;
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
        rgSystemEmail.Visible = true;
        rgSystemEmail.EditIndexes.Clear();
        rgSystemEmail.DataSource = null;
        rgSystemEmail.Rebind();
    }

    #endregion

    #region protected events

    #region telerik grid

    /// <summary>
    /// grid on need data source
    /// </summary>
    /// <param name="source"></param>
    /// <param name="e"></param>
    protected void rgSystemEmail_NeedDataSource(object source, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        rgSystemEmail.DataSource = DesktopShared.SystemEmail.Get();
    }

    /// <summary>
    /// grid on update command
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rgSystemEmail_UpdateCommand(object sender, GridCommandEventArgs e)
    {
        GridEditableItem editedItem = (GridEditableItem)e.Item;
        int id = int.Parse(editedItem.OwnerTableView.DataKeyValues[editedItem.ItemIndex]["Id"].ToString());
        UserControl_Grid_EditForm_SystemEmailEdit ucEditForm = e.Item.FindControl(GridEditFormItem.EditFormUserControlID) as UserControl_Grid_EditForm_SystemEmailEdit;
        ucEditForm.SaveValues(id);
        RebindGrid();
        DisplayMessage("System Email has been updated.", Bootstrap.Alert.AlertType.Success);
    }


    /// <summary>
    /// grid on item data bound
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rgSystemEmail_ItemDataBound(object sender, GridItemEventArgs e)
    {
        #region Item or AlternatingItem

        if (e.Item.ItemType == GridItemType.Item || e.Item.ItemType == GridItemType.AlternatingItem)
        {
            GridDataItem _gdi = (GridDataItem)e.Item;
            DesktopShared.EntityClasses.SystemEmailEntity objSystemEmail = (DesktopShared.EntityClasses.SystemEmailEntity)e.Item.DataItem;

            TelerikHelper.AddLabelToCell(objSystemEmail.Name.Trim(), _gdi["Name"]);
            TelerikHelper.AddLabelToCell(objSystemEmail.Subject.Trim(), _gdi["Subject"]);
            TelerikHelper.AddLabelToCell(objSystemEmail.From.Trim(), _gdi["From"]);
            TelerikHelper.AddLabelToCell(DesktopShared.Utility.Html.StripHtml(objSystemEmail.Body.Trim()), _gdi["Body"]);
            TelerikHelper.AddLabelToCell(objSystemEmail.Description.Trim(), _gdi["Description"]);
            
            if (!objSystemEmail.AllowEdit)
            {
                LinkButton btnEditSystemEmail = e.Item.FindControl("btnEditSystemEmail") as LinkButton;
                btnEditSystemEmail.Visible = false;
            }
        }

        #endregion

        #region GridEditFormItem and IsInEditMode

        if (e.Item is GridEditFormItem && e.Item.IsInEditMode)
        {
            UserControl_Grid_EditForm_SystemEmailEdit ucEditForm = e.Item.FindControl(GridEditFormItem.EditFormUserControlID) as UserControl_Grid_EditForm_SystemEmailEdit;
            if (!e.Item.OwnerTableView.IsItemInserted) //edit/view
            {
                DesktopShared.EntityClasses.SystemEmailEntity objSystemEmail = (DesktopShared.EntityClasses.SystemEmailEntity)e.Item.DataItem;
                ucEditForm.LoadValues(objSystemEmail);
            }
        }

        #endregion
    }

    /// <summary>
    /// grid on pre render
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rgSystemEmail_PreRender(object sender, EventArgs e)
    {
        if (DesktopShared.Utility.BrowserHelper.IsTabletOrSmartPhone(this.Request))
        {
            int _colIndex = -1;

            rgSystemEmail.MasterTableView.Columns[++_colIndex].HeaderStyle.Width = Unit.Pixel(50);
            rgSystemEmail.MasterTableView.Columns[++_colIndex].HeaderStyle.Width = Unit.Pixel(150);
            rgSystemEmail.MasterTableView.Columns[++_colIndex].HeaderStyle.Width = Unit.Pixel(150);
            rgSystemEmail.MasterTableView.Columns[++_colIndex].HeaderStyle.Width = Unit.Pixel(150);
            rgSystemEmail.MasterTableView.Columns[++_colIndex].HeaderStyle.Width = Unit.Pixel(250);
            rgSystemEmail.MasterTableView.Columns[++_colIndex].HeaderStyle.Width = Unit.Pixel(250);
        }

        rgSystemEmail.ShowHeader = true;
        rgSystemEmail.MasterTableView.PagerStyle.AlwaysVisible = true;
        rgSystemEmail.MasterTableView.PagerStyle.Visible = true;

        #region add/edit

        if (rgSystemEmail.EditItems.Count > 0 || rgSystemEmail.MasterTableView.IsItemInserted)
        {
            foreach (GridDataItem item in rgSystemEmail.MasterTableView.Items)
                item.Visible = false;

            rgSystemEmail.ShowHeader = false;
        }

        #endregion

    }

    #endregion

    #endregion
}