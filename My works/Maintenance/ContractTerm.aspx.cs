using SD.LLBLGen.Pro.ORMSupportClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class Maintenance_ContractTerm : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        BindGrid();
        rgContractTerms.DataBind();
    }

    protected void lbClear_Click(object sender, EventArgs e)
    {
        Response.Redirect("ContractTerm.aspx");
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        if (true)
        {
            lblModalTitleAddEdit.Text = "Add/Edit Contract Term";
            btnAddContractTerm.Visible = true;
            btnEditContractTerm.Visible = false;
            txtDesc.Text = string.Empty;
            hfOldDesc.Value = string.Empty;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "myModalAddEdit", "$('#myModalAddEdit').modal('show');", true);
        }
    }

    protected void rgContractTerms_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        BindGrid();
    }

    protected void rgContractTerms_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
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

                    lblModalTitleAddEdit.Text = "Add/Edit SKU Category";
                    btnAddContractTerm.Visible = false;
                    btnEditContractTerm.Visible = true;
                    hfId.Value = item["pcontractterm"].Text.Trim();
                    txtDesc.Text = item["Description"].Text.Trim();
                    hfOldDesc.Value = item["Description"].Text.Trim();
                    hfTermCode.Value = item["code"].Text.Trim();
                    txtTermCode.Text = item["code"].Text.Trim();
                    cbActive.Checked = Convert.ToBoolean(item["Active"].Text.Trim());
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "myModalAddEdit", "$('#myModalAddEdit').modal('show');", true);
                }
                break;
            case "DeleteRecord":
                if (true)
                {
                    GridDataItem item = (GridDataItem)e.Item;
                    int contracttermid = Convert.ToInt32(item["pcontractterm"].Text.Trim().ToString());
                    lblModalDeleteTitle.Text = "Delete Category";
                    lblModalDeleteWording.Text = "Are you sure want to delete this Category : " + item["Description"].Text.Trim() + " ? ";

                    hfDelete.Value = contracttermid.ToString();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "myModalDelete", "$('#myModalDelete').modal('show');", true);
                }
                break;
        }
    }

    private void BindGrid()
    { 
        string err = "";
        rgContractTerms.DataSource = DesktopShared.ContractTerm.Get(txtSeachDesc.Text.Trim(),cbActive.Checked, ref err);
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
                _check = DesktopShared.ContractTerm.checkDuplicateTermCode(txtTermCode.Text.Trim(), ref err);
                if (_check > 0)
                {
                    DisplayMessage("Term code already exist", Bootstrap.Alert.AlertType.Danger);
                    break;
                }

                if (_check == 0)
                {
                    if (DesktopShared.ContractTerm.AddContactTerm(0, txtDesc.Text.Trim(),txtTermCode.Text.Trim(), cbActive.Checked, _auditUserId, ref err) == true)
                    {
                        DisplayMessage(String.Format("Billing SKU {0} - {1}", "Added", DateTime.Now), Bootstrap.Alert.AlertType.Success);
                        BindGrid();
                        rgContractTerms.DataBind();
                    }
                    else
                        DisplayMessage(err, Bootstrap.Alert.AlertType.Danger);
                }
                else if (_check == -1)
                    DisplayMessage(err, Bootstrap.Alert.AlertType.Danger);
               
                break;
            case "Edit":
                if (hfTermCode.Value.Trim().ToLower() != txtTermCode.Text.Trim().ToLower())
                {
                    _check = DesktopShared.ContractTerm.checkDuplicateTermCode(txtTermCode.Text.Trim(), ref err);
                    if (_check > 0)
                    {
                        DisplayMessage("Term code already exist", Bootstrap.Alert.AlertType.Danger);
                        break;
                    }
                    else if (_check == -1)
                    {
                        DisplayMessage(err, Bootstrap.Alert.AlertType.Danger);
                        break;
                    }
                }
                if (DesktopShared.ContractTerm.AddContactTerm(Convert.ToInt32(hfId.Value), txtDesc.Text.Trim(),txtTermCode.Text.Trim(), cbActive.Checked, _auditUserId, ref err) == true)
                {
                    DisplayMessage(String.Format("Contract term has been {0} - {1}", "Edited", DateTime.Now), Bootstrap.Alert.AlertType.Success);
                    BindGrid();
                    rgContractTerms.DataBind();
                }
                else
                    DisplayMessage(err, Bootstrap.Alert.AlertType.Danger);
                break;
            case "Delete":

                if (DesktopShared.ContractTerm.DeleteBillingSKU(Convert.ToInt32(hfDelete.Value), ref err) == true)
                {
                    DisplayMessage(String.Format("Contract terms has been {0} - {1}", "Deleted", DateTime.Now), Bootstrap.Alert.AlertType.Success);
                    BindGrid();
                    rgContractTerms.DataBind();
                }
                else
                    DisplayMessage(err, Bootstrap.Alert.AlertType.Danger);

                break;
        }

    }
    private void DisplayMessage(string message, Bootstrap.Alert.AlertType alertType, bool dismissable = true, string overrideHeader = "")
    {
        ScriptManager.RegisterClientScriptBlock(this, Page.GetType(), "ToTheTop", "ToTopOfPage();", true);
        litMessage.Visible = true;
        litMessage.Text = Bootstrap.Alert.GetDivText(alertType, message, dismissable, true, overrideHeader);
    }
}