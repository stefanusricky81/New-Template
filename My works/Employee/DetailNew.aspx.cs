using DesktopShared.CollectionClasses;
using DesktopShared.EntityClasses;
using SD.LLBLGen.Pro.ORMSupportClasses;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Employee_DetailNew : System.Web.UI.Page
{
    public int? EmployeeId { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.QueryString["Id"] != null)
        {
            EmployeeId = Convert.ToInt32(Request.QueryString["Id"]);
        }

        if (!IsPostBack)
        {
            BindDropDowns();
            if (EmployeeId.HasValue)
            {
                if (EmployeeId > 0)
                    btnSaveAndAddProjects.Visible = false;
                LoadEmployee();
                BindProjects();
            }
        }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            LinkButton button = (LinkButton)sender;
            string ButtonCommandName = button.CommandName;

            var res = DesktopShared.Employee.GetEmployee(txtFirstName.Text, txtLastName.Text);
            if (res.Count() > 0 && EmployeeId.HasValue && res.First().Pemployee != EmployeeId.Value)
            {
                DisplayMessage(String.Format("Duplicate Employee Found"), Bootstrap.Alert.AlertType.Warning);
            }
            else if (SaveEmployeeInformation())
            {
                if (ButtonCommandName == "SaveAndAddProject")
                    Response.Redirect("~/Projectgroups/Default.aspx");
                else
                {
                    if (!EmployeeId.HasValue)
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowModal", "$('#modal-new-employee').modal('show');", true);
                    else
                    {
                        btnSaveAndAddProjects.Visible = false;
                        LoadEmployee();
                        BindProjects();

                        DisplayMessage(String.Format("Employee Information has been {0} - {1}.{2}",
                            EmployeeId.HasValue && EmployeeId.Value == 0 ? "added" : "updated",
                            DateTime.Now,
                            EmployeeId.HasValue && EmployeeId.Value == 0 ? "" : ""),
                            Bootstrap.Alert.AlertType.Success);
                    }
                }
            }
            else
            {
                DisplayMessage(String.Format("Error occured while {0} the record",
                          EmployeeId.HasValue && EmployeeId.Value == 0 ? "adding" : "updating",
                          DateTime.Now,
                          EmployeeId.HasValue && EmployeeId.Value == 0 ? "" : ""
                          ),
                          Bootstrap.Alert.AlertType.Warning);
            }
        }
    }

    /// <summary>
    /// change password or cancel change password button click
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnChangePassword_Click(object sender, EventArgs e)
    {
        LinkButton btn = sender as LinkButton;
        mvPassword.ActiveViewIndex = btn.CommandArgument == "1" ? 1 : 0;
    }

    private void LoadEmployee()
    {
        var res = DesktopShared.Employee.GetEmployeeEntity(EmployeeId.Value);
        txtCode.Text = res.Code.Trim();
        txtAddress1.Text = res.Addr1.Trim(); 
        txtCell.Text = res.Cellphone.Trim();
        txtTeamPhone.Text = res.TeamPhone.Trim();
        txtCity.Text = res.City.Trim();
        txtEmail.Text = res.Email.Trim();
        txtExt.Text = res.Busext.Trim();
        txtFirstName.Text = res.First.Trim();
        txtLastName.Text = res.Last.Trim();
        txtPhone.Text = res.Busphone.Trim();
        txtState.Text = res.State.Trim();
        txtZip.Text = res.Zip.Trim();
        chkTicketTimeOfDay.Checked = res.TicketTimesheetUseTime;
        chkActive.Checked = res.Active.ToLower() == "y" ? true:false ;
        chkAllowContractRenewal.Checked = res.CanRenewContracts.HasValue ? res.CanRenewContracts.Value : false;
        ddlActivity.SelectedValue = res.DefaultActivityId.HasValue ? Convert.ToString(res.DefaultActivityId.Value) : "";
        ddlType.SelectedValue = res.Empltype.Trim();
        txtSFEmail.Text = res.SalesForceEmail;
        if (res.StartTime.HasValue)
            ddlStartTime.SelectedDateTime = res.StartTime.Value;
        if (res.EndTime.HasValue)
            ddlEndTime.SelectedDateTime = res.EndTime.Value;
        txtDescription.Text = res.Description.Trim();
        txtSalesPersonCode.Text = res.SalesPersonCode.Trim();
        txtBillingRate.Text = res.BillingRate.ToString("0.00");

        if (res.TicketTypeId.HasValue)
            ddlTicketType.TicketTypeId = Convert.ToInt32(res.TicketTypeId);


        ListItem _li = null;
        if (res.TierId.HasValue)
        {
            _li = ddlTiers.Items.FindByValue(res.TierId.Value.ToString());
            if (_li != null)
            {
                ddlTiers.ClearSelection();
                _li.Selected = true;
            }
        }

        //user entity
        var objUser = DesktopShared.User.GetForEmployee(res.Pemployee);
        if ((objUser != null) && (objUser.Fields.State == EntityState.Fetched))
        {
            if (objUser.TeamId.HasValue)
            {
                _li = ddlTeam.Items.FindByValue(objUser.TeamId.Value.ToString());
                if (_li != null)
                {
                    ddlTeam.ClearSelection();
                    _li.Selected = true;
                }
            }
            cbAssignToTickets.Checked = objUser.AssignToTickets;
            cbAvailableFosScheduling.Checked = objUser.AvailableForScheduling;
            chkTrackActivity.Checked = objUser.TrackActivity;
        }

        foreach (var group in res.UserProjectAssociation)
        {
            var groupID = group.FkProjectGroupId;
            foreach (ListItem item in ddlProjectGroup.Items)
            {
                if (item.Value.Equals(Convert.ToString(groupID)))
                {
                    item.Selected = true;
                    break;
                }
            }
        }

        phChangePassword.Visible = false;
        mvPassword.ActiveViewIndex = 0;
        objUser = DesktopShared.User.GetLoggedInUserEntity();
        if (objUser.Fields.State == EntityState.Fetched)
        {
            phChangePassword.Visible = objUser.PasswordManager;
            phBillingRate.Visible = objUser.BillingRateAdmin;
        }

        if (!String.IsNullOrEmpty(res.Birthday))
        {
            string[] values = res.Birthday.Split('/');
            for (int i = 0; i < values.Length; i++)
            {
                ddlDate.SelectedValue = values[0].Trim();
                ddlMonth.SelectedValue = values[1].Trim();
            }
        }

    }

    private bool SaveEmployeeInformation()
    {
        try
        {
            DateTime _auditDate = DateTime.Now;

            EmployeeEntity employee = EmployeeId.HasValue ? new EmployeeEntity(EmployeeId.Value) : new EmployeeEntity();
            employee.Code = txtCode.Text.Trim();
            employee.Active = chkActive.Checked ? "Y" : "N";
            employee.Addr1 = txtAddress1.Text;
            employee.Addr2 = txtAddress2.Text;
            employee.CanRenewContracts = chkAllowContractRenewal.Checked;
            employee.Busext = txtExt.Text;
            employee.Cellphone = txtCell.Text;
            employee.Busphone = txtPhone.Text;
            employee.City = txtCity.Text;
            employee.State = txtState.Text;
            employee.Zip = txtZip.Text;
            employee.Created = _auditDate.ToString("MM-dd-yyyy");
            employee.Email = txtEmail.Text;
            employee.First = txtFirstName.Text;
            employee.Last = txtLastName.Text;
            employee.Empltype = ddlType.SelectedValue;
            employee.TicketTimesheetUseTime = chkTicketTimeOfDay.Checked;
            employee.DefaultActivityId = Convert.ToInt32(ddlActivity.SelectedValue);
            employee.SalesForceEmail = txtSFEmail.Text;
            employee.TierId = Convert.ToInt32(ddlTiers.SelectedValue);
            employee.StartTime = StartTime;
            employee.EndTime = EndTime;
            employee.Description = txtDescription.Text.Trim();
            employee.TeamPhone = txtTeamPhone.Text.Trim();
            employee.SalesPersonCode = txtSalesPersonCode.Text.Trim();
            employee.Birthday = ddlDate.SelectedValue + "/" + ddlMonth.SelectedValue;
            employee.TicketTypeId = ddlTicketType.TicketTypeId;

            if (phChangePassword.Visible) //password updated admin
            {
                if (mvPassword.ActiveViewIndex == 1)
                {
                    string _saltedPassword = "";
                    string _salt = "";
                    DesktopShared.Utility.PasswordHelper.HashPassword(txtPassword.Text.Trim(), ref _salt, ref _saltedPassword);

                    employee.Salt = _salt;
                    employee.SaltedPassword = _saltedPassword;
                }
            }

            if (phBillingRate.Visible)
                employee.BillingRate = Convert.ToDecimal(txtBillingRate.Text);

            employee.Save();

            //user entity
            var objUser = DesktopShared.User.GetForEmployee(employee.Pemployee);
            if ((objUser != null) && (objUser.Fields.State == EntityState.Fetched))
            {
                objUser.TeamId = Convert.ToInt32(ddlTeam.SelectedValue);
                objUser.TierId = Convert.ToInt32(ddlTiers.SelectedValue);
                objUser.AssignToTickets = cbAssignToTickets.Checked;
                objUser.AvailableForScheduling = cbAvailableFosScheduling.Checked;
                objUser.TrackActivity = chkTrackActivity.Checked;
                objUser.Save();
            }

            employee.Refetch();
            EmployeeId = employee.Pemployee;

            foreach (var upa in employee.UserProjectAssociation.Select(s => s))
            {
                if (upa.FkProjectId == null)
                {
                    upa.Delete();
                    upa.Save();
                }
            }
            return true;
        }
        catch (Exception e)
        {
            DisplayMessage(e.Message, Bootstrap.Alert.AlertType.Danger);
            return false;
        }
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

    private void BindDropDowns()
    {
        ddlProjectGroup.DataSource = DesktopShared.Project.GetProjectGroups();
        ddlProjectGroup.DataTextField = "Name";
        ddlProjectGroup.DataValueField = "Id";
        ddlProjectGroup.DataBind();
        ddlProjectGroup.Items.Insert(0, new ListItem("Select Project Group(s)", "-1"));

        ddlTeam.DataSource = DesktopShared.Employee.GetAllTeam(true);
        ddlTeam.DataTextField = "Name";
        ddlTeam.DataValueField = "Id";
        ddlTeam.DataBind();
        ddlTeam.Items.Insert(0, new ListItem("Select Team", ""));

        
        ddlActivity.DataSource = DesktopShared.Employee.GetActiveDefaultActivities();
        ddlActivity.DataTextField = "action";
        ddlActivity.DataValueField = "ptaskaction";
        ddlActivity.DataBind();
        ddlActivity.Items.Insert(0, new ListItem("Select Activity", "-1"));

        ddlType.DataSource = DesktopShared.Employee.GetEmployeeType();
        ddlType.DataTextField = "Value";
        ddlType.DataValueField = "Key";
        ddlType.DataBind();

        ddlTiers.DataSource = DesktopShared.Employee.GetAllTiers(true);
        ddlTiers.DataTextField = "Name";
        ddlTiers.DataValueField = "Id";
        ddlTiers.DataBind();
        ddlTiers.Items.Insert(0, new ListItem("Select Tier", ""));

        int daysInMonth = System.DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
        for (int i = 1; i <= daysInMonth; i++)
        {
           ddlDate.Items.Add(i.ToString());
        }
        ddlDate.Items.Insert(0, "Select");
        ddlDate.SelectedIndex = 0;

        ListItem lc;
        foreach (var item in System.Globalization.DateTimeFormatInfo.CurrentInfo.MonthNames)
        {
            lc = new ListItem();
            lc.Text = item;
            lc.Value = item;
            ddlMonth.Items.Add(lc);
        }

        ddlMonth.Items.Insert(0, "Select");
        ddlMonth.SelectedIndex = 0;

        for (int i = 0; i < ddlMonth.Items.Count; i++)
        {
            if (i == 2 || i == 4)
            {
                // Disable items at index 2 and 4
                ddlMonth.Items[i].Attributes.Add("disabled", "disabled");
            }
            else
            {
                // Set the color to red for other items
                ddlMonth.Items[i].Attributes.CssStyle.Add("color", "red");
            }
        }
    }

    private void BindProjects()
    {
        DataTable dt = DesktopShared.StoredProcedureCallerClasses.RetrievalProcedures.ProcGetEmployeeProjects((int)EmployeeId);
        rgProjects.DataSource = dt;
        rgProjects.DataBind();
        lblPrjCount.Text= "("+Convert.ToString(dt.Rows.Count)+")";
    }

    /// <summary>
    /// get start time
    /// </summary>
    private DateTime? StartTime
    {
        get
        {
            DateTime _auditDate = DateTime.Now;
            return ddlStartTime.Hour > 0 || ddlStartTime.Minute > 0 ? new DateTime(_auditDate.Year, _auditDate.Month, _auditDate.Day, ddlStartTime.Hour > 0 ? ddlStartTime.Hour : 0, ddlStartTime.Minute > 0 ? ddlStartTime.Minute : 0, 0) : (DateTime?)null;

        }
    }

    /// <summary>
    /// get end time
    /// </summary>
    private DateTime? EndTime
    {
        get
        {
            DateTime _auditDate = DateTime.Now;
            return ddlEndTime.Hour > 0 || ddlEndTime.Minute > 0 ? new DateTime(_auditDate.Year, _auditDate.Month, _auditDate.Day, ddlEndTime.Hour > 0 ? ddlEndTime.Hour : 0, ddlEndTime.Minute > 0 ? ddlEndTime.Minute : 0, 0) : (DateTime?)null;

        }
    }

    /// <summary>
    /// validate employee code is unique
    /// </summary>
    /// <param name="source"></param>
    /// <param name="args"></param>
    protected void cvCode_ServerValidate(object source, ServerValidateEventArgs args)
    {
        DesktopShared.CollectionClasses.EmployeeCollection _employees = new EmployeeCollection();
        IPredicateExpression _employeesFilter = new PredicateExpression();
        _employeesFilter.Add(DesktopShared.HelperClasses.EmployeeFields.Code == txtCode.Text.Trim());
        if (EmployeeId.HasValue)
            _employeesFilter.Add(DesktopShared.HelperClasses.EmployeeFields.Pemployee != EmployeeId.Value);
        args.IsValid = _employees.GetDbCount(_employeesFilter) == 0;
    }

    /// <summary>
    /// validate start & end times
    /// </summary>
    /// <param name="source"></param>
    /// <param name="args"></param>
    protected void cvStartEndTime_ServerValidate(object source, ServerValidateEventArgs args)
    {
        args.IsValid = true;
        if (ddlStartTime.Hour < 0 && ddlStartTime.Minute < 0 && ddlEndTime.Hour < 0 && ddlEndTime.Minute < 0)
            return;

        DateTime _startTime = DateTime.Now;
        bool _startTimeEntered = false;
        if (ddlStartTime.Hour >= 0 || ddlStartTime.Minute >= 0)
        {
            if (!DateTime.TryParse(String.Format("2019-01-01 {0}:{1}:00", ddlStartTime.Hour > 0 ? ddlStartTime.Hour : 0, ddlStartTime.Minute > 0 ? ddlStartTime.Minute : 0), out _startTime))
            {
                args.IsValid = false;
                cvStartEndTime.ErrorMessage = "Invalid Start Time";
                return;
            }
            _startTimeEntered = true;
        }

        DateTime _endTime = DateTime.Now;
        bool _endTimeEntered = false;
        if (ddlEndTime.Hour >= 0 || ddlEndTime.Minute >= 0)
        {
            if (!DateTime.TryParse(String.Format("2019-01-01 {0}:{1}:00", ddlEndTime.Hour > 0 ? ddlEndTime.Hour : 0, ddlEndTime.Minute > 0 ? ddlEndTime.Minute : 0), out _endTime))
            {
                args.IsValid = false;
                cvStartEndTime.ErrorMessage = "Invalid End Time";
                return;
            }
            _endTimeEntered = true;
        }

        if (_startTimeEntered && _endTimeEntered)
        {
            if (_startTime >= _endTime)
            {
                args.IsValid = false;
                cvStartEndTime.ErrorMessage = "Start Time must be less than End Time";
                return;
            }
        }

    }

    /// <summary>
    /// validate end time
    /// </summary>
    /// <param name="source"></param>
    /// <param name="args"></param>
    protected void cvEndTime_ServerValidate(object source, ServerValidateEventArgs args)
    {

    }
}