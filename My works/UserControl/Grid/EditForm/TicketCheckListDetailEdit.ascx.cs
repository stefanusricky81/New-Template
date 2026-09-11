using SD.LLBLGen.Pro.ORMSupportClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class UserControl_Grid_EditForm_TicketCheckListDetailEdit : System.Web.UI.UserControl
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
    /// load values
    /// </summary>
    /// <param name="objTicketCheckListDetail"></param>
    /// <param name="ticketCheckListMasterId"></param>
    public void LoadValues(DesktopShared.EntityClasses.TicketCheckListDetailEntity objTicketCheckListDetail, int ticketCheckListMasterId)
    {
        txtTask.Focus();
        btnAdd.Visible = objTicketCheckListDetail == null;
        btnEdit.Visible = !btnAdd.Visible;

        if (objTicketCheckListDetail != null)
        {
            litHeader.Text = "Edit task";
            txtTask.Text = objTicketCheckListDetail.Task.Trim();
            txtOrder.Text = objTicketCheckListDetail.DisplayOrder.HasValue ? objTicketCheckListDetail.DisplayOrder.Value.ToString() : "";
            chkActive.Checked = objTicketCheckListDetail.Active;
        }
        else
        {
            chkActive.Checked = true;
            txtOrder.Text = DesktopShared.Ticket.CheckList.Detail.GetNextDisplayOrder(ticketCheckListMasterId).ToString();
        }
    }

    /// <summary>
    /// save values
    /// </summary>
    /// <param name="ticketCheckListDetail"></param>
    /// <param name="ticketCheckListMasterId"></param>
    /// <returns></returns>
    public int SaveValues(int? ticketCheckListDetail, int ticketCheckListMasterId)
    {
        DesktopShared.EntityClasses.TicketCheckListDetailEntity objTicketCheckListDetail = ticketCheckListDetail.HasValue ? new DesktopShared.EntityClasses.TicketCheckListDetailEntity(ticketCheckListDetail.Value) : new DesktopShared.EntityClasses.TicketCheckListDetailEntity();
        DateTime _auditDate = DateTime.Now;
        int _auditUserId = DesktopShared.User.UserID;
        int _newDisplayOrder = Convert.ToInt32(txtOrder.Text.Trim());
        int _oldDisplayOrder = -1;

        if (!ticketCheckListDetail.HasValue)
        {
            objTicketCheckListDetail.TicketCheckListMasterId = ticketCheckListMasterId;
            objTicketCheckListDetail.Created = _auditDate;
            objTicketCheckListDetail.CreatedByUserId = _auditUserId;
        }
        else
        {
            _oldDisplayOrder = objTicketCheckListDetail.DisplayOrder.HasValue ? objTicketCheckListDetail.DisplayOrder.Value : -1;
        }

        objTicketCheckListDetail.Task = txtTask.Text.Trim();
        objTicketCheckListDetail.DisplayOrder = _newDisplayOrder;
        objTicketCheckListDetail.Active = chkActive.Checked;
        objTicketCheckListDetail.LastUpdated = _auditDate;
        objTicketCheckListDetail.LastUpdatedByUserId = _auditUserId;
        objTicketCheckListDetail.Save();

        DesktopShared.Ticket.CheckList.Detail.UpdateOrder(objTicketCheckListDetail.Id, ticketCheckListMasterId, _oldDisplayOrder, _newDisplayOrder);
        
        return objTicketCheckListDetail.Id;
    }

    #endregion

    #region protected events

    #region custom validators

    /// <summary>
    /// validate task entered
    /// </summary>
    /// <param name="source"></param>
    /// <param name="args"></param>
    protected void cvTask_ServerValidate(object source, ServerValidateEventArgs args)
    {
        args.IsValid = txtTask.Text.Trim().Length > 1;
    }

    #endregion

    #endregion
}