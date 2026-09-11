<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ClientDocumentType.ascx.cs" Inherits="UserControl_DropDownList_ClientDocumentType" %>
<asp:DropDownList ID="ddlClientDocumentType" runat="server" />
<asp:RequiredFieldValidator ID="rfvClientDocumentType" ControlToValidate="ddlClientDocumentType" ErrorMessage="Document Type is required" runat="server" Display="None" />
<asp:Literal ID="litJs" runat="server" Visible="false"/>