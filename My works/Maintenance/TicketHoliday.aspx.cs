using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class Maintenance_TicketHoliday : System.Web.UI.Page
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
    protected void rgHoliday_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        bindgrid();
    }

    protected void bindgrid()
    {
        rgHoliday.DataSource = DesktopShared.Holiday.Search();
    }

    protected void Decision_Command(object sender, CommandEventArgs e)
    {
        int _auditUserId = DesktopShared.User.UserID;
        string str = e.CommandArgument.ToString().Trim();

        switch (str)
        {
            case "Add":
                if (DesktopShared.Holiday.checkDuplicateHolidayDate(dpHolidayDate.SelectedDate) == 0)
                {
                    if (DesktopShared.Holiday.AddHolidayDate(0,dpHolidayDate.SelectedDate) == true)
                        DisplayMessage(String.Format("Holiday Date has been {0} - {1}", "Added", DateTime.Now), Bootstrap.Alert.AlertType.Success);
                    else
                        DisplayMessage("Something went wrong", Bootstrap.Alert.AlertType.Danger);
                }
                else
                    DisplayMessage(String.Format("{0}-{1} : Already Exists", dpHolidayDate.SelectedDate, DateTime.Now), Bootstrap.Alert.AlertType.Danger);
                break;
            case "Edit":
                if (hfEdit.Value != dpHolidayDate.SelectedDate.Value.ToShortDateString())
                {
                    if (DesktopShared.Holiday.checkDuplicateHolidayDate(dpHolidayDate.SelectedDate) > 0)
                    {
                        DisplayMessage(String.Format("{0}-{1} : Already Exists", dpHolidayDate.SelectedDate, DateTime.Now), Bootstrap.Alert.AlertType.Danger);
                        break;
                    }
                }

                if (DesktopShared.Holiday.AddHolidayDate(Convert.ToInt32(lblIdEdit.Text), dpHolidayDate.SelectedDate) == true)
                    DisplayMessage(String.Format("Holiday Date has been {0} - {1}", "Edited", DateTime.Now), Bootstrap.Alert.AlertType.Success);
                else
                    DisplayMessage("Something went wrong", Bootstrap.Alert.AlertType.Danger);
                break;
            case "Delete":
                if (DesktopShared.Holiday.DeleteHolidayDate(Convert.ToInt32(hfDelete.Value)))
                    DisplayMessage(String.Format("Holiday Date has been {0} - {1}", "Deleted", DateTime.Now), Bootstrap.Alert.AlertType.Success);
                else
                    DisplayMessage("Something went wrong", Bootstrap.Alert.AlertType.Danger);
                break;
        }
        rgHoliday.Rebind();
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        try
        {
            lblModalTitleAddEdit.Text = "Add/Edit Holiday Date";
            //pnlEdit.Visible = false;
            lblIdEdit.Text = string.Empty;
            btnAddDate.Visible = true;
            btnEditDate.Visible = false;
            dpHolidayDate.SelectedDate = null;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "myModalAddEdit", "$('#myModalAddEdit').modal('show');", true);
        }
        catch { }
    }

    protected void rgHoliday_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
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
                    btnAddDate.Visible = false;
                    btnEditDate.Visible = true;
                    lblIdEdit.Text = item["id"].Text.Trim();
                    dpHolidayDate.SelectedDate = Convert.ToDateTime(item["HolidayDate"].Text.Trim());
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "myModalAddEdit", "$('#myModalAddEdit').modal('show');", true);
                }
                break;
            case "DeleteRecord":
                if (true)
                {

                    GridDataItem item = (GridDataItem)e.Item;

                    lblModalDeleteTitle.Text = "Delete Ticket Type";
                    lblModalDeleteWording.Text = "Are you sure want to delete " + item["HolidayDate"].Text.Trim() + " ? ";
                    hfDelete.Value = item["id"].Text;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "myModalDelete", "$('#myModalDelete').modal('show');", true);
                }
                break;
        }
    }
}