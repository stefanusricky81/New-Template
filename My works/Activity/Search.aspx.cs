using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class Activity_Search : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
            SetUpPage();
    }

    #region private methods

    /// <summary>
    /// set up for initial page load
    /// </summary>
    private void SetUpPage()
    {
        //user id of employee
        if (DesktopShared.User.Activity.Search.EmployeeId.HasValue)
            ddlEmployee.EmployeeId = DesktopShared.User.Activity.Search.EmployeeId.Value;

        //team id
        ddlTeam.TeamId = DesktopShared.User.Activity.Search.TeamId;

        //client id
        if (DesktopShared.User.Activity.Search.ClientId.HasValue)
            cbClient.SelectedClientId = DesktopShared.User.Activity.Search.ClientId.Value;

        //tag
        string _tagName = DesktopShared.User.Activity.Search.TicketTag.Trim();
        if (_tagName.Length > 0)
        {
            //if tag is public, need to append "(P)" or "(PC)" to it and select correct list item in ddl
            SetPublicTagName(ref _tagName);

            //set value in drop down list
            ddlUserTicketTags.TagName = _tagName; 
        }

        //start date
        rdpStart.SelectedDate = DesktopShared.User.Activity.Search.StartDate.HasValue ? DesktopShared.User.Activity.Search.StartDate.Value : DateTime.Now;

        //end date
        rdpEnd.SelectedDate = DesktopShared.User.Activity.Search.EndDate.HasValue ? DesktopShared.User.Activity.Search.EndDate.Value : DateTime.Now;

        //clip notes text
        chkClipNotes.Checked = DesktopShared.User.Activity.Search.ClipNotesText;

        //include user check ins
        chkIncludeUserCheckIns.Checked = DesktopShared.User.Activity.Search.IncludeUserCheckIns;

        //bind grid
        BindGrid();
    }

    /// <summary>
    /// bind grid
    /// </summary>
    private void BindGrid()
    {
        //user id of employee
        gridSearchActivity.SearchUserId = (ddlEmployee.EmployeeId > 0) ? DesktopShared.Employee.GetUserId(ddlEmployee.EmployeeId) : (int?)null;

        //team id
        gridSearchActivity.SearchTeamId = ddlTeam.TeamId; 

        //client id
        gridSearchActivity.SearchClientId = (cbClient.SelectedClientId > 0) ? cbClient.SelectedClientId : (int?)null;

        //tag
        gridSearchActivity.SearchTicketTag = ddlUserTicketTags.TagValue.Trim();
        gridSearchActivity.SearchTicketTagIsPublic = ddlUserTicketTags.IsPublic;

        //start date        
        gridSearchActivity.SearchStartDate = rdpStart.SelectedDate;

        //end date
        gridSearchActivity.SearchEndDate = rdpEnd.SelectedDate;

        //clip text in notes column
        gridSearchActivity.ClipDisplayNotes = chkClipNotes.Checked;

        //include user check ins
        gridSearchActivity.SearchIncludeUserCheckIns = chkIncludeUserCheckIns.Checked;

        //bind grid
        gridSearchActivity.Visible = true;
        gridSearchActivity.ResetGrid();
    }

    /// <summary>
    /// set selected value in tag drop down list for public tags
    /// returns string with appending of either (P) or (PC) if found
    /// </summary>
    /// <param name="tagName"></param>
    /// <returns></returns>
    private void SetPublicTagName(ref string tagName)
    {
        //get drop down list user control & make sure populated
        DropDownList ddlTags = ddlUserTicketTags.GetDropDownList();
        if (ddlTags.Items.Count == 0)
            ddlUserTicketTags.Populate();

        bool continueToClientPublic = true;

        //first try public
        string _tempTagName = tagName.Trim() + " (P)";
        ListItem li = ddlTags.Items.FindByText(_tempTagName);
        if (li != null)
        {
            li.Selected = true; //select 
            tagName = _tempTagName;
            continueToClientPublic = false;
        }

        //next try client public
        if (continueToClientPublic)
        {
            _tempTagName = tagName.Trim() + " (PC)";
            li = ddlTags.Items.FindByText(_tempTagName);
            if (li != null)
            {
                li.Selected = true; //select 
                tagName = _tempTagName;
            }
        }
    }

    #endregion

    #region protected events

    /// <summary>
    /// search button click
    /// </summary>
    /// <param name="source"></param>
    /// <param name="e"></param>
    protected void btnSearch_Click(object source, EventArgs e)
    {
        if (Page.IsValid)
        {
            //user id of employee
            DesktopShared.User.Activity.Search.EmployeeId = (ddlEmployee.EmployeeId > 0) ? ddlEmployee.EmployeeId : (int?)null;

            //team id
            DesktopShared.User.Activity.Search.TeamId = ddlTeam.TeamId; 

            //client id
            DesktopShared.User.Activity.Search.ClientId = (cbClient.SelectedClientId > 0) ? cbClient.SelectedClientId : (int?)null;

            //tag
            DesktopShared.User.Activity.Search.TicketTag = ddlUserTicketTags.TagValue.Trim();
            DesktopShared.User.Activity.Search.TicketTagIsPublic = ddlUserTicketTags.IsPublic;

            //start date
            DesktopShared.User.Activity.Search.StartDate = rdpStart.SelectedDate;

            //end date
            DesktopShared.User.Activity.Search.EndDate = rdpEnd.SelectedDate;

            //clip notes text
            DesktopShared.User.Activity.Search.ClipNotesText = chkClipNotes.Checked;

            //include user check ins
            DesktopShared.User.Activity.Search.IncludeUserCheckIns = chkIncludeUserCheckIns.Checked;

            //bind grid
            BindGrid();
        }
    }

    /// <summary>
    /// clear button click
    /// </summary>
    /// <param name="source"></param>
    /// <param name="e"></param>
    protected void btnClearSearch_Click(object source, EventArgs e)
    {
        //user id of employee
        ddlEmployee.EmployeeId = -1;
        DesktopShared.User.Activity.Search.EmployeeId = null;
        gridSearchActivity.SearchUserId = null;

        //team id
        ddlTeam.TeamId = null;
        DesktopShared.User.Activity.Search.TeamId = null; 
        gridSearchActivity.SearchTeamId = null;

        //client id
        cbClient.SelectedClientId = -1;
        DesktopShared.User.Activity.Search.ClientId = null;
        gridSearchActivity.SearchClientId = null;

        //tag
        ddlUserTicketTags.TagValue = "";
        DesktopShared.User.Activity.Search.TicketTag = "";
        DesktopShared.User.Activity.Search.TicketTagIsPublic = false;
        gridSearchActivity.SearchTicketTag = "";

        //start date
        rdpStart.Clear();
        DesktopShared.User.Activity.Search.StartDate = null;
        gridSearchActivity.SearchStartDate = null;

        //end date
        rdpEnd.Clear();
        DesktopShared.User.Activity.Search.EndDate = null;
        gridSearchActivity.SearchEndDate = null;

        //include user check ins
        chkIncludeUserCheckIns.Checked = true;
        DesktopShared.User.Activity.Search.IncludeUserCheckIns = true;
        gridSearchActivity.SearchIncludeUserCheckIns = true;


        Button btnSender = (Button)source;
        if (btnSender.CommandName.Trim().ToLower() == "setdate")
        {
            //start date
            rdpStart.SelectedDate = DateTime.Now;
            DesktopShared.User.Activity.Search.StartDate = DateTime.Now;

            //end date
            rdpEnd.SelectedDate = DateTime.Now;
            DesktopShared.User.Activity.Search.EndDate = DateTime.Now;

            //bind grid
            BindGrid();
        }
        else //hide grid
            gridSearchActivity.Visible = false;
        
    }

    /// <summary>
    /// telerik date picker on children created
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rdp_ChildrenCreated(object sender, System.EventArgs e)
    {
        RadDatePicker picker = (RadDatePicker)sender;

        HyperLink clearLink = new HyperLink();
        clearLink.NavigateUrl = string.Format("javascript:$find('{0}').clear()", picker.ClientID);
        clearLink.Text = "Clear";

        picker.Controls.Add(clearLink);
    }

    #endregion
}