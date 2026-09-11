<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Ticket.ascx.cs" Inherits="UserControl_ListBox_Ticket" %>
<asp:ListBox ID="lbTicket" runat="server" SelectionMode="Multiple"/>
<asp:RequiredFieldValidator ID="rfvTicket" runat="server" ControlToValidate="lbTicket" ErrorMessage="Ticket is required" Visible="false" Display="None"/>
<asp:Literal ID="litJs" runat="server" Visible="false"/>