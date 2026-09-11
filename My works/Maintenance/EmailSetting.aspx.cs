using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Telerik.Web.UI;
using BitByBit;
using SD.LLBLGen.Pro.ORMSupportClasses;
using DesktopShared;
using DesktopShared.CollectionClasses;

public partial class Email_Default : BasePage
{

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    /// <summary>
    /// grid on need data source
    /// </summary>
    /// <param name="source"></param>
    /// <param name="e"></param>
    protected void rgEmail_NeedDataSource(object source, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        DataTable dt = DesktopShared.CollectionClasses.EmailSettingCollection.GetMultiAsDataTable(null,0,null);
        rgEmail.DataSource = dt;
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

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        foreach (GridDataItem item in rgEmail.Items)
        {
            try
            {
                TextBox Txtdays = (TextBox)item.FindControl("txtDays") as TextBox;
                HiddenField hfID = (HiddenField)item.FindControl("hfID") as HiddenField;

                int id = Convert.ToInt16(hfID.Value);

                DesktopShared.EntityClasses.EmailSettingEntity objClientTamEmail = new DesktopShared.EntityClasses.EmailSettingEntity(id);

                DateTime _now = DateTime.Now;
                int _auditUserId = DesktopShared.User.UserID;
                objClientTamEmail.NumberOfDays = Txtdays.Text == "" ? 0 : Convert.ToInt16(Txtdays.Text);
                objClientTamEmail.LastUpdated = _now;
                objClientTamEmail.LastUpdatedByUserId = _auditUserId;
                objClientTamEmail.Save();

                DisplayMessage(String.Format("All records saved successfully."),
                          Bootstrap.Alert.AlertType.Success);
            }
            catch
            {
                DisplayMessage(String.Format("Error occured while Saving the record"),
                          Bootstrap.Alert.AlertType.Warning);
            }

        }
    }
  
    protected void rgEmail_ItemDataBound(object sender, GridItemEventArgs e)
    {
        HiddenField days = (HiddenField)e.Item.FindControl("hfDays") as HiddenField;
        TextBox txtdays = (TextBox)e.Item.FindControl("txtDays") as TextBox;
        if(days!=null && txtdays != null)
        txtdays.Text = days.Value;
      
    }
}
