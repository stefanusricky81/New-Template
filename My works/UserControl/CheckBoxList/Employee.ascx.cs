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
using SD.LLBLGen.Pro.ORMSupportClasses;
using System.Collections.Generic;

public partial class UserControl_CheckBoxList_Employee : System.Web.UI.UserControl
{
    /// <summary>
    /// page load
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        if (cblEmployee.Items.Count == 0)
            LoadEmployees();
    }

    #region public methods

    /// <summary>
    /// populdate check box list
    /// </summary>
    public void LoadEmployees()
    {
        DataTable dtEmployee = DesktopShared.Employee.GetActiveEmployees();

        cblEmployee.DataSource = dtEmployee;
        cblEmployee.DataTextField = "FullNameWithCommaTrimmed";
        cblEmployee.DataValueField = "Email";
        cblEmployee.DataBind();
    }

    /// <summary>
    /// get selected email addresses
    /// </summary>
    /// <returns></returns>
    public string GetSelectedEmailAddresses()
    {
        StringBuilder sb = new StringBuilder();

        foreach (ListItem li in cblEmployee.Items)
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
    /// set selected options via tagged tickete data
    /// </summary>
    /// <param name="ticketId"></param>
    public void SetSelectedViaTaggedTicket(int ticketId)
    {
        if (cblEmployee.Items.Count == 0)
            LoadEmployees();

        DesktopShared.TypedListClasses.TaggedTicketUserTypedList ttuTypedList =
            new DesktopShared.TypedListClasses.TaggedTicketUserTypedList();

        IPredicateExpression ttuTypedListFilter = new PredicateExpression();
        ttuTypedListFilter.Add(DesktopShared.HelperClasses.TaggedTicketFields.CscDefectsId == ticketId);

        ttuTypedList.Fill(0, null, false, ttuTypedListFilter);
        foreach (DesktopShared.TypedListClasses.TaggedTicketUserRow _row in ttuTypedList.Rows)
        {
            string email = _row.Email;
            ListItem li = cblEmployee.Items.FindByValue(email);
            if (li != null)
                li.Selected = true;
        }
    }

    /// <summary>
    /// set selected options via ticket noficiations data
    /// </summary>
    /// <param name="ticketId"></param>
    public void SetSelectedViaNotification(int ticketId)
    {
        List<DesktopShared.Ticket.Notification.Contact> _contacts = DesktopShared.Ticket.Notification.GetEmployees(ticketId, "");
        SetSelectedViaNotification(_contacts);
    }

    /// <summary>
    /// set selected options via ticket noficiations data
    /// </summary>
    /// <param name="contacts"></param>
    public void SetSelectedViaNotification(List<DesktopShared.Ticket.Notification.Contact> contacts)
    {
        if (cblEmployee.Items.Count == 0)
            LoadEmployees();

        foreach (DesktopShared.Ticket.Notification.Contact objContact in contacts)
        {
            string email = objContact.Email.Trim();
            if (!String.IsNullOrWhiteSpace(email))
            {
                ListItem li = cblEmployee.Items.FindByValue(email);
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
        foreach (ListItem li in cblEmployee.Items)
        {
            if (li.Selected)
                _contacts.Add(new DesktopShared.Ticket.Notification.Contact { Email = li.Value.Trim(), FullName = li.Text.Trim() });
        }
        /*litDebug.Text = "";
        foreach (var _c in _contacts)
            litDebug.Text += String.Format("<br />{0} | {1}", _c.Email, _c.FullName);*/
        DesktopShared.Ticket.Notification.SetForEmployee(_contacts, ticketId);
    }

    /// <summary>
    /// clear selected items
    /// </summary>
    public void ClearSelected()
    {
        foreach (ListItem li in cblEmployee.Items)
            li.Selected = false;
    }

    #endregion
}
