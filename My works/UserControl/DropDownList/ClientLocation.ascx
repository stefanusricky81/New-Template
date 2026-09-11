<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ClientLocation.ascx.cs" Inherits="UserControl_DropDownList_ClientLocation" %>
<asp:DropDownList ID="ddlClientLocation" runat="server" />
<asp:RequiredFieldValidator ID="rfvClientLocation" runat="server" ControlToValidate="ddlClientLocation" ErrorMessage="Location is required" Visible="false" Display="None" />
<asp:Literal ID="litJs" runat="server" Visible="false"/>
<asp:Literal ID="litDebug" runat="server" />