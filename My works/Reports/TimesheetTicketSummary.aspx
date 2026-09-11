<%@ Page Title="Bit By Bit Intranet - Timesheet Ticket Summary" Language="C#" MasterPageFile="~/Template/Base.master" AutoEventWireup="true" CodeFile="TimesheetTicketSummary.aspx.cs" Inherits="Reports_TimesheetTicketSummary" %>
<%@ Register TagPrefix="uc" TagName="TimesheetTicketSummaryGrid" Src="~/UserControl/Grid/TimesheetTicketSummary.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server"></asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cphMainContent" Runat="Server">

<h2>Timesheet Ticket Summary</h2>

<asp:Panel ID="pnlSearch" runat="server">
    <table class="searchForm" border="0">
        <tr>
            <td colspan="4">
                <p style="text-align:center; padding-bottom:2px;"><b>TIMESHEET TICKET SUMMARY SEARCH CRITERIA</b></p>
                <asp:ValidationSummary ID="vsTimesheetTicketSummary" runat="server" CssClass="ValidationSummary" ValidationGroup="vgTimesheetTicketSummary" />
            </td>
        </tr>
        <tr>
            <th>Date Range:</th>
            <td>
                <uc:ToFrom ID="ucToFrom" runat="server" ValidationGroup="vgTimesheetTicketSummary" MaxixumDayDifference="365" StartDateRequired="true" EndDateRequired="true" MaxixumDayDifferenceErrorMessage="Maximum Date Range is 365 days" />
                <asp:CustomValidator ID="cvSearch" runat="server" ControlToValidate="tbValidator" ValidateEmptyText="true" OnServerValidate="cvSearch_ServerValidate" ValidationGroup="vgTimesheetTicketSummary" Display="None"/>
            </td>
            <th>Employee:</th>
            <td><uc:EmployeeDDL ID="ucEmployeeDDL" runat="server" DefaultValue="" DefaultText="" DisplayDefaultValue="true" Width="200" UseEmployeeIdAsDataValue="true"/></td>
        </tr>
        <tr>
            <th>Client:</th>
            <td><uc:ClientComboBox ID="ucClientComboBox" runat="server" Width="276"/></td>
            <th>&nbsp;</th>
            <td><asp:TextBox ID="tbValidator" runat="server" Visible="false" /></td>
        </tr>
        <tr>
            <td colspan="4" style="text-align:center;">
                <asp:Button ID="btnSubmit" text="SUBMIT" runat="server" CssClass="actionbutton" OnClick="btnSubmit_Click" ValidationGroup="vgTimesheetTicketSummary" />
                <asp:Button ID="btnClear" text="CLEAR" runat="server" CssClass="actionbutton2" OnClick="btnClear_Click" CausesValidation="false" />
            </td>        
        </tr>
    </table>
</asp:Panel>
    
<br /><br />

<asp:Panel ID="pnlGrid" runat="server" Visible="false">
    <uc:TimesheetTicketSummaryGrid ID="ucTimesheetTicketSummaryGrid" runat="server" />    
</asp:Panel>

<telerik:RadAjaxManagerProxy ID="rampTimesheetTicketSummary" runat="server"> 
    <AjaxSettings>
        <telerik:AjaxSetting AjaxControlID="pnlSearch">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="pnlGrid" LoadingPanelID="ralpTimesheetTicketSummary"/>
                <telerik:AjaxUpdatedControl ControlID="pnlSearch"/>
            </UpdatedControls>
        </telerik:AjaxSetting> 
        <telerik:AjaxSetting AjaxControlID="pnlGrid">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="pnlGrid" LoadingPanelID="ralpTimesheetTicketSummary"/>
            </UpdatedControls>
        </telerik:AjaxSetting>
    </AjaxSettings>
</telerik:RadAjaxManagerProxy>

<telerik:RadAjaxLoadingPanel ID="ralpTimesheetTicketSummary" runat="server" Transparency="25" BackColor="#E0E0E0">
    <img alt="Loading..." src="/Images/loading.gif" style="border: 0px; padding-top:90px;" />
</telerik:RadAjaxLoadingPanel>
    
</asp:Content>

