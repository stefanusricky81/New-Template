using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Maintenance_DID_DIDAuditTrail : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            bindddlTrunkGroup();
            bindddlAuditType();
            bindddlUserID();
        }
    }

    #region Grid

    private void BindGrid()
    {
        string trunk = string.Empty;
        if (ddlTrunkGroup.SelectedValue != string.Empty)
            trunk = ddlTrunkGroup.SelectedItem.Text.Trim();
        rgDIDAuditTrail.DataSource = DesktopShared.DidAudiTrail.GetDIDAuditTrails(txtDIDNumber.Text.Trim(), trunk, ddlAssigned.SelectedValue, ddlClient.ClientId.ToString(), ddlUser.SelectedValue
            , ucModifStart.SelectedDate, ucModifEnd.SelectedDate, ddlAuditType.SelectedValue);
    }

    protected void rgDIDAuditTrail_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        BindGrid();
    }
    #endregion

    #region button
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        BindGrid();
        rgDIDAuditTrail.DataBind();
    }

    protected void lbClear_Click(object sender, EventArgs e)
    {
        Response.Redirect("DIDAuditTrail.aspx");
    }
    #endregion

    #region binddropdown
    private void bindddlTrunkGroup()
    {
        ddlTrunkGroup.DataSource = DesktopShared.DID.GetDDLTrunkGroup();
        ddlTrunkGroup.DataTextField = "trunkgroupdescr";
        ddlTrunkGroup.DataValueField = "Trunkgroupid";
        ddlTrunkGroup.DataBind();
        ddlTrunkGroup.Items.Insert(0, new ListItem("Select", string.Empty));
    }

    private void bindddlAuditType()
    {
        ddlAuditType.DataSource = DesktopShared.DID.GetDDLAuditType();
        ddlAuditType.DataTextField = "Audittype";
        ddlAuditType.DataValueField = "Audittype";
        ddlAuditType.DataBind();
        ddlAuditType.Items.Insert(0, new ListItem("Select", string.Empty));
    }

    private void bindddlUserID()
    {
        ddlUser.DataSource = DesktopShared.DID.GetDDLUserIDAuditTrailDID();
        ddlUser.DataTextField = "name";
        ddlUser.DataValueField = "userid";
        ddlUser.DataBind();
        ddlUser.Items.Insert(0, new ListItem("Select", string.Empty));
    }
    #endregion
}