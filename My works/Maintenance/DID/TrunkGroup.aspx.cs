using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class Maintenance_DID_TrunkGroup : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            SetUpPage();
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
        rgTrunkGroup.DataSource = DesktopShared.Trunk.Search(txtDescription.Text.Trim());
    }

    private void SetUpPage()
    {
        txtDescription.Focus();

        #region tab index
        short _tabIndex = 0;
        txtDescription.TabIndex= ++_tabIndex;
        btnSubmit.TabIndex = ++_tabIndex;
        lbClear.TabIndex= ++_tabIndex;
        btnAdd.TabIndex= ++_tabIndex;
        #endregion
    }

    protected void rgTrunkGroup_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        BindGrid();
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        BindGrid();
        rgTrunkGroup.DataBind();
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

    protected void Decision_Command(object sender, CommandEventArgs e)
    {
        int _auditUserId = DesktopShared.User.UserID;
        string str = e.CommandArgument.ToString().Trim();

        switch (str)
        {
            case "Add":
                if (DesktopShared.Trunk.AddTrunkGroup(0, txtDesc.Text) == true)
                {
                    DisplayMessage(String.Format("Trunk group has been {0} - {1}", "Added", DateTime.Now), Bootstrap.Alert.AlertType.Success);
                    BindGrid();
                    rgTrunkGroup.DataBind();
                }
                else
                    DisplayMessage("Something went wrong", Bootstrap.Alert.AlertType.Danger);
                break;
            case "Edit":
                if (DesktopShared.Trunk.AddTrunkGroup(Convert.ToInt32(lblIdEdit.Text), txtDesc.Text) == true)
                {
                    DisplayMessage(String.Format("Trunk group has been {0} - {1}", "Edited", DateTime.Now), Bootstrap.Alert.AlertType.Success);
                    BindGrid();
                    rgTrunkGroup.DataBind();
                }
                else
                    DisplayMessage("Something went wrong", Bootstrap.Alert.AlertType.Danger);
                break;
            case "Delete":
                if (DesktopShared.Trunk.DeleteTrunkGroup(Convert.ToInt32(hfDelete.Value)))
                {
                    DisplayMessage(String.Format("Trunk group has been {0} - {1}", "Deleted", DateTime.Now), Bootstrap.Alert.AlertType.Success);
                    BindGrid();
                    rgTrunkGroup.DataBind();
                }
                else
                    DisplayMessage("Something went wrong", Bootstrap.Alert.AlertType.Danger);
                break;
        }
    }

    protected void rgTrunkGroup_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        int _auditUserId = DesktopShared.User.UserID;
        string command = e.CommandArgument.ToString().Trim();

        switch (command)
        {
            case "UpdateRecord":
                if (true)
                {
                    GridDataItem item = (GridDataItem)e.Item;

                    pnlEdit.Visible = true;
                    lblModalTitleAddEdit.Text = "Add/Edit Trunk Group";
                    btnAddTrunk.Visible = false;
                    btnEditTrunk.Visible = true;
                    lblIdEdit.Text = item["trunkgroupid"].Text.Trim();
                    txtDesc.Text = item["trunkgroupdescr"].Text.Trim();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "myModalAddEdit", "$('#myModalAddEdit').modal('show');", true);
                }
                break;
            case "DeleteRecord":
                if (true)
                {
                    
                    GridDataItem item = (GridDataItem)e.Item;

                    lblModalDeleteTitle.Text = "Delete Trunk Group";
                    lblModalDeleteWording.Text = "Are you sure want to delete " + item["trunkgroupdescr"].Text.Trim() + " ? ";
                    hfDelete.Value= item["trunkgroupid"].Text;

                    if (DesktopShared.Trunk.checkTrunk(Convert.ToInt32(hfDelete.Value)) > 0)
                        DisplayMessage(String.Format("{0} has been {1}", item["trunkgroupdescr"].Text.Trim(), "Assigned"), Bootstrap.Alert.AlertType.Danger);
                    else
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "myModalDelete", "$('#myModalDelete').modal('show');", true);
                }
                break;
        }

    }

    protected void lbClear_Click(object sender, EventArgs e)
    {
        txtDescription.Text = string.Empty;

        BindGrid();
        rgTrunkGroup.DataBind();
    }
}