<%@ Control Language="C#" AutoEventWireup="true" CodeFile="TicketDesignation.ascx.cs" Inherits="UserControl_DropDownList_TicketDesignation" %>

<asp:DropDownList ID="ddlTicketDesignation" runat="server" Width="100" Height="20">
    <asp:ListItem Value="" Text=""></asp:ListItem>
    <asp:ListItem Value="Z">Alert</asp:ListItem>
    <asp:ListItem Value="A">App Dev</asp:ListItem>
    <asp:ListItem Value="N">Network</asp:ListItem>
</asp:DropDownList>

<asp:RequiredFieldValidator ID="rfvDesignation" runat="server" ControlToValidate="ddlTicketDesignation"
    ErrorMessage="Designation is required" Visible="false">
    <span class="error">*</span>
</asp:RequiredFieldValidator>