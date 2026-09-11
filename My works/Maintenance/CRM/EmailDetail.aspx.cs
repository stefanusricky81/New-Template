using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using System.Data;
using System.Configuration;
using System.Text;

public partial class Maintenance_CRM_EmailDetail : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            SetUpPage();
            DesktopShared.EntityClasses.CrmemailEntity obj = null;
            if (GetEmailId(ref obj))
                LoadValues(obj);
        }
    }


    #region private methods


    /// <summary>
    /// load details
    /// </summary>
    /// <param name="obj"></param>
    private void LoadValues(DesktopShared.EntityClasses.CrmemailEntity obj)
    {
        

        bool _isNew = ((obj == null) || (obj.Fields.State != SD.LLBLGen.Pro.ORMSupportClasses.EntityState.Fetched));

        if (_isNew)
        {            
            return;
        }


        int EmailId = obj.Id;
        txtDisplayName.Text = obj.EmailName.Trim();
        txtEmailSubject.Text = obj.EmailSubject.Trim();
        ddlLeadProduct.LeadProductId = obj.CrmleadProductId;
        ddlSenderEmail.SalesEmailId = obj.CrmsenderEmailId;
        txtEmailContent.Content = obj.EmailBody;
    }

   
    /// <summary>
    /// display message
    /// </summary>
    /// <param name="message"></param>
    /// <param name="alertType"></param>
    /// <param name="overrideHeader"></param>
    private void DisplayMessage(string message, Bootstrap.Alert.AlertType alertType, bool dismissable = true, string overrideHeader = "")
    {
        ScriptManager.RegisterClientScriptBlock(this, Page.GetType(), "ToTheTop", "ToTopOfPage();", true);
        litMessage.Visible = true;
        litMessage.Text = Bootstrap.Alert.GetDivText(alertType, message, dismissable, true, overrideHeader);
    }


    /// <summary>
    /// get ountry id from query string
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    private bool GetEmailId(ref DesktopShared.EntityClasses.CrmemailEntity obj)
    {
        int _id = -1;
        if (int.TryParse(BitByBit.Web.Request.GetString("Id").Trim(), out _id))
        {
            EmailId = _id;
            if (_id == 0) //add new
            {            
                return true;
            }

            obj = new DesktopShared.EntityClasses.CrmemailEntity(_id);

            return true;
        }

        phBreadcrumbs.Visible = false;
        pnlContainer.Visible = false;
        DisplayMessage(String.Format("Unable to retrieve Email.  ID = {0}", _id > 0 ? _id.ToString() : "N/A"), Bootstrap.Alert.AlertType.Danger, false);
        
        return false;
    }

    /// <summary>
    /// set up for initial page load
    /// </summary>
    private void SetUpPage()
    {
     
        txtDisplayName.Focus();

        #region tab index

        short _tabIndex = 0;
        
        btnSubmit.TabIndex = ++_tabIndex;

        #endregion
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
        if (Page.IsValid)
        {
            DateTime CurrentTime = DateTime.Now;
            int CurrentUser = DesktopShared.User.EmployeeID;

            
            try
            {
                DesktopShared.EntityClasses.CrmemailEntity obj_e = (EmailId == 0) ? new DesktopShared.EntityClasses.CrmemailEntity() : new DesktopShared.EntityClasses.CrmemailEntity((int)EmailId);

                #region update Email
                if (EmailId > 0)
                {
                    if (obj_e.IsNew)
                        throw new Exception();
                }
                #endregion
                #region add new Email
                else
                {
                    obj_e.CreatedByUserId = CurrentUser;
                    obj_e.Created = CurrentTime;
                }
                #endregion

                obj_e.EmailName = txtDisplayName.Text.Trim();
                obj_e.CrmsenderEmailId = (int)ddlSenderEmail.SalesEmailId;
                obj_e.EmailBody = txtEmailContent.Content;
                obj_e.EmailSubject = txtEmailSubject.Text.Trim();
                obj_e.CrmleadProductId = (int)ddlLeadProduct.LeadProductId;

                
                obj_e.Lastupdated = CurrentTime;
                obj_e.LastupdatedByUserId = CurrentUser;
                obj_e.Save();

                DisplayMessage(String.Format("Email has been {0} - {1}.{2}",
                EmailId.Value == 0 ? "added" : "updated",
                DateTime.Now,
                EmailId.Value == 0 ? "" : ""
                ),
                Bootstrap.Alert.AlertType.Success);

                EmailId =obj_e.Id;
                LoadValues(obj_e);
            }
            catch (Exception err)
            {
                DisplayMessage(String.Format("Unable to {0} " + " Email. ", EmailId > 0 ? "update" : "add") + "Error = " + err.Message, Bootstrap.Alert.AlertType.Danger, false);
            }
        }
    }

    //protected void cvDisplayName_ServerValidate(object source, ServerValidateEventArgs args)
    //{
    //    cvDisplayName.ErrorMessage = "Email already exists.";
    //    args.IsValid = CRM.Maintenance.IsUniqueAction(txtDisplayName.Text.Trim(), (int)EmailId);

    //}

   
    #endregion


    #region private properties

    /// <summary>
    /// get/set Email id
    /// </summary>
    private int? EmailId
    {
        get
        {
            object obj = this.ViewState["EmailIdForDetail"];
            return (obj == null) ? (int?)null : (int)obj;
        }
        set { this.ViewState["EmailIdForDetail"] = value; }
    }

    #endregion
}

