<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Ticket.ascx.cs" Inherits="UserControl_DropDownList_Ticket" %>
<asp:DropDownList ID="ddlTicket" runat="server" />
<asp:RequiredFieldValidator ID="rfvTicket" runat="server" ControlToValidate="ddlTicket" ErrorMessage="Ticket is required" Visible="false" Display="None"/>
<asp:Literal ID="litJs" runat="server" Visible="false"/>
<asp:Literal ID="litDebug" runat="server" />