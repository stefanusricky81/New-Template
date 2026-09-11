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
using System.Collections.Generic;
using System.Text;

public partial class Reports_ClientEmailBroadcast : BasePage
{
    private DateTime _date = DateTime.Now;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            // current date by default
            rdpStartDate.SelectedDate = _date;
            rdpEndDate.SelectedDate = _date;

            ucEmailBroadcastLogGrid.SearchStartDate = (DateTime)rdpStartDate.SelectedDate;
            ucEmailBroadcastLogGrid.SearchEndDate = (DateTime)rdpEndDate.SelectedDate;
        }
    }

    #region protected methods

    /// <summary>
    /// search button click
    /// </summary>
    /// <param name="source"></param>
    /// <param name="e"></param>
    protected void btnSearch_Click(object source, EventArgs e)
    {
        if (Page.IsValid)
        {
            //search criteria
            //get a list of search clients 
            ArrayList _Companylist = new ArrayList();
            string checkedText = string.Empty;

            foreach (RadComboBoxItem item in ucClientCompanyComboBox.rcbCompany.Items)
            {
                CheckBox chkCompany = (CheckBox)item.FindControl("chkCompany");
                if (chkCompany.Checked)
                {
                    _Companylist.Add(item.Value);
                    checkedText += item.Text + ", ";
                }
            }
            lblSelectedCompanies.Text = checkedText.Trim().TrimEnd(',');

            if (_Companylist.Count > 0)
                ucEmailBroadcastLogGrid.SearchClients = _Companylist;
            else
                ucEmailBroadcastLogGrid.SearchClients = null;

            ucEmailBroadcastLogGrid.SearchProductId = ucClientProductComboBox.SelectedProductId > 0 ? ucClientProductComboBox.SelectedProductId : (int?)null;

            ucEmailBroadcastLogGrid.SearchStartDate = (DateTime)rdpStartDate.SelectedDate;
            ucEmailBroadcastLogGrid.SearchEndDate = (DateTime)rdpEndDate.SelectedDate;
            

            //reset grid for new search parameters
            ucEmailBroadcastLogGrid.ResetGrid();

        }
    }

    /// <summary>
    /// clear search button click
    /// </summary>
    /// <param name="source"></param>
    /// <param name="e"></param>
    protected void btnClearSearch_Click(object source, EventArgs e)
    {
        ucClientCompanyComboBox.ClearItems();
        ucClientProductComboBox.ClearItems();

        //clear grid
        ucEmailBroadcastLogGrid.ClearGrid();

        rdpStartDate.SelectedDate = _date;
        rdpEndDate.SelectedDate = _date;

        //clear selected label
        lblSelectedCompanies.Text = string.Empty;
    }

    /// <summary>
    /// select company button click
    /// </summary>
    /// <param name="source"></param>
    /// <param name="e"></param>
    protected void btnSelectCompany_Click(object source, EventArgs e)
    {
        string checkedText = string.Empty;

        foreach (RadComboBoxItem item in ucClientCompanyComboBox.rcbCompany.Items)
        {

            CheckBox chkCompany = (CheckBox)item.FindControl("chkCompany");
            if (chkCompany.Checked)
            {
                checkedText += item.Text + ", ";
            }
            lblSelectedCompanies.Text = checkedText.Trim().TrimEnd(',');
        }

    }

    #endregion
}