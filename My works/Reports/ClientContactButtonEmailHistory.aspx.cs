using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Reports_ClientContactButtonEmailHistory : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            BindDropDownList();
            ucDateEnd.SelectedDate = DateTime.Now;
            ucDateStart.SelectedDate = DateTime.Now.AddDays(-1);
        }
    }

    protected void BindDropDownList()
    {
        ddlActions.DataSource = DesktopShared.Client.Contact.GetDistinctButtonName();
        ddlActions.DataTextField = "ButtonName";
        ddlActions.DataValueField = "ButtonId";
        ddlActions.DataBind();
        ddlActions.Items.Insert(0, string.Empty);
    }
    private void RebindGrid()
    {
        rgButtonHistory.Visible = true;
        rgButtonHistory.EditIndexes.Clear();
        rgButtonHistory.DataSource = null;
        rgButtonHistory.Rebind();
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        RebindGrid();
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        Response.Redirect("/Reports/ClientContactButtonEmailHistory.aspx");
    }

    protected void cvDate_ServerValidate(object source, ServerValidateEventArgs args)
    {
        args.IsValid = true;
        if (!ucDateStart.SelectedDate.HasValue || !ucDateEnd.SelectedDate.HasValue)
            return;
        args.IsValid = ucDateEnd.SelectedDate.Value.Date >= ucDateStart.SelectedDate.Value.Date;
    }

    protected void rgButtonHistory_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        DataTable dt = DesktopShared.StoredProcedureCallerClasses.RetrievalProcedures.ProcSearchButtonHist(ddlClient.ClientId, txtEmail.Text.Trim(),ucDateStart.SelectedDate,ucDateEnd.SelectedDate,ddlActions.SelectedValue);
        rgButtonHistory.DataSource = dt;
    }
}