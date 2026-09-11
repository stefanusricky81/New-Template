using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class UserControl_Grid_EditForm_ClientSetupEmailMessageEdit : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    public void LoadValues(DesktopShared.EntityClasses.ClientSetupInvitationEmailEntity objClientSetupEmail)
    {
        txtBody.Focus();
        btnAdd.Visible = objClientSetupEmail == null;
        btnEdit.Visible = !btnAdd.Visible;

        if (objClientSetupEmail != null)
        {
            ClientSetupEmailId = (int)objClientSetupEmail.Id;
            litHeader.Text = "Edit System Email";
            txtBody.Text = objClientSetupEmail.EmailMessage.Trim();
        }
    }

    /// <summary>
    /// save values
    /// </summary>
    /// <param name="ClientsetupEmailId"></param>
    public int SaveValues(int systemEmailId)
    {
        DesktopShared.EntityClasses.ClientSetupInvitationEmailEntity objClientSetupEmail = new DesktopShared.EntityClasses.ClientSetupInvitationEmailEntity(systemEmailId);

        objClientSetupEmail.EmailMessage = txtBody.Text.Trim();
        objClientSetupEmail.CreatedDate = DateTime.Now;
        objClientSetupEmail.CreatedBy= DesktopShared.User.UserID;
        objClientSetupEmail.LastUpdateDate = DateTime.Now;
        objClientSetupEmail.LastUpdateBy = DesktopShared.User.UserID;
        objClientSetupEmail.Save();

        return (int)objClientSetupEmail.Id;
    }

    #region private properties

    /// <summary>
    /// get/set Client setup email id
    /// </summary>
    private int? ClientSetupEmailId
    {
        get
        {
            object obj = this.ViewState["csem_id"];
            return (obj == null) ? (int?)null : (int)obj;
        }
        set { this.ViewState["csem_id"] = value; }
    }

    #endregion
}