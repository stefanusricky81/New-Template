<%@ Page Language="C#" MasterPageFile="~/Template/Base.master" AutoEventWireup="true" CodeFile="Ticket.aspx.cs" Inherits="Reports_Ticket" Title="Bit By Bit Intranet - Ticket Report" %>
<%@ Reference VirtualPath="~/UserControl/Report/Ticket/TicketAnalysis.ascx" %>
<%@ Reference VirtualPath="~/UserControl/Report/Ticket/TicketEmployee.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server"></asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cphMainContent" Runat="Server">

<asp:Panel ID="pnlSearch" runat="server">
        <h2>Ticket Report
            <asp:Literal ID="litClientName" runat="server" />
        </h2>
        
        <table class="searchForm" border="0">
            <tr>
                <td colspan="4">
                    <p style="text-align:center; padding-bottom:2px;"><b>REPORT CRITERIA</b></p>
                    
                    <asp:ValidationSummary ID="vsTicketReport" runat="server" 
                        CssClass="ValidationSummary" ValidationGroup="vgTicketReport" />
                </td>
            </tr>
            <tr>
                <th>Report Type:</th>
                <td>
                    <asp:DropDownList ID="ddlReportType" runat="server" Width="276">
                        <asp:ListItem Text="" Value=""></asp:ListItem>
                        <asp:ListItem Text="Ticket Analysis" Value="TicketAnalysis"></asp:ListItem>
                        <asp:ListItem Text="Employee" Value="TicketEmployee"></asp:ListItem>
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="rfvReportType" runat="server" ControlToValidate="ddlReportType"
                        ErrorMessage="Report Type is required" ValidationGroup="vgTicketReport">
                        <span class="error">*</span>
                    </asp:RequiredFieldValidator>
                </td>
                <th>Assigned To:</th>
                <td>
                    <uc:EmployeeDDL ID="ucEmployeeAssignedTo" runat="server" 
                        CssClass="dropdown" Width="275" DefaultText="All"/>
                </td>
            </tr>
            <tr>
                <th>Client:</th>
                <td>
                    <uc:ClientComboBox ID="ucClientComboBox" runat="server" Width="276"/>
                </td>
                <th>Options:</th>
                <td ><asp:CheckBox ID="chk3d" runat="server" Text="3D Chart" CssClass="checkHome" /></td>
            </tr>
                
            <tr>
                <th>Date Range:</th>
                <td>
                    <uc:ToFrom ID="ucToFrom" runat="server" ValidationGroup="vgTicketReport" MaxixumDayDifference="31"/>
                </td>
                <th>Tickets To Include:</th>
                <td>
                    <asp:CheckBox ID="chkOpen" Text="Open" runat="server" CssClass="checkHome"/>&nbsp
                    <asp:CheckBox ID="chkNew" Text="New" runat="server" CssClass="checkHome"/>&nbsp
                    <asp:CheckBox ID="chkClosed" Text="Closed" runat="server" CssClass="checkHome"/>&nbsp
                    <asp:CheckBox ID="chkUpdated" Text="Updated" runat="server" CssClass="checkHome"/>&nbsp
                </td>
            </tr>
            <tr>
                <td colspan="4" style="text-align:center;">
                    <asp:button id="btnSubmit" text="SUBMIT" runat="server" 
                        CssClass="actionbutton" OnClick="btnSubmit_Click" ValidationGroup="vgTicketReport">
                    </asp:button>&nbsp;
                    <asp:button id="btnClear" text="CLEAR" runat="server" 
                        CssClass="actionbutton2" OnClick="btnClear_Click" CausesValidation="false">
                    </asp:button>
                </td>        
            </tr>
        </table>
    
        <br /><br />
    
    </asp:Panel>
    
    <asp:PlaceHolder ID="phReport" runat="server" />
    
</asp:Content>

