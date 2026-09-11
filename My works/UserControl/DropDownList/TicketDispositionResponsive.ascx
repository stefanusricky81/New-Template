<%@ Control Language="C#" AutoEventWireup="true" CodeFile="TicketDispositionResponsive.ascx.cs" Inherits="UserControl_DropDownList_TicketDispositionResponsive" %>
<asp:DropDownList ID="ddlTicketDisposition" runat="server" />
<asp:RequiredFieldValidator ID="rfvTicketDisposition" runat="server" ControlToValidate="ddlTicketDisposition" ErrorMessage="Disposition is required" Visible="false" Display="None"/>
<asp:Literal ID="litJs" runat="server" Visible="false"/>