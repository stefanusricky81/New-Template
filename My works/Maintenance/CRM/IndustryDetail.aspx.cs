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

public partial class Maintenance_CRM_IndustryDetail : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            SetUpPage();
            DesktopShared.EntityClasses.CrmindustryEntity obj = null;
            if (GetIndustryId(ref obj))
                LoadValues(obj);
        }
    }


    #region private methods


    /// <summary>
    /// load details
    /// </summary>
    /// <param name="obj"></param>
    private void LoadValues(DesktopShared.EntityClasses.CrmindustryEntity obj)
    {
        

        bool _isNew = ((obj == null) || (obj.Fields.State != SD.LLBLGen.Pro.ORMSupportClasses.EntityState.Fetched));

        if (_isNew)
        {            
            return;
        }


        int IndustryId = obj.Id;
        txtIndustry.Text = obj.Industry.Trim();
        
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
    private bool GetIndustryId(ref DesktopShared.EntityClasses.CrmindustryEntity obj)
    {
        int _id = -1;
        if (int.TryParse(BitByBit.Web.Request.GetString("Id").Trim(), out _id))
        {
            IndustryId = _id;
            if (_id == 0) //add new
            {            
                return true;
            }

            obj = new DesktopShared.EntityClasses.CrmindustryEntity(_id);

            return true;
        }

        phBreadcrumbs.Visible = false;
        pnlContainer.Visible = false;
        DisplayMessage(String.Format("Unable to retrieve Industry.  ID = {0}", _id > 0 ? _id.ToString() : "N/A"), Bootstrap.Alert.AlertType.Danger, false);
        
        return false;
    }

    /// <summary>
    /// set up for initial page load
    /// </summary>
    private void SetUpPage()
    {
     
        txtIndustry.Focus();

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
                DesktopShared.EntityClasses.CrmindustryEntity obj_e = (IndustryId == 0) ? new DesktopShared.EntityClasses.CrmindustryEntity() : new DesktopShared.EntityClasses.CrmindustryEntity((int)IndustryId);

                #region update Industry
                if (IndustryId > 0)
                {
                    if (obj_e.IsNew)
                        throw new Exception();
                }
                #endregion
                #region add new Industry
                else
                {
                    obj_e.CreatedByUserId = CurrentUser;
                    obj_e.Created = CurrentTime;
                }
                #endregion

                obj_e.Industry = txtIndustry.Text.Trim();

                obj_e.Active = true;
                
                obj_e.Lastupdated = CurrentTime;
                obj_e.LastupdatedByUserId = CurrentUser;
                obj_e.Save();

                DisplayMessage(String.Format("Industry has been {0} - {1}.{2}",
                IndustryId.Value == 0 ? "added" : "updated",
                DateTime.Now,
                IndustryId.Value == 0 ? "" : ""
                ),
                Bootstrap.Alert.AlertType.Success);

                IndustryId =obj_e.Id;
                LoadValues(obj_e);
            }
            catch (Exception err)
            {
                DisplayMessage(String.Format("Unable to {0} " + " Industry. ", IndustryId > 0 ? "update" : "add") + "Error = " + err.Message, Bootstrap.Alert.AlertType.Danger, false);
            }
        }
    }

    protected void cvIndustry_ServerValidate(object source, ServerValidateEventArgs args)
    {
        cvIndustry.ErrorMessage = "Industry already exists.";
        args.IsValid = CRM.Maintenance.IsUniqueIndustry(txtIndustry.Text.Trim(), (int)IndustryId);

    }

   
    #endregion


    #region private properties

    /// <summary>
    /// get/set Industry id
    /// </summary>
    private int? IndustryId
    {
        get
        {
            object obj = this.ViewState["IndustryIdForDetail"];
            return (obj == null) ? (int?)null : (int)obj;
        }
        set { this.ViewState["IndustryIdForDetail"] = value; }
    }

    #endregion
}

