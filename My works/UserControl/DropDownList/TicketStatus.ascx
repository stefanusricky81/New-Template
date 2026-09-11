<%@ Control Language="C#" AutoEventWireup="true" CodeFile="TicketStatus.ascx.cs" Inherits="UserControl_DropDownList_TicketStatus" %>

<asp:DropDownList ID="ddlTicketStatus" runat="server" Width="100" Height="20" />

<asp:RequiredFieldValidator ID="rfvStatus" runat="server" ControlToValidate="ddlTicketStatus"
    ErrorMessage="Status is required" Visible="false">
    <span class="error">*</span>
</asp:RequiredFieldValidator>