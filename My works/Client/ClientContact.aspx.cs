using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using SD.LLBLGen.Pro.ORMSupportClasses;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Telerik.Web.UI;

public partial class Client_ClientContact : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            chkClient.Checked = true; //default to check on page load
            GetClientId();
            SetUpPage();
        }
    }
    private void GetClientId()
    {
        if (!String.IsNullOrEmpty(BitByBit.Web.Request.GetString("ClientId")))
        {

            try { ClientId = Convert.ToInt32(BitByBit.Web.Request.GetString("ClientId")); }
            catch { ClientId = 0; }

            ucClientContactGrid.SearchClientId = ClientId;
        }
    }
    private void SetUpPage()
    {
        if (ClientId > 0)
        {
            if (ClientId.HasValue)
            {
                ucClientComboBox.SelectedClientId = ClientId.Value;
                litClientName.Text = " - " + DesktopShared.Client.GetName(ClientId.Value).Trim();

                //ucClientContactGrid.SearchClientId = ClientId.Value;
                //ucClientContactGrid.ResetGrid();
                SetGridValues();
            }
        }

        ucClientComboBox.Width = Unit.Pixel(276);
    }
    public int? ClientId
    {
        get
        {
            object obj = this.ViewState["ClientIdForClientContact"];
            if (obj == null)
                return null;
            else
                return (int)obj;
        }

        set
        {
            this.ViewState["ClientIdForClientContact"] = value;
        }
    }
    protected void btnSearch_Click(object source, EventArgs e)
    {
        if (Page.IsValid)
        {
            SetGridValues();
        }
    }

    private void SetGridValues()
    {
        //search criteria
        //ucClientContactGrid.SearchCode = tbCode.Text.Trim();
        ucClientContactGrid.SearchId = tbId.Text.Trim();
        ucClientContactGrid.SearchClientId = ucClientComboBox.SelectedClientId > 0 ? ucClientComboBox.SelectedClientId : (int?)null;
        // ucClientContactGrid.SearchCompany = tbCompany.Text.Trim(); 
        ucClientContactGrid.SearchFirst = tbFirst.Text.Trim();
        ucClientContactGrid.SearchLast = tbLast.Text.Trim();
        ucClientContactGrid.SearchEmail = tbEmail.Text.Trim();
        ucClientContactGrid.SearchPhone = tbPhone.Text.Trim();
        //ucClientContactGrid.SearchStatusAll = ucArchiveStatus.Archived ? true : (bool?)null;
        //ucClientContactGrid.SearchStatusArchived = ucArchiveStatus.Archived ? true : (bool?)null;
        //ucClientContactGrid.SearchStatusArchived = ucArchiveStatus.Archived ? true : (bool?)null;
        ucClientContactGrid.SearchPriority = chkPriority.Checked ? true : (bool?)null;
        //pass in the value of the three checkboxes
        ucClientContactGrid.SearchTypeClient = chkClient.Checked ? true : (bool?)null;
        ucClientContactGrid.SearchTypeVendor = chkVendor.Checked ? true : (bool?)null;
        ucClientContactGrid.SearchTypeGeneral = chkGeneral.Checked ? true : (bool?)null;
        //pass the search active status to the grid here.
        //Active = true, INactive = false, All = null
        bool? _active = null;
        if (ddlStatus.SelectedValue == "1")
            _active = true;
        else if (ddlStatus.SelectedValue == "0")
            _active = false;
        //ucClientContactGrid.SearchActiveStatus = ddlStatus.SelectedValue ? _active : (bool?)null;
        ucClientContactGrid.SearchActiveStatus = _active;
        //reset grid for new search parameters
        ucClientContactGrid.ResetGrid();

        if (ucClientComboBox.SelectedClientId > 0)
            litClientName.Text = " - " + ucClientComboBox.SelectedClientName;
        
    }

    /// <summary>
    /// clear search button click
    /// </summary>
    /// <param name="source"></param>
    /// <param name="e"></param>
    protected void btnClearSearch_Click(object source, EventArgs e)
    {
        tbId.Text = "";
        ucClientContactGrid.SearchId = tbId.Text.Trim();

        //ucArchiveStatus.Archived = false;
        
        ucClientContactGrid.SearchActiveStatus = null;

        ucClientComboBox.ClearItems();
        ucClientContactGrid.SearchClientId = null;
        litClientName.Text = "";

        tbFirst.Text = "";
        ucClientContactGrid.SearchFirst = tbFirst.Text.Trim();

        tbLast.Text = "";
        ucClientContactGrid.SearchLast = tbLast.Text.Trim();

        tbEmail.Text = "";
        ucClientContactGrid.SearchEmail = tbEmail.Text.Trim();

        tbPhone.Text = "";
        ucClientContactGrid.SearchPhone = tbPhone.Text.Trim();
    }
}