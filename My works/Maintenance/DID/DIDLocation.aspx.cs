using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class Maintenance_DID_DIDLocation : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            bindddlState();
            SetUpPage();
        }
    }

    private void DisplayMessage(string message, Bootstrap.Alert.AlertType alertType, bool dismissable = true, string overrideHeader = "")
    {
        ScriptManager.RegisterClientScriptBlock(this, Page.GetType(), "ToTheTop", "ToTopOfPage();", true);
        litMessage.Visible = true;
        litMessage.Text = Bootstrap.Alert.GetDivText(alertType, message, dismissable, true, overrideHeader);
    }

    private void bindddlState()
    {
        ddlState.DataSource = DesktopShared.UsState.Get();
        ddlState.DataTextField = "Name";
        ddlState.DataValueField = "Pstate";
        ddlState.DataBind();
        ddlState.Items.Insert(0, new ListItem("Select all", string.Empty));

        ddlAddState.DataSource = DesktopShared.UsState.Get();
        ddlAddState.DataTextField = "Name";
        ddlAddState.DataValueField = "Pstate";
        ddlAddState.DataBind();

        //ddlState.DataSource = null;
    }

    private void BindGrid()
    {
        rgDidLocation.DataSource = DesktopShared.DidLocation.Search(txtLocName.Text.Trim(), txtCity.Text.Trim(), ddlState.SelectedValue, txtZip.Text.Trim(), txtServiceId.Text.Trim(), txtAreCode.Text.Trim());
    }

    private void SetUpPage()
    {
        txtLocName.Focus();

        #region tab index
        short _tabIndex = 0;
        txtLocName.TabIndex = ++_tabIndex;
        txtCity.TabIndex = ++_tabIndex;
        ddlState.TabIndex = ++_tabIndex;
        txtZip.TabIndex = ++_tabIndex;
        txtServiceId.TabIndex = ++_tabIndex;
        txtAreCode.TabIndex = ++_tabIndex;
        btnSubmit.TabIndex = ++_tabIndex;
        lbClear.TabIndex = ++_tabIndex;
        btnAdd.TabIndex = ++_tabIndex;
        #endregion
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        BindGrid();
        rgDidLocation.DataBind();
    }

    protected void rgDidLocation_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        BindGrid();
    }

    protected void rgDidLocation_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        int _auditUserId = DesktopShared.User.UserID;
        string command = e.CommandArgument.ToString().Trim();

        switch (command)
        {
            case "UpdateRecord":
                if (true)
                {
                    GridDataItem item = (GridDataItem)e.Item;
                    bindddlState();
                    pnlEdit.Visible = true;
                    lblModalTitleAddEdit.Text = "Add/Edit DID Location";
                    btnAddTrunk.Visible = false;
                    btnEditTrunk.Visible = true;
                    lblIdEdit.Text = item["didlocationid"].Text;

                    txtAddLocName.Text = item["locationname"].Text == "&nbsp;" ? string.Empty : item["locationname"].Text.Trim();
                    txtAddrLine1.Text = item["addr1"].Text == "&nbsp;" ? string.Empty : item["addr1"].Text.Trim();
                    txtAddrLine2.Text = item["addr2"].Text == "&nbsp;" ? string.Empty : item["addr2"].Text.Trim();
                    txtAddCity.Text = item["city"].Text == "&nbsp;" ? string.Empty : item["city"].Text.Trim();
                    txtAddZip.Text = item["zip"].Text == "&nbsp;" ? string.Empty : item["zip"].Text.Trim();
                    txtAddServiceID.Text = item["serviceid"].Text == "&nbsp;" ? string.Empty : item["serviceid"].Text.Trim();
                    txtAreaCode.Text = item["areacode"].Text == "&nbsp;" ? string.Empty : item["areacode"].Text.Trim();
                    ddlAddState.SelectedValue = item["state"].Text.Trim();

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "myModalAddEdit", "$('#myModalAddEdit').modal('show');", true);
                }
                break;
            case "DeleteRecord":
                if (true)
                {
                    GridDataItem item = (GridDataItem)e.Item;

                    lblModalDeleteTitle.Text = "Delete DID Location";
                    lblModalDeleteWording.Text = "Are you sure want to delete " + item["locationname"].Text.Trim() + " ? ";
                    hfDelete.Value = item["didlocationid"].Text;
                    
                    if (DesktopShared.DidLocation.checkDIDLocation(Convert.ToInt32(hfDelete.Value)) > 0)
                        DisplayMessage(String.Format("{0} has been {1}", item["locationname"].Text.Trim(), "Assigned"), Bootstrap.Alert.AlertType.Danger);
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
                if (DesktopShared.DidLocation.AddDIDLocation(0, txtAddLocName.Text, txtAddrLine1.Text, txtAddrLine2.Text, txtAddCity.Text, ddlAddState.SelectedValue, txtAddZip.Text, txtAddServiceID.Text, txtAreaCode.Text) == true)
                {
                    DisplayMessage(String.Format("DID location has been {0} - {1}", "Added", DateTime.Now), Bootstrap.Alert.AlertType.Success);
                    BindGrid();
                    rgDidLocation.DataBind();
                }
                else
                    DisplayMessage("Something went wrong", Bootstrap.Alert.AlertType.Danger);
                break;
            case "Edit":
                if (DesktopShared.DidLocation.AddDIDLocation(Convert.ToInt32(lblIdEdit.Text), txtAddLocName.Text, txtAddrLine1.Text, txtAddrLine2.Text, txtAddCity.Text, ddlAddState.SelectedValue, txtAddZip.Text, txtAddServiceID.Text, txtAreaCode.Text) == true)
                {
                    DisplayMessage(String.Format("DID location has been {0} - {1}", "Edited", DateTime.Now), Bootstrap.Alert.AlertType.Success);
                    BindGrid();
                    rgDidLocation.DataBind();
                }
                else
                    DisplayMessage("Something went wrong", Bootstrap.Alert.AlertType.Danger);
                break;
            case "Delete":
                if (DesktopShared.DidLocation.DeleteDidLocation(Convert.ToInt32(hfDelete.Value)))
                {
                    DisplayMessage(String.Format("DID Location has been {0} - {1}", "Deleted", DateTime.Now), Bootstrap.Alert.AlertType.Success);
                    BindGrid();
                    rgDidLocation.DataBind();
                }
                else
                    DisplayMessage("Something went wrong", Bootstrap.Alert.AlertType.Danger);
                break;
        }
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        if (true)
        {
            lblModalTitleAddEdit.Text = "Add/Edit DID Location";
            pnlEdit.Visible = false;
            lblIdEdit.Text = string.Empty;
            btnAddTrunk.Visible = true;
            btnEditTrunk.Visible = false;

            txtAddLocName.Text = string.Empty;
            txtAddrLine1.Text = string.Empty;
            txtAddrLine2.Text = string.Empty;
            txtAddCity.Text = string.Empty;
            txtAddZip.Text = string.Empty;
            txtAddServiceID.Text = string.Empty;
            txtAreaCode.Text = string.Empty;

            ScriptManager.RegisterStartupScript(this, this.GetType(), "myModalAddEdit", "$('#myModalAddEdit').modal('show');", true);
        }
    }

    protected void lbClear_Click(object sender, EventArgs e)
    {
        txtLocName.Text = string.Empty;
        txtCity.Text = string.Empty;
        ddlState.SelectedValue = string.Empty;
        txtZip.Text = string.Empty;
        txtServiceId.Text = string.Empty;
        txtAreCode.Text = string.Empty;

        BindGrid();
        rgDidLocation.DataBind();
    }
}