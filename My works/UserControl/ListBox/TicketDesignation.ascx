<%@ Control Language="C#" AutoEventWireup="true" CodeFile="TicketDesignation.ascx.cs" Inherits="UserControl_ListBox_TicketDesignation" %>
<asp:ListBox ID="lbTicketDesignation" runat="server" Width="100" Height="60" SelectionMode="Multiple">
    <asp:ListItem Value="Z">Alert</asp:ListItem>
    <asp:ListItem Value="A">App Dev</asp:ListItem>
    <asp:ListItem Value="N">Network</asp:ListItem>
</asp:ListBox>

<asp:RequiredFieldValidator ID="rfvDesignation" runat="server" ControlToValidate="lbDesignation"
    ErrorMessage="Designation is required" Visible="false">
    <span class="error">*</span>
</asp:RequiredFieldValidator>