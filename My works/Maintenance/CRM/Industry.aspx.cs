using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class Maintenance_CRM_Industry : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {        
        if (!this.IsPostBack)
        {
            if (!CRM.UserRole.isAdmin(DesktopShared.User.UserID))
                Response.Redirect("~/CRM/NoAccess.aspx?Id=110");

            SetUpPage();
        }

    }

    
    /// <summary>
    /// rebind grid
    /// </summary>
    private void RebindGrid()
    {
        phSearchResults.Visible = true;
        rgGrid.EditIndexes.Clear();
        rgGrid.DataSource = null;
        rgGrid.Rebind();
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
            RebindGrid();
        }
    }


    #region telerik grid

    /// <summary>
    /// grid on need data source
    /// </summary>
    /// <param name="source"></param>
    /// <param name="e"></param>
    protected void rgGrid_NeedDataSource(object source, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {               
        //bool IncludeInactive = (chkActive.Checked) ? true : false;        
        rgGrid.DataSource = CRM.Maintenance.GetIndustries(true, txtIndustry.Text.Trim());
    }

    /// <summary>
    /// grid on item data bound
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rgGrid_ItemDataBound(object sender, GridItemEventArgs e)
    {

        //if (e.Item.ItemType == GridItemType.AlternatingItem || e.Item.ItemType == GridItemType.Item)
        //{            
        //    int Index = Convert.ToInt32(e.Item.ItemIndex.ToString());

        //    Telerik.Web.UI.DataKey data;

        //    data = rgGrid.MasterTableView.DataKeyValues[Index];
        //    int Id = (int)data["Id"];

        //    Label lblActionNotes = (Label)e.Item.FindControl("lblActionNotes");    
        //}
    }

    protected void rgGrid_OnRowCommand(object source, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "Delete")
            {
                int _Id = Convert.ToInt32(e.CommandArgument);

                DesktopShared.EntityClasses.CrmindustryEntity obj = new DesktopShared.EntityClasses.CrmindustryEntity(_Id);
                obj.Delete();
                obj.Save();
                DisplayMessage(String.Format("Record deleted successfully."),
                   Bootstrap.Alert.AlertType.Success);
                rgGrid.Rebind();
            }
        }
        catch (Exception ex)
        {
            DisplayMessage(String.Format("Error occured while deleting the record: {0}", ex.Message),
                     Bootstrap.Alert.AlertType.Warning);
        }
    }

    /// <summary>
    /// grid on item created
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rgGrid_ItemCreated(object sender, GridItemEventArgs e)
    {
        #region command item -> hide for export

        //if (e.Item is GridCommandItem && _isExport)
        //    e.Item.Display = false;

        #endregion
    }

    protected void rgGrid_DataBound(object sender, EventArgs e)
    {

    }    

    #endregion

    #endregion

    #region private properties

   
    #endregion
}