<%@ Control Language="C#" AutoEventWireup="true" CodeFile="TicketCategory.ascx.cs" Inherits="UserControl_DropDownList_TicketCategory" %>
<asp:DropDownList ID="ddlTicketCategory" runat="server" />
<asp:RequiredFieldValidator ID="rfvTicketCategory" runat="server" ControlToValidate="ddlTicketCategory" ErrorMessage="Ticket Category is required" Visible="false" Display="None" />
<asp:Literal ID="litJs" runat="server" Visible="false"/>