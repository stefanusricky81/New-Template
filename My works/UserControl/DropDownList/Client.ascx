<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Client.ascx.cs" Inherits="UserControl_DropDownList_Client" %>
<asp:DropDownList ID="ddlClient" runat="server" />
<asp:RequiredFieldValidator ID="rfvClient" runat="server" ControlToValidate="ddlClient" ErrorMessage="Client is required" Visible="false" Display="None" />
<asp:Literal ID="litJs" runat="server" Visible="false"/>
<asp:Literal ID="litDebug" runat="server" />