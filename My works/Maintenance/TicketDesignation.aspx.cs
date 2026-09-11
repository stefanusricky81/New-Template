using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class Maintenance_TicketDesignation : System.Web.UI.Page
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
        rgTicketDesignation.DataSource = DesktopShared.TicketDesignation.Search(txtDescription.Text.Trim());
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        BindGrid();
        rgTicketDesignation.DataBind();
    }

    protected void lbClear_Click(object sender, EventArgs e)
    {
        txtDescription.Text = string.Empty;

        BindGrid();
        rgTicketDesignation.DataBind();
    }

    protected void rgTicketDesignation_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        BindGrid();
    }

    protected void rgTicketDesignation_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        int _auditUserId = DesktopShared.User.UserID;
        string command = e.CommandArgument.ToString().Trim();

        switch (command)
        {
            case "UpdateRecord":
                if (true)
                {
                    GridDataItem item = (GridDataItem)e.Item;

                    //pnlEdit.Visible = true;
                    lblModalTitleAddEdit.Text = "Add/Edit Ticket Designation";
                    btnAddTrunk.Visible = false;
                    btnEditTrunk.Visible = true;
                    lblIdEdit.Text = item["id"].Text.Trim();
                    txtDesc.Text = item["Designation"].Text.Trim();
                    hfEdit.Value = item["Designation"].Text.Trim();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "myModalAddEdit", "$('#myModalAddEdit').modal('show');", true);
                }
                break;
            case "DeleteRecord":
                if (true)
                {

                    GridDataItem item = (GridDataItem)e.Item;

                    lblModalDeleteTitle.Text = "Delete Ticket Designation";
                    lblModalDeleteWording.Text = "Are you sure want to delete " + item["Designation"].Text.Trim() + " ? ";
                    hfDelete.Value = item["id"].Text;

                    if (DesktopShared.TicketDesignation.checkTicketDesignation(Convert.ToInt32(hfDelete.Value)) > 0)
                        DisplayMessage(String.Format("{0} has been {1}", item["Designation"].Text.Trim(), "Assigned"), Bootstrap.Alert.AlertType.Danger);
                    else
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
                if (DesktopShared.TicketDesignation.checkDuplicateTicketDesignation(txtDesc.Text) == 0)
                {
                    if (DesktopShared.TicketDesignation.AddTicketDesignation(0, txtDesc.Text) == true)
                        _func("Ticket Designation has been {0} - {1}", "Added");
                    else
                        DisplayMessage("Something went wrong", Bootstrap.Alert.AlertType.Danger);
                }
                else
                    DisplayMessage(String.Format("{0} already existed", txtDesc.Text, DateTime.Now), Bootstrap.Alert.AlertType.Danger);
                break;
            case "Edit":
                if (hfEdit.Value != txtDesc.Text)
                {
                    if (DesktopShared.TicketDesignation.checkDuplicateTicketDesignation(txtDesc.Text) == 0)
                    {
                        if (DesktopShared.TicketDesignation.AddTicketDesignation(Convert.ToInt32(lblIdEdit.Text), txtDesc.Text) == true)
                            _func("Ticket Designation has been {0} - {1}", "Edited");
                        else
                            DisplayMessage("Something went wrong", Bootstrap.Alert.AlertType.Danger);
                    }
                    else
                        DisplayMessage(String.Format("{0} already existed", txtDesc.Text, DateTime.Now), Bootstrap.Alert.AlertType.Danger);
                }
                break;
            case "Delete":
                if (DesktopShared.TicketDesignation.DeleteTicketDesignation(Convert.ToInt32(hfDelete.Value)))
                    _func("Ticket Designation has been {0} - {1}", "Deleted");
                else
                    DisplayMessage("Something went wrong", Bootstrap.Alert.AlertType.Danger);
                break;
        }
    }

    private void _func(string wording, string statuswording)
    {
        DisplayMessage(String.Format(wording, statuswording, DateTime.Now), Bootstrap.Alert.AlertType.Success);
        BindGrid();
        rgTicketDesignation.DataBind();
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        if (true)
        {
            lblModalTitleAddEdit.Text = "Add/Edit Trunk Group";
            pnlEdit.Visible = false;
            lblIdEdit.Text = string.Empty;
            btnAddTrunk.Visible = true;
            btnEditTrunk.Visible = false;
            txtDesc.Text = string.Empty;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "myModalAddEdit", "$('#myModalAddEdit').modal('show');", true);
        }
    }
}