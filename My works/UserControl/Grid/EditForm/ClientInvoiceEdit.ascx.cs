using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class UserControl_Grid_EditForm_ClientInvoiceEdit : System.Web.UI.UserControl
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
    /// <param name="objClientInvoice"></param>
    public void LoadValues(DesktopShared.EntityClasses.ClientInvoiceEntity objClientInvoice)
    {
        txtNumber.Focus();
        //string _scrollTo = String.Format("document.getElementById('{0}').scrollIntoView()", txtDescription.ClientID);
        //ScriptManager.RegisterStartupScript(Page, typeof(Page), "ScrollToInvoice", String.Format("setTimeout({0}, 1);", _scrollTo), true);

        btnAdd.Visible = objClientInvoice == null;
        btnEdit.Visible = !btnAdd.Visible;
        phExisting.Visible = !btnAdd.Visible;
        if (objClientInvoice != null)
        {
            ClientInvoiceId = objClientInvoice.Id;
            litHeader.Text = "Edit Client Invoice";
            txtNumber.Text = objClientInvoice.InvoiceNumber.Trim();
            ucInvoiceDate.SelectedDate = objClientInvoice.BillDate;
            litInvoiceDate.Text = objClientInvoice.BillDate.HasValue ? String.Format("<p class=\"form-control-static\">{0}</p>", objClientInvoice.BillDate.Value.ToString("MM/dd/yyyy")) : "";
            txtAmount.Text = objClientInvoice.Amount.ToString("C").Replace("$", "");
            txtDescription.Text = objClientInvoice.Description.Trim();
            txtEmail.Text = objClientInvoice.EmailTo.Trim();
            litPaid.Text = objClientInvoice.Paid ? "Yes" : "No";
            litLink.Text = String.Format("{0}/Default.aspx?InvId={1}", BitByBit.Configuration.GetConfigString("PaymentPortalUrl", "https://paymentinfo.bitxbit.com"), objClientInvoice.Guid.Trim());
            mvAttachment.ActiveViewIndex = String.IsNullOrWhiteSpace(objClientInvoice.AzureFileId) ? 0 : 1;

            if (objClientInvoice.Paid)
            {
                btnEdit.Visible = false;
                if (objClientInvoice.PaidDate.HasValue)
                {
                    txtNumber.Enabled = false;
                    ucInvoiceDate.Visible = false;
                    litInvoiceDate.Visible = true;
                    txtAmount.Enabled = false;
                    txtEmail.Enabled = false;
                    txtDescription.Enabled = false;
                    litPaid.Text += String.Format(" - {0}", objClientInvoice.PaidDate.Value);

                    litNumberRequired.Visible = false;
                    litDateRequired.Visible = false;
                    litAmountRequired.Visible = false;
                    litEmailRequired.Visible = false;
                    litDescriptionRequired.Visible = false;
                    if (String.IsNullOrWhiteSpace(objClientInvoice.AzureFileId))
                        mvAttachment.SetActiveView(viewAttachmentNone);
                    btnDeleteAttachment.Visible = false;
                }
            }
        }
        else
        {
            ucInvoiceDate.SelectedDate = DateTime.Now;

            //billing admins
            var _users = DesktopShared.StoredProcedureCallerClasses.RetrievalProcedures.ProcGetClientContactsByRole(ClientIdForInvoice.Value, DesktopShared.Role.Portal.Id.BillingAdministrator);
            System.Text.StringBuilder _sb = new System.Text.StringBuilder();
            foreach (DataRow objUser in _users.Rows)
            {
                string _adminEmail = objUser["email"].ToString().Trim();
                if (!String.IsNullOrWhiteSpace(_adminEmail))
                {
                    if (_sb.Length > 0)
                        _sb.Append(",");
                    _sb.Append(_adminEmail);
                }
            }
            txtEmail.Text = _sb.ToString();
            
        }
    }

    /// <summary>
    /// save values
    /// </summary>
    /// <param name="fileUploadError"></param>
    /// <param name="fileUploadErrorMessage"></param>
    /// <param name="clientInvoiceId"></param>
    /// <returns></returns>
    public int SaveValues(ref bool fileUploadError, ref string fileUploadErrorMessage, int? clientInvoiceId = null)
    {
        DesktopShared.EntityClasses.ClientInvoiceEntity objClientInvoice = null;
        DateTime _auditDate = DateTime.Now;
        int _auditUserId = DesktopShared.User.UserID;

        if (!clientInvoiceId.HasValue)
        {
            objClientInvoice = new DesktopShared.EntityClasses.ClientInvoiceEntity();
            objClientInvoice.ClientId = ClientIdForInvoice;
            objClientInvoice.Created = _auditDate;
            objClientInvoice.CreatedByUserId = _auditUserId;
            objClientInvoice.Guid = Guid.NewGuid().ToString();
        }
        else
            objClientInvoice = new DesktopShared.EntityClasses.ClientInvoiceEntity(clientInvoiceId.Value);

        objClientInvoice.InvoiceNumber = txtNumber.Text.Trim();
        objClientInvoice.BillDate = ucInvoiceDate.SelectedDate;
        objClientInvoice.Amount = Convert.ToDecimal(txtAmount.Text);
        objClientInvoice.EmailTo = txtEmail.Text.Trim();
        objClientInvoice.Description = txtDescription.Text.Trim();
        objClientInvoice.LastUpdated = _auditDate;
        objClientInvoice.LastUpdatedByUserId = _auditUserId;
        objClientInvoice.Save();

        #region file upload

        #region telerirk

        /*
        string _errorMessage = "";
        foreach (UploadedFile _file in rauMain.UploadedFiles)
        {
            try
            {
                #region upload to azure

                string _azureFileId = "";
                System.IO.MemoryStream _ms = new System.IO.MemoryStream();
                _file.InputStream.CopyTo(_ms);
                _ms.Seek(0, System.IO.SeekOrigin.Begin);

                bool _uploaded = DesktopShared.AzureHelper.UploadBlob(DesktopShared.AzureHelper.ContainerName.invoice, ClientIdForInvoice.Value.ToString(), _file.GetExtension(), _ms, ref _azureFileId, ref _errorMessage);

                if (_ms != null)
                    _ms.Dispose();

                #endregion

                if (!_uploaded)
                {
                    fileUploadError = true;
                    fileUploadErrorMessage = _errorMessage;
                }
                else
                {
                    objClientInvoice.AzureFileId = _azureFileId.Trim();
                    objClientInvoice.AzureContainerName = DesktopShared.AzureHelper.ContainerName.invoice.ToString();
                    objClientInvoice.FileExtension = _file.GetExtension();
                    objClientInvoice.FileName = _file.GetNameWithoutExtension();
                    objClientInvoice.Save();
                }
            }
            catch (Exception ex)
            {
                fileUploadError = true;
                fileUploadErrorMessage = ex.ToString();
            }
        }
        */

        #endregion

        #region asp.net file upload

        string _errorMessage = "";
        if (fuOne.PostedFile != null && fuOne.PostedFile.ContentLength > 0)
        {
            try
            {
                #region upload to azure

                string _extension = System.IO.Path.GetExtension(fuOne.FileName);
                string _azureFileId = "";
                System.IO.MemoryStream _ms = new System.IO.MemoryStream();
                fuOne.PostedFile.InputStream.CopyTo(_ms);
                _ms.Seek(0, System.IO.SeekOrigin.Begin);

                bool _uploaded = DesktopShared.AzureHelper.UploadBlob(DesktopShared.AzureHelper.ContainerName.invoice, ClientIdForInvoice.Value.ToString(), _extension, _ms, ref _azureFileId, ref _errorMessage);

                if (_ms != null)
                    _ms.Dispose();

                #endregion

                if (!_uploaded)
                {
                    fileUploadError = true;
                    fileUploadErrorMessage = _errorMessage;
                }
                else
                {
                    objClientInvoice.AzureFileId = _azureFileId.Trim();
                    objClientInvoice.AzureContainerName = DesktopShared.AzureHelper.ContainerName.invoice.ToString();
                    objClientInvoice.FileExtension = _extension;
                    objClientInvoice.FileName = System.IO.Path.GetFileNameWithoutExtension(fuOne.FileName);
                    objClientInvoice.Save();
                }
            }
            catch (Exception ex)
            {
                fileUploadError = true;
                fileUploadErrorMessage = ex.ToString();
            }
        }

        #endregion

        #endregion

        return objClientInvoice.Id;
    }

    #endregion

    #region protected events

    /// <summary>
    /// download attachment on button click
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnDownloadAttachment_Click(object sender, EventArgs e)
    {
        var objClientInvoice = new DesktopShared.EntityClasses.ClientInvoiceEntity(ClientInvoiceId.Value);
        string _errorMessage = "";

        string _containerName = objClientInvoice.AzureContainerName.Trim();
        DesktopShared.AzureHelper.ContainerName _containerNameEnum = DesktopShared.AzureHelper.ContainerName.client;
        if (!Enum.TryParse(_containerName, out _containerNameEnum))
        {
            _errorMessage = "Unable to parse Container Name";
            DesktopShared.Bootstrap.Alert.DisplayMessage(litMessage, String.Format("Unable to get File - {0}", _errorMessage), DesktopShared.Bootstrap.Alert.AlertType.Danger);
            return;
        }

        System.IO.MemoryStream _ms = DesktopShared.AzureHelper.GetBlobAsStream(_containerNameEnum, objClientInvoice.AzureFileId, ref _errorMessage);

        if (_ms == null)
        {
            DesktopShared.Bootstrap.Alert.DisplayMessage(litMessage, String.Format("Unable to get File - {0}", _errorMessage), DesktopShared.Bootstrap.Alert.AlertType.Danger);
            return;
        }
        else
        {
            string _fullFileName = String.Format("{0}{1}", objClientInvoice.FileName.Trim(), objClientInvoice.FileExtension.Trim());
            string _headerValue = String.Format("attachment;filename={0}", _fullFileName);
            Response.ContentType = MimeMapping.GetMimeMapping(_fullFileName);
            Response.AddHeader("content-disposition", _headerValue);
            Response.Buffer = true;
            _ms.WriteTo(Response.OutputStream);
            Response.End();
        }
    }

    /// <summary>
    /// delete attachment on button click
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnDeleteAttachment_Click(object sender, EventArgs e)
    {
        var objClientInvoice = new DesktopShared.EntityClasses.ClientInvoiceEntity(ClientInvoiceId.Value);

        string _containerName = objClientInvoice.AzureContainerName.Trim();
        DesktopShared.AzureHelper.ContainerName _containerNameEnum = DesktopShared.AzureHelper.ContainerName.client;
        if (Enum.TryParse(_containerName, out _containerNameEnum))
            DesktopShared.AzureHelper.DeleteBlob(_containerNameEnum, objClientInvoice.AzureFileId.Trim());

        objClientInvoice.AzureFileId = null;
        objClientInvoice.AzureContainerName = null;
        objClientInvoice.FileExtension = null;
        objClientInvoice.FileName = null;
        objClientInvoice.Save();

        LoadValues(objClientInvoice);
    }

    #region custom validators

    /// <summary>
    /// validate at least one 1 file has been upload
    /// </summary>
    /// <param name="server"></param>
    /// <param name="e"></param>
    protected void cvFile_ServerValidate(object server, ServerValidateEventArgs args)
    {
        #region asp.net file upload

        if (fuOne.PostedFile != null && fuOne.PostedFile.ContentLength > 0)
        {
            List<string> _allowedExtensions = new List<string>();
            _allowedExtensions.Add("pdf");
            cvFile.ErrorMessage = String.Format("Invalid File Upload. Allowed extensions: {0}", string.Join(",", _allowedExtensions.ToArray()));
            args.IsValid = _allowedExtensions.Contains(System.IO.Path.GetExtension(fuOne.PostedFile.FileName).Replace(".", "").ToLower());
        }

        #endregion
    }

    /// <summary>
    /// validate name is unique
    /// </summary>
    /// <param name="source"></param>
    /// <param name="args"></param>
    protected void cvNumber_ServerValidate(object source, ServerValidateEventArgs args)
    {
        args.IsValid = true;
        if (!DesktopShared.Client.Invoice.IsUniqueNumber(txtNumber.Text.Trim(), ClientInvoiceId))
        {
            args.IsValid = false;
            return;
        }
    }

    /// <summary>
    /// validate email to
    /// </summary>
    /// <param name="source"></param>
    /// <param name="args"></param>
    protected void cvEmail_ServerValidate(object source, ServerValidateEventArgs args)
    {
        args.IsValid = DesktopShared.Utility.IsValidEmailAddress(txtEmail.Text.Trim());
    }

    #endregion

    #endregion

    #region private properties

    /// <summary>
    /// get/set Client Invoice id
    /// </summary>
    private int? ClientInvoiceId
    {
        get
        {
            object obj = this.ViewState["clid_cie"];
            return (obj == null) ? (int?)null : (int)obj;
        }
        set { this.ViewState["clid_cie"] = value; }
    }

    #endregion

    #region public properties

    /// <summary>
    /// get/set Client  id
    /// </summary>
    public int? ClientIdForInvoice
    {
        get
        {
            object obj = this.ViewState["cid_cie"];
            return (obj == null) ? (int?)null : (int)obj;
        }
        set { this.ViewState["cid_cie"] = value; }

    }
    #endregion




  
}