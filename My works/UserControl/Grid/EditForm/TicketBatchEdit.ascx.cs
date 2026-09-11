using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SD.LLBLGen.Pro.ORMSupportClasses;

public partial class UserControl_Grid_EditForm_TicketBatchEdit : AbstractTicketBatchEdit
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    #region public methods

    #region override

    /// <summary>
    /// load ticket values via ticked id
    /// </summary>
    /// <param name="ticketID"></param>
    public override void LoadValues(int ticketID)
    {
        DesktopShared.TypedListClasses.TicketTypedList tickets =
            new DesktopShared.TypedListClasses.TicketTypedList();

        IPredicateExpression ticketsFilter = new PredicateExpression();
        ticketsFilter.Add(DesktopShared.HelperClasses.CscDefectsFields.Pcscdefects == ticketID);

        tickets.Fill(0, null, false, ticketsFilter);

        LoadValues(tickets[0]);
    }

    /// <summary>
    /// load ticket values ticket typed list row
    /// </summary>
    /// <param name="ticket"></param>
    public override void LoadValues(DesktopShared.TypedListClasses.TicketRow ticket)
    {
        TicketId = ticket.Pcscdefects;
        Description = ticket.Description;
        Summary = ticket.Summary.Trim();
        SelectedClientId = ticket.FkClient;
        DispositionId = ticket.FkDisposition;
        PriorityId = ticket.FkPriority;
        AssignedTo = ticket.Assignedto;
        ProductId = ticket.ProductId;
        if (ticket.IsTicketTypeIdNull())
            ucTicketType.PopulateDropDownList();
        else
            TicketTypeId =  ticket.TicketTypeId;
    }

     /// <summary>
    /// save ticket values via ticked id
    /// </summary>
    /// <param name="ticketID"></param>
    public override void SaveValues(int ticketID)
    {
        DesktopShared.EntityClasses.CscDefectsEntity ticket =
            new DesktopShared.EntityClasses.CscDefectsEntity(ticketID);

        SaveValues(ticket);
    }

    /// <summary>
    /// save ticket values via ticket entity
    /// </summary>
    /// <param name="ticket"></param>
    /// <param name="userManagerId"></param>
    public override void SaveValues(DesktopShared.EntityClasses.CscDefectsEntity ticket, int? userManagerId = null)
    {
        //fetch ticket typed list row
        DesktopShared.TypedListClasses.TicketRow ticketRow = DesktopShared.Ticket.GetTicketTypedListRow(ticket.Pcscdefects);

        //string builder for inter history note if entity was updated
        System.Text.StringBuilder sbHistoryNote = new System.Text.StringBuilder();

        //string builder for external history note if summary or status was updated
        System.Text.StringBuilder sbExternalHistoryNote = new System.Text.StringBuilder();

        //list of employees to be emailed
        string _employeeEmails = "";

        #region check each column to see if any update was made.  
        
        #region summary

        if (ticket.Summary.Trim() != Summary.Trim())
        {
            //add to history note
            sbHistoryNote.AppendLine("Summary updated from \"" + ticket.Summary.Trim () + 
                "\" to \"" + Summary.Trim() + "\"" );
            
            //external history note
            sbExternalHistoryNote.AppendLine("Summary updated from \"" + ticket.Summary.Trim () +
                "\" to \"" + Summary.Trim() + "\"");

            ticket.Summary = Summary.Trim();
        }

        #endregion

        #region client

        if (ticket.FkClient != SelectedClientId)
        {
            //add to history note
            sbHistoryNote.AppendLine("Client updated from " + ticketRow.ClientCompany.Trim() +
                " to " + ucClient.SelectedClientName.Trim());

            ticket.FkClient = SelectedClientId;
        }

        #endregion

        #region status / disposition

        int ticketStatusID = DispositionId == 93 ? 77 : 76;

        bool statusUpdated = false;
        bool _ticketWasClosed = false;

        if (ticket.FkDisposition.HasValue)
            statusUpdated = ticket.FkDisposition.Value != DispositionId;
        else
            statusUpdated = true; //if ticket did not originally have status, it has been updated as selection is required to submit form

        if (statusUpdated)
        {
            if (DispositionId == DesktopShared.Ticket.Disposition.Id.Closed)
                _ticketWasClosed = true;

            //need to fetch cscfields entity to get original disposition name
            string originalDisposition = "";
            if (ticket.FkDisposition.HasValue)
            {
                DesktopShared.EntityClasses.CscFieldsEntity disposition = new DesktopShared.EntityClasses.CscFieldsEntity(ticket.FkDisposition.Value);
                if (disposition.Fields.State == EntityState.Fetched)
                    originalDisposition = disposition.Name.Trim();
            }

            //todo: review ticket edit screen
            /*
            //ticket was changed to active, all other active tickets need to be set to idle
            if ((DispositionId == DesktopShared.SiteHelper.Ticket.Dispostion.Id.Active) && (ticket.FkDisposition != DesktopShared.SiteHelper.Ticket.Dispostion.Id.Active))
                DesktopShared.Ticket.MakeActive(ticket.Pcscdefects, DesktopShared.User.UserID, AssignedTo, false);
            else if ((DispositionId != DesktopShared.SiteHelper.Ticket.Dispostion.Id.Active) && (ticket.FkDisposition == DesktopShared.SiteHelper.Ticket.Dispostion.Id.Active)) //was active, changed to alternate disposition
                DesktopShared.Ticket.MakeInactive(ticket.Pcscdefects, DesktopShared.User.UserID, AssignedTo, AddTimesheet);

            //if status is active and assignment has changed .... set status to idle, run make inactive 
            if (_assignedToChanged && (DispositionId == DesktopShared.SiteHelper.Ticket.Dispostion.Id.Active))
            {
                DesktopShared.Ticket.MakeInactive(ticket.Pcscdefects, DesktopShared.User.UserID, DesktopShared.User.UserID, AddTimesheet);
                ticket.FkDisposition = DesktopShared.SiteHelper.Ticket.Dispostion.Id.Idle;
            }
            else //if user is updating a ticket with a status of new, switch to idle if they don't manually update the status themselves
                ticket.FkDisposition = (DispositionId == DesktopShared.SiteHelper.Ticket.Dispostion.Id.New) ? DesktopShared.SiteHelper.Ticket.Dispostion.Id.Idle : DispositionId;*/

            //add to history note
            sbHistoryNote.AppendLine("Status updated from " + originalDisposition + " to " + ucTicketDisposition.DispositionName.Trim());

            //external history note
            sbExternalHistoryNote.AppendLine("Status updated from " + originalDisposition + " to " + ucTicketDisposition.DispositionName.Trim());

            ticket.FkDisposition = DispositionId > 0 ? DispositionId : (int?)null;
            ticket.FkStatus = ticketStatusID;
        }
        else
        {
            //if user is updating a ticket with a status of new, switch to idle if they don't manually update the status themselves
            if (DispositionId == DesktopShared.SiteHelper.Ticket.Dispostion.Id.New)
                ticket.FkDisposition = DesktopShared.SiteHelper.Ticket.Dispostion.Id.Idle;
        }

        #endregion

        #region priority

        if (ticket.FkPriority != PriorityId)
        {
            //original priority name
            string _originalPriorityName = "n/a";
            if (ticket.FkPriority.HasValue)
            {
                DesktopShared.EntityClasses.CscFieldsEntity objPriority = new DesktopShared.EntityClasses.CscFieldsEntity(ticket.FkPriority.Value);
                if (objPriority.Fields.State == EntityState.Fetched)
                    _originalPriorityName = objPriority.Name.Trim();
            }

            //new priority name
            string _newPriorityName = PriorityId.HasValue ? ucTicketPriority.PriorityName.Trim() : "n/a";

            //add to history note
            sbHistoryNote.AppendLine(String.Format("Priority updated from {0} to {1}", _originalPriorityName, _newPriorityName));

            ticket.FkPriority = PriorityId;
            
        }

        #endregion

        #region assigned to
        bool _sendSlackMessage = false;

        if (ticket.Assignedto != AssignedTo)
        {
            //add to history note
            sbHistoryNote.AppendLine("Assigned To updated from " + ticketRow.AssignedToFirst.Trim() + " " 
                + ticketRow.AssignedToLast.Trim() + 
                " to " + ucEmployeeAssignedTo.EmployeeName.Trim());

            ticket.Assignedto = AssignedTo;

            //dont' send email alert to employee if they were the one who updated the ticket
            if (DesktopShared.User.UserID != AssignedTo)
            {
                //get user entity
                DesktopShared.EntityClasses.UsersEntity _userEmployee =
                   new DesktopShared.EntityClasses.UsersEntity(AssignedTo);

                //if email address exists for user, add to string
                if (!String.IsNullOrEmpty(_userEmployee.Email))
                    _employeeEmails += _userEmployee.Email.Trim();

                _sendSlackMessage = true;
            }
        }

        #endregion

        #region product

        if (ticket.ProductId != ProductId)
        {
            //add to history note
            sbHistoryNote.AppendLine("Product updated from " + ticket.Product.Name.Trim() 
                + " to " + ucTicketProduct.ProductName.Trim());

            ticket.ProductId = ProductId.HasValue ? ProductId.Value : (int?)null;
        }

        #endregion

        #region ticket ticket type 

        if (ticket.TicketTypeId != TicketTypeId)
        {
            //add to history note
            sbHistoryNote.AppendLine("Type updated from " + ticket.TicketType.Name.Trim() + " to " + ucTicketType.TicketTypeName.Trim().Replace("-- Select Type--", ""));
            ticket.TicketTypeId = TicketTypeId;
        }

        #endregion

        #endregion

        #region update was made -> 1.  save entity.  2.  create history record.  3.  send email alert

        if (sbHistoryNote.ToString().Length > 0)
        {
            string _internalNote = "Batch Edit:\n";
            _internalNote += sbHistoryNote.ToString();

            ticket.Lastupdated = DateTime.Now;
            ticket.Save();

            #region add history to ticket and send emails

            DesktopShared.Ticket.History.Add(
                ticket.Pcscdefects, //ticket id
                DesktopShared.User.UserID, //user id, 
                sbExternalHistoryNote.ToString().Trim(), //notes
                _internalNote, //internal notes 
                false, // is new ticket
                "", // client email addresses
                _employeeEmails, // employee email addresses
                true, // send email
                DesktopShared.Ticket.History.Type.Id.BatchUpdate //history type
                );

            #endregion
        }

        #endregion

        //if (_sendSlackMessage)
            //DesktopShared.Ticket.Slack.SendMessage(ticket.Pcscdefects, DesktopShared.User.UserID);

        //if ticket closed, user has mananger, and checklist items open -> send alert to manager
        if (_ticketWasClosed)
        {
            if (userManagerId.HasValue)
            {
                if (DesktopShared.Ticket.CheckList.HasOpenItems(ticket.Pcscdefects))
                    DesktopShared.Email.Ticket.SendClosedWithOpenChecklist(ticket.Pcscdefects, DesktopShared.User.UserID, userManagerId.Value);
            }
        }
    }

    #endregion

    #endregion

     #region public properties

    /// <summary>
    /// get/set ticket id
    /// </summary>
    public int TicketId
    {
        get
        {
            object obj = this.ViewState["TicketIdForEdit"];
            if (obj == null)
                return -1;
            else
                return (int)obj;
        }

        set
        {
            this.ViewState["TicketIdForEdit"] = value;

            lblTicketNumber.Text = value.ToString();

        }
    }

    /// <summary>
    /// set description for rad tool tip
    /// </summary>
    public string Description
    {
        set { rttTicketDescription.Text = value.Trim().Replace("\n", "<br />"); }
    }

    /// <summary>
    /// get/set summary
    /// </summary>
    public string Summary
    {
        get { return tbSummary.Text; }
        set { tbSummary.Text = value; }
    }

    /// <summary>
    /// get/set selected client id
    /// </summary>
    public int SelectedClientId
    {
        get { return ucClient.SelectedClientId; }
        set { ucClient.SelectedClientId = value; }
    }

    /// <summary>
    /// get/set disposition id
    /// </summary>
    public int DispositionId
    {
        get { return ucTicketDisposition.DispositionId; }
        set { ucTicketDisposition.DispositionId = value; }
    }

    /// <summary>
    /// get/set priority id
    /// </summary>
    public int? PriorityId
    {
        get { return ucTicketPriority.PriorityId > 0 ? ucTicketPriority.PriorityId : (int?)null; }
        set { ucTicketPriority.PriorityId = value.HasValue ? value.Value : -1; }
    }

    /// <summary>
    /// get/set assigned to
    /// </summary>
    public int AssignedTo
    {
        get { return ucEmployeeAssignedTo.EmployeeId; }
        set { ucEmployeeAssignedTo.EmployeeId = value; }
    }

    /// <summary>
    /// get/set product id
    /// </summary>
    public int? ProductId
    {
        get { return ucTicketProduct.ProductId; }
        set
        { 
            if (value.HasValue)
                ucTicketProduct.ProductId = value.Value; 
        }
    }

    /// <summary>
    /// get/set ticket type id
    /// </summary>
    public int? TicketTypeId
    {
        get { return ucTicketType.TicketTypeId; }
        set
        {
            if (value.HasValue)
                ucTicketType.TicketTypeId = value;
        }
    }

    #endregion

}