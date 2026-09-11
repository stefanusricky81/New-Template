using SD.LLBLGen.Pro.ORMSupportClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class UserControl_Grid_EditForm_ClientLocationEdit : System.Web.UI.UserControl
{
    /// <summary>
    /// page load
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        
    }

    private void BindStates()
    {
        SD.LLBLGen.Pro.ORMSupportClasses.SortExpression sorter = new SD.LLBLGen.Pro.ORMSupportClasses.SortExpression();
        sorter.Add(DesktopShared.HelperClasses.StateFields.Pstate | SD.LLBLGen.Pro.ORMSupportClasses.SortOperator.Ascending);

        ddlState.DataSource = DesktopShared.CollectionClasses.StateCollection.GetMultiAsDataTable(null, 0, sorter);
        ddlState.DataTextField = "name";
        ddlState.DataValueField = "pstate";
        ddlState.DataBind();
        ddlState.Items.Insert(0, new ListItem("Select a State", ""));

        ////clear items
        //ddlState.ClearSelection();
        //ddlState.Items.Clear();

        //#region bind drop down list

        //PredicateExpression stateFilter = new PredicateExpression();

        //var res = DesktopShared.CollectionClasses.StateCollection.GetMultiAsDataTable(stateFilter, 0, null);


        //ddlState.DataSource = res;

        //ddlState.DataTextField = "name";
        //ddlState.DataValueField = "pstate";
        //ddlState.DataBind();
        ////ddlState.DataSource = null;

        //#endregion


        //ListItem liDefault = new ListItem("Select a State", "");
        //ddlState.Items.Insert(0, liDefault);
    }
    #region public methods

    /// <summary>
    /// load values
    /// </summary>
    /// <param name="objClientLocation"></param>
    public void LoadValues(DesktopShared.EntityClasses.ClientLocationEntity objClientLocation)
    {
        txtName.Focus();
        btnAdd.Visible = objClientLocation == null;
        btnEdit.Visible = !btnAdd.Visible;
        BindStates();

        if (objClientLocation != null)
        {
            ClientLocationId = objClientLocation.Id;
            litHeader.Text = "Edit Client Location";
            txtName.Text = objClientLocation.Name.Trim();
            txtAddress.Text = objClientLocation.Addr1.Trim();
            txtxCity.Text = objClientLocation.City.Trim();
            ddlState.SelectedValue = objClientLocation.Pstate == null ? "": objClientLocation.Pstate;
            txtZip.Text = objClientLocation.Zip.Trim();
            txtDescription.Text = objClientLocation.Description.Trim();
            chkActive.Checked = objClientLocation.Active;
        }
        else
        {
            chkActive.Checked = true;
        }
    }

    /// <summary>
    /// save values
    /// </summary>
    /// <param name="clientLocationId"></param>
    public int SaveValues(int? clientLocationId = null)
    {
        DesktopShared.EntityClasses.ClientLocationEntity objClientLocation = null;
        DateTime _auditDate = DateTime.Now;
        int _auditUserId = DesktopShared.User.UserID;

        if (!clientLocationId.HasValue)
        {
            objClientLocation = new DesktopShared.EntityClasses.ClientLocationEntity();
            objClientLocation.ClientId = ClientIdForLocation;
            objClientLocation.Created = _auditDate;
            objClientLocation.CreatedByUserId = _auditUserId;
        }
        else
            objClientLocation = new DesktopShared.EntityClasses.ClientLocationEntity(clientLocationId.Value);

        objClientLocation.Name = txtName.Text.Trim();
        objClientLocation.Description = txtDescription.Text.Trim();
        objClientLocation.Addr1 = txtAddress.Text.Trim();
        objClientLocation.City = txtxCity.Text.Trim();
        objClientLocation.Pstate = ddlState.SelectedValue.Trim();
        objClientLocation.Statename = ddlState.SelectedItem.Text.Trim();
        objClientLocation.Zip = txtZip.Text.Trim();
        objClientLocation.Active = chkActive.Checked;
        objClientLocation.LastUpdated = _auditDate;
        objClientLocation.LastUpdatedByUserId = _auditUserId;
        objClientLocation.Save();

        return objClientLocation.Id;
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
        args.IsValid = DesktopShared.Client.Location.IsUniqueName(ClientIdForLocation.Value, txtName.Text.Trim(), ClientLocationId);
    }

    protected void cvZip_ServerValidate(object source, ServerValidateEventArgs args)
    {
        System.Text.RegularExpressions.Regex r = new System.Text.RegularExpressions.Regex("^[0-9]+$");
        args.IsValid = r.IsMatch(txtZip.Text.Trim());
    }

    #endregion

    #endregion

    #region private properties

    /// <summary>
    /// get/set Client Location id
    /// </summary>
    private int? ClientLocationId
    {
        get
        {
            object obj = this.ViewState["clid_cle"];
            return (obj == null) ? (int?)null : (int)obj;
        }
        set { this.ViewState["clid_cle"] = value; }
    }

    #endregion

    #region public properties

    /// <summary>
    /// get/set Client  id
    /// </summary>
    public int? ClientIdForLocation
    {
        get
        {
            object obj = this.ViewState["cid_cle"];
            return (obj == null) ? (int?)null : (int)obj;
        }
        set { this.ViewState["cid_cle"] = value; }

    }
    #endregion

}