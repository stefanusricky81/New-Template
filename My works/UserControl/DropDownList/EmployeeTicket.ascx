<%@ Control Language="C#" AutoEventWireup="true" CodeFile="EmployeeTicket.ascx.cs" Inherits="UserControl_DropDownList_EmployeeTicket" %>

<asp:DropDownList ID="ddlTicket" runat="server" Width="100" Height="20" />

<asp:RequiredFieldValidator ID="rfvTicket" runat="server" ControlToValidate="ddlTicket"
    ErrorMessage="Ticket is required" Visible="false">
    <span class="error">*</span>
</asp:RequiredFieldValidator>