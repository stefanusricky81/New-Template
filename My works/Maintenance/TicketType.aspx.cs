using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class Maintenance_TicketType : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            bindddlTicketDesignation();
        }
    }

    private void DisplayMessage(string message, Bootstrap.Alert.AlertType alertType, bool dismissable = true, string overrideHeader = "")
    {
        ScriptManager.RegisterClientScriptBlock(this, Page.GetType(), "ToTheTop", "ToTopOfPage();", true);
        litMessage.Visible = true;
        litMessage.Text = Bootstrap.Alert.GetDivText(alertType, message, dismissable, true, overrideHeader);
    }

    private void BindGrid()
    {
        rgTicketType.DataSource = DesktopShared.TicketType.Search(txtDescription.Text.Trim(), ddlTicketDesignation.TicketDesignationId == null ? 0 : Convert.ToInt32(ddlTicketDesignation.TicketDesignationId));
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        BindGrid();
        rgTicketType.DataBind();
    }

    protected void lbClear_Click(object sender, EventArgs e)
    {
        txtDescription.Text = string.Empty;
        ddlTicketDesignation.TicketDesignationId = -1;

        BindGrid();
        rgTicketType.DataBind();
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        if (true)
        {
            lblModalTitleAddEdit.Text = "Add/Edit Ticket Type";
            //pnlEdit.Visible = false;
            lblIdEdit.Text = string.Empty;
            btnAddTrunk.Visible = true;
            btnEditTrunk.Visible = false;
            txtDesc.Text = string.Empty;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "myModalAddEdit", "$('#myModalAddEdit').modal('show');", true);
        }
    }

    protected void Decision_Command(object sender, CommandEventArgs e)
    {
        int _auditUserId = DesktopShared.User.UserID;
        string str = e.CommandArgument.ToString().Trim();

        switch (str)
        {
            case "Add":
                if (DesktopShared.TicketType.checkDuplicateTicketType(txtDesc.Text) == 0)
                {
                    if (DesktopShared.TicketType.AddTicketType(0, txtDesc.Text, Convert.ToInt32(ddlAddDesignation.TicketDesignationId), _auditUserId) == true)
                        _func("Ticket Type has been {0} - {1}", "Added");
                    else
                        DisplayMessage("Something went wrong", Bootstrap.Alert.AlertType.Danger);
                }
                else
                    DisplayMessage(String.Format("{0} : Already Exists", txtDesc.Text, DateTime.Now), Bootstrap.Alert.AlertType.Danger);
                break;
            case "Edit":
                if (hfEdit.Value != txtDesc.Text)
                {
                    if (DesktopShared.TicketType.checkDuplicateTicketType(txtDesc.Text) > 0)
                    {
                        DisplayMessage(String.Format("{0} : Already Exists", txtDesc.Text, DateTime.Now), Bootstrap.Alert.AlertType.Danger);
                        break;
                    }
                }

                if (DesktopShared.TicketType.AddTicketType(Convert.ToInt32(lblIdEdit.Text), txtDesc.Text, Convert.ToInt32(ddlAddDesignation.TicketDesignationId), _auditUserId) == true)
                    _func("Ticket Type has been {0} - {1}", "Edited");
                else
                    DisplayMessage("Something went wrong", Bootstrap.Alert.AlertType.Danger);
                break;
            case "Delete":
                if (DesktopShared.TicketType.DeleteTicketType(Convert.ToInt32(hfDelete.Value)))
                    _func("Ticket Type has been {0} - {1}", "Deleted");
                else
                    DisplayMessage("Something went wrong", Bootstrap.Alert.AlertType.Danger);
                break;
        }
    }

    private void _func(string wording, string statuswording)
    {
        DisplayMessage(String.Format(wording, statuswording, DateTime.Now), Bootstrap.Alert.AlertType.Success);
        BindGrid();
        rgTicketType.DataBind();
    }

    protected void rgTicketType_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        BindGrid();
    }

    protected void rgTicketType_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
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
                    lblModalTitleAddEdit.Text = "Add/Edit Ticket Type";
                    btnAddTrunk.Visible = false;
                    btnEditTrunk.Visible = true;
                    lblIdEdit.Text = item["id"].Text.Trim();
                    txtDesc.Text = item["Name"].Text.Trim();
                    ddlAddDesignation.TicketDesignationId = item["fk_DesignationId"].Text == "&nbsp;" ? -1 : Convert.ToInt32(item["fk_DesignationId"].Text);
                    hfEdit.Value = item["Name"].Text.Trim();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "myModalAddEdit", "$('#myModalAddEdit').modal('show');", true);
                }
                break;
            case "DeleteRecord":
                if (true)
                {

                    GridDataItem item = (GridDataItem)e.Item;

                    lblModalDeleteTitle.Text = "Delete Ticket Type";
                    lblModalDeleteWording.Text = "Are you sure want to delete " + item["Name"].Text.Trim() + " ? ";
                    hfDelete.Value = item["id"].Text;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "myModalDelete", "$('#myModalDelete').modal('show');", true);
                }
                break;
        }
    }

    #region binddropdown
    private void bindddlTicketDesignation()
    {
        //ddlTicketDesignation.DataSource = DesktopShared.TicketDesignation.Search(string.Empty);
        //ddlTicketDesignation.DataTextField = "Designation";
        //ddlTicketDesignation.DataValueField = "id";
        //ddlTicketDesignation.DataBind();
        //ddlTicketDesignation.Items.Insert(0, new ListItem("Select", string.Empty));

        //ddlAddDesignation.DataSource = DesktopShared.TicketDesignation.Search(string.Empty);
        //ddlAddDesignation.DataTextField = "Designation";
        //ddlAddDesignation.DataValueField = "id";
        //ddlAddDesignation.DataBind();
        //ddlAddDesignation.Items.Insert(0, new ListItem("Select", string.Empty));
    }
    #endregion
}