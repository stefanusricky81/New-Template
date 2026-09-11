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

public partial class Maintenance_FrequentComment : BasePage
{
    /// <summary>
    /// page load
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        DropDownList _ddl = ddlFrequentCommentType.GetDropDownList();
        _ddl.AutoPostBack = true;
        _ddl.SelectedIndexChanged += ddlFrequentCommentType_SelectedIndexChanged;

        ConfigureForDevice();
        if (!this.IsPostBack)
        {
            SetUpPage();
        }
    }

    #region private methods

    /// <summary>
    /// configure for device
    /// </summary>
    private void ConfigureForDevice()
    {
        if (DesktopShared.Utility.BrowserHelper.IsTabletOrSmartPhone(this.Request))
        {
            rgFrequentCommentValue.RenderMode = RenderMode.Lightweight;
            ddlFrequentCommentType.DisplayChosenScript = false;
            ddlFrequentCommentType.CssClass = "form-control";
        }
        else
        {
            ddlFrequentCommentType.DisplayChosenScript = true;
        }
    }

    /// <summary>
    /// set up for initial page load
    /// </summary>
    private void SetUpPage()
    {

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
        rgFrequentCommentValue.Visible = true;
        rgFrequentCommentValue.EditIndexes.Clear();
        rgFrequentCommentValue.DataSource = null;
        rgFrequentCommentValue.Rebind();
    }

    #endregion

    #region protected events

    /// <summary>
    /// exam comment type on drop list selected index changed
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlFrequentCommentType_SelectedIndexChanged(object sender, EventArgs e)
    {
        RebindGrid();
    }

    #region telerik grid

    /// <summary>
    /// grid on need data source
    /// </summary>
    /// <param name="source"></param>
    /// <param name="e"></param>
    protected void rgFrequentCommentValue_NeedDataSource(object source, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        rgFrequentCommentValue.DataSource = DesktopShared.FrequentComment.Value.GetTypedList(null, ddlFrequentCommentType.FrequentCommentTypeId);
    }

    /// <summary>
    /// grid on update command
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rgFrequentCommentValue_UpdateCommand(object sender, GridCommandEventArgs e)
    {
        GridEditableItem editedItem = (GridEditableItem)e.Item;
        int id = int.Parse(editedItem.OwnerTableView.DataKeyValues[editedItem.ItemIndex]["Id"].ToString());
        UserControl_Grid_EditForm_FrequentCommentValueEdit ucEditForm = e.Item.FindControl(GridEditFormItem.EditFormUserControlID) as UserControl_Grid_EditForm_FrequentCommentValueEdit;
        ucEditForm.SaveValues(id);
        RebindGrid();
        DisplayMessage("Comment has been updated.", Bootstrap.Alert.AlertType.Success);
    }


    /// <summary>
    /// grid on insert command
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rgFrequentCommentValue_InsertCommand(object sender, GridCommandEventArgs e)
    {
        UserControl_Grid_EditForm_FrequentCommentValueEdit ucEditForm = e.Item.FindControl(GridEditFormItem.EditFormUserControlID) as UserControl_Grid_EditForm_FrequentCommentValueEdit;
        ucEditForm.SaveValues(null);
        RebindGrid();
        DisplayMessage("Comment has been added.", Bootstrap.Alert.AlertType.Success);
    }

    /// <summary>
    /// grid on delete command
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rgFrequentCommentValue_DeleteCommand(object sender, GridCommandEventArgs e)
    {
        GridEditableItem editedItem = (GridEditableItem)e.Item;
        int id = int.Parse(editedItem.OwnerTableView.DataKeyValues[editedItem.ItemIndex]["Id"].ToString());

        DesktopShared.EntityClasses.FrequentCommentValueEntity objFrequentCommentValue = new DesktopShared.EntityClasses.FrequentCommentValueEntity(id);
        objFrequentCommentValue.Delete();

        e.Item.Edit = false;
        RebindGrid();
        DisplayMessage("Comment has been deleted.", Bootstrap.Alert.AlertType.Success);
    }

    /// <summary>
    /// grid on item data bound
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rgFrequentCommentValue_ItemDataBound(object sender, GridItemEventArgs e)
    {
        #region Item or AlternatingItem

        if (e.Item.ItemType == GridItemType.Item || e.Item.ItemType == GridItemType.AlternatingItem)
        {
            GridDataItem _gdi = (GridDataItem)e.Item;
            DesktopShared.TypedListClasses.FrequentCommentValuesRow objFrequentCommentValue = (DesktopShared.TypedListClasses.FrequentCommentValuesRow)((DataRowView)e.Item.DataItem).Row;

            TelerikHelper.AddLabelToCell(objFrequentCommentValue.Text.Trim(), _gdi["Text"]);
            TelerikHelper.AddLabelToCell(objFrequentCommentValue.FrequentCommentTypeName.Trim(), _gdi["FrequentCommentTypeName"]);
            TelerikHelper.AddLabelToCell(objFrequentCommentValue.Active ? "Yes" : "No", _gdi["Active"]);
        }

        #endregion

        #region GridEditFormItem and IsInEditMode

        if (e.Item is GridEditFormItem && e.Item.IsInEditMode)
        {
            string _commentType = ddlFrequentCommentType.FrequentCommentTypeId.HasValue ? ddlFrequentCommentType.FrequentCommentTypeName.Trim() : "";
            UserControl_Grid_EditForm_FrequentCommentValueEdit ucEditForm = e.Item.FindControl(GridEditFormItem.EditFormUserControlID) as UserControl_Grid_EditForm_FrequentCommentValueEdit;
            if (!e.Item.OwnerTableView.IsItemInserted) //edit/view
            {
                DesktopShared.TypedListClasses.FrequentCommentValuesRow objFrequentCommentValue = (DesktopShared.TypedListClasses.FrequentCommentValuesRow)((DataRowView)e.Item.DataItem).Row;
                ucEditForm.LoadValues(objFrequentCommentValue);
            }
            else
                ucEditForm.LoadValues(null, _commentType, ddlFrequentCommentType.FrequentCommentTypeId);
        }

        #endregion
    }

    /// <summary>
    /// grid on pre render
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rgFrequentCommentValue_PreRender(object sender, EventArgs e)
    {
        if (DesktopShared.Utility.BrowserHelper.IsSmartPhone(this.Request))
        {
            rgFrequentCommentValue.MasterTableView.Columns[0].HeaderStyle.Width = Unit.Pixel(75);
            rgFrequentCommentValue.MasterTableView.Columns[1].HeaderStyle.Width = Unit.Pixel(250);
            rgFrequentCommentValue.MasterTableView.Columns[2].HeaderStyle.Width = Unit.Pixel(150);
            rgFrequentCommentValue.MasterTableView.Columns[3].HeaderStyle.Width = Unit.Pixel(75);
        }

        rgFrequentCommentValue.ShowHeader = true;
        rgFrequentCommentValue.MasterTableView.PagerStyle.AlwaysVisible = true;
        rgFrequentCommentValue.MasterTableView.PagerStyle.Visible = true;

        GridCommandItem commandItem = null;
        if (rgFrequentCommentValue.MasterTableView.Items.Count > 0)
        {
            commandItem = (GridCommandItem)rgFrequentCommentValue.MasterTableView.GetItems(GridItemType.CommandItem)[0];
            commandItem.Visible = true;

        }

        #region add/edit

        if (rgFrequentCommentValue.EditItems.Count > 0 || rgFrequentCommentValue.MasterTableView.IsItemInserted)
        {
            //hide all other line items while in edit mode
            foreach (GridDataItem item in rgFrequentCommentValue.MasterTableView.Items)
                item.Visible = false;

            rgFrequentCommentValue.ShowHeader = false;
            //rgFrequentCommentValue.MasterTableView.PagerStyle.AlwaysVisible = false;
            //rgFrequentCommentValue.MasterTableView.PagerStyle.Visible = false;

            if (commandItem != null)
                commandItem.Visible = false; //hide command template
        }

        #endregion

    }

    /// <summary>
    /// grid on item created
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rgFrequentCommentValue_ItemCreated(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridCommandItem && ddlFrequentCommentType.FrequentCommentTypeId.HasValue)
        {
            GridCommandItem commandItem = (GridCommandItem)e.Item;
            LinkButton btnAddNewRecord = (LinkButton)commandItem.FindControl("btnAddNewRecord");
            btnAddNewRecord.Text = String.Format("<i class='gi gi-circle_plus'></i></i>  Add New {0}", ddlFrequentCommentType.FrequentCommentTypeName.Trim());
        }
    }

    #endregion

    #endregion
}