using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class UserControl_Grid_EditForm_SystemEmailEdit : System.Web.UI.UserControl
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
    /// <param name="objSystemEmail"></param>
    public void LoadValues(DesktopShared.EntityClasses.SystemEmailEntity objSystemEmail)
    {
        txtFrom.Focus();
        btnAdd.Visible = objSystemEmail == null;
        btnEdit.Visible = !btnAdd.Visible;

        if (objSystemEmail != null)
        {
            SystemEmailId = objSystemEmail.Id;
            litHeader.Text = "Edit System Email";
            litName.Text = objSystemEmail.Name.Trim();
            txtFrom.Text = objSystemEmail.From.Trim();
            txtSubject.Text = objSystemEmail.Subject.Trim();
            txtCc.Text = objSystemEmail.Cc.Trim();
            txtBcc.Text = objSystemEmail.Bcc.Trim();
            txtBody.Text = objSystemEmail.Body.Trim();
            txtDescription.Text = objSystemEmail.Description.Trim();
        }
    }

    /// <summary>
    /// save values
    /// </summary>
    /// <param name="systemEmailId"></param>
    public int SaveValues(int systemEmailId)
    {
        DesktopShared.EntityClasses.SystemEmailEntity objSystemEmail = new DesktopShared.EntityClasses.SystemEmailEntity(systemEmailId);
        
        objSystemEmail.From = txtFrom.Text.Trim();
        objSystemEmail.Subject = txtSubject.Text.Trim();
        objSystemEmail.Cc = txtCc.Text.Trim();
        objSystemEmail.Bcc = txtBcc.Text.Trim();
        objSystemEmail.Body = txtBody.Text.Trim();
        objSystemEmail.Description = txtDescription.Text.Trim();
        objSystemEmail.LastUpdated = DateTime.Now;
        objSystemEmail.LastUpdatedByUserId = DesktopShared.User.UserID;
        objSystemEmail.Save();

        return objSystemEmail.Id;
    }

    #endregion

    #region protected events

    #region custom validators

    /// <summary>
    /// validate email address
    /// </summary>
    /// <param name="source"></param>
    /// <param name="args"></param>
    protected void cvEmail_ServerValidate(object source, ServerValidateEventArgs args)
    {
        args.IsValid = true;
        string _emailText= args.Value.Trim().Replace(";", ",");
        string[] _allEmail = _emailText.Split(',');
        foreach (string _email in _allEmail)
        {
            if (!DesktopShared.Utility.IsValidEmailAddress(_email))
            {
                args.IsValid = false;
                ScriptManager.RegisterClientScriptBlock(this, Page.GetType(), "ToTheTop", "ToTopOfPage();", true);
                return;
            }
        }

    }

    #endregion

    #endregion

    #region private properties

    /// <summary>
    /// get/set system email id
    /// </summary>
    private int? SystemEmailId
    {
        get
        {
            object obj = this.ViewState["seid_ed"];
            return (obj == null) ? (int?)null : (int)obj;
        }
        set { this.ViewState["seid_ed"] = value; }
    }

    #endregion
}