using DesktopShared.CollectionClasses;
using Newtonsoft.Json.Linq;
using SD.LLBLGen.Pro.ORMSupportClasses;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class Role_NewRoles : System.Web.UI.Page
{
    private DataTable newdt;
    private static readonly HttpClient httpClient = new HttpClient();
    protected void Page_Load(object sender, EventArgs e)
    {
        //DisplayChosenPluginJs();
        if (!IsPostBack)
        {
            ucCreatedStart.SelectedDate = DateTime.Now;

            binddropdownlist(lb24x7Primary);
            binddropdownlist(lb24x7Secondary);
            binddropdownlist(lbOnsite);
            binddropdownlist(lbHelpDesk);
            binddropdownlist(lbScheduling);
            binddropdownlist(lbTicketDispatch);

            getAllLisyboxitem(lb24x7Primary, "24x7primary");
            getAllLisyboxitem(lb24x7Secondary, "24x7secondary");
            getAllLisyboxitem(lbOnsite, "onsite");
            getAllLisyboxitem(lbHelpDesk, "helpdesk");
            getAllLisyboxitem(lbScheduling, "scheduling");
            getAllLisyboxitem(lbTicketDispatch, "ticketdispatch");
        }
    }
    protected System.Data.DataTable Createddt()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("Id", System.Type.GetType("System.String"));//0
        dt.Columns.Add("Email", System.Type.GetType("System.String"));//1
        dt.Columns.Add("Subject", System.Type.GetType("System.String"));//2
        dt.Columns.Add("Date", System.Type.GetType("System.String"));//3
        dt.Columns.Add("Start", System.Type.GetType("System.String"));//3
        dt.Columns.Add("End", System.Type.GetType("System.String"));//4

        return dt;
    }

    protected void BindGrid()
    {
        try
        {
            if (ucCreatedStart.SelectedDate.HasValue)
            {
                List<emp> _pemployee = GetSchedulingEmp();
                DataTable dtEmail = new DataTable();
                dtEmail.Columns.Add("pemployee", typeof(string));

                for (int i = 0; i <= _pemployee.Count - 1; i++)
                {
                    dtEmail.Rows.Add(_pemployee[i].pemployee);
                }
                //foreach (int item in _pemployee.Count)
                //{
                //    //if(item.ToString().Trim() == "bruce@bitxbit.com")
                //        dtEmail.Rows.Add(item);
                //}

                rptEmployeeScheduling.DataSource = dtEmail;
                rptEmployeeScheduling.DataBind();
            }
        }
        catch (Exception ex)
        {
            DisplayMessage(litMessage, ex.ToString(), Bootstrap.Alert.AlertType.Danger);
        }
    }

    #region databound

    protected void rptSchedule_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            DataRowView row = (DataRowView)e.Item.DataItem;

            Label lblSchedule = (Label)e.Item.FindControl("lblSchedule");

            DateTime date = Convert.ToDateTime(row["Date"].ToString());
            DateTime estdate = TimeZoneInfo.ConvertTimeFromUtc(date, TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time"));

            DateTime end = Convert.ToDateTime(row["End"].ToString());
            DateTime estend = TimeZoneInfo.ConvertTimeFromUtc(end, TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time"));

            if (estdate.Date != estend.Date)
                lblSchedule.Text = estdate.ToString("MM/dd/yyyy HH:mm") + "-" + estend.ToString("MM/dd/yyyy HH:mm") + " : " + row["Subject"].ToString();
            else
                lblSchedule.Text = estdate.ToString("MM/dd/yyyy HH:mm") + "-" + estend.ToString("HH:mm") + " : " + row["Subject"].ToString();
        }
    }

    protected async void rptEmployeeScheduling_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        DateTime startDate = DateTime.Parse(ucCreatedStart.SelectedDate.Value.ToString().Trim());

        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            DataRowView row = (DataRowView)e.Item.DataItem;

            int pemployee = Convert.ToInt32(row["pemployee"].ToString());

            Label lbname = (Label)e.Item.FindControl("lblName");
            HiddenField hfEmpId = (HiddenField)e.Item.FindControl("hfEmpId");
            HiddenField hfEmpEmail = (HiddenField)e.Item.FindControl("hfEmpEmail");

            hfEmpId.Value = pemployee.ToString().Trim();
            var _name = DesktopShared.Employee.GetEmployeeEntity(pemployee);
            var _users = DesktopShared.User.GetForEmployee(_name.Pemployee);
            hfEmpEmail.Value = _name.Email.Trim();

            if (_users != null)
            {
                lbname.Text = _name.First.Trim() + " " + _name.Last.Trim();

                if (_users.AvailableForScheduling == true)
                {
                    // Find the RadGrid inside this Repeater item
                    Repeater rptSchedule = (Repeater)e.Item.FindControl("rptSchedule");

                    var calendarService = new GraphCalendarService();

                    var events = await calendarService.GetUserCalendarEvents(_name.Email, startDate);

                    if (events.Count > 0)
                    {
                        newdt = Createddt();

                        for (int i = 0; i <= events.Count - 1; i++)
                        {
                            DataRow rowevent = newdt.NewRow();
                            rowevent["Id"] = i + 1;
                            rowevent["Email"] = events[i].Organizer;
                            rowevent["Subject"] = events[i].Subject;
                            rowevent["Date"] = events[i].StartTime;
                            rowevent["Start"] = events[i].StartTime;
                            rowevent["End"] = events[i].EndTime;

                            newdt.Rows.Add(rowevent);
                        }

                        rptSchedule.DataSource = newdt;
                        rptSchedule.DataBind();
                    }
                    else
                        e.Item.Visible = false;
                }
                else
                    e.Item.Visible = false;
            }
            else
                e.Item.Visible = false;
        }
    }
    #endregion

    #region button
    protected void lbSave_Click(object sender, EventArgs e)
    {
        AddEmployeeSchedule(lb24x7Primary, "24x7primary");
        AddEmployeeSchedule(lb24x7Secondary, "24x7secondary");
        AddEmployeeSchedule(lbOnsite, "onsite");
        AddEmployeeSchedule(lbHelpDesk, "helpdesk");
        AddEmployeeSchedule(lbScheduling, "scheduling");
        AddEmployeeSchedule(lbTicketDispatch, "ticketdispatch");
    }
    protected void btnClear_Click(object sender, EventArgs e)
    {
        Response.Redirect("NewRoles.aspx");
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        DateTime _startdate = Convert.ToDateTime(ucCreatedStart.SelectedDate);
        if (Page.IsValid)
        {
            litMessage.Text = "";
            pnlContainer2.Visible = true;
            pnlContainer3.Visible = true;

            BindGrid();

            ddlEmailEmployee.DataSource = GetSchedulingEmp();
            ddlEmailEmployee.DataTextField = "name";
            ddlEmailEmployee.DataValueField = "pemployee";
            ddlEmailEmployee.DataBind();
            ddlEmailEmployee.Items.Insert(0, new ListItem("", ""));

            getAllLisyboxitem(ddlEmailEmployee, "Email");
        }
    }
    protected void lbSendMail_Click(object sender, EventArgs e)
    {
        string selectedEmpName = string.Empty;
        string selectedEmpEmail = string.Empty;
        string err = string.Empty;
        string _allemail = string.Empty;
        StringBuilder sb = new StringBuilder();

        _allemail = BitByBit.Configuration.GetConfigString("DailyTech");

        if (AddEmployeeSchedule(ddlEmailEmployee, "Email"))
        {
            _allemail += "," + email(ddlEmailEmployee);
            _allemail += "," + email(lb24x7Primary);
            _allemail += "," + email(lb24x7Secondary);
            _allemail += "," + email(lbOnsite);
            _allemail += "," + email(lbHelpDesk);
            _allemail += "," + email(lbScheduling);
            _allemail += "," + email(lbTicketDispatch);

            sb.Append("<h2>Employee Scheduling</h2>");

            sb.Append("1: 24x7 Primary - " + getEmployee(lb24x7Primary) + "<br/>");
            sb.Append("2: 24x7 Secondary - " + getEmployee(lb24x7Secondary) + "<br/>");
            sb.Append("3: Onsite - " + getEmployee(lbOnsite) + "<br/>");
            sb.Append("4: Help Desk - " + getEmployee(lbHelpDesk) + "<br/>");
            sb.Append("5: Scheduling - " + getEmployee(lbScheduling) + "<br/>");
            sb.Append("6: Ticket Dispatch - " + getEmployee(lbTicketDispatch) + "<br/>");

            foreach (RepeaterItem item in rptEmployeeScheduling.Items)
            {
                HiddenField hfEmpId = (HiddenField)item.FindControl("hfEmpId");
                HiddenField hfEmpEmail = (HiddenField)item.FindControl("hfEmpEmail");
                Label lblName = (Label)item.FindControl("lblName");

                if (hfEmpEmail.Value != string.Empty)
                {
                    selectedEmpName = lblName.Text.Trim();
                    selectedEmpEmail = hfEmpEmail.Value.Trim();
                    if (_allemail == string.Empty)
                        _allemail = hfEmpEmail.Value.Trim();
                    else
                        _allemail += "," + hfEmpEmail.Value.Trim();

                    #region bodymail

                    sb.AppendFormat("<h3>{0} ({1})</h3>", selectedEmpName, selectedEmpEmail);

                    Repeater rptSchedule = (Repeater)item.FindControl("rptSchedule");
                    if (rptSchedule.Items.Count > 0)
                    {
                        sb.Append("<table border='1' cellpadding='5' cellspacing='0' style='border-collapse:collapse;width:100%;'>");
                        foreach (RepeaterItem schedItem in rptSchedule.Items)
                        {
                            Label lblSchedule = (Label)schedItem.FindControl("lblSchedule");
                            sb.AppendFormat("<li>{0}</li>", lblSchedule.Text);
                        }
                        sb.Append("</table><br/>");
                    }
                    else
                    {
                        sb.Append("<p><i>No availability records found.</i></p>");
                    }

                    sb.Append("<hr/>");
                    #endregion
                }
            }
            string[] emailArray = _allemail.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            // remove duplicates
            var distinctEmails = emailArray
                .Select(x => x.Trim()) // clean spaces
                .Distinct(StringComparer.OrdinalIgnoreCase) // ignore case (A@Gmail.com = a@gmail.com)
                .ToList();

            // join back into string
            _allemail = string.Join(",", distinctEmails);

            //if (DesktopShared.Email.SendEmail2("support@bitxbit.com", "Email Scheduling", selectedEmpEmail + "," + _allemail, "sricky@bitxbit.com", sb.ToString(), ref err, null, null, "") == true)

            if (DesktopShared.Email.SendEmail2("dispatch@bitxbit.com", "Daily Tech Update - " + ucCreatedStart.SelectedDate.Value.ToString("MM/dd/yyyy"), "sricky@bixbit.com", "sricky@bitxbit.com", sb.ToString(), ref err, null, null, "") == true)
                DisplayMessage(litMessage, "Email send", Bootstrap.Alert.AlertType.Success);
            else
                DisplayMessage(litMessage, err, Bootstrap.Alert.AlertType.Danger);
        }
    }
    #endregion

    #region PRIVATE
    private string email(ListBox lb)
    {
        string email = string.Empty;
        foreach (ListItem li in lb.Items)
        {
            if (li.Selected)
            {
                var _email = DesktopShared.Employee.GetEmployeeEntity(Convert.ToInt32(li.Value));
                if (email == string.Empty)
                    email = _email.Email.Trim();
                else
                    email += "," + _email.Email.Trim();
            }
        }
        return email;
    }
    private void binddropdownlist(ListBox lb)
    {
        lb.DataSource = GetSchedulingEmp();
        lb.DataTextField = "name";
        lb.DataValueField = "pemployee";
        lb.DataBind();
        lb.Items.Insert(0, new ListItem("", ""));
    }

    private void getAllLisyboxitem(ListBox lb, string section)
    {
        PredicateExpression roleemp = new PredicateExpression();
        roleemp.Add(DesktopShared.HelperClasses.EmployeeSchedulingEmailFields.Section == section);
        ISortExpression roleempSort = new SD.LLBLGen.Pro.ORMSupportClasses.SortExpression();
        var resEmailEmp = EmployeeSchedulingEmailCollection.GetMultiAsDataTable(roleemp, 0, roleempSort);

        foreach (DataRow drEmpEmail in resEmailEmp.Rows)
        {
            int emp = Convert.ToInt16(drEmpEmail["pemployee"]);
            foreach (ListItem listItem in lb.Items)
            {
                if (listItem.Value.Equals(Convert.ToString(emp)))
                {
                    listItem.Selected = true;
                    break;
                }
            }
        }
    }

    private string getEmployee(ListBox lb)
    {
        try {
            string emp = string.Empty;
            foreach (ListItem listItem in lb.Items)
            {
                if (listItem.Selected)
                {
                    if (emp == string.Empty)
                        emp = listItem.Text.Trim();
                    else
                        emp+=", "+ listItem.Text.Trim();
                }
            }

            return emp;
        }
        catch {
            return "";
        }
    }
    private void DisplayMessage(Literal ltName, string message, Bootstrap.Alert.AlertType alertType, bool dismissable = true, string overrideHeader = "")
    {
        ScriptManager.RegisterClientScriptBlock(this, Page.GetType(), "ToTheTop", "ToTopOfPage();", true);
        ltName.Visible = true;
        ltName.Text = Bootstrap.Alert.GetDivText(alertType, message, dismissable, true, overrideHeader);
    }
    private List<emp> GetSchedulingEmp()
    {
        List<emp> employee = new List<emp>();

        EmployeeCollection _pemployee = new EmployeeCollection();
        IPredicateExpression expr = new PredicateExpression();
        expr.Add(DesktopShared.HelperClasses.EmployeeFields.Active == 'Y');
        _pemployee.GetMulti(expr);

        if (_pemployee.Count > 0)
        {
            for (int i = 0; i <= _pemployee.Count - 1; i++)
            {
                var _users = DesktopShared.User.GetForEmployee(_pemployee[i].Pemployee);
                if (_users != null)
                {
                    if (_users.AvailableForScheduling == true)
                    {
                        employee.Add(new emp
                        {
                            pemployee = _pemployee[i].Pemployee,
                            email = _pemployee[i].Email.ToString().Trim(),
                            name = _pemployee[i].First.Trim() + " " + _pemployee[i].Last.Trim(),
                        });
                    }
                }
            }
        }
        return employee;
    }

    private bool AddEmployeeSchedule(ListBox lb, string section)
    {
        try {
            PredicateExpression empEmail = new PredicateExpression();
            empEmail.Add(DesktopShared.HelperClasses.EmployeeSchedulingEmailFields.Section == section);
            var resempEmail = EmployeeSchedulingEmailCollection.GetMultiAsDataTable(empEmail, 0, null);

            foreach (DataRow dr in resempEmail.Rows)
            {
                int pkey = Convert.ToInt16(dr["Id"]);
                DesktopShared.EntityClasses.EmployeeSchedulingEmailEntity objEmplEmail = new DesktopShared.EntityClasses.EmployeeSchedulingEmailEntity(pkey);
                objEmplEmail.Delete();
                objEmplEmail.Save();
            }

            foreach (ListItem li in lb.Items)
            {
                if (li.Selected)
                {
                    DesktopShared.EntityClasses.EmployeeSchedulingEmailEntity objAddEmplEmail = new DesktopShared.EntityClasses.EmployeeSchedulingEmailEntity();
                    objAddEmplEmail.Pemployee = Convert.ToInt32(li.Value);
                    objAddEmplEmail.Section = section;
                    objAddEmplEmail.Save();
                }
            }

            return true;
        }
        catch (Exception ex)
        {
            DisplayMessage(litMessage, ex.ToString(), Bootstrap.Alert.AlertType.Danger);
            return false;
        }
    }

    private class emp
    {
        public int pemployee { get; set; }
        public string email { get; set; }
        public string name { get; set; }
    }
    #endregion
}