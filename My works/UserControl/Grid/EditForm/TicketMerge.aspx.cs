using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class UserControl_Grid_EditForm_TicketMerge : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
            SetUpPage();
    }

    #region private methods

    /// <summary>
    /// set up page for initial page load -> display list of selected tickets to be merged
    /// </summary>
    private void SetUpPage()
    {
        string _ids = BitByBit.Web.Request.GetString("Ids");
        litIds.Text = _ids;

        if (_ids.Contains(","))
            litTicketsText.Text = "tickets";
    }

    /// <summary>
    /// call close window function which also refreshes parent page
    /// </summary>
    private void CloseWindow()
    {
        Page.ClientScript.RegisterStartupScript(Page.GetType(), "mykey", "Close();", true);
    }


    #endregion

    #region protected events

    /// <summary>
    /// submit button on click
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (this.IsValid)
        {
            int masterTicketId = Convert.ToInt32(tbMasterTicketId.Text.Trim()); 

            string[] allValues = litIds.Text.Trim().Split(',');
            foreach (string currentValue in allValues)
            {
                int? currentTicketId = null;
                try { currentTicketId = Convert.ToInt32(currentValue); }
                catch { currentTicketId = null; }

                if (currentTicketId.HasValue)
                {
                    //note - right now we just silently ignore tickets that fail validation and process those that do
                    string errorMessage = "";
                    
                    if (DesktopShared.Ticket.Merge.IsValid(currentTicketId.Value, masterTicketId, ref errorMessage))
                        DesktopShared.Ticket.Merge.CompleteMerge(currentTicketId.Value, masterTicketId, DesktopShared.User.UserID);
                }
            }

            CloseWindow();
        }
    }

    /// <summary>
    /// close button on click
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnClose_Click(object sender, EventArgs e)
    {
        CloseWindow();
    }

    #region custom validators

    /// <summary>
    /// validate merge master ticket is valid
    /// </summary>
    /// <param name="source"></param>
    /// <param name="args"></param>
    protected void cvMasterTicketId_ServerValidate(object source, ServerValidateEventArgs args)
    {
        int _mergeMasterTicketId = Convert.ToInt32(tbMasterTicketId.Text);
        DesktopShared.EntityClasses.CscDefectsEntity _masterTicket = new DesktopShared.EntityClasses.CscDefectsEntity(_mergeMasterTicketId);
        args.IsValid = (_masterTicket.Fields.State == SD.LLBLGen.Pro.ORMSupportClasses.EntityState.Fetched);
    }

    #endregion

    #endregion

}