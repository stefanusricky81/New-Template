<%@ Control Language="C#" AutoEventWireup="true" CodeFile="RightfaxServer.ascx.cs" Inherits="UserControl_DropDownList_RightfaxServer" %>
<asp:DropDownList ID="ddlRightfaxServer" runat="server" />
<asp:RequiredFieldValidator ID="rfvRightfaxServer" ControlToValidate="ddlRightfaxServer" ErrorMessage="Server is required" runat="server" Display="None" />
<asp:Literal ID="litJs" runat="server" Visible="false"/>