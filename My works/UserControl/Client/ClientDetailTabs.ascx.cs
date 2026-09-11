using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class UserControl_Client_ClientDetailTabs : System.Web.UI.UserControl
{

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            GetClientDetails();
        }
    }

    public int SelectedTabIndex
    {
        get
        {
            return tabstripModeldetail.SelectedTab.Index;
        }
        set
        {
            tabstripModeldetail.Tabs[value].Selected = true;
            tabstripModeldetail.SelectedTab.CssClass = "selected";
        }
    }

    /// <summary>
    /// display message
    /// </summary>
    /// <param name="message"></param>
    /// <param name="alertType"></param>
    /// <param name="overrideHeader"></param>
    private void DisplayMessage(string message, Bootstrap.Alert.AlertType alertType, bool dismissable = true, string overrideHeader = "")
    {
        ScriptManager.RegisterClientScriptBlock(this, Page.GetType(), "ToTheTop", "ToTopOfPage();", true);
        litMessage.Visible = true;
        litMessage.Text = Bootstrap.Alert.GetDivText(alertType, message, dismissable, true, overrideHeader);
    }

    /// <summary>
    /// get user id from query string
    /// </summary>
    /// <param name="objMenur"></param>
    /// <returns></returns>
    private bool GetClientDetails()
    {
        int _id = -1;
        if (int.TryParse(BitByBit.Web.Request.GetString("ClientId").Trim(), out _id))
        {
            ClientId = _id;

            if (ClientId == 0)
            {
                int TabCnt = tabstripModeldetail.Tabs.Count;
                for (int i = 1; i < TabCnt; i++)
                {
                    tabstripModeldetail.Tabs[i].Visible = false;
                }
                lblClientNameLabel.Text = "Add New Client";
            }
            else
            {
                DesktopShared.EntityClasses.ClientEntity objClient = new DesktopShared.EntityClasses.ClientEntity(_id);
                lblClientName.Text = objClient.Company;

                if (objClient.DisplayTicketOrder != null)
                    tabstripModeldetail.Tabs[6].Visible = (bool)objClient.DisplayTicketOrder;
                else
                    tabstripModeldetail.Tabs[6].Visible = false;
            }
            return true;
        }
        DisplayMessage(String.Format("Unable to fetch Client Information.  ID = {0}", _id > 0 ? _id.ToString() : "N/A"), Bootstrap.Alert.AlertType.Danger, false);
        return false;
    }

    public void EnableTabs(int NewlyAddedClient_Id)
    {
        ClientId = NewlyAddedClient_Id;

        if (ClientId > 0)
        {
            DesktopShared.EntityClasses.ClientEntity objClient = new DesktopShared.EntityClasses.ClientEntity((int)ClientId);
            lblClientName.Text = objClient.Company;

            int TabCnt = tabstripModeldetail.Tabs.Count;
            for (int i = 1; i < TabCnt; i++)
            {
                tabstripModeldetail.Tabs[i].Visible = true;
            }
        }
    }

    protected void tabstripModeldetail_TabClick(object sender, Telerik.Web.UI.RadTabStripEventArgs e)
    {
        RadTabStrip RadTabStrip1 = sender as RadTabStrip;
        if (e.Tab.Text == "Client")
        {
            Response.Redirect(String.Format("~/Client/ClientDetail.aspx?ClientId={0}&TabIndex=0", ClientId));
        }
        else if (e.Tab.Text == "Billing")
        {
            Response.Redirect(String.Format("~/Client/ClientBilling.aspx?ClientId={0}&TabIndex=1", ClientId));
        }
        else if (e.Tab.Text == "Contacts")
        {
            Response.Redirect(String.Format("~/Client/ClientContacts.aspx?ClientId={0}&TabIndex=2", ClientId));
        }
        else if (e.Tab.Text == "Timesheet Projects")
        {
            Response.Redirect(String.Format("~/Client/ClientProjects.aspx?ClientId={0}&TabIndex=3", ClientId));
        }
        else if (e.Tab.Text == "Products")
        {
            Response.Redirect(String.Format("~/Client/ClientProducts.aspx?ClientId={0}&TabIndex=4", ClientId));
        }
        else if (e.Tab.Text == "Renewals")
        {
            Response.Redirect(String.Format("~/Client/ClientSupportContracts.aspx?ClientId={0}&TabIndex=5", ClientId));
        }
        else if (e.Tab.Text == "Projects")
        {
            Response.Redirect(String.Format("~/Client/ClientTicketProjects.aspx?ClientId={0}&TabIndex=6", ClientId));
        }
        else if (e.Tab.Text == "Documents")
        {
            Response.Redirect(String.Format("~/Client/Document.aspx?ClientId={0}&TabIndex=7", ClientId));
        }
        else if (e.Tab.Text == "TAM")
        {
            Response.Redirect(String.Format("~/Client/TamClientTask.aspx?ClientId={0}&TabIndex=8", ClientId));
        }
        else if (e.Tab.Text == "Settings")
        {
            Response.Redirect(String.Format("~/Client/ClientDetail2.aspx?ClientId={0}&TabIndex=9", ClientId));
        }
        else if (e.Tab.Text == "Servers")
        {
            Response.Redirect(String.Format("~/Client/Server.aspx?ClientId={0}&TabIndex=10", ClientId));
        }
        else if (e.Tab.Text == "Workstations")
        {
            Response.Redirect(String.Format("~/Client/Workstation.aspx?ClientId={0}&TabIndex=11", ClientId));
        }
        else if (e.Tab.Text == "Master Client")
        {
            Response.Redirect(String.Format("~/Client/MasterClient.aspx?ClientId={0}&TabIndex=12", ClientId));
        }
        else if (e.Tab.Text == "Reports")
        {
            Response.Redirect(String.Format("~/Client/Report.aspx?ClientId={0}&TabIndex=13", ClientId));
        }
        else if (e.Tab.Text == "Tickets")
        {
            Response.Redirect(String.Format("~/Client/ClientTickets.aspx?ClientId={0}&TabIndex=14", ClientId));
        }
        else if (e.Tab.Text == "Dashboard")
        {
            Response.Redirect(String.Format("~/Client/DashBoard.aspx?ClientId={0}&TabIndex=15", ClientId));
        }
        else if (e.Tab.Text == "Invoice")
        {
            Response.Redirect(String.Format("~/Client/ClientInvoice.aspx?ClientId={0}&TabIndex=16", ClientId));
        }
        else if (e.Tab.Text == "Payments")
        {
            Response.Redirect(String.Format("~/Client/ClientPayment.aspx?ClientId={0}&TabIndex=17", ClientId));
        }
        else if (e.Tab.Text == "Microsoft Licenses")
        {
            Response.Redirect(String.Format("~/Client/ClientMicrosoftLicenses.aspx?ClientId={0}&TabIndex=18", ClientId));
        }
        else if (e.Tab.Text == "Supported Users")
        {
            Response.Redirect(String.Format("~/Client/ClientSupportedLicenseUsers.aspx?ClientId={0}&TabIndex=19", ClientId));
        }
        else if (e.Tab.Text == "Client Notes")
        {
            Response.Redirect(String.Format("~/Client/ClientNotes.aspx?ClientId={0}&TabIndex=20", ClientId));
        }
        else if (e.Tab.Text == "Client Services")
        {
            Response.Redirect(String.Format("~/Client/ClientServicesOffered.aspx?ClientId={0}&TabIndex=21", ClientId));
        }
    }

    #region private properties

    /// <summary>
    /// get/set user id
    /// </summary>
    private int? ClientId
    {
        get
        {
            object obj = this.ViewState["ClientId"];
            return (obj == null) ? (int?)null : (int)obj;
        }
        set { this.ViewState["ClientId"] = value; }
    }

    #endregion
}