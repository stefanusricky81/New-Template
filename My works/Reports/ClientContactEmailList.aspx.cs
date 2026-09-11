using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class Reports_ClientContactEmailList : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
            litMessage.Text = "";
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            phSearchResults.Visible = true;
            BindGrid();
            rgClientEmailCategory.DataBind();
        }
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        Response.Redirect("ClientContactEmailList.aspx");
    }

    protected void rgClientEmailCategory_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        BindGrid();
    }

    protected void lbexport_Click(object sender, EventArgs e)
    {
        rgClientEmailCategory.ExportSettings.IgnorePaging = true;
        rgClientEmailCategory.ExportSettings.ExportOnlyData = true;
        rgClientEmailCategory.ExportSettings.HideStructureColumns = true;

        rgClientEmailCategory.GridLines = GridLines.Both;
        rgClientEmailCategory.BorderStyle = BorderStyle.Solid;

        if (ddlClientCategory.ClientEmailCategoryId != null && ddlClientContactCategory.ClientEmailCategoryId != null)
            rgClientEmailCategory.ExportSettings.FileName = "Email Category_Client_" + ddlClientCategory.ClientEmailCategoryName + "-Client_Contact_" + ddlClientContactCategory.ClientEmailCategoryName + "_" + DateTime.Now.ToString("yyyyMMdd-hhmmss");
        else if (ddlClientCategory.ClientEmailCategoryId != null && ddlClientContactCategory.ClientEmailCategoryId == null)
            rgClientEmailCategory.ExportSettings.FileName = "Email Category_Client_" + ddlClientCategory.ClientEmailCategoryName + "_" + DateTime.Now.ToString("yyyyMMdd-hhmmss");
        else if (ddlClientCategory.ClientEmailCategoryId == null && ddlClientContactCategory.ClientEmailCategoryId != null)
            rgClientEmailCategory.ExportSettings.FileName = "Email Category_Client_Contact_" + ddlClientContactCategory.ClientEmailCategoryName + "_" + DateTime.Now.ToString("yyyyMMdd-hhmmss");

        rgClientEmailCategory.MasterTableView.ExportToExcel();
    }

    private void BindGrid()
    {
        int? _clientcategoryid = null;
        int? _cccategoryid = null;
        int? _client = null;

        if (ddlClientCategory.ClientEmailCategoryId.HasValue)
            _clientcategoryid = ddlClientCategory.ClientEmailCategoryId;
        if (ddlClientContactCategory.ClientEmailCategoryId.HasValue)
            _cccategoryid = ddlClientContactCategory.ClientEmailCategoryId;
        if (ddlClient.ClientId.HasValue)
            _client = ddlClient.ClientId;

        rgClientEmailCategory.DataSource = DesktopShared.ClientEmailCategory.GetEmailList(_clientcategoryid, _cccategoryid, _client);
    }


    protected void lbSendTest_Click(object sender, EventArgs e)
    {
        //StringBuilder sbBody = new StringBuilder();
        string _mailTo = txtSendTest.Text.Trim();
        string _mailSubject = txtSubject.Text;
        string _mailFrom = System.Web.Configuration.WebConfigurationManager.AppSettings["ClientContactEmailFrom"];
        string err = string.Empty; 
        //sbBody.Append(txtBody.Text);

        if (DesktopShared.Email.SendEmail2(_mailFrom, _mailSubject, _mailTo, string.Empty, txtBody.Text + DesktopShared.Email.GetDisclaimer(), ref err, null) == false)
        {
            DesktopShared.Bootstrap.Alert.DisplayMessage(litMessage, err, DesktopShared.Bootstrap.Alert.AlertType.Danger, false);
            //DisplayMessage("Error Send Email", litMessage, DesktopShared.Bootstrap.Alert.AlertType.Danger);
            return;
        }
        else
        {
            DesktopShared.Bootstrap.Alert.DisplayMessage(litMessage, "Email Succesfully Send", DesktopShared.Bootstrap.Alert.AlertType.Success, false);
        }
    }

    protected void lbSendEmail_Click(object sender, EventArgs e)
    {
        int _count = 0;
        int pid = 0;
        string errormessage = string.Empty;
        List<string> Email = new List<string>();
        //StringBuilder sbBody = new StringBuilder();
        string _mailSubject = txtSubject.Text;
        string _mailFrom = System.Web.Configuration.WebConfigurationManager.AppSettings["ClientContactEmailFrom"];
        string err = string.Empty;
        //sbBody.Append(txtBody.Text);
        try
        {
            foreach (GridDataItem _assign in rgClientEmailCategory.MasterTableView.Items)
            {
                CheckBox chk = (CheckBox)_assign.FindControl("cboxSelect");
                if (chk.Checked == true)
                {
                    _count++;
                    string _emailto = _assign.GetDataKeyValue("Email").ToString().Trim();
                    string _clientcontact= _assign.GetDataKeyValue("pclient_contact").ToString().Trim();

                    Email.Add(_emailto);

                    if (DesktopShared.Email.SendEmail2(_mailFrom, _mailSubject, _emailto, string.Empty, txtBody.Content + DesktopShared.Email.GetDisclaimer(), ref err, null) == false)
                    {
                        DesktopShared.Bootstrap.Alert.DisplayMessage(litMessage, err, DesktopShared.Bootstrap.Alert.AlertType.Danger, false);
                        return;
                    }

                    DesktopShared.Client.Contact.AddButtonHistory("Send Email Client Contact", _emailto, Convert.ToInt32(_clientcontact), DesktopShared.User.UserID, DesktopShared.User.UserFullName, txtSubject.Text.Trim(), ref err);
                }
            }
            if (DesktopShared.ClientContactEmailSend.AddEmailSendHistory(_mailSubject, txtBody.Content + DesktopShared.Email.GetDisclaimer(), DesktopShared.User.UserID, _count++, ref pid, ref errormessage))
            {
                if (Email.Count > 0)
                {
                    for (int i = 0; i <= Email.Count - 1; i++)
                    {
                        if (!DesktopShared.ClientContactEmailSend.AddEmailContactHistory(pid, Email[i].ToString().Trim(), DesktopShared.User.UserID, ref errormessage))
                        {
                            DesktopShared.Bootstrap.Alert.DisplayMessage(litMessage, errormessage, DesktopShared.Bootstrap.Alert.AlertType.Danger, false);
                            return;
                        }
                    }
                }
            }
            else
            {
                DesktopShared.Bootstrap.Alert.DisplayMessage(litMessage, errormessage.Trim(), DesktopShared.Bootstrap.Alert.AlertType.Danger, false);
                return;
            }

            DesktopShared.Bootstrap.Alert.DisplayMessage(litMessage, String.Format("Successfully send to {0} {1}", Email.Count, Email.Count > 1 ? "contacts" : "contact"), DesktopShared.Bootstrap.Alert.AlertType.Success, false);
        }
        catch (Exception ex)
        { }
    }


}