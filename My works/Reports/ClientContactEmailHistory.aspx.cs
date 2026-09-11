using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class Reports_ClientContactEmailHistory : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void rgEmailHistory_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        BindGrid();
        //rgEmailHistory.DataBind();
    }
    protected void rgEmailContact_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        EmailContactBindGrid(null);
    }
    protected void BindGrid()
    {
        rgEmailHistory.DataSource = DesktopShared.ClientContactEmailSend.GetDetailEmailHist(txtEmailSubject.Text.Trim(), txtEmailBody.Text.Trim(), ucSendDateStart.SelectedDate, ucSendDateEnd.SelectedDate);
    }

    protected void EmailContactBindGrid(int? emailhistid)
    {
        rgEmailContact.DataSource = DesktopShared.ClientContactEmailSend.GetContactDetail(emailhistid);
    }

    protected void rgEmailHistory_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        int _auditUserId = DesktopShared.User.UserID;
        string command = e.CommandArgument.ToString().Trim();
        switch (command)
        {
            case "LookUp":
                if (true)
                {
                    GridDataItem item = (GridDataItem)e.Item;

                    lblModalTitleEmailContact.Text = "Email Contact";
                    hfID.Value = item["ID"].Text.Trim();
                    EmailContactBindGrid(Convert.ToInt32(hfID.Value));
                    rgEmailContact.Rebind();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "myModalEmailContact", "$('#myModalEmailContact').modal('show');", true);
                }
                break;
            case "BodyEmail":
                if (true)
                {
                    GridDataItem item = (GridDataItem)e.Item;
                    lblModalBodyEmail.Text = "Body Email";

                    txtBody.Content= item["Body"].Text.Trim();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "myBodyEmail", "$('#myBodyEmail').modal('show');", true);
                }
                break;
        }
    }

    protected void rgEmailHistory_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item.ItemType == GridItemType.Item || e.Item.ItemType == GridItemType.AlternatingItem)
        {
            DataRowView _row = (DataRowView)e.Item.DataItem;
            GridDataItem _gdi = e.Item as GridDataItem;

            LinkButton lbEmailBody = e.Item.FindControl("lbEmailBody") as LinkButton;
            lbEmailBody.Text = Regex.Replace(_row["Body"].ToString().Trim(), "<.*?>", String.Empty);
            if (lbEmailBody.Text.Length > 100)
                lbEmailBody.Text = lbEmailBody.Text.Substring(0, 90) + "...";
        }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            BindGrid();
            rgEmailHistory.Rebind();
        }
        catch (Exception ex)
        { }
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        Response.Redirect("ClientContactEmailHistory.aspx");
    }
}