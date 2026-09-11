using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Text;
using System.Collections.Generic;

public partial class UserControl_CheckBoxList_ClientContact : System.Web.UI.UserControl
{
    /// <summary>
    /// page load
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
       
    }

    #region public methods

    /// <summary>
    /// populate drop down list
    /// </summary>
    public void LoadUsers()
    {
        DataTable dtClientContact = DesktopShared.Client.GetActiveContacts(SelectedClientId);

        cblClientContact.DataSource = dtClientContact;
        cblClientContact.DataTextField = "FullNameWithCommaTrimmed";
        cblClientContact.DataValueField = "Email";
        cblClientContact.DataBind();
    }

    /// <summary>
    /// get/set selected email addresses
    /// </summary>
    /// <returns></returns>
    public string GetSelectedEmailAddresses()
    {
        StringBuilder sb = new StringBuilder();

        foreach (ListItem li in cblClientContact.Items)
        {
            if (li.Selected)
            {
                if (sb.Length > 0)
                    sb.Append("; ");

                sb.Append(li.Value.Trim());
            }
        }

        return sb.ToString();
    }

    /// <summary>
    /// clear selected items
    /// </summary>
    public void ClearSelected()
    {
        foreach (ListItem li in cblClientContact.Items)
            li.Selected = false;
    }

    /// <summary>
    /// set selected options via ticket noficiations data
    /// </summary>
    /// <param name="ticketId"></param>
    public void SetSelectedViaNotification(int ticketId)
    {
        List<DesktopShared.Ticket.Notification.Contact> _contacts = DesktopShared.Ticket.Notification.GetClientContacts(ticketId, "");
        SetSelectedViaNotification(_contacts);
    }

    /// <summary>
    /// set selected options via ticket noficiations data
    /// </summary>
    /// <param name="contacts"></param>
    public void SetSelectedViaNotification(List<DesktopShared.Ticket.Notification.Contact> contacts)
    {
        if (cblClientContact.Items.Count == 0)
            LoadUsers();

        foreach (DesktopShared.Ticket.Notification.Contact objContact in contacts)
        {
            string email = objContact.Email.Trim();
            if (!String.IsNullOrWhiteSpace(email))
            {
                ListItem li = cblClientContact.Items.FindByValue(email);
                if (li != null)
                    li.Selected = true;
            }
        }
    }

    /// <summary>
    /// update ticket notification collection
    /// </summary>
    /// <param name="ticketId"></param>
    public void UpdateTicketNotification(int ticketId)
    {
        List<DesktopShared.Ticket.Notification.Contact> _contacts = new List<DesktopShared.Ticket.Notification.Contact>();
        foreach (ListItem li in cblClientContact.Items)
        {
            if (li.Selected)
                _contacts.Add(new DesktopShared.Ticket.Notification.Contact { Email = li.Value.Trim(), FullName = li.Text.Trim() });
        }
        DesktopShared.Ticket.Notification.SetForClientContact(_contacts, ticketId);
    }

    #endregion

    #region public properties

    /// <summary>
    /// get/set selected client id
    /// </summary>
    public int SelectedClientId
    {
        get
        {
            object obj = this.ViewState["SelectedClientId"];
            if (obj == null)
                return -1;
            else
                return (int)obj;
        }

        set
        {
            this.ViewState["SelectedClientId"] = value;
            LoadUsers();
        }
    }

    #endregion
}
