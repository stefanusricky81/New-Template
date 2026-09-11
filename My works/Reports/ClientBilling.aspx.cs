using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Report_ClientBilling : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            phSearchResults.Visible = false;
        }
    }

    /// <summary>
    /// rebind grid
    /// </summary>
    private void RebindGrid()
    {
        phSearchResults.Visible = true;
        rgClient.EditIndexes.Clear();
        rgClient.DataSource = null;
        rgClient.Rebind();
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        RebindGrid();
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        txtCode.Text = string.Empty;
        txtCompany.Text = string.Empty;
        ddlAutoBill.SelectedIndex = 0;
    }
    protected void rgClient_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        //try
        //{
        //    if (e.CommandName == "DeleteMenu")
        //    {
        //        int _menuId = Convert.ToInt16(e.CommandArgument);
        //        DesktopShared.EntityClasses.MenuEntity objMenu = new DesktopShared.EntityClasses.MenuEntity(_menuId);
        //        objMenu.Delete();
        //        objMenu.Save();
        //        DisplayMessage(String.Format("Record deleted successfully."),
        //             Bootstrap.Alert.AlertType.Success);
        //        rgClient.Rebind();
        //    }
        //}
        //catch
        //{
        //    DisplayMessage(String.Format("Error occured while deleting the record"),
        //              Bootstrap.Alert.AlertType.Warning);
        //}
    }
    protected void rgClient_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        rgClient.DataSource = DesktopShared.Client.SearchClients(txtCompany.Text, txtCode.Text, ddlAutoBill.SelectedIndex == 0 ? (bool?)null : ddlAutoBill.SelectedIndex == 1 ? true : false);
    }
}