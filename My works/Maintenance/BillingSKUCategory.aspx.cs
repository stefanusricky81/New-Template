using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class Maintenance_BillingSKUCategory : System.Web.UI.Page
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
        rgSKUCategory.DataSource = DesktopShared.SKUCategory.Search(txtCategoryName.Text.Trim(), cbSearchActive.Checked ? true : false, ref err);
        if (err != string.Empty)
            DisplayMessage(err, Bootstrap.Alert.AlertType.Danger);
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        BindGrid();
        rgSKUCategory.DataBind();
    }

    protected void lbClear_Click(object sender, EventArgs e)
    {
        Response.Redirect("BillingSKUCategory.aspx");
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        if (true)
        {
            lblModalTitleAddEdit.Text = "Add/Edit SKU Category";
            btnAddSKUCategory.Visible = true;
            btnEditSKUCategory.Visible = false;
            txtAddCategoryName.Text = string.Empty;
            cbActive.Checked = true;
            cbBillingRollUp.Checked = true;
            cbInvoiceRollUp.Checked = true;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "myModalAddEdit", "$('#myModalAddEdit').modal('show');", true);
        }
    }

    protected void rgSKUCategory_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        BindGrid();
    }

    protected void rgSKUCategory_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
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
                    btnAddSKUCategory.Visible = false;
                    btnEditSKUCategory.Visible = true;
                    hfId.Value = item["Id"].Text.Trim();
                    txtAddCategoryName.Text = item["CategoryName"].Text.Trim();
                    hfOldCategoryName.Value = item["CategoryName"].Text.Trim();
                    cbBillingRollUp.Checked = item["BillingRollup"].Text.Trim() == "Y" ? true : false;
                    cbInvoiceRollUp.Checked = item["InvoiceRollup"].Text.Trim() == "Y" ? true : false;
                    cbActive.Checked = Convert.ToBoolean(item["Active"].Text.Trim());
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "myModalAddEdit", "$('#myModalAddEdit').modal('show');", true);
                }
                break;
            case "DeleteRecord":
                if (true)
                {
                    GridDataItem item = (GridDataItem)e.Item;
                    int skucategoryid = Convert.ToInt32(item["id"].Text.Trim().ToString());
                    int _assignedSKU = DesktopShared.SKUCategory.checkSKUCategoryAssign(skucategoryid, ref err);

                    if (_assignedSKU > 0)
                        DisplayMessage(String.Format("SKU Category has been {0}", "Assigned"), Bootstrap.Alert.AlertType.Warning);
                    else if (_assignedSKU < 0)
                        DisplayMessage(err, Bootstrap.Alert.AlertType.Danger);
                    else if (_assignedSKU == 0)
                    {
                        lblModalDeleteTitle.Text = "Delete Category";
                        lblModalDeleteWording.Text = "Are you sure want to delete this Category : " + item["CategoryName"].Text.Trim() + " ? ";


                        hfDelete.Value = skucategoryid.ToString();
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "myModalDelete", "$('#myModalDelete').modal('show');", true);
                    }
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
        string billingrollup = string.Empty;
        string inovoicerollup = string.Empty;

        billingrollup = cbBillingRollUp.Checked ? "Y" : "N";
        inovoicerollup = cbInvoiceRollUp.Checked ? "Y" : "N";

        switch (str)
        {
            case "Add":
                _check = DesktopShared.SKUCategory.checkDuplicateSKUCategory(txtAddCategoryName.Text, ref err);
                if (_check == 0)
                {
                    if (DesktopShared.SKUCategory.AddSKUCategoy(0,txtAddCategoryName.Text.Trim(),cbActive.Checked, _auditUserId, billingrollup, inovoicerollup, ref err) == true)
                    {
                        DisplayMessage(String.Format("SKU Category {0} - {1}", "Added", DateTime.Now), Bootstrap.Alert.AlertType.Success);
                        BindGrid();
                        rgSKUCategory.DataBind();
                    }
                    else
                        DisplayMessage(err, Bootstrap.Alert.AlertType.Danger);
                }
                else if(_check == -1)
                    DisplayMessage(err, Bootstrap.Alert.AlertType.Danger);
                else
                    DisplayMessage("Category name already existed", Bootstrap.Alert.AlertType.Danger);
                break;
            case "Edit":
                if (hfOldCategoryName.Value.Trim().ToLower() != txtAddCategoryName.Text.Trim().ToLower())
                {
                    _check = DesktopShared.SKUCategory.checkDuplicateSKUCategory(txtAddCategoryName.Text, ref err);
                    if (_check > 0)
                    {
                        DisplayMessage("Category name already existed", Bootstrap.Alert.AlertType.Danger);
                        break;
                    }
                    else if (_check == -1)
                    {
                        DisplayMessage(err, Bootstrap.Alert.AlertType.Danger);
                        break;
                    }
                }
                if (DesktopShared.SKUCategory.AddSKUCategoy(Convert.ToInt32(hfId.Value), txtAddCategoryName.Text, cbActive.Checked, _auditUserId, billingrollup, inovoicerollup, ref err) == true)
                {
                    DisplayMessage(String.Format("SKU Category has been {0} - {1}", "Edited", DateTime.Now), Bootstrap.Alert.AlertType.Success);
                    BindGrid();
                    rgSKUCategory.DataBind();
                }
                else
                    DisplayMessage(err, Bootstrap.Alert.AlertType.Danger);
                break;
            case "Delete":

                if (DesktopShared.SKUCategory.DeleteSKUCategory(Convert.ToInt32(hfDelete.Value),ref err) == true)
                {
                    DisplayMessage(String.Format("SKU Category has been {0} - {1}", "Deleted", DateTime.Now), Bootstrap.Alert.AlertType.Success);
                    BindGrid();
                    rgSKUCategory.DataBind();
                }
                else
                    DisplayMessage(err, Bootstrap.Alert.AlertType.Danger);

                break;
        }
    }
}