using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class Employee_DefaultNew : System.Web.UI.Page
{
    /// <summary>
    /// page load
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindDropDowns();
        }
    }

    #region private methods
    
    /// <summary>
    /// bind drop downs lists
    /// </summary>
    private void BindDropDowns()
    {
        ddlTeam.DataSource = DesktopShared.Employee.GetAllTeam(true);
        ddlTeam.DataTextField = "Name";
        ddlTeam.DataValueField = "Id";
        ddlTeam.DataBind();
        ddlTeam.Items.Insert(0, new ListItem("", ""));

        ddlTier.DataSource = DesktopShared.Employee.GetAllTiers(true);
        ddlTier.DataTextField = "Name";
        ddlTier.DataValueField = "Id";
        ddlTier.DataBind();
        ddlTier.Items.Insert(0, new ListItem("", ""));
    }

    /// <summary>
    /// rebind grid
    /// </summary>
    private void RebindGrid()
    {
        phSearchResults.Visible = true;
        rgEmployee.Visible = true;
        rgEmployee.EditIndexes.Clear();
        rgEmployee.DataSource = null;
        rgEmployee.Rebind();
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
        RebindGrid();
    }

    /// <summary>
    /// clear button on click
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnClear_Click(object sender, EventArgs e)
    {
        Response.Redirect(Request.RawUrl);
    }

    #region telerik grid

    /// <summary>
    /// grid on need data source
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rgEmployee_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        bool? _active = null;
        if (!String.IsNullOrWhiteSpace(ddlStatus.SelectedValue))
            _active = ddlStatus.SelectedValue == "1" ? true : false;

        int? _teamId = null;
        if (!String.IsNullOrWhiteSpace(ddlTeam.SelectedValue))
            _teamId = Convert.ToInt32(ddlTeam.SelectedValue);

        int? _tierId = null;
        if (!String.IsNullOrWhiteSpace(ddlTier.SelectedValue))
            _tierId = Convert.ToInt32(ddlTier.SelectedValue);
        
        rgEmployee.DataSource = DesktopShared.Employee.GetDataTable(txtFirstName.Text.Trim(), txtLastName.Text.Trim(), _active, _teamId, _tierId);
    }

    /// <summary>
    /// grid on item data bound
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rgEmployee_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        #region Item or AlternatingItem

        if (e.Item.ItemType == GridItemType.Item || e.Item.ItemType == GridItemType.AlternatingItem)
        {
            DataRowView _row = (DataRowView)e.Item.DataItem;
            GridDataItem _gdi = e.Item as GridDataItem;

            string _email = _row["Email"].ToString().Trim();
            string _emailLink = _email;
            if (DesktopShared.Utility.IsValidEmailAddress(_email))
                _emailLink = String.Format("<a href=\"mailto:{0}\">{0}</a>", _email);

            DesktopShared.Utility.Grid.TelerikHelper.AddLabelToCell(_row["Code"].ToString().Trim(), _gdi["Code"]);
            DesktopShared.Utility.Grid.TelerikHelper.AddLabelToCell(_row["First"].ToString().Trim(), _gdi["First"]);
            DesktopShared.Utility.Grid.TelerikHelper.AddLabelToCell(_row["Last"].ToString().Trim(), _gdi["Last"]);
            DesktopShared.Utility.Grid.TelerikHelper.AddLabelToCell(_row["BusExt"].ToString().Trim(), _gdi["BusExt"]);
            DesktopShared.Utility.Grid.TelerikHelper.AddLabelToCell(_emailLink, _email, _gdi["Email"]);
            DesktopShared.Utility.Grid.TelerikHelper.AddLabelToCell(_row["TierName"].ToString().Trim(), _gdi["TierName"]);            
            DesktopShared.Utility.Grid.TelerikHelper.AddLabelToCell(_row["TeamName"].ToString().Trim(), _gdi["TeamName"]);
            DesktopShared.Utility.Grid.TelerikHelper.AddLabelToCell(_row["Empltype"].ToString().Trim(), _gdi["Empltype"]);

        }

        #endregion
    }

    #endregion

    #endregion


}