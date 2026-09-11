using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class UserControl_Grid_EditForm_ClientDocumentEdit : System.Web.UI.UserControl
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
    /// <param name="objClientDocument"></param>
    public void LoadValues(DesktopShared.TypedListClasses.ClientDocumentsRow objClientDocument)
    {
        if (objClientDocument == null)
        {
            btnEdit.Visible = false;
            ScriptManager.RegisterClientScriptBlock(this, Page.GetType(), "ToTheTop", "ToTopOfPage();", true);
            litMessage.Visible = true;
            litMessage.Text = Bootstrap.Alert.GetDivText(Bootstrap.Alert.AlertType.Danger, "Document Entity is required.", false, true);
            return;
        }

        txtName.Focus();
        litHeader.Text = "Edit Client Document";

        ClientDocumentId = objClientDocument.Id;
        txtName.Text = objClientDocument.Name.Trim();
        ddlClientDocumentType.ClientDocumentTypeId = objClientDocument.ClientDocumentTypeId;
        txtDescription.Text = objClientDocument.Description.Trim();
        chkActive.Checked = objClientDocument.Active;
        //todo report date
           
    }

    /// <summary>
    /// save values
    /// </summary>
    /// <param name="clientDoccumentId"></param>
    public int SaveValues(int clientDoccumentId)
    {
        DesktopShared.EntityClasses.ClientDocumentEntity objClientDocument = new DesktopShared.EntityClasses.ClientDocumentEntity(clientDoccumentId);

        objClientDocument.Name = txtName.Text.Trim();
        objClientDocument.ClientDocumentTypeId = ddlClientDocumentType.ClientDocumentTypeId;
        objClientDocument.Description = txtDescription.Text.Trim();
        objClientDocument.Active = chkActive.Checked;
        objClientDocument.Save();
        //todo report date

        return objClientDocument.Id;
    }

    #endregion

    #region protected events

    #endregion

    #region private properties

    /// <summary>
    /// get/set client document  id
    /// </summary>
    private int? ClientDocumentId
    {
        get
        {
            object obj = this.ViewState["cdid_ed"];
            return (obj == null) ? (int?)null : (int)obj;
        }
        set { this.ViewState["cdid_ed"] = value; }
    }

    #endregion
}