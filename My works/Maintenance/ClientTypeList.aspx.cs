using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class Maintenance_ClientTypeList : System.Web.UI.Page
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
        string err = string.Empty;

        rgClientTypeLists.DataSource = DesktopShared.ClientTypeLists.Search(txtSeachClientType.Text.Trim(), cbSearchActive.Checked, ref err);
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        BindGrid();
        rgClientTypeLists.DataBind();
    }

    protected void lbClear_Click(object sender, EventArgs e)
    {
        Response.Redirect("ClientTypeList.aspx");
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        if (true)
        {
            lblModalTitleAddEdit.Text = "Add/Edit Client Type";
            btnAddBillingSKU.Visible = true;
            btnEditBillingSKU.Visible = false;
            txtClientType.Text = string.Empty;
            cbActive.Checked = true;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "myModalAddEdit", "$('#myModalAddEdit').modal('show');", true);
        }
    }

    protected void rgClientTypeLists_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        BindGrid();
    }

    protected void rgClientTypeLists_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        int _auditUserId = DesktopShared.User.UserID;
        string command = e.CommandArgument.ToString().Trim();
        string err = string.Empty;

        switch (command)
        {
            case "UpdateRecord":
                if (true)
                {
                    GridDataItem item = (GridDataItem)e.Item;

                    lblModalTitleAddEdit.Text = "Add/Edit Client Type";
                    btnAddBillingSKU.Visible = false;
                    btnEditBillingSKU.Visible = true;
                    hfId.Value = item["Id"].Text.Trim();
                    txtClientType.Text = item["Client_type"].Text.Trim();
                    hfOldClientType.Value = item["Client_type"].Text.Trim();
                    cbActive.Checked = Convert.ToBoolean(item["Active"].Text.Trim());
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "myModalAddEdit", "$('#myModalAddEdit').modal('show');", true);
                }
                break;
            case "DeleteRecord":
                if (true)
                {
                    GridDataItem item = (GridDataItem)e.Item;
                    int skucategoryid = Convert.ToInt32(item["id"].Text.Trim().ToString());
                    lblModalDeleteTitle.Text = "Delete Client Type";
                    lblModalDeleteWording.Text = "Are you sure want to delete this type : " + item["Client_type"].Text.Trim() + " ? ";


                    hfDelete.Value = skucategoryid.ToString();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "myModalDelete", "$('#myModalDelete').modal('show');", true);
                }
                break;
        }
    }

    protected void Decision_Command(object sender, CommandEventArgs e)
    {
        int _auditUserId = DesktopShared.User.UserID;
        string str = e.CommandArgument.ToString().Trim();
        string err = string.Empty;
        int _check = 0;

        switch (str)
        {
            case "Add":
                _check = DesktopShared.ClientTypeLists.checkDuplicateClientType(txtClientType.Text.Trim(), ref err);
                if (_check == 0)
                {
                    if (DesktopShared.ClientTypeLists.AddClientType(0, txtClientType.Text.Trim(), cbActive.Checked, _auditUserId, ref err) == true)
                    {
                        DisplayMessage(String.Format("Client type has been {0} - {1}", "Added", DateTime.Now), Bootstrap.Alert.AlertType.Success);
                        BindGrid();
                        rgClientTypeLists.DataBind();
                    }
                    else
                        DisplayMessage(err, Bootstrap.Alert.AlertType.Danger);
                }
                else if (_check == -1)
                    DisplayMessage(err, Bootstrap.Alert.AlertType.Danger);
                else
                    DisplayMessage("Client type already existed", Bootstrap.Alert.AlertType.Danger);
                break;
            case "Edit":
                if (hfOldClientType.Value.Trim().ToLower() != txtClientType.Text.Trim().ToLower())
                {
                    _check = DesktopShared.ClientTypeLists.checkDuplicateClientType(txtClientType.Text, ref err);
                    if (_check > 0)
                    {
                        DisplayMessage("Client type already existed", Bootstrap.Alert.AlertType.Danger);
                        break;
                    }
                    else if (_check == -1)
                    {
                        DisplayMessage(err, Bootstrap.Alert.AlertType.Danger);
                        break;
                    }
                }
                if (DesktopShared.ClientTypeLists.AddClientType(Convert.ToInt32(hfId.Value), txtClientType.Text, cbActive.Checked, _auditUserId, ref err) == true)
                {
                    DisplayMessage(String.Format("Client type has been {0} - {1}", "Edited", DateTime.Now), Bootstrap.Alert.AlertType.Success);
                    BindGrid();
                    rgClientTypeLists.DataBind();
                }
                else
                    DisplayMessage(err, Bootstrap.Alert.AlertType.Danger);
                break;
            case "Delete":

                if (DesktopShared.ClientTypeLists.DeleteClientType(Convert.ToInt32(hfDelete.Value), ref err) == true)
                {
                    DisplayMessage(String.Format("Client Type has been {0} - {1}", "Deleted", DateTime.Now), Bootstrap.Alert.AlertType.Success);
                    BindGrid();
                    rgClientTypeLists.DataBind();
                }
                else
                    DisplayMessage(err, Bootstrap.Alert.AlertType.Danger);

                break;
        }

    }
}