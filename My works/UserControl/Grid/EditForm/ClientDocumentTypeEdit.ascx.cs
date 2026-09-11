using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class UserControl_Grid_EditForm_ClientDocumentTypeEdit : System.Web.UI.UserControl
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
    /// <param name="objClientDocumentType"></param>
    public void LoadValues(DesktopShared.EntityClasses.ClientDocumentTypeEntity objClientDocumentType)
    {
        txtName.Focus(); 
        btnAdd.Visible = objClientDocumentType == null;
        btnEdit.Visible = !btnAdd.Visible;

        if (objClientDocumentType != null)
        {
            ClientDocumentTypeId = objClientDocumentType.Id;
            litHeader.Text = "Edit Client Document Type";
            txtName.Text = objClientDocumentType.Name.Trim();
            txtDescription.Text = objClientDocumentType.Description.Trim();
            chkActive.Checked = objClientDocumentType.Active;

            if (objClientDocumentType.DisableEdit)
            {
                btnEdit.Visible = false;

                ScriptManager.RegisterClientScriptBlock(this, Page.GetType(), "ToTheTop", "ToTopOfPage();", true);
                litMessage.Visible = true;
                litMessage.Text = Bootstrap.Alert.GetDivText(Bootstrap.Alert.AlertType.Warning, "Document Type cannot be modified.", false, true);
            }
        }
        else
        {
            litHeader.Text = "Add Client Document Type";
            chkActive.Checked = true;
        }
    }

    /// <summary>
    /// save values
    /// </summary>
    /// <param name="clientDoccumentTypeId"></param>
    public int SaveValues(int? clientDoccumentTypeId = null)
    {
        DesktopShared.EntityClasses.ClientDocumentTypeEntity objClientDocumentType = clientDoccumentTypeId.HasValue ? new DesktopShared.EntityClasses.ClientDocumentTypeEntity(clientDoccumentTypeId.Value) : new DesktopShared.EntityClasses.ClientDocumentTypeEntity();
        DateTime _now = DateTime.Now;
        int _userId = DesktopShared.User.UserID;

        objClientDocumentType.Name = txtName.Text.Trim();
        objClientDocumentType.Description = txtDescription.Text.Trim();
        objClientDocumentType.Active = chkActive.Checked;
        objClientDocumentType.LastUpdated = _now;
        objClientDocumentType.LastUpdatedByUserId = _userId;
        if (!clientDoccumentTypeId.HasValue)
        {
            objClientDocumentType.Created = _now;
            objClientDocumentType.CreatedByUserId = _userId;
        }
        objClientDocumentType.Save();

        return objClientDocumentType.Id;
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
        args.IsValid = DesktopShared.Client.Document.Type.IsUniqueName(txtName.Text.Trim(), ClientDocumentTypeId);
    }

    #endregion

    #endregion

    #region private properties

    /// <summary>
    /// get/set client document type id
    /// </summary>
    private int? ClientDocumentTypeId
    {
        get
        {
            object obj = this.ViewState["cdtid_ed"];
            return (obj == null) ? (int?)null : (int)obj;
        }
        set { this.ViewState["cdtid_ed"] = value; }
    }

    #endregion
}