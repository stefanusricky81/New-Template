using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class UserControl_Grid_EditForm_ClientDomainEdit : System.Web.UI.UserControl
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
    /// <param name="objClientDomain"></param>
    public void LoadValues(DesktopShared.EntityClasses.ClientDomainEntity objClientDomain)
    {
        txtName.Focus();
        btnAdd.Visible = objClientDomain == null;
        btnEdit.Visible = !btnAdd.Visible;

        if (objClientDomain != null)
        {
            ClientDomainId = objClientDomain.Id;
            litHeader.Text = "Edit Client Domain";
            txtName.Text = objClientDomain.Name.Trim();
            txtDescription.Text = objClientDomain.Description.Trim();
            chkActive.Checked = objClientDomain.Active;
        }
        else
        {
            chkActive.Checked = true;
        }
    }

    /// <summary>
    /// save values
    /// </summary>
    /// <param name="clientDomainId"></param>
    public int SaveValues(int? clientDomainId = null)
    {
        DesktopShared.EntityClasses.ClientDomainEntity objClientDomain = null;
        DateTime _auditDate = DateTime.Now;
        int _auditUserId = DesktopShared.User.UserID;

        if (!clientDomainId.HasValue)
        {
            objClientDomain = new DesktopShared.EntityClasses.ClientDomainEntity();
            objClientDomain.ClientId = ClientIdForDomain;
            objClientDomain.Created = _auditDate;
            objClientDomain.CreatedByUserId = _auditUserId;
        }
        else
            objClientDomain = new DesktopShared.EntityClasses.ClientDomainEntity(clientDomainId.Value);

        objClientDomain.Name = txtName.Text.Trim();
        objClientDomain.Description = txtDescription.Text.Trim();
        objClientDomain.Active = chkActive.Checked;
        objClientDomain.LastUpdated = _auditDate;
        objClientDomain.LastUpdatedByUserId = _auditUserId;
        objClientDomain.Save();

        return objClientDomain.Id;
    }

    #endregion

    #region protected events

    #region custom validators

    /// <summary>
    /// validate name is unique
    /// </summary>
    /// <param name="source"></param>
    /// <param name="args"></param>
    protected void cvName_ServerValidate(object source, ServerValidateEventArgs args)
    {
        cvName.ErrorMessage = "Domain is already in use";
        if (!DesktopShared.Client.Domain.IsUniqueName(txtName.Text.Trim(), ClientDomainId))
        {
            args.IsValid = false;
            return;
        }
        cvName.ErrorMessage = "Invalid Domain";
        args.IsValid = DesktopShared.Client.Domain.IsValid(txtName.Text.Trim());
    }

    #endregion

    #endregion

    #region private properties

    /// <summary>
    /// get/set Client Domain id
    /// </summary>
    private int? ClientDomainId
    {
        get
        {
            object obj = this.ViewState["clid_cle"];
            return (obj == null) ? (int?)null : (int)obj;
        }
        set { this.ViewState["clid_cle"] = value; }
    }

    #endregion

    #region public properties

    /// <summary>
    /// get/set Client  id
    /// </summary>
    public int? ClientIdForDomain
    {
        get
        {
            object obj = this.ViewState["cid_cle"];
            return (obj == null) ? (int?)null : (int)obj;
        }
        set { this.ViewState["cid_cle"] = value; }

    }
    #endregion

}