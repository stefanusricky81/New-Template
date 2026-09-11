<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ClientContactResponsive.ascx.cs" Inherits="UserControl_DropDownList_ClientContactResponsive" %>
<asp:DropDownList ID="ddlClientContact" runat="server" />
<asp:RequiredFieldValidator ID="rfvClientContact" runat="server" ControlToValidate="ddlClientContact" ErrorMessage="Client Contact is required" Visible="false" Display="None"/>
<asp:Literal ID="litJs" runat="server" Visible="false"/>
<asp:Literal ID="litDebug" runat="server" />