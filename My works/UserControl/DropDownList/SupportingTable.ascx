<%@ Control Language="C#" AutoEventWireup="true" CodeFile="SupportingTable.ascx.cs" Inherits="UserControl_DropDownList_SupportingTable" %>
<asp:DropDownList ID="ddlSupportingTable" runat="server" />
<asp:RequiredFieldValidator ID="rfvSupportingTable" runat="server" ControlToValidate="ddlSupportingTable" ErrorMessage="Supporing Table Type is required" Visible="false" Display="None" />
<asp:Literal ID="litJs" runat="server" Visible="false"/>