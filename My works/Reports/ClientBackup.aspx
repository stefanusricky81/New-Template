<%@ Page Language="C#" MasterPageFile="~/Template/Base.master" AutoEventWireup="true" CodeFile="ClientBackup.aspx.cs" Inherits="Reports_ClientBackup" Title="Bit By Bit Intranet - Client Backup Report" %>
<%@ Register TagPrefix="uc" TagName="BackupMonitorStatusTypeDDL" Src="~/UserControl/DropDownList/BackupMonitorStatus.ascx" %>
<%@ Register TagPrefix="uc" TagName="BackupTypeDDL" Src="~/UserControl/DropDownList/BackupType.ascx" %>
<%@ Register TagPrefix="uc" TagName="ClientBackupReportGrid" Src="~/UserControl/Grid/ClientBackupReport.ascx" %>
<%@ Register TagPrefix="uc" TagName="BackupCoreDDL" Src="~/UserControl/DropDownList/BackupMonitorCore.ascx" %>
<%@ Register TagPrefix="uc" TagName="BackupServerDDL" Src="~/UserControl/DropDownList/BackupMonitorServer.ascx" %>
<%@ Register TagPrefix="ddl" TagName="VeeamBackupType" Src="~/UserControl/DropDownList/VeeamBackupType.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server" >
    <script type="text/javascript" src="/js/jquery.simplemodal.js"></script>
    <script type="text/javascript" src="/js/osx.js"></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cphMainContent" Runat="Server">
    <script type="text/javascript">
        function onRequestStart(sender, args) {
            if (args.get_eventTarget().indexOf("btnExport") > 0) {
                args.set_enableAjax(false);
            }
        }
    </script>
    <asp:Panel ID="pnlSearch" runat="server" DefaultButton="btnSearch">
        <h2>Client Backup Report<asp:Literal ID="litClientName" runat="server" /></h2>
        <table class="searchForm" border="0">
            <tr>
                <td colspan="4">
                    <p style="text-align: center; padding-bottom: 2px;"><b>SEARCH CRITERIA</b></p>
                    <asp:ValidationSummary ID="vsSearch" runat="server" CssClass="ValidationSummary" ValidationGroup="vgSearch" />
                </td>
            </tr>
            <tr>
                <th style="width:12%;">Client:</th>
                <td style="width:38%;"><uc:ClientComboBox ID="ucClientComboBox" runat="server" Width="276"/></td>
                <th style="width:12%;">Status:</th>
                <td style="width:38%;"><uc:BackupMonitorStatusTypeDDL ID="ucBackupMonitorStatusDDL" runat="server" Width="175" IsRequired="false" DisplayDefaultValue="true" ValidationGroup="vgSearch" Active="true"/><a href="#" class="osx" style="display:none">Status Key</a></td>
            </tr>
            <tr>
                <th style="width:12%;">Start:</th>
                <td style="width:38%;">
                    <telerik:RadDatePicker id="rdpStartDate" Runat="server" SharedCalendarID="sharedCalendarStartDate" 
                        Width="120px" EnableEmbeddedSkins="false" Skin="BitByBit">
                    </telerik:RadDatePicker>
                    <asp:RequiredFieldValidator ID="rfvStartDate" runat="server" ErrorMessage="Start Date is required" 
                        ControlToValidate="rdpStartDate" ValidationGroup="vgSearch">
                        <span class="error">*</span>
                    </asp:RequiredFieldValidator>
                    <telerik:RadCalendar ID="sharedCalendarStartDate" runat="server" EnableMultiSelect="false" 
                        EnableEmbeddedSkins="false" Skin="BitByBit" ShowRowHeaders="false">
                    </telerik:RadCalendar> 
                    End: 
                    <telerik:RadDatePicker id="rdpEndDate" Runat="server" SharedCalendarID="sharedCalendarEndDate" 
                        Width="120px" EnableEmbeddedSkins="false" Skin="BitByBit">
                    </telerik:RadDatePicker>
                    <asp:RequiredFieldValidator ID="rfvEndDate" runat="server" ErrorMessage="End Date is required" 
                        ControlToValidate="rdpEndDate" ValidationGroup="vgSearch">
                        <span class="error">*</span>
                    </asp:RequiredFieldValidator>
                    <telerik:RadCalendar ID="sharedCalendarEndDate" runat="server" EnableMultiSelect="false" 
                        EnableEmbeddedSkins="false" Skin="BitByBit" ShowRowHeaders="false">
                    </telerik:RadCalendar>  
                    <asp:CompareValidator ID="cvDates" runat="server" ControlToValidate="rdpEndDate" ControlToCompare="rdpStartDate" Operator="GreaterThanEqual" Type="Date" ValidationGroup="vgSearch" ErrorMessage="End Date must be greater than or equal to Start Date">
                        <span class="error">*</span>
                    </asp:CompareValidator>                                
                </td>     
                <th style="width:12%;">Backup Type:</th>  
                <td style="width:38%;text-align:left;"><uc:BackupTypeDDL id="ucBackupTypeDDL" runat="server" DisplayDefaultValue="true" IsRequired="false" Width="175" /></td>
            </tr>
            <tr>
                <th style="width:12%;">Protected Machine:</th>  
                <td style="width:38%;text-align:left;"><asp:TextBox ID="txtServer" runat="server" MaxLength="50" Width="275" /></td>
                <th style="width:12%;">Veeam Job Type:</th>
                <td style="width:38%;"><ddl:VeeamBackupType ID="ddlVeeamBackupType" runat="server" DisplayDefaultValue="true" IsRequired="false" Width="175" DefaultText="ALL" /></td>
            </tr>
            <tr>
                <th style="width:12%;">Backup Server:</th>  
                <td style="width:38%;text-align:left;"><asp:TextBox ID="txtBackupServer" runat="server" MaxLength="50" Width="275" /></td>
                <th style="width:12%;">Is Unsuccessful:</th>
                <td style="width:38%;text-align:left;"><asp:CheckBox ID="chkIsUnsuccessful" runat="server" /></td>
            </tr>
            <tr><td colspan="4">&nbsp;</td></tr>
            <tr>
                <td colspan="4" style="text-align:center;">
                    <asp:Button ID="btnSearch" Text="SEARCH" runat="server" CssClass="actionbutton" OnClick="btnSearch_Click" ValidationGroup="vgSearch" />&nbsp;
                    <asp:Button ID="btnViewToday" Text="VIEW TODAY" runat="server" CssClass="actionbutton" OnClick="btnViewToday_Click" CausesValidation="false" />&nbsp;
                    <asp:Button ID="btnClearSearch" Text="CLEAR" runat="server" CssClass="actionbutton2" OnClick="btnClearSearch_Click" CausesValidation="false" />
                </td>
            </tr>
        </table>
        
        <br /><br />

        <!-- modal content -->
		<div id="osx-modal-content">
			<div id="osx-modal-title">Status Key</div>
			<div class="close"><a href="#" class="simplemodal-close">x</a></div>
			<div id="osx-modal-data" style="float:left;">
				<ul class="gridKey">
                    <li><span style="color:#24BF49">Successful</span> -> Backup has completed successfully.</li>
                    <li>TBD -> To Be Determined.</li>
                    <li><span style="color:#DB2929">Unsuccessful - Backup (BU)</span> -> <strong>Amount Data Changed</strong> is 0.</li>
                    <li><span style="color:#DB2929">Unsuccessful - Missing Backup Data (MBD)</span> -> <strong>Backup</strong> file was not found.</li>
                    <li><span style="color:#DB2929">Unsuccessful - Missing Files (MF)</span> -> Backup has not run.</li>
                    <li><span style="color:#DB2929">Unsuccessful - Missing Repository Data (MRD)</span> -> <strong>Repository</strong> file was not found.</li>
                    <li><span style="color:#DB2929">Unsuccessful - Replication (RP)</span> -> Items in <strong>Queue</strong> have exceeded threshold.</li>
                    <li><span style="color:#DB2929">Unsuccessful - Rollup (RU)</span> -> Number of <strong>Rollups</strong> is 0.</li>
                </ul>
			</div>
		</div>
        
    </asp:Panel>

    <asp:Panel ID="pnlGrid" runat="server">
        <asp:Literal ID="litDebug" runat="server" />
        <uc:ClientBackupReportGrid ID="ucClientBackupReportGrid" runat="server" Visible="true" />
    </asp:Panel>

    
<telerik:RadAjaxManagerProxy ID="rampClientBackup" runat="server">
    <AjaxSettings>
        <telerik:AjaxSetting AjaxControlID="pnlSearch">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="pnlGrid" LoadingPanelID="ralpClientBackup" />
                <telerik:AjaxUpdatedControl ControlID="pnlSearch" />
            </UpdatedControls>
        </telerik:AjaxSetting>
        <telerik:AjaxSetting AjaxControlID="pnlGrid">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="pnlGrid" LoadingPanelID="ralpClientBackup" />
            </UpdatedControls>
        </telerik:AjaxSetting>
    </AjaxSettings>
</telerik:RadAjaxManagerProxy>
    
<telerik:RadAjaxLoadingPanel ID="ralpClientBackup" runat="server" Transparency="25" BackColor="#E0E0E0">
    <img alt="Loading..." src="/Images/loading.gif" style="border: 0px; padding-top:90px;" />
</telerik:RadAjaxLoadingPanel>
</asp:Content>

