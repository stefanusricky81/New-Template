using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Reports_PatchRebootReport : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    private void BindGrid()
    {
        rgPatchRebootReportSummarry.DataSource = GetPatchRebootSummary();
        rgPatchRebootReportDetail.DataSource = GetPatchRebootDetail();
        rgExportDetailGrid.DataSource = GetPatchRebootDetail();
    }

    private DataTable GetPatchRebootSummary()
    {
        DataTable dt = null;
        dt = DesktopShared.StoredProcedureCallerClasses.RetrievalProcedures.ProcGetNumberofPatchRebootReports(ddlClient.ClientId);
        ViewState["dtSummary"] = dt;
        return dt;
    }
    private DataTable GetPatchRebootDetail()
    {
        DataTable dt = null;
        int patchstatus = 0;

        patchstatus = cbPatchReboot.Checked ? 1 : 0;

        dt = DesktopShared.StoredProcedureCallerClasses.RetrievalProcedures.ProcGetPatchRebootReports(ddlClient.ClientId, patchstatus);
        ViewState["dtDetail"] = dt;
        return dt;
    }
    protected void rgPatchRebootReportSummarry_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        BindGrid();
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        int nod = 0;
        string errormessage = string.Empty;
        bool success=false;

        phSearchResults.Visible = true;

        nod = DesktopShared.PatchAndReboot.GetNumberofdaysPatchAndRebootReport(ref success, ref errormessage);
        if (nod > 0)
        {
            if (success)
                ltDates.Text = "Report for " + DateTime.Now.AddDays(-nod).ToShortDateString() + " - " + DateTime.Now.ToShortDateString();
            else
                ltDates.Text = errormessage;
        }
        
        BindGrid();
        rgPatchRebootReportDetail.Rebind();
        rgPatchRebootReportSummarry.Rebind();
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        Response.Redirect("/PatchRebootReport.aspx");
    }

    protected void lbSendMail_Click(object sender, EventArgs e)
    {
        DataTable dtSummary = null;
        DataTable dtDetail = null;
        int _clientid;
        string _emailto = string.Empty;
        dtSummary= (DataTable)ViewState["dtSummary"];
        dtDetail = (DataTable)ViewState["dtDetail"];

        string mailfrom = System.Web.Configuration.WebConfigurationManager.AppSettings["ClientSetupFrom"];

        for (int i = 0; i <= dtSummary.Rows.Count - 1; i++)
        {
            _clientid = Convert.ToInt32(dtSummary.Rows[i]["pclient"].ToString().Trim());
            getallemail(_clientid, ref _emailto);
        }
        string subject = "Bit By Bit Client Setup Process";
        string bodymail = string.Empty;

        System.Text.StringBuilder _sb = new System.Text.StringBuilder();

        EmailMessage(dtDetail, ref bodymail);
        EmailMessage2(dtSummary, ref bodymail);
        _sb.Append(bodymail);
        DesktopShared.Email.SendEmail2(mailfrom, subject, _emailto, string.Empty, _sb.ToString());
    }

    protected void EmailMessage(DataTable dtDetail, ref string notemsg)
    {
        notemsg = "<h1>Patch/Reboot Report</h1>";

        notemsg += "<table border=1 style=" + "width:100%" + ">" +
                    "<tr bgcolor=" + "#D8D8D8" + ">" +
                    "<td style=" + "width:10%" + " align=" + "center" + ">No</td>" +
                    "<td style=" + "width:10%" + " align=" + "center" + ">Client</td>" +
                    "<td style=" + "width:9%" + " align=" + "center" + ">Computer Name</td>" +
                    "<td style=" + "width:9%" + " align=" + "center" + ">Type</td>" +
                    "<td style=" + "width:9%" + " align=" + "center" + ">Last Login</td>" +
                    "<td style=" + "width:9%" + " align=" + "center" + ">Last Patch Date</td>" +
                    "<td style=" + "width:9%" + " align=" + "center" + ">Last Reboot</td>" +
                    "<td style=" + "width:9%" + " align=" + "center" + ">Last Check In</td>" +
                    "<td style=" + "width:9%" + " align=" + "center" + ">Operating System</td>" +
                    "<td style=" + "width:9%" + " align=" + "center" + ">OS Information</td>" +
                    "<td style=" + "width:9%" + " align=" + "center" + ">Missing Approved</td>" +
                    "<td style=" + "width:9%" + " align=" + "center" + ">KB Article</td>" +
                    "<td style=" + "width:9%" + " align=" + "center" + ">Patch Status</td>" +
                    "</tr>"
                    ;

        if (dtDetail.Rows.Count != 0)
        {
            for (int i = 0; dtDetail.Rows.Count > i; i++)
            {
                int runningno = i + 1;
                notemsg += "<tr bgcolor=" + "#FFFFFF" + ">" +
                    "<td align=" + "center" + ">" + runningno + "</td>" +
                    "<td align=" + "center" + ">" + dtDetail.Rows[i]["COMPANY"] + "</td>" +
                    "<td align=" + "center" + ">" + dtDetail.Rows[i]["NAME"] + "</td>" +
                    "<td align=" + "center" + ">" + dtDetail.Rows[i]["DESCRP"] + "</td>" +
                    "<td align=" + "center" + ">" + dtDetail.Rows[i]["LASTLOGGEDINUSER"] + "</td>" +
                    "<td align=" + "center" + ">" + dtDetail.Rows[i]["LASTPATCHDATE"] + "</td>" +
                    "<td align=" + "center" + ">" + dtDetail.Rows[i]["KASEYALASTREBOOT"] + "</td>" +
                    "<td align=" + "center" + ">" + dtDetail.Rows[i]["LastActiveDate"] + "</td>" +
                    "<td align=" + "center" + ">" + dtDetail.Rows[i]["KASEYAOPERATINGSYSTEM"] + "</td>" +
                    "<td align=" + "center" + ">" + dtDetail.Rows[i]["KASEYAOSINFORMATION"] + "</td>" +
                    "<td align=" + "center" + ">" + dtDetail.Rows[i]["KASEYAMISSINGAPPROVED"] + "</td>" +
                    "<td align=" + "center" + ">" + dtDetail.Rows[i]["KASEYAKBARTICLE"] + "</td>" +
                    "<td align=" + "center" + ">" + dtDetail.Rows[i]["KASEYAPATCHSTATUS"] + "</td>" +
                    "</tr>";
            }
        }
        else
        {
            notemsg += "<tr bgcolor=" + "#FFFFFF" + ">" +
                "<td align=" + "center" + "></td>" +
                  "<td align=" + "center" + ">No Data</td>" +
                  "<td align=" + "center" + ">-</td>" +
                  "<td align=" + "center" + ">-</td>" +
                  "<td align=" + "center" + ">-</td>" +
                  "<td align=" + "center" + ">-</td>" +
                  "<td align=" + "center" + ">-</td>" +
                  "<td align=" + "center" + ">-</td>" +
                  "<td align=" + "center" + ">-</td>" +
                  "<td align=" + "center" + ">-</td>" +
                  "<td align=" + "center" + ">-</td>" +
                  "<td align=" + "center" + ">-</td>" +
                  "<td align=" + "center" + ">-</td>" +
                  "</tr>";
        }

        notemsg += "</table></center><br/><br/>";
    }

    protected void EmailMessage2(DataTable dtReport, ref string notemsg)
    {
        int numofComputer = 0, numofwks = 0, numofservers = 0, numofnotrebooted = 0, 
            numofnotpatches = 0, numofwksnotrebooted = 0, numofwksnotpatches = 0;

        notemsg += "<table border=1 style=" + "width:100%" + ">" +
                    "<tr bgcolor=" + "#D8D8D8" + ">" +
                    "<td style=" + "width:10%" + " align=" + "center" + ">No</td>" +
                    "<td style=" + "width:10%" + " align=" + "center" + ">Company</td>" +
                    "<td style=" + "width:9%" + " align=" + "center" + "># of Computers</td>" +
                    "<td style=" + "width:9%" + " align=" + "center" + "># of WorkStations</td>" +
                    "<td style=" + "width:9%" + " align=" + "center" + "># of Servers</td>" +
                    "<td style=" + "width:9%" + " align=" + "center" + "># of Server not Rebooted</td>" +
                    "<td style=" + "width:9%" + " align=" + "center" + "># of Server not Patches</td>" +
                    "<td style=" + "width:9%" + " align=" + "center" + "># of WorkStation not Rebooted</td>" +
                    "<td style=" + "width:9%" + " align=" + "center" + "># of WorkStation not Patches</td>" +
                    "</tr>"
                    ;

        if (dtReport.Rows.Count != 0)
        {
            for (int x = 0; dtReport.Rows.Count > x; x++)
            {
                numofComputer += Convert.ToInt16(dtReport.Rows[x]["NumofComputer"]);
                numofwks += Convert.ToInt16(dtReport.Rows[x]["NumofWordkStation"]);
                numofservers += Convert.ToInt16(dtReport.Rows[x]["NumofServer"]);
                numofnotrebooted += Convert.ToInt16(dtReport.Rows[x]["NumOfNotRebooted"]);
                numofnotpatches += Convert.ToInt16(dtReport.Rows[x]["NumofNotPatches"]);
                numofwksnotrebooted += Convert.ToInt16(dtReport.Rows[x]["wksNotRebooted"]);
                numofwksnotpatches += Convert.ToInt16(dtReport.Rows[x]["wksNotPatched"]);
            }

            for (int i = 0; dtReport.Rows.Count > i; i++)
            {
                int runningno = i + 1;
                notemsg += "<tr bgcolor=" + "#FFFFFF" + ">" +
                    "<td align=" + "center" + ">" + runningno + "</td>" +
                    "<td align=" + "center" + ">" + dtReport.Rows[i]["company"] + "</td>" +
                    "<td align=" + "center" + ">" + dtReport.Rows[i]["NumofComputer"] + "</td>" +
                    "<td align=" + "center" + ">" + dtReport.Rows[i]["NumofWordkStation"] + "</td>" +
                    "<td align=" + "center" + ">" + dtReport.Rows[i]["NumofServer"] + "</td>" +
                    "<td align=" + "center" + ">" + dtReport.Rows[i]["NumOfNotRebooted"] + "</td>" +
                    "<td align=" + "center" + ">" + dtReport.Rows[i]["NumofNotPatches"] + "</td>" +
                    "<td align=" + "center" + ">" + dtReport.Rows[i]["wksNotRebooted"] + "</td>" +
                    "<td align=" + "center" + ">" + dtReport.Rows[i]["wksNotPatched"] + "</td>" +
                    "</tr>";
            }

            notemsg += "<tr bgcolor=" + "#FFFFFF" + ">" +
                "<td align=" + "center" + "></td>" +
                "<td align=" + "center" + ">Total</td>" +
                "<td align=" + "center" + ">" + numofComputer + "</td>" +
                "<td align=" + "center" + ">" + numofwks + "</td>" +
                "<td align=" + "center" + ">" + numofservers + "</td>" +
                "<td align=" + "center" + ">" + numofnotrebooted + "</td>" +
                "<td align=" + "center" + ">" + numofnotpatches + "</td>" +
                "<td align=" + "center" + ">" + numofwksnotrebooted + "</td>" +
                "<td align=" + "center" + ">" + numofwksnotpatches + "</td>" +
                "</tr>";
        }
        else
        {
            notemsg += "<tr bgcolor=" + "#FFFFFF" + ">" +
                    "<td align=" + "center" + "></td>" +
                  "<td align=" + "center" + ">No Data</td>" +
                  "<td align=" + "center" + ">-</td>" +
                  "<td align=" + "center" + ">-</td>" +
                  "<td align=" + "center" + ">-</td>" +
                  "<td align=" + "center" + ">-</td>" +
                  "<td align=" + "center" + ">-</td>" +
                  "<td align=" + "center" + ">-</td>" +
                  "<td align=" + "center" + ">-</td>" +
                  "</tr>";
        }

        notemsg += "</table></center><br/><br/>";
    }

    protected void getallemail(int client, ref string emailaddress)
    {
        DesktopShared.TypedListClasses.ClientPatchReportUsersTypedList _report = DesktopShared.PatchReport.GetAllUsers(client, "Patch", false, true);
        foreach (var objReport in _report)
        {
            if (emailaddress == string.Empty)
                emailaddress = objReport.UserEmail.Trim();
            else
                emailaddress +=","+ objReport.UserEmail.Trim();
        }
    }

    protected void lbexport_Click(object sender, EventArgs e)
    {
        try
        {
            rgExportDetailGrid.ExportSettings.IgnorePaging = true;
            rgExportDetailGrid.ExportSettings.ExportOnlyData = true;
            rgExportDetailGrid.ExportSettings.HideStructureColumns = true;

            rgExportDetailGrid.GridLines = GridLines.Both;
            rgExportDetailGrid.BorderStyle = BorderStyle.Solid;

            rgExportDetailGrid.ExportSettings.FileName = "Patch and Reboot " + ltDates.Text + "_" + ddlClient.ClientName;

            rgExportDetailGrid.MasterTableView.ExportToExcel();
        }
        catch (Exception ex)
        { }
    }

    protected void cbPatchReboot_CheckedChanged(object sender, EventArgs e)
    {
        rgPatchRebootReportDetail.DataSource = GetPatchRebootDetail();
        rgExportDetailGrid.DataSource = GetPatchRebootDetail();

        rgPatchRebootReportDetail.Rebind();
        rgExportDetailGrid.Rebind();
    }
}