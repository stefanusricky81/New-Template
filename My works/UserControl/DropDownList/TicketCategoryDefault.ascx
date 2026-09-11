<%@ Control Language="C#" AutoEventWireup="true" CodeFile="TicketCategoryDefault.ascx.cs" Inherits="UserControl_DropDownList_TicketCategoryDefault" %>

<asp:DropDownList ID="ddlTicketCategoryDefault" runat="server" />
<asp:RequiredFieldValidator ID="rfvTicketCategoryDefault" runat="server" ControlToValidate="ddlTicketCategoryDefault" ErrorMessage="Ticket Category is required" Visible="false" Display="None" />
<asp:Literal ID="litJs" runat="server" Visible="false"/>