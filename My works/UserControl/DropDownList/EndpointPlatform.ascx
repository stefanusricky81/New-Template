<%@ Control Language="C#" AutoEventWireup="true" CodeFile="EndpointPlatform.ascx.cs" Inherits="UserControl_DropDownList_EndpointPlatform" %>
<asp:DropDownList ID="ddlEndpointPlatform" runat="server" />
<asp:RequiredFieldValidator ID="rfvEndpointPlatform" ControlToValidate="ddlEndpointPlatform" ErrorMessage="Code is required" runat="server" Display="None" />
<asp:Literal ID="litJs" runat="server" Visible="false"/>