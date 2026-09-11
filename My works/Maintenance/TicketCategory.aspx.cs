using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class Maintenance_TicketCategory : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    private void DisplayMessage(string message, Bootstrap.Alert.AlertType alertType, bool dismissable = true, string overrideHeader = "")
    {
        ScriptManager.RegisterClientScriptBlock(this, Page.GetType(), "ToTheTop", "ToTopOfPage();", true);
        litMessage.Visible = true;
        litMessage.Text = Bootstrap.Alert.GetDivText(alertType, message, dismissable, true, overrideHeader);
    }

    private void BindGrid()
    {
        rgTicketCategory.DataSource = DesktopShared.TicketCategories.Search(txtDescription.Text);
    }

    private void deletecategory(int ticketcategoryid)
    {
        if (DesktopShared.TicketCategories.DeleteTicketCategory(ticketcategoryid) == true)
        {
            DisplayMessage(String.Format("Ticket Category has been {0} - {1}", "Deleted", DateTime.Now), Bootstrap.Alert.AlertType.Success);
            BindGrid();
            rgTicketCategory.DataBind();
        }
        else
            DisplayMessage("Something went wrong", Bootstrap.Alert.AlertType.Danger);
    }

    private void updateinactive(int ticketcategoryid, int _userid, string _active)
    {
        if (DesktopShared.TicketCategories.UpdateActiveInactive(ticketcategoryid, _userid, _active) == true)
        {
            DisplayMessage(String.Format("Ticket Category has been {0} - {1}", "Inactive", DateTime.Now), Bootstrap.Alert.AlertType.Success);
            BindGrid();
            rgTicketCategory.DataBind();
        }
        else
            DisplayMessage("Something went wrong", Bootstrap.Alert.AlertType.Danger);
    }

    protected void Decision_Command(object sender, CommandEventArgs e)
    {
        int _auditUserId = DesktopShared.User.UserID;
        string str = e.CommandArgument.ToString().Trim();

        switch (str)
        {
            case "Add":
                if (DesktopShared.TicketCategories.checkDuplicateTicketCategory(txtCategoryName.Text) == 0)
                {
                    if (DesktopShared.TicketCategories.AddTicketCategory(0, txtCategoryName.Text, _auditUserId, cbDefault.Checked == true ? "Y" : "N", cbActive.Checked == true ? "Y" : "N") == true)
                    {
                        DisplayMessage(String.Format("Ticket Category {0} - {1}", "Added", DateTime.Now), Bootstrap.Alert.AlertType.Success);
                        BindGrid();
                        rgTicketCategory.DataBind();
                    }
                    else
                        DisplayMessage("Something went wrong", Bootstrap.Alert.AlertType.Danger);
                }
                else
                    DisplayMessage("Category name already existed", Bootstrap.Alert.AlertType.Danger);
                break;
            case "Edit":
                if (hfOldCategoryName.Value.Trim().ToLower() != txtCategoryName.Text.Trim().ToLower())
                {
                    if (DesktopShared.TicketCategories.checkDuplicateTicketCategory(txtCategoryName.Text) > 0)
                    {
                        DisplayMessage("Category name already existed", Bootstrap.Alert.AlertType.Danger);
                        break;
                    }
                }
                if (DesktopShared.TicketCategories.AddTicketCategory(Convert.ToInt32(hfId.Value), txtCategoryName.Text, _auditUserId, cbDefault.Checked == true ? "Y" : "N", cbActive.Checked == true ? "Y" : "N") == true)
                {
                    DisplayMessage(String.Format("Ticket Category has been {0} - {1}", "Edited", DateTime.Now), Bootstrap.Alert.AlertType.Success);
                    BindGrid();
                    rgTicketCategory.DataBind();
                }
                else
                    DisplayMessage("Something went wrong", Bootstrap.Alert.AlertType.Danger);
                break;
            case "Delete":

                if (DesktopShared.ClientTicketCategory.checkAssignedCategory(Convert.ToInt32(hfDelete.Value)) > 0)
                    updateinactive(Convert.ToInt32(hfDelete.Value), _auditUserId, "N");
                else if (DesktopShared.ClientTicketCategory.checkCategoryinTicket(Convert.ToInt32(hfDelete.Value)) > 0)
                    updateinactive(Convert.ToInt32(hfDelete.Value), _auditUserId, "N");
                else
                    deletecategory(Convert.ToInt32(hfDelete.Value));
                    //if (DesktopShared.ClientTicketCategory.checkAssignedCategory(Convert.ToInt32(hfDelete.Value)) > 0)
                    //{
                    //    if (DesktopShared.ClientTicketCategory.DeleteTicketCategory(Convert.ToInt32(hfDelete.Value)) == true)
                    //    {
                    //        DisplayMessage(String.Format("Ticket Category has been {0} - {1}", "Deleted", DateTime.Now), Bootstrap.Alert.AlertType.Success);
                    //        BindGrid();
                    //        rgTicketCategory.DataBind();
                    //    }
                    //    else
                    //        DisplayMessage("Something went wrong", Bootstrap.Alert.AlertType.Danger);
                    //    deletecategory(Convert.ToInt32(hfDelete.Value));
                    //}
                    //else
                    //{
                    //    deletecategory(Convert.ToInt32(hfDelete.Value));
                    //}
                    break;
        }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        BindGrid();
        rgTicketCategory.DataBind();
    }

    protected void lbClear_Click(object sender, EventArgs e)
    {
        txtDescription.Text = string.Empty;

        BindGrid();
        rgTicketCategory.DataBind();
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        if (true)
        {
            lblModalTitleAddEdit.Text = "Add/Edit Ticket Category";
            btnAddTicketCategory.Visible = true;
            btnEditTicketCategory.Visible = false;
            txtCategoryName.Text = string.Empty;
            cbDefault.Checked = true;
            cbActive.Checked = true;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "myModalAddEdit", "$('#myModalAddEdit').modal('show');", true);
        }
    }

    protected void rgTicketCategory_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        BindGrid();
    }

    protected void rgTicketCategory_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        int _auditUserId = DesktopShared.User.UserID;
        string command = e.CommandArgument.ToString().Trim();

        switch (command)
        {
            case "UpdateRecord":
                if (true)
                {
                    GridDataItem item = (GridDataItem)e.Item;

                    lblModalTitleAddEdit.Text = "Add/Edit Email Black List";
                    btnAddTicketCategory.Visible = false;
                    btnEditTicketCategory.Visible = true;
                    hfId.Value = item["Id"].Text.Trim();
                    txtCategoryName.Text = item["CategoryName"].Text.Trim();
                    hfOldCategoryName.Value = item["CategoryName"].Text.Trim();
                    cbDefault.Checked = item["CategoryDefault"].Text.Trim() == "Y" ? true : false;
                    cbActive.Checked = item["Active"].Text.Trim() == "Y" ? true : false;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "myModalAddEdit", "$('#myModalAddEdit').modal('show');", true);
                }
                break;
            case "DeleteRecord":
                if (true)
                {
                    GridDataItem item = (GridDataItem)e.Item;
                    int ticketcategoryid = Convert.ToInt32(item["id"].Text.Trim().ToString());

                    if (DesktopShared.ClientTicketCategory.checkAssignedCategory(ticketcategoryid) > 0)
                    {
                        lblModalDeleteTitle.Text = "Inactive Category";
                        btnYes.Text = "Submit";
                        lblModalDeleteWording.Text = " Category " + item["CategoryName"].Text.Trim() + " is currently assigned to clients are you sure you want to delete ? ";
                    }
                    else if (DesktopShared.ClientTicketCategory.checkCategoryinTicket(ticketcategoryid) > 0)
                    {
                        lblModalDeleteTitle.Text = "Inactive Category";
                        btnYes.Text = "Submit";
                        lblModalDeleteWording.Text = " Category " + item["CategoryName"].Text.Trim() + " is already associated with tickets. Do you want to inactivate? ";
                    }
                    else
                    {
                        lblModalDeleteTitle.Text = "Delete Category";
                        lblModalDeleteWording.Text = "Are you sure want to delete this Category : " + item["CategoryName"].Text.Trim() + " ? ";
                    }

                    hfDelete.Value = ticketcategoryid.ToString();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "myModalDelete", "$('#myModalDelete').modal('show');", true);
                }
                break;
        }
    }
}