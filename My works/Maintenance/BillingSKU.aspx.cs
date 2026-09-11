using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class Maintenance_BillingSKU : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            PopulateDropDownlist();
            GetSKUtId();
        }
    }

    private void DisplayMessage(string message, Bootstrap.Alert.AlertType alertType, bool dismissable = true, string overrideHeader = "")
    {
        ScriptManager.RegisterClientScriptBlock(this, Page.GetType(), "ToTheTop", "ToTopOfPage();", true);
        litMessage.Visible = true;
        litMessage.Text = Bootstrap.Alert.GetDivText(alertType, message, dismissable, true, overrideHeader);
    }

    protected void PopulateDropDownlist()
    {
        ddlSearchSKUCategory.DataSource = DesktopShared.Billing.GetSKUCategory();
        ddlSearchSKUCategory.DataTextField = "CategoryName";
        ddlSearchSKUCategory.DataValueField = "Id";

        ddlSearchSKUCategory.DataBind();
        ddlSearchSKUCategory.Items.Insert(0, "");

        ddlAddSKUCategory.DataSource = DesktopShared.Billing.GetSKUCategory();
        ddlAddSKUCategory.DataTextField = "CategoryName";
        ddlAddSKUCategory.DataValueField = "Id";
   
        ddlAddSKUCategory.DataBind();
        ddlAddSKUCategory.Items.Insert(0, "");
    }

    private void BindGrid()
    {
        string err = string.Empty;
        int? _fkid = null;

        if (ddlSearchSKUCategory.SelectedValue != string.Empty)
            _fkid = Convert.ToInt32(ddlSearchSKUCategory.SelectedValue);

        rgBillingSKU.DataSource = DesktopShared.BillingSKU.Search(txtSeachSKU.Text.Trim(),txtSeachDesc.Text.Trim(), _fkid, cbSearchActive.Checked,ref err);
        if (err != string.Empty)
            DisplayMessage(err, Bootstrap.Alert.AlertType.Danger);
    }

    private void GetSKUtId()
    {
        int _id = -1;
        if (int.TryParse(BitByBit.Web.Request.GetString("skuId").Trim(), out _id))
        {
            txtSeachSKU.Text = _id.ToString().Trim();
        }
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        if (true)
        {
            lblModalTitleAddEdit.Text = "Add/Edit SKU Category";
            btnAddBillingSKU.Visible = true;
            btnEditBillingSKU.Visible = false;
            txtSKU.Text = string.Empty;
            txtDescription.Text = string.Empty;
            ddlAddSKUCategory.SelectedIndex = -1;
            cbActive.Checked = true;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "myModalAddEdit", "$('#myModalAddEdit').modal('show');", true);
        }
    }

    protected void rgBillingSKU_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        BindGrid();
    }

    protected void rgBillingSKU_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        int _auditUserId = DesktopShared.User.UserID;
        string command = e.CommandArgument.ToString().Trim();
        string err = string.Empty;

        switch (command)
        {
            case "UpdateRecord":
                if (true)
                {
                    PopulateDropDownlist();
                    GridDataItem item = (GridDataItem)e.Item;

                    lblModalTitleAddEdit.Text = "Add/Edit SKU Category";
                    btnAddBillingSKU.Visible = false;
                    btnEditBillingSKU.Visible = true;
                    hfId.Value = item["Id"].Text.Trim();
                    txtSKU.Text = item["SKU"].Text.Trim();
                    hfOldSKU.Value = item["SKU"].Text.Trim();
                    txtDescription.Text= HttpUtility.HtmlDecode(item["Description"].Text).Trim();
                    string skuid= HttpUtility.HtmlDecode(item["fk_SKUcategoryid"].Text).Trim();
                    ddlAddSKUCategory.SelectedValue = skuid == string.Empty ? "" : skuid;

                    cbActive.Checked = Convert.ToBoolean(item["Active"].Text.Trim());
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "myModalAddEdit", "$('#myModalAddEdit').modal('show');", true);
                }
                break;
            case "DeleteRecord":
                if (true)
                {
                    GridDataItem item = (GridDataItem)e.Item;
                    int skucategoryid = Convert.ToInt32(item["id"].Text.Trim().ToString());
                    lblModalDeleteTitle.Text = "Delete Category";
                    lblModalDeleteWording.Text = "Are you sure want to delete this Category : " + item["CategoryName"].Text.Trim() + " ? ";


                    hfDelete.Value = skucategoryid.ToString();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "myModalDelete", "$('#myModalDelete').modal('show');", true);
                }
                break;
        }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        BindGrid();
        rgBillingSKU.DataBind();
    }

    protected void lbClear_Click(object sender, EventArgs e)
    {
        Response.Redirect("BillingSKU.aspx");
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
                _check = DesktopShared.BillingSKU.checkDuplicateBillingSKU(txtSKU.Text.Trim(), ref err);
                if (_check == 0)
                {
                    if (DesktopShared.BillingSKU.AddBillingSKU(0,txtSKU.Text.Trim(),txtDescription.Text.Trim(),Convert.ToInt32(ddlAddSKUCategory.SelectedValue), cbActive.Checked, _auditUserId, ref err) == true)
                    {
                        DisplayMessage(String.Format("Billing SKU {0} - {1}", "Added", DateTime.Now), Bootstrap.Alert.AlertType.Success);
                        BindGrid();
                        rgBillingSKU.DataBind();
                    }
                    else
                        DisplayMessage(err, Bootstrap.Alert.AlertType.Danger);
                }
                else if (_check == -1)
                    DisplayMessage(err, Bootstrap.Alert.AlertType.Danger);
                else
                    DisplayMessage("Billing SKU already existed", Bootstrap.Alert.AlertType.Danger);
                break;
            case "Edit":
                if (hfOldSKU.Value.Trim().ToLower() != txtSKU.Text.Trim().ToLower())
                {
                    _check = DesktopShared.BillingSKU.checkDuplicateBillingSKU(txtSKU.Text, ref err);
                    if (_check > 0)
                    {
                        DisplayMessage("Billing SKU already existed", Bootstrap.Alert.AlertType.Danger);
                        break;
                    }
                    else if (_check == -1)
                    {
                        DisplayMessage(err, Bootstrap.Alert.AlertType.Danger);
                        break;
                    }
                }
                if (DesktopShared.BillingSKU.AddBillingSKU(Convert.ToInt32(hfId.Value), txtSKU.Text,txtDescription.Text.Trim(),Convert.ToInt32(ddlAddSKUCategory.SelectedValue), cbActive.Checked, _auditUserId, ref err) == true)
                {
                    DisplayMessage(String.Format("SKU Category has been {0} - {1}", "Edited", DateTime.Now), Bootstrap.Alert.AlertType.Success);
                    BindGrid();
                    rgBillingSKU.DataBind();
                }
                else
                    DisplayMessage(err, Bootstrap.Alert.AlertType.Danger);
                break;
            case "Delete":

                if (DesktopShared.BillingSKU.DeleteBillingSKU(Convert.ToInt32(hfDelete.Value), ref err) == true)
                {
                    DisplayMessage(String.Format("Billing SKU has been {0} - {1}", "Deleted", DateTime.Now), Bootstrap.Alert.AlertType.Success);
                    BindGrid();
                    rgBillingSKU.DataBind();
                }
                else
                    DisplayMessage(err, Bootstrap.Alert.AlertType.Danger);

                break;
        }

    }
}