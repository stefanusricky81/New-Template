using DesktopShared.CollectionClasses;
using DesktopShared.EntityClasses;
using DesktopShared.HelperClasses;
using SD.LLBLGen.Pro.ORMSupportClasses;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class Maintenance_ReportSettings : System.Web.UI.Page
{
    static Regex validate_emailaddress = email_validation();

    #region Protected

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
            SetupPage(sender,e);
    }

    protected void rgPatchReports_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        BindGrid();
    }
    protected void rgBackupReports_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        BindGrid();
    }

    protected void btnAddPatchReport_Click(object sender, EventArgs e)
    {
        txtEmailPatchReports.Text = string.Empty;
        ScriptManager.RegisterStartupScript(this, this.GetType(), "myModalAddPatchRecipients", "$('#myModalAddPatchRecipients').modal('show');", true);
    }

    protected void lbAddnewBackupReports_Click(object sender, EventArgs e)
    {
        txtEmailBackupReports.Text = string.Empty;
        ScriptManager.RegisterStartupScript(this, this.GetType(), "myModalAddBackupRecipients", "$('#myModalAddBackupRecipients').modal('show');", true);
    }

    protected void Decision_Command(object sender, CommandEventArgs e)
    {
        bool _valid = true;
        string _errmessage = string.Empty;

        int _auditUserId = DesktopShared.User.UserID;
        string str = e.CommandArgument.ToString().Trim();
        switch (str)
        {
            case "AddPatch":
                if (txtEmailPatchReports.Text.Trim() != string.Empty)
                {
                    if (validate_emailaddress.IsMatch(txtEmailPatchReports.Text.Trim()) != true)
                    {
                        DisplayMessage(litMessage, "Invalid Email Address", Bootstrap.Alert.AlertType.Warning);
                        return;
                    }
                }
                else
                {
                    DisplayMessage(litMessage, "Email Address cannot empty", Bootstrap.Alert.AlertType.Warning);
                    return;
                }
                if (_valid)
                {
                    if (SaveRecipients(txtEmailPatchReports.Text.Trim(), "P", _auditUserId) == true)
                    {
                        DisplayMessage(litMessage, "Email succesfully added", Bootstrap.Alert.AlertType.Success);
                        BindGrid();
                        rgPatchReports.DataBind();
                    }
                    else
                        DisplayMessage(litMessage, "Email failed to add", Bootstrap.Alert.AlertType.Warning);
                }
                break;
            case "AddBackup":
                if (validate_emailaddress.IsMatch(txtEmailBackupReports.Text.Trim()) != true)
                {
                    DisplayMessage(litMessage, "Invalid Email Address", Bootstrap.Alert.AlertType.Warning);
                    return;
                }
                if (_valid)
                {
                    if (SaveRecipients(txtEmailBackupReports.Text.Trim(), "B", _auditUserId) == true)
                    {
                        DisplayMessage(litMessage, "Email succesfully added", Bootstrap.Alert.AlertType.Success);
                        BindGrid();
                        rgBackupReports.DataBind();
                    }
                    else
                        DisplayMessage(litMessage, "Email failed to add", Bootstrap.Alert.AlertType.Warning);
                }
                break;
            case "Delete":
                if (DeleteEmail(Convert.ToInt32(hfDelete.Value)))
                {
                    DisplayMessage(litMessage, "Record has been deleted", Bootstrap.Alert.AlertType.Success);
                    BindGrid();
                    rgPatchReports.DataBind();
                    rgBackupReports.DataBind();
                    rgARReport.DataBind();
                    rgClosedTicket.DataBind();
                    rgTicketReport.DataBind();
                    rgBackupReport.DataBind();
                    rgComputerManagementReport.DataBind();
                    rgVIPEmail.DataBind();
                }
                else
                    DisplayMessage(litMessage, "Something went wrong", Bootstrap.Alert.AlertType.Danger);
                break;
            case "AddARReport":
                lblModalTitleAddRecipients.Text = "Add AR Report Email Recipients";
                hfFromReport.Value = "ARReport";
                tbAddEmailRecipients.Text = string.Empty;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "myModalAddRecipients", "$('#myModalAddRecipients').modal('show');", true);
                break;
            case "AddComputerManagementReport":
                lblModalTitleAddRecipients.Text = "Add Computer Management Report Recipients";
                hfFromReport.Value = "ComputerManagementReport";
                tbAddEmailRecipients.Text = string.Empty;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "myModalAddRecipients", "$('#myModalAddRecipients').modal('show');", true);
                break;
            case "AddBackupReport":
                lblModalTitleAddRecipients.Text = "Add Backup Report Recipients";
                hfFromReport.Value = "BackupReport";
                tbAddEmailRecipients.Text = string.Empty;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "myModalAddRecipients", "$('#myModalAddRecipients').modal('show');", true);
                break;
            case "AddClosedTicket":
                lblModalTitleAddRecipients.Text = "Add Close Ticket Report Recipients";
                hfFromReport.Value = "CloseTicketReport";
                tbAddEmailRecipients.Text = string.Empty;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "myModalAddRecipients", "$('#myModalAddRecipients').modal('show');", true);
                break;
            case "AddTicketReport":
                lblModalTitleAddRecipients.Text = "Add Ticket Report Recipients";
                hfFromReport.Value = "TicketReport";
                tbAddEmailRecipients.Text = string.Empty;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "myModalAddRecipients", "$('#myModalAddRecipients').modal('show');", true);
                break;
            case "AddVIPEmail":
                lblModalTitleAddRecipients.Text = "Add VIP Email Recipients";
                hfFromReport.Value = "VIPEmail";
                tbAddEmailRecipients.Text = string.Empty;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "myModalAddRecipients", "$('#myModalAddRecipients').modal('show');", true);
                break;
            case "AddRecipients":
                if (tbAddEmailRecipients.Text.Trim() != string.Empty)
                {
                    if (validate_emailaddress.IsMatch(tbAddEmailRecipients.Text.Trim()) != true)
                    {
                        DisplayMessage(litMessage, "Invalid Email Address", Bootstrap.Alert.AlertType.Warning);
                        return;
                    }
                }
                else
                {
                    DisplayMessage(litMessage, "Email Address cannot empty", Bootstrap.Alert.AlertType.Warning);
                    return;
                }

                if (SaveRecipients(tbAddEmailRecipients.Text.Trim(), hfFromReport.Value.Trim(), _auditUserId) == true)
                {
                    DisplayMessage(litMessage, "Email succesfully added", Bootstrap.Alert.AlertType.Success);
                    BindGrid();

                    rgARReport.DataBind();
                    rgClosedTicket.DataBind();
                    rgTicketReport.DataBind();
                    rgBackupReport.DataBind();
                    rgComputerManagementReport.DataBind();
                    rgVIPEmail.DataBind();
                }
                else
                    DisplayMessage(litMessage, "Email failed to add", Bootstrap.Alert.AlertType.Warning);
                break;
        }
    }

    protected void rgPatchReports_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        int _auditUserId = DesktopShared.User.UserID;
        string command = e.CommandArgument.ToString().Trim();

        switch (command)
        {
            case "DeleteRecord":
                if (true)
                {

                    GridDataItem item = (GridDataItem)e.Item;

                    lblModalDeleteTitle.Text = "Delete Record";
                    lblModalDeleteWording.Text = "Are you sure want to delete " + item["Email"].Text.Trim() + " ? ";
                    hfDelete.Value = item["id"].Text;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "myModalDelete", "$('#myModalDelete').modal('show');", true);
                }
                break;
        }
    }

    protected void btnSubmitPatch_Click(object sender, EventArgs e)
    {
        try {
            int nod = Convert.ToInt32(txtNumberofDays.Text.Trim());
            int _auditUserId = DesktopShared.User.UserID;

            if (SaveNoD(nod,"P","",null, _auditUserId,null))
                DisplayMessage(litMessage, "# of days has been Added", Bootstrap.Alert.AlertType.Success);
            else
                DisplayMessage(litMessage, "Something went wrong", Bootstrap.Alert.AlertType.Danger);
        }
        catch (Exception ex)
        {
            DisplayMessage(litMessage, "Err =" + ex.Message.Trim(), Bootstrap.Alert.AlertType.Danger);
        }
    }

    private void SetupPage(object sender, EventArgs e)
    {
        loadNOD();
        cbEveryDaysArReport_CheckedChanged(sender, e);
        cbComputerManagementReport_CheckedChanged(sender, e);
        cbBackupReport_CheckedChanged(sender, e);
        cbClosedTicket_CheckedChanged(sender, e);
        cbTicketReportEmail_CheckedChanged(sender, e);
    }

    protected void lbARReport_Click(object sender, EventArgs e)
    {
        TimeSpan ts;
        string _time = ddlTimeARReport.Hour + ":" + ddlTimeARReport.Minute;
        TimeSpan.TryParse(_time, out ts);

        ProceedSetting(cbEveryDaysArReport, lbDaysARReport, cbEnabledARReport, "ARReport", "AR report email setting have been Updated", ts);
    }

    protected void lbComputerManagementReport_Click(object sender, EventArgs e)
    {
        TimeSpan ts;
        string _time = ddlTimeComputerManagementReport.Hour + ":" + ddlTimeComputerManagementReport.Minute;
        TimeSpan.TryParse(_time, out ts);

        ProceedSetting(cbComputerManagementReport, lbDaysComputerManagementReport, cbEnabledComputerManagementReport, "ComputerManagementReport", "Computer management report email setting have been Updated", ts);
    }

    protected void lbBackupMonitorReport_Click(object sender, EventArgs e)
    {
        TimeSpan ts;
        string _time = ddlTimeBackupReport.Hour + ":" + ddlTimeBackupReport.Minute;
        TimeSpan.TryParse(_time, out ts);

        ProceedSetting(cbBackupReport, lbDaysBackupReport, cbEnableBackupReport, "BackupReport", "Backup report rmail setting have been Updated", ts);
    }

    protected void lbCloseTicket_Click(object sender, EventArgs e)
    {
        TimeSpan ts;
        string _time = ddlTimeCloseTicket.Hour + ":" + ddlTimeCloseTicket.Minute;
        TimeSpan.TryParse(_time, out ts);

        ProceedSetting(cbClosedTicket, lbDaysClosedTicket, cbEnableClosedTicket, "CloseTicketReport", "Close ticket report email setting have been Updated", ts);
    }

    protected void lbDailyTicketReport_Click(object sender, EventArgs e)
    {
        TimeSpan ts;
        string _time=  ddlTimeTicketReportEmail.Hour + ":" + ddlTimeTicketReportEmail.Minute;        
        TimeSpan.TryParse(_time, out ts);

        ProceedSetting(cbTicketReportEmail, lbDaysTicketReportEmail, cbEnabledTicketReport, "TicketReport", "Ticket report email setting have been Updated", ts);
    }

    protected void lbVIPEmail_Click(object sender, EventArgs e)
    {
        TimeSpan ts;
        string _time = ddlTimeVIPEmail.Hour + ":" + ddlTimeVIPEmail.Minute;
        TimeSpan.TryParse(_time, out ts);

        ProceedSetting(cbEveryDayVIPEmail, lbDaysVIPEmail, cbActiveVIPEmail, "VIPEmail", "VIP Email have been Updated", ts);
    }
    #endregion

    #region private
    private void DisplayMessage(Literal ltName, string message, Bootstrap.Alert.AlertType alertType, bool dismissable = true, string overrideHeader = "")
    {
        ScriptManager.RegisterClientScriptBlock(this, Page.GetType(), "ToTheTop", "ToTopOfPage();", true);
        ltName.Visible = true;
        ltName.Text = Bootstrap.Alert.GetDivText(alertType, message, dismissable, true, overrideHeader);
    }

    private void BindGrid()
    {
        rgPatchReports.DataSource = GetData("P");
        rgBackupReports.DataSource = GetData("B");
        rgARReport.DataSource= GetData("ARReport");
        rgClosedTicket.DataSource = GetData("CloseTicketReport");
        rgTicketReport.DataSource = GetData("TicketReport");
        rgBackupReport.DataSource = GetData("BackupReport");
        rgComputerManagementReport.DataSource = GetData("ComputerManagementReport");
        rgVIPEmail.DataSource = GetData("VIPEmail");
    }

    public static ReportRecipientCollection GetData(string report)
    {
        ReportRecipientCollection _reportrecipient = new ReportRecipientCollection();


        IPredicateExpression _orFilter = new PredicateExpression();

        if (!string.IsNullOrEmpty(report))
            _orFilter.AddWithAnd(ReportRecipientFields.ForReport == report);

        ISortExpression _reportrecipientSort = new SD.LLBLGen.Pro.ORMSupportClasses.SortExpression();
        _reportrecipientSort.Add(ReportRecipientFields.Email | SortOperator.Ascending);

        //fetch
        _reportrecipient.GetMulti(_orFilter, 0, _reportrecipientSort);

        //return
        return _reportrecipient;
    }

    public static bool DeleteEmail(int pid)
    {
        try
        {
            ReportRecipientEntity _report = new ReportRecipientEntity(pid);
            _report.Delete();
            _report.Save();
            return true;
        }
        catch
        {
            return false;
        }
    }

    private static Regex email_validation()
    {
        string pattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|"
            + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)"
            + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";

        return new Regex(pattern, RegexOptions.IgnoreCase);
    }

    private static bool SaveRecipients(string email, string report, int userid)
    {
        try
        {
            ReportRecipientEntity _report = new ReportRecipientEntity();

            _report.Email = email;
            _report.ForReport = report;
            _report.CreatedDate = DateTime.Now;
            _report.CreatedBy = userid;

            _report.Save();
            _report.Refetch();
            return true;
        }
        catch (Exception ex)
        {
            return false;
        }
    }

    //private static bool SaveNoD(int nod, int userid)
    //{
    //    try
    //    {
    //        NumberofDaysReportEntity _nod = null;

    //        if (checkNOD() == 0)
    //            _nod = new NumberofDaysReportEntity();
    //        else
    //            _nod = new NumberofDaysReportEntity(1);

    //        _nod.NumberofDays = nod;
    //        _nod.ForReport = "P";
    //        _nod.CreatedDate = DateTime.Now;
    //        _nod.CreatedBy = userid;

    //        _nod.Save();
    //        _nod.Refetch();
    //        return true;
    //    }
    //    catch (Exception ex)
    //    {
    //        return false;
    //    }
    //}

    private static bool SaveNoD(int? nod, string ForReport, string days, bool? _enable, int userid, TimeSpan? hours)
    {
        try
        {
            NumberofDaysReportCollection _nodReport = new NumberofDaysReportCollection();
            IPredicateExpression _orFilter = new PredicateExpression();
            _orFilter.AddWithAnd(NumberofDaysReportFields.ForReport == ForReport);
            _nodReport.GetMulti(_orFilter, 0);

            NumberofDaysReportEntity _nod = null;

            if (_nodReport.Count > 0)
                _nod = new NumberofDaysReportEntity(_nodReport[0].Id);
            else
                _nod = new NumberofDaysReportEntity();

            _nod.Days = days;
            _nod.Hours = hours;
            _nod.NumberofDays = nod;
            _nod.Enabled = _enable;
            _nod.ForReport = ForReport;
            _nod.CreatedDate = DateTime.Now;
            _nod.CreatedBy = userid;

            _nod.Save();
            _nod.Refetch();
            return true;
        }
        catch (Exception ex)
        {
            return false;
        }
    }

    private static int checkNOD()
    {
        NumberofDaysReportCollection _nod = new NumberofDaysReportCollection();

        _nod.GetMulti(null, 0, null);

        return _nod.Count;
    }

    private void loadNOD()
    {
        //NumberofDaysReportCollection _allnod = new NumberofDaysReportCollection();

        NumberofDaysReportEntity _nod = new NumberofDaysReportEntity(1);

        txtNumberofDays.Text = _nod.NumberofDays.ToString();

        LoadDataReportSettings("ARReport", cbEveryDaysArReport, lbDaysARReport, cbEnabledARReport, ddlTimeARReport);
        LoadDataReportSettings("ComputerManagementReport", cbComputerManagementReport, lbDaysComputerManagementReport, cbEnabledComputerManagementReport, ddlTimeComputerManagementReport);
        LoadDataReportSettings("BackupReport", cbBackupReport, lbDaysBackupReport, cbEnableBackupReport, ddlTimeBackupReport);
        LoadDataReportSettings("CloseTicketReport", cbClosedTicket, lbDaysClosedTicket, cbEnableClosedTicket, ddlTimeCloseTicket);
        LoadDataReportSettings("TicketReport", cbTicketReportEmail, lbDaysTicketReportEmail, cbEnabledTicketReport, ddlTimeTicketReportEmail);
        LoadDataReportSettings("VIPEmail", cbEveryDayVIPEmail, lbDaysVIPEmail, cbActiveVIPEmail, ddlTimeVIPEmail);
    }

    private void LoadDataReportSettings(string forReport, CheckBox cbEverydayname, UserControl_ListBox_Days lbName, CheckBox cbEnableName, UserControl_DropDownList_Time ddlTimeName)
    {
        NumberofDaysReportCollection _reportsetting = new NumberofDaysReportCollection();
        IPredicateExpression _orFilter = new PredicateExpression();
        _orFilter.AddWithAnd(NumberofDaysReportFields.ForReport == forReport);
        _reportsetting.GetMulti(_orFilter, 0);

        if (_reportsetting.Count > 0)
        {
            cbEnableName.Checked = (bool)_reportsetting[0].Enabled;

            if (_reportsetting[0].Hours != null)
            {
                List<string> resulttime = _reportsetting[0].Hours.ToString().Trim().Split(new char[] { ':' }).ToList();
                ddlTimeName.Hour = Convert.ToInt32(resulttime[0].ToString().Trim());
                ddlTimeName.Minute = Convert.ToInt32(resulttime[1].ToString().Trim());
            }

            if (_reportsetting[0].Days == "Every Day")
                cbEverydayname.Checked = true;
            else
            {
                cbEverydayname.Checked = false;

                List<string> result = _reportsetting[0].Days.Trim().Split(new char[] { ',' }).ToList();
                lbName.Populate(result);
            }
        }
    }

    private void ProceedSetting(CheckBox cbEverydayname, UserControl_ListBox_Days lbName, CheckBox cbEnableName, string reportname, string message, TimeSpan? _time)
    {
        try
        {
            string _days = string.Empty;
            bool? enable;

            if (cbEverydayname.Checked)
                _days = "Every Day";
            else
            {
                if (lbName.SelectedValues.Count > 0)
                {
                    List<string> listdays = lbName.SelectedValues;
                    for (int i = 0; i <= listdays.Count - 1; i++)
                    {
                        if (String.IsNullOrEmpty(_days))
                            _days = listdays[i].ToString().Trim();
                        else
                            _days += "," + listdays[i].ToString().Trim();
                    }
                }
            }
            enable = cbEnableName.Checked;

            if (SaveNoD(null, reportname, _days, enable, DesktopShared.User.UserID, _time))
                DisplayMessage(litMessage, message, Bootstrap.Alert.AlertType.Success);
            else
                DisplayMessage(litMessage, "Something went wrong", Bootstrap.Alert.AlertType.Danger);

        }
        catch (Exception ex)
        {
            DisplayMessage(litMessage, "Error =" + ex.Message.Trim(), Bootstrap.Alert.AlertType.Danger);
        }
    }

    
    #endregion

    #region Check box Changes
    protected void cbEveryDaysArReport_CheckedChanged(object sender, EventArgs e)
    {
        checkboxedchanged(cbEveryDaysArReport, pnllbARReport, lbDaysARReport);
    }

    protected void cbComputerManagementReport_CheckedChanged(object sender, EventArgs e)
    {
        checkboxedchanged(cbComputerManagementReport, pnllbComputerManagementReport, lbDaysComputerManagementReport);
    }

    protected void cbBackupReport_CheckedChanged(object sender, EventArgs e)
    {
        checkboxedchanged(cbBackupReport, pnllbBackupReport, lbDaysBackupReport);
    }

    protected void cbClosedTicket_CheckedChanged(object sender, EventArgs e)
    {
        checkboxedchanged(cbClosedTicket, pnllbClosedTicket, lbDaysClosedTicket);
    }

    protected void cbTicketReportEmail_CheckedChanged(object sender, EventArgs e)
    {
        checkboxedchanged(cbTicketReportEmail, pnllbTicketReportEmail, lbDaysTicketReportEmail);
    }

    protected void cbEveryDayVIPEmail_CheckedChanged(object sender, EventArgs e)
    {
        checkboxedchanged(cbEveryDayVIPEmail, pnlVIPEmail, lbDaysVIPEmail);
    }

    private void checkboxedchanged(CheckBox cbName, Panel pnlName, UserControl_ListBox_Days lbName)
    {
        if (cbName.Checked)
        {
            //lbName.Visible = false;

            lbName.ClearData = true;
            pnlName.Visible = false;
        }
    }
    #endregion

    #region Run button

    protected void lbRunARReport_Click(object sender, EventArgs e)
    {
        runconsole("ARReport", "AR report successfully executed");
        //try
        //{
        //    //string uri = "http://consoleapi.bbbappdev.com/ExecuteConsole?consolepath=RunARReport";
        //    //var request = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(uri);
        //    //var response = (System.Net.HttpWebResponse)request.GetResponse();
        //    //string responseString;
        //    //using (var stream = response.GetResponseStream())
        //    //{
        //    //    using (var reader = new System.IO.StreamReader(stream))
        //    //    {
        //    //        responseString = reader.ReadToEnd();
        //    //    }
        //    //}

        //    ProcessStartInfo startinfo = new ProcessStartInfo();
        //    startinfo.FileName = System.Configuration.ConfigurationManager.AppSettings["RunARReport"];
        //    startinfo.CreateNoWindow = true;
        //    startinfo.UseShellExecute = true;
        //    Process myProcess = Process.Start(startinfo);
        //    myProcess.Start();
        //    DisplayMessage(litMessage, "Ticket type email successfully executed", Bootstrap.Alert.AlertType.Success);
        //}
        //catch (Exception ex)
        //{
        //    DisplayMessage(litMessage, ex.Message , Bootstrap.Alert.AlertType.Danger);
        //}
    }

    protected void lbRunDailyTicketReport_Click(object sender, EventArgs e)
    {
        runconsole("TicketReport", "Daily ticket report successfully executed");
    }

    protected void lbRunCloseTicket_Click(object sender, EventArgs e)
    {
        runconsole("CloseTicketReport", "Close ticket report successfully executed");
    }

    protected void lbRunBackupMonitorReport_Click(object sender, EventArgs e)
    {
        runconsole("BackupReport", "Backup Monitor report successfully executed");
    }

    protected void lbRunComputerManagementReport_Click(object sender, EventArgs e)
    {
        runconsole("ComputerManagementReport", "Computer Management report successfully executed");
    }

    protected void runconsole(string consolename, string successmessage)
    {
        try
        {
            ProcessStartInfo startinfo = new ProcessStartInfo();
            startinfo.FileName = System.Configuration.ConfigurationManager.AppSettings[consolename];
            startinfo.CreateNoWindow = true;
            startinfo.UseShellExecute = true;
            Process myProcess = Process.Start(startinfo);
            //myProcess.Start();

            DisplayMessage(litMessage, successmessage, Bootstrap.Alert.AlertType.Success);
        }
        catch (Exception ex)
        {
            DisplayMessage(litMessage, ex.Message, Bootstrap.Alert.AlertType.Danger);
        }
    }
    #endregion
}
