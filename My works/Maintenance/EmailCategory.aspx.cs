using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class Maintenance_EmailCategory : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    #region protected method
    protected void lbClear_Click(object sender, EventArgs e)
    {
        txtemailCategory.Text = string.Empty;

        BindGrid();
        rgEmailCategory.DataBind();
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        BindGrid();
        rgEmailCategory.DataBind();
    }

    protected void rgEmailCategory_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        BindGrid();
    }

    protected void rgEmailCategory_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        int _auditUserId = DesktopShared.User.UserID;
        string command = e.CommandArgument.ToString().Trim();

        switch (command)
        {
            case "UpdateRecord":
                if (true)
                {
                    GridDataItem item = (GridDataItem)e.Item;

                    lblModalTitleAddEdit.Text = "Add/Edit Email Category";
                    btnAddEmailCategory.Visible = false;
                    btnEditEmailCategory.Visible = true;
                    lblIdEdit.Text = item["ID"].Text.Trim();
                    txtemailCategory.Text = item["CategoryNameEdit"].Text.Trim();
                    ddlAvailableFor.SelectedValue= item["AvailableForEdit"].Text.Trim();
                    ddlAvailableFor.Enabled = false;
                    cbActive.Checked = Convert.ToBoolean(item["Active"].Text.Trim());
                    hfEdit.Value = item["CategoryNameEdit"].Text.Trim();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "myModalAddEdit", "$('#myModalAddEdit').modal('show');", true);
                }
                break;
            case "DeleteRecord":
                if (true)
                {

                    GridDataItem item = (GridDataItem)e.Item;

                    lblModalDeleteTitle.Text = "Delete Email Category";
                    lblModalDeleteWording.Text = "Are you sure want to delete " + item["CategoryNameEdit"].Text.Trim() + " ? ";
                    hfDelete.Value = item["ID"].Text;

                    //if (DesktopShared.TicketDesignation.checkTicketDesignation(Convert.ToInt32(hfDelete.Value)) > 0)
                    //    DisplayMessage(String.Format("{0} has been {1}", item["CategoryNameEdit"].Text.Trim(), "Assigned"), Bootstrap.Alert.AlertType.Danger);
                    //else
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "myModalDelete", "$('#myModalDelete').modal('show');", true);
                }
                break;
            case "AssignCC":
                if (true)
                {
                    GridDataItem item = (GridDataItem)e.Item;
                    int id = Convert.ToInt32(item["ID"].Text.Trim());
                    bool active = Convert.ToBoolean(item["Active"].Text.Trim());
                    if (active)
                        //Response.Redirect(String.Format("NewClientContactSearch.aspx?ecat={0}", id));
                        //Response.Write("<script>window.open ('NewClientContactSearch.aspx?ecat=" + id + "','_blank');</script>");
                        System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "openModal", "window.open('NewClientContactSearch.aspx?ecat= " + id + "' ,'_blank');", true);
                        //Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "openModal", "window.open('NewClientContactSearch.aspx?ecat= " + id + "' ,'_blank');", true);
                    else
                        DisplayMessage("Email Category is Inactive. Cannot be assigned to Contacts", Bootstrap.Alert.AlertType.Danger);
                }
                break;
        }
    }
    protected void Decision_Command(object sender, CommandEventArgs e)
    {
        int _auditUserId = DesktopShared.User.UserID;
        string str = e.CommandArgument.ToString().Trim();

        switch (str)
        {
            case "Add":
                if (DesktopShared.ClientEmailCategory.checkDuplicateEmailCategory(txtemailCategory.Text) == 0)
                {
                    if (DesktopShared.ClientEmailCategory.AddEmailCategory(0, txtemailCategory.Text, ddlAvailableFor.SelectedValue, cbActive.Checked) == true)
                    {
                        DisplayMessage(String.Format("{0} have been {1}-{2}", txtemailCategory.Text, "Added", DateTime.Now), Bootstrap.Alert.AlertType.Success);
                        BindGrid();
                        rgEmailCategory.Rebind();
                    }
                    else
                        DisplayMessage("Something went wrong", Bootstrap.Alert.AlertType.Danger);
                }
                else
                    DisplayMessage(String.Format("{0} already existed", txtemailCategory.Text, DateTime.Now), Bootstrap.Alert.AlertType.Danger);
                break;
            case "Edit":
                bool existed = true;
                if (hfEdit.Value != txtemailCategory.Text)
                {
                    if (DesktopShared.ClientEmailCategory.checkDuplicateEmailCategory(txtemailCategory.Text) == 0)
                        existed = false;
                }
                else
                    existed = false;

                if (!existed)
                {
                    if (DesktopShared.ClientEmailCategory.AddEmailCategory(Convert.ToInt32(lblIdEdit.Text), txtemailCategory.Text, ddlAvailableFor.SelectedValue, cbActive.Checked)==true)
                    {
                        DisplayMessage(String.Format("{0} have been {1} into {2}-{3}", hfEdit.Value.ToString().Trim(), txtemailCategory.Text, "Updated", DateTime.Now), Bootstrap.Alert.AlertType.Success);

                        BindGrid();
                        rgEmailCategory.Rebind();
                    }
                    else
                        DisplayMessage("Something went wrong", Bootstrap.Alert.AlertType.Danger);
                }
                else
                    DisplayMessage(String.Format("{0} already existed", txtemailCategory.Text, DateTime.Now), Bootstrap.Alert.AlertType.Danger);
        break;
            case "Delete":
                int _deletestatuse = DesktopShared.ClientEmailCategory.DeleteClientEmailCategory(Convert.ToInt32(hfDelete.Value));
                if (_deletestatuse == 0)
                {
                    DisplayMessage(String.Format("Email Category has been {0} - {1}", "Deleted", DateTime.Now), Bootstrap.Alert.AlertType.Success);

                    BindGrid();
                    rgEmailCategory.Rebind();
                }
                else if(_deletestatuse == 1)
                    DisplayMessage(String.Format("Email Category has been {0} - {1}", "Assigned", DateTime.Now), Bootstrap.Alert.AlertType.Danger);
                else if(_deletestatuse == -1)
                    DisplayMessage("Something went wrong", Bootstrap.Alert.AlertType.Danger);
                break;
        }
    }
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        if (true)
        {
            lblModalTitleAddEdit.Text = "Add/Edit Email Category";
            pnlEdit.Visible = false;
            lblIdEdit.Text = string.Empty;
            btnAddEmailCategory.Visible = true;
            btnEditEmailCategory.Visible = false;
            txtemailCategory.Text = string.Empty;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "myModalAddEdit", "$('#myModalAddEdit').modal('show');", true);
        }
    }
    #endregion

    #region Private method
    private void DisplayMessage(string message, Bootstrap.Alert.AlertType alertType, bool dismissable = true, string overrideHeader = "")
    {
        ScriptManager.RegisterClientScriptBlock(this, Page.GetType(), "ToTheTop", "ToTopOfPage();", true);
        litMessage.Visible = true;
        litMessage.Text = Bootstrap.Alert.GetDivText(alertType, message, dismissable, true, overrideHeader);
    }
    private void BindGrid()
    {
        rgEmailCategory.DataSource = DesktopShared.ClientEmailCategory.EmailCategory(txtSearchEmailCategory.Text.Trim());
    }
    #endregion


}