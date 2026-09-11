using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class Maintenance_ClientSetupEmailMessage : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            var Email = Get();
            if (Email.Count > 0)
                btnAdd.Visible = false;
        }
    }

    protected void rgClientSetupEmail_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        rgClientSetupEmail.DataSource = Get();
    }

    private void DisplayMessage(string message, Bootstrap.Alert.AlertType alertType, bool dismissable = true, string overrideHeader = "")
    {
        ScriptManager.RegisterClientScriptBlock(this, Page.GetType(), "ToTheTop", "ToTopOfPage();", true);
        litMessage.Visible = true;
        litMessage.Text = Bootstrap.Alert.GetDivText(alertType, message, dismissable, true, overrideHeader);
    }

    private void RebindGrid()
    {
        rgClientSetupEmail.Visible = true;
        rgClientSetupEmail.EditIndexes.Clear();
        rgClientSetupEmail.DataSource = null;
        rgClientSetupEmail.Rebind();
    }

    protected void rgClientSetupEmail_UpdateCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        GridEditableItem editedItem = (GridEditableItem)e.Item;
        int id = int.Parse(editedItem.OwnerTableView.DataKeyValues[editedItem.ItemIndex]["Id"].ToString());
        UserControl_Grid_EditForm_ClientSetupEmailMessageEdit ucEditForm = e.Item.FindControl(GridEditFormItem.EditFormUserControlID) as UserControl_Grid_EditForm_ClientSetupEmailMessageEdit;
        ucEditForm.SaveValues(id);
        RebindGrid();
        DisplayMessage("Client Setup Email has been updated.", Bootstrap.Alert.AlertType.Success);
    }

    protected void rgClientSetupEmail_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        #region Item or AlternatingItem

        if (e.Item.ItemType == GridItemType.Item || e.Item.ItemType == GridItemType.AlternatingItem)
        {
            GridDataItem _gdi = (GridDataItem)e.Item;
            DesktopShared.EntityClasses.ClientSetupInvitationEmailEntity objClientSetupEmail = (DesktopShared.EntityClasses.ClientSetupInvitationEmailEntity)e.Item.DataItem;

            TelerikHelper.AddLabelToCell(DesktopShared.Utility.Html.StripHtml(objClientSetupEmail.EmailMessage.Trim()), _gdi["EmailMessage"]);

        }

        #endregion

        #region GridEditFormItem and IsInEditMode

        if (e.Item is GridEditFormItem && e.Item.IsInEditMode)
        {
            UserControl_Grid_EditForm_ClientSetupEmailMessageEdit ucEditForm = e.Item.FindControl(GridEditFormItem.EditFormUserControlID) as UserControl_Grid_EditForm_ClientSetupEmailMessageEdit;
            if (!e.Item.OwnerTableView.IsItemInserted) //edit/view
            {
                DesktopShared.EntityClasses.ClientSetupInvitationEmailEntity objClientSetupEmail = (DesktopShared.EntityClasses.ClientSetupInvitationEmailEntity)e.Item.DataItem;
                ucEditForm.LoadValues(objClientSetupEmail);
            }
        }
        if (e.Item is GridDataItem)
        {
            Telerik.Web.UI.GridDataItem item = e.Item as Telerik.Web.UI.GridDataItem;
            TableCell cell = item["EmailMessage"];
            string _newnotes = Server.HtmlDecode(item["EmailMessage"].Text).Replace("\r\n", "<br/>");
            cell.Text = _newnotes;
        }
        #endregion
    }

    protected void rgClientSetupEmail_PreRender(object sender, EventArgs e)
    {
        if (DesktopShared.Utility.BrowserHelper.IsTabletOrSmartPhone(this.Request))
        {
            int _colIndex = -1;

            rgClientSetupEmail.MasterTableView.Columns[++_colIndex].HeaderStyle.Width = Unit.Pixel(50);
            rgClientSetupEmail.MasterTableView.Columns[++_colIndex].HeaderStyle.Width = Unit.Pixel(150);
            rgClientSetupEmail.MasterTableView.Columns[++_colIndex].HeaderStyle.Width = Unit.Pixel(150);
            rgClientSetupEmail.MasterTableView.Columns[++_colIndex].HeaderStyle.Width = Unit.Pixel(150);
            rgClientSetupEmail.MasterTableView.Columns[++_colIndex].HeaderStyle.Width = Unit.Pixel(250);
            rgClientSetupEmail.MasterTableView.Columns[++_colIndex].HeaderStyle.Width = Unit.Pixel(250);
        }

        rgClientSetupEmail.ShowHeader = true;
        rgClientSetupEmail.MasterTableView.PagerStyle.AlwaysVisible = true;
        rgClientSetupEmail.MasterTableView.PagerStyle.Visible = true;

        #region add/edit

        if (rgClientSetupEmail.EditItems.Count > 0 || rgClientSetupEmail.MasterTableView.IsItemInserted)
        {
            foreach (GridDataItem item in rgClientSetupEmail.MasterTableView.Items)
                item.Visible = false;

            rgClientSetupEmail.ShowHeader = false;
        }

        #endregion
    }

    public static DesktopShared.CollectionClasses.ClientSetupInvitationEmailCollection Get(bool? active = null)
    {
        //collection
        DesktopShared.CollectionClasses.ClientSetupInvitationEmailCollection _emails = new DesktopShared.CollectionClasses.ClientSetupInvitationEmailCollection();

        //predicate
        //SD.LLBLGen.Pro.ORMSupportClasses.IPredicateExpression _emailsFilter = new SD.LLBLGen.Pro.ORMSupportClasses.PredicateExpression();
        //if (active.HasValue)
        //    _emailsFilter.Add(DesktopShared.HelperClasses.SystemEmailFields.Active == active.Value);

        //sort
        SD.LLBLGen.Pro.ORMSupportClasses.ISortExpression _emailsSort = new SD.LLBLGen.Pro.ORMSupportClasses.SortExpression();
        _emailsSort.Add(DesktopShared.HelperClasses.ClientSetupInvitationEmailFields.CreatedDate | SD.LLBLGen.Pro.ORMSupportClasses.SortOperator.Descending);

        //fetch
        _emails.GetMulti(null, 0, _emailsSort);

        //return
        return _emails;
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        if (true)
        {
            lblModalTitleAdd.Text = "Add Emergency Contact";
            txtBody.Text = string.Empty;

            ScriptManager.RegisterStartupScript(this, this.GetType(), "myModalAdd", "$('#myModalAdd').modal('show');", true);
        }
    }

    protected void Decision_Command(object sender, CommandEventArgs e)
    {
        string str = e.CommandArgument.ToString().Trim();


        switch (str)
        {
            case "Add":
                AddEmailCategory(txtBody.Text.ToString().Trim());
                break;
        }
    }

    public static bool AddEmailCategory(string message)
    {
        try
        {
            DesktopShared.EntityClasses.ClientSetupInvitationEmailEntity _clientsetupemail = new DesktopShared.EntityClasses.ClientSetupInvitationEmailEntity();

            _clientsetupemail.EmailMessage = message;
            _clientsetupemail.CreatedDate = DateTime.Now;
            _clientsetupemail.CreatedBy = DesktopShared.User.UserID;
            _clientsetupemail.Save();

            _clientsetupemail.Refetch();

            return true;
        }
        catch
        {
            return false;
        }
    }
}