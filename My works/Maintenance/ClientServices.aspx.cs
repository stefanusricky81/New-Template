using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class Maintenance_ClientServices : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void lbClear_Click(object sender, EventArgs e)
    {
        txtClientServicesName.Text = string.Empty;

        BindGrid();
        rgClientServices.DataBind();
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        litMessage.Text = "";
        BindGrid();
        rgClientServices.DataBind();
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        if (true)
        {
            lblModalTitleAddEdit.Text = "Add/Edit Client Service";
            pnlEdit.Visible = false;
            lblIdEdit.Text = string.Empty;
            txtAddEditClientServicesName.Text = "";
            btnSubmitAdd.Visible = true;
            btnSubmitEdit.Visible = false;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "myModalAddEdit", "$('#myModalAddEdit').modal('show');", true);
        }
    }

    protected void rgClientServices_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        BindGrid();
    }

    protected void rgClientServices_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        int _auditUserId = DesktopShared.User.UserID;
        string command = e.CommandArgument.ToString().Trim();

        switch (command)
        {
            case "UpdateRecord":
                if (true)
                {
                    GridDataItem item = (GridDataItem)e.Item;

                    lblModalTitleAddEdit.Text = "Add/Edit Client Service";
                    btnSubmitAdd.Visible = false;
                    btnSubmitEdit.Visible = true;
                    lblIdEdit.Text = item["ID"].Text.Trim();
                    txtAddEditClientServicesName.Text = item["ClientServicename"].Text.Trim();
                    cbActive.Checked = Convert.ToBoolean(item["FlagActive"].Text.Trim());
                    hfEdit.Value = item["ClientServicename"].Text.Trim();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "myModalAddEdit", "$('#myModalAddEdit').modal('show');", true);
                }
                break;
            case "DeleteRecord":
                if (true)
                {

                    GridDataItem item = (GridDataItem)e.Item;

                    lblModalDeleteTitle.Text = "Delete Client Service";
                    lblModalDeleteWording.Text = "Are you sure want to delete " + item["ClientServicename"].Text.Trim() + " ? ";
                    hfDelete.Value = item["ID"].Text;

                    //if (DesktopShared.TicketDesignation.checkTicketDesignation(Convert.ToInt32(hfDelete.Value)) > 0)
                    //    DisplayMessage(String.Format("{0} has been {1}", item["CategoryNameEdit"].Text.Trim(), "Assigned"), Bootstrap.Alert.AlertType.Danger);
                    //else
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "myModalDelete", "$('#myModalDelete').modal('show');", true);
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
                if (DesktopShared.Client.ClientServices.checkDuplicateClientServices(txtAddEditClientServicesName.Text) == 0)
                {
                    if (DesktopShared.Client.ClientServices.AddClientServices(0, txtAddEditClientServicesName.Text, cbActive.Checked) == true)
                    {
                        DisplayMessage(String.Format("{0} have been {1}-{2}", txtAddEditClientServicesName.Text, "Added", DateTime.Now), Bootstrap.Alert.AlertType.Success);
                        BindGrid();
                        rgClientServices.Rebind();
                    }
                    else
                        DisplayMessage("Something went wrong", Bootstrap.Alert.AlertType.Danger);
                }
                else
                    DisplayMessage(String.Format("{0} already existed", txtAddEditClientServicesName.Text, DateTime.Now), Bootstrap.Alert.AlertType.Danger);
                break;
            case "Edit":
                bool existed = true;
                if (hfEdit.Value != txtAddEditClientServicesName.Text)
                {
                    if (DesktopShared.Client.ClientServices.checkDuplicateClientServices(txtAddEditClientServicesName.Text) == 0)
                        existed = false;
                }
                else
                    existed = false;

                if (!existed)
                {
                    if (DesktopShared.Client.ClientServices.AddClientServices(Convert.ToInt32(lblIdEdit.Text), txtAddEditClientServicesName.Text, cbActive.Checked) == true)
                    {
                        DisplayMessage(String.Format("{0} have been {1} into {2}-{3}", hfEdit.Value.ToString().Trim(), txtAddEditClientServicesName.Text, "Updated", DateTime.Now), Bootstrap.Alert.AlertType.Success);

                        BindGrid();
                        rgClientServices.Rebind();
                    }
                    else
                        DisplayMessage("Something went wrong", Bootstrap.Alert.AlertType.Danger);
                }
                else
                    DisplayMessage(String.Format("{0} already existed", txtAddEditClientServicesName.Text, DateTime.Now), Bootstrap.Alert.AlertType.Danger);
                break;

            case "Delete":
                int _deletestatuse = DesktopShared.Client.ClientServices.DeleteClientServices(Convert.ToInt32(hfDelete.Value));
                if (_deletestatuse == 0)
                {
                    DisplayMessage(String.Format("Client Service has been {0} - {1}", "Deleted", DateTime.Now), Bootstrap.Alert.AlertType.Success);

                    BindGrid();
                    rgClientServices.Rebind();
                }
                else if (_deletestatuse == 1)
                    DisplayMessage(String.Format("Client Service has been {0} - {1}", "Assigned", DateTime.Now), Bootstrap.Alert.AlertType.Danger);
                else if (_deletestatuse == -1)
                    DisplayMessage("Something went wrong", Bootstrap.Alert.AlertType.Danger);
                break;
        }
    }

    #region Private method
    private void DisplayMessage(string message, Bootstrap.Alert.AlertType alertType, bool dismissable = true, string overrideHeader = "")
    {
        ScriptManager.RegisterClientScriptBlock(this, Page.GetType(), "ToTheTop", "ToTopOfPage();", true);
        litMessage.Visible = true;
        litMessage.Text = Bootstrap.Alert.GetDivText(alertType, message, dismissable, true, overrideHeader);
    }
    private void BindGrid()
    {
        rgClientServices.DataSource = DesktopShared.Client.ClientServices.GetClientServices(txtClientServicesName.Text.Trim());
    }
    #endregion
}