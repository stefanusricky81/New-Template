using DesktopShared.CollectionClasses;
using DesktopShared.EntityClasses;
using DesktopShared.HelperClasses;
using SD.LLBLGen.Pro.ORMSupportClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class Maintenance_EmailBlackList : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    #region protected
    protected void Decision_Command(object sender, CommandEventArgs e)
    {
        int _auditUserId = DesktopShared.User.UserID;
        string str = e.CommandArgument.ToString().Trim();

        switch (str)
        {
            case "Add":
                if (AddEdit(0, txtAddEditEmail.Text, Convert.ToBoolean(ddlAddEditDisableBlock.SelectedValue)) == true)
                {
                    DisplayMessage(String.Format("Email {0} has been black list - {1}", txtAddEditEmail.Text, DateTime.Now), Bootstrap.Alert.AlertType.Success);
                    BindGrid();
                    rgEmailBlackList.DataBind();
                }
                else
                    DisplayMessage("Something went wrong", Bootstrap.Alert.AlertType.Danger);
                break;
            case "Edit":
                if (AddEdit(Convert.ToInt32(hfId.Value), txtAddEditEmail.Text, Convert.ToBoolean(ddlAddEditDisableBlock.SelectedValue)) == true)
                {
                    DisplayMessage(String.Format("Email has been {0} - {1}", "Edited", DateTime.Now), Bootstrap.Alert.AlertType.Success);
                    BindGrid();
                    rgEmailBlackList.DataBind();
                }
                else
                    DisplayMessage("Something went wrong", Bootstrap.Alert.AlertType.Danger);
                break;
            case "Delete":
                if (Delete(Convert.ToInt32(hfDelete.Value)))
                {
                    DisplayMessage(String.Format("Email has been {0} - {1}", "Deleted", DateTime.Now), Bootstrap.Alert.AlertType.Success);
                    BindGrid();
                    rgEmailBlackList.DataBind();
                }
                else
                    DisplayMessage("Something went wrong", Bootstrap.Alert.AlertType.Danger);
                break;
        }
    }
    #endregion

    #region Button
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        BindGrid();
        rgEmailBlackList.DataBind();
    }

    protected void lbClear_Click(object sender, EventArgs e)
    {
        txtEmail.Text = string.Empty;
        ddlBlock.SelectedIndex = -1;

        BindGrid();
        rgEmailBlackList.DataBind();
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        lblModalTitleAddEdit.Text = "Add/Edit Email Black List";
        btnAddTrunk.Visible = true;
        btnEditTrunk.Visible = false;
        txtAddEditEmail.Text = string.Empty;
        ddlAddEditDisableBlock.SelectedIndex = -1;
        ScriptManager.RegisterStartupScript(this, this.GetType(), "myModalAddEdit", "$('#myModalAddEdit').modal('show');", true);
    }
    #endregion

    #region Grid
    private void BindGrid()
    {
        rgEmailBlackList.DataSource = Search(txtEmail.Text, ddlBlock.SelectedValue == string.Empty ? (bool?)null : Convert.ToBoolean(ddlBlock.SelectedValue));
    }

    protected void rgEmailBlackList_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        BindGrid();
    }

    protected void rgEmailBlackList_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        int _auditUserId = DesktopShared.User.UserID;
        string command = e.CommandArgument.ToString().Trim();

        switch (command)
        {
            case "UpdateRecord":
                if (true)
                {
                    GridDataItem item = (GridDataItem)e.Item;

                    lblModalTitleAddEdit.Text = "Add/Edit Email Black List";
                    btnAddTrunk.Visible = false;
                    btnEditTrunk.Visible = true;
                    hfId.Value = item["id"].Text.Trim();
                    ddlAddEditDisableBlock.SelectedValue = Convert.ToBoolean(item["DisableBlock"].Text).ToString().ToLower();
                    txtAddEditEmail.Text = item["Email"].Text.Trim();
                    //if (item["DisableBlock"].Text.ToLower() == "yes")
                    //    ddlAddEditDisableBlock.SelectedValue = "true";
                    //else
                    //    ddlAddEditDisableBlock.SelectedValue = "false";

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "myModalAddEdit", "$('#myModalAddEdit').modal('show');", true);
                }
                break;
            case "DeleteRecord":
                if (true)
                {

                    GridDataItem item = (GridDataItem)e.Item;

                    lblModalDeleteTitle.Text = "Delete Email Black List";
                    lblModalDeleteWording.Text = "Are you sure want to delete " + item["Email"].Text.Trim() + " ? ";
                    hfDelete.Value = item["id"].Text;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "myModalDelete", "$('#myModalDelete').modal('show');", true);
                }
                break;
        }
    }
    #endregion

    #region DBstuff
    private EmailBlacklistCollection Search(string email, bool? block)
    {
        EmailBlacklistCollection _emailblacklist = new EmailBlacklistCollection();

        string _email = "%" + email + "%";

        IPredicateExpression _orFilter = new PredicateExpression();

        if (!string.IsNullOrEmpty(email))
            _orFilter.AddWithAnd(EmailBlacklistFields.Email % _email);

        if(block.HasValue)
            _orFilter.AddWithAnd(EmailBlacklistFields.DisableBlock == block.Value);

        //sort expression
        ISortExpression _emailblacklistsort = new SD.LLBLGen.Pro.ORMSupportClasses.SortExpression();
        _emailblacklistsort.Add(EmailBlacklistFields.Created | SortOperator.Descending);

        //fetch
        _emailblacklist.GetMulti(_orFilter, 0, _emailblacklistsort);

        //return
        return _emailblacklist;
    }

    private bool AddEdit(int pid, string email, bool? block)
    {
        try
        {
            EmailBlacklistEntity _email = pid > 0 ? new EmailBlacklistEntity(pid) : new EmailBlacklistEntity();

            _email.Email = email;
            _email.DisableBlock = block.Value;

            if (pid > 0)
            {
                _email.Created = DateTime.Now;
                _email.CreatedBy = "";
            }
            else
            {
                _email.Lastupdated = DateTime.Now;
                _email.LastupdatedBy = "";
            }

            _email.Save();
            _email.Refetch();

            return true;
        }
        catch
        {
            return false;
        }
    }

    private bool Delete(int pid)
    {
        try
        {
            EmailBlacklistEntity _email = new EmailBlacklistEntity(pid);
            _email.Delete();
            _email.Save();
            return true;
        }
        catch
        {
            return false;
        }
    }
    #endregion

    #region Private
    private void DisplayMessage(string message, Bootstrap.Alert.AlertType alertType, bool dismissable = true, string overrideHeader = "")
    {
        ScriptManager.RegisterClientScriptBlock(this, Page.GetType(), "ToTheTop", "ToTopOfPage();", true);
        litMessage.Visible = true;
        litMessage.Text = Bootstrap.Alert.GetDivText(alertType, message, dismissable, true, overrideHeader);
    }
    #endregion
}