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

public partial class Maintenance_CRM_CompanySizeDetail : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            SetUpPage();
            DesktopShared.EntityClasses.CrmcompanySizeEntity obj = null;
            if (GetCompanySizeId(ref obj))
                LoadValues(obj);
        }
    }


    #region private methods


    /// <summary>
    /// load details
    /// </summary>
    /// <param name="obj"></param>
    private void LoadValues(DesktopShared.EntityClasses.CrmcompanySizeEntity obj)
    {
        

        bool _isNew = ((obj == null) || (obj.Fields.State != SD.LLBLGen.Pro.ORMSupportClasses.EntityState.Fetched));

        if (_isNew)
        {            
            return;
        }


        int CompanySizeId = obj.Id;
        txtCompanySize.Text = obj.Size.Trim();
        
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
    private bool GetCompanySizeId(ref DesktopShared.EntityClasses.CrmcompanySizeEntity obj)
    {
        int _id = -1;
        if (int.TryParse(BitByBit.Web.Request.GetString("Id").Trim(), out _id))
        {
            CompanySizeId = _id;
            if (_id == 0) //add new
            {            
                return true;
            }

            obj = new DesktopShared.EntityClasses.CrmcompanySizeEntity(_id);

            return true;
        }

        phBreadcrumbs.Visible = false;
        pnlContainer.Visible = false;
        DisplayMessage(String.Format("Unable to retrieve Company Size.  ID = {0}", _id > 0 ? _id.ToString() : "N/A"), Bootstrap.Alert.AlertType.Danger, false);
        
        return false;
    }

    /// <summary>
    /// set up for initial page load
    /// </summary>
    private void SetUpPage()
    {
     
        txtCompanySize.Focus();

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

            bool Active = true;
            try
            {
                DesktopShared.EntityClasses.CrmcompanySizeEntity obj_e = (CompanySizeId == 0) ? new DesktopShared.EntityClasses.CrmcompanySizeEntity() : new DesktopShared.EntityClasses.CrmcompanySizeEntity((int)CompanySizeId);

                #region update Company Size
                if (CompanySizeId > 0)
                {
                    if (obj_e.IsNew)
                        throw new Exception();
                }
                #endregion
                #region add new Company Size
                else
                {
                    obj_e.CreatedByUserid = CurrentUser;
                    obj_e.Created = CurrentTime;
                }
                #endregion

                obj_e.Size = txtCompanySize.Text.Trim();


                obj_e.DisplayOrder = 1;
                obj_e.Lastupdated = CurrentTime;
                obj_e.LastupdatedByUserId = CurrentUser;
                obj_e.Save();

                DisplayMessage(String.Format("Company Size has been {0} - {1}.{2}",
                CompanySizeId.Value == 0 ? "added" : "updated",
                DateTime.Now,
                CompanySizeId.Value == 0 ? "" : ""
                ),
                Bootstrap.Alert.AlertType.Success);

                CompanySizeId =obj_e.Id;
                LoadValues(obj_e);
            }
            catch (Exception err)
            {
                DisplayMessage(String.Format("Unable to {0} " + " Company Size. ", CompanySizeId > 0 ? "update" : "add") + "Error = " + err.Message, Bootstrap.Alert.AlertType.Danger, false);
            }
        }
    }

    protected void cvCompanySize_ServerValidate(object source, ServerValidateEventArgs args)
    {
        cvCompanySize.ErrorMessage = "Company Size already exists.";
        args.IsValid = CRM.Maintenance.IsUniqueCompanySize(txtCompanySize.Text.Trim(), (int)CompanySizeId);

    }

   
    #endregion


    #region private properties

    /// <summary>
    /// get/set Industry id
    /// </summary>
    private int? CompanySizeId
    {
        get
        {
            object obj = this.ViewState["CompanySizeIdForDetail"];
            return (obj == null) ? (int?)null : (int)obj;
        }
        set { this.ViewState["CompanySizeIdForDetail"] = value; }
    }

    #endregion
}

